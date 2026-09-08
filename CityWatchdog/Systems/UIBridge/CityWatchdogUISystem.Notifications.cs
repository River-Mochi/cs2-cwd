// <copyright file="CityWatchdogUISystem.Notifications.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/CityWatchdogUISystem.Notifications.cs
// Purpose: Notification checkbox bindings, bulk Show/Hide, and saved preset handling.

namespace CityWatchdog.Systems
{
    using System;
    using CityWatchdog.Alerts;
    using CS2Shared.RiverMochi;

    public partial class CityWatchdogUISystem
    {
        private BoolBinding m_ElectricElectricNotificationBinding = null!;
        private BoolBinding m_ElectricBottleneckNotificationBinding = null!;
        private BoolBinding m_ElectricBuildingBottleneckNotificationBinding = null!;
        private BoolBinding m_ElectricNotEnoughProductionNotificationBinding = null!;
        private BoolBinding m_ElectricTransformerNotificationBinding = null!;
        private BoolBinding m_ElectricNotEnoughConnectedNotificationBinding = null!;
        private BoolBinding m_ElectricBatteryEmptyNotificationBinding = null!;
        private BoolBinding m_ElectricLowVoltageNotConnectedBinding = null!;
        private BoolBinding m_ElectricHighVoltageNotConnectedBinding = null!;

        private BoolBinding m_WaterPipeWaterNotificationBinding = null!;
        private BoolBinding m_WaterPipeDirtyWaterNotificationBinding = null!;
        private BoolBinding m_WaterPipeSewageNotificationBinding = null!;
        private BoolBinding m_WaterPipeWaterPipeNotConnectedNotificationBinding = null!;
        private BoolBinding m_WaterPipeSewagePipeNotConnectedNotificationBinding = null!;
        private BoolBinding m_WaterPipeNotEnoughWaterCapacityNotificationBinding = null!;
        private BoolBinding m_WaterPipeNotEnoughSewageCapacityNotificationBinding = null!;
        private BoolBinding m_WaterPipeNotEnoughGroundwaterNotificationBinding = null!;
        private BoolBinding m_WaterPipeNotEnoughSurfaceWaterNotificationBinding = null!;
        private BoolBinding m_WaterPipeDirtyWaterPumpNotificationBinding = null!;

        private BoolBinding m_BuildingAbandonedCollapsedNotificationBinding = null!;
        private BoolBinding m_BuildingAbandonedNotificationBinding = null!;
        private BoolBinding m_BuildingCondemnedNotificationBinding = null!;
        private BoolBinding m_BuildingTurnedOffNotificationBinding = null!;
        private BoolBinding m_BuildingHighRentNotificationBinding = null!;
        private BoolBinding m_BuildingLevelingNotificationBinding = null!;

        private BoolBinding m_TrafficBottleneckNotificationBinding = null!;
        private BoolBinding m_TrafficDeadEndNotificationBinding = null!;
        private BoolBinding m_TrafficRoadConnectionNotificationBinding = null!;
        private BoolBinding m_TrafficTrackConnectionNotificationBinding = null!;
        private BoolBinding m_TrafficCarConnectionNotificationBinding = null!;
        private BoolBinding m_TrafficShipConnectionNotificationBinding = null!;
        private BoolBinding m_TrafficTrainConnectionNotificationBinding = null!;
        private BoolBinding m_TrafficPedestrianConnectionNotificationBinding = null!;
        private BoolBinding m_TrafficBicycleConnectionNotificationBinding = null!;

        private BoolBinding m_CompanyNoInputsNotificationBinding = null!;
        private BoolBinding m_CompanyNoCustomersNotificationBinding = null!;

        private BoolBinding m_WorkProviderUneducatedNotificationBinding = null!;
        private BoolBinding m_WorkProviderEducatedNotificationBinding = null!;

        private BoolBinding m_DisasterWeatherDamageNotificationBinding = null!;
        private BoolBinding m_DisasterWeatherDestroyedNotificationBinding = null!;
        private BoolBinding m_DisasterWaterDamageNotificationBinding = null!;
        private BoolBinding m_DisasterWaterDestroyedNotificationBinding = null!;
        private BoolBinding m_DisasterDestroyedNotificationBinding = null!;

        private BoolBinding m_FireFireNotificationBinding = null!;
        private BoolBinding m_FireBurnedDownNotificationBinding = null!;

        private BoolBinding m_GarbageGarbageNotificationBinding = null!;
        private BoolBinding m_GarbageFacilityFullNotificationBinding = null!;

        private BoolBinding m_HealthcareAmbulanceNotificationBinding = null!;
        private BoolBinding m_HealthcareHearseNotificationBinding = null!;
        private BoolBinding m_HealthcareFacilityFullNotificationBinding = null!;

        private BoolBinding m_PoliceTrafficAccidentNotificationBinding = null!;
        private BoolBinding m_PoliceCrimeSceneNotificationBinding = null!;

        private BoolBinding m_PollutionAirPollutionNotificationBinding = null!;
        private BoolBinding m_PollutionNoisePollutionNotificationBinding = null!;
        private BoolBinding m_PollutionGroundPollutionNotificationBinding = null!;

        private BoolBinding m_ResourceConsumerNoResourceNotificationBinding = null!;
        private BoolBinding m_ResourceConsumerNoFuelNotificationBinding = null!;
        private BoolBinding m_ResourceConnectionWarningNotificationBinding = null!;
        private BoolBinding m_ResourceConnectionOilPipeNotConnectedNotificationBinding = null!;
        private BoolBinding m_ResourceConnectionFishingPierNotConnectedNotificationBinding = null!;

        private BoolBinding m_RoutePathfindNotificationBinding = null!;
        private BoolBinding m_RouteGateBypassNotificationBinding = null!;

        private BoolBinding m_TransportLineVehicleNotificationBinding = null!;

        #region OnElectricityNotificationToggle
        private void OnElectricityElectricityNotificationToggle(bool value) {
            m_ElectricElectricNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityElectricityNotification = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.ElectricityNotification, value, true);
        }
        private void OnElectricityBottleneckNotificationToggle(bool value) {
            m_ElectricBottleneckNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityBottleneckNotification = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.BottleneckNotification, value, true);
        }
        private void OnElectricityBuildingBottleneckNotificationToggle(bool value) {
            m_ElectricBuildingBottleneckNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityBuildingBottleneckNotification = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.BuildingBottleneckNotification, value, true);
        }
        private void OnElectricityNotEnoughProductionNotificationToggle(bool value) {
            m_ElectricNotEnoughProductionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityNotEnoughProductionNotification = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.NotEnoughProductionNotification, value, true);
        }
        private void OnElectricityTransformerNotificationToggle(bool value) {
            m_ElectricTransformerNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityTransformerNotification = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.TransformerNotification, value, true);
        }
        private void OnElectricityNotEnoughConnectedNotificationToggle(bool value) {
            m_ElectricNotEnoughConnectedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityNotEnoughConnectedNotification = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.NotEnoughConnectedNotification, value, true);
        }
        private void OnElectricityBatteryEmptyNotificationToggle(bool value) {
            m_ElectricBatteryEmptyNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityBatteryEmptyNotification = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.BatteryEmptyNotification, value, true);
        }
        private void OnElectricityLowVoltageNotConnectedToggle(bool value) {
            m_ElectricLowVoltageNotConnectedBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityLowVoltageNotConnected = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.LowVoltageNotConnected, value, true);
        }
        private void OnElectricityHighVoltageNotConnectedToggle(bool value) {
            m_ElectricHighVoltageNotConnectedBinding.Update(value);
            CwdSettings.Instance.Notification.ElectricityHighVoltageNotConnected = value;
            m_AlertIconSystem.EnableElectricityNotification(ElectricityNotificationIcon.HighVoltageNotConnected, value, true);
        }

        #endregion

        #region OnWaterPipeNotificationToggle
        private void OnWaterPipeWaterNotificationToggle(bool value) {
            m_WaterPipeWaterNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeWaterNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.WaterNotification, value, true);
        }
        private void OnWaterPipeDirtyWaterNotificationToggle(bool value) {
            m_WaterPipeDirtyWaterNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeDirtyWaterNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.DirtyWaterNotification, value, true);
        }
        private void OnWaterPipeSewageNotificationToggle(bool value) {
            m_WaterPipeSewageNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeSewageNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.SewageNotification, value, true);
        }
        private void OnWaterPipeWaterPipeNotConnectedNotificationToggle(bool value) {
            m_WaterPipeWaterPipeNotConnectedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeWaterPipeNotConnectedNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.WaterPipeNotConnectedNotification, value, true);
        }
        private void OnWaterPipeSewagePipeNotConnectedNotificationToggle(bool value) {
            m_WaterPipeSewagePipeNotConnectedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeSewagePipeNotConnectedNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.SewagePipeNotConnectedNotification, value, true);
        }
        private void OnWaterPipeNotEnoughWaterCapacityNotificationToggle(bool value) {
            m_WaterPipeNotEnoughWaterCapacityNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeNotEnoughWaterCapacityNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughWaterCapacityNotification, value, true);
        }
        private void OnWaterPipeNotEnoughSewageCapacityNotificationToggle(bool value) {
            m_WaterPipeNotEnoughSewageCapacityNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeNotEnoughSewageCapacityNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughSewageCapacityNotification, value, true);
        }
        private void OnWaterPipeNotEnoughGroundwaterNotificationToggle(bool value) {
            m_WaterPipeNotEnoughGroundwaterNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeNotEnoughGroundwaterNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughGroundwaterNotification, value, true);
        }
        private void OnWaterPipeNotEnoughSurfaceWaterNotificationToggle(bool value) {
            m_WaterPipeNotEnoughSurfaceWaterNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeNotEnoughSurfaceWaterNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.NotEnoughSurfaceWaterNotification, value, true);
        }
        private void OnWaterPipeDirtyWaterPumpNotificationToggle(bool value) {
            m_WaterPipeDirtyWaterPumpNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WaterPipeDirtyWaterPumpNotification = value;
            m_AlertIconSystem.EnableWaterPipeNotification(WaterPipeNotificationIcon.DirtyWaterPumpNotification, value, true);
        }
        #endregion

        #region OnBuildingNotificationToggle
        private void OnBuildingAbandonedCollapsedNotificationToggle(bool value) {
            m_BuildingAbandonedCollapsedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.BuildingAbandonedCollapsedNotification = value;
            m_AlertIconSystem.EnableBuildingNotification(BuildingNotificationIcon.AbandonedCollapsedNotification, value, true);
        }
        private void OnBuildingAbandonedNotificationToggle(bool value) {
            m_BuildingAbandonedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.BuildingAbandonedNotification = value;
            m_AlertIconSystem.EnableBuildingNotification(BuildingNotificationIcon.AbandonedNotification, value, true);
        }
        private void OnBuildingCondemnedNotificationToggle(bool value) {
            m_BuildingCondemnedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.BuildingCondemnedNotification = value;
            m_AlertIconSystem.EnableBuildingNotification(BuildingNotificationIcon.CondemnedNotification, value, true);
        }
        private void OnBuildingTurnedOffNotificationToggle(bool value) {
            m_BuildingTurnedOffNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.BuildingTurnedOffNotification = value;
            m_AlertIconSystem.EnableBuildingNotification(BuildingNotificationIcon.TurnedOffNotification, value, true);
        }
        private void OnBuildingHighRentNotificationToggle(bool value) {
            m_BuildingHighRentNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.BuildingHighRentNotification = value;
            m_AlertIconSystem.EnableBuildingNotification(BuildingNotificationIcon.HighRentNotification, value, true);
        }
        private void OnBuildingLevelingNotificationToggle(bool value) {
            m_BuildingLevelingNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.BuildingLevelingNotification = value;
            m_AlertIconSystem.EnableBuildingNotification(BuildingNotificationIcon.LevelingNotification, value, true);
        }
        #endregion

        #region OnTrafficNotificationToggle
        private void OnTrafficBottleneckNotificationToggle(bool value) {
            m_TrafficBottleneckNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficBottleneckNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.BottleneckNotification, value, true);
        }
        private void OnTrafficDeadEndNotificationToggle(bool value) {
            m_TrafficDeadEndNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficDeadEndNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.DeadEndNotification, value, true);
        }
        private void OnTrafficRoadConnectionNotificationToggle(bool value) {
            m_TrafficRoadConnectionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficRoadConnectionNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.RoadConnectionNotification, value, true);
        }
        private void OnTrafficTrackConnectionNotificationToggle(bool value) {
            m_TrafficTrackConnectionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficTrackConnectionNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.TrackConnectionNotification, value, true);
        }
        private void OnTrafficCarConnectionNotificationToggle(bool value) {
            m_TrafficCarConnectionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficCarConnectionNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.CarConnectionNotification, value, true);
        }
        private void OnTrafficShipConnectionNotificationToggle(bool value) {
            m_TrafficShipConnectionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficShipConnectionNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.ShipConnectionNotification, value, true);
        }
        private void OnTrafficTrainConnectionNotificationToggle(bool value) {
            m_TrafficTrainConnectionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficTrainConnectionNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.TrainConnectionNotification, value, true);
        }
        private void OnTrafficPedestrianConnectionNotificationToggle(bool value) {
            m_TrafficPedestrianConnectionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficPedestrianConnectionNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.PedestrianConnectionNotification, value, true);
        }
        private void OnTrafficBicycleConnectionNotificationToggle(bool value) {
            m_TrafficBicycleConnectionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TrafficBicycleConnectionNotification = value;
            m_AlertIconSystem.EnableTrafficNotification(TrafficNotificationIcon.BicycleConnectionNotification, value, true);
        }
        #endregion
        #region OnCompanyNotificationToggle
        private void OnCompanyNoInputsNotificationToggle(bool value) {
            m_CompanyNoInputsNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.CompanyNoInputsNotification = value;
            m_AlertIconSystem.EnableCompanyNotification(CompanyNotificationIcon.NoInputsNotification, value, true);
        }
        private void OnCompanyNoCustomersNotificationToggle(bool value) {
            m_CompanyNoCustomersNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.CompanyNoCustomersNotification = value;
            m_AlertIconSystem.EnableCompanyNotification(CompanyNotificationIcon.NoCustomersNotification, value, true);
        }
        #endregion

        #region OnWorkProviderNotificationToggle
        private void OnWorkProviderUneducatedNotificationToggle(bool value) {
            m_WorkProviderUneducatedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WorkProviderUneducatedNotification = value;
            m_AlertIconSystem.EnableWorkProviderNotification(WorkProviderNotificationIcon.UneducatedNotification, value, true);
        }
        private void OnWorkProviderEducatedNotificationToggle(bool value) {
            m_WorkProviderEducatedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.WorkProviderEducatedNotification = value;
            m_AlertIconSystem.EnableWorkProviderNotification(WorkProviderNotificationIcon.EducatedNotification, value, true);
        }
        #endregion

        #region OnDisasterNotificationToggle
        private void OnDisasterWeatherDamageNotificationToggle(bool value) {
            m_DisasterWeatherDamageNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.DisasterWeatherDamageNotification = value;
            m_AlertIconSystem.EnableDisasterNotification(DisasterNotificationIcon.WeatherDamageNotification, value, true);
        }
        private void OnDisasterWeatherDestroyedNotificationToggle(bool value) {
            m_DisasterWeatherDestroyedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.DisasterWeatherDestroyedNotification = value;
            m_AlertIconSystem.EnableDisasterNotification(DisasterNotificationIcon.WeatherDestroyedNotification, value, true);
        }
        private void OnDisasterWaterDamageNotificationToggle(bool value) {
            m_DisasterWaterDamageNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.DisasterWaterDamageNotification = value;
            m_AlertIconSystem.EnableDisasterNotification(DisasterNotificationIcon.WaterDamageNotification, value, true);
        }
        private void OnDisasterWaterDestroyedNotificationToggle(bool value) {
            m_DisasterWaterDestroyedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.DisasterWaterDestroyedNotification = value;
            m_AlertIconSystem.EnableDisasterNotification(DisasterNotificationIcon.WaterDestroyedNotification, value, true);
        }
        private void OnDisasterDestroyedNotificationToggle(bool value) {
            m_DisasterDestroyedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.DisasterDestroyedNotification = value;
            m_AlertIconSystem.EnableDisasterNotification(DisasterNotificationIcon.DestroyedNotification, value, true);
        }
        #endregion

        #region OnFireNotificationToggle
        private void OnFireFireNotificationToggle(bool value) {
            m_FireFireNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.FireFireNotification = value;
            m_AlertIconSystem.EnableFireNotification(FireNotificationIcon.FireNotification, value, true);
        }
        private void OnFireBurnedDownNotificationToggle(bool value) {
            m_FireBurnedDownNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.FireBurnedDownNotification = value;
            m_AlertIconSystem.EnableFireNotification(FireNotificationIcon.BurnedDownNotification, value, true);
        }
        #endregion

        #region OnGarbageNotificationToggle
        private void OnGarbageGarbageNotificationToggle(bool value) {
            m_GarbageGarbageNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.GarbageGarbageNotification = value;
            m_AlertIconSystem.EnableGarbageNotification(GarbageNotificationIcon.GarbageNotification, value, true);
        }
        private void OnGarbageFacilityFullNotificationToggle(bool value) {
            m_GarbageFacilityFullNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.GarbageFacilityFullNotification = value;
            m_AlertIconSystem.EnableGarbageNotification(GarbageNotificationIcon.FacilityFullNotification, value, true);
        }
        #endregion

        #region OnHealthcareNotificationToggle
        private void OnHealthcareAmbulanceNotificationToggle(bool value) {
            m_HealthcareAmbulanceNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.HealthcareAmbulanceNotification = value;
            m_AlertIconSystem.EnableHealthcareNotification(HealthcareNotificationIcon.AmbulanceNotification, value, true);
        }
        private void OnHealthcareHearseNotificationToggle(bool value) {
            m_HealthcareHearseNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.HealthcareHearseNotification = value;
            m_AlertIconSystem.EnableHealthcareNotification(HealthcareNotificationIcon.HearseNotification, value, true);
        }
        private void OnHealthcareFacilityFullNotificationToggle(bool value) {
            m_HealthcareFacilityFullNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.HealthcareFacilityFullNotification = value;
            m_AlertIconSystem.EnableHealthcareNotification(HealthcareNotificationIcon.FacilityFullNotification, value, true);
        }
        #endregion

        #region OnPoliceNotificationToggle
        private void OnPoliceTrafficAccidentNotificationToggle(bool value) {
            m_PoliceTrafficAccidentNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.PoliceTrafficAccidentNotification = value;
            m_AlertIconSystem.EnablePoliceNotification(PoliceNotificationIcon.TrafficAccidentNotification, value, true);
        }
        private void OnPoliceCrimeSceneNotificationToggle(bool value) {
            m_PoliceCrimeSceneNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.PoliceCrimeSceneNotification = value;
            m_AlertIconSystem.EnablePoliceNotification(PoliceNotificationIcon.CrimeSceneNotification, value, true);
        }
        #endregion

        #region OnPollutionNotificationToggle
        private void OnPollutionAirPollutionNotificationToggle(bool value) {
            m_PollutionAirPollutionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.PollutionAirPollutionNotification = value;
            m_AlertIconSystem.EnablePollutionNotification(PollutionNotificationIcon.AirPollutionNotification, value, true);
        }
        private void OnPollutionNoisePollutionNotificationToggle(bool value) {
            m_PollutionNoisePollutionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.PollutionNoisePollutionNotification = value;
            m_AlertIconSystem.EnablePollutionNotification(PollutionNotificationIcon.NoisePollutionNotification, value, true);
        }
        private void OnPollutionGroundPollutionNotificationToggle(bool value) {
            m_PollutionGroundPollutionNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.PollutionGroundPollutionNotification = value;
            m_AlertIconSystem.EnablePollutionNotification(PollutionNotificationIcon.GroundPollutionNotification, value, true);
        }
        #endregion

        #region OnResourceConsumerNotificationToggle
        private void OnResourceConsumerNoResourceNotificationToggle(bool value) {
            m_ResourceConsumerNoResourceNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ResourceConsumerNoResourceNotification = value;
            m_AlertIconSystem.EnableResourceConsumerNotification(ResourceConsumerNotificationIcon.NoResourceNotification, value, true);
        }

        private void OnResourceConsumerNoFuelNotificationToggle(bool value) {
            m_ResourceConsumerNoFuelNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ResourceConsumerNoFuelNotification = value;
            m_AlertIconSystem.EnableResourceConsumerNotification(ResourceConsumerNotificationIcon.NoFuelNotification, value, true);
        }
        #endregion

        #region OnResourceConnectionNotificationToggle
        private void OnResourceConnectionWarningNotificationToggle(bool value) {
            m_ResourceConnectionWarningNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ResourceConnectionWarningNotification = value;
            m_AlertIconSystem.EnableResourceConnectionNotification(ResourceConnectionNotificationIcon.ConnectionWarningNotification, value, true);
        }

        private void OnResourceConnectionOilPipeNotConnectedNotificationToggle(bool value) {
            m_ResourceConnectionOilPipeNotConnectedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ResourceConnectionOilPipeNotConnectedNotification = value;
            m_AlertIconSystem.EnableResourceConnectionNotification(ResourceConnectionNotificationIcon.OilPipeNotConnectedNotification, value, true);
        }

        private void OnResourceConnectionFishingPierNotConnectedNotificationToggle(bool value) {
            m_ResourceConnectionFishingPierNotConnectedNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.ResourceConnectionFishingPierNotConnectedNotification = value;
            m_AlertIconSystem.EnableResourceConnectionNotification(ResourceConnectionNotificationIcon.FishingPierNotConnectedNotification, value, true);
        }
        #endregion

        #region OnRouteNotificationToggle
        private void OnRoutePathfindNotificationToggle(bool value) {
            m_RoutePathfindNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.RoutePathfindNotification = value;
            m_AlertIconSystem.EnableRouteNotification(RouteNotificationIcon.PathfindNotification, value, true);
        }

        private void OnRouteGateBypassNotificationToggle(bool value) {
            m_RouteGateBypassNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.RouteGateBypassNotification = value;
            m_AlertIconSystem.EnableRouteNotification(RouteNotificationIcon.GateBypassNotification, value, true);
        }
        #endregion

        #region OnTransportLineNotificationToggle
        private void OnTransportLineVehicleNotificationToggle(bool value) {
            m_TransportLineVehicleNotificationBinding.Update(value);
            CwdSettings.Instance.Notification.TransportLineVehicleNotification = value;
            m_AlertIconSystem.EnableTransportLineNotification(TransportLineNotificationIcon.VehicleNotification, value, true);
        }
        #endregion

        private void ToggleAllNotificationsFromHotkey()
        {
            bool enabled = !AreAllNotificationSettingsEnabled();

            ApplyAllNotificationToggles(enabled);
        }

        private void ApplyAllNotificationToggles(bool enabled)
        {
            // Shared path for the hotkey and panel SHOW ICONS button.
            // The controller applies icon state in bulk, then bindings update panel state.
            m_AlertIconSystem.SetAllNotifications(enabled);
            UpdateAllNotificationBindings(enabled);

            // Show/Hide Icons no longer matches either saved slot, so drop the "selected" ring.
            SetActivePreset(0);
        }

        private void UpdateAllNotificationBindings(bool enabled)
        {
            // Keep this list aligned with CwdSettings.NotificationSetting and the BoolBinding fields above.
            m_ElectricElectricNotificationBinding.Update(enabled);
            m_ElectricBottleneckNotificationBinding.Update(enabled);
            m_ElectricBuildingBottleneckNotificationBinding.Update(enabled);
            m_ElectricNotEnoughProductionNotificationBinding.Update(enabled);
            m_ElectricTransformerNotificationBinding.Update(enabled);
            m_ElectricNotEnoughConnectedNotificationBinding.Update(enabled);
            m_ElectricBatteryEmptyNotificationBinding.Update(enabled);
            m_ElectricLowVoltageNotConnectedBinding.Update(enabled);
            m_ElectricHighVoltageNotConnectedBinding.Update(enabled);

            m_WaterPipeWaterNotificationBinding.Update(enabled);
            m_WaterPipeDirtyWaterNotificationBinding.Update(enabled);
            m_WaterPipeSewageNotificationBinding.Update(enabled);
            m_WaterPipeWaterPipeNotConnectedNotificationBinding.Update(enabled);
            m_WaterPipeSewagePipeNotConnectedNotificationBinding.Update(enabled);
            m_WaterPipeNotEnoughWaterCapacityNotificationBinding.Update(enabled);
            m_WaterPipeNotEnoughSewageCapacityNotificationBinding.Update(enabled);
            m_WaterPipeNotEnoughGroundwaterNotificationBinding.Update(enabled);
            m_WaterPipeNotEnoughSurfaceWaterNotificationBinding.Update(enabled);
            m_WaterPipeDirtyWaterPumpNotificationBinding.Update(enabled);

            m_BuildingAbandonedCollapsedNotificationBinding.Update(enabled);
            m_BuildingAbandonedNotificationBinding.Update(enabled);
            m_BuildingCondemnedNotificationBinding.Update(enabled);
            m_BuildingTurnedOffNotificationBinding.Update(enabled);
            m_BuildingHighRentNotificationBinding.Update(enabled);

            // Intentionally skip m_BuildingLevelingNotificationBinding:
            // Leveling is positive/optional, so bulk Show/Hide and N do not change it.
            m_TrafficBottleneckNotificationBinding.Update(enabled);
            m_TrafficDeadEndNotificationBinding.Update(enabled);
            m_TrafficRoadConnectionNotificationBinding.Update(enabled);
            m_TrafficTrackConnectionNotificationBinding.Update(enabled);
            m_TrafficCarConnectionNotificationBinding.Update(enabled);
            m_TrafficShipConnectionNotificationBinding.Update(enabled);
            m_TrafficTrainConnectionNotificationBinding.Update(enabled);
            m_TrafficPedestrianConnectionNotificationBinding.Update(enabled);
            m_TrafficBicycleConnectionNotificationBinding.Update(enabled);

            m_CompanyNoInputsNotificationBinding.Update(enabled);
            m_CompanyNoCustomersNotificationBinding.Update(enabled);

            m_WorkProviderUneducatedNotificationBinding.Update(enabled);
            m_WorkProviderEducatedNotificationBinding.Update(enabled);

            m_DisasterWeatherDamageNotificationBinding.Update(enabled);
            m_DisasterWeatherDestroyedNotificationBinding.Update(enabled);
            m_DisasterWaterDamageNotificationBinding.Update(enabled);
            m_DisasterWaterDestroyedNotificationBinding.Update(enabled);
            m_DisasterDestroyedNotificationBinding.Update(enabled);

            m_FireFireNotificationBinding.Update(enabled);
            m_FireBurnedDownNotificationBinding.Update(enabled);

            m_GarbageGarbageNotificationBinding.Update(enabled);
            m_GarbageFacilityFullNotificationBinding.Update(enabled);

            m_HealthcareAmbulanceNotificationBinding.Update(enabled);
            m_HealthcareHearseNotificationBinding.Update(enabled);
            m_HealthcareFacilityFullNotificationBinding.Update(enabled);

            m_PoliceTrafficAccidentNotificationBinding.Update(enabled);
            m_PoliceCrimeSceneNotificationBinding.Update(enabled);

            m_PollutionAirPollutionNotificationBinding.Update(enabled);
            m_PollutionNoisePollutionNotificationBinding.Update(enabled);
            m_PollutionGroundPollutionNotificationBinding.Update(enabled);

            m_ResourceConsumerNoResourceNotificationBinding.Update(enabled);
            m_ResourceConsumerNoFuelNotificationBinding.Update(enabled);
            m_ResourceConnectionWarningNotificationBinding.Update(enabled);
            m_ResourceConnectionOilPipeNotConnectedNotificationBinding.Update(enabled);
            m_ResourceConnectionFishingPierNotConnectedNotificationBinding.Update(enabled);
            m_RoutePathfindNotificationBinding.Update(enabled);
            m_RouteGateBypassNotificationBinding.Update(enabled);
            m_TransportLineVehicleNotificationBinding.Update(enabled);
        }

        // Holding 1 or 2 saves the current checkbox layout into that slot.
        private void SavePreset(int slot)
        {
            CwdSettings.NotificationSetting live = CwdSettings.Instance.Notification;

            if (slot == 1)
            {
                CwdSettings.Instance.Preset1.CopyFrom(live);
                CwdSettings.Instance.Preset1Saved = true;
                m_Preset1SavedBinding.Update(true);
            }
            else if (slot == 2)
            {
                CwdSettings.Instance.Preset2.CopyFrom(live);
                CwdSettings.Instance.Preset2Saved = true;
                m_Preset2SavedBinding.Update(true);
            }
            else
            {
                return;
            }

            SetActivePreset(slot);
            TrySavePresetSettings("preset-save");
        }

        // Clicking a saved slot restores its checkbox layout.
        // Unsaved slots do nothing, so they cannot replace the live layout with defaults.
        private void LoadPreset(int slot)
        {
            CwdSettings.NotificationSetting source;

            if (slot == 1)
            {
                if (!CwdSettings.Instance.Preset1Saved)
                {
                    return;
                }
                source = CwdSettings.Instance.Preset1;
            }
            else if (slot == 2)
            {
                if (!CwdSettings.Instance.Preset2Saved)
                {
                    return;
                }
                source = CwdSettings.Instance.Preset2;
            }
            else
            {
                return;
            }

            // Apply the saved layout in one pass, then refresh every panel checkbox.
            CwdSettings.Instance.Notification.CopyFrom(source);
            m_AlertIconSystem.ApplyNotificationSettings();
            PushNotificationBindingsFromSettings();
            SetActivePreset(slot);
            TrySavePresetSettings("preset-load");
        }

        private void SetActivePreset(int slot)
        {
            CwdSettings.Instance.ActivePreset = slot;
            m_ActivePresetBinding.Update(slot);
        }

        // Manual checkbox changes no longer match the selected preset.
        // One React trigger avoids repeating this in every notification handler.
        private void ClearActivePreset()
        {
            SetActivePreset(0);
        }

        // Presets restore each saved value, including Building Leveling.
        // Bulk Show/Hide deliberately skips Building Leveling.
        private void PushNotificationBindingsFromSettings()
        {
            CwdSettings.NotificationSetting n = CwdSettings.Instance.Notification;

            m_ElectricElectricNotificationBinding.Update(n.ElectricityElectricityNotification);
            m_ElectricBottleneckNotificationBinding.Update(n.ElectricityBottleneckNotification);
            m_ElectricBuildingBottleneckNotificationBinding.Update(n.ElectricityBuildingBottleneckNotification);
            m_ElectricNotEnoughProductionNotificationBinding.Update(n.ElectricityNotEnoughProductionNotification);
            m_ElectricTransformerNotificationBinding.Update(n.ElectricityTransformerNotification);
            m_ElectricNotEnoughConnectedNotificationBinding.Update(n.ElectricityNotEnoughConnectedNotification);
            m_ElectricBatteryEmptyNotificationBinding.Update(n.ElectricityBatteryEmptyNotification);
            m_ElectricLowVoltageNotConnectedBinding.Update(n.ElectricityLowVoltageNotConnected);
            m_ElectricHighVoltageNotConnectedBinding.Update(n.ElectricityHighVoltageNotConnected);

            m_WaterPipeWaterNotificationBinding.Update(n.WaterPipeWaterNotification);
            m_WaterPipeDirtyWaterNotificationBinding.Update(n.WaterPipeDirtyWaterNotification);
            m_WaterPipeSewageNotificationBinding.Update(n.WaterPipeSewageNotification);
            m_WaterPipeWaterPipeNotConnectedNotificationBinding.Update(n.WaterPipeWaterPipeNotConnectedNotification);
            m_WaterPipeSewagePipeNotConnectedNotificationBinding.Update(n.WaterPipeSewagePipeNotConnectedNotification);
            m_WaterPipeNotEnoughWaterCapacityNotificationBinding.Update(n.WaterPipeNotEnoughWaterCapacityNotification);
            m_WaterPipeNotEnoughSewageCapacityNotificationBinding.Update(n.WaterPipeNotEnoughSewageCapacityNotification);
            m_WaterPipeNotEnoughGroundwaterNotificationBinding.Update(n.WaterPipeNotEnoughGroundwaterNotification);
            m_WaterPipeNotEnoughSurfaceWaterNotificationBinding.Update(n.WaterPipeNotEnoughSurfaceWaterNotification);
            m_WaterPipeDirtyWaterPumpNotificationBinding.Update(n.WaterPipeDirtyWaterPumpNotification);

            m_BuildingAbandonedCollapsedNotificationBinding.Update(n.BuildingAbandonedCollapsedNotification);
            m_BuildingAbandonedNotificationBinding.Update(n.BuildingAbandonedNotification);
            m_BuildingCondemnedNotificationBinding.Update(n.BuildingCondemnedNotification);
            m_BuildingTurnedOffNotificationBinding.Update(n.BuildingTurnedOffNotification);
            m_BuildingHighRentNotificationBinding.Update(n.BuildingHighRentNotification);
            m_BuildingLevelingNotificationBinding.Update(n.BuildingLevelingNotification);

            m_TrafficBottleneckNotificationBinding.Update(n.TrafficBottleneckNotification);
            m_TrafficDeadEndNotificationBinding.Update(n.TrafficDeadEndNotification);
            m_TrafficRoadConnectionNotificationBinding.Update(n.TrafficRoadConnectionNotification);
            m_TrafficTrackConnectionNotificationBinding.Update(n.TrafficTrackConnectionNotification);
            m_TrafficCarConnectionNotificationBinding.Update(n.TrafficCarConnectionNotification);
            m_TrafficShipConnectionNotificationBinding.Update(n.TrafficShipConnectionNotification);
            m_TrafficTrainConnectionNotificationBinding.Update(n.TrafficTrainConnectionNotification);
            m_TrafficPedestrianConnectionNotificationBinding.Update(n.TrafficPedestrianConnectionNotification);
            m_TrafficBicycleConnectionNotificationBinding.Update(n.TrafficBicycleConnectionNotification);

            m_CompanyNoInputsNotificationBinding.Update(n.CompanyNoInputsNotification);
            m_CompanyNoCustomersNotificationBinding.Update(n.CompanyNoCustomersNotification);

            m_WorkProviderUneducatedNotificationBinding.Update(n.WorkProviderUneducatedNotification);
            m_WorkProviderEducatedNotificationBinding.Update(n.WorkProviderEducatedNotification);

            m_DisasterWeatherDamageNotificationBinding.Update(n.DisasterWeatherDamageNotification);
            m_DisasterWeatherDestroyedNotificationBinding.Update(n.DisasterWeatherDestroyedNotification);
            m_DisasterWaterDamageNotificationBinding.Update(n.DisasterWaterDamageNotification);
            m_DisasterWaterDestroyedNotificationBinding.Update(n.DisasterWaterDestroyedNotification);
            m_DisasterDestroyedNotificationBinding.Update(n.DisasterDestroyedNotification);

            m_FireFireNotificationBinding.Update(n.FireFireNotification);
            m_FireBurnedDownNotificationBinding.Update(n.FireBurnedDownNotification);

            m_GarbageGarbageNotificationBinding.Update(n.GarbageGarbageNotification);
            m_GarbageFacilityFullNotificationBinding.Update(n.GarbageFacilityFullNotification);

            m_HealthcareAmbulanceNotificationBinding.Update(n.HealthcareAmbulanceNotification);
            m_HealthcareHearseNotificationBinding.Update(n.HealthcareHearseNotification);
            m_HealthcareFacilityFullNotificationBinding.Update(n.HealthcareFacilityFullNotification);

            m_PoliceTrafficAccidentNotificationBinding.Update(n.PoliceTrafficAccidentNotification);
            m_PoliceCrimeSceneNotificationBinding.Update(n.PoliceCrimeSceneNotification);

            m_PollutionAirPollutionNotificationBinding.Update(n.PollutionAirPollutionNotification);
            m_PollutionNoisePollutionNotificationBinding.Update(n.PollutionNoisePollutionNotification);
            m_PollutionGroundPollutionNotificationBinding.Update(n.PollutionGroundPollutionNotification);

            m_ResourceConsumerNoResourceNotificationBinding.Update(n.ResourceConsumerNoResourceNotification);
            m_ResourceConsumerNoFuelNotificationBinding.Update(n.ResourceConsumerNoFuelNotification);
            m_ResourceConnectionWarningNotificationBinding.Update(n.ResourceConnectionWarningNotification);
            m_ResourceConnectionOilPipeNotConnectedNotificationBinding.Update(n.ResourceConnectionOilPipeNotConnectedNotification);
            m_ResourceConnectionFishingPierNotConnectedNotificationBinding.Update(n.ResourceConnectionFishingPierNotConnectedNotification);

            m_RoutePathfindNotificationBinding.Update(n.RoutePathfindNotification);
            m_RouteGateBypassNotificationBinding.Update(n.RouteGateBypassNotification);

            m_TransportLineVehicleNotificationBinding.Update(n.TransportLineVehicleNotification);
        }

        private static void TrySavePresetSettings(string tag)
        {
            try
            {
                CwdSettings.Instance.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    tag,
                    () => $"Failed to persist notification preset: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private static bool AreAllNotificationSettingsEnabled()
        {
            // Keep this list aligned with CwdSettings.NotificationSetting.
            CwdSettings.NotificationSetting notification = CwdSettings.Instance.Notification;

            return notification.ElectricityElectricityNotification &&
                   notification.ElectricityBottleneckNotification &&
                   notification.ElectricityBuildingBottleneckNotification &&
                   notification.ElectricityNotEnoughProductionNotification &&
                   notification.ElectricityTransformerNotification &&
                   notification.ElectricityNotEnoughConnectedNotification &&
                   notification.ElectricityBatteryEmptyNotification &&
                   notification.ElectricityLowVoltageNotConnected &&
                   notification.ElectricityHighVoltageNotConnected &&
                   notification.WaterPipeWaterNotification &&
                   notification.WaterPipeDirtyWaterNotification &&
                   notification.WaterPipeSewageNotification &&
                   notification.WaterPipeWaterPipeNotConnectedNotification &&
                   notification.WaterPipeSewagePipeNotConnectedNotification &&
                   notification.WaterPipeNotEnoughWaterCapacityNotification &&
                   notification.WaterPipeNotEnoughSewageCapacityNotification &&
                   notification.WaterPipeNotEnoughGroundwaterNotification &&
                   notification.WaterPipeNotEnoughSurfaceWaterNotification &&
                   notification.WaterPipeDirtyWaterPumpNotification &&
                   notification.BuildingAbandonedCollapsedNotification &&
                   notification.BuildingAbandonedNotification &&
                   notification.BuildingCondemnedNotification &&
                   notification.BuildingTurnedOffNotification &&
                   notification.BuildingHighRentNotification &&
                   notification.TrafficBottleneckNotification &&
                   notification.TrafficDeadEndNotification &&
                   notification.TrafficRoadConnectionNotification &&
                   notification.TrafficTrackConnectionNotification &&
                   notification.TrafficCarConnectionNotification &&
                   notification.TrafficShipConnectionNotification &&
                   notification.TrafficTrainConnectionNotification &&
                   notification.TrafficPedestrianConnectionNotification &&
                   notification.TrafficBicycleConnectionNotification &&
                   notification.CompanyNoInputsNotification &&
                   notification.CompanyNoCustomersNotification &&
                   notification.WorkProviderUneducatedNotification &&
                   notification.WorkProviderEducatedNotification &&
                   notification.DisasterWeatherDamageNotification &&
                   notification.DisasterWeatherDestroyedNotification &&
                   notification.DisasterWaterDamageNotification &&
                   notification.DisasterWaterDestroyedNotification &&
                   notification.DisasterDestroyedNotification &&
                   notification.FireFireNotification &&
                   notification.FireBurnedDownNotification &&
                   notification.GarbageGarbageNotification &&
                   notification.GarbageFacilityFullNotification &&
                   notification.HealthcareAmbulanceNotification &&
                   notification.HealthcareHearseNotification &&
                   notification.HealthcareFacilityFullNotification &&
                   notification.PoliceTrafficAccidentNotification &&
                   notification.PoliceCrimeSceneNotification &&
                   notification.PollutionAirPollutionNotification &&
                   notification.PollutionNoisePollutionNotification &&
                   notification.PollutionGroundPollutionNotification &&
                   notification.ResourceConsumerNoResourceNotification &&
                   notification.ResourceConsumerNoFuelNotification &&
                   notification.ResourceConnectionWarningNotification &&
                   notification.ResourceConnectionOilPipeNotConnectedNotification &&
                   notification.ResourceConnectionFishingPierNotConnectedNotification &&
                   notification.RoutePathfindNotification &&
                   notification.RouteGateBypassNotification &&
                   notification.TransportLineVehicleNotification;
        }
    }
}
