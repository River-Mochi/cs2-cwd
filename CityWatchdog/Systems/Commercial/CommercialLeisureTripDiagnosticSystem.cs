// <copyright file="CommercialLeisureTripDiagnosticSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialLeisureTripDiagnosticSystem.cs
// Purpose: DEBUG-only tracing of corrective leisure trips.

#if DEBUG

using Colossal.Serialization.Entities;

using CS2Shared.RiverMochi;

using Game;
using Game.Simulation;

using Unity.Entities;
using Unity.Mathematics;

namespace CityWatchdog.Systems
{
    public partial class CommercialLeisureTripDiagnosticSystem :
        GameSystemBase
    {
        private const int kLogEveryUpdates = 128;

        private const uint kAbortGraceFrames = 512;
        private const uint kProbeTimeoutFrames = 20000;

        private const byte kProbeSeen = 1;
        private const byte kTripSeen = 2;
        private const byte kTargetSeen = 4;
        private const byte kPathPendingSeen = 8;
        private const byte kPathResolvedSeen = 16;
        private const byte kTravelSeen = 32;
        private const byte kArrivalSeen = 64;

        private SimulationSystem m_SimulationSystem = null!;
        private EndFrameBarrier m_EndFrameBarrier = null!;

        private int m_UpdateCount;

        private int m_WindowCreated;
        private int m_WindowTripSeen;
        private int m_WindowTargetSeen;
        private int m_WindowPathPending;
        private int m_WindowPathResolved;
        private int m_WindowTraveling;
        private int m_WindowArrived;
        private int m_WindowSpendConfirmed;

        private int m_WindowPathFailed;
        private int m_WindowAborted;
        private int m_WindowTimedOut;

        private long m_WindowNetServiceDrop;

        private int m_ActiveProbes;

        public override int GetUpdateInterval(
            SystemUpdatePhase phase)
        {
            // Faster than LeisureSystem so short-lived path/travel states
            // are less likely to be missed.
            return 16;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_SimulationSystem =
                World.GetOrCreateSystemManaged<
                    SimulationSystem>();

            m_EndFrameBarrier =
                World.GetOrCreateSystemManaged<
                    EndFrameBarrier>();
        }

        protected override void OnGameLoaded(
            Context serializationContext)
        {
            base.OnGameLoaded(
                serializationContext);

            m_UpdateCount = 0;

            ResetWindow();
        }

        protected override void OnUpdate()
        {
            uint simulationFrame =
                m_SimulationSystem.frameIndex;

            ComponentLookup<
                Game.Pathfind.PathInformation>
                pathInformations =
                    SystemAPI.GetComponentLookup<
                        Game.Pathfind.PathInformation>(
                            true);

            ComponentLookup<
                Game.Common.Target>
                targets =
                    SystemAPI.GetComponentLookup<
                        Game.Common.Target>(
                            true);

            ComponentLookup<
                Game.Citizens.TravelPurpose>
                travelPurposes =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.TravelPurpose>(
                            true);

            ComponentLookup<
                Game.Citizens.CurrentBuilding>
                currentBuildings =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.CurrentBuilding>(
                            true);

            ComponentLookup<
                Game.Citizens.Leisure>
                leisures =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.Leisure>(
                            true);

            ComponentLookup<
                Game.Citizens.Citizen>
                citizens =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.Citizen>(
                            true);

            ComponentLookup<
                Game.Companies.ServiceAvailable>
                serviceAvailables =
                    SystemAPI.GetComponentLookup<
                        Game.Companies.ServiceAvailable>(
                            true);

            BufferLookup<
                Game.Citizens.TripNeeded>
                tripBuffers =
                    SystemAPI.GetBufferLookup<
                        Game.Citizens.TripNeeded>(
                            true);

            EntityCommandBuffer commandBuffer =
                m_EndFrameBarrier.CreateCommandBuffer();

            int active = 0;
            int removed = 0;

            foreach ((
                RefRW<CommercialLeisureTripProbe>
                    probeRef,
                Entity citizenEntity) in
                SystemAPI
                    .Query<
                        RefRW<
                            CommercialLeisureTripProbe>>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>()
                    .WithEntityAccess())
            {
                active++;

                CommercialLeisureTripProbe probe =
                    probeRef.ValueRO;

                if ((probe.Flags &
                    kProbeSeen) == 0)
                {
                    probe.Flags |=
                        kProbeSeen;

                    m_WindowCreated++;
                }

                bool tripSeen =
                    HasLeisureTrip(
                        citizenEntity,
                        probe.Provider,
                        ref tripBuffers);

                if (tripSeen &&
                    (probe.Flags &
                        kTripSeen) == 0)
                {
                    probe.Flags |=
                        kTripSeen;

                    m_WindowTripSeen++;
                }

                bool targetSeen =
                    targets.HasComponent(
                        citizenEntity) &&
                    targets[
                        citizenEntity]
                        .m_Target ==
                    probe.Provider;

                if (targetSeen &&
                    (probe.Flags &
                        kTargetSeen) == 0)
                {
                    probe.Flags |=
                        kTargetSeen;

                    m_WindowTargetSeen++;
                }

                bool hasPath =
                    pathInformations.HasComponent(
                        citizenEntity);

                bool pathFailed = false;

                if (hasPath)
                {
                    Game.Pathfind.PathInformation
                        pathInformation =
                            pathInformations[
                                citizenEntity];

                    if ((pathInformation.m_State &
                        Game.Pathfind.PathFlags.Pending) !=
                        0)
                    {
                        if ((probe.Flags &
                            kPathPendingSeen) == 0)
                        {
                            probe.Flags |=
                                kPathPendingSeen;

                            m_WindowPathPending++;
                        }
                    }
                    else if (
                        pathInformation
                            .m_Destination !=
                        Entity.Null)
                    {
                        if ((probe.Flags &
                            kPathResolvedSeen) == 0)
                        {
                            probe.Flags |=
                                kPathResolvedSeen;

                            m_WindowPathResolved++;
                        }
                    }
                    else
                    {
                        pathFailed = true;
                    }
                }

                bool traveling =
                    travelPurposes.HasComponent(
                        citizenEntity) &&
                    travelPurposes[
                        citizenEntity]
                        .m_Purpose ==
                    Game.Citizens.Purpose.Leisure;

                if (traveling &&
                    (probe.Flags &
                        kTravelSeen) == 0)
                {
                    probe.Flags |=
                        kTravelSeen;

                    m_WindowTraveling++;
                }

                bool leisureTargetMatches =
                    leisures.HasComponent(
                        citizenEntity) &&
                    leisures[
                        citizenEntity]
                        .m_TargetAgent ==
                    probe.Provider;

                bool atProvider =
                    currentBuildings.HasComponent(
                        citizenEntity) &&
                    currentBuildings[
                        citizenEntity]
                        .m_CurrentBuilding ==
                    probe.Building;

               bool leisureAdvanced =
                    citizens.HasComponent(
                        citizenEntity) &&
                    citizens[
                        citizenEntity]
                        .m_LeisureCounter >
                    probe.StartLeisureCounter;

                if (atProvider &&
                    leisureTargetMatches)
                {
                    if ((probe.Flags &
                        kArrivalSeen) == 0)
                    {
                        probe.Flags |=
                            kArrivalSeen;

                        probe.ArrivalFrame =
                            simulationFrame;

                        m_WindowArrived++;
                    }

                    // Vanilla SpendLeisureJob increments the cim's LeisureCounter.
                    // This therefore confirms that the targeted cim actually reached
                    // the provider and vanilla processed a real leisure visit.
                    if (leisureAdvanced)
                    {
                        m_WindowSpendConfirmed++;

                        if (serviceAvailables.HasComponent(
                                probe.Provider))
                        {
                            int currentService =
                                serviceAvailables[
                                    probe.Provider]
                                    .m_ServiceAvailable;

                            m_WindowNetServiceDrop +=
                                math.max(
                                    0,
                                    probe.StartService -
                                    currentService);
                        }

                        commandBuffer.RemoveComponent<
                            CommercialLeisureTripProbe>(
                                citizenEntity);

                        removed++;

                        continue;
                    }
                }


                if (pathFailed)
                {
                    m_WindowPathFailed++;

                    commandBuffer.RemoveComponent<
                        CommercialLeisureTripProbe>(
                            citizenEntity);

                    removed++;

                    continue;
                }

                uint age =
                    unchecked(
                        simulationFrame -
                        probe.StartFrame);

                bool hasLeisure =
                    leisures.HasComponent(
                        citizenEntity);

                if (age >=
                        kAbortGraceFrames &&
                    !tripSeen &&
                    !targetSeen &&
                    !hasPath &&
                    !traveling &&
                    !hasLeisure)
                {
                    m_WindowAborted++;

                    commandBuffer.RemoveComponent<
                        CommercialLeisureTripProbe>(
                            citizenEntity);

                    removed++;

                    continue;
                }

                if (age >=
                    kProbeTimeoutFrames)
                {
                    m_WindowTimedOut++;

                    commandBuffer.RemoveComponent<
                        CommercialLeisureTripProbe>(
                            citizenEntity);

                    removed++;

                    continue;
                }

                probeRef.ValueRW =
                    probe;
            }

            m_ActiveProbes =
                math.max(
                    0,
                    active -
                    removed);

            m_UpdateCount++;

            if (m_UpdateCount %
                kLogEveryUpdates == 0)
            {
                LogWindow();
            }
        }

        private static bool HasLeisureTrip(
            Entity citizenEntity,
            Entity provider,
            ref BufferLookup<
                Game.Citizens.TripNeeded>
                    tripBuffers)
        {
            if (!tripBuffers.HasBuffer(
                    citizenEntity))
            {
                return false;
            }

            DynamicBuffer<
                Game.Citizens.TripNeeded>
                    trips =
                        tripBuffers[
                            citizenEntity];

            for (int i = 0;
                i < trips.Length;
                i++)
            {
                if (trips[i].m_Purpose ==
                        Game.Citizens.Purpose.Leisure &&
                    trips[i].m_TargetAgent ==
                        provider)
                {
                    return true;
                }
            }

            return false;
        }

        private void LogWindow()
        {
            LogUtils.Info(
                "[CWD-LEISURE-PROBE] " +
                $"created={m_WindowCreated} " +
                $"tripSeen={m_WindowTripSeen} " +
                $"targetSeen={m_WindowTargetSeen} " +
                $"pathPending={m_WindowPathPending} " +
                $"pathResolved={m_WindowPathResolved} " +
                $"traveling={m_WindowTraveling} " +
                $"arrived={m_WindowArrived} " +
                $"spendConfirmed={m_WindowSpendConfirmed} " +
                $"pathFailed={m_WindowPathFailed} " +
                $"aborted={m_WindowAborted} " +
                $"timedOut={m_WindowTimedOut} " +
                $"active={m_ActiveProbes} " +
                $"netServiceDrop=" +
                $"{m_WindowNetServiceDrop}");

            ResetWindow();
        }

        private void ResetWindow()
        {
            m_WindowCreated = 0;
            m_WindowTripSeen = 0;
            m_WindowTargetSeen = 0;
            m_WindowPathPending = 0;
            m_WindowPathResolved = 0;
            m_WindowTraveling = 0;
            m_WindowArrived = 0;
            m_WindowSpendConfirmed = 0;

            m_WindowPathFailed = 0;
            m_WindowAborted = 0;
            m_WindowTimedOut = 0;

            m_WindowNetServiceDrop = 0;

            m_ActiveProbes = 0;
        }
    }
}

#endif
