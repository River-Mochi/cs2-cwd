// <copyright file="AlertIconSystem.Counts.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/AlertIconSystem.Counts.cs
// Purpose: Counts active vanilla notification icon entities for the expanded CWD rows.

namespace CityWatchdog.Systems
{
    using System;
    using System.Collections.Generic;
    using Game.Prefabs;
    using Unity.Collections;
    using Unity.Entities;

    public partial class AlertIconSystem
    {
        public const int NotificationCountLength = 63;
        private readonly Dictionary<int, int> m_NextNotificationEntityOffsets = new();

        public int[] GetNotificationCounts()
        {
            BuildActiveIconCounts();

            int[] counts = new int[NotificationCountLength];
            int index = 0;

            if (m_ElectricParameterQuery.TryGetSingleton(out ElectricityParameterData electricity))
            {
                counts[index++] = Count(electricity.m_ElectricityNotificationPrefab);
                counts[index++] = Count(electricity.m_BottleneckNotificationPrefab);
                counts[index++] = Count(electricity.m_BuildingBottleneckNotificationPrefab);
                counts[index++] = Count(electricity.m_NotEnoughProductionNotificationPrefab);
                counts[index++] = Count(electricity.m_TransformerNotificationPrefab);
                counts[index++] = Count(electricity.m_NotEnoughConnectedNotificationPrefab);
                counts[index++] = Count(electricity.m_BatteryEmptyNotificationPrefab);
                counts[index++] = Count(electricity.m_LowVoltageNotConnectedPrefab);
                counts[index++] = Count(electricity.m_HighVoltageNotConnectedPrefab);
            }
            else
            {
                index += 9;
            }

            if (m_WaterPipeParameterQuery.TryGetSingleton(out WaterPipeParameterData water))
            {
                counts[index++] = Count(water.m_WaterNotification);
                counts[index++] = Count(water.m_DirtyWaterNotification);
                counts[index++] = Count(water.m_SewageNotification);
                counts[index++] = Count(water.m_WaterPipeNotConnectedNotification);
                counts[index++] = Count(water.m_SewagePipeNotConnectedNotification);
                counts[index++] = Count(water.m_NotEnoughWaterCapacityNotification);
                counts[index++] = Count(water.m_NotEnoughSewageCapacityNotification);
                counts[index++] = Count(water.m_NotEnoughGroundwaterNotification);
                counts[index++] = Count(water.m_NotEnoughSurfaceWaterNotification);
                counts[index++] = Count(water.m_DirtyWaterPumpNotification);
            }
            else
            {
                index += 10;
            }

            if (m_BuildingConfigurationDataQuery.TryGetSingleton(out BuildingConfigurationData building))
            {
                counts[index++] = Count(building.m_AbandonedCollapsedNotification);
                counts[index++] = Count(building.m_AbandonedNotification);
                counts[index++] = Count(building.m_CondemnedNotification);
                counts[index++] = Count(building.m_TurnedOffNotification);
                counts[index++] = Count(building.m_HighRentNotification);
                counts[index++] = Count(building.m_LevelingBuildingNotificationPrefab);
            }
            else
            {
                index += 6;
            }

            if (m_TrafficConfigurationDataQuery.TryGetSingleton(out TrafficConfigurationData traffic))
            {
                counts[index++] = Count(traffic.m_BottleneckNotification);
                counts[index++] = Count(traffic.m_DeadEndNotification);
                counts[index++] = Count(traffic.m_RoadConnectionNotification);
                counts[index++] = Count(traffic.m_TrackConnectionNotification);
                counts[index++] = Count(traffic.m_CarConnectionNotification);
                counts[index++] = Count(traffic.m_ShipConnectionNotification);
                counts[index++] = Count(traffic.m_TrainConnectionNotification);
                counts[index++] = Count(traffic.m_PedestrianConnectionNotification);
                counts[index++] = Count(traffic.m_BicycleConnectionNotification);
            }
            else
            {
                index += 9;
            }

            if (m_CompanyNotificationParameterQuery.TryGetSingleton(out CompanyNotificationParameterData company))
            {
                counts[index++] = Count(company.m_NoInputsNotificationPrefab);
                counts[index++] = Count(company.m_NoCustomersNotificationPrefab);
            }
            else
            {
                index += 2;
            }

            if (m_WorkProviderNotificationParameterQuery.TryGetSingleton(out WorkProviderParameterData workProvider))
            {
                counts[index++] = Count(workProvider.m_UneducatedNotificationPrefab);
                counts[index++] = Count(workProvider.m_EducatedNotificationPrefab);
            }
            else
            {
                index += 2;
            }

            if (m_DisasterNotificationParameterQuery.TryGetSingleton(out DisasterConfigurationData disaster))
            {
                counts[index++] = Count(disaster.m_WeatherDamageNotificationPrefab);
                counts[index++] = Count(disaster.m_WeatherDestroyedNotificationPrefab);
                counts[index++] = Count(disaster.m_WaterDamageNotificationPrefab);
                counts[index++] = Count(disaster.m_WaterDestroyedNotificationPrefab);
                counts[index++] = Count(disaster.m_DestroyedNotificationPrefab);
            }
            else
            {
                index += 5;
            }

            if (m_FireNotificationParameterQuery.TryGetSingleton(out FireConfigurationData fire))
            {
                counts[index++] = Count(fire.m_FireNotificationPrefab);
                counts[index++] = Count(fire.m_BurnedDownNotificationPrefab);
            }
            else
            {
                index += 2;
            }

            if (m_GarbageNotificationParameterQuery.TryGetSingleton(out GarbageParameterData garbage))
            {
                counts[index++] = Count(garbage.m_GarbageNotificationPrefab);
                counts[index++] = Count(garbage.m_FacilityFullNotificationPrefab);
            }
            else
            {
                index += 2;
            }

            if (m_HealthcareNotificationParameterQuery.TryGetSingleton(out HealthcareParameterData healthcare))
            {
                counts[index++] = Count(healthcare.m_AmbulanceNotificationPrefab);
                counts[index++] = Count(healthcare.m_HearseNotificationPrefab);
                counts[index++] = Count(healthcare.m_FacilityFullNotificationPrefab);
            }
            else
            {
                index += 3;
            }

            if (m_PoliceNotificationParameterQuery.TryGetSingleton(out PoliceConfigurationData police))
            {
                counts[index++] = Count(police.m_TrafficAccidentNotificationPrefab);
                counts[index++] = Count(police.m_CrimeSceneNotificationPrefab);
            }
            else
            {
                index += 2;
            }

            if (m_PollutionNotificationParameterQuery.TryGetSingleton(out PollutionParameterData pollution))
            {
                counts[index++] = Count(pollution.m_AirPollutionNotification);
                counts[index++] = Count(pollution.m_NoisePollutionNotification);
                counts[index++] = Count(pollution.m_GroundPollutionNotification);
            }
            else
            {
                index += 3;
            }

            CountResourceConsumerNotifications(out int lowSuppliesCount, out int noFuelCount);
            counts[index++] = lowSuppliesCount;
            counts[index++] = noFuelCount;

            CountResourceConnectionNotifications(
                out int oilPipeCount,
                out int fishingPierCount,
                out int otherConnectionCount);
            counts[index++] = oilPipeCount;
            counts[index++] = fishingPierCount;
            counts[index++] = otherConnectionCount;

            if (m_RouteNotificationParameterQuery.TryGetSingleton(out RouteConfigurationData route))
            {
                counts[index++] = Count(route.m_PathfindNotification);
                counts[index++] = Count(route.m_GateBypassNotification);
            }
            else
            {
                index += 2;
            }

            if (m_TransportLineNotificationParameterQuery.TryGetSingleton(out TransportLineData transport))
            {
                counts[index++] = Count(transport.m_VehicleNotification);
            }
            else
            {
                index++;
            }

#if DEBUG
            if (index != NotificationCountLength)
            {
                CS2Shared.RiverMochi.LogUtils.Debug(() => $"Notification count mapping length mismatch: expected={NotificationCountLength}, actual={index}.");
            }
#endif

            return counts;
        }

        private void BuildActiveIconCounts()
        {
            m_EntityDictionary.Clear();

            ComponentTypeHandle<PrefabRef> prefabRefTypeHandle = GetComponentTypeHandle<PrefabRef>(true);
            using NativeArray<ArchetypeChunk> chunks = m_IconQuery.ToArchetypeChunkArray(Allocator.TempJob);

            // ToArchetypeChunkArray syncs only change-filter and enableable types — it cannot know
            // which components we read back out of the chunks, so PrefabRef's write dependency is
            // still live here. ToComponentDataArray<T> completes T's itself, which is why the
            // resource-consumer/connection helpers below do not need this.
            CompleteDependency();

            for (int i = 0; i < chunks.Length; i++)
            {
                NativeArray<PrefabRef> prefabRefs = chunks[i].GetNativeArray(ref prefabRefTypeHandle);
                for (int j = 0; j < prefabRefs.Length; j++)
                {
                    Entity prefab = prefabRefs[j].m_Prefab;
                    m_EntityDictionary.TryGetValue(prefab, out int count);
                    m_EntityDictionary[prefab] = count + 1;
                }
            }
        }

        private int Count(Entity prefab)
        {
            return prefab != Entity.Null && m_EntityDictionary.TryGetValue(prefab, out int count)
                ? count
                : 0;
        }

        public bool TryGetNextNotificationEntity(int index, out Entity entity)
        {
            entity = Entity.Null;

            if (!TryGetNotificationPrefabMatcher(index, out Func<Entity, bool> matcher))
            {
                return false;
            }

            m_NextNotificationEntityOffsets.TryGetValue(index, out int nextOffset);
            Entity firstMatch = Entity.Null;
            int matchIndex = 0;
            ComponentTypeHandle<PrefabRef> prefabRefTypeHandle = GetComponentTypeHandle<PrefabRef>(true);
            EntityTypeHandle entityTypeHandle = GetEntityTypeHandle();
            using NativeArray<ArchetypeChunk> chunks = m_IconQuery.ToArchetypeChunkArray(Allocator.TempJob);

            // Same reason as BuildActiveIconCounts: a chunk walk completes no read dependency for us.
            CompleteDependency();

            for (int i = 0; i < chunks.Length; i++)
            {
                NativeArray<PrefabRef> prefabRefs = chunks[i].GetNativeArray(ref prefabRefTypeHandle);
                NativeArray<Entity> entities = chunks[i].GetNativeArray(entityTypeHandle);
                for (int j = 0; j < prefabRefs.Length; j++)
                {
                    if (!matcher(prefabRefs[j].m_Prefab))
                    {
                        continue;
                    }

                    Entity candidate = entities[j];
                    if (firstMatch == Entity.Null)
                    {
                        firstMatch = candidate;
                    }

                    if (matchIndex == nextOffset)
                    {
                        entity = candidate;
                        m_NextNotificationEntityOffsets[index] = nextOffset + 1;
                        return true;
                    }

                    matchIndex++;
                }
            }

            if (firstMatch == Entity.Null)
            {
                m_NextNotificationEntityOffsets.Remove(index);
                return false;
            }

            entity = firstMatch;
            m_NextNotificationEntityOffsets[index] = 1;
            return true;
        }

        private bool TryGetNotificationPrefabMatcher(int index, out Func<Entity, bool> matcher)
        {
            matcher = _ => false;

            if (index >= 0 && index <= 8 && m_ElectricParameterQuery.TryGetSingleton(out ElectricityParameterData electricity))
            {
                Entity prefab = index switch
                {
                    0 => electricity.m_ElectricityNotificationPrefab,
                    1 => electricity.m_BottleneckNotificationPrefab,
                    2 => electricity.m_BuildingBottleneckNotificationPrefab,
                    3 => electricity.m_NotEnoughProductionNotificationPrefab,
                    4 => electricity.m_TransformerNotificationPrefab,
                    5 => electricity.m_NotEnoughConnectedNotificationPrefab,
                    6 => electricity.m_BatteryEmptyNotificationPrefab,
                    7 => electricity.m_LowVoltageNotConnectedPrefab,
                    _ => electricity.m_HighVoltageNotConnectedPrefab,
                };
                matcher = PrefabMatcher(prefab);
                return true;
            }

            if (index >= 9 && index <= 18 && m_WaterPipeParameterQuery.TryGetSingleton(out WaterPipeParameterData water))
            {
                Entity prefab = index switch
                {
                    9 => water.m_WaterNotification,
                    10 => water.m_DirtyWaterNotification,
                    11 => water.m_SewageNotification,
                    12 => water.m_WaterPipeNotConnectedNotification,
                    13 => water.m_SewagePipeNotConnectedNotification,
                    14 => water.m_NotEnoughWaterCapacityNotification,
                    15 => water.m_NotEnoughSewageCapacityNotification,
                    16 => water.m_NotEnoughGroundwaterNotification,
                    17 => water.m_NotEnoughSurfaceWaterNotification,
                    _ => water.m_DirtyWaterPumpNotification,
                };
                matcher = PrefabMatcher(prefab);
                return true;
            }

            if (index >= 19 && index <= 24 && m_BuildingConfigurationDataQuery.TryGetSingleton(out BuildingConfigurationData building))
            {
                Entity prefab = index switch
                {
                    19 => building.m_AbandonedCollapsedNotification,
                    20 => building.m_AbandonedNotification,
                    21 => building.m_CondemnedNotification,
                    22 => building.m_TurnedOffNotification,
                    23 => building.m_HighRentNotification,
                    _ => building.m_LevelingBuildingNotificationPrefab,
                };
                matcher = PrefabMatcher(prefab);
                return true;
            }

            if (index >= 25 && index <= 33 && m_TrafficConfigurationDataQuery.TryGetSingleton(out TrafficConfigurationData traffic))
            {
                Entity prefab = index switch
                {
                    25 => traffic.m_BottleneckNotification,
                    26 => traffic.m_DeadEndNotification,
                    27 => traffic.m_RoadConnectionNotification,
                    28 => traffic.m_TrackConnectionNotification,
                    29 => traffic.m_CarConnectionNotification,
                    30 => traffic.m_ShipConnectionNotification,
                    31 => traffic.m_TrainConnectionNotification,
                    32 => traffic.m_PedestrianConnectionNotification,
                    _ => traffic.m_BicycleConnectionNotification,
                };
                matcher = PrefabMatcher(prefab);
                return true;
            }

            if (index >= 34 && index <= 35 && m_CompanyNotificationParameterQuery.TryGetSingleton(out CompanyNotificationParameterData company))
            {
                matcher = PrefabMatcher(index == 34
                    ? company.m_NoInputsNotificationPrefab
                    : company.m_NoCustomersNotificationPrefab);
                return true;
            }

            if (index >= 36 && index <= 37 && m_WorkProviderNotificationParameterQuery.TryGetSingleton(out WorkProviderParameterData workProvider))
            {
                matcher = PrefabMatcher(index == 36
                    ? workProvider.m_UneducatedNotificationPrefab
                    : workProvider.m_EducatedNotificationPrefab);
                return true;
            }

            if (index >= 38 && index <= 42 && m_DisasterNotificationParameterQuery.TryGetSingleton(out DisasterConfigurationData disaster))
            {
                Entity prefab = index switch
                {
                    38 => disaster.m_WeatherDamageNotificationPrefab,
                    39 => disaster.m_WeatherDestroyedNotificationPrefab,
                    40 => disaster.m_WaterDamageNotificationPrefab,
                    41 => disaster.m_WaterDestroyedNotificationPrefab,
                    _ => disaster.m_DestroyedNotificationPrefab,
                };
                matcher = PrefabMatcher(prefab);
                return true;
            }

            if (index >= 43 && index <= 44 && m_FireNotificationParameterQuery.TryGetSingleton(out FireConfigurationData fire))
            {
                matcher = PrefabMatcher(index == 43
                    ? fire.m_FireNotificationPrefab
                    : fire.m_BurnedDownNotificationPrefab);
                return true;
            }

            if (index >= 45 && index <= 46 && m_GarbageNotificationParameterQuery.TryGetSingleton(out GarbageParameterData garbage))
            {
                matcher = PrefabMatcher(index == 45
                    ? garbage.m_GarbageNotificationPrefab
                    : garbage.m_FacilityFullNotificationPrefab);
                return true;
            }

            if (index >= 47 && index <= 49 && m_HealthcareNotificationParameterQuery.TryGetSingleton(out HealthcareParameterData healthcare))
            {
                Entity prefab = index switch
                {
                    47 => healthcare.m_AmbulanceNotificationPrefab,
                    48 => healthcare.m_HearseNotificationPrefab,
                    _ => healthcare.m_FacilityFullNotificationPrefab,
                };
                matcher = PrefabMatcher(prefab);
                return true;
            }

            if (index >= 50 && index <= 51 && m_PoliceNotificationParameterQuery.TryGetSingleton(out PoliceConfigurationData police))
            {
                matcher = PrefabMatcher(index == 50
                    ? police.m_TrafficAccidentNotificationPrefab
                    : police.m_CrimeSceneNotificationPrefab);
                return true;
            }

            if (index >= 52 && index <= 54 && m_PollutionNotificationParameterQuery.TryGetSingleton(out PollutionParameterData pollution))
            {
                Entity prefab = index switch
                {
                    52 => pollution.m_AirPollutionNotification,
                    53 => pollution.m_NoisePollutionNotification,
                    _ => pollution.m_GroundPollutionNotification,
                };
                matcher = PrefabMatcher(prefab);
                return true;
            }

            if (index == 55)
            {
                matcher = IsLowSuppliesNotificationPrefab;
                return true;
            }

            if (index == 56)
            {
                matcher = IsNoFuelNotificationPrefab;
                return true;
            }

            if (index >= 57 && index <= 59)
            {
                matcher = index switch
                {
                    57 => ResourceConnectionPrefabMatcher(IsOilPipeNotConnectedNotification),
                    58 => ResourceConnectionPrefabMatcher(IsFishingPierNotConnectedNotification),
                    _ => ResourceConnectionPrefabMatcher(IsOtherResourceConnectionNotification),
                };
                return true;
            }

            if (index >= 60 && index <= 61 && m_RouteNotificationParameterQuery.TryGetSingleton(out RouteConfigurationData route))
            {
                matcher = PrefabMatcher(index == 60
                    ? route.m_PathfindNotification
                    : route.m_GateBypassNotification);
                return true;
            }

            if (index == 62 && m_TransportLineNotificationParameterQuery.TryGetSingleton(out TransportLineData transport))
            {
                matcher = PrefabMatcher(transport.m_VehicleNotification);
                return true;
            }

            return false;
        }

        private static Func<Entity, bool> PrefabMatcher(Entity prefab)
        {
            return candidate => prefab != Entity.Null && candidate == prefab;
        }

        private Func<Entity, bool> ResourceConnectionPrefabMatcher(Func<ResourceConnectionData, bool> predicate)
        {
            HashSet<Entity> prefabs = new();
            using NativeArray<ResourceConnectionData> connections =
                m_ResourceConnectionNotificationParameterQuery.ToComponentDataArray<ResourceConnectionData>(Allocator.Temp);

            for (int i = 0; i < connections.Length; i++)
            {
                ResourceConnectionData connection = connections[i];
                Entity notificationPrefab = connection.m_ConnectionWarningNotification;
                if (notificationPrefab != Entity.Null && predicate(connection))
                {
                    prefabs.Add(notificationPrefab);
                }
            }

            return candidate => candidate != Entity.Null && prefabs.Contains(candidate);
        }

        private void CountResourceConsumerNotifications(
            out int lowSuppliesCount,
            out int noFuelCount)
        {
            lowSuppliesCount = 0;
            noFuelCount = 0;
            HashSet<Entity> seen = new();
            using NativeArray<ResourceConsumerData> consumers =
                m_ResourceConsumerNotificationParameterQuery.ToComponentDataArray<ResourceConsumerData>(Allocator.Temp);

            for (int i = 0; i < consumers.Length; i++)
            {
                Entity prefab = consumers[i].m_NoResourceNotificationPrefab;
                if (!seen.Add(prefab))
                {
                    continue;
                }

                if (IsLowSuppliesNotificationPrefab(prefab))
                {
                    lowSuppliesCount += Count(prefab);
                }

                if (IsNoFuelNotificationPrefab(prefab))
                {
                    noFuelCount += Count(prefab);
                }
            }
        }

        private void CountResourceConnectionNotifications(
            out int oilPipeCount,
            out int fishingPierCount,
            out int otherConnectionCount)
        {
            oilPipeCount = 0;
            fishingPierCount = 0;
            otherConnectionCount = 0;
            HashSet<Entity> seen = new();
            using NativeArray<ResourceConnectionData> connections =
                m_ResourceConnectionNotificationParameterQuery.ToComponentDataArray<ResourceConnectionData>(Allocator.Temp);

            for (int i = 0; i < connections.Length; i++)
            {
                ResourceConnectionData connection = connections[i];
                Entity prefab = connection.m_ConnectionWarningNotification;
                if (!seen.Add(prefab))
                {
                    continue;
                }

                bool isOilPipe = IsOilPipeNotConnectedNotification(connection);
                bool isFishingPier = IsFishingPierNotConnectedNotification(connection);
                if (isOilPipe)
                {
                    oilPipeCount += Count(prefab);
                }

                if (isFishingPier)
                {
                    fishingPierCount += Count(prefab);
                }

                if (!isOilPipe && !isFishingPier)
                {
                    otherConnectionCount += Count(prefab);
                }
            }
        }
    }
}
