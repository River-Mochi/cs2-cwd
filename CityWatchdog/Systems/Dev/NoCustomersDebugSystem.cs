// <copyright file="NoCustomersDebugSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Dev/NoCustomersDebugSystem.cs
// Purpose: DEBUG-only diagnostics for vanilla "Not enough customers" commercial warnings.

#if DEBUG

using System.Globalization;
using System.Text;

using Colossal.Serialization.Entities;

using CS2Shared.RiverMochi;

using Game;
using Game.Buildings;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;

using Unity.Entities;

namespace CityWatchdog.Systems
{
    public partial class NoCustomersDebugSystem : GameSystemBase
    {
        // Power-of-two interval required by CS2's UpdateSystem.
        private const int kScanInterval = 2048;

        // Write detailed per-company rows every fourth scan.
        private const int kFullReportEveryScans = 4;

        private PrefabSystem m_PrefabSystem = null!;
        private int m_ScanCount;

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return kScanInterval;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem =
                World.GetOrCreateSystemManaged<PrefabSystem>();
        }

        protected override void OnGameLoaded(Context serializationContext)
        {
            base.OnGameLoaded(serializationContext);

            // Systems survive city loads; start the diagnostics fresh for each city.
            m_ScanCount = 0;
        }

        protected override void OnUpdate()
        {
            m_ScanCount++;

            bool writeFullReport =
                m_ScanCount == 1 ||
                m_ScanCount % kFullReportEveryScans == 0;

            int commercialCount = 0;
            int warningCount = 0;
            int over90Count = 0;
            int over95Count = 0;
            int over99Count = 0;

            // DEBUG-only, low-frequency diagnostic. Keeping this non-null also lets
            // nullable analysis protect us instead of suppressing CS8602.
            StringBuilder details = new(4096);

            ComponentLookup<Game.Companies.ServiceCompanyData> serviceCompanyDatas =
                SystemAPI.GetComponentLookup<Game.Companies.ServiceCompanyData>(true);

            ComponentLookup<Game.Prefabs.IndustrialProcessData> industrialProcessDatas =
                SystemAPI.GetComponentLookup<Game.Prefabs.IndustrialProcessData>(true);

            ComponentLookup<Game.Companies.StorageLimitData> storageLimits =
                SystemAPI.GetComponentLookup<Game.Companies.StorageLimitData>(true);

            ComponentLookup<Game.Companies.WorkProvider> workProviders =
                SystemAPI.GetComponentLookup<Game.Companies.WorkProvider>(true);

            ComponentLookup<Game.Companies.CompanyStatisticData> companyStatistics =
                SystemAPI.GetComponentLookup<Game.Companies.CompanyStatisticData>(true);

            ComponentLookup<Game.Companies.ResourceBuyer> resourceBuyers =
                SystemAPI.GetComponentLookup<Game.Companies.ResourceBuyer>(true);

            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefs =
                SystemAPI.GetComponentLookup<Game.Prefabs.PrefabRef>(true);

            BufferLookup<Game.Economy.Resources> resourcesLookup =
                SystemAPI.GetBufferLookup<Game.Economy.Resources>(true);

            BufferLookup<Game.Companies.Employee> employeesLookup =
                SystemAPI.GetBufferLookup<Game.Companies.Employee>(true);

            BufferLookup<Game.Citizens.TripNeeded> tripNeededLookup =
                SystemAPI.GetBufferLookup<Game.Citizens.TripNeeded>(true);

            foreach ((
                RefRO<ServiceAvailable> serviceRef,
                RefRO<CompanyNotifications> notificationsRef,
                RefRO<PrefabRef> companyPrefabRef,
                RefRO<PropertyRenter> propertyRenterRef,
                Entity company) in
                SystemAPI
                    .Query<
                        RefRO<Game.Companies.ServiceAvailable>,
                        RefRO<Game.Companies.CompanyNotifications>,
                        RefRO<Game.Prefabs.PrefabRef>,
                        RefRO<Game.Buildings.PropertyRenter>>()
                    .WithAll<Game.Companies.CommercialCompany>()
                    .WithNone<Game.Common.Deleted, Game.Tools.Temp>()
                    .WithEntityAccess())
            {
                commercialCount++;

                Game.Companies.ServiceAvailable service =
                    serviceRef.ValueRO;

                Game.Companies.CompanyNotifications notifications =
                    notificationsRef.ValueRO;

                Entity companyPrefab =
                    companyPrefabRef.ValueRO.m_Prefab;

                if (!serviceCompanyDatas.HasComponent(companyPrefab) ||
                    !industrialProcessDatas.HasComponent(companyPrefab))
                {
                    continue;
                }

                Game.Companies.ServiceCompanyData serviceData =
                    serviceCompanyDatas[companyPrefab];

                Game.Prefabs.IndustrialProcessData process =
                    industrialProcessDatas[companyPrefab];

                if (serviceData.m_MaxService <= 0)
                {
                    continue;
                }

                float unsoldRatio =
                    (float)service.m_ServiceAvailable /
                    serviceData.m_MaxService;

                if (unsoldRatio > 0.90f)
                {
                    over90Count++;
                }

                if (unsoldRatio > 0.95f)
                {
                    over95Count++;
                }

                if (unsoldRatio > 0.99f)
                {
                    over99Count++;
                }

                bool hasWarning =
                    notifications.m_NoCustomersEntity != Entity.Null;

                if (!hasWarning)
                {
                    continue;
                }

                warningCount++;

                if (!writeFullReport)
                {
                    continue;
                }

                Resource soldResource =
                    process.m_Output.m_Resource;

                int physicalStock = 0;

                if (resourcesLookup.HasBuffer(company))
                {
                    DynamicBuffer<Game.Economy.Resources> resources =
                        resourcesLookup[company];

                    physicalStock =
                        EconomyUtils.GetResources(
                            soldResource,
                            resources);
                }

                int storageLimit = -1;

                if (storageLimits.HasComponent(companyPrefab))
                {
                    storageLimit =
                        storageLimits[companyPrefab].m_Limit;
                }

                int workers = -1;

                if (employeesLookup.HasBuffer(company))
                {
                    workers =
                        employeesLookup[company].Length;
                }

                int maxWorkers = -1;

                if (workProviders.HasComponent(company))
                {
                    maxWorkers =
                        workProviders[company].m_MaxWorkers;
                }

                int currentCustomers = -1;
                int monthlyCustomers = -1;
                int maxCustomers = -1;

                if (companyStatistics.HasComponent(company))
                {
                    Game.Companies.CompanyStatisticData statistics =
                        companyStatistics[company];

                    currentCustomers =
                        statistics.m_CurrentNumberOfCustomers;

                    monthlyCustomers =
                        statistics.m_MonthlyCustomerCount;

                    maxCustomers =
                        statistics.m_MaxNumberOfCustomers;
                }

                int pendingRestockTrips = 0;
                int pendingRestockAmount = 0;

                if (tripNeededLookup.HasBuffer(company))
                {
                    DynamicBuffer<Game.Citizens.TripNeeded> trips =
                        tripNeededLookup[company];

                    for (int i = 0; i < trips.Length; i++)
                    {
                        Game.Citizens.TripNeeded trip = trips[i];

                        bool restockPurpose =
                            trip.m_Purpose == Game.Citizens.Purpose.Shopping ||
                            trip.m_Purpose == Game.Citizens.Purpose.CompanyShopping;

                        if (restockPurpose &&
                            IsInputResource(trip.m_Resource, process))
                        {
                            pendingRestockTrips++;
                            pendingRestockAmount += trip.m_Data;
                        }
                    }
                }

                Resource buyRequestResource =
                    Resource.NoResource;

                int buyRequestAmount = 0;

                if (resourceBuyers.HasComponent(company))
                {
                    Game.Companies.ResourceBuyer buyer =
                        resourceBuyers[company];

                    buyRequestResource =
                        buyer.m_ResourceNeeded;

                    buyRequestAmount =
                        buyer.m_AmountNeeded;
                }

                Entity building =
                    propertyRenterRef.ValueRO.m_Property;

                string companyName =
                    GetCompanyPrefabName(companyPrefab);

                string buildingName =
                    GetBuildingPrefabName(
                        building,
                        prefabRefs);

                details
                    .Append("[CWD-NOCUSTOMERS] ")
                    .Append("company=").Append(company)
                    .Append(" companyPrefab=").Append(companyName)
                    .Append(" building=").Append(building)
                    .Append(" buildingPrefab=").Append(buildingName)
                    .Append(" resource=").Append(soldResource)
                    .Append(" service=")
                    .Append(service.m_ServiceAvailable)
                    .Append('/')
                    .Append(serviceData.m_MaxService)
                    .Append(" unsold=")
                    .Append(
                        (unsoldRatio * 100f).ToString(
                            "F1",
                            CultureInfo.InvariantCulture))
                    .Append('%')
                    .Append(" stock=").Append(physicalStock)
                    .Append(" storageLimit=").Append(storageLimit)
                    .Append(" meanPriority=")
                    .Append(
                        service.m_MeanPriority.ToString(
                            "F3",
                            CultureInfo.InvariantCulture))
                    .Append(" customersCurrent=").Append(currentCustomers)
                    .Append(" customersMonthly=").Append(monthlyCustomers)
                    .Append(" customersMax=").Append(maxCustomers)
                    .Append(" workers=")
                    .Append(workers)
                    .Append('/')
                    .Append(maxWorkers)
                    .Append(" input1=").Append(process.m_Input1.m_Resource)
                    .Append(" input2=").Append(process.m_Input2.m_Resource)
                    .Append(" buyRequestResource=").Append(buyRequestResource)
                    .Append(" buyRequestAmount=").Append(buyRequestAmount)
                    .Append(" restockTrips=").Append(pendingRestockTrips)
                    .Append(" restockAmount=").Append(pendingRestockAmount)
                    .Append(" noCustomersCounter=")
                    .Append(notifications.m_NoCustomersCounter)
                    .AppendLine();
            }

            LogUtils.Info(
                $"[CWD-NOCUSTOMERS] scan={m_ScanCount} " +
                $"commercial={commercialCount} " +
                $"warnings={warningCount} " +
                $">90%={over90Count} " +
                $">95%={over95Count} " +
                $">99%={over99Count}");

            if (writeFullReport && details.Length > 0)
            {
                LogUtils.Info(details.ToString());
            }
        }

        private string GetCompanyPrefabName(Entity prefab)
        {
            try
            {
                CompanyPrefab companyPrefab =
                    m_PrefabSystem.GetPrefab<CompanyPrefab>(prefab);

                return companyPrefab.name ?? prefab.ToString();
            }
            catch
            {
                return prefab.ToString();
            }
        }

        private string GetBuildingPrefabName(
            Entity building,
            ComponentLookup<PrefabRef> prefabRefs)
        {
            if (building == Entity.Null ||
                !prefabRefs.HasComponent(building))
            {
                return "<none>";
            }

            Entity prefab =
                prefabRefs[building].m_Prefab;

            try
            {
                BuildingPrefab buildingPrefab =
                    m_PrefabSystem.GetPrefab<BuildingPrefab>(prefab);

                return buildingPrefab.name ?? prefab.ToString();
            }
            catch
            {
                return prefab.ToString();
            }
        }

        private static bool IsInputResource(
            Resource resource,
            IndustrialProcessData process)
        {
            if (resource == Resource.NoResource)
            {
                return false;
            }

            return resource == process.m_Input1.m_Resource ||
                resource == process.m_Input2.m_Resource;
        }
    }
}

#endif
