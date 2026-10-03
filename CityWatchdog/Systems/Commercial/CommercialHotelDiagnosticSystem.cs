// <copyright file="CommercialHotelDiagnosticSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialHotelDiagnosticSystem.cs
// Purpose: DEBUG-only diagnostics for hotel occupancy and tourist lodging demand.

#if DEBUG

using System.Globalization;

using CS2Shared.RiverMochi;

using Game;
using Game.Simulation;

using Unity.Entities;

namespace CityWatchdog.Systems
{
    public partial class CommercialHotelDiagnosticSystem :
        GameSystemBase
    {
        private const int kLogEveryUpdates = 128;

        private int m_UpdateCount;

        public override int GetUpdateInterval(
            SystemUpdatePhase phase)
        {
            return 16;
        }

        protected override void OnUpdate()
        {
            m_UpdateCount++;

            if (m_UpdateCount %
                kLogEveryUpdates != 0)
            {
                return;
            }

            BufferLookup<
                Game.Buildings.Renter>
                renters =
                    SystemAPI.GetBufferLookup<
                        Game.Buildings.Renter>(
                            true);

            ComponentLookup<
                Game.Companies.ServiceCompanyData>
                serviceCompanyDatas =
                    SystemAPI.GetComponentLookup<
                        Game.Companies.ServiceCompanyData>(
                            true);

            ComponentLookup<
                Game.Citizens.LodgingSeeker>
                lodgingSeekers =
                    SystemAPI.GetComponentLookup<
                        Game.Citizens.LodgingSeeker>(
                            true);

            ComponentLookup<
                Game.Common.Target>
                targets =
                    SystemAPI.GetComponentLookup<
                        Game.Common.Target>(
                            true);

            ComponentLookup<
                Game.Pathfind.PathInformation>
                pathInformations =
                    SystemAPI.GetComponentLookup<
                        Game.Pathfind.PathInformation>(
                            true);

            ComponentLookup<
                Game.Agents.MovingAway>
                movingAways =
                    SystemAPI.GetComponentLookup<
                        Game.Agents.MovingAway>(
                            true);

            ComponentLookup<
                Game.Buildings.Building>
                buildings =
                    SystemAPI.GetComponentLookup<
                        Game.Buildings.Building>(
                            true);

            int hotelCount = 0;
            int warningCount = 0;

            int occupiedRooms = 0;
            int freeRooms = 0;

            long serviceAvailable = 0;
            long maxService = 0;

            foreach ((
                RefRO<Game.Companies.LodgingProvider>
                    lodgingRef,
                RefRO<Game.Companies.ServiceAvailable>
                    serviceRef,
                RefRO<Game.Companies.CompanyNotifications>
                    notificationsRef,
                RefRO<Game.Prefabs.PrefabRef>
                    prefabRef,
                Entity hotelEntity) in
                SystemAPI
                    .Query<
                        RefRO<
                            Game.Companies.LodgingProvider>,
                        RefRO<
                            Game.Companies.ServiceAvailable>,
                        RefRO<
                            Game.Companies.CompanyNotifications>,
                        RefRO<
                            Game.Prefabs.PrefabRef>>()
                    .WithAll<
                        Game.Companies.CommercialCompany>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>()
                    .WithEntityAccess())
            {
                hotelCount++;

                if (notificationsRef.ValueRO
                        .m_NoCustomersEntity !=
                    Entity.Null)
                {
                    warningCount++;
                }

                int occupied = 0;

                if (renters.HasBuffer(
                        hotelEntity))
                {
                    occupied =
                        renters[
                            hotelEntity].Length;
                }

                occupiedRooms +=
                    occupied;

                freeRooms +=
                    lodgingRef.ValueRO
                        .m_FreeRooms;

                serviceAvailable +=
                    serviceRef.ValueRO
                        .m_ServiceAvailable;

                Entity prefab =
                    prefabRef.ValueRO
                        .m_Prefab;

                if (serviceCompanyDatas.HasComponent(
                        prefab))
                {
                    maxService +=
                        serviceCompanyDatas[
                            prefab]
                            .m_MaxService;
                }
            }

            int touristHouseholds = 0;
            int hotelAssigned = 0;
            int noHotel = 0;

            int noHotelTarget = 0;
            int noHotelValidTarget = 0;
            int noHotelPath = 0;
            int noHotelLodgingSeeker = 0;
            int noHotelMovingAway = 0;
            int noHotelBalanceEligible = 0;
            int noHotelIdle = 0;

            foreach ((
                RefRO<Game.Citizens.TouristHousehold>
                    touristRef,
                Entity householdEntity) in
                SystemAPI
                    .Query<
                        RefRO<
                            Game.Citizens.TouristHousehold>>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>()
                    .WithEntityAccess())
            {
                touristHouseholds++;

                if (touristRef.ValueRO.m_Hotel !=
                    Entity.Null)
                {
                    hotelAssigned++;
                    continue;
                }

                noHotel++;

                bool hasTarget =
                    targets.HasComponent(
                        householdEntity);

                bool hasValidTarget =
                    hasTarget &&
                    targets[
                        householdEntity]
                        .m_Target !=
                    Entity.Null &&
                    buildings.HasComponent(
                        targets[
                            householdEntity]
                            .m_Target);

                bool hasPath =
                    pathInformations.HasComponent(
                        householdEntity);

                bool isLodgingSeeker =
                    lodgingSeekers.HasComponent(
                        householdEntity);

                bool isMovingAway =
                    movingAways.HasComponent(
                        householdEntity);

                if (hasTarget)
                {
                    noHotelTarget++;
                }

                if (hasValidTarget)
                {
                    noHotelValidTarget++;
                }

                if (hasPath)
                {
                    noHotelPath++;
                }

                if (isLodgingSeeker)
                {
                    noHotelLodgingSeeker++;
                }

                if (isMovingAway)
                {
                    noHotelMovingAway++;
                }

                if (isLodgingSeeker &&
                    !hasTarget &&
                    !hasPath &&
                    !isMovingAway)
                {
                    noHotelBalanceEligible++;
                }

                if (!hasTarget &&
                    !hasPath &&
                    !isLodgingSeeker &&
                    !isMovingAway)
                {
                    noHotelIdle++;
                }
            }

            int totalRooms =
                occupiedRooms +
                freeRooms;

            float occupancy =
                totalRooms > 0
                    ? 100f *
                        occupiedRooms /
                        totalRooms
                    : 0f;

            float unusedService =
                maxService > 0
                    ? 100f *
                        serviceAvailable /
                        maxService
                    : 0f;

            LogUtils.Info(
                "[CWD-HOTEL] " +
                $"hotels={hotelCount} " +
                $"warnings={warningCount} " +
                $"roomsOccupied={occupiedRooms} " +
                $"roomsFree={freeRooms} " +
                $"occupancy=" +
                $"{occupancy.ToString("F1", CultureInfo.InvariantCulture)}% " +
                $"unusedService=" +
                $"{unusedService.ToString("F1", CultureInfo.InvariantCulture)}% " +
                $"touristHouseholds={touristHouseholds} " +
                $"hotelAssigned={hotelAssigned} " +
                $"noHotel={noHotel} " +
                $"noHotelTarget={noHotelTarget} " +
                $"noHotelValidTarget={noHotelValidTarget} " +
                $"noHotelPath={noHotelPath} " +
                $"noHotelLodgingSeeker={noHotelLodgingSeeker} " +
                $"noHotelMovingAway={noHotelMovingAway} " +
                $"noHotelBalanceEligible={noHotelBalanceEligible} " +
                $"noHotelIdle={noHotelIdle}");
        }
    }
}

#endif
