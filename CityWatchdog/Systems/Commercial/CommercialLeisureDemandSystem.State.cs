// <copyright file="CommercialLeisureDemandSystem.State.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialLeisureDemandSystem.State.cs
// Purpose: Pressure calculation and diagnostics for corrective leisure demand.

#if DEBUG

using System;
using System.Globalization;
using System.Text;

using CS2Shared.RiverMochi;

using Game.Agents;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;

using Unity.Entities;
using Unity.Mathematics;

namespace CityWatchdog.Systems
{
    public partial class CommercialLeisureDemandSystem
    {
        private const float kLeisureTargetServiceRatio =
            0.85f;

        // CitizenBehavior runs four times as often as LeisureSystem.
        // 64 correction updates therefore roughly span one complete
        // LeisureSystem pass through its 16 UpdateFrame buckets.
        private const int kLeisureCorrectionHorizonUpdates =
            64;

        private const int kLeisureLogEveryUpdates =
            128;

        private const int kMaxLeisureSeekersPerUpdate =
            32;

        // A selected resident still uses vanilla SelectLeisureType(), so not
        // every added leisure attempt will choose one of the pressured
        // commercial types. Keep the controller modestly ahead of that loss.
        private const double kVanillaSelectionCompensation =
            2.0;

        // Candidates have LeisureCounter < 128, so they need at least about
        // half of the full 0..255 leisure range. This is used only to estimate
        // how many extra seekers to release, not to modify their leisure.
        private const int kAssumedLeisurePointsToGain =
            160;

        private readonly long[]
            m_LeisureServiceAvailable =
                new long[
                    EconomyUtils.ResourceCount];

        private readonly long[]
            m_LeisureMaxService =
                new long[
                    EconomyUtils.ResourceCount];

        private readonly long[]
            m_LeisureExcess =
                new long[
                    EconomyUtils.ResourceCount];

        private readonly int[]
            m_LeisureShopCount =
                new int[
                    EconomyUtils.ResourceCount];

        private readonly int[]
            m_LeisureWarningCount =
                new int[
                    EconomyUtils.ResourceCount];

        private readonly float[]
            m_LeisureUnusedRatio =
                new float[
                    EconomyUtils.ResourceCount];

        private readonly long[]
            m_LeisureTypeExcess =
                new long[
                    (int)LeisureType.Count];

        private long m_TotalLeisureExcess;

        private long
            m_WeightedEstimatedVisitService;

        private int
            m_EstimatedServicePerVisit;

        private int
            m_MissingLeisureProviderData;

        private double
            m_LeisureSeekerCredit;

        private int
            m_LeisureSeekerBudget;

        private void ResetLeisureState()
        {
            Array.Clear(
                m_LeisureServiceAvailable,
                0,
                m_LeisureServiceAvailable.Length);

            Array.Clear(
                m_LeisureMaxService,
                0,
                m_LeisureMaxService.Length);

            Array.Clear(
                m_LeisureExcess,
                0,
                m_LeisureExcess.Length);

            Array.Clear(
                m_LeisureShopCount,
                0,
                m_LeisureShopCount.Length);

            Array.Clear(
                m_LeisureWarningCount,
                0,
                m_LeisureWarningCount.Length);

            Array.Clear(
                m_LeisureUnusedRatio,
                0,
                m_LeisureUnusedRatio.Length);

            Array.Clear(
                m_LeisureTypeExcess,
                0,
                m_LeisureTypeExcess.Length);

            m_TotalLeisureExcess = 0;

            m_WeightedEstimatedVisitService = 0;

            m_EstimatedServicePerVisit = 1;

            m_MissingLeisureProviderData = 0;

            m_LeisureSeekerCredit = 0d;

            m_LeisureSeekerBudget = 0;
        }

        private void BuildLeisurePressure(
            ComponentLookup<
                Game.Companies.ServiceCompanyData>
                    serviceCompanyDatas,
            ComponentLookup<
                IndustrialProcessData>
                    industrialProcessDatas,
            ComponentLookup<
                LeisureProviderData>
                    leisureProviderDatas)
        {
            Array.Clear(
                m_LeisureServiceAvailable,
                0,
                m_LeisureServiceAvailable.Length);

            Array.Clear(
                m_LeisureMaxService,
                0,
                m_LeisureMaxService.Length);

            Array.Clear(
                m_LeisureExcess,
                0,
                m_LeisureExcess.Length);

            Array.Clear(
                m_LeisureShopCount,
                0,
                m_LeisureShopCount.Length);

            Array.Clear(
                m_LeisureWarningCount,
                0,
                m_LeisureWarningCount.Length);

            Array.Clear(
                m_LeisureUnusedRatio,
                0,
                m_LeisureUnusedRatio.Length);

            Array.Clear(
                m_LeisureTypeExcess,
                0,
                m_LeisureTypeExcess.Length);

            m_TotalLeisureExcess = 0;

            m_WeightedEstimatedVisitService = 0;

            m_MissingLeisureProviderData = 0;

            BufferLookup<Game.Economy.Resources>
                resourcesLookup =
                    SystemAPI.GetBufferLookup<
                        Game.Economy.Resources>(
                            true);

            foreach ((
                RefRO<Game.Companies.ServiceAvailable>
                    serviceRef,
                RefRO<Game.Companies.CompanyNotifications>
                    notificationsRef,
                RefRO<PrefabRef>
                    prefabRef,
                Entity company) in
                SystemAPI
                    .Query<
                        RefRO<
                            Game.Companies.ServiceAvailable>,
                        RefRO<
                            Game.Companies.CompanyNotifications>,
                        RefRO<
                            PrefabRef>>()
                    .WithAll<
                        Game.Companies.CommercialCompany>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>()
                    .WithEntityAccess())
            {
                Entity companyPrefab =
                    prefabRef.ValueRO.m_Prefab;

                if (!serviceCompanyDatas.HasComponent(
                        companyPrefab) ||
                    !industrialProcessDatas.HasComponent(
                        companyPrefab))
                {
                    continue;
                }

                Resource resource =
                    industrialProcessDatas[
                        companyPrefab]
                        .m_Output.m_Resource;

                if (!IsCommercialLeisureResource(
                        resource))
                {
                    continue;
                }

                Game.Companies.ServiceCompanyData
                    serviceData =
                        serviceCompanyDatas[
                            companyPrefab];

                if (serviceData.m_MaxService <= 0)
                {
                    continue;
                }

                int resourceIndex =
                    EconomyUtils.GetResourceIndex(
                        resource);

                if (resourceIndex < 0 ||
                    resourceIndex >=
                        m_LeisureShopCount.Length)
                {
                    continue;
                }

                int available =
                    math.clamp(
                        serviceRef.ValueRO
                            .m_ServiceAvailable,
                        0,
                        serviceData.m_MaxService);

                m_LeisureShopCount[
                    resourceIndex]++;

                m_LeisureServiceAvailable[
                    resourceIndex] +=
                        available;

                m_LeisureMaxService[
                    resourceIndex] +=
                        serviceData.m_MaxService;

                if (notificationsRef.ValueRO
                        .m_NoCustomersEntity !=
                    Entity.Null)
                {
                    m_LeisureWarningCount[
                        resourceIndex]++;
                }

                int stock = 0;

                if (resourcesLookup.HasBuffer(
                        company))
                {
                    stock =
                        EconomyUtils.GetResources(
                            resource,
                            resourcesLookup[
                                company]);
                }

                // Match the physical-stock side of vanilla's warning rule.
                if (stock <= 200)
                {
                    continue;
                }

                if (!leisureProviderDatas.HasComponent(
                        companyPrefab))
                {
                    m_MissingLeisureProviderData++;
                    continue;
                }

                int targetService =
                    (int)math.floor(
                        serviceData.m_MaxService *
                        kLeisureTargetServiceRatio);

                int excess =
                    math.max(
                        0,
                        available -
                            targetService);

                if (excess <= 0)
                {
                    continue;
                }

                LeisureProviderData
                    providerData =
                        leisureProviderDatas[
                            companyPrefab];

                m_LeisureExcess[
                    resourceIndex] +=
                        excess;

                m_TotalLeisureExcess +=
                    excess;

                int leisureTypeIndex =
                    (int)providerData
                        .m_LeisureType;

                if (leisureTypeIndex >= 0 &&
                    leisureTypeIndex <
                        m_LeisureTypeExcess.Length)
                {
                    m_LeisureTypeExcess[
                        leisureTypeIndex] +=
                            excess;
                }

                int estimatedVisitService =
                    EstimateServicePerVisit(
                        serviceData,
                        providerData);

                m_WeightedEstimatedVisitService +=
                    (long)excess *
                    estimatedVisitService;
            }

            for (int i = 0;
                i < m_LeisureUnusedRatio.Length;
                i++)
            {
                if (m_LeisureMaxService[i] <= 0)
                {
                    continue;
                }

                m_LeisureUnusedRatio[i] =
                    math.saturate(
                        (float)
                            m_LeisureServiceAvailable[i] /
                        m_LeisureMaxService[i]);
            }

            if (m_TotalLeisureExcess > 0)
            {
                m_EstimatedServicePerVisit =
                    math.max(
                        1,
                        (int)(
                            m_WeightedEstimatedVisitService /
                            m_TotalLeisureExcess));
            }
            else
            {
                m_EstimatedServicePerVisit = 1;
            }
        }

        private void PrepareLeisureBudget()
        {
            if (m_TotalLeisureExcess <= 0)
            {
                m_LeisureSeekerBudget = 0;
                m_LeisureSeekerCredit = 0d;
                return;
            }

            double requestedSeekers =
                (double)m_TotalLeisureExcess /
                kLeisureCorrectionHorizonUpdates /
                math.max(
                    1,
                    m_EstimatedServicePerVisit);

            requestedSeekers *=
                kVanillaSelectionCompensation;

            m_LeisureSeekerCredit +=
                requestedSeekers;

            // Do not allow a temporary lack of eligible cims to build an
            // enormous burst for later.
            m_LeisureSeekerCredit =
                Math.Min(
                    m_LeisureSeekerCredit,
                    kMaxLeisureSeekersPerUpdate *
                    2d);

            int wholeSeekers =
                (int)Math.Floor(
                    m_LeisureSeekerCredit);

            m_LeisureSeekerBudget =
                Math.Min(
                    kMaxLeisureSeekersPerUpdate,
                    wholeSeekers);

            m_LeisureSeekerCredit -=
                m_LeisureSeekerBudget;
        }

        private static int EstimateServicePerVisit(
            Game.Companies.ServiceCompanyData
                serviceData,
            LeisureProviderData providerData)
        {
            int servicePerTick =
                math.max(
                    (int)(
                        (float)serviceData
                            .m_ServiceConsuming /
                        LeisureSystem
                            .kUpdateInterval),
                    1);

            int leisurePerTick =
                math.max(
                    (int)math.ceil(
                        (float)math.max(
                            1,
                            providerData
                                .m_Efficiency) /
                        LeisureSystem
                            .kUpdateInterval),
                    1);

            int ticks =
                math.max(
                    1,
                    (int)math.ceil(
                        (float)
                            kAssumedLeisurePointsToGain /
                        leisurePerTick));

            return math.max(
                1,
                servicePerTick *
                    ticks);
        }

        private bool HasCommercialLeisurePreference(
            Game.Citizens.Citizen citizen,
            Game.Citizens.Household household,
            Game.Buildings.PropertyRenter
                propertyRenter,
            int householdSize,
            EconomyParameterData economyParameters,
            float weather,
            float temperature)
        {
            if (householdSize <= 0)
            {
                return false;
            }

            int disposableIncome =
                EconomyUtils.GetHouseholdDisposableIncome(
                    household,
                    propertyRenter);

            int affluenceIncome =
                EconomyUtils.GetAffluenceReferenceIncome(
                    economyParameters) *
                householdSize;

            if (affluenceIncome <= 0)
            {
                return false;
            }

            Game.Citizens.CitizenAge age =
                citizen.GetAge();

            for (int i = 0;
                i < m_LeisureTypeExcess.Length;
                i++)
            {
                if (m_LeisureTypeExcess[i] <= 0)
                {
                    continue;
                }

                float weight =
                    GetVanillaLeisureWeight(
                        (LeisureType)i,
                        disposableIncome,
                        affluenceIncome,
                        age,
                        weather,
                        temperature);

                if (weight > 0f)
                {
                    return true;
                }
            }

            return false;
        }

        private static float GetVanillaLeisureWeight(
            LeisureType type,
            int disposableIncome,
            int affluenceIncome,
            Game.Citizens.CitizenAge age,
            float weather,
            float temperature)
        {
            float environmentMultiplier = 1f;

            float baseWeight;
            float wealthThreshold;
            float ageWeight;

            switch (type)
            {
                case LeisureType.Meals:
                    baseWeight = 10f;
                    wealthThreshold = 0.2f;

                    ageWeight =
                        age switch
                        {
                            Game.Citizens.CitizenAge.Child =>
                                10f,

                            Game.Citizens.CitizenAge.Teen =>
                                25f,

                            Game.Citizens.CitizenAge.Elderly =>
                                35f,

                            _ =>
                                35f,
                        };

                    break;

                case LeisureType.Entertainment:
                    baseWeight = 10f;
                    wealthThreshold = 0.3f;

                    ageWeight =
                        age switch
                        {
                            Game.Citizens.CitizenAge.Child =>
                                0f,

                            Game.Citizens.CitizenAge.Teen =>
                                45f,

                            Game.Citizens.CitizenAge.Elderly =>
                                10f,

                            _ =>
                                45f,
                        };

                    break;

                case LeisureType.Commercial:
                    baseWeight = 10f;
                    wealthThreshold = 0.4f;

                    ageWeight =
                        age switch
                        {
                            Game.Citizens.CitizenAge.Child =>
                                20f,

                            Game.Citizens.CitizenAge.Teen =>
                                25f,

                            Game.Citizens.CitizenAge.Elderly =>
                                25f,

                            _ =>
                                30f,
                        };

                    break;

                case LeisureType.CityIndoors:
                case LeisureType.CityPark:
                case LeisureType.CityBeach:
                    baseWeight = 10f;
                    wealthThreshold = 0f;

                    ageWeight =
                        age switch
                        {
                            Game.Citizens.CitizenAge.Child =>
                                30f,

                            Game.Citizens.CitizenAge.Teen =>
                                25f,

                            Game.Citizens.CitizenAge.Elderly =>
                                15f,

                            _ =>
                                30f,
                        };

                    environmentMultiplier =
                        type switch
                        {
                            LeisureType.CityIndoors =>
                                1f,

                            LeisureType.CityPark =>
                                2f *
                                (1f -
                                    0.95f *
                                    weather),

                            _ =>
                                0.05f +
                                4f *
                                math.saturate(
                                    0.35f -
                                    weather) *
                                math.saturate(
                                    (temperature -
                                        20f) /
                                    30f),
                        };

                    break;

                case LeisureType.Travel:
                    baseWeight = 1f;
                    wealthThreshold = 0.5f;

                    environmentMultiplier =
                        0.5f +
                        math.saturate(
                            (30f -
                                temperature) /
                            50f);

                    ageWeight =
                        age switch
                        {
                            Game.Citizens.CitizenAge.Child =>
                                15f,

                            Game.Citizens.CitizenAge.Teen =>
                                15f,

                            Game.Citizens.CitizenAge.Elderly =>
                                30f,

                            _ =>
                                40f,
                        };

                    break;

                default:
                    return 0f;
            }

            return
                ageWeight *
                environmentMultiplier *
                baseWeight *
                math.smoothstep(
                    wealthThreshold,
                    1f,
                    math.max(
                        0.01f,
                        (float)disposableIncome /
                        affluenceIncome));
        }

        private static bool IsCommercialLeisureResource(
            Resource resource)
        {
            return
                resource ==
                    Resource.Entertainment ||
                resource ==
                    Resource.Meals ||
                resource ==
                    Resource.Recreation;
        }

        private void LogLeisureWindow()
        {
            int shops = 0;
            int warnings = 0;

            for (int i = 0;
                i < m_LeisureShopCount.Length;
                i++)
            {
                shops +=
                    m_LeisureShopCount[i];

                warnings +=
                    m_LeisureWarningCount[i];
            }

            LogUtils.Info(
                "[CWD-LEISURE] " +
                $"updates={kLeisureLogEveryUpdates} " +
                $"shops={shops} " +
                $"warnings={warnings} " +
                $"excess85={m_TotalLeisureExcess} " +
                $"estServicePerVisit=" +
                $"{m_EstimatedServicePerVisit} " +
                $"examined={m_WindowExamined} " +
                $"eligible={m_WindowEligible} " +
                $"injected={m_WindowInjected} " +
                $"credit=" +
                $"{m_LeisureSeekerCredit.ToString("F2", CultureInfo.InvariantCulture)} " +
                $"missingProviderData=" +
                $"{m_MissingLeisureProviderData}");

            StringBuilder details =
                new(1024);

            Resource[] resources =
            {
                Resource.Entertainment,
                Resource.Meals,
                Resource.Recreation,
            };

            foreach (Resource resource in
                resources)
            {
                int index =
                    EconomyUtils.GetResourceIndex(
                        resource);

                if (index < 0 ||
                    index >=
                        m_LeisureShopCount.Length)
                {
                    continue;
                }

                details
                    .Append(
                        "[CWD-LEISURE-RESOURCE] ")
                    .Append("resource=")
                    .Append(resource)
                    .Append(" shops=")
                    .Append(
                        m_LeisureShopCount[
                            index])
                    .Append(" warnings=")
                    .Append(
                        m_LeisureWarningCount[
                            index])
                    .Append(" avgUnused=")
                    .Append(
                        (m_LeisureUnusedRatio[
                            index] * 100f)
                            .ToString(
                                "F1",
                                CultureInfo.InvariantCulture))
                    .Append('%')
                    .Append(" excess85=")
                    .Append(
                        m_LeisureExcess[
                            index])
                    .AppendLine();
            }

            for (int i = 0;
                i < m_LeisureTypeExcess.Length;
                i++)
            {
                if (m_LeisureTypeExcess[i] <= 0)
                {
                    continue;
                }

                details
                    .Append(
                        "[CWD-LEISURE-TYPE] ")
                    .Append("type=")
                    .Append(
                        (LeisureType)i)
                    .Append(" excess85=")
                    .Append(
                        m_LeisureTypeExcess[i])
                    .AppendLine();
            }

            if (details.Length > 0)
            {
                LogUtils.Info(
                    details.ToString());
            }

            m_WindowExamined = 0;
            m_WindowEligible = 0;
            m_WindowInjected = 0;
        }
    }
}

#endif
