// File: src/UI/src/bindings/bindings.tsx
// Purpose: UI bindings and triggers shared between C# systems and React components.

import { bindValue, trigger } from "cs2/api";
import mod from "../../mod.json";

export const controlPanelEnabled$ = bindValue<boolean>(mod.id, "ControlPanelEnabled", false);
export const moneyView$ = bindValue<boolean>(mod.id, "MoneyView", true);
export const moneyViewMode$ = bindValue<number>(mod.id, "MoneyViewMode", 1);
export const moneyTooltipMode$ = bindValue<number>(mod.id, "MoneyTooltipMode", 0);

export const moneyTooltipFontScale$ = bindValue<number>(mod.id, "MoneyTooltipFontScale", 120);
export const populationTooltipFontScale$ = bindValue<number>(mod.id, "PopulationTooltipFontScale", 120);
export const disableAllTooltips$ = bindValue<boolean>(mod.id, "DisableAllTooltips", false);
export const disableCwdTooltips$ = bindValue<boolean>(mod.id, "DisableCwdTooltips", false);
export const hideRoadNames$ = bindValue<boolean>(mod.id, "HideRoadNames", false);
export const hideDistrictNames$ = bindValue<boolean>(mod.id, "HideDistrictNames", false);
export const showRoadArrows$ = bindValue<boolean>(mod.id, "ShowRoadArrows", false);
export const notificationCounts$ = bindValue<number[]>(mod.id, "NotificationCounts", []);

export const miniHudFavorites$ = bindValue<number[]>(mod.id, "MiniHudFavorites", []);
export const miniHudEnabled$ = bindValue<boolean>(mod.id, "MiniHudEnabled", true);
export const miniHudOrientation$ = bindValue<number>(mod.id, "MiniHudOrientation", 0);  // horizontal
export const miniHudPlacement$ = bindValue<number>(mod.id, "MiniHudPlacement", 2);      // draggable
export const miniHudMode$ = bindValue<number>(mod.id, "MiniHudMode", 1);                // favorites
export const miniHudItemCount$ = bindValue<number>(mod.id, "MiniHudItemCount", 5);
export const miniHudScale$ = bindValue<number>(mod.id, "MiniHudScale", 100);
export const miniHudHideZero$ = bindValue<boolean>(mod.id, "MiniHudHideZero", true);
export const miniHudPanelStyle$ = bindValue<number>(mod.id, "MiniHudPanelStyle", 0);
export const miniHudPanelOpacity$ = bindValue<number>(mod.id, "MiniHudPanelOpacity", 30);
export const miniHudHorizontalPositionX$ = bindValue<number>(mod.id, "MiniHudHorizontalPositionX", 0);
export const miniHudHorizontalPositionY$ = bindValue<number>(mod.id, "MiniHudHorizontalPositionY", 0);
export const miniHudVerticalPositionX$ = bindValue<number>(mod.id, "MiniHudVerticalPositionX", 0);
export const miniHudVerticalPositionY$ = bindValue<number>(mod.id, "MiniHudVerticalPositionY", 0);

export const panelButtonsOnlyStart$ = bindValue<boolean>(mod.id, "PanelButtonsOnlyStart", false);
export const panelPositionX$ = bindValue<number>(mod.id, "PanelPositionX", 0);
export const panelPositionY$ = bindValue<number>(mod.id, "PanelPositionY", 0);
export const editorQuickControlsEnabled$ = bindValue<boolean>(mod.id, "EditorQuickControlsEnabled", false);
export const editorQuickControlsPositionX$ = bindValue<number>(mod.id, "EditorQuickControlsPositionX", 0);
export const editorQuickControlsPositionY$ = bindValue<number>(mod.id, "EditorQuickControlsPositionY", 0);
export const panelCollapsedSectionsMask$ = bindValue<number>(mod.id, "PanelCollapsedSectionsMask", 0);
export const panelSortMode$ = bindValue<number>(mod.id, "PanelSortMode", 0);
export const mainPanelOpacity$ = bindValue<number>(mod.id, "MainPanelOpacity", 80);

// Notification-checkbox presets: the panel's "1 | 2" split button. Each flag is false until the
// player first saves that slot (an unsaved slot renders dimmed and ignores a load click).
export const preset1Saved$ = bindValue<boolean>(mod.id, "Preset1Saved", false);
export const preset2Saved$ = bindValue<boolean>(mod.id, "Preset2Saved", false);
// Which preset slot is currently applied: 0 = none, 1, or 2. Drives the "selected" ring + dot.
export const activePreset$ = bindValue<number>(mod.id, "ActivePreset", 0);

// Vanilla UI scaling (normally dev-mode only). Title-bar button and CWD Options toggle share this live value.
export const interfaceScaleEnabled$ = bindValue<boolean>(mod.id, "InterfaceScaleEnabled", false);
export const OnToggleInterfaceScale = (enable: boolean) => trigger(mod.id, "InterfaceScaleEnabled", enable);

export const ElectricityElectricityNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityElectricityNotification");
export const ElectricityBottleneckNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityBottleneckNotification");
export const ElectricityBuildingBottleneckNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityBuildingBottleneckNotification");
export const ElectricityNotEnoughProductionNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityNotEnoughProductionNotification");
export const ElectricityTransformerNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityTransformerNotification");
export const ElectricityNotEnoughConnectedNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityNotEnoughConnectedNotification");
export const ElectricityBatteryEmptyNotificationBinding$ = bindValue<boolean>(mod.id, "ElectricityBatteryEmptyNotification");
export const ElectricityLowVoltageNotConnectedBinding$ = bindValue<boolean>(mod.id, "ElectricityLowVoltageNotConnected");
export const ElectricityHighVoltageNotConnectedBinding$ = bindValue<boolean>(mod.id, "ElectricityHighVoltageNotConnected");
export const WaterPipeWaterNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeWaterNotification");
export const WaterPipeDirtyWaterNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeDirtyWaterNotification");
export const WaterPipeSewageNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeSewageNotification");
export const WaterPipeWaterPipeNotConnectedNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeWaterPipeNotConnectedNotification");
export const WaterPipeSewagePipeNotConnectedNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeSewagePipeNotConnectedNotification");
export const WaterPipeNotEnoughWaterCapacityNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeNotEnoughWaterCapacityNotification");
export const WaterPipeNotEnoughSewageCapacityNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeNotEnoughSewageCapacityNotification");
export const WaterPipeNotEnoughGroundwaterNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeNotEnoughGroundwaterNotification");
export const WaterPipeNotEnoughSurfaceWaterNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeNotEnoughSurfaceWaterNotification");
export const WaterPipeDirtyWaterPumpNotificationBinding$ = bindValue<boolean>(mod.id, "WaterPipeDirtyWaterPumpNotification");
export const BuildingAbandonedCollapsedNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingAbandonedCollapsedNotification");
export const BuildingAbandonedNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingAbandonedNotification");
export const BuildingCondemnedNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingCondemnedNotification");
export const BuildingTurnedOffNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingTurnedOffNotification");
export const BuildingHighRentNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingHighRentNotification");
export const BuildingLevelingNotificationBinding$ = bindValue<boolean>(mod.id, "BuildingLevelingNotification");
export const TrafficBottleneckNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficBottleneckNotification");
export const TrafficDeadEndNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficDeadEndNotification");
export const TrafficRoadConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficRoadConnectionNotification");
export const TrafficTrackConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficTrackConnectionNotification");
export const TrafficCarConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficCarConnectionNotification");
export const TrafficShipConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficShipConnectionNotification");
export const TrafficTrainConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficTrainConnectionNotification");
export const TrafficPedestrianConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficPedestrianConnectionNotification");
export const TrafficBicycleConnectionNotificationBinding$ = bindValue<boolean>(mod.id, "TrafficBicycleConnectionNotification");
export const CompanyNoInputsNotificationBinding$ = bindValue<boolean>(mod.id, "CompanyNoInputsNotification");
export const CompanyNoCustomersNotificationBinding$ = bindValue<boolean>(mod.id, "CompanyNoCustomersNotification");
export const WorkProviderUneducatedNotificationBinding$ = bindValue<boolean>(mod.id, "WorkProviderUneducatedNotification");
export const WorkProviderEducatedNotificationBinding$ = bindValue<boolean>(mod.id, "WorkProviderEducatedNotification");
export const DisasterWeatherDamageNotificationBinding$ = bindValue<boolean>(mod.id, "DisasterWeatherDamageNotification");
export const DisasterWeatherDestroyedNotificationBinding$ = bindValue<boolean>(mod.id, "DisasterWeatherDestroyedNotification");
export const DisasterWaterDamageNotificationBinding$ = bindValue<boolean>(mod.id, "DisasterWaterDamageNotification");
export const DisasterWaterDestroyedNotificationBinding$ = bindValue<boolean>(mod.id, "DisasterWaterDestroyedNotification");
export const DisasterDestroyedNotificationBinding$ = bindValue<boolean>(mod.id, "DisasterDestroyedNotification");
export const FireFireNotificationBinding$ = bindValue<boolean>(mod.id, "FireFireNotification");
export const FireBurnedDownNotificationBinding$ = bindValue<boolean>(mod.id, "FireBurnedDownNotification");
export const GarbageGarbageNotificationBinding$ = bindValue<boolean>(mod.id, "GarbageGarbageNotification");
export const GarbageFacilityFullNotificationBinding$ = bindValue<boolean>(mod.id, "GarbageFacilityFullNotification");
export const HealthcareAmbulanceNotificationBinding$ = bindValue<boolean>(mod.id, "HealthcareAmbulanceNotification");
export const HealthcareHearseNotificationBinding$ = bindValue<boolean>(mod.id, "HealthcareHearseNotification");
export const HealthcareFacilityFullNotificationBinding$ = bindValue<boolean>(mod.id, "HealthcareFacilityFullNotification");
export const PoliceTrafficAccidentNotificationBinding$ = bindValue<boolean>(mod.id, "PoliceTrafficAccidentNotification");
export const PoliceCrimeSceneNotificationBinding$ = bindValue<boolean>(mod.id, "PoliceCrimeSceneNotification");
export const PollutionAirPollutionNotificationBinding$ = bindValue<boolean>(mod.id, "PollutionAirPollutionNotification");
export const PollutionNoisePollutionNotificationBinding$ = bindValue<boolean>(mod.id, "PollutionNoisePollutionNotification");
export const PollutionGroundPollutionNotificationBinding$ = bindValue<boolean>(mod.id, "PollutionGroundPollutionNotification");
export const ResourceConsumerNoResourceNotificationBinding$ = bindValue<boolean>(mod.id, "ResourceConsumerNoResourceNotification");
export const ResourceConsumerNoFuelNotificationBinding$ = bindValue<boolean>(mod.id, "ResourceConsumerNoFuelNotification");
export const ResourceConnectionWarningNotificationBinding$ = bindValue<boolean>(mod.id, "ResourceConnectionWarningNotification");
export const ResourceConnectionOilPipeNotConnectedNotificationBinding$ = bindValue<boolean>(mod.id, "ResourceConnectionOilPipeNotConnectedNotification");
export const ResourceConnectionFishingPierNotConnectedNotificationBinding$ = bindValue<boolean>(mod.id, "ResourceConnectionFishingPierNotConnectedNotification");
export const RoutePathfindNotificationBinding$ = bindValue<boolean>(mod.id, "RoutePathfindNotification");
export const RouteGateBypassNotificationBinding$ = bindValue<boolean>(mod.id, "RouteGateBypassNotification");
export const TransportLineVehicleNotificationBinding$ = bindValue<boolean>(mod.id, "TransportLineVehicleNotification");

export const OnControlPanelBindingToggle = (enable: boolean) => trigger(mod.id, "ControlPanelEnabled", enable);
export const OnToggleAllNotifications = (enable: boolean) => trigger(mod.id, "ToggleAllNotifications", enable);
// Presets: click a slot to load it, hold a slot to save the current checkboxes into it.
export const OnLoadPreset = (slot: number) => trigger(mod.id, "LoadPreset", slot);
export const OnSavePreset = (slot: number) => trigger(mod.id, "SavePreset", slot);
// Clears the "selected" preset ring once the live layout diverges from a loaded preset (manual checkbox change).
export const OnClearActivePreset = () => trigger(mod.id, "ClearActivePreset");
export const OnDisableAllTooltipsToggle = (disable: boolean) => trigger(mod.id, "DisableAllTooltips", disable);
export const OnDisableCwdTooltipsToggle = (disable: boolean) => trigger(mod.id, "DisableCwdTooltips", disable);
export const OnHideRoadNamesToggle = (hide: boolean) => trigger(mod.id, "HideRoadNames", hide);
export const OnHideDistrictNamesToggle = (hide: boolean) => trigger(mod.id, "HideDistrictNames", hide);
export const OnShowRoadArrowsToggle = (show: boolean) => trigger(mod.id, "ShowRoadArrows", show);
export const OnToggleMiniHudFavorite = (index: number) => trigger(mod.id, "ToggleMiniHudFavorite", index);
export const OnMiniHudNotificationClicked = (index: number) => trigger(mod.id, "MiniHudNotificationClicked", index);
export const OnMiniHudPositionChanged = (orientation: number, x: number, y: number) => trigger(mod.id, "MiniHudPositionChanged", `${orientation},${Math.round(x)},${Math.round(y)}`);
export const OnPanelPositionChanged = (x: number, y: number) => trigger(mod.id, "PanelPositionChanged", `${Math.round(x)},${Math.round(y)}`);
export const OnEditorQuickControlsEnabledChanged = (enabled: boolean) => trigger(mod.id, "EditorQuickControlsEnabled", enabled);
export const OnEditorQuickControlsPositionChanged = (x: number, y: number) => trigger(mod.id, "EditorQuickControlsPositionChanged", `${Math.round(x)},${Math.round(y)}`);
export const OnPanelCollapsedSectionsChanged = (mask: number) => trigger(mod.id, "PanelCollapsedSectionsChanged", mask);
export const OnPanelSortModeChanged = (mode: number) => trigger(mod.id, "PanelSortModeChanged", mode);

export const OnElectricityElectricityNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityElectricityNotification", enable);
export const OnElectricityBottleneckNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityBottleneckNotification", enable);
export const OnElectricityBuildingBottleneckNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityBuildingBottleneckNotification", enable);
export const OnElectricityNotEnoughProductionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityNotEnoughProductionNotification", enable);
export const OnElectricityTransformerNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityTransformerNotification", enable);
export const OnElectricityNotEnoughConnectedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityNotEnoughConnectedNotification", enable);
export const OnElectricityBatteryEmptyNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityBatteryEmptyNotification", enable);
export const OnElectricityLowVoltageNotConnectedBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityLowVoltageNotConnected", enable);
export const OnElectricityHighVoltageNotConnectedBindingToggle = (enable: boolean) => trigger(mod.id, "ElectricityHighVoltageNotConnected", enable);
export const OnWaterPipeWaterNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeWaterNotification", enable);
export const OnWaterPipeDirtyWaterNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeDirtyWaterNotification", enable);
export const OnWaterPipeSewageNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeSewageNotification", enable);
export const OnWaterPipeWaterPipeNotConnectedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeWaterPipeNotConnectedNotification", enable);
export const OnWaterPipeSewagePipeNotConnectedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeSewagePipeNotConnectedNotification", enable);
export const OnWaterPipeNotEnoughWaterCapacityNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeNotEnoughWaterCapacityNotification", enable);
export const OnWaterPipeNotEnoughSewageCapacityNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeNotEnoughSewageCapacityNotification", enable);
export const OnWaterPipeNotEnoughGroundwaterNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeNotEnoughGroundwaterNotification", enable);
export const OnWaterPipeNotEnoughSurfaceWaterNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeNotEnoughSurfaceWaterNotification", enable);
export const OnWaterPipeDirtyWaterPumpNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WaterPipeDirtyWaterPumpNotification", enable);
export const OnBuildingAbandonedCollapsedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingAbandonedCollapsedNotification", enable);
export const OnBuildingAbandonedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingAbandonedNotification", enable);
export const OnBuildingCondemnedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingCondemnedNotification", enable);
export const OnBuildingTurnedOffNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingTurnedOffNotification", enable);
export const OnBuildingHighRentNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingHighRentNotification", enable);
export const OnBuildingLevelingNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "BuildingLevelingNotification", enable);
export const OnTrafficBottleneckNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficBottleneckNotification", enable);
export const OnTrafficDeadEndNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficDeadEndNotification", enable);
export const OnTrafficRoadConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficRoadConnectionNotification", enable);
export const OnTrafficTrackConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficTrackConnectionNotification", enable);
export const OnTrafficCarConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficCarConnectionNotification", enable);
export const OnTrafficShipConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficShipConnectionNotification", enable);
export const OnTrafficTrainConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficTrainConnectionNotification", enable);
export const OnTrafficPedestrianConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficPedestrianConnectionNotification", enable);
export const OnTrafficBicycleConnectionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TrafficBicycleConnectionNotification", enable);
export const OnCompanyNoInputsNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "CompanyNoInputsNotification", enable);
export const OnCompanyNoCustomersNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "CompanyNoCustomersNotification", enable);
export const OnWorkProviderUneducatedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WorkProviderUneducatedNotification", enable);
export const OnWorkProviderEducatedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "WorkProviderEducatedNotification", enable);
export const OnDisasterWeatherDamageNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "DisasterWeatherDamageNotification", enable);
export const OnDisasterWeatherDestroyedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "DisasterWeatherDestroyedNotification", enable);
export const OnDisasterWaterDamageNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "DisasterWaterDamageNotification", enable);
export const OnDisasterWaterDestroyedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "DisasterWaterDestroyedNotification", enable);
export const OnDisasterDestroyedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "DisasterDestroyedNotification", enable);
export const OnFireFireNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "FireFireNotification", enable);
export const OnFireBurnedDownNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "FireBurnedDownNotification", enable);
export const OnGarbageGarbageNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "GarbageGarbageNotification", enable);
export const OnGarbageFacilityFullNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "GarbageFacilityFullNotification", enable);
export const OnHealthcareAmbulanceNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "HealthcareAmbulanceNotification", enable);
export const OnHealthcareHearseNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "HealthcareHearseNotification", enable);
export const OnHealthcareFacilityFullNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "HealthcareFacilityFullNotification", enable);
export const OnPoliceTrafficAccidentNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "PoliceTrafficAccidentNotification", enable);
export const OnPoliceCrimeSceneNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "PoliceCrimeSceneNotification", enable);
export const OnPollutionAirPollutionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "PollutionAirPollutionNotification", enable);
export const OnPollutionNoisePollutionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "PollutionNoisePollutionNotification", enable);
export const OnPollutionGroundPollutionNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "PollutionGroundPollutionNotification", enable);
export const OnResourceConsumerNoResourceNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ResourceConsumerNoResourceNotification", enable);
export const OnResourceConsumerNoFuelNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ResourceConsumerNoFuelNotification", enable);
export const OnResourceConnectionWarningNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ResourceConnectionWarningNotification", enable);
export const OnResourceConnectionOilPipeNotConnectedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ResourceConnectionOilPipeNotConnectedNotification", enable);
export const OnResourceConnectionFishingPierNotConnectedNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "ResourceConnectionFishingPierNotConnectedNotification", enable);
export const OnRoutePathfindNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "RoutePathfindNotification", enable);
export const OnRouteGateBypassNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "RouteGateBypassNotification", enable);
export const OnTransportLineVehicleNotificationBindingToggle = (enable: boolean) => trigger(mod.id, "TransportLineVehicleNotification", enable);
