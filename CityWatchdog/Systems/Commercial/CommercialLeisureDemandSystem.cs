// <copyright file="CommercialLeisureDemandSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialLeisureDemandSystem.cs
// Purpose: DEBUG-only targeted correction for commercial leisure providers.

#if DEBUG

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
    /// Adds a small number of targeted real leisure trips to individual
    /// commercial leisure providers with excessive unused Service.
    ///
    /// Only residents who genuinely still have low leisure and are currently
    /// idle at home are considered. Normal TripNeeded and Leisure systems
    /// still handle travel, spending, Service consumption, and leisure gain.
    /// </summary>
    public partial class CommercialLeisureDemandSystem : GameSystemBase
    {
        private SimulationSystem m_SimulationSystem = null!;
        private TimeSystem m_TimeSystem = null!;
        private EndFrameBarrier m_EndFrameBarrier = null!;

        private int m_UpdateCount;

        private int m_WindowExamined;
        private int m_WindowEligible;
        private int m_WindowTargeted;

        private long m_WindowEstimatedService;

        private bool m_ConfigurationLogged;

        public override int GetUpdateInterval(
            SystemUpdatePhase phase)
        {
            // Match vanilla LeisureSystem exactly: 64 simulation frames.
            return 262144 /
                LeisureSystem.kUpdatePerDay;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_SimulationSystem =
                World.GetOrCreateSystemManaged<
                    SimulationSystem>();

            m_TimeSystem =
                World.GetOrCreateSystemManaged<
                    TimeSystem>();

            m_EndFrameBarrier =
                World.GetOrCreateSystemManaged<
                    EndFrameBarrier>();
        }

        protected override void OnGameLoaded(
            Context serializationContext)
        {
            base.OnGameLoaded(
                serializationContext);

            ResetLeisureState();

            m_UpdateCount = 0;

            m_WindowExamined = 0;
            m_WindowEligible = 0;
            m_WindowTargeted = 0;

            m_WindowEstimatedService = 0;

            m_ConfigurationLogged = false;
        }

        protected override void OnUpdate()
        {
            uint simulationFrame =
                m_SimulationSystem.frameIndex;

            if (!SystemAPI.TryGetSingleton<
                    EconomyParameterData>(
                    out EconomyParameterData
                        economyParameters))
            {
                return;
            }

            ComponentLookup<
                Game.Companies.ServiceCompanyData>
                serviceCompanyDatas =
                    SystemAPI.GetComponentLookup<
                        Game.Companies.ServiceCompanyData>(
                            true);

            ComponentLookup<
                IndustrialProcessData>
                industrialProcessDatas =
                    SystemAPI.GetComponentLookup<
                        IndustrialProcessData>(
                            true);

            ComponentLookup<
                LeisureProviderData>
                leisureProviderDatas =
                    SystemAPI.GetComponentLookup<
                        LeisureProviderData>(
                            true);

            BuildLeisurePressure(
                serviceCompanyDatas,
                industrialProcessDatas,
                leisureProviderDatas);

            PrepareTargetedVisitBudget();

            if (!m_ConfigurationLogged)
            {
                m_ConfigurationLogged = true;

                LogUtils.Info(
                    "[CWD-LEISURE] ACTIVE DEBUG targeted leisure prototype v2. " +
                    "Only individual Entertainment, Meals, and Recreation " +
                    "providers above 85% unused Service are corrective targets. " +
                    "Only idle residents at home with genuinely low leisure are " +
                    "eligible. Normal TripNeeded and LeisureSystem behavior still " +
                    "handles the actual trip, payment, Service consumption, and " +
                    "leisure gain.");
            }

            uint updateFrame =
                SimulationUtils.GetUpdateFrameWithInterval(
                    simulationFrame,
                    (uint)GetUpdateInterval(
                        SystemUpdatePhase.GameSimulation),
                    16);

            ComponentLookup<
                Game.Citizens.Household>
                households =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.Household>(
                            true);

            ComponentLookup<
                Game.Buildings.PropertyRenter>
                propertyRenters =
                    SystemAPI.GetComponentLookup<
                        Game.Buildings.PropertyRenter>(
                            true);

            ComponentLookup<
                Game.Citizens.TouristHousehold>
                touristHouseholds =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.TouristHousehold>(
                            true);

            ComponentLookup<
                Game.Citizens.CommuterHousehold>
                commuterHouseholds =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.CommuterHousehold>(
                            true);

            ComponentLookup<
                Game.Citizens.HomelessHousehold>
                homelessHouseholds =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.HomelessHousehold>(
                            true);

            ComponentLookup<
                Game.Agents.MovingAway>
                movingAway =
                    SystemAPI.GetComponentLookup<
                        Game.Agents.MovingAway>(
                            true);

            ComponentLookup<
                Game.Citizens.LeisureSeekerCooldown>
                leisureCooldowns =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.LeisureSeekerCooldown>(
                            true);

            ComponentLookup<
                Game.Objects.Transform>
                transforms =
                    SystemAPI.GetComponentLookup<
                        Game.Objects.Transform>(
                            true);

            BufferLookup<
                Game.Citizens.TripNeeded>
                tripBuffers =
                    SystemAPI.GetBufferLookup<
                        Game.Citizens.TripNeeded>(
                            true);

            BufferLookup<
                Game.Citizens.HouseholdCitizen>
                householdCitizens =
                    SystemAPI.GetBufferLookup<
                        Game.Citizens.HouseholdCitizen>(
                            true);

            BufferLookup<
                Game.Economy.Resources>
                householdResources =
                    SystemAPI.GetBufferLookup<
                        Game.Economy.Resources>(
                            true);

            EntityCommandBuffer commandBuffer =
                m_EndFrameBarrier.CreateCommandBuffer();

            int examined = 0;
            int eligible = 0;
            int targeted = 0;

            long estimatedService = 0;

            foreach ((
                RefRO<Game.Citizens.Citizen>
                    citizenRef,
                RefRO<Game.Citizens.HouseholdMember>
                    householdMemberRef,
                RefRO<Game.Citizens.CurrentBuilding>
                    currentBuildingRef,
                Entity citizenEntity) in
                SystemAPI
                    .Query<
                        RefRO<Game.Citizens.Citizen>,
                        RefRO<Game.Citizens.HouseholdMember>,
                        RefRO<Game.Citizens.CurrentBuilding>>()
                    .WithAll<
                        Game.Citizens.TripNeeded>()
                    .WithNone<
                        Game.Citizens.Leisure,
                        Game.Citizens.TravelPurpose,
                        Game.Citizens.AttendingMeeting>()
                    .WithNone<
                        Game.Citizens.HealthProblem,
                        Game.Citizens.Worker,
                        Game.Citizens.Student>()
                    .WithNone<
                        Game.Citizens.Criminal,
                        Game.Companies.ResourceBuyer,
                        Game.Common.Target>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>()
                    .WithSharedComponentFilter(
                        new UpdateFrame(updateFrame))
                    .WithEntityAccess())
            {
                examined++;

                if (m_TargetedVisitBudget <= 0 ||
                    m_ProviderPressures.Count == 0)
                {
                    continue;
                }

                Game.Citizens.Citizen citizen =
                    citizenRef.ValueRO;

                Game.Citizens.CitizenAge age =
                    citizen.GetAge();

                // Keep this prototype conservative.
                // No work or school schedules are being overridden.
                if (age !=
                        Game.Citizens.CitizenAge.Adult &&
                    age !=
                        Game.Citizens.CitizenAge.Elderly)
                {
                    continue;
                }

                // Vanilla's own leisure decision starts becoming relevant
                // below 128. Do not manufacture leisure for a satisfied cim.
                if (citizen.m_LeisureCounter >= 128)
                {
                    continue;
                }

                Entity householdEntity =
                    householdMemberRef.ValueRO
                        .m_Household;

                if (householdEntity ==
                        Entity.Null ||
                    !households.HasComponent(
                        householdEntity) ||
                    !propertyRenters.HasComponent(
                        householdEntity) ||
                    !householdCitizens.HasBuffer(
                        householdEntity) ||
                    !householdResources.HasBuffer(
                        householdEntity))
                {
                    continue;
                }

                if (touristHouseholds.HasComponent(
                        householdEntity) ||
                    commuterHouseholds.HasComponent(
                        householdEntity) ||
                    homelessHouseholds.HasComponent(
                        householdEntity) ||
                    movingAway.HasComponent(
                        householdEntity))
                {
                    continue;
                }

                Game.Buildings.PropertyRenter
                    propertyRenter =
                        propertyRenters[
                            householdEntity];

                Entity home =
                    propertyRenter.m_Property;

                // Only idle residents who are physically at home.
                if (home == Entity.Null ||
                    currentBuildingRef.ValueRO
                        .m_CurrentBuilding !=
                        home)
                {
                    continue;
                }

                if (!tripBuffers.HasBuffer(
                        citizenEntity) ||
                    tripBuffers[
                        citizenEntity].Length != 0)
                {
                    continue;
                }

                if (!TrySelectTargetProvider(
                        citizenEntity,
                        simulationFrame,
                        citizen,
                        households[
                            householdEntity],
                        propertyRenter,
                        householdCitizens[
                            householdEntity].Length,
                        economyParameters,
                        home,
                        ref transforms,
                        out int providerIndex))
                {
                    continue;
                }

                eligible++;

                ProviderPressure provider =
                    m_ProviderPressures[
                        providerIndex];

                float2 sleepTime =
                    CitizenBehaviorSystem.GetSleepTime(
                        citizenEntity,
                        citizen,
                        ref economyParameters,
                        isWorker: false,
                        default,
                        isStudent: false,
                        default);

                float timeLeft =
                    GetTimeLeftUntilInterval(
                        sleepTime,
                        m_TimeSystem.normalizedTime);

                uint availableFrames =
                    (uint)math.max(
                        1f,
                        timeLeft * 262144f);

                // LeisureSystem has already run this update.
                // These components become visible at EndFrame and create one
                // normal Purpose.Leisure trip to the selected stressed provider.
                commandBuffer.AddComponent(
                    citizenEntity,
                    new Game.Citizens.Leisure
                    {
                        m_TargetAgent =
                            provider.Provider,

                        m_LastPossibleFrame =
                            unchecked(
                                simulationFrame +
                                availableFrames),
                    });

                commandBuffer.AppendToBuffer(
                    citizenEntity,
                    new Game.Citizens.TripNeeded
                    {
                        m_TargetAgent =
                            provider.Provider,

                        m_Purpose =
                            Game.Citizens.Purpose.Leisure,

                        m_Priority = 128,
                    });

                commandBuffer.AddComponent(
                    citizenEntity,
                    new Game.Common.Target
                    {
                        m_Target =
                            provider.Provider,
                    });

                // A previous failed leisure search may have left vanilla's
                // 20,000-frame retry cooldown. This new targeted trip is valid,
                // so clear that stale block if it exists.
                if (leisureCooldowns.HasComponent(
                        citizenEntity))
                {
                    commandBuffer.RemoveComponent<
                        Game.Citizens.LeisureSeekerCooldown>(
                            citizenEntity);
                }

                ConsumeTargetProvider(
                    providerIndex);

                targeted++;

                estimatedService +=
                    provider
                        .EstimatedServicePerVisit;

                int resourceIndex =
                    EconomyUtils.GetResourceIndex(
                        provider.Resource);

                if (resourceIndex >= 0 &&
                    resourceIndex <
                        m_WindowTargetedByResource
                            .Length)
                {
                    m_WindowTargetedByResource[
                        resourceIndex]++;
                }

                m_TargetedVisitBudget--;
            }

            m_UpdateCount++;

            m_WindowExamined +=
                examined;

            m_WindowEligible +=
                eligible;

            m_WindowTargeted +=
                targeted;

            m_WindowEstimatedService +=
                estimatedService;

            if (m_UpdateCount %
                kLeisureLogEveryUpdates == 0)
            {
                LogLeisureWindow();
            }
        }

        private static float GetTimeLeftUntilInterval(
            float2 interval,
            float normalizedTime)
        {
            if (normalizedTime < interval.x)
            {
                return interval.x -
                    normalizedTime;
            }

            return 1f -
                normalizedTime +
                interval.x;
        }
    }
}

#endif
