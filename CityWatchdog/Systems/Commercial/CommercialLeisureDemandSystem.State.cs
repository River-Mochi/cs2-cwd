// <copyright file="CommercialLeisureDemandSystem.State.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialLeisureDemandSystem.State.cs
// Purpose: Individual-provider pressure and diagnostics for targeted leisure demand.

#if DEBUG

using System;
using System.Collections.Generic;
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
        private struct ProviderPressure
        {
            public Entity Provider;
            public Entity Building;

            public Resource Resource;
            public LeisureType LeisureType;

            public int ExcessService;
            public int WorkingExcessService;

            public int EstimatedServicePerVisit;
        }

        private const float kLeisureTargetServiceRatio =
            0.85f;

        // This system now runs at vanilla LeisureSystem's interval.
        // Spread one measured correction over roughly half a full pass
        // through LeisureSystem's 16 UpdateFrame buckets.
        private const int kLeisureCorrectionHorizonUpdates =
            8;

        private const int kLeisureLogEveryUpdates =
            32;

        // The trips are now specifically targeted, so we no longer need
        // dozens of speculative extra seekers in one update.
        private const int kMaxTargetedVisitorsPerUpdate =
            4;

        private const int kAssumedLeisurePointsToGain =
            160;

        // Prefer nearby stressed businesses but do not completely exclude
        // a more distant one. A sufficiently severe Service excess can
        // therefore still overcome the distance penalty.
        private const float kDistanceScale =
            2000f;

        private readonly List<ProviderPressure>
            m_ProviderPressures = new();

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

        private readonly int[]
            m_WindowTargetedByResource =
                new int[
                    EconomyUtils.ResourceCount];

        private long m_TotalLeisureExcess;

        private int m_RequiredTargetedVisits;

        private double m_TargetedVisitCredit;

        private int m_TargetedVisitBudget;

        private int m_MissingProviderData;

        private void ResetLeisureState()
        {
            m_ProviderPressures.Clear();

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
                m_WindowTargetedByResource,
                0,
                m_WindowTargetedByResource.Length);

            m_TotalLeisureExcess = 0;

            m_RequiredTargetedVisits = 0;

            m_TargetedVisitCredit = 0d;

            m_TargetedVisitBudget = 0;

            m_MissingProviderData = 0;
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
            m_ProviderPressures.Clear();

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

            m_TotalLeisureExcess = 0;

            m_RequiredTargetedVisits = 0;

            m_MissingProviderData = 0;

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
                RefRO<Game.Buildings.PropertyRenter>
                    propertyRenterRef,
                Entity company) in
                SystemAPI
                    .Query<
                        RefRO<
                            Game.Companies.ServiceAvailable>,
                        RefRO<
                            Game.Companies.CompanyNotifications>,
                        RefRO<
                            PrefabRef>,
                        RefRO<
                            Game.Buildings.PropertyRenter>>()
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

                // Match the important stock side of vanilla's
                // No Customers notification.
                if (stock <= 200)
                {
                    continue;
                }

                if (!leisureProviderDatas.HasComponent(
                        companyPrefab))
                {
                    m_MissingProviderData++;
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

                LeisureProviderData providerData =
                    leisureProviderDatas[
                        companyPrefab];

                int estimatedServicePerVisit =
                    EstimateServicePerVisit(
                        serviceData,
                        providerData);

                m_ProviderPressures.Add(
                    new ProviderPressure
                    {
                        Provider =
                            company,

                        Building =
                            propertyRenterRef
                                .ValueRO
                                .m_Property,

                        Resource =
                            resource,

                        LeisureType =
                            providerData
                                .m_LeisureType,

                        ExcessService =
                            excess,

                        WorkingExcessService =
                            excess,

                        EstimatedServicePerVisit =
                            estimatedServicePerVisit,
                    });

                m_LeisureExcess[
                    resourceIndex] +=
                        excess;

                m_TotalLeisureExcess +=
                    excess;

                // Count per provider, not globally. Even a shop only 100
                // Service above target still needs at least one visit.
                m_RequiredTargetedVisits +=
                    Math.Max(
                        1,
                        (int)Math.Ceiling(
                            (double)excess /
                            estimatedServicePerVisit));
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
        }

        private void PrepareTargetedVisitBudget()
        {
            if (m_RequiredTargetedVisits <= 0)
            {
                m_TargetedVisitBudget = 0;

                m_TargetedVisitCredit = 0d;

                return;
            }

            m_TargetedVisitCredit +=
                (double)m_RequiredTargetedVisits /
                kLeisureCorrectionHorizonUpdates;

            // Prevent a temporary shortage of suitable cims from producing
            // a large burst later.
            m_TargetedVisitCredit =
                Math.Min(
                    m_TargetedVisitCredit,
                    kMaxTargetedVisitorsPerUpdate *
                    2d);

            int wholeVisits =
                (int)Math.Floor(
                    m_TargetedVisitCredit);

            m_TargetedVisitBudget =
                Math.Min(
                    kMaxTargetedVisitorsPerUpdate,
                    wholeVisits);

            m_TargetedVisitCredit -=
                m_TargetedVisitBudget;
        }

        private bool TrySelectTargetProvider(
            Entity citizenEntity,
            uint simulationFrame,
            Game.Citizens.Citizen citizen,
            Game.Citizens.Household household,
            Game.Buildings.PropertyRenter
                propertyRenter,
            int householdSize,
            EconomyParameterData economyParameters,
            Entity home,
            ref ComponentLookup<
                Game.Objects.Transform>
                    transforms,
            out int providerIndex)
        {
            providerIndex = -1;

            if (householdSize <= 0 ||
                m_ProviderPressures.Count == 0)
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

            bool hasHomePosition =
                transforms.HasComponent(
                    home);

            float3 homePosition =
                hasHomePosition
                    ? transforms[
                        home].m_Position
                    : default;

            float totalScore = 0f;

            for (int i = 0;
                i < m_ProviderPressures.Count;
                i++)
            {
                ProviderPressure provider =
                    m_ProviderPressures[i];

                if (provider.WorkingExcessService <= 0)
                {
                    continue;
                }

                float vanillaWeight =
                    GetVanillaLeisureWeight(
                        provider.LeisureType,
                        disposableIncome,
                        affluenceIncome,
                        age);

                if (vanillaWeight <= 0f)
                {
                    continue;
                }

                float distanceWeight = 1f;

                if (hasHomePosition &&
                    provider.Building !=
                        Entity.Null &&
                    transforms.HasComponent(
                        provider.Building))
                {
                    float distance =
                        math.distance(
                            homePosition,
                            transforms[
                                provider.Building]
                                .m_Position);

                    distanceWeight =
                        1f /
                        (1f +
                            distance /
                            kDistanceScale);
                }

                float preference =
                    math.sqrt(
                        vanillaWeight);

                float score =
                    provider
                        .WorkingExcessService *
                    preference *
                    distanceWeight;

                totalScore +=
                    score;
            }

            if (totalScore <= 0f ||
                !math.isfinite(
                    totalScore))
            {
                return false;
            }

            uint seed =
                unchecked(
                    ((uint)citizenEntity.Index *
                        747796405u) ^
                    ((uint)citizenEntity.Version *
                        2891336453u) ^
                    (simulationFrame *
                        277803737u) ^
                    0x1E157A9u);

            if (seed == 0)
            {
                seed = 1;
            }

            Unity.Mathematics.Random random =
                new(seed);

            float selection =
                random.NextFloat(
                    0f,
                    totalScore);

            for (int i = 0;
                i < m_ProviderPressures.Count;
                i++)
            {
                ProviderPressure provider =
                    m_ProviderPressures[i];

                if (provider.WorkingExcessService <= 0)
                {
                    continue;
                }

                float vanillaWeight =
                    GetVanillaLeisureWeight(
                        provider.LeisureType,
                        disposableIncome,
                        affluenceIncome,
                        age);

                if (vanillaWeight <= 0f)
                {
                    continue;
                }

                float distanceWeight = 1f;

                if (hasHomePosition &&
                    provider.Building !=
                        Entity.Null &&
                    transforms.HasComponent(
                        provider.Building))
                {
                    float distance =
                        math.distance(
                            homePosition,
                            transforms[
                                provider.Building]
                                .m_Position);

                    distanceWeight =
                        1f /
                        (1f +
                            distance /
                            kDistanceScale);
                }

                float preference =
                    math.sqrt(
                        vanillaWeight);

                float score =
                    provider
                        .WorkingExcessService *
                    preference *
                    distanceWeight;

                if (selection < score)
                {
                    providerIndex = i;

                    return true;
                }

                selection -=
                    score;
            }

            return false;
        }

        private void ConsumeTargetProvider(
            int providerIndex)
        {
            if (providerIndex < 0 ||
                providerIndex >=
                    m_ProviderPressures.Count)
            {
                return;
            }

            ProviderPressure provider =
                m_ProviderPressures[
                    providerIndex];

            provider.WorkingExcessService =
                math.max(
                    0,
                    provider.WorkingExcessService -
                    provider
                        .EstimatedServicePerVisit);

            m_ProviderPressures[
                providerIndex] =
                    provider;
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

        private static float GetVanillaLeisureWeight(
            LeisureType type,
            int disposableIncome,
            int affluenceIncome,
            Game.Citizens.CitizenAge age)
        {
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

                default:
                    return 0f;
            }

            return
                ageWeight *
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
                "mode=targeted " +
                $"updates={kLeisureLogEveryUpdates} " +
                $"shops={shops} " +
                $"warnings={warnings} " +
                $"stressedProviders={m_ProviderPressures.Count} " +
                $"excess85={m_TotalLeisureExcess} " +
                $"requiredVisits={m_RequiredTargetedVisits} " +
                $"examined={m_WindowExamined} " +
                $"eligible={m_WindowEligible} " +
                $"targeted={m_WindowTargeted} " +
                $"estimatedService={m_WindowEstimatedService} " +
                $"credit=" +
                $"{m_TargetedVisitCredit.ToString("F2", CultureInfo.InvariantCulture)} " +
                $"missingProviderData={m_MissingProviderData}");

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
                    .Append(" targeted=")
                    .Append(
                        m_WindowTargetedByResource[
                            index])
                    .AppendLine();
            }

            if (details.Length > 0)
            {
                LogUtils.Info(
                    details.ToString());
            }

            Array.Clear(
                m_WindowTargetedByResource,
                0,
                m_WindowTargetedByResource.Length);

            m_WindowExamined = 0;
            m_WindowEligible = 0;
            m_WindowTargeted = 0;

            m_WindowEstimatedService = 0;
        }
    }
}

#endif
