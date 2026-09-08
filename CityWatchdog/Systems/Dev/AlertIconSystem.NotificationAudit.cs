// <copyright file="AlertIconSystem.NotificationAudit.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Dev/AlertIconSystem.NotificationAudit.cs
// Purpose: One-click report for comparing game notification prefabs with City Watchdog coverage.

namespace CityWatchdog.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CS2Shared.RiverMochi;
    using Game.Economy;
    using Game.Prefabs;
    using Game.UI;
    using Unity.Collections;
    using Unity.Entities;

    public partial class AlertIconSystem
    {
        public void WriteNotificationAuditLog()
        {
            LogUtils.Info(BuildNotificationAuditReport);
        }

        private string BuildNotificationAuditReport()
        {
            Dictionary<Entity, string> tracked = BuildTrackedNotificationPrefabMap();
            List<NotificationAuditRow> rows = GetNotificationAuditRows(tracked);
            HashSet<Entity> gameNotificationPrefabs = new(rows.Select(row => row.Entity));

            List<NotificationAuditRow> trackedRows = rows
                .Where(row => row.Tracked)
                .OrderBy(row => row.TrackedBy, StringComparer.Ordinal)
                .ThenBy(row => row.PrefabName, StringComparer.Ordinal)
                .ToList();

            List<NotificationAuditRow> untrackedGameNotificationRows = rows
                .Where(row => !row.Tracked && IsGameNotificationIcon(row.Icon))
                .OrderBy(row => row.Icon, StringComparer.Ordinal)
                .ThenBy(row => row.PrefabName, StringComparer.Ordinal)
                .ToList();

            List<NotificationAuditRow> untrackedOtherRows = rows
                .Where(row => !row.Tracked && !IsGameNotificationIcon(row.Icon))
                .OrderBy(row => row.Icon, StringComparer.Ordinal)
                .ThenBy(row => row.PrefabName, StringComparer.Ordinal)
                .ToList();

            List<KeyValuePair<Entity, string>> missingTrackedRows = tracked
                .Where(item => !gameNotificationPrefabs.Contains(item.Key))
                .OrderBy(item => item.Value, StringComparer.Ordinal)
                .ToList();

            StringBuilder builder = new();
            builder.AppendLine("=== City Watchdog Notification Coverage Audit ===");
            builder.AppendLine("Build: manual one-click audit");
            builder.AppendLine("Purpose: compare live game NotificationIconDisplayData prefabs with CWD-controlled notification rows.");
            builder.AppendLine("Note: titleKeyGuess is inferred from prefab description/name. Confirm exact keys in dev mode if needed.");
            builder.AppendLine();
            builder.AppendLine($"Game notification prefab count: {rows.Count}");
            builder.AppendLine($"CWD tracked prefab count: {tracked.Count}");
            builder.AppendLine($"CWD tracked prefabs found in game query: {trackedRows.Count}");
            builder.AppendLine($"Untracked prefabs using Media/Game/Notifications icons: {untrackedGameNotificationRows.Count}");
            builder.AppendLine($"Untracked other notification prefabs: {untrackedOtherRows.Count}");
            builder.AppendLine($"Tracked CWD prefabs missing NotificationIconDisplayData: {missingTrackedRows.Count}");
            builder.AppendLine();

            AppendAuditSection(builder, "CWD TRACKED PREFABS FOUND", trackedRows);
            AppendAuditSection(builder, "UNTRACKED GAME NOTIFICATION ICON PREFABS - REVIEW THESE AFTER PATCHES", untrackedGameNotificationRows);
            AppendAuditSection(builder, "UNTRACKED OTHER NOTIFICATION PREFABS - OFTEN TOOL/MARKER/EDITOR ITEMS", untrackedOtherRows);
            AppendMissingTrackedSection(builder, missingTrackedRows);

            builder.AppendLine("=== End City Watchdog Notification Coverage Audit ===");
            return builder.ToString();
        }

        private Dictionary<Entity, string> BuildTrackedNotificationPrefabMap()
        {
            Dictionary<Entity, string> tracked = new();

            if (TryGetQuerySingleton(m_ElectricParameterQuery, out ElectricityParameterData electricity))
            {
                AddTracked(tracked, electricity.m_ElectricityNotificationPrefab, "ElectricityElectricityNotification");
                AddTracked(tracked, electricity.m_BottleneckNotificationPrefab, "ElectricityBottleneckNotification");
                AddTracked(tracked, electricity.m_BuildingBottleneckNotificationPrefab, "ElectricityBuildingBottleneckNotification");
                AddTracked(tracked, electricity.m_NotEnoughProductionNotificationPrefab, "ElectricityNotEnoughProductionNotification");
                AddTracked(tracked, electricity.m_TransformerNotificationPrefab, "ElectricityTransformerNotification");
                AddTracked(tracked, electricity.m_NotEnoughConnectedNotificationPrefab, "ElectricityNotEnoughConnectedNotification");
                AddTracked(tracked, electricity.m_BatteryEmptyNotificationPrefab, "ElectricityBatteryEmptyNotification");
                AddTracked(tracked, electricity.m_LowVoltageNotConnectedPrefab, "ElectricityLowVoltageNotConnected");
                AddTracked(tracked, electricity.m_HighVoltageNotConnectedPrefab, "ElectricityHighVoltageNotConnected");
            }

            if (TryGetQuerySingleton(m_WaterPipeParameterQuery, out WaterPipeParameterData water))
            {
                AddTracked(tracked, water.m_WaterNotification, "WaterPipeWaterNotification");
                AddTracked(tracked, water.m_DirtyWaterNotification, "WaterPipeDirtyWaterNotification");
                AddTracked(tracked, water.m_SewageNotification, "WaterPipeSewageNotification");
                AddTracked(tracked, water.m_WaterPipeNotConnectedNotification, "WaterPipeWaterPipeNotConnectedNotification");
                AddTracked(tracked, water.m_SewagePipeNotConnectedNotification, "WaterPipeSewagePipeNotConnectedNotification");
                AddTracked(tracked, water.m_NotEnoughWaterCapacityNotification, "WaterPipeNotEnoughWaterCapacityNotification");
                AddTracked(tracked, water.m_NotEnoughSewageCapacityNotification, "WaterPipeNotEnoughSewageCapacityNotification");
                AddTracked(tracked, water.m_NotEnoughGroundwaterNotification, "WaterPipeNotEnoughGroundwaterNotification");
                AddTracked(tracked, water.m_NotEnoughSurfaceWaterNotification, "WaterPipeNotEnoughSurfaceWaterNotification");
                AddTracked(tracked, water.m_DirtyWaterPumpNotification, "WaterPipeDirtyWaterPumpNotification");
            }

            if (TryGetQuerySingleton(m_BuildingConfigurationDataQuery, out BuildingConfigurationData building))
            {
                AddTracked(tracked, building.m_AbandonedCollapsedNotification, "BuildingAbandonedCollapsedNotification");
                AddTracked(tracked, building.m_AbandonedNotification, "BuildingAbandonedNotification");
                AddTracked(tracked, building.m_CondemnedNotification, "BuildingCondemnedNotification");
                AddTracked(tracked, building.m_TurnedOffNotification, "BuildingTurnedOffNotification");
                AddTracked(tracked, building.m_HighRentNotification, "BuildingHighRentNotification");
            }

            if (TryGetQuerySingleton(m_TrafficConfigurationDataQuery, out TrafficConfigurationData traffic))
            {
                AddTracked(tracked, traffic.m_BottleneckNotification, "TrafficBottleneckNotification");
                AddTracked(tracked, traffic.m_DeadEndNotification, "TrafficDeadEndNotification");
                AddTracked(tracked, traffic.m_RoadConnectionNotification, "TrafficRoadConnectionNotification");
                AddTracked(tracked, traffic.m_TrackConnectionNotification, "TrafficTrackConnectionNotification");
                AddTracked(tracked, traffic.m_CarConnectionNotification, "TrafficCarConnectionNotification");
                AddTracked(tracked, traffic.m_ShipConnectionNotification, "TrafficShipConnectionNotification");
                AddTracked(tracked, traffic.m_TrainConnectionNotification, "TrafficTrainConnectionNotification");
                AddTracked(tracked, traffic.m_PedestrianConnectionNotification, "TrafficPedestrianConnectionNotification");
                AddTracked(tracked, traffic.m_BicycleConnectionNotification, "TrafficBicycleConnectionNotification");
            }

            if (TryGetQuerySingleton(m_CompanyNotificationParameterQuery, out CompanyNotificationParameterData company))
            {
                AddTracked(tracked, company.m_NoInputsNotificationPrefab, "CompanyNoInputsNotification");
                AddTracked(tracked, company.m_NoCustomersNotificationPrefab, "CompanyNoCustomersNotification");
            }

            if (TryGetQuerySingleton(m_WorkProviderNotificationParameterQuery, out WorkProviderParameterData workProvider))
            {
                AddTracked(tracked, workProvider.m_UneducatedNotificationPrefab, "WorkProviderUneducatedNotification");
                AddTracked(tracked, workProvider.m_EducatedNotificationPrefab, "WorkProviderEducatedNotification");
            }

            if (TryGetQuerySingleton(m_DisasterNotificationParameterQuery, out DisasterConfigurationData disaster))
            {
                AddTracked(tracked, disaster.m_WeatherDamageNotificationPrefab, "DisasterWeatherDamageNotification");
                AddTracked(tracked, disaster.m_WeatherDestroyedNotificationPrefab, "DisasterWeatherDestroyedNotification");
                AddTracked(tracked, disaster.m_WaterDamageNotificationPrefab, "DisasterWaterDamageNotification");
                AddTracked(tracked, disaster.m_WaterDestroyedNotificationPrefab, "DisasterWaterDestroyedNotification");
                AddTracked(tracked, disaster.m_DestroyedNotificationPrefab, "DisasterDestroyedNotification");
            }

            if (TryGetQuerySingleton(m_FireNotificationParameterQuery, out FireConfigurationData fire))
            {
                AddTracked(tracked, fire.m_FireNotificationPrefab, "FireFireNotification");
                AddTracked(tracked, fire.m_BurnedDownNotificationPrefab, "FireBurnedDownNotification");
            }

            if (TryGetQuerySingleton(m_GarbageNotificationParameterQuery, out GarbageParameterData garbage))
            {
                AddTracked(tracked, garbage.m_GarbageNotificationPrefab, "GarbageGarbageNotification");
                AddTracked(tracked, garbage.m_FacilityFullNotificationPrefab, "GarbageFacilityFullNotification");
            }

            if (TryGetQuerySingleton(m_HealthcareNotificationParameterQuery, out HealthcareParameterData healthcare))
            {
                AddTracked(tracked, healthcare.m_AmbulanceNotificationPrefab, "HealthcareAmbulanceNotification");
                AddTracked(tracked, healthcare.m_HearseNotificationPrefab, "HealthcareHearseNotification");
                AddTracked(tracked, healthcare.m_FacilityFullNotificationPrefab, "HealthcareFacilityFullNotification");
            }

            if (TryGetQuerySingleton(m_PoliceNotificationParameterQuery, out PoliceConfigurationData police))
            {
                AddTracked(tracked, police.m_TrafficAccidentNotificationPrefab, "PoliceTrafficAccidentNotification");
                AddTracked(tracked, police.m_CrimeSceneNotificationPrefab, "PoliceCrimeSceneNotification");
            }

            if (TryGetQuerySingleton(m_PollutionNotificationParameterQuery, out PollutionParameterData pollution))
            {
                AddTracked(tracked, pollution.m_AirPollutionNotification, "PollutionAirPollutionNotification");
                AddTracked(tracked, pollution.m_NoisePollutionNotification, "PollutionNoisePollutionNotification");
                AddTracked(tracked, pollution.m_GroundPollutionNotification, "PollutionGroundPollutionNotification");
            }

            AddResourceConsumerTrackedPrefabs(tracked);
            AddResourceConnectionTrackedPrefabs(tracked);

            if (TryGetQuerySingleton(m_RouteNotificationParameterQuery, out RouteConfigurationData route))
            {
                AddTracked(tracked, route.m_PathfindNotification, "RoutePathfindNotification");
                AddTracked(tracked, route.m_GateBypassNotification, "RouteGateBypassNotification");
            }

            if (TryGetQuerySingleton(m_TransportLineNotificationParameterQuery, out TransportLineData transportLine))
            {
                AddTracked(tracked, transportLine.m_VehicleNotification, "TransportLineVehicleNotification");
            }

            return tracked;
        }

        private void AddResourceConsumerTrackedPrefabs(Dictionary<Entity, string> tracked)
        {
            if (m_ResourceConsumerNotificationParameterQuery.CalculateEntityCount() == 0)
            {
                return;
            }

            NativeArray<ResourceConsumerData> consumers =
                m_ResourceConsumerNotificationParameterQuery.ToComponentDataArray<ResourceConsumerData>(Allocator.Temp);
            try
            {
                for (int i = 0; i < consumers.Length; i++)
                {
                    string trackedBy = GetResourceConsumerTrackedBy(consumers[i].m_NoResourceNotificationPrefab);
                    AddTracked(tracked, consumers[i].m_NoResourceNotificationPrefab, trackedBy);
                }
            }
            finally
            {
                consumers.Dispose();
            }
        }

        private void AddResourceConnectionTrackedPrefabs(Dictionary<Entity, string> tracked)
        {
            if (m_ResourceConnectionNotificationParameterQuery.CalculateEntityCount() == 0)
            {
                return;
            }

            NativeArray<ResourceConnectionData> connections =
                m_ResourceConnectionNotificationParameterQuery.ToComponentDataArray<ResourceConnectionData>(Allocator.Temp);
            try
            {
                for (int i = 0; i < connections.Length; i++)
                {
                    string trackedBy = GetResourceConnectionTrackedBy(connections[i]);
                    AddTracked(tracked, connections[i].m_ConnectionWarningNotification, trackedBy);
                }
            }
            finally
            {
                connections.Dispose();
            }
        }

        private List<NotificationAuditRow> GetNotificationAuditRows(Dictionary<Entity, string> tracked)
        {
            List<NotificationAuditRow> rows = new();
            NativeArray<Entity> entities = m_NotificationIconDisplayDataQuery.ToEntityArray(Allocator.Temp);
            try
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    Entity entity = entities[i];
                    NotificationIconPrefab? prefab = TryGetNotificationIconPrefab(entity);
                    if (prefab == null)
                    {
                        continue;
                    }

                    tracked.TryGetValue(entity, out string trackedBy);
                    string icon = ImageSystem.GetIcon(prefab) ?? string.Empty;
                    rows.Add(new NotificationAuditRow
                    {
                        Entity = entity,
                        PrefabName = prefab.name ?? string.Empty,
                        Kind = GetNotificationAuditKind(entity),
                        Icon = icon,
                        DescriptionKey = prefab.m_Description ?? string.Empty,
                        TargetDescriptionKey = prefab.m_TargetDescription ?? string.Empty,
                        TitleKeyGuess = GuessTitleKey(prefab),
                        Enabled = EntityManager.IsComponentEnabled<NotificationIconDisplayData>(entity),
                        TrackedBy = trackedBy ?? string.Empty,
                    });
                }
            }
            finally
            {
                entities.Dispose();
            }

            return rows;
        }

        private static bool TryGetQuerySingleton<T>(EntityQuery query, out T value)
            where T : unmanaged, IComponentData
        {
            value = default;
            if (query.CalculateEntityCount() == 0)
            {
                return false;
            }

            try
            {
                value = query.GetSingleton<T>();
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Warn(() => $"Notification audit skipped singleton {typeof(T).Name}: {ex.GetType().Name}: {ex.Message}", ex);
                return false;
            }
        }

        private string GetResourceConsumerTrackedBy(Entity notificationPrefab)
        {
            NotificationIconPrefab? prefab = TryGetNotificationIconPrefab(notificationPrefab);
            string icon = prefab == null ? string.Empty : ImageSystem.GetIcon(prefab) ?? string.Empty;
            string name = prefab?.name ?? string.Empty;

            if (icon.EndsWith("NoFuel.svg", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Fuel", StringComparison.OrdinalIgnoreCase))
            {
                return "ResourceConsumerNoFuelNotification";
            }

            if (icon.EndsWith("NotEnoughIndustrialGoods.svg", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Supplies", StringComparison.OrdinalIgnoreCase))
            {
                return "ResourceConsumerNoResourceNotification";
            }

            return "ResourceConsumerNoResourceNotification";
        }

        private string GetResourceConnectionTrackedBy(ResourceConnectionData connection)
        {
            Entity notificationPrefab = connection.m_ConnectionWarningNotification;
            NotificationIconPrefab? prefab = TryGetNotificationIconPrefab(notificationPrefab);
            string icon = prefab == null ? string.Empty : ImageSystem.GetIcon(prefab) ?? string.Empty;
            string name = prefab?.name ?? string.Empty;

            if (connection.m_Resource == Resource.Oil ||
                icon.EndsWith("OilPipeNotConnected.svg", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Oil", StringComparison.OrdinalIgnoreCase))
            {
                return "ResourceConnectionOilPipeNotConnectedNotification";
            }

            if (connection.m_Resource == Resource.Fish ||
                icon.EndsWith("FishingPierNotConnected.svg", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Fishing", StringComparison.OrdinalIgnoreCase))
            {
                return "ResourceConnectionFishingPierNotConnectedNotification";
            }

            return "ResourceConnectionWarningNotification";
        }

        private NotificationIconPrefab? TryGetNotificationIconPrefab(Entity entity)
        {
            try
            {
                return m_PrefabSystem.GetPrefab<NotificationIconPrefab>(entity);
            }
            catch
            {
                return null;
            }
        }

        private static void AddTracked(Dictionary<Entity, string> tracked, Entity entity, string settingName)
        {
            if (entity == Entity.Null)
            {
                return;
            }

            if (tracked.TryGetValue(entity, out string existing))
            {
                if (!existing.Contains(settingName, StringComparison.Ordinal))
                {
                    tracked[entity] = existing + ", " + settingName;
                }

                return;
            }

            tracked.Add(entity, settingName);
        }

        private static void AppendAuditSection(StringBuilder builder, string title, List<NotificationAuditRow> rows)
        {
            builder.AppendLine($"--- {title} ({rows.Count}) ---");
            if (rows.Count == 0)
            {
                builder.AppendLine("(none)");
                builder.AppendLine();
                return;
            }

            for (int i = 0; i < rows.Count; i++)
            {
                NotificationAuditRow row = rows[i];
                builder.AppendLine(
                    $"- prefab=\"{Clean(row.PrefabName)}\" | kind=\"{Clean(row.Kind)}\" | trackedBy=\"{Clean(row.TrackedBy)}\" | enabled={row.Enabled} | icon=\"{Clean(row.Icon)}\" | desc=\"{Clean(row.DescriptionKey)}\" | targetDesc=\"{Clean(row.TargetDescriptionKey)}\" | titleKeyGuess=\"{Clean(row.TitleKeyGuess)}\"");
            }

            builder.AppendLine();
        }

        private string GetNotificationAuditKind(Entity entity)
        {
            List<string> kinds = new();

            try
            {
                if (EntityManager.HasComponent<ToolErrorData>(entity))
                {
                    ToolErrorData data = EntityManager.GetComponentData<ToolErrorData>(entity);
                    kinds.Add($"ToolError:{data.m_Error}/{data.m_Flags}");
                }

                if (EntityManager.HasComponent<BuildingMarkerData>(entity))
                {
                    BuildingMarkerData data = EntityManager.GetComponentData<BuildingMarkerData>(entity);
                    kinds.Add($"BuildingMarker:{data.m_BuildingType}");
                }

                if (EntityManager.HasComponent<VehicleMarkerData>(entity))
                {
                    VehicleMarkerData data = EntityManager.GetComponentData<VehicleMarkerData>(entity);
                    kinds.Add($"VehicleMarker:{data.m_VehicleType}");
                }

                if (EntityManager.HasComponent<MarkerMarkerData>(entity))
                {
                    MarkerMarkerData data = EntityManager.GetComponentData<MarkerMarkerData>(entity);
                    kinds.Add($"MarkerMarker:{data.m_MarkerType}");
                }

                if (EntityManager.HasComponent<TransportStopMarkerData>(entity))
                {
                    TransportStopMarkerData data = EntityManager.GetComponentData<TransportStopMarkerData>(entity);
                    kinds.Add($"TransportStopMarker:{data.m_TransportType}");
                }
            }
            catch (Exception ex)
            {
                kinds.Add("KindLookupFailed:" + ex.GetType().Name);
            }

            return kinds.Count == 0 ? "NotificationIcon" : string.Join(", ", kinds);
        }

        private static void AppendMissingTrackedSection(StringBuilder builder, List<KeyValuePair<Entity, string>> missingTrackedRows)
        {
            builder.AppendLine($"--- CWD TRACKED PREFABS MISSING FROM GAME NOTIFICATION QUERY ({missingTrackedRows.Count}) ---");
            if (missingTrackedRows.Count == 0)
            {
                builder.AppendLine("(none)");
                builder.AppendLine();
                return;
            }

            for (int i = 0; i < missingTrackedRows.Count; i++)
            {
                KeyValuePair<Entity, string> row = missingTrackedRows[i];
                builder.AppendLine($"- trackedBy=\"{Clean(row.Value)}\" | entity={row.Key}");
            }

            builder.AppendLine();
        }

        private static string GuessTitleKey(NotificationIconPrefab prefab)
        {
            string keyText = ExtractBracketText(prefab.m_Description);
            if (string.IsNullOrWhiteSpace(keyText))
            {
                keyText = prefab.name ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(keyText))
            {
                return string.Empty;
            }

            return $"Notifications.TITLE[{keyText}] | NOTIFICATIONS.TITLE[{keyText.ToUpperInvariant()}]";
        }

        private static string ExtractBracketText(string? value)
        {
            string text = value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            int start = text.IndexOf('[', StringComparison.Ordinal);
            int end = text.LastIndexOf(']');
            if (start < 0 || end <= start)
            {
                return string.Empty;
            }

            return text.Substring(start + 1, end - start - 1);
        }

        private static bool IsGameNotificationIcon(string icon)
        {
            return icon.Contains("Media/Game/Notifications", StringComparison.OrdinalIgnoreCase);
        }

        private static string Clean(string? value)
        {
            string text = value ?? string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text
                .Replace("\r", "\\r", StringComparison.Ordinal)
                .Replace("\n", "\\n", StringComparison.Ordinal);
        }

        private sealed class NotificationAuditRow
        {
            public Entity Entity { get; set; }
            public string PrefabName { get; set; } = string.Empty;
            public string Kind { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
            public string DescriptionKey { get; set; } = string.Empty;
            public string TargetDescriptionKey { get; set; } = string.Empty;
            public string TitleKeyGuess { get; set; } = string.Empty;
            public bool Enabled { get; set; }
            public string TrackedBy { get; set; } = string.Empty;

            public bool Tracked => !string.IsNullOrEmpty(TrackedBy);
        }
    }
}
