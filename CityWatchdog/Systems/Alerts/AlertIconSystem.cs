// <copyright file="AlertIconSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/AlertIconSystem.cs
// Purpose: Applies City Watchdog notification icon settings to vanilla alert prefabs.

namespace CityWatchdog.Systems
{
    using System.Collections.Generic;
    using System.Text;
    using Colossal.Serialization.Entities;
    using Game.Common;
    using Game.Notifications;
    using Game.Prefabs;
    using Unity.Collections;
    using Unity.Entities;

    public partial class AlertIconSystem : GameSystemBaseExtension {
        private StringBuilder m_LogBuilder = null!;
        private EntityQuery m_IconQuery;
        private EntityQuery m_WaterPipeParameterQuery;
        private PrefabSystem m_PrefabSystem = null!;
        private EntityQuery m_NotificationIconDisplayDataQuery;
        private EntityQuery m_ElectricParameterQuery;
        private EntityQuery m_BuildingConfigurationDataQuery;
        private EntityQuery m_TrafficConfigurationDataQuery;
        private EntityQuery m_CompanyNotificationParameterQuery;
        private EntityQuery m_WorkProviderNotificationParameterQuery;
        private EntityQuery m_DisasterNotificationParameterQuery;
        private EntityQuery m_FireNotificationParameterQuery;
        private EntityQuery m_GarbageNotificationParameterQuery;
        private EntityQuery m_HealthcareNotificationParameterQuery;
        private EntityQuery m_PoliceNotificationParameterQuery;
        private EntityQuery m_PollutionNotificationParameterQuery;
        private EntityQuery m_ResourceConsumerNotificationParameterQuery;
        private EntityQuery m_ResourceConnectionNotificationParameterQuery;
        private EntityQuery m_RouteNotificationParameterQuery;
        private EntityQuery m_TransportLineNotificationParameterQuery;

        protected override void OnGameLoaded(Context serializationContext) {
            base.OnGameLoaded(serializationContext);

            // Entity values are recycled across city loads, so last city's prefab strings must not
            // survive into this one.
            m_NotificationPrefabStrings.Clear();

            SetElectricityNotifications();
            SetWaterPipeNotifications();
            SetBuildingNotifications();
            SetTrafficNotifications();
            SetCompanyNotifications();
            SetWorkProviderNotifications();
            SetDisasterNotifications();
            SetFireNotifications();
            SetGarbageNotifications();
            SetHealthcareNotifications();
            SetPoliceNotifications();
            SetPollutionNotifications();
            SetResourceConsumerNotifications();
            SetResourceConnectionNotifications();
            SetRouteNotifications();
            SetTransportLineNotifications();
    #if DEBUG
            Debug();
    #endif
        }

        protected override void OnCreate() {
            base.OnCreate();
            m_LogBuilder = new();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            m_IconQuery = GetEntityQuery(new ComponentType[] {
                ComponentType.ReadOnly<Icon>(),
                ComponentType.ReadOnly<PrefabRef>(),
                ComponentType.Exclude<Deleted>()
            });

            m_NotificationIconDisplayDataQuery = GetEntityQuery(new ComponentType[] {
                ComponentType.ReadOnly<NotificationIconDisplayData>(),
            });

            m_ElectricParameterQuery = GetEntityQuery(new ComponentType[] {
                ComponentType.ReadOnly<ElectricityParameterData>()
            });
            m_WaterPipeParameterQuery = GetEntityQuery(new ComponentType[] {
                ComponentType.ReadOnly<WaterPipeParameterData>()
            });
            m_BuildingConfigurationDataQuery = GetEntityQuery(new ComponentType[] {
                ComponentType.ReadOnly<BuildingConfigurationData>()
            });
            m_TrafficConfigurationDataQuery = GetEntityQuery(new ComponentType[] {
                ComponentType.ReadOnly<TrafficConfigurationData>()
            });
            m_CompanyNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<CompanyNotificationParameterData>());
            m_WorkProviderNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<WorkProviderParameterData>());
            m_DisasterNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<DisasterConfigurationData>());
            m_FireNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<FireConfigurationData>());
            m_GarbageNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<GarbageParameterData>());
            m_HealthcareNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<HealthcareParameterData>());
            m_PoliceNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<PoliceConfigurationData>());
            m_PollutionNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<PollutionParameterData>());
            m_ResourceConsumerNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<ResourceConsumerData>());
            m_ResourceConnectionNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<ResourceConnectionData>());
            m_RouteNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<RouteConfigurationData>());
            m_TransportLineNotificationParameterQuery = GetEntityQuery(ComponentType.ReadOnly<TransportLineData>());
            RequireForUpdate(m_ElectricParameterQuery);
            RequireForUpdate(m_WaterPipeParameterQuery);
            RequireForUpdate(m_BuildingConfigurationDataQuery);
            RequireForUpdate(m_TrafficConfigurationDataQuery);
            RequireForUpdate(m_CompanyNotificationParameterQuery);
            RequireForUpdate(m_WorkProviderNotificationParameterQuery);
            RequireForUpdate(m_DisasterNotificationParameterQuery);
            RequireForUpdate(m_FireNotificationParameterQuery);
            RequireForUpdate(m_GarbageNotificationParameterQuery);
            RequireForUpdate(m_HealthcareNotificationParameterQuery);
            RequireForUpdate(m_PoliceNotificationParameterQuery);
            RequireForUpdate(m_PollutionNotificationParameterQuery);
            RequireForUpdate(m_ResourceConsumerNotificationParameterQuery);
            RequireForUpdate(m_RouteNotificationParameterQuery);
            RequireForUpdate(m_TransportLineNotificationParameterQuery);
        }

        private readonly Dictionary<Entity, int> m_EntityDictionary = new();

        public void Refresh() {
            m_EntityDictionary.Clear();
            NativeArray<ArchetypeChunk> nativeArray = m_IconQuery.ToArchetypeChunkArray(Allocator.TempJob);
            ComponentTypeHandle<PrefabRef> prefabRefTypeHandle = GetComponentTypeHandle<PrefabRef>();
            for (int i = 0; i < nativeArray.Length; i++) {
                NativeArray<PrefabRef> nativeArray2 = nativeArray[i].GetNativeArray(ref prefabRefTypeHandle);
                for (int j = 0; j < nativeArray2.Length; j++) {
                    Entity prefab = nativeArray2[j].m_Prefab;
                    if (m_EntityDictionary.TryGetValue(prefab, out int num)) {
                        m_EntityDictionary[prefab] = num + 1;
                    }
                    else {
                        m_EntityDictionary.Add(prefab, 1);
                    }
                }
            }

            nativeArray.Dispose();
        }

        public void EnableNotification(Entity entity, bool enabled) {
            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(entity, enabled);
            RefreshIcon();
        }

        public void SetAllNotifications(bool enabled)
        {
            SetAllNotificationSettings(enabled);

            SetElectricityNotifications(false);
            SetWaterPipeNotifications(false);
            SetBuildingNotifications(false);
            SetTrafficNotifications(false);
            SetCompanyNotifications(false);
            SetWorkProviderNotifications(false);
            SetDisasterNotifications(false);
            SetFireNotifications(false);
            SetGarbageNotifications(false);
            SetHealthcareNotifications(false);
            SetPoliceNotifications(false);
            SetPollutionNotifications(false);
            SetResourceConsumerNotifications(false);
            SetResourceConnectionNotifications(false);
            SetRouteNotifications(false);
            SetTransportLineNotifications(false);

            RefreshIcon();
        }

        // Apply the CURRENT per-notification settings to the map icons. Unlike SetAllNotifications this
        // does NOT force one value — each notification keeps its own saved state — so it is the apply
        // path after a preset LOAD, where slots differ per notification. Each category applier reads
        // CwdSettings.Instance.Notification and skips its own refresh; one RefreshIcon runs at the end.
        public void ApplyNotificationSettings()
        {
            SetElectricityNotifications(false);
            SetWaterPipeNotifications(false);
            SetBuildingNotifications(false);
            SetTrafficNotifications(false);
            SetCompanyNotifications(false);
            SetWorkProviderNotifications(false);
            SetDisasterNotifications(false);
            SetFireNotifications(false);
            SetGarbageNotifications(false);
            SetHealthcareNotifications(false);
            SetPoliceNotifications(false);
            SetPollutionNotifications(false);
            SetResourceConsumerNotifications(false);
            SetResourceConnectionNotifications(false);
            SetRouteNotifications(false);
            SetTransportLineNotifications(false);

            RefreshIcon();
        }

        private static void SetAllNotificationSettings(bool enabled)
        {
            CwdSettings.NotificationSetting notification = CwdSettings.Instance.Notification;

            notification.ElectricityElectricityNotification = enabled;
            notification.ElectricityBottleneckNotification = enabled;
            notification.ElectricityBuildingBottleneckNotification = enabled;
            notification.ElectricityNotEnoughProductionNotification = enabled;
            notification.ElectricityTransformerNotification = enabled;
            notification.ElectricityNotEnoughConnectedNotification = enabled;
            notification.ElectricityBatteryEmptyNotification = enabled;
            notification.ElectricityLowVoltageNotConnected = enabled;
            notification.ElectricityHighVoltageNotConnected = enabled;

            notification.WaterPipeWaterNotification = enabled;
            notification.WaterPipeDirtyWaterNotification = enabled;
            notification.WaterPipeSewageNotification = enabled;
            notification.WaterPipeWaterPipeNotConnectedNotification = enabled;
            notification.WaterPipeSewagePipeNotConnectedNotification = enabled;
            notification.WaterPipeNotEnoughWaterCapacityNotification = enabled;
            notification.WaterPipeNotEnoughSewageCapacityNotification = enabled;
            notification.WaterPipeNotEnoughGroundwaterNotification = enabled;
            notification.WaterPipeNotEnoughSurfaceWaterNotification = enabled;
            notification.WaterPipeDirtyWaterPumpNotification = enabled;

            notification.BuildingAbandonedCollapsedNotification = enabled;
            notification.BuildingAbandonedNotification = enabled;
            notification.BuildingCondemnedNotification = enabled;
            notification.BuildingTurnedOffNotification = enabled;
            notification.BuildingHighRentNotification = enabled;

            notification.TrafficBottleneckNotification = enabled;
            notification.TrafficDeadEndNotification = enabled;
            notification.TrafficRoadConnectionNotification = enabled;
            notification.TrafficTrackConnectionNotification = enabled;
            notification.TrafficCarConnectionNotification = enabled;
            notification.TrafficShipConnectionNotification = enabled;
            notification.TrafficTrainConnectionNotification = enabled;
            notification.TrafficPedestrianConnectionNotification = enabled;
            notification.TrafficBicycleConnectionNotification = enabled;

            notification.CompanyNoInputsNotification = enabled;
            notification.CompanyNoCustomersNotification = enabled;

            notification.WorkProviderUneducatedNotification = enabled;
            notification.WorkProviderEducatedNotification = enabled;

            notification.DisasterWeatherDamageNotification = enabled;
            notification.DisasterWeatherDestroyedNotification = enabled;
            notification.DisasterWaterDamageNotification = enabled;
            notification.DisasterWaterDestroyedNotification = enabled;
            notification.DisasterDestroyedNotification = enabled;

            notification.FireFireNotification = enabled;
            notification.FireBurnedDownNotification = enabled;

            notification.GarbageGarbageNotification = enabled;
            notification.GarbageFacilityFullNotification = enabled;

            notification.HealthcareAmbulanceNotification = enabled;
            notification.HealthcareHearseNotification = enabled;
            notification.HealthcareFacilityFullNotification = enabled;

            notification.PoliceTrafficAccidentNotification = enabled;
            notification.PoliceCrimeSceneNotification = enabled;

            notification.PollutionAirPollutionNotification = enabled;
            notification.PollutionNoisePollutionNotification = enabled;
            notification.PollutionGroundPollutionNotification = enabled;

            notification.ResourceConsumerNoResourceNotification = enabled;
            notification.ResourceConsumerNoFuelNotification = enabled;
            notification.ResourceConnectionWarningNotification = enabled;
            notification.ResourceConnectionOilPipeNotConnectedNotification = enabled;
            notification.ResourceConnectionFishingPierNotConnectedNotification = enabled;
            notification.RoutePathfindNotification = enabled;
            notification.RouteGateBypassNotification = enabled;
            notification.TransportLineVehicleNotification = enabled;
        }


        public void RefreshIcon() => World.GetOrCreateSystemManaged<IconClusterSystem>().RecalculateClusters();

    }
}
