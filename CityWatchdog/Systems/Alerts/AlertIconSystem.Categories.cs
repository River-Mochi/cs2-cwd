// <copyright file="AlertIconSystem.Categories.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/AlertIconSystem.Categories.cs
// Purpose: Contains per-category notification alert mapping for City Watchdog.

namespace CityWatchdog.Systems
{
    using System.Collections.Generic;
    using CityWatchdog.Alerts;
    using Game.Economy;
    using Game.Prefabs;
    using Game.UI;
    using Unity.Collections;
    using Unity.Entities;

    public partial class AlertIconSystem
    {
        public void EnableTransportLineNotification(TransportLineNotificationIcon transportLineNotificationIcon, bool value, bool refresh = false) {
            TransportLineData singleton = m_TransportLineNotificationParameterQuery.GetSingleton<TransportLineData>();
            if (transportLineNotificationIcon == TransportLineNotificationIcon.VehicleNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_VehicleNotification, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetTransportLineNotifications(bool refresh = true) {
            EnableTransportLineNotification(TransportLineNotificationIcon.VehicleNotification, CwdSettings.Instance.Notification.TransportLineVehicleNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableRouteNotification(RouteNotificationIcon routeNotificationIcon, bool value, bool refresh = false) {
            RouteConfigurationData singleton = m_RouteNotificationParameterQuery.GetSingleton<RouteConfigurationData>();
            if (routeNotificationIcon == RouteNotificationIcon.PathfindNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_PathfindNotification, value);
            }
            else if (routeNotificationIcon == RouteNotificationIcon.GateBypassNotification) {
                SetNotificationIconDisplayEnabled(singleton.m_GateBypassNotification, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetRouteNotifications(bool refresh = true) {
            EnableRouteNotification(RouteNotificationIcon.PathfindNotification, CwdSettings.Instance.Notification.RoutePathfindNotification);
            EnableRouteNotification(RouteNotificationIcon.GateBypassNotification, CwdSettings.Instance.Notification.RouteGateBypassNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableResourceConsumerNotification(ResourceConsumerNotificationIcon resourceConsumerNotificationIcon, bool value, bool refresh = false) {
            if (resourceConsumerNotificationIcon == ResourceConsumerNotificationIcon.NoResourceNotification) {
                SetResourceConsumerNotifications(value, IsLowSuppliesNotificationPrefab);
            }
            else if (resourceConsumerNotificationIcon == ResourceConsumerNotificationIcon.NoFuelNotification) {
                SetResourceConsumerNotifications(value, IsNoFuelNotificationPrefab);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetResourceConsumerNotifications(bool refresh = true) {
            EnableResourceConsumerNotification(ResourceConsumerNotificationIcon.NoResourceNotification, CwdSettings.Instance.Notification.ResourceConsumerNoResourceNotification);
            EnableResourceConsumerNotification(ResourceConsumerNotificationIcon.NoFuelNotification, CwdSettings.Instance.Notification.ResourceConsumerNoFuelNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableResourceConnectionNotification(ResourceConnectionNotificationIcon resourceConnectionNotificationIcon, bool value, bool refresh = false) {
            if (resourceConnectionNotificationIcon == ResourceConnectionNotificationIcon.ConnectionWarningNotification) {
                SetResourceConnectionNotifications(value, IsOtherResourceConnectionNotification);
            }
            else if (resourceConnectionNotificationIcon == ResourceConnectionNotificationIcon.OilPipeNotConnectedNotification) {
                SetResourceConnectionNotifications(value, IsOilPipeNotConnectedNotification);
            }
            else if (resourceConnectionNotificationIcon == ResourceConnectionNotificationIcon.FishingPierNotConnectedNotification) {
                SetResourceConnectionNotifications(value, IsFishingPierNotConnectedNotification);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetResourceConnectionNotifications(bool refresh = true) {
            EnableResourceConnectionNotification(ResourceConnectionNotificationIcon.ConnectionWarningNotification, CwdSettings.Instance.Notification.ResourceConnectionWarningNotification);
            EnableResourceConnectionNotification(ResourceConnectionNotificationIcon.OilPipeNotConnectedNotification, CwdSettings.Instance.Notification.ResourceConnectionOilPipeNotConnectedNotification);
            EnableResourceConnectionNotification(ResourceConnectionNotificationIcon.FishingPierNotConnectedNotification, CwdSettings.Instance.Notification.ResourceConnectionFishingPierNotConnectedNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnablePollutionNotification(PollutionNotificationIcon pollutionNotificationIcon, bool value, bool refresh = false) {
            PollutionParameterData singleton = m_PollutionNotificationParameterQuery.GetSingleton<PollutionParameterData>();
            if (pollutionNotificationIcon == PollutionNotificationIcon.AirPollutionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_AirPollutionNotification, value);
            }
            else if (pollutionNotificationIcon == PollutionNotificationIcon.NoisePollutionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NoisePollutionNotification, value);
            }
            else if (pollutionNotificationIcon == PollutionNotificationIcon.GroundPollutionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_GroundPollutionNotification, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetPollutionNotifications(bool refresh = true) {
            EnablePollutionNotification(PollutionNotificationIcon.AirPollutionNotification, CwdSettings.Instance.Notification.PollutionAirPollutionNotification);
            EnablePollutionNotification(PollutionNotificationIcon.NoisePollutionNotification, CwdSettings.Instance.Notification.PollutionNoisePollutionNotification);
            EnablePollutionNotification(PollutionNotificationIcon.GroundPollutionNotification, CwdSettings.Instance.Notification.PollutionGroundPollutionNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnablePoliceNotification(PoliceNotificationIcon policeNotificationIcon, bool value, bool refresh = false) {
            PoliceConfigurationData singleton = m_PoliceNotificationParameterQuery.GetSingleton<PoliceConfigurationData>();
            if (policeNotificationIcon == PoliceNotificationIcon.TrafficAccidentNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TrafficAccidentNotificationPrefab, value);
            }
            else if (policeNotificationIcon == PoliceNotificationIcon.CrimeSceneNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_CrimeSceneNotificationPrefab, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetPoliceNotifications(bool refresh = true) {
            EnablePoliceNotification(PoliceNotificationIcon.TrafficAccidentNotification, CwdSettings.Instance.Notification.PoliceTrafficAccidentNotification);
            EnablePoliceNotification(PoliceNotificationIcon.CrimeSceneNotification, CwdSettings.Instance.Notification.PoliceCrimeSceneNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableHealthcareNotification(HealthcareNotificationIcon healthcareNotificationIcon, bool value, bool refresh = false) {
            HealthcareParameterData singleton = m_HealthcareNotificationParameterQuery.GetSingleton<HealthcareParameterData>();
            if (healthcareNotificationIcon == HealthcareNotificationIcon.AmbulanceNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_AmbulanceNotificationPrefab, value);
            }
            else if (healthcareNotificationIcon == HealthcareNotificationIcon.HearseNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_HearseNotificationPrefab, value);
            }
            else if (healthcareNotificationIcon == HealthcareNotificationIcon.FacilityFullNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_FacilityFullNotificationPrefab, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetHealthcareNotifications(bool refresh = true) {
            EnableHealthcareNotification(HealthcareNotificationIcon.AmbulanceNotification, CwdSettings.Instance.Notification.HealthcareAmbulanceNotification);
            EnableHealthcareNotification(HealthcareNotificationIcon.HearseNotification, CwdSettings.Instance.Notification.HealthcareHearseNotification);
            EnableHealthcareNotification(HealthcareNotificationIcon.FacilityFullNotification, CwdSettings.Instance.Notification.HealthcareFacilityFullNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableGarbageNotification(GarbageNotificationIcon garbageNotificationIcon, bool value, bool refresh = false) {
            GarbageParameterData singleton = m_GarbageNotificationParameterQuery.GetSingleton<GarbageParameterData>();
            if (garbageNotificationIcon == GarbageNotificationIcon.GarbageNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_GarbageNotificationPrefab, value);
            }
            else if (garbageNotificationIcon == GarbageNotificationIcon.FacilityFullNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_FacilityFullNotificationPrefab, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetGarbageNotifications(bool refresh = true) {
            EnableGarbageNotification(GarbageNotificationIcon.GarbageNotification, CwdSettings.Instance.Notification.GarbageGarbageNotification);
            EnableGarbageNotification(GarbageNotificationIcon.FacilityFullNotification, CwdSettings.Instance.Notification.GarbageFacilityFullNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableFireNotification(FireNotificationIcon fireNotificationIcon, bool value, bool refresh = false) {
            FireConfigurationData singleton = m_FireNotificationParameterQuery.GetSingleton<FireConfigurationData>();
            if (fireNotificationIcon == FireNotificationIcon.FireNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_FireNotificationPrefab, value);
            }
            else if (fireNotificationIcon == FireNotificationIcon.BurnedDownNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BurnedDownNotificationPrefab, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetFireNotifications(bool refresh = true) {
            EnableFireNotification(FireNotificationIcon.FireNotification, CwdSettings.Instance.Notification.FireFireNotification);
            EnableFireNotification(FireNotificationIcon.BurnedDownNotification, CwdSettings.Instance.Notification.FireBurnedDownNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableDisasterNotification(DisasterNotificationIcon disasterNotificationIcon, bool value, bool refresh = false) {
            DisasterConfigurationData singleton = m_DisasterNotificationParameterQuery.GetSingleton<DisasterConfigurationData>();
            if (disasterNotificationIcon == DisasterNotificationIcon.WeatherDamageNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WeatherDamageNotificationPrefab, value);
            }
            else if (disasterNotificationIcon == DisasterNotificationIcon.WeatherDestroyedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WeatherDestroyedNotificationPrefab, value);
            }
            else if (disasterNotificationIcon == DisasterNotificationIcon.WaterDamageNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WaterDamageNotificationPrefab, value);
            }
            else if (disasterNotificationIcon == DisasterNotificationIcon.WaterDestroyedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WaterDestroyedNotificationPrefab, value);
            }
            else if (disasterNotificationIcon == DisasterNotificationIcon.DestroyedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_DestroyedNotificationPrefab, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetDisasterNotifications(bool refresh = true) {
            EnableDisasterNotification(DisasterNotificationIcon.WeatherDamageNotification, CwdSettings.Instance.Notification.DisasterWeatherDamageNotification);
            EnableDisasterNotification(DisasterNotificationIcon.WeatherDestroyedNotification, CwdSettings.Instance.Notification.DisasterWeatherDestroyedNotification);
            EnableDisasterNotification(DisasterNotificationIcon.WaterDamageNotification, CwdSettings.Instance.Notification.DisasterWaterDamageNotification);
            EnableDisasterNotification(DisasterNotificationIcon.WaterDestroyedNotification, CwdSettings.Instance.Notification.DisasterWaterDestroyedNotification);
            EnableDisasterNotification(DisasterNotificationIcon.DestroyedNotification, CwdSettings.Instance.Notification.DisasterDestroyedNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableWorkProviderNotification(WorkProviderNotificationIcon workProviderNotificationIcon, bool value, bool refresh = false) {
            WorkProviderParameterData singleton = m_WorkProviderNotificationParameterQuery.GetSingleton<WorkProviderParameterData>();
            if (workProviderNotificationIcon == WorkProviderNotificationIcon.UneducatedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_UneducatedNotificationPrefab, value);
            }
            else if (workProviderNotificationIcon == WorkProviderNotificationIcon.EducatedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_EducatedNotificationPrefab, value);
            }
            if (refresh)
                RefreshIcon();
        }

        public void SetWorkProviderNotifications(bool refresh = true) {
            EnableWorkProviderNotification(WorkProviderNotificationIcon.UneducatedNotification, CwdSettings.Instance.Notification.WorkProviderUneducatedNotification);
            EnableWorkProviderNotification(WorkProviderNotificationIcon.EducatedNotification, CwdSettings.Instance.Notification.WorkProviderEducatedNotification);
            if (refresh)
                RefreshIcon();
        }

        public void SetCompanyNotifications(bool refresh = true) {
            EnableCompanyNotification(CompanyNotificationIcon.NoInputsNotification, CwdSettings.Instance.Notification.CompanyNoInputsNotification);
            EnableCompanyNotification(CompanyNotificationIcon.NoCustomersNotification, CwdSettings.Instance.Notification.CompanyNoCustomersNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableCompanyNotification(CompanyNotificationIcon companyNotificationIcon, bool value, bool refresh = false) {
            CompanyNotificationParameterData singleton = m_CompanyNotificationParameterQuery.GetSingleton<CompanyNotificationParameterData>();
            if (companyNotificationIcon == CompanyNotificationIcon.NoInputsNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NoInputsNotificationPrefab, value);
            }
            else if (companyNotificationIcon == CompanyNotificationIcon.NoCustomersNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NoCustomersNotificationPrefab, value);
            }
            if (refresh) {
                RefreshIcon();
            }
        }

        public void SetTrafficNotifications(bool refresh = true) {
            EnableTrafficNotification(TrafficNotificationIcon.BottleneckNotification, CwdSettings.Instance.Notification.TrafficBottleneckNotification);
            EnableTrafficNotification(TrafficNotificationIcon.DeadEndNotification, CwdSettings.Instance.Notification.TrafficDeadEndNotification);
            EnableTrafficNotification(TrafficNotificationIcon.RoadConnectionNotification, CwdSettings.Instance.Notification.TrafficRoadConnectionNotification);
            EnableTrafficNotification(TrafficNotificationIcon.TrackConnectionNotification, CwdSettings.Instance.Notification.TrafficTrackConnectionNotification);
            EnableTrafficNotification(TrafficNotificationIcon.CarConnectionNotification, CwdSettings.Instance.Notification.TrafficCarConnectionNotification);
            EnableTrafficNotification(TrafficNotificationIcon.ShipConnectionNotification, CwdSettings.Instance.Notification.TrafficShipConnectionNotification);
            EnableTrafficNotification(TrafficNotificationIcon.TrainConnectionNotification, CwdSettings.Instance.Notification.TrafficTrainConnectionNotification);
            EnableTrafficNotification(TrafficNotificationIcon.PedestrianConnectionNotification, CwdSettings.Instance.Notification.TrafficPedestrianConnectionNotification);
            EnableTrafficNotification(TrafficNotificationIcon.BicycleConnectionNotification, CwdSettings.Instance.Notification.TrafficBicycleConnectionNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableTrafficNotification(TrafficNotificationIcon trafficNotificationIcon, bool value, bool refresh = false) {
            TrafficConfigurationData singleton = m_TrafficConfigurationDataQuery.GetSingleton<TrafficConfigurationData>();
            if (trafficNotificationIcon == TrafficNotificationIcon.BottleneckNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BottleneckNotification, value);
            }
            else if (trafficNotificationIcon == TrafficNotificationIcon.DeadEndNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_DeadEndNotification, value);
            }
            else if (trafficNotificationIcon == TrafficNotificationIcon.RoadConnectionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_RoadConnectionNotification, value);
            }
            else if (trafficNotificationIcon == TrafficNotificationIcon.TrackConnectionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TrackConnectionNotification, value);
            }
            else if (trafficNotificationIcon == TrafficNotificationIcon.CarConnectionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_CarConnectionNotification, value);
            }
            else if (trafficNotificationIcon == TrafficNotificationIcon.ShipConnectionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_ShipConnectionNotification, value);
            }
            else if (trafficNotificationIcon == TrafficNotificationIcon.TrainConnectionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TrainConnectionNotification, value);
            }
            else if (trafficNotificationIcon == TrafficNotificationIcon.PedestrianConnectionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_PedestrianConnectionNotification, value);
            }
            else if (trafficNotificationIcon == TrafficNotificationIcon.BicycleConnectionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BicycleConnectionNotification, value);
            }

            if (refresh) {
                RefreshIcon();
            }
        }

        private void SetResourceConsumerNotifications(bool value, System.Func<Entity, bool> predicate) {
            NativeArray<ResourceConsumerData> consumers = m_ResourceConsumerNotificationParameterQuery.ToComponentDataArray<ResourceConsumerData>(Allocator.Temp);
            try {
                HashSet<Entity> seen = new();
                for (int i = 0; i < consumers.Length; i++) {
                    Entity notificationPrefab = consumers[i].m_NoResourceNotificationPrefab;
                    if (seen.Add(notificationPrefab) && predicate(notificationPrefab)) {
                        SetNotificationIconDisplayEnabled(notificationPrefab, value);
                    }
                }
            }
            finally {
                consumers.Dispose();
            }
        }

        private bool IsLowSuppliesNotificationPrefab(Entity notificationPrefab) {
            return IsNotificationIcon(notificationPrefab, "NotEnoughIndustrialGoods.svg") ||
                   IsNotificationPrefabName(notificationPrefab, "Supplies");
        }

        private bool IsNoFuelNotificationPrefab(Entity notificationPrefab) {
            return IsNotificationIcon(notificationPrefab, "NoFuel.svg") ||
                   IsNotificationPrefabName(notificationPrefab, "Fuel");
        }

        // A notification prefab is created once per city load and keeps its identity for the rest of the
        // session, so its icon path and name never change. Resolving them costs a PrefabSystem lookup
        // plus ImageSystem.GetIcon, and that ran for every resource prefab on every count scan — the only
        // string work left in that path. Cached per entity, a scan does nothing but trivial compares.
        // Cleared in OnGameLoaded because Entity values are recycled across city loads.
        private readonly Dictionary<Entity, (string IconPath, string Name)> m_NotificationPrefabStrings = new();

        // Only GetPrefab can realistically throw: ImageSystem.GetIcon returns null rather than throwing,
        // so a single try/catch here reproduces the previous per-method behaviour exactly.
        private bool TryGetNotificationPrefabStrings(Entity notificationPrefab, out (string IconPath, string Name) strings) {
            if (notificationPrefab == Entity.Null) {
                strings = default;
                return false;
            }

            if (m_NotificationPrefabStrings.TryGetValue(notificationPrefab, out strings)) {
                return true;
            }

            try {
                NotificationIconPrefab prefab = m_PrefabSystem.GetPrefab<NotificationIconPrefab>(notificationPrefab);
                strings = (ImageSystem.GetIcon(prefab) ?? string.Empty, prefab.name ?? string.Empty);
            }
            catch {
                // Never cache a failure. An entity that cannot be resolved yet has to be retried on the
                // next scan, not poisoned into a permanent negative for the rest of the session.
                strings = default;
                return false;
            }

            m_NotificationPrefabStrings[notificationPrefab] = strings;
            return true;
        }

        private bool IsNotificationIcon(Entity notificationPrefab, string iconName) {
            return TryGetNotificationPrefabStrings(notificationPrefab, out (string IconPath, string Name) strings) &&
                   strings.IconPath.EndsWith(iconName, System.StringComparison.OrdinalIgnoreCase);
        }

        private bool IsNotificationPrefabName(Entity notificationPrefab, string text) {
            return TryGetNotificationPrefabStrings(notificationPrefab, out (string IconPath, string Name) strings) &&
                   strings.Name.Contains(text, System.StringComparison.OrdinalIgnoreCase);
        }

        private void SetResourceConnectionNotifications(bool value, System.Func<ResourceConnectionData, bool> predicate) {
            NativeArray<ResourceConnectionData> connections = m_ResourceConnectionNotificationParameterQuery.ToComponentDataArray<ResourceConnectionData>(Allocator.Temp);
            try {
                HashSet<Entity> seen = new();
                for (int i = 0; i < connections.Length; i++) {
                    ResourceConnectionData connection = connections[i];
                    Entity notificationPrefab = connection.m_ConnectionWarningNotification;
                    if (seen.Add(notificationPrefab) && predicate(connection)) {
                        SetNotificationIconDisplayEnabled(notificationPrefab, value);
                    }
                }
            }
            finally {
                connections.Dispose();
            }
        }

        private bool IsOilPipeNotConnectedNotification(ResourceConnectionData connection) {
            return connection.m_Resource == Resource.Oil ||
                   IsNotificationIcon(connection.m_ConnectionWarningNotification, "OilPipeNotConnected.svg") ||
                   IsNotificationPrefabName(connection.m_ConnectionWarningNotification, "Oil");
        }

        private bool IsFishingPierNotConnectedNotification(ResourceConnectionData connection) {
            return connection.m_Resource == Resource.Fish ||
                   IsNotificationIcon(connection.m_ConnectionWarningNotification, "FishingPierNotConnected.svg") ||
                   IsNotificationPrefabName(connection.m_ConnectionWarningNotification, "Fishing");
        }

        private bool IsOtherResourceConnectionNotification(ResourceConnectionData connection) {
            return !IsOilPipeNotConnectedNotification(connection) &&
                   !IsFishingPierNotConnectedNotification(connection);
        }

        private void SetNotificationIconDisplayEnabled(Entity notificationPrefab, bool value) {
            if (notificationPrefab == Entity.Null || !EntityManager.HasComponent<NotificationIconDisplayData>(notificationPrefab)) {
                return;
            }

            EntityManager.SetComponentEnabled<NotificationIconDisplayData>(notificationPrefab, value);
        }

        public void SetBuildingNotifications(bool refresh = true) {
            EnableBuildingNotification(BuildingNotificationIcon.AbandonedCollapsedNotification, CwdSettings.Instance.Notification.BuildingAbandonedCollapsedNotification);
            EnableBuildingNotification(BuildingNotificationIcon.AbandonedNotification, CwdSettings.Instance.Notification.BuildingAbandonedNotification);
            EnableBuildingNotification(BuildingNotificationIcon.CondemnedNotification, CwdSettings.Instance.Notification.BuildingCondemnedNotification);
            EnableBuildingNotification(BuildingNotificationIcon.TurnedOffNotification, CwdSettings.Instance.Notification.BuildingTurnedOffNotification);
            EnableBuildingNotification(BuildingNotificationIcon.HighRentNotification, CwdSettings.Instance.Notification.BuildingHighRentNotification);
            EnableBuildingNotification(BuildingNotificationIcon.LevelingNotification, CwdSettings.Instance.Notification.BuildingLevelingNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableBuildingNotification(BuildingNotificationIcon buildingNotificationIcon, bool value, bool refresh = false) {
            BuildingConfigurationData singleton = m_BuildingConfigurationDataQuery.GetSingleton<BuildingConfigurationData>();
            if (buildingNotificationIcon == BuildingNotificationIcon.AbandonedCollapsedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_AbandonedCollapsedNotification, value);
            }
            else if (buildingNotificationIcon == BuildingNotificationIcon.AbandonedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_AbandonedNotification, value);
            }
            else if (buildingNotificationIcon == BuildingNotificationIcon.CondemnedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_CondemnedNotification, value);
            }
            else if (buildingNotificationIcon == BuildingNotificationIcon.TurnedOffNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TurnedOffNotification, value);
            }
            else if (buildingNotificationIcon == BuildingNotificationIcon.HighRentNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_HighRentNotification, value);
            }
            else if (buildingNotificationIcon == BuildingNotificationIcon.LevelingNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_LevelingBuildingNotificationPrefab, value);
            }

            if (refresh)
                RefreshIcon();
        }

        public void SetWaterPipeNotifications(bool refresh = true) {
            EnableWaterPipeNotification(WaterPipeNotificationIcon.WaterNotification, CwdSettings.Instance.Notification.WaterPipeWaterNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.DirtyWaterNotification, CwdSettings.Instance.Notification.WaterPipeDirtyWaterNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.SewageNotification, CwdSettings.Instance.Notification.WaterPipeSewageNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.WaterPipeNotConnectedNotification, CwdSettings.Instance.Notification.WaterPipeWaterPipeNotConnectedNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.SewagePipeNotConnectedNotification, CwdSettings.Instance.Notification.WaterPipeSewagePipeNotConnectedNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughWaterCapacityNotification, CwdSettings.Instance.Notification.WaterPipeNotEnoughWaterCapacityNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughSewageCapacityNotification, CwdSettings.Instance.Notification.WaterPipeNotEnoughSewageCapacityNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughGroundwaterNotification, CwdSettings.Instance.Notification.WaterPipeNotEnoughGroundwaterNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughSurfaceWaterNotification, CwdSettings.Instance.Notification.WaterPipeNotEnoughSurfaceWaterNotification);
            EnableWaterPipeNotification(WaterPipeNotificationIcon.DirtyWaterPumpNotification, CwdSettings.Instance.Notification.WaterPipeDirtyWaterPumpNotification);
            if (refresh)
                RefreshIcon();
        }

        public void EnableWaterPipeNotification(WaterPipeNotificationIcon waterPipeNotificationIcon, bool value, bool refresh = false) {
            WaterPipeParameterData singleton = m_WaterPipeParameterQuery.GetSingleton<WaterPipeParameterData>();
            if (waterPipeNotificationIcon == WaterPipeNotificationIcon.WaterNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WaterNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.DirtyWaterNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_DirtyWaterNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.SewageNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_SewageNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.WaterPipeNotConnectedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_WaterPipeNotConnectedNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.SewagePipeNotConnectedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_SewagePipeNotConnectedNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.NotEnoughWaterCapacityNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughWaterCapacityNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.NotEnoughSewageCapacityNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughSewageCapacityNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.NotEnoughGroundwaterNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughGroundwaterNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.NotEnoughSurfaceWaterNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughSurfaceWaterNotification, value);
            }
            else if (waterPipeNotificationIcon == WaterPipeNotificationIcon.DirtyWaterPumpNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_DirtyWaterPumpNotification, value);
            }

            if (refresh) {
                RefreshIcon();
            }
        }

        public void SetElectricityNotifications(bool refresh = true) {
            EnableElectricityNotification(ElectricityNotificationIcon.ElectricityNotification, CwdSettings.Instance.Notification.ElectricityElectricityNotification);
            EnableElectricityNotification(ElectricityNotificationIcon.BottleneckNotification, CwdSettings.Instance.Notification.ElectricityBottleneckNotification);
            EnableElectricityNotification(ElectricityNotificationIcon.BuildingBottleneckNotification, CwdSettings.Instance.Notification.ElectricityBuildingBottleneckNotification);
            EnableElectricityNotification(ElectricityNotificationIcon.NotEnoughProductionNotification, CwdSettings.Instance.Notification.ElectricityNotEnoughProductionNotification);
            EnableElectricityNotification(ElectricityNotificationIcon.TransformerNotification, CwdSettings.Instance.Notification.ElectricityTransformerNotification);
            EnableElectricityNotification(ElectricityNotificationIcon.NotEnoughConnectedNotification, CwdSettings.Instance.Notification.ElectricityNotEnoughConnectedNotification);
            EnableElectricityNotification(ElectricityNotificationIcon.BatteryEmptyNotification, CwdSettings.Instance.Notification.ElectricityBatteryEmptyNotification);
            EnableElectricityNotification(ElectricityNotificationIcon.LowVoltageNotConnected, CwdSettings.Instance.Notification.ElectricityLowVoltageNotConnected);
            EnableElectricityNotification(ElectricityNotificationIcon.HighVoltageNotConnected, CwdSettings.Instance.Notification.ElectricityHighVoltageNotConnected);
            if (refresh)
                RefreshIcon();
        }

        public void EnableElectricityNotification(ElectricityNotificationIcon electricityNotificationIcon, bool value, bool refresh = false) {
            ElectricityParameterData singleton = m_ElectricParameterQuery.GetSingleton<ElectricityParameterData>();
            if (electricityNotificationIcon == ElectricityNotificationIcon.ElectricityNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_ElectricityNotificationPrefab, value);
            }
            else if (electricityNotificationIcon == ElectricityNotificationIcon.BottleneckNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BottleneckNotificationPrefab, value);
            }
            else if (electricityNotificationIcon == ElectricityNotificationIcon.BuildingBottleneckNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BuildingBottleneckNotificationPrefab, value);
            }
            else if (electricityNotificationIcon == ElectricityNotificationIcon.NotEnoughProductionNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughProductionNotificationPrefab, value);
            }
            else if (electricityNotificationIcon == ElectricityNotificationIcon.TransformerNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_TransformerNotificationPrefab, value);
            }
            else if (electricityNotificationIcon == ElectricityNotificationIcon.NotEnoughConnectedNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_NotEnoughConnectedNotificationPrefab, value);
            }
            else if (electricityNotificationIcon == ElectricityNotificationIcon.BatteryEmptyNotification) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_BatteryEmptyNotificationPrefab, value);
            }
            else if (electricityNotificationIcon == ElectricityNotificationIcon.LowVoltageNotConnected) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_LowVoltageNotConnectedPrefab, value);
            }
            else if (electricityNotificationIcon == ElectricityNotificationIcon.HighVoltageNotConnected) {
                EntityManager.SetComponentEnabled<NotificationIconDisplayData>(singleton.m_HighVoltageNotConnectedPrefab,
                    value);
            }

            if (refresh) {
                RefreshIcon();
            }
        }
    }
}
