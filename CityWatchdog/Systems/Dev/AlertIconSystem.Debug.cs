// <copyright file="AlertIconSystem.Debug.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Dev/AlertIconSystem.Debug.cs
// Purpose: Keeps AlertIconSystem debug-only prefab/icon inspection helpers out of the release-facing system file.

namespace CityWatchdog.Systems
{
    using System.Collections.Generic;   // needed for DEBUG
    using System.Linq;
    using CityWatchdog.Extensions;
    using CS2Shared.RiverMochi;         // LogUtils
    using Game.Prefabs;
    using Game.UI;

    public partial class AlertIconSystem
    {
    #if DEBUG
        public void Debug() => new List<System.Func<string>> {
            //LogElectricityNotificationSvgSources,
            //LogElectricityNotificationPrefabName,
            //LogWaterPipeNotificationSvgSources,
            //LogWaterPipeNotificationPrefabName,
            //LogBuildingNotificationSvgSources,
            //LogBuildingNotificationPrefabName,
            //LogTrafficNotificationSvgSources,
            //LogTrafficNotificationPrefabName,
            //LogCompanyNotificationSvgSources,
            //LogCompanyNotificationPrefabName,
            //LogWorkProviderNotificationSvgSources,
            //LogWorkProviderNotificationPrefabName
            //LogDisasterNotificationSvgSources,
            //LogDisasterNotificationPrefabName,
            //LogFireNotificationSvgSources,
            //LogFireNotificationPrefabName,
            //LogGarbageNotificationSvgSources,
            //LogGarbageNotificationPrefabName,
            //LogHealthcareNotificationSvgSources,
            //LogHealthcareNotificationPrefabName,
            //LogPoliceNotificationSvgSources,
            //LogPoliceNotificationPrefabName,
            //LogPollutionNotificationSvgSources,
            //LogPollutionNotificationPrefabName,
            //LogResourceConsumerNotificationSvgSources,
            //LogResourceConsumerNotificationPrefabName,
            //LogRouteNotificationSvgSources,
            //LogRouteNotificationPrefabName,
            //LogTransportLineNotificationSvgSources,
            //LogTransportLineNotificationPrefabName,
        }.ForEach(action => LogUtils.Debug(action));

        private List<NotificationIconPrefab> GetTransportLineNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            TransportLineData singleton = m_TransportLineNotificationParameterQuery.GetSingleton<TransportLineData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_VehicleNotification));
            return result;
        }

        private List<string> GetTransportLineNotificationSvg() => GetTransportLineNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogTransportLineNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogTransportLineNotificationSvgSources")).ToString(_ => GetTransportLineNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetTransportLineNotificationPrefabName() {
            List<string> result = new();
            TransportLineData singleton = m_TransportLineNotificationParameterQuery.GetSingleton<TransportLineData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_VehicleNotification).name);
            return result;
        }

        private string LogTransportLineNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogTransportLineNotificationPrefabName")).ToString(_ => GetTransportLineNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<NotificationIconPrefab> GetRouteNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            RouteConfigurationData singleton = m_RouteNotificationParameterQuery.GetSingleton<RouteConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_PathfindNotification));
            return result;
        }

        private List<string> GetRouteNotificationSvg() => GetRouteNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogRouteNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogRouteNotificationSvgSources")).ToString(_ => GetRouteNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetRouteNotificationPrefabName() {
            List<string> result = new();
            RouteConfigurationData singleton = m_RouteNotificationParameterQuery.GetSingleton<Game.Prefabs.RouteConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_PathfindNotification).name);
            return result;
        }

        private string LogRouteNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogRouteNotificationPrefabName")).ToString(_ => GetRouteNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<NotificationIconPrefab> GetResourceConsumerNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            ResourceConsumerData singleton = m_ResourceConsumerNotificationParameterQuery.GetSingleton<ResourceConsumerData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoResourceNotificationPrefab));
            return result;
        }

        private List<string> GetResourceConsumerNotificationSvg() => GetResourceConsumerNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogResourceConsumerNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogResourceConsumerNotificationSvgSources")).ToString(_ => GetResourceConsumerNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetResourceConsumerNotificationPrefabName() {
            List<string> result = new();
            ResourceConsumerData singleton = m_ResourceConsumerNotificationParameterQuery.GetSingleton<Game.Prefabs.ResourceConsumerData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoResourceNotificationPrefab).name);
            return result;
        }

        private string LogResourceConsumerNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogResourceConsumerNotificationPrefabName")).ToString(_ => GetResourceConsumerNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<NotificationIconPrefab> GetPollutionNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            PollutionParameterData singleton = m_PollutionNotificationParameterQuery.GetSingleton<PollutionParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AirPollutionNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoisePollutionNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_GroundPollutionNotification));
            return result;
        }

        private List<string> GetPollutionNotificationSvg() => GetPollutionNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogPollutionNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogPollutionNotificationSvgSources")).ToString(_ => GetPollutionNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetPollutionNotificationPrefabName() {
            List<string> result = new();
            PollutionParameterData singleton = m_PollutionNotificationParameterQuery.GetSingleton<Game.Prefabs.PollutionParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AirPollutionNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoisePollutionNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_GroundPollutionNotification).name);
            return result;
        }

        private string LogPollutionNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogPollutionNotificationPrefabName")).ToString(_ => GetPollutionNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<NotificationIconPrefab> GetPoliceNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            PoliceConfigurationData singleton = m_PoliceNotificationParameterQuery.GetSingleton<PoliceConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrafficAccidentNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CrimeSceneNotificationPrefab));
            return result;
        }

        private List<string> GetPoliceNotificationSvg() => GetPoliceNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogPoliceNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogPoliceNotificationSvgSources")).ToString(_ => GetPoliceNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetPoliceNotificationPrefabName() {
            List<string> result = new();
            PoliceConfigurationData singleton = m_PoliceNotificationParameterQuery.GetSingleton<Game.Prefabs.PoliceConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrafficAccidentNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CrimeSceneNotificationPrefab).name);
            return result;
        }

        private string LogPoliceNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogPoliceNotificationPrefabName")).ToString(_ => GetPoliceNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<NotificationIconPrefab> GetHealthcareNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            HealthcareParameterData singleton = m_HealthcareNotificationParameterQuery.GetSingleton<HealthcareParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AmbulanceNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HearseNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FacilityFullNotificationPrefab));
            return result;
        }

        private List<string> GetHealthcareNotificationSvg() => GetHealthcareNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogHealthcareNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogHealthcareNotificationSvgSources")).ToString(_ => GetHealthcareNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetHealthcareNotificationPrefabName() {
            List<string> result = new();
            HealthcareParameterData singleton = m_HealthcareNotificationParameterQuery.GetSingleton<Game.Prefabs.HealthcareParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AmbulanceNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HearseNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FacilityFullNotificationPrefab).name);
            return result;
        }

        private string LogHealthcareNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogHealthcareNotificationPrefabName")).ToString(_ => GetHealthcareNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<NotificationIconPrefab> GetGarbageNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            GarbageParameterData singleton = m_GarbageNotificationParameterQuery.GetSingleton<GarbageParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_GarbageNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FacilityFullNotificationPrefab));
            return result;
        }

        private List<string> GetGarbageNotificationSvg() => GetGarbageNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogGarbageNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogGarbageNotificationSvgSources")).ToString(_ => GetGarbageNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetGarbageNotificationPrefabName() {
            List<string> result = new();
            GarbageParameterData singleton = m_GarbageNotificationParameterQuery.GetSingleton<Game.Prefabs.GarbageParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_GarbageNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FacilityFullNotificationPrefab).name);
            return result;
        }

        private string LogGarbageNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogGarbageNotificationPrefabName")).ToString(_ => GetGarbageNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<NotificationIconPrefab> GetFireNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            FireConfigurationData singleton = m_FireNotificationParameterQuery.GetSingleton<FireConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FireNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BurnedDownNotificationPrefab));
            return result;
        }

        private List<string> GetFireNotificationSvg() => GetFireNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogFireNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogFireNotificationSvgSources")).ToString(_ => GetFireNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetFireNotificationPrefabName() {
            List<string> result = new();
            FireConfigurationData singleton = m_FireNotificationParameterQuery.GetSingleton<Game.Prefabs.FireConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_FireNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BurnedDownNotificationPrefab).name);
            return result;
        }

        private string LogFireNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogFireNotificationPrefabName")).ToString(_ => GetFireNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);


        private List<NotificationIconPrefab> GetDisasterNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            DisasterConfigurationData singleton = m_DisasterNotificationParameterQuery.GetSingleton<DisasterConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WeatherDamageNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WeatherDestroyedNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterDamageNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterDestroyedNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DestroyedNotificationPrefab));
            return result;
        }

        private List<string> GetDisasterNotificationSvg() => GetDisasterNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private string LogDisasterNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogDisasterNotificationSvgSources")).ToString(_ => GetDisasterNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetDisasterNotificationPrefabName() {
            List<string> result = new();
            DisasterConfigurationData singleton = m_DisasterNotificationParameterQuery.GetSingleton<Game.Prefabs.DisasterConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WeatherDamageNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WeatherDestroyedNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterDamageNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterDestroyedNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DestroyedNotificationPrefab).name);
            return result;
        }

        private string LogDisasterNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogDisasterNotificationPrefabName")).ToString(_ => GetDisasterNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private string LogWorkProviderNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogWorkProviderNotificationPrefabName")).ToString(_ => GetWorkProviderNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetWorkProviderNotificationPrefabName() {
            List<string> result = new();
            WorkProviderParameterData singleton = m_WorkProviderNotificationParameterQuery.GetSingleton<Game.Prefabs.WorkProviderParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_UneducatedNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_EducatedNotificationPrefab).name);
            return result;
        }

        private string LogWorkProviderNotificationSvgSources() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogWorkProviderNotificationSvgSources")).ToString(_ => GetWorkProviderNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);

        private List<string> GetWorkProviderNotificationSvg() => GetWorkProviderNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private List<NotificationIconPrefab> GetWorkProviderNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            WorkProviderParameterData singleton = m_WorkProviderNotificationParameterQuery.GetSingleton<WorkProviderParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_UneducatedNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_EducatedNotificationPrefab));
            return result;
        }

        private string LogCompanyNotificationPrefabName() => m_LogBuilder.ClearAndAppendLine(LogFlag("LogCompanyNotificationPrefabName")).ToString(_ => GetCompanyNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);


        private List<string> GetCompanyNotificationPrefabName() {
            List<string> result = new();
            CompanyNotificationParameterData singleton = m_CompanyNotificationParameterQuery.GetSingleton<CompanyNotificationParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoInputsNotificationPrefab).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoCustomersNotificationPrefab).name);
            return result;
        }

        private string LogCompanyNotificationSvgSources() {
            m_LogBuilder.ClearAndAppendLine(LogFlag("LogCompanyNotificationSvgSources"));
            return m_LogBuilder.ToString(_ => GetCompanyNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);
        }

        private List<string> GetCompanyNotificationSvg() => GetCompanyNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private List<NotificationIconPrefab> GetCompanyNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            CompanyNotificationParameterData singleton = m_CompanyNotificationParameterQuery.GetSingleton<CompanyNotificationParameterData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoInputsNotificationPrefab));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NoCustomersNotificationPrefab));
            return result;
        }

        private string LogTrafficNotificationPrefabName() {
            m_LogBuilder.Clear();
            m_LogBuilder.AppendLine(LogFlag("LogTrafficNotificationPrefabName"));
            m_LogBuilder.ToString(_ => GetTrafficNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);
            return m_LogBuilder.ToString();
        }

        private List<string> GetTrafficNotificationPrefabName() {
            List<string> result = new();
            TrafficConfigurationData singleton = m_TrafficConfigurationDataQuery.GetSingleton<TrafficConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BottleneckNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DeadEndNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_RoadConnectionNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrackConnectionNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CarConnectionNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_ShipConnectionNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrainConnectionNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_PedestrianConnectionNotification).name);
            return result;
        }

        private string LogTrafficNotificationSvgSources() {
            m_LogBuilder.Clear();
            m_LogBuilder.AppendLine(LogFlag("LogTrafficNotificationSvgSources"));
            m_LogBuilder.ToString(_ => GetTrafficNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);
            return m_LogBuilder.ToString();
        }

        private List<string> GetTrafficNotificationSvg() => GetTrafficNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private List<NotificationIconPrefab> GetTrafficNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            TrafficConfigurationData singleton = m_TrafficConfigurationDataQuery.GetSingleton<TrafficConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BottleneckNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DeadEndNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_RoadConnectionNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrackConnectionNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CarConnectionNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_ShipConnectionNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TrainConnectionNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_PedestrianConnectionNotification));
            return result;
        }

        private string LogBuildingNotificationPrefabName() {
            m_LogBuilder.Clear();
            m_LogBuilder.AppendLine(LogFlag("LogBuildingNotificationPrefabName"));
            m_LogBuilder.ToString(_ => GetBuildingNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);
            return m_LogBuilder.ToString();
        }

        private List<string> GetBuildingNotificationPrefabName() {
            List<string> result = new();
            BuildingConfigurationData singleton = m_BuildingConfigurationDataQuery.GetSingleton<BuildingConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AbandonedCollapsedNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AbandonedNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CondemnedNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_LevelUpNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TurnedOffNotification).name);
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HighRentNotification).name);
            return result;
        }

        private string LogBuildingNotificationSvgSources() {
            m_LogBuilder.Clear();
            m_LogBuilder.AppendLine(LogFlag("LogBuildingNotificationSvgSources"));
            m_LogBuilder.ToString(_ => GetBuildingNotificationSvg().ForEach(v => _.AppendLine($"\"{v}\",")), false);
            return m_LogBuilder.ToString();
        }

        private List<string> GetBuildingNotificationSvg() => GetBuildingNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private List<NotificationIconPrefab> GetBuildingNotificationPrefab() {
            List<NotificationIconPrefab> result = new();
            BuildingConfigurationData singleton = m_BuildingConfigurationDataQuery.GetSingleton<BuildingConfigurationData>();
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AbandonedCollapsedNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_AbandonedNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_CondemnedNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_LevelUpNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TurnedOffNotification));
            result.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HighRentNotification));
            return result;
        }

        private string LogWaterPipeNotificationPrefabName() {
            m_LogBuilder.Clear();
            m_LogBuilder.AppendLine(LogFlag("LogWaterPipeNotificationPrefabName"));
            m_LogBuilder.ToString(_ => GetWaterPipeNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);
            return m_LogBuilder.ToString();
        }

        private List<string> GetWaterPipeNotificationPrefabName() {
            List<string> name = new();
            WaterPipeParameterData singleton = m_WaterPipeParameterQuery.GetSingleton<WaterPipeParameterData>();
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DirtyWaterNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_SewageNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterPipeNotConnectedNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_SewagePipeNotConnectedNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughWaterCapacityNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughSewageCapacityNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughGroundwaterNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughSurfaceWaterNotification).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DirtyWaterPumpNotification).name);
            return name;
        }

        private string LogWaterPipeNotificationSvgSources() {
            m_LogBuilder.Clear();
            m_LogBuilder.AppendLine(LogFlag("LogWaterPipeNotificationSvgSources"));
            m_LogBuilder.ToString(_ => GetWaterPipeNotificationSvgSources().ForEach(v => _.AppendLine($"\"{v}\",")), false);
            return m_LogBuilder.ToString();
        }

        private List<string> GetWaterPipeNotificationSvgSources() => GetWaterPipeNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private List<NotificationIconPrefab> GetWaterPipeNotificationPrefab() {
            List<NotificationIconPrefab> notificationIconPrefabs = new();
            WaterPipeParameterData singleton = m_WaterPipeParameterQuery.GetSingleton<WaterPipeParameterData>();
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DirtyWaterNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_SewageNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_WaterPipeNotConnectedNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_SewagePipeNotConnectedNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughWaterCapacityNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughSewageCapacityNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughGroundwaterNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughSurfaceWaterNotification));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_DirtyWaterPumpNotification));
            return notificationIconPrefabs;
        }

        private string LogElectricityNotificationPrefabName() {
            m_LogBuilder.Clear();
            m_LogBuilder.AppendLine(LogFlag("LogElectricityNotificationPrefabName"));
            m_LogBuilder.ToString(_ => GetElectricityNotificationPrefabName().ForEach(v => _.AppendLine($"\"{v}\",")), false);
            return m_LogBuilder.ToString();
        }

        private List<string> GetElectricityNotificationPrefabName() {
            List<string> name = new();
            ElectricityParameterData singleton = m_ElectricParameterQuery.GetSingleton<ElectricityParameterData>();
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_ElectricityNotificationPrefab).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BottleneckNotificationPrefab).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BuildingBottleneckNotificationPrefab).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughProductionNotificationPrefab).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TransformerNotificationPrefab).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughConnectedNotificationPrefab).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BatteryEmptyNotificationPrefab).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_LowVoltageNotConnectedPrefab).name);
            name.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HighVoltageNotConnectedPrefab).name);
            return name;
        }

        private string LogElectricityNotificationSvgSources() {
            m_LogBuilder.Clear();
            m_LogBuilder.AppendLine(LogFlag("LogElectricityNotificationSvgSources"));
            m_LogBuilder.ToString(_ => GetElectricityNotificationSvgSources().ForEach(v => _.AppendLine($"\"{v}\",")), false);
            return m_LogBuilder.ToString();
        }

        private List<string> GetElectricityNotificationSvgSources() => GetElectricityNotificationPrefab().Select(_ => ImageSystem.GetIcon(_)).ToList();

        private List<NotificationIconPrefab> GetElectricityNotificationPrefab() {
            List<NotificationIconPrefab> notificationIconPrefabs = new();
            ElectricityParameterData singleton = m_ElectricParameterQuery.GetSingleton<ElectricityParameterData>();
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_ElectricityNotificationPrefab));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BottleneckNotificationPrefab));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BuildingBottleneckNotificationPrefab));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughProductionNotificationPrefab));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_TransformerNotificationPrefab));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_NotEnoughConnectedNotificationPrefab));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_BatteryEmptyNotificationPrefab));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_LowVoltageNotConnectedPrefab));
            notificationIconPrefabs.Add(m_PrefabSystem.GetPrefab<NotificationIconPrefab>(singleton.m_HighVoltageNotConnectedPrefab));
            return notificationIconPrefabs;
        }

        private static string LogFlag(string name) => $"--- {name} ---";

    #endif
    }
}
