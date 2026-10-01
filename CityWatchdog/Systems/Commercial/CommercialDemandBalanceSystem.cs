// <copyright file="CommercialDemandBalanceSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialDemandBalanceSystem.cs
// Purpose: DEBUG-only prototype that gently biases newly-created household
// shopping needs toward commercially abundant resources while preserving
// vanilla shopping choices whenever no abundance boost is applied.

#if DEBUG

using System;
using System.Globalization;
using System.Text;

using Colossal.Serialization.Entities;

using CS2Shared.RiverMochi;

using Game;
using Game.Buildings;
using Game.Citizens;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Prefabs.Modes;
using Game.Simulation;
using Game.Vehicles;

using Unity.Entities;
using Unity.Mathematics;

namespace CityWatchdog.Systems
{
    /// <summary>
    /// Runs immediately after vanilla HouseholdBehaviorSystem and adds only
    /// the extra probability mass caused by commercial overabundance.
    /// </summary>
    public partial class CommercialDemandBalanceSystem : GameSystemBase
    {
        // Vanilla commercial production already starts throttling at 80%
        // unsold service. Below this level we leave shopping completely vanilla.
        private const float kBoostStartUnsoldRatio = 0.80f;

        // At 100% unsold service, the resource gets at most 3x its normal
        // vanilla shopping weight.
        private const float kMaxWeightMultiplier = 3f;

        // HouseholdBehaviorSystem runs every 64 simulation frames.
        // 32 executions gives us roughly the same diagnostic window as before.
        private const int kLogEveryUpdates = 32;

        private readonly long[] m_ServiceAvailable =
            new long[EconomyUtils.ResourceCount];

        private readonly long[] m_MaxService =
            new long[EconomyUtils.ResourceCount];

        private readonly float[] m_UnsoldRatio =
            new float[EconomyUtils.ResourceCount];

        private readonly float[] m_WeightMultiplier =
            new float[EconomyUtils.ResourceCount];

        // This contains only EXTRA weight above vanilla.
        private readonly int[] m_ExtraWeights =
            new int[EconomyUtils.ResourceCount];

        private readonly int[] m_WindowChangedFrom =
            new int[EconomyUtils.ResourceCount];

        private readonly int[] m_WindowChangedTo =
            new int[EconomyUtils.ResourceCount];

        private SimulationSystem m_SimulationSystem = null!;
        private ResourceSystem m_ResourceSystem = null!;
        private CommercialDemandBalancePrepareSystem m_PrepareSystem = null!;

        private int m_UpdateCount;
        private int m_WindowNewNeeds;
        private int m_WindowEligibleNeeds;
        private int m_WindowExtraDraws;
        private int m_WindowChanged;
        private bool m_ConfigurationLogged;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 262144 /
                (HouseholdBehaviorSystem.kUpdatesPerDay * 16);
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_SimulationSystem =
                World.GetOrCreateSystemManaged<SimulationSystem>();

            m_ResourceSystem =
                World.GetOrCreateSystemManaged<ResourceSystem>();

            m_PrepareSystem =
                World.GetOrCreateSystemManaged<
                    CommercialDemandBalancePrepareSystem>();
        }

        protected override void OnGameLoaded(Context serializationContext)
        {
            base.OnGameLoaded(serializationContext);

            Array.Clear(
                m_ServiceAvailable,
                0,
                m_ServiceAvailable.Length);

            Array.Clear(
                m_MaxService,
                0,
                m_MaxService.Length);

            Array.Clear(
                m_UnsoldRatio,
                0,
                m_UnsoldRatio.Length);

            Array.Clear(
                m_ExtraWeights,
                0,
                m_ExtraWeights.Length);

            Array.Clear(
                m_WindowChangedFrom,
                0,
                m_WindowChangedFrom.Length);

            Array.Clear(
                m_WindowChangedTo,
                0,
                m_WindowChangedTo.Length);

            for (int i = 0; i < m_WeightMultiplier.Length; i++)
            {
                m_WeightMultiplier[i] = 1f;
            }

            m_UpdateCount = 0;
            m_WindowNewNeeds = 0;
            m_WindowEligibleNeeds = 0;
            m_WindowExtraDraws = 0;
            m_WindowChanged = 0;
            m_ConfigurationLogged = false;
        }

        protected override void OnUpdate()
        {
            uint simulationFrame =
                m_SimulationSystem.frameIndex;

            // Defensive check: only touch needs if our before-vanilla snapshot
            // came from this exact simulation update.
            if (!m_PrepareSystem.HasSnapshotFor(
                    simulationFrame))
            {
                return;
            }

            if (!SystemAPI.TryGetSingleton<
                    Game.Prefabs.EconomyParameterData>(
                    out Game.Prefabs.EconomyParameterData economyParameters))
            {
                return;
            }

            float resourceDemandMultiplier = 1f;

            if (SystemAPI.TryGetSingleton<
                    Game.Prefabs.Modes.ModeSettingData>(
                    out Game.Prefabs.Modes.ModeSettingData modeSetting) &&
                modeSetting.m_Enable)
            {
                resourceDemandMultiplier =
                    modeSetting.m_ResourceDemandPerCitizenMultiplier;
            }

            ComponentLookup<Game.Companies.ServiceCompanyData>
                serviceCompanyDatas =
                    SystemAPI.GetComponentLookup<
                        Game.Companies.ServiceCompanyData>(true);

            ComponentLookup<Game.Prefabs.IndustrialProcessData>
                industrialProcessDatas =
                    SystemAPI.GetComponentLookup<
                        Game.Prefabs.IndustrialProcessData>(true);

            ComponentLookup<Game.Prefabs.ResourceData>
                resourceDatas =
                    SystemAPI.GetComponentLookup<
                        Game.Prefabs.ResourceData>(true);

            ComponentLookup<Game.Citizens.Citizen>
                citizenDatas =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.Citizen>(true);

            ComponentLookup<Game.Prefabs.ConsumptionData>
                consumptionDatas =
                    SystemAPI.GetComponentLookup<
                        Game.Prefabs.ConsumptionData>(true);

            ComponentLookup<Game.Prefabs.PrefabRef>
                prefabRefs =
                    SystemAPI.GetComponentLookup<
                        Game.Prefabs.PrefabRef>(true);

            BufferLookup<Game.Citizens.HouseholdCitizen>
                householdCitizens =
                    SystemAPI.GetBufferLookup<
                        Game.Citizens.HouseholdCitizen>(true);

            BufferLookup<Game.Economy.Resources>
                householdResources =
                    SystemAPI.GetBufferLookup<
                        Game.Economy.Resources>(true);

            BufferLookup<Game.Vehicles.OwnedVehicle>
                ownedVehicles =
                    SystemAPI.GetBufferLookup<
                        Game.Vehicles.OwnedVehicle>(true);

            BufferLookup<Game.Buildings.Renter>
                renterBuffers =
                    SystemAPI.GetBufferLookup<
                        Game.Buildings.Renter>(true);

            BuildCommercialAbundance(
                serviceCompanyDatas,
                industrialProcessDatas,
                resourceDatas);

            if (!m_ConfigurationLogged)
            {
                m_ConfigurationLogged = true;

                LogUtils.Info(
                    "[CWD-BALANCE] ACTIVE DEBUG prototype v2. " +
                    "Only newly-created vanilla retail shopping needs may be " +
                    "redirected. Vanilla choices are preserved unless commercial " +
                    "abundance adds extra probability above 80% unsold service. " +
                    "Vehicles, leisure resources, and office resources remain vanilla.");
            }

            uint updateFrame =
                SimulationUtils.GetUpdateFrameWithInterval(
                    simulationFrame,
                    (uint)GetUpdateInterval(
                        SystemUpdatePhase.GameSimulation),
                    16);

            int newNeeds = 0;
            int eligibleNeeds = 0;
            int extraDraws = 0;
            int changedNeeds = 0;

            foreach ((
                RefRW<Game.Citizens.HouseholdNeed> needRef,
                RefRO<Game.Citizens.Household> householdRef,
                RefRO<Game.Buildings.PropertyRenter> propertyRenterRef,
                Entity householdEntity) in
                SystemAPI
                    .Query<
                        RefRW<Game.Citizens.HouseholdNeed>,
                        RefRO<Game.Citizens.Household>,
                        RefRO<Game.Buildings.PropertyRenter>>()
                    .WithAll<
                        Game.Citizens.HouseholdCitizen,
                        Game.Economy.Resources>()
                    .WithNone<
                        Game.Citizens.TouristHousehold,
                        Game.Citizens.HomelessHousehold,
                        Game.Agents.MovingAway>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>()
                    .WithSharedComponentFilter(
                        new Game.Simulation.UpdateFrame(updateFrame))
                    .WithEntityAccess())
            {
                // This household already had a need before vanilla ran.
                // Do not reconsider an existing/pending shopping request.
                if (!m_PrepareSystem.WasEmptyBeforeVanilla(
                        householdEntity))
                {
                    continue;
                }

                Game.Citizens.HouseholdNeed need =
                    needRef.ValueRO;

                // Vanilla did not create a need during this update.
                if (need.m_Resource == Resource.NoResource ||
                    need.m_Amount <= 0)
                {
                    continue;
                }

                newNeeds++;

                // Keep vehicles, leisure, office services, and resources without
                // an active commercial retail market completely vanilla.
                if (!IsEligibleRetailResource(
                        need.m_Resource,
                        ref resourceDatas))
                {
                    continue;
                }

                DynamicBuffer<Game.Citizens.HouseholdCitizen> citizens =
                    householdCitizens[householdEntity];

                if (citizens.Length == 0)
                {
                    continue;
                }

                eligibleNeeds++;

                Game.Citizens.Household household =
                    householdRef.ValueRO;

                Game.Buildings.PropertyRenter propertyRenter =
                    propertyRenterRef.ValueRO;

                int disposableIncome =
                    EconomyUtils.GetHouseholdDisposableIncome(
                        household,
                        propertyRenter);

                int affluenceIncome =
                    EconomyUtils.GetAffluenceReferenceIncome(
                        economyParameters) *
                    citizens.Length;

                int carCount = 0;

                if (ownedVehicles.HasBuffer(
                        householdEntity))
                {
                    carCount =
                        ownedVehicles[householdEntity].Length;
                }

                Resource targetResource =
                    SelectExtraAbundanceResource(
                        householdEntity,
                        simulationFrame,
                        disposableIncome,
                        affluenceIncome,
                        carCount,
                        citizens,
                        ref citizenDatas,
                        ref resourceDatas,
                        out bool usedExtraDraw);

                if (!usedExtraDraw)
                {
                    // This is the normal case: keep vanilla's choice untouched.
                    continue;
                }

                extraDraws++;

                if (targetResource == Resource.NoResource ||
                    targetResource == need.m_Resource)
                {
                    continue;
                }

                DynamicBuffer<Game.Economy.Resources> resources =
                    householdResources[householdEntity];

                int spendableMoney =
                    EconomyUtils.GetHouseholdSpendableMoney(
                        household,
                        resources,
                        ref renterBuffers,
                        ref consumptionDatas,
                        ref prefabRefs,
                        propertyRenter);

                if (spendableMoney <
                    HouseholdBehaviorSystem.kMinimumShoppingMoney)
                {
                    continue;
                }

                Entity targetPrefab =
                    m_ResourceSystem.GetPrefab(
                        targetResource);

                if (targetPrefab == Entity.Null ||
                    !resourceDatas.HasComponent(
                        targetPrefab))
                {
                    continue;
                }

                Game.Prefabs.ResourceData targetResourceData =
                    resourceDatas[targetPrefab];

                float marketPrice =
                    EconomyUtils.GetMarketPrice(
                        targetResourceData);

                if (marketPrice <= 0f)
                {
                    continue;
                }

                int amount =
                    math.clamp(
                        (int)((float)spendableMoney /
                            marketPrice),
                        0,
                        HouseholdBehaviorSystem
                            .kMaxHouseholdNeedAmount);

                amount =
                    (int)((float)amount *
                        resourceDemandMultiplier);

                if (amount <= 0)
                {
                    continue;
                }

                int fromIndex =
                    EconomyUtils.GetResourceIndex(
                        need.m_Resource);

                int toIndex =
                    EconomyUtils.GetResourceIndex(
                        targetResource);

                need.m_Resource = targetResource;
                need.m_Amount = amount;

                needRef.ValueRW = need;

                changedNeeds++;

                if (fromIndex >= 0 &&
                    fromIndex <
                    m_WindowChangedFrom.Length)
                {
                    m_WindowChangedFrom[fromIndex]++;
                }

                if (toIndex >= 0 &&
                    toIndex <
                    m_WindowChangedTo.Length)
                {
                    m_WindowChangedTo[toIndex]++;
                }
            }

            m_UpdateCount++;
            m_WindowNewNeeds += newNeeds;
            m_WindowEligibleNeeds += eligibleNeeds;
            m_WindowExtraDraws += extraDraws;
            m_WindowChanged += changedNeeds;

            if (m_UpdateCount % kLogEveryUpdates == 0)
            {
                LogWindow();
            }
        }

        private void BuildCommercialAbundance(
            ComponentLookup<Game.Companies.ServiceCompanyData>
                serviceCompanyDatas,
            ComponentLookup<Game.Prefabs.IndustrialProcessData>
                industrialProcessDatas,
            ComponentLookup<Game.Prefabs.ResourceData>
                resourceDatas)
        {
            Array.Clear(
                m_ServiceAvailable,
                0,
                m_ServiceAvailable.Length);

            Array.Clear(
                m_MaxService,
                0,
                m_MaxService.Length);

            for (int i = 0;
                i < m_UnsoldRatio.Length;
                i++)
            {
                m_UnsoldRatio[i] = 0f;
                m_WeightMultiplier[i] = 1f;
            }

            foreach ((
                RefRO<Game.Companies.ServiceAvailable> serviceRef,
                RefRO<Game.Prefabs.PrefabRef> prefabRef) in
                SystemAPI
                    .Query<
                        RefRO<Game.Companies.ServiceAvailable>,
                        RefRO<Game.Prefabs.PrefabRef>>()
                    .WithAll<
                        Game.Companies.CommercialCompany>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>())
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

                Game.Companies.ServiceCompanyData serviceData =
                    serviceCompanyDatas[
                        companyPrefab];

                if (serviceData.m_MaxService <= 0)
                {
                    continue;
                }

                Resource resource =
                    industrialProcessDatas[
                        companyPrefab]
                        .m_Output.m_Resource;

                int resourceIndex =
                    EconomyUtils.GetResourceIndex(
                        resource);

                if (resourceIndex < 0 ||
                    resourceIndex >=
                    m_ServiceAvailable.Length)
                {
                    continue;
                }

                int available =
                    math.clamp(
                        serviceRef.ValueRO
                            .m_ServiceAvailable,
                        0,
                        serviceData.m_MaxService);

                m_ServiceAvailable[
                    resourceIndex] += available;

                m_MaxService[
                    resourceIndex] +=
                    serviceData.m_MaxService;
            }

            for (int i = 0;
                i < m_WeightMultiplier.Length;
                i++)
            {
                if (m_MaxService[i] <= 0)
                {
                    continue;
                }

                Resource resource =
                    EconomyUtils.GetResource(i);

                float unsoldRatio =
                    (float)m_ServiceAvailable[i] /
                    m_MaxService[i];

                m_UnsoldRatio[i] =
                    math.saturate(unsoldRatio);

                if (resource == Resource.Vehicles ||
                    EconomyUtils.IsOfficeResource(
                        resource))
                {
                    continue;
                }

                Entity resourcePrefab =
                    m_ResourceSystem.GetPrefab(
                        resource);

                if (resourcePrefab == Entity.Null ||
                    !resourceDatas.HasComponent(
                        resourcePrefab))
                {
                    continue;
                }

                Game.Prefabs.ResourceData resourceData =
                    resourceDatas[resourcePrefab];

                // Meals, Lodging, Entertainment, Recreation, etc. continue
                // through their normal leisure/tourism systems.
                if (resourceData.m_IsLeisure)
                {
                    continue;
                }

                if (unsoldRatio <=
                    kBoostStartUnsoldRatio)
                {
                    continue;
                }

                float abundance =
                    math.saturate(
                        (unsoldRatio -
                            kBoostStartUnsoldRatio) /
                        (1f -
                            kBoostStartUnsoldRatio));

                m_WeightMultiplier[i] =
                    math.lerp(
                        1f,
                        kMaxWeightMultiplier,
                        abundance);
            }
        }

        private bool IsEligibleRetailResource(
            Resource resource,
            ref ComponentLookup<Game.Prefabs.ResourceData>
                resourceDatas)
        {
            if (resource == Resource.NoResource ||
                resource == Resource.Vehicles ||
                EconomyUtils.IsOfficeResource(resource))
            {
                return false;
            }

            int resourceIndex =
                EconomyUtils.GetResourceIndex(resource);

            if (resourceIndex < 0 ||
                resourceIndex >= m_MaxService.Length ||
                m_MaxService[resourceIndex] <= 0)
            {
                return false;
            }

            Entity resourcePrefab =
                m_ResourceSystem.GetPrefab(resource);

            if (resourcePrefab == Entity.Null ||
                !resourceDatas.HasComponent(resourcePrefab))
            {
                return false;
            }

            return !resourceDatas[
                resourcePrefab].m_IsLeisure;
        }

        /// <summary>
        /// Keeps the existing vanilla choice unless the random draw lands in
        /// the EXTRA probability mass created by abundance.
        ///
        /// If vanilla weights are W and abundance adds E:
        ///     keep vanilla choice with probability W / (W + E)
        ///     choose from extra weights with probability E / (W + E)
        ///
        /// Therefore when E == 0 there is exactly zero intervention.
        /// </summary>
        private Resource SelectExtraAbundanceResource(
            Entity household,
            uint simulationFrame,
            int disposableIncome,
            int affluenceIncome,
            int carCount,
            DynamicBuffer<Game.Citizens.HouseholdCitizen> citizens,
            ref ComponentLookup<Game.Citizens.Citizen>
                citizenDatas,
            ref ComponentLookup<Game.Prefabs.ResourceData>
                resourceDatas,
            out bool usedExtraDraw)
        {
            usedExtraDraw = false;

            Array.Clear(
                m_ExtraWeights,
                0,
                m_ExtraWeights.Length);

            int vanillaRetailWeight = 0;
            int extraWeightTotal = 0;

            ResourceIterator iterator =
                ResourceIterator.GetIterator();

            while (iterator.Next())
            {
                Resource resource =
                    iterator.resource;

                if (!IsEligibleRetailResource(
                        resource,
                        ref resourceDatas))
                {
                    continue;
                }

                Entity resourcePrefab =
                    m_ResourceSystem.GetPrefab(
                        resource);

                Game.Prefabs.ResourceData resourceData =
                    resourceDatas[
                        resourcePrefab];

                int vanillaWeight =
                    HouseholdBehaviorSystem
                        .GetResourceShopWeightWithAge(
                            disposableIncome,
                            resourceData,
                            carCount,
                            leisureIncluded: false,
                            citizens,
                            ref citizenDatas,
                            affluenceIncome);

                if (vanillaWeight <= 0)
                {
                    continue;
                }

                if (vanillaRetailWeight >
                    int.MaxValue - vanillaWeight)
                {
                    return Resource.NoResource;
                }

                vanillaRetailWeight +=
                    vanillaWeight;

                int resourceIndex =
                    EconomyUtils.GetResourceIndex(
                        resource);

                float multiplier =
                    m_WeightMultiplier[
                        resourceIndex];

                if (multiplier <= 1.001f)
                {
                    continue;
                }

                int extraWeight =
                    math.max(
                        0,
                        (int)math.round(
                            vanillaWeight *
                            (multiplier - 1f)));

                if (extraWeight <= 0)
                {
                    continue;
                }

                if (extraWeightTotal >
                    int.MaxValue - extraWeight)
                {
                    return Resource.NoResource;
                }

                m_ExtraWeights[
                    resourceIndex] =
                    extraWeight;

                extraWeightTotal +=
                    extraWeight;
            }

            // This is the critical difference from v1:
            // no abundance = absolutely no new random resource selection.
            if (vanillaRetailWeight <= 0 ||
                extraWeightTotal <= 0)
            {
                return Resource.NoResource;
            }

            if (vanillaRetailWeight >
                int.MaxValue - extraWeightTotal)
            {
                return Resource.NoResource;
            }

            int combinedWeight =
                vanillaRetailWeight +
                extraWeightTotal;

            uint seed =
                unchecked(
                    ((uint)household.Index *
                        747796405u) ^
                    ((uint)household.Version *
                        2891336453u) ^
                    (simulationFrame *
                        277803737u) ^
                    0xA511E9B3u);

            if (seed == 0)
            {
                seed = 1;
            }

            Unity.Mathematics.Random random =
                new(seed);

            int selection =
                random.NextInt(
                    combinedWeight);

            // The draw landed in vanilla's original probability mass.
            // Keep the resource vanilla already chose.
            if (selection <
                vanillaRetailWeight)
            {
                return Resource.NoResource;
            }

            usedExtraDraw = true;

            selection -=
                vanillaRetailWeight;

            for (int i = 0;
                i < m_ExtraWeights.Length;
                i++)
            {
                int weight =
                    m_ExtraWeights[i];

                if (weight <= 0)
                {
                    continue;
                }

                if (selection < weight)
                {
                    return EconomyUtils.GetResource(i);
                }

                selection -= weight;
            }

            return Resource.NoResource;
        }

        private void LogWindow()
        {
            float changedPercent =
                m_WindowEligibleNeeds > 0
                    ? 100f *
                      m_WindowChanged /
                      m_WindowEligibleNeeds
                    : 0f;

            float extraDrawPercent =
                m_WindowEligibleNeeds > 0
                    ? 100f *
                      m_WindowExtraDraws /
                      m_WindowEligibleNeeds
                    : 0f;

            LogUtils.Info(
                "[CWD-BALANCE] " +
                $"updates={kLogEveryUpdates} " +
                $"newNeeds={m_WindowNewNeeds} " +
                $"eligibleRetail={m_WindowEligibleNeeds} " +
                $"extraDraws={m_WindowExtraDraws} " +
                $"extraRate=" +
                $"{extraDrawPercent.ToString("F1", CultureInfo.InvariantCulture)}% " +
                $"rebalanced={m_WindowChanged} " +
                $"changeRate=" +
                $"{changedPercent.ToString("F1", CultureInfo.InvariantCulture)}%");

            StringBuilder details =
                new(2048);

            for (int i = 0;
                i < EconomyUtils.ResourceCount;
                i++)
            {
                bool boosted =
                    m_WeightMultiplier[i] >
                    1.001f;

                bool changed =
                    m_WindowChangedFrom[i] > 0 ||
                    m_WindowChangedTo[i] > 0;

                if (!boosted && !changed)
                {
                    continue;
                }

                Resource resource =
                    EconomyUtils.GetResource(i);

                details
                    .Append(
                        "[CWD-BALANCE-RESOURCE] ")
                    .Append("resource=")
                    .Append(resource)
                    .Append(" unsold=")
                    .Append(
                        (m_UnsoldRatio[i] * 100f)
                            .ToString(
                                "F1",
                                CultureInfo.InvariantCulture))
                    .Append('%')
                    .Append(" weight=")
                    .Append(
                        m_WeightMultiplier[i]
                            .ToString(
                                "F2",
                                CultureInfo.InvariantCulture))
                    .Append('x')
                    .Append(" changedFrom=")
                    .Append(
                        m_WindowChangedFrom[i])
                    .Append(" changedTo=")
                    .Append(
                        m_WindowChangedTo[i])
                    .AppendLine();
            }

            if (details.Length > 0)
            {
                LogUtils.Info(
                    details.ToString());
            }

            Array.Clear(
                m_WindowChangedFrom,
                0,
                m_WindowChangedFrom.Length);

            Array.Clear(
                m_WindowChangedTo,
                0,
                m_WindowChangedTo.Length);

            m_WindowNewNeeds = 0;
            m_WindowEligibleNeeds = 0;
            m_WindowExtraDraws = 0;
            m_WindowChanged = 0;
        }
    }
}

#endif
