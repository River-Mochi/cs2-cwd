// <copyright file="CommercialLeisureDemandSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialLeisureDemandSystem.cs
// Purpose: DEBUG-only corrective demand for commercial leisure providers.

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
    /// Adds controlled extra vanilla leisure attempts when commercial leisure
    /// providers have excessive unused Service.
    ///
    /// Only citizens who still need leisure and are currently blocked by
    /// vanilla's LeisureSeekerCooldown are considered. From that point onward,
    /// vanilla LeisureSystem chooses the leisure type, destination, travel,
    /// spending, Service consumption, and leisure gain normally.
    /// </summary>
    public partial class CommercialLeisureDemandSystem : GameSystemBase
    {
        private SimulationSystem m_SimulationSystem = null!;
        private TimeSystem m_TimeSystem = null!;
        private ClimateSystem m_ClimateSystem = null!;
        private EndFrameBarrier m_EndFrameBarrier = null!;

        private int m_UpdateCount;

        private int m_WindowExamined;
        private int m_WindowEligible;
        private int m_WindowInjected;

        private bool m_ConfigurationLogged;

        public override int GetUpdateInterval(
            SystemUpdatePhase phase)
        {
            // Match CitizenBehaviorSystem so we inspect the same citizen
            // UpdateFrame bucket immediately after vanilla citizen behavior.
            return 16;
        }

        public override int GetUpdateOffset(
            SystemUpdatePhase phase)
        {
            return 11;
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

            m_ClimateSystem =
                World.GetOrCreateSystemManaged<
                    ClimateSystem>();

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
            m_WindowInjected = 0;

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

            PrepareLeisureBudget();

            if (!m_ConfigurationLogged)
            {
                m_ConfigurationLogged = true;

                LogUtils.Info(
                    "[CWD-LEISURE] ACTIVE DEBUG leisure prototype. " +
                    "Entertainment, Meals, and Recreation shops above " +
                    "85% unused Service create a small adaptive increase " +
                    "in real vanilla leisure attempts. Only idle residents " +
                    "with low leisure and an active vanilla leisure cooldown " +
                    "are used. Vanilla still chooses the leisure type, " +
                    "destination, travel, payment, and Service consumption.");
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
            int injected = 0;

            float weather =
                m_ClimateSystem.precipitation.value;

            float temperature =
                m_ClimateSystem.temperature;

            foreach ((
                RefRO<Game.Citizens.Citizen>
                    citizenRef,
                RefRO<Game.Citizens.HouseholdMember>
                    householdMemberRef,
                RefRO<Game.Citizens.CurrentBuilding>
                    currentBuildingRef,
                RefRO<Game.Citizens.LeisureSeekerCooldown>
                    cooldownRef,
                Entity citizenEntity) in
                SystemAPI
                    .Query<
                        RefRO<Game.Citizens.Citizen>,
                        RefRO<Game.Citizens.HouseholdMember>,
                        RefRO<Game.Citizens.CurrentBuilding>,
                        RefRO<Game.Citizens.LeisureSeekerCooldown>>()
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

                if (m_LeisureSeekerBudget <= 0)
                {
                    continue;
                }

                // Only use a cooldown that is still actively blocking vanilla.
                // That guarantees CitizenBehaviorSystem did not also create a
                // Leisure component for this citizen during this update.
                uint cooldownAge =
                    simulationFrame -
                    cooldownRef.ValueRO
                        .m_SimulationFrame;

                if (cooldownAge >=
                    CitizenBehaviorSystem
                        .kLeisureSeekerCooldownFrames)
                {
                    continue;
                }

                Game.Citizens.Citizen citizen =
                    citizenRef.ValueRO;

                Game.Citizens.CitizenAge age =
                    citizen.GetAge();

                // Keep this first prototype conservative: independent adults
                // and elderly residents only.
                if (age !=
                        Game.Citizens.CitizenAge.Adult &&
                    age !=
                        Game.Citizens.CitizenAge.Elderly)
                {
                    continue;
                }

                // Vanilla only becomes interested in leisure when this counter
                // is below 128. Do not force leisure on an already-satisfied cim.
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

                if (propertyRenter.m_Property ==
                        Entity.Null ||
                    currentBuildingRef.ValueRO
                        .m_CurrentBuilding !=
                        propertyRenter.m_Property)
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

                if (!HasCommercialLeisurePreference(
                        citizen,
                        households[
                            householdEntity],
                        propertyRenter,
                        householdCitizens[
                            householdEntity].Length,
                        economyParameters,
                        weather,
                        temperature))
                {
                    continue;
                }

                eligible++;

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

                commandBuffer.AddComponent(
                    citizenEntity,
                    new Game.Citizens.Leisure
                    {
                        m_TargetAgent =
                            Entity.Null,

                        m_LastPossibleFrame =
                            unchecked(
                                simulationFrame +
                                availableFrames),
                    });

                commandBuffer.RemoveComponent<
                    Game.Citizens.LeisureSeekerCooldown>(
                        citizenEntity);

                injected++;
                m_LeisureSeekerBudget--;
            }

            m_UpdateCount++;

            m_WindowExamined +=
                examined;

            m_WindowEligible +=
                eligible;

            m_WindowInjected +=
                injected;

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
