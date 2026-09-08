// <copyright file="CityWatchdogUISystem.Bindings.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/CityWatchdogUISystem.Bindings.cs
// Purpose: C# bindings and triggers shared with the React panel and Mini HUD.

namespace CityWatchdog.Systems
{
    using Colossal.UI.Binding;
    using Game.UI;

    public partial class CityWatchdogUISystem
    {
        private BoolBinding m_PanelVisibleBinding = null!;
        private UIUpdateState m_NotificationCountUpdateState = null!;
        private UIUpdateState m_MiniHudCountUpdateState = null!;
        private ValueBinding<int[]> m_NotificationCountsBinding = null!;
        private ValueBinding<int[]> m_MiniHudFavoritesBinding = null!;
        private ValueBinding<bool> m_MiniHudEnabledBinding = null!;
        private ValueBinding<int> m_MiniHudModeBinding = null!;
        private ValueBinding<int> m_MiniHudItemCountBinding = null!;
        private ValueBinding<int> m_MiniHudScaleBinding = null!;
        private ValueBinding<int> m_MiniHudOrientationBinding = null!;
        private ValueBinding<int> m_MiniHudPlacementBinding = null!;
        private ValueBinding<bool> m_MiniHudHideZeroBinding = null!;
        private ValueBinding<int> m_MiniHudPanelStyleBinding = null!;
        private ValueBinding<int> m_MiniHudPanelOpacityBinding = null!;
        private ValueBinding<int> m_MiniHudHorizontalPositionXBinding = null!;
        private ValueBinding<int> m_MiniHudHorizontalPositionYBinding = null!;
        private ValueBinding<int> m_MiniHudVerticalPositionXBinding = null!;
        private ValueBinding<int> m_MiniHudVerticalPositionYBinding = null!;
        private ValueBinding<int> m_PanelPositionXBinding = null!;
        private ValueBinding<int> m_PanelPositionYBinding = null!;
        private ValueBinding<int> m_PanelCollapsedSectionsMaskBinding = null!;
        private ValueBinding<int> m_PanelSortModeBinding = null!;
        private ValueBinding<bool> m_PanelButtonsOnlyStartBinding = null!;
        private ValueBinding<int> m_MainPanelOpacityBinding = null!;
        private ValueBinding<bool> m_Preset1SavedBinding = null!;
        private ValueBinding<bool> m_Preset2SavedBinding = null!;
        private ValueBinding<int> m_ActivePresetBinding = null!;
        private ValueBinding<bool>? m_MoneyViewBinding;
        private ValueBinding<int>? m_MoneyViewModeBinding;
        private ValueBinding<int>? m_MoneyTooltipModeBinding;
        private ValueBinding<int>? m_MoneyTooltipFontScaleBinding;
        private ValueBinding<int>? m_PopulationTooltipFontScaleBinding;

        // Binding IDs are the C# <-> React contract. Field names can change, but these IDs
        // must stay aligned with UI.
        protected override void OnCreate() {
            base.OnCreate();

            m_AlertIconSystem = World.GetOrCreateSystemManaged<AlertIconSystem>();
            InitializeKeybindActions();
            m_NotificationCountUpdateState = UIUpdateState.Create(World, kPanelCountUpdateInterval);
            m_MiniHudCountUpdateState = UIUpdateState.Create(World, kMiniHudCountUpdateInterval);

            m_PanelVisibleBinding = AddBoolBindingAndTriggerBinding("ControlPanelEnabled", false, OnControlPanelBindingToggle);
            AddBoolTriggerBinding("ToggleAllNotifications", ApplyAllNotificationToggles);
            m_NotificationCountsBinding = new ValueBinding<int[]>(
                ModId,
                "NotificationCounts",
                new int[AlertIconSystem.NotificationCountLength],
                new ArrayWriter<int>());
            AddBinding(m_NotificationCountsBinding);
            m_MiniHudFavoritesBinding = new ValueBinding<int[]>(
                ModId,
                "MiniHudFavorites",
                GetMiniHudFavoriteIndexes(),
                new ArrayWriter<int>());
            AddBinding(m_MiniHudFavoritesBinding);
            AddTriggerBinding<int>("ToggleMiniHudFavorite", ToggleMiniHudFavorite);
            AddTriggerBinding<int>("MiniHudNotificationClicked", JumpToMiniHudNotification);
            AddTriggerBinding<string>("MiniHudPositionChanged", SaveMiniHudPosition);
            AddTriggerBinding<string>("PanelPositionChanged", SavePanelPosition);
            AddTriggerBinding<int>("PanelCollapsedSectionsChanged", SavePanelCollapsedSections);
            AddTriggerBinding<int>("PanelSortModeChanged", SavePanelSortMode);
            m_MiniHudEnabledBinding = AddValueBinding(nameof(CwdSettings.MiniHudEnabled), CwdSettings.Instance.MiniHudEnabled);
            m_MiniHudModeBinding = AddValueBinding(nameof(CwdSettings.MiniHudMode), CwdSettings.Instance.MiniHudMode);
            m_MiniHudItemCountBinding = AddValueBinding(nameof(CwdSettings.MiniHudItemCount), CwdSettings.Instance.MiniHudItemCount);
            m_MiniHudScaleBinding = AddValueBinding(nameof(CwdSettings.MiniHudScale), CwdSettings.Instance.MiniHudScale);
            m_MiniHudOrientationBinding = AddValueBinding(nameof(CwdSettings.MiniHudOrientation), CwdSettings.Instance.MiniHudOrientation);
            m_MiniHudPlacementBinding = AddValueBinding(nameof(CwdSettings.MiniHudPlacement), CwdSettings.Instance.MiniHudPlacement);
            m_MiniHudHideZeroBinding = AddValueBinding(nameof(CwdSettings.MiniHudHideZero), CwdSettings.Instance.MiniHudHideZero);
            m_MiniHudPanelStyleBinding = AddValueBinding(nameof(CwdSettings.MiniHudPanelStyle), CwdSettings.Instance.MiniHudPanelStyle);
            m_MiniHudPanelOpacityBinding = AddValueBinding(nameof(CwdSettings.MiniHudPanelOpacity), CwdSettings.Instance.MiniHudPanelOpacity);
            m_MiniHudHorizontalPositionXBinding = AddValueBinding(nameof(CwdSettings.MiniHudHorizontalPositionX), CwdSettings.Instance.MiniHudHorizontalPositionX);
            m_MiniHudHorizontalPositionYBinding = AddValueBinding(nameof(CwdSettings.MiniHudHorizontalPositionY), CwdSettings.Instance.MiniHudHorizontalPositionY);
            m_MiniHudVerticalPositionXBinding = AddValueBinding(nameof(CwdSettings.MiniHudVerticalPositionX), CwdSettings.Instance.MiniHudVerticalPositionX);
            m_MiniHudVerticalPositionYBinding = AddValueBinding(nameof(CwdSettings.MiniHudVerticalPositionY), CwdSettings.Instance.MiniHudVerticalPositionY);
            m_PanelPositionXBinding = AddValueBinding(nameof(CwdSettings.PanelPositionX), CwdSettings.Instance.PanelPositionX);
            m_PanelPositionYBinding = AddValueBinding(nameof(CwdSettings.PanelPositionY), CwdSettings.Instance.PanelPositionY);
            m_PanelCollapsedSectionsMaskBinding = AddValueBinding(nameof(CwdSettings.PanelCollapsedSectionsMask), CwdSettings.Instance.PanelCollapsedSectionsMask);
            m_PanelSortModeBinding = AddValueBinding(nameof(CwdSettings.PanelSortMode), CwdSettings.Instance.PanelSortMode);
            m_PanelButtonsOnlyStartBinding = AddValueBinding(nameof(CwdSettings.PanelButtonsOnlyStart), CwdSettings.Instance.PanelButtonsOnlyStart);
            m_MainPanelOpacityBinding = AddValueBinding(nameof(CwdSettings.MainPanelOpacity), CwdSettings.Instance.MainPanelOpacity);
            m_Preset1SavedBinding = AddValueBinding(nameof(CwdSettings.Preset1Saved), CwdSettings.Instance.Preset1Saved);
            m_Preset2SavedBinding = AddValueBinding(nameof(CwdSettings.Preset2Saved), CwdSettings.Instance.Preset2Saved);
            m_ActivePresetBinding = AddValueBinding(nameof(CwdSettings.ActivePreset), CwdSettings.Instance.ActivePreset);
            AddTriggerBinding<int>("SavePreset", SavePreset);
            AddTriggerBinding<int>("LoadPreset", LoadPreset);
            AddTriggerBinding("ClearActivePreset", ClearActivePreset);
            m_MoneyViewBinding = AddValueBinding(nameof(CwdSettings.MoneyView), CwdSettings.Instance.MoneyView);
            m_MoneyViewModeBinding = AddValueBinding(nameof(CwdSettings.MoneyViewMode), CwdSettings.Instance.MoneyViewMode);
            m_MoneyTooltipModeBinding = AddValueBinding(nameof(CwdSettings.MoneyTooltipMode), CwdSettings.Instance.MoneyTooltipMode);
            m_MoneyTooltipFontScaleBinding = AddValueBinding(nameof(CwdSettings.MoneyTooltipFontScale), CwdSettings.Instance.MoneyTooltipFontScale);
            m_PopulationTooltipFontScaleBinding = AddValueBinding(nameof(CwdSettings.PopulationTooltipFontScale), CwdSettings.Instance.PopulationTooltipFontScale);

            m_ElectricElectricNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityElectricityNotification), CwdSettings.Instance.Notification.ElectricityElectricityNotification, OnElectricityElectricityNotificationToggle);
            m_ElectricBottleneckNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityBottleneckNotification), CwdSettings.Instance.Notification.ElectricityBottleneckNotification, OnElectricityBottleneckNotificationToggle);
            m_ElectricBuildingBottleneckNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityBuildingBottleneckNotification), CwdSettings.Instance.Notification.ElectricityBuildingBottleneckNotification, OnElectricityBuildingBottleneckNotificationToggle);
            m_ElectricNotEnoughProductionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityNotEnoughProductionNotification), CwdSettings.Instance.Notification.ElectricityNotEnoughProductionNotification, OnElectricityNotEnoughProductionNotificationToggle);
            m_ElectricTransformerNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityTransformerNotification), CwdSettings.Instance.Notification.ElectricityTransformerNotification, OnElectricityTransformerNotificationToggle);
            m_ElectricNotEnoughConnectedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityNotEnoughConnectedNotification), CwdSettings.Instance.Notification.ElectricityNotEnoughConnectedNotification, OnElectricityNotEnoughConnectedNotificationToggle);
            m_ElectricBatteryEmptyNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityBatteryEmptyNotification), CwdSettings.Instance.Notification.ElectricityBatteryEmptyNotification, OnElectricityBatteryEmptyNotificationToggle);
            m_ElectricLowVoltageNotConnectedBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityLowVoltageNotConnected), CwdSettings.Instance.Notification.ElectricityLowVoltageNotConnected, OnElectricityLowVoltageNotConnectedToggle);
            m_ElectricHighVoltageNotConnectedBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ElectricityHighVoltageNotConnected), CwdSettings.Instance.Notification.ElectricityHighVoltageNotConnected, OnElectricityHighVoltageNotConnectedToggle);

            m_WaterPipeWaterNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeWaterNotification), CwdSettings.Instance.Notification.WaterPipeWaterNotification, OnWaterPipeWaterNotificationToggle);
            m_WaterPipeDirtyWaterNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeDirtyWaterNotification), CwdSettings.Instance.Notification.WaterPipeDirtyWaterNotification, OnWaterPipeDirtyWaterNotificationToggle);
            m_WaterPipeSewageNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeSewageNotification), CwdSettings.Instance.Notification.WaterPipeSewageNotification, OnWaterPipeSewageNotificationToggle);
            m_WaterPipeWaterPipeNotConnectedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeWaterPipeNotConnectedNotification), CwdSettings.Instance.Notification.WaterPipeWaterPipeNotConnectedNotification, OnWaterPipeWaterPipeNotConnectedNotificationToggle);
            m_WaterPipeSewagePipeNotConnectedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeSewagePipeNotConnectedNotification), CwdSettings.Instance.Notification.WaterPipeSewagePipeNotConnectedNotification, OnWaterPipeSewagePipeNotConnectedNotificationToggle);
            m_WaterPipeNotEnoughWaterCapacityNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeNotEnoughWaterCapacityNotification), CwdSettings.Instance.Notification.WaterPipeNotEnoughWaterCapacityNotification, OnWaterPipeNotEnoughWaterCapacityNotificationToggle);
            m_WaterPipeNotEnoughSewageCapacityNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeNotEnoughSewageCapacityNotification), CwdSettings.Instance.Notification.WaterPipeNotEnoughSewageCapacityNotification, OnWaterPipeNotEnoughSewageCapacityNotificationToggle);
            m_WaterPipeNotEnoughGroundwaterNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeNotEnoughGroundwaterNotification), CwdSettings.Instance.Notification.WaterPipeNotEnoughGroundwaterNotification, OnWaterPipeNotEnoughGroundwaterNotificationToggle);
            m_WaterPipeNotEnoughSurfaceWaterNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeNotEnoughSurfaceWaterNotification), CwdSettings.Instance.Notification.WaterPipeNotEnoughSurfaceWaterNotification, OnWaterPipeNotEnoughSurfaceWaterNotificationToggle);
            m_WaterPipeDirtyWaterPumpNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WaterPipeDirtyWaterPumpNotification), CwdSettings.Instance.Notification.WaterPipeDirtyWaterPumpNotification, OnWaterPipeDirtyWaterPumpNotificationToggle);

            m_BuildingAbandonedCollapsedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.BuildingAbandonedCollapsedNotification), CwdSettings.Instance.Notification.BuildingAbandonedCollapsedNotification, OnBuildingAbandonedCollapsedNotificationToggle);
            m_BuildingAbandonedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.BuildingAbandonedNotification), CwdSettings.Instance.Notification.BuildingAbandonedNotification, OnBuildingAbandonedNotificationToggle);
            m_BuildingCondemnedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.BuildingCondemnedNotification), CwdSettings.Instance.Notification.BuildingCondemnedNotification, OnBuildingCondemnedNotificationToggle);
            m_BuildingTurnedOffNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.BuildingTurnedOffNotification), CwdSettings.Instance.Notification.BuildingTurnedOffNotification, OnBuildingTurnedOffNotificationToggle);
            m_BuildingHighRentNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.BuildingHighRentNotification), CwdSettings.Instance.Notification.BuildingHighRentNotification, OnBuildingHighRentNotificationToggle);
            m_BuildingLevelingNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.BuildingLevelingNotification), CwdSettings.Instance.Notification.BuildingLevelingNotification, OnBuildingLevelingNotificationToggle);

            m_TrafficBottleneckNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficBottleneckNotification), CwdSettings.Instance.Notification.TrafficBottleneckNotification, OnTrafficBottleneckNotificationToggle);
            m_TrafficDeadEndNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficDeadEndNotification), CwdSettings.Instance.Notification.TrafficDeadEndNotification, OnTrafficDeadEndNotificationToggle);
            m_TrafficRoadConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficRoadConnectionNotification), CwdSettings.Instance.Notification.TrafficRoadConnectionNotification, OnTrafficRoadConnectionNotificationToggle);
            m_TrafficTrackConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficTrackConnectionNotification), CwdSettings.Instance.Notification.TrafficTrackConnectionNotification, OnTrafficTrackConnectionNotificationToggle);
            m_TrafficCarConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficCarConnectionNotification), CwdSettings.Instance.Notification.TrafficCarConnectionNotification, OnTrafficCarConnectionNotificationToggle);
            m_TrafficShipConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficShipConnectionNotification), CwdSettings.Instance.Notification.TrafficShipConnectionNotification, OnTrafficShipConnectionNotificationToggle);
            m_TrafficTrainConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficTrainConnectionNotification), CwdSettings.Instance.Notification.TrafficTrainConnectionNotification, OnTrafficTrainConnectionNotificationToggle);
            m_TrafficPedestrianConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficPedestrianConnectionNotification), CwdSettings.Instance.Notification.TrafficPedestrianConnectionNotification, OnTrafficPedestrianConnectionNotificationToggle);
            m_TrafficBicycleConnectionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TrafficBicycleConnectionNotification), CwdSettings.Instance.Notification.TrafficBicycleConnectionNotification, OnTrafficBicycleConnectionNotificationToggle);

            m_CompanyNoInputsNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.CompanyNoInputsNotification), CwdSettings.Instance.Notification.CompanyNoInputsNotification, OnCompanyNoInputsNotificationToggle);
            m_CompanyNoCustomersNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.CompanyNoCustomersNotification), CwdSettings.Instance.Notification.CompanyNoCustomersNotification, OnCompanyNoCustomersNotificationToggle);

            m_WorkProviderUneducatedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WorkProviderUneducatedNotification), CwdSettings.Instance.Notification.WorkProviderUneducatedNotification, OnWorkProviderUneducatedNotificationToggle);
            m_WorkProviderEducatedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.WorkProviderEducatedNotification), CwdSettings.Instance.Notification.WorkProviderEducatedNotification, OnWorkProviderEducatedNotificationToggle);

            m_DisasterWeatherDamageNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.DisasterWeatherDamageNotification), CwdSettings.Instance.Notification.DisasterWeatherDamageNotification, OnDisasterWeatherDamageNotificationToggle);
            m_DisasterWeatherDestroyedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.DisasterWeatherDestroyedNotification), CwdSettings.Instance.Notification.DisasterWeatherDestroyedNotification, OnDisasterWeatherDestroyedNotificationToggle);
            m_DisasterWaterDamageNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.DisasterWaterDamageNotification), CwdSettings.Instance.Notification.DisasterWaterDamageNotification, OnDisasterWaterDamageNotificationToggle);
            m_DisasterWaterDestroyedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.DisasterWaterDestroyedNotification), CwdSettings.Instance.Notification.DisasterWaterDestroyedNotification, OnDisasterWaterDestroyedNotificationToggle);
            m_DisasterDestroyedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.DisasterDestroyedNotification), CwdSettings.Instance.Notification.DisasterDestroyedNotification, OnDisasterDestroyedNotificationToggle);

            m_FireFireNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.FireFireNotification), CwdSettings.Instance.Notification.FireFireNotification, OnFireFireNotificationToggle);
            m_FireBurnedDownNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.FireBurnedDownNotification), CwdSettings.Instance.Notification.FireBurnedDownNotification, OnFireBurnedDownNotificationToggle);

            m_GarbageGarbageNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.GarbageGarbageNotification), CwdSettings.Instance.Notification.GarbageGarbageNotification, OnGarbageGarbageNotificationToggle);
            m_GarbageFacilityFullNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.GarbageFacilityFullNotification), CwdSettings.Instance.Notification.GarbageFacilityFullNotification, OnGarbageFacilityFullNotificationToggle);

            m_HealthcareAmbulanceNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.HealthcareAmbulanceNotification), CwdSettings.Instance.Notification.HealthcareAmbulanceNotification, OnHealthcareAmbulanceNotificationToggle);
            m_HealthcareHearseNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.HealthcareHearseNotification), CwdSettings.Instance.Notification.HealthcareHearseNotification, OnHealthcareHearseNotificationToggle);
            m_HealthcareFacilityFullNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.HealthcareFacilityFullNotification), CwdSettings.Instance.Notification.HealthcareFacilityFullNotification, OnHealthcareFacilityFullNotificationToggle);

            m_PoliceTrafficAccidentNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.PoliceTrafficAccidentNotification), CwdSettings.Instance.Notification.PoliceTrafficAccidentNotification, OnPoliceTrafficAccidentNotificationToggle);
            m_PoliceCrimeSceneNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.PoliceCrimeSceneNotification), CwdSettings.Instance.Notification.PoliceCrimeSceneNotification, OnPoliceCrimeSceneNotificationToggle);

            m_PollutionAirPollutionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.PollutionAirPollutionNotification), CwdSettings.Instance.Notification.PollutionAirPollutionNotification, OnPollutionAirPollutionNotificationToggle);
            m_PollutionNoisePollutionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.PollutionNoisePollutionNotification), CwdSettings.Instance.Notification.PollutionNoisePollutionNotification, OnPollutionNoisePollutionNotificationToggle);
            m_PollutionGroundPollutionNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.PollutionGroundPollutionNotification), CwdSettings.Instance.Notification.PollutionGroundPollutionNotification, OnPollutionGroundPollutionNotificationToggle);

            m_ResourceConsumerNoResourceNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ResourceConsumerNoResourceNotification), CwdSettings.Instance.Notification.ResourceConsumerNoResourceNotification, OnResourceConsumerNoResourceNotificationToggle);
            m_ResourceConsumerNoFuelNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ResourceConsumerNoFuelNotification), CwdSettings.Instance.Notification.ResourceConsumerNoFuelNotification, OnResourceConsumerNoFuelNotificationToggle);
            m_ResourceConnectionWarningNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ResourceConnectionWarningNotification), CwdSettings.Instance.Notification.ResourceConnectionWarningNotification, OnResourceConnectionWarningNotificationToggle);
            m_ResourceConnectionOilPipeNotConnectedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ResourceConnectionOilPipeNotConnectedNotification), CwdSettings.Instance.Notification.ResourceConnectionOilPipeNotConnectedNotification, OnResourceConnectionOilPipeNotConnectedNotificationToggle);
            m_ResourceConnectionFishingPierNotConnectedNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.ResourceConnectionFishingPierNotConnectedNotification), CwdSettings.Instance.Notification.ResourceConnectionFishingPierNotConnectedNotification, OnResourceConnectionFishingPierNotConnectedNotificationToggle);

            m_RoutePathfindNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.RoutePathfindNotification), CwdSettings.Instance.Notification.RoutePathfindNotification, OnRoutePathfindNotificationToggle);
            m_RouteGateBypassNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.RouteGateBypassNotification), CwdSettings.Instance.Notification.RouteGateBypassNotification, OnRouteGateBypassNotificationToggle);

            m_TransportLineVehicleNotificationBinding = AddBoolBindingAndTriggerBinding(nameof(CwdSettings.Instance.Notification.TransportLineVehicleNotification), CwdSettings.Instance.Notification.TransportLineVehicleNotification, OnTransportLineVehicleNotificationToggle);
        }
    }
}
