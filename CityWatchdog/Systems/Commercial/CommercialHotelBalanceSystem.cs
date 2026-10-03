// <copyright file="CommercialHotelBalanceSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialHotelBalanceSystem.cs
// Purpose: DEBUG-only balancing of existing tourist demand across hotels.

#if DEBUG

using System.Collections.Generic;

using CS2Shared.RiverMochi;

using Game;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;

using Unity.Entities;
using Unity.Mathematics;

namespace CityWatchdog.Systems
{
    public partial class CommercialHotelBalanceSystem :
        GameSystemBase
    {
        private struct HotelPressure
        {
            public Entity Hotel;
            public Entity Building;

            public int ExcessService;
            public int FreeRooms;
        }

        private struct HotelAssignment
        {
            public Entity Household;
            public Entity Building;
        }

        private const float kTargetServiceRatio =
            0.85f;

        // Intercept at most one real vanilla lodging seeker per update.
        // This keeps the correction gradual while still running often enough
        // to catch seekers before TouristFindTargetSystem consumes them.
        private const int kMaxAssignmentsPerUpdate =
            1;

        private readonly List<HotelPressure>
            m_Hotels = new();

        private readonly List<HotelAssignment>
            m_Assignments = new();

        public override int GetUpdateInterval(
            SystemUpdatePhase phase)
        {
            // Match vanilla TouristFindTargetSystem.
            return 16;
        }

        protected override void OnUpdate()
        {
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
                Game.Buildings.Building>
                buildings =
                    SystemAPI.GetComponentLookup<
                        Game.Buildings.Building>(
                            true);

            BufferLookup<
                Game.Economy.Resources>
                resources =
                    SystemAPI.GetBufferLookup<
                        Game.Economy.Resources>(
                            true);

            m_Hotels.Clear();
            m_Assignments.Clear();

            int hotelCount = 0;
            int stressedHotels = 0;

            foreach ((
                RefRO<Game.Companies.LodgingProvider>
                    lodgingRef,
                RefRO<Game.Companies.ServiceAvailable>
                    serviceRef,
                RefRO<PrefabRef>
                    prefabRef,
                RefRO<Game.Buildings.PropertyRenter>
                    propertyRenterRef,
                Entity hotelEntity) in
                SystemAPI
                    .Query<
                        RefRO<
                            Game.Companies.LodgingProvider>,
                        RefRO<
                            Game.Companies.ServiceAvailable>,
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
                Entity prefab =
                    prefabRef.ValueRO
                        .m_Prefab;

                if (!serviceCompanyDatas.HasComponent(
                        prefab) ||
                    !industrialProcessDatas.HasComponent(
                        prefab))
                {
                    continue;
                }

                IndustrialProcessData process =
                    industrialProcessDatas[
                        prefab];

                if (process.m_Output.m_Resource !=
                    Resource.Lodging)
                {
                    continue;
                }

                hotelCount++;

                Entity building =
                    propertyRenterRef.ValueRO
                        .m_Property;

                if (building == Entity.Null ||
                    !buildings.HasComponent(
                        building))
                {
                    continue;
                }

                Game.Companies.ServiceCompanyData
                    serviceData =
                        serviceCompanyDatas[
                            prefab];

                if (serviceData.m_MaxService <= 0 ||
                    lodgingRef.ValueRO
                        .m_FreeRooms <= 0)
                {
                    continue;
                }

                int stock = 0;

                if (resources.HasBuffer(
                        hotelEntity))
                {
                    stock =
                        EconomyUtils.GetResources(
                            Resource.Lodging,
                            resources[
                                hotelEntity]);
                }

                // Match the stock side of vanilla's NEC notification.
                if (stock <= 200)
                {
                    continue;
                }

                int targetService =
                    (int)math.floor(
                        serviceData.m_MaxService *
                        kTargetServiceRatio);

                int excessService =
                    math.max(
                        0,
                        serviceRef.ValueRO
                            .m_ServiceAvailable -
                        targetService);

                if (excessService <= 0)
                {
                    continue;
                }

                stressedHotels++;

                m_Hotels.Add(
                    new HotelPressure
                    {
                        Hotel =
                            hotelEntity,

                        Building =
                            building,

                        ExcessService =
                            excessService,

                        FreeRooms =
                            lodgingRef.ValueRO
                                .m_FreeRooms,
                    });
            }

            if (m_Hotels.Count == 0)
            {
                return;
            }

            BufferLookup<
                Game.Buildings.Renter>
                renters =
                    SystemAPI.GetBufferLookup<
                        Game.Buildings.Renter>();

            ComponentLookup<
                Game.Companies.LodgingProvider>
                lodgingProviders =
                    SystemAPI.GetComponentLookup<
                        Game.Companies.LodgingProvider>();

            int seekersSeen = 0;
            int assignments = 0;

            foreach ((
                RefRW<Game.Citizens.TouristHousehold>
                    touristRef,
                Entity householdEntity) in
                SystemAPI
                    .Query<
                        RefRW<
                            Game.Citizens.TouristHousehold>>()
                    .WithAll<
                        Game.Citizens.LodgingSeeker>()
                    .WithNone<
                        Game.Common.Target,
                        Game.Agents.MovingAway,
                        Game.Common.Deleted>()
                    .WithNone<
                        Game.Tools.Temp,
                        Game.Pathfind.PathInformation>()
                    .WithEntityAccess())
            {
                seekersSeen++;

                if (assignments >=
                    kMaxAssignmentsPerUpdate)
                {
                    break;
                }

                Game.Citizens.TouristHousehold
                    tourist =
                        touristRef.ValueRO;

                if (tourist.m_Hotel !=
                    Entity.Null)
                {
                    continue;
                }

                int hotelIndex =
                    SelectHotel();

                if (hotelIndex < 0)
                {
                    break;
                }

                HotelPressure pressure =
                    m_Hotels[
                        hotelIndex];

                if (!renters.HasBuffer(
                        pressure.Hotel) ||
                    !lodgingProviders.HasComponent(
                        pressure.Hotel))
                {
                    continue;
                }

                Game.Companies.LodgingProvider
                    lodging =
                        lodgingProviders[
                            pressure.Hotel];

                if (lodging.m_FreeRooms <= 0)
                {
                    continue;
                }

                // Mirror vanilla HotelReserveJob:
                // reserve one real room for this real tourist household.
                renters[
                    pressure.Hotel]
                    .Add(
                        new Game.Buildings.Renter
                        {
                            m_Renter =
                                householdEntity,
                        });

                lodging.m_FreeRooms--;

                lodgingProviders[
                    pressure.Hotel] =
                        lodging;

                tourist.m_Hotel =
                    pressure.Hotel;

                touristRef.ValueRW =
                    tourist;

                m_Assignments.Add(
                    new HotelAssignment
                    {
                        Household =
                            householdEntity,

                        Building =
                            pressure.Building,
                    });

                assignments++;
            }

            // Do the structural changes after the SystemAPI.Query iteration.
            // Vanilla successful reservation removes LodgingSeeker and leaves
            // the household targeted at the selected physical hotel building.
            for (int i = 0;
                i < m_Assignments.Count;
                i++)
            {
                HotelAssignment assignment =
                    m_Assignments[i];

                if (EntityManager.HasComponent<
                        Game.Citizens.LodgingSeeker>(
                        assignment.Household))
                {
                    EntityManager.RemoveComponent<
                        Game.Citizens.LodgingSeeker>(
                            assignment.Household);
                }

                if (!EntityManager.HasComponent<
                        Game.Common.Target>(
                        assignment.Household))
                {
                    EntityManager.AddComponentData(
                        assignment.Household,
                        new Game.Common.Target
                        {
                            m_Target =
                                assignment.Building,
                        });
                }
            }

            if (assignments > 0 ||
                seekersSeen > 0)
            {
                LogUtils.Info(
                    "[CWD-HOTEL-BALANCE] " +
                    $"hotels={hotelCount} " +
                    $"stressed={stressedHotels} " +
                    $"seekersSeen={seekersSeen} " +
                    $"assigned={assignments}");
            }
        }

        private int SelectHotel()
        {
            int selected = -1;
            int bestExcess = 0;

            for (int i = 0;
                i < m_Hotels.Count;
                i++)
            {
                HotelPressure hotel =
                    m_Hotels[i];

                if (hotel.FreeRooms <= 0 ||
                    hotel.ExcessService <=
                        bestExcess)
                {
                    continue;
                }

                bestExcess =
                    hotel.ExcessService;

                selected = i;
            }

            return selected;
        }
    }
}

#endif
