// <copyright file="CommercialDemandBalanceSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialDemandBalanceSystem.cs
// Purpose: DEBUG-only prototype that gently biases new household shopping needs
// toward commercially abundant resources while preserving vanilla preferences.

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
    public partial class CommercialDemandBalanceSystem : GameSystemBase
    {
        // Vanilla commercial production starts throttling at 80% unsold service.
        // Below this point, do not alter the vanilla shopping weight.
        private const float kBoostStartUnsoldRatio = 0.80f;

        // At 100% unsold service, vanilla shopping weight can be at most 3x.
        private const float kMaxWeightMultiplier = 3f;

        // HouseholdBehaviorSystem runs every 64 simulation frames in 1.6.2.
        // Logging every 32 executions lines up with about 2048 simulation frames,
        // matching our NoCustomers diagnostic cadence.
        private const int kLogEveryUpdates = 32;

        private readonly long[] m_ServiceAvailable =
            new long[EconomyUtils.ResourceCount];

        private readonly long[] m_MaxService =
            new long[EconomyUtils.ResourceCount];

        private readonly float[] m_UnsoldRatio =
            new float[EconomyUtils.ResourceCount];

        private readonly float[] m_WeightMultiplier =
            new float[EconomyUtils.ResourceCount];

        private readonly int[] m_WorkingWeights =
            new int[EconomyUtils.ResourceCount];

        private readonly int[] m_WindowChangedFrom =
            new int[EconomyUtils.ResourceCount];

        private readonly int[] m_WindowChangedTo =
            new int[EconomyUtils.ResourceCount];

        private SimulationSystem m_SimulationSystem = null!;
        private ResourceSystem m_ResourceSystem = null!;

        private int m_UpdateCount;
        private int m_WindowNeeds;
        private int m_WindowChanged;
        private bool m_ConfigurationLogged;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            // Keep exactly the same interval as vanilla HouseholdBehaviorSystem.
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
            m_WindowNeeds = 0;
            m_WindowChanged = 0;
            m_ConfigurationLogged = false;
        }

        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingleton<Game.Prefabs.EconomyParameterData>(
                    out Game.Prefabs.EconomyParameterData economyParameters))
            {
                return;
            }

            float resourceDemandMultiplier = 1f;

            if (SystemAPI.TryGetSingleton<Game.Prefabs.Modes.ModeSettingData>(
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
                    "[CWD-BALANCE] ACTIVE DEBUG prototype. " +
                    "Vanilla household shopping weights are boosted only for " +
                    "commercial resources above 80% unsold service; " +
                    "maximum boost is 3.00x at 100% unsold. " +
                    "Vehicle shopping is left vanilla for this first test.");
            }

            uint updateFrame =
                SimulationUtils.GetUpdateFrameWithInterval(
                    m_SimulationSystem.frameIndex,
                    (uint)GetUpdateInterval(
                        SystemUpdatePhase.GameSimulation),
                    16);

            int activeNeeds = 0;
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
                Game.Citizens.HouseholdNeed need =
                    needRef.ValueRO;

                if (need.m_Resource == Resource.NoResource ||
                    need.m_Amount <= 0)
                {
                    continue;
                }

                activeNeeds++;

                // Vehicle shopping has a special vanilla code path that can create
                // a household-owned car when the purchase amount is exactly 50.
                // Leave those decisions untouched in the first prototype.
                if (need.m_Resource == Resource.Vehicles)
                {
                    continue;
                }

                DynamicBuffer<Game.Citizens.HouseholdCitizen> citizens =
                    householdCitizens[householdEntity];

                if (citizens.Length == 0)
                {
                    continue;
                }

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

                if (ownedVehicles.HasBuffer(householdEntity))
                {
                    carCount =
                        ownedVehicles[householdEntity].Length;
                }

                Resource targetResource =
                    SelectBalancedResource(
                        householdEntity,
                        m_SimulationSystem.frameIndex,
                        disposableIncome,
                        affluenceIncome,
                        carCount,
                        citizens,
                        ref citizenDatas,
                        ref resourceDatas);

                if (targetResource == Resource.NoResource ||
                    targetResource == Resource.Vehicles ||
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
                    m_ResourceSystem.GetPrefab(targetResource);

                if (targetPrefab == Entity.Null ||
                    !resourceDatas.HasComponent(targetPrefab))
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
                    fromIndex < m_WindowChangedFrom.Length)
                {
                    m_WindowChangedFrom[fromIndex]++;
                }

                if (toIndex >= 0 &&
                    toIndex < m_WindowChangedTo.Length)
                {
                    m_WindowChangedTo[toIndex]++;
                }
            }

            m_UpdateCount++;
            m_WindowNeeds += activeNeeds;
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

            for (int i = 0; i < m_UnsoldRatio.Length; i++)
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
                    .WithAll<Game.Companies.CommercialCompany>()
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
                    serviceCompanyDatas[companyPrefab];

                if (serviceData.m_MaxService <= 0)
                {
                    continue;
                }

                Resource resource =
                    industrialProcessDatas[
                        companyPrefab].m_Output.m_Resource;

                int resourceIndex =
                    EconomyUtils.GetResourceIndex(resource);

                if (resourceIndex < 0 ||
                    resourceIndex >= m_ServiceAvailable.Length)
                {
                    continue;
                }

                int available =
                    math.clamp(
                        serviceRef.ValueRO.m_ServiceAvailable,
                        0,
                        serviceData.m_MaxService);

                m_ServiceAvailable[resourceIndex] += available;
                m_MaxService[resourceIndex] +=
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

                // Do not influence actual vehicle demand until the general
                // balancing approach has been proven safe.
                if (resource == Resource.Vehicles)
                {
                    continue;
                }

                Entity resourcePrefab =
                    m_ResourceSystem.GetPrefab(resource);

                if (resourcePrefab == Entity.Null ||
                    !resourceDatas.HasComponent(resourcePrefab))
                {
                    continue;
                }

                Game.Prefabs.ResourceData resourceData =
                    resourceDatas[resourcePrefab];

                // HouseholdBehaviorSystem excludes leisure resources from its
                // normal shopping-resource lottery. Preserve that behavior.
                if (resourceData.m_IsLeisure)
                {
                    continue;
                }

                if (unsoldRatio <= kBoostStartUnsoldRatio)
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

        private Resource SelectBalancedResource(
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
                m_WorkingWeights,
                0,
                m_WorkingWeights.Length);

            int totalWeight = 0;

            ResourceIterator iterator =
                ResourceIterator.GetIterator();

            while (iterator.Next())
            {
                Resource resource =
                    iterator.resource;

                Entity resourcePrefab =
                    m_ResourceSystem.GetPrefab(resource);

                if (resourcePrefab == Entity.Null ||
                    !resourceDatas.HasComponent(resourcePrefab))
                {
                    continue;
                }

                Game.Prefabs.ResourceData resourceData =
                    resourceDatas[resourcePrefab];

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
                    EconomyUtils.GetResourceIndex(resource);

                if (resourceIndex < 0 ||
                    resourceIndex >= m_WorkingWeights.Length)
                {
                    continue;
                }

                float multiplier =
                    resource == Resource.Vehicles
                        ? 1f
                        : m_WeightMultiplier[resourceIndex];

                int adjustedWeight =
                    math.max(
                        1,
                        (int)math.round(
                            vanillaWeight *
                            multiplier));

                // Defensive guard. Vanilla weights are much smaller than this,
                // but do not allow an overflow to corrupt the random selection.
                if (totalWeight >
                    int.MaxValue - adjustedWeight)
                {
                    return Resource.NoResource;
                }

                m_WorkingWeights[resourceIndex] =
                    adjustedWeight;

                totalWeight += adjustedWeight;
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
                        277803737u));

            if (seed == 0)
            {
                seed = 1;
            }

            Unity.Mathematics.Random random =
                new(seed);

            int selection =
                random.NextInt(totalWeight);

            for (int i = 0;
                i < m_WorkingWeights.Length;
                i++)
            {
                int weight =
                    m_WorkingWeights[i];

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
                m_WindowNeeds > 0
                    ? 100f *
                      m_WindowChanged /
                      m_WindowNeeds
                    : 0f;

            LogUtils.Info(
                "[CWD-BALANCE] " +
                $"updates={kLogEveryUpdates} " +
                $"shoppingNeeds={m_WindowNeeds} " +
                $"rebalanced={m_WindowChanged} " +
                $"rate=" +
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
                    .Append("[CWD-BALANCE-RESOURCE] ")
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
                    .Append(m_WindowChangedFrom[i])
                    .Append(" changedTo=")
                    .Append(m_WindowChangedTo[i])
                    .AppendLine();
            }

            if (details.Length > 0)
            {
                LogUtils.Info(details.ToString());
            }

            Array.Clear(
                m_WindowChangedFrom,
                0,
                m_WindowChangedFrom.Length);

            Array.Clear(
                m_WindowChangedTo,
                0,
                m_WindowChangedTo.Length);

            m_WindowNeeds = 0;
            m_WindowChanged = 0;
        }
    }
}

#endif
