// <copyright file="CommercialDemandBalanceSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialDemandBalanceSystem.cs
// Purpose: DEBUG-only prototype that rebalances newly-created physical retail
// shopping needs using actual commercial service availability.

#if DEBUG

using System;
using System.Globalization;
using System.Text;

using Colossal.Serialization.Entities;

using CS2Shared.RiverMochi;

using Game;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;

using Unity.Entities;
using Unity.Mathematics;

namespace CityWatchdog.Systems
{
    /// <summary>
    /// Runs immediately after vanilla HouseholdBehaviorSystem.
    /// Rebalances only newly-created physical retail shopping needs.
    /// </summary>
    public partial class CommercialDemandBalanceSystem : GameSystemBase
    {
        // Desired center of the feedback curve.
        //
        // Vanilla commercial production starts throttling around 80% unused
        // service and the No Customers warning appears around 90%, so 70%
        // gives the economy room to move before either threshold is reached.
        private const float kTargetUnsoldRatio = 0.70f;

        // Never completely starve a resource of its vanilla shopping weight.
        private const float kMinWeightMultiplier = 0.25f;

        // Strong correction for resources that are almost completely unused.
        private const float kMaxWeightMultiplier = 8f;

        // ln(2) / 0.10.
        //
        // This means every +10 percentage points above the 70% target doubles
        // the shopping weight:
        //
        // 70% = 1x
        // 80% = 2x
        // 90% = 4x
        // 100% = 8x
        //
        // Going the other direction halves the weight:
        //
        // 60% = 0.5x
        // 50% = 0.25x
        private const float kCorrectionSlope = 6.9314718f;

        // Individual warning counts matter too. A category-wide average can
        // hide many individual companies already above the warning threshold.
        //
        // warningFloor = 80% + (20% * warning share)
        //
        // Example:
        //   50% of shops warning -> effective floor 90%
        //   100% warning         -> effective floor 100%
        private const float kWarningFloorStart = 0.80f;

        // Preserve fractional multiplier differences even when vanilla resource
        // weights are small.
        private const float kSelectionWeightScale = 16f;

        // HouseholdBehaviorSystem runs every 64 simulation frames.
        private const int kLogEveryUpdates = 32;

        private readonly long[] m_ServiceAvailable =
            new long[EconomyUtils.ResourceCount];

        private readonly long[] m_MaxService =
            new long[EconomyUtils.ResourceCount];

        private readonly int[] m_CompanyCount =
            new int[EconomyUtils.ResourceCount];

        private readonly int[] m_WarningCount =
            new int[EconomyUtils.ResourceCount];

        private readonly float[] m_UnsoldRatio =
            new float[EconomyUtils.ResourceCount];

        private readonly float[] m_EffectiveUnsoldRatio =
            new float[EconomyUtils.ResourceCount];

        private readonly float[] m_WeightMultiplier =
            new float[EconomyUtils.ResourceCount];

        private readonly int[] m_BalancedWeights =
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
        private int m_WindowEligibleRetailNeeds;

        private int m_WindowVehicleNeeds;
        private int m_WindowOfficeNeeds;
        private int m_WindowLeisureNeeds;
        private int m_WindowOtherExcludedNeeds;

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
                m_CompanyCount,
                0,
                m_CompanyCount.Length);

            Array.Clear(
                m_WarningCount,
                0,
                m_WarningCount.Length);

            Array.Clear(
                m_UnsoldRatio,
                0,
                m_UnsoldRatio.Length);

            Array.Clear(
                m_EffectiveUnsoldRatio,
                0,
                m_EffectiveUnsoldRatio.Length);

            Array.Clear(
                m_BalancedWeights,
                0,
                m_BalancedWeights.Length);

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
            m_WindowEligibleRetailNeeds = 0;

            m_WindowVehicleNeeds = 0;
            m_WindowOfficeNeeds = 0;
            m_WindowLeisureNeeds = 0;
            m_WindowOtherExcludedNeeds = 0;

            m_WindowChanged = 0;

            m_ConfigurationLogged = false;
        }

        protected override void OnUpdate()
        {
            uint simulationFrame =
                m_SimulationSystem.frameIndex;

            // Only touch shopping needs when the before-vanilla snapshot was
            // captured during this exact simulation update.
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

            BuildCommercialBalanceState(
                serviceCompanyDatas,
                industrialProcessDatas,
                resourceDatas);

            if (!m_ConfigurationLogged)
            {
                m_ConfigurationLogged = true;

                LogUtils.Info(
                    "[CWD-BALANCE] ACTIVE DEBUG prototype v3. " +
                    "Only newly-created physical retail shopping needs are " +
                    "rebalanced. 70% unused service is neutral. Every +10 " +
                    "percentage points doubles vanilla shopping weight; every " +
                    "-10 points halves it, clamped to 0.25x-8.00x. Individual " +
                    "No Customers warning share can increase receiver pressure. " +
                    "Vehicles, leisure, and office resources remain vanilla.");
            }

            uint updateFrame =
                SimulationUtils.GetUpdateFrameWithInterval(
                    simulationFrame,
                    (uint)GetUpdateInterval(
                        SystemUpdatePhase.GameSimulation),
                    16);

            int newNeeds = 0;
            int eligibleRetailNeeds = 0;

            int vehicleNeeds = 0;
            int officeNeeds = 0;
            int leisureNeeds = 0;
            int otherExcludedNeeds = 0;

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
                // Do not repeatedly reconsider an existing pending shopping
                // request. V3 only evaluates a need vanilla just created.
                if (!m_PrepareSystem.WasEmptyBeforeVanilla(
                        householdEntity))
                {
                    continue;
                }

                Game.Citizens.HouseholdNeed need =
                    needRef.ValueRO;

                if (need.m_Resource == Resource.NoResource ||
                    need.m_Amount <= 0)
                {
                    continue;
                }

                newNeeds++;

                if (!IsEligibleRetailResource(
                        need.m_Resource,
                        ref resourceDatas))
                {
                    if (need.m_Resource == Resource.Vehicles)
                    {
                        vehicleNeeds++;
                    }
                    else if (EconomyUtils.IsOfficeResource(
                                 need.m_Resource))
                    {
                        officeNeeds++;
                    }
                    else
                    {
                        Entity excludedPrefab =
                            m_ResourceSystem.GetPrefab(
                                need.m_Resource);

                        if (excludedPrefab != Entity.Null &&
                            resourceDatas.HasComponent(
                                excludedPrefab) &&
                            resourceDatas[
                                excludedPrefab].m_IsLeisure)
                        {
                            leisureNeeds++;
                        }
                        else
                        {
                            otherExcludedNeeds++;
                        }
                    }

                    continue;
                }

                DynamicBuffer<Game.Citizens.HouseholdCitizen> citizens =
                    householdCitizens[householdEntity];

                if (citizens.Length == 0)
                {
                    continue;
                }

                eligibleRetailNeeds++;

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
                        ownedVehicles[
                            householdEntity].Length;
                }

                Resource targetResource =
                    SelectBalancedRetailResource(
                        householdEntity,
                        simulationFrame,
                        disposableIncome,
                        affluenceIncome,
                        carCount,
                        citizens,
                        ref citizenDatas,
                        ref resourceDatas);

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

                need.m_Resource =
                    targetResource;

                need.m_Amount =
                    amount;

                needRef.ValueRW =
                    need;

                changedNeeds++;

                if (fromIndex >= 0 &&
                    fromIndex <
                    m_WindowChangedFrom.Length)
                {
                    m_WindowChangedFrom[
                        fromIndex]++;
                }

                if (toIndex >= 0 &&
                    toIndex <
                    m_WindowChangedTo.Length)
                {
                    m_WindowChangedTo[
                        toIndex]++;
                }
            }

            m_UpdateCount++;

            m_WindowNewNeeds +=
                newNeeds;

            m_WindowEligibleRetailNeeds +=
                eligibleRetailNeeds;

            m_WindowVehicleNeeds +=
                vehicleNeeds;

            m_WindowOfficeNeeds +=
                officeNeeds;

            m_WindowLeisureNeeds +=
                leisureNeeds;

            m_WindowOtherExcludedNeeds +=
                otherExcludedNeeds;

            m_WindowChanged +=
                changedNeeds;

            if (m_UpdateCount %
                kLogEveryUpdates == 0)
            {
                LogWindow();
            }
        }

        private void BuildCommercialBalanceState(
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

            Array.Clear(
                m_CompanyCount,
                0,
                m_CompanyCount.Length);

            Array.Clear(
                m_WarningCount,
                0,
                m_WarningCount.Length);

            for (int i = 0;
                i < m_UnsoldRatio.Length;
                i++)
            {
                m_UnsoldRatio[i] = 0f;
                m_EffectiveUnsoldRatio[i] = 0f;
                m_WeightMultiplier[i] = 1f;
            }

            foreach ((
                RefRO<Game.Companies.ServiceAvailable> serviceRef,
                RefRO<Game.Companies.CompanyNotifications> notificationsRef,
                RefRO<Game.Prefabs.PrefabRef> prefabRef) in
                SystemAPI
                    .Query<
                        RefRO<Game.Companies.ServiceAvailable>,
                        RefRO<Game.Companies.CompanyNotifications>,
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
                    resourceIndex] +=
                    available;

                m_MaxService[
                    resourceIndex] +=
                    serviceData.m_MaxService;

                m_CompanyCount[
                    resourceIndex]++;

                if (notificationsRef.ValueRO
                        .m_NoCustomersEntity != Entity.Null)
                {
                    m_WarningCount[
                        resourceIndex]++;
                }
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
                    math.saturate(
                        (float)m_ServiceAvailable[i] /
                        m_MaxService[i]);

                m_UnsoldRatio[i] =
                    unsoldRatio;

                // Vehicles have special car-purchase semantics.
                // Office resources are virtual and use the office pipeline.
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
                    resourceDatas[
                        resourcePrefab];

                // Leisure, hotels, meals, entertainment, recreation, etc.
                // need their own balancing path later.
                if (resourceData.m_IsLeisure)
                {
                    continue;
                }

                float effectiveUnsoldRatio =
                    unsoldRatio;

                // A category-wide average can hide many individual companies
                // already above the warning threshold.
                if (m_CompanyCount[i] > 0 &&
                    m_WarningCount[i] > 0)
                {
                    float warningShare =
                        (float)m_WarningCount[i] /
                        m_CompanyCount[i];

                    float warningFloor =
                        kWarningFloorStart +
                        ((1f -
                            kWarningFloorStart) *
                            warningShare);

                    effectiveUnsoldRatio =
                        math.max(
                            effectiveUnsoldRatio,
                            warningFloor);
                }

                effectiveUnsoldRatio =
                    math.saturate(
                        effectiveUnsoldRatio);

                m_EffectiveUnsoldRatio[i] =
                    effectiveUnsoldRatio;

                float multiplier =
                    math.exp(
                        kCorrectionSlope *
                        (effectiveUnsoldRatio -
                            kTargetUnsoldRatio));

                m_WeightMultiplier[i] =
                    math.clamp(
                        multiplier,
                        kMinWeightMultiplier,
                        kMaxWeightMultiplier);
            }
        }

        private bool IsEligibleRetailResource(
            Resource resource,
            ref ComponentLookup<Game.Prefabs.ResourceData>
                resourceDatas)
        {
            if (resource == Resource.NoResource ||
                resource == Resource.Vehicles ||
                EconomyUtils.IsOfficeResource(
                    resource))
            {
                return false;
            }

            int resourceIndex =
                EconomyUtils.GetResourceIndex(
                    resource);

            if (resourceIndex < 0 ||
                resourceIndex >=
                    m_MaxService.Length ||
                m_MaxService[
                    resourceIndex] <= 0)
            {
                return false;
            }

            Entity resourcePrefab =
                m_ResourceSystem.GetPrefab(
                    resource);

            if (resourcePrefab == Entity.Null ||
                !resourceDatas.HasComponent(
                    resourcePrefab))
            {
                return false;
            }

            return !resourceDatas[
                resourcePrefab].m_IsLeisure;
        }

        /// <summary>
        /// Re-runs only the physical retail portion of vanilla's resource
        /// lottery using:
        ///
        ///     vanilla household weight * commercial balance multiplier
        ///
        /// Total physical-shopping frequency is unchanged. Only the resource
        /// selected for a newly-created physical retail need can change.
        /// </summary>
        private Resource SelectBalancedRetailResource(
            Entity household,
            uint simulationFrame,
            int disposableIncome,
            int affluenceIncome,
            int carCount,
            DynamicBuffer<Game.Citizens.HouseholdCitizen> citizens,
            ref ComponentLookup<Game.Citizens.Citizen>
                citizenDatas,
            ref ComponentLookup<Game.Prefabs.ResourceData>
                resourceDatas)
        {
            Array.Clear(
                m_BalancedWeights,
                0,
                m_BalancedWeights.Length);

            int totalWeight = 0;

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

                if (resourcePrefab == Entity.Null ||
                    !resourceDatas.HasComponent(
                        resourcePrefab))
                {
                    continue;
                }

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

                int resourceIndex =
                    EconomyUtils.GetResourceIndex(
                        resource);

                if (resourceIndex < 0 ||
                    resourceIndex >=
                        m_BalancedWeights.Length)
                {
                    continue;
                }

                float multiplier =
                    m_WeightMultiplier[
                        resourceIndex];

                int adjustedWeight =
                    math.max(
                        1,
                        (int)math.round(
                            vanillaWeight *
                            multiplier *
                            kSelectionWeightScale));

                if (totalWeight >
                    int.MaxValue -
                    adjustedWeight)
                {
                    return Resource.NoResource;
                }

                m_BalancedWeights[
                    resourceIndex] =
                    adjustedWeight;

                totalWeight +=
                    adjustedWeight;
            }

            if (totalWeight <= 0)
            {
                return Resource.NoResource;
            }

            uint seed =
                unchecked(
                    ((uint)household.Index *
                        747796405u) ^
                    ((uint)household.Version *
                        2891336453u) ^
                    (simulationFrame *
                        277803737u) ^
                    0xB41A9CE3u);

            if (seed == 0)
            {
                seed = 1;
            }

            Unity.Mathematics.Random random =
                new(seed);

            int selection =
                random.NextInt(
                    totalWeight);

            for (int i = 0;
                i < m_BalancedWeights.Length;
                i++)
            {
                int weight =
                    m_BalancedWeights[i];

                if (weight <= 0)
                {
                    continue;
                }

                if (selection < weight)
                {
                    return EconomyUtils.GetResource(i);
                }

                selection -=
                    weight;
            }

            return Resource.NoResource;
        }

        private void LogWindow()
        {
            float changedPercent =
                m_WindowEligibleRetailNeeds > 0
                    ? 100f *
                      m_WindowChanged /
                      m_WindowEligibleRetailNeeds
                    : 0f;

            LogUtils.Info(
                "[CWD-BALANCE] " +
                $"updates={kLogEveryUpdates} " +
                $"newNeeds={m_WindowNewNeeds} " +
                $"retail={m_WindowEligibleRetailNeeds} " +
                $"vehicle={m_WindowVehicleNeeds} " +
                $"office={m_WindowOfficeNeeds} " +
                $"leisure={m_WindowLeisureNeeds} " +
                $"other={m_WindowOtherExcludedNeeds} " +
                $"rebalanced={m_WindowChanged} " +
                $"changeRate=" +
                $"{changedPercent.ToString("F1", CultureInfo.InvariantCulture)}%");

            StringBuilder details =
                new(4096);

            for (int i = 0;
                i < EconomyUtils.ResourceCount;
                i++)
            {
                if (m_CompanyCount[i] <= 0)
                {
                    continue;
                }

                Resource resource =
                    EconomyUtils.GetResource(i);

                int changedFrom =
                    m_WindowChangedFrom[i];

                int changedTo =
                    m_WindowChangedTo[i];

                bool interesting =
                    m_WarningCount[i] > 0 ||
                    changedFrom > 0 ||
                    changedTo > 0 ||
                    m_WeightMultiplier[i] <
                        0.999f ||
                    m_WeightMultiplier[i] >
                        1.001f;

                if (!interesting)
                {
                    continue;
                }

                details
                    .Append(
                        "[CWD-BALANCE-RESOURCE] ")
                    .Append("resource=")
                    .Append(resource)
                    .Append(" avgUnsold=")
                    .Append(
                        (m_UnsoldRatio[i] * 100f)
                            .ToString(
                                "F1",
                                CultureInfo.InvariantCulture))
                    .Append('%')
                    .Append(" effective=")
                    .Append(
                        (m_EffectiveUnsoldRatio[i] * 100f)
                            .ToString(
                                "F1",
                                CultureInfo.InvariantCulture))
                    .Append('%')
                    .Append(" shops=")
                    .Append(
                        m_CompanyCount[i])
                    .Append(" warnings=")
                    .Append(
                        m_WarningCount[i])
                    .Append(" weight=")
                    .Append(
                        m_WeightMultiplier[i]
                            .ToString(
                                "F2",
                                CultureInfo.InvariantCulture))
                    .Append('x')
                    .Append(" changedFrom=")
                    .Append(
                        changedFrom)
                    .Append(" changedTo=")
                    .Append(
                        changedTo)
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
            m_WindowEligibleRetailNeeds = 0;

            m_WindowVehicleNeeds = 0;
            m_WindowOfficeNeeds = 0;
            m_WindowLeisureNeeds = 0;
            m_WindowOtherExcludedNeeds = 0;

            m_WindowChanged = 0;
        }
    }
}

#endif
