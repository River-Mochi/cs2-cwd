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

            public int ExcessService;
            public int FreeRooms;

            public bool AssignedThisPass;
        }

        private const float kTargetServiceRatio =
            0.85f;

        // One new real tourist household per stressed hotel per full
        // LodgingProvider pass. No extra path request is created.
        private const int kMaxAssignmentsPerPass =
            4;

        private readonly List<HotelPressure>
            m_Hotels = new();

        public override int GetUpdateInterval(
            SystemUpdatePhase phase)
        {
            // Each LodgingProvider is processed 32 times per game day.
            // Run once per complete provider pass rather than every frame.
            return 262144 /
                LodgingProviderSystem.kUpdatesPerDay;
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

            BufferLookup<
                Game.Economy.Resources>
                resources =
                    SystemAPI.GetBufferLookup<
                        Game.Economy.Resources>(
                            true);

            m_Hotels.Clear();

            int hotelCount = 0;
            int stressedHotels = 0;

            foreach ((
                RefRO<Game.Companies.LodgingProvider>
                    lodgingRef,
                RefRO<Game.Companies.ServiceAvailable>
                    serviceRef,
                RefRO<PrefabRef>
                    prefabRef,
                Entity hotelEntity) in
                SystemAPI
                    .Query<
                        RefRO<
                            Game.Companies.LodgingProvider>,
                        RefRO<
                            Game.Companies.ServiceAvailable>,
                        RefRO<
                            PrefabRef>>()
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

                        ExcessService =
                            excessService,

                        FreeRooms =
                            lodgingRef.ValueRO
                                .m_FreeRooms,

                        AssignedThisPass =
                            false,
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

            int idleUnassigned = 0;
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
                        Game.Common.Deleted,
                        Game.Tools.Temp>()

                    .WithEntityAccess())
            {
                if (assignments >=
                    kMaxAssignmentsPerPass)
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

                idleUnassigned++;

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
                    pressure.AssignedThisPass =
                        true;

                    m_Hotels[
                        hotelIndex] =
                            pressure;

                    continue;
                }

                Game.Companies.LodgingProvider
                    lodging =
                        lodgingProviders[
                            pressure.Hotel];

                if (lodging.m_FreeRooms <= 0)
                {
                    pressure.AssignedThisPass =
                        true;

                    m_Hotels[
                        hotelIndex] =
                            pressure;

                    continue;
                }

                // Mirror vanilla HotelReserveJob's real reservation state.
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

                // Important:
                // Do NOT create a Target.
                // Do NOT create a PathInformation.
                // Do NOT remove LodgingSeeker.
                //
                // Vanilla tourism remains free to choose the household's
                // next attraction/activity. We only reserve its hotel room.

                pressure.FreeRooms =
                    lodging.m_FreeRooms;

                pressure.AssignedThisPass =
                    true;

                m_Hotels[
                    hotelIndex] =
                        pressure;

                assignments++;
            }

            LogUtils.Info(
                "[CWD-HOTEL-BALANCE] " +
                $"hotels={hotelCount} " +
                $"stressed={stressedHotels} " +
                $"idleUnassigned={idleUnassigned} " +
                $"assigned={assignments}");
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

                if (hotel.AssignedThisPass ||
                    hotel.FreeRooms <= 0 ||
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
