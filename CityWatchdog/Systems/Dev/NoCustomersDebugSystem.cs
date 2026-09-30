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

namespace CityWatchdog.Systems
{
    using System;
    using System.Text;

    using CS2Shared.RiverMochi;

    using Game.Buildings;
    using Game.Common;
    using Game.Companies;
    using Game.Economy;
    using Game.Prefabs;
    using Game.Simulation;

    using Unity.Collections;
    using Unity.Entities;

    public sealed class NoCustomersDebugSystem : GameSystemBase
    {
        // Frequent enough to see warnings appear/disappear without scanning every simulation tick.
        private const uint kScanIntervalFrames = 2048;

        // Full active-warning report less often to keep the log readable.
        private const uint kReportIntervalFrames = 8192;

        private SimulationSystem m_SimulationSystem = null!;
        private PrefabSystem m_PrefabSystem = null!;

        private EntityQuery m_CommercialQuery;

        private uint m_LastScanFrame;
        private uint m_LastReportFrame;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_CommercialQuery = GetEntityQuery(
                ComponentType.ReadOnly<CommercialCompany>(),
                ComponentType.ReadOnly<ServiceAvailable>(),
                ComponentType.ReadOnly<CompanyNotifications>(),
                ComponentType.ReadOnly<PrefabRef>(),
                ComponentType.ReadOnly<PropertyRenter>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            RequireForUpdate(m_CommercialQuery);
        }

        protected override void OnUpdate()
        {
            uint frame = m_SimulationSystem.frameIndex;

            if (frame - m_LastScanFrame < kScanIntervalFrames)
            {
                return;
            }

            m_LastScanFrame = frame;

            bool writeFullReport =
                m_LastReportFrame == 0 ||
                frame - m_LastReportFrame >= kReportIntervalFrames;

            int commercialCount = 0;
            int warningCount = 0;
            int over90Count = 0;
            int over95Count = 0;
            int over99Count = 0;

            StringBuilder? details = writeFullReport
                ? new StringBuilder(4096)
                : null;

            using NativeArray<Entity> companies =
                m_CommercialQuery.ToEntityArray(Allocator.Temp);

            for (int i = 0; i < companies.Length; i++)
            {
                Entity company = companies[i];
                commercialCount++;

                ServiceAvailable service =
                    EntityManager.GetComponentData<ServiceAvailable>(company);

                CompanyNotifications notifications =
                    EntityManager.GetComponentData<CompanyNotifications>(company);

                PrefabRef companyPrefabRef =
                    EntityManager.GetComponentData<PrefabRef>(company);

                Entity companyPrefab = companyPrefabRef.m_Prefab;

                if (!EntityManager.HasComponent<ServiceCompanyData>(companyPrefab) ||
                    !EntityManager.HasComponent<IndustrialProcessData>(companyPrefab))
                {
                    continue;
                }

                ServiceCompanyData serviceData =
                    EntityManager.GetComponentData<ServiceCompanyData>(companyPrefab);

                IndustrialProcessData process =
                    EntityManager.GetComponentData<IndustrialProcessData>(companyPrefab);

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

                Resource soldResource = process.m_Output.m_Resource;

                int physicalStock = 0;
                if (EntityManager.HasBuffer<Game.Economy.Resources>(company))
                {
                    DynamicBuffer<Game.Economy.Resources> resources =
                        EntityManager.GetBuffer<Game.Economy.Resources>(company);

                    physicalStock =
                        EconomyUtils.GetResources(soldResource, resources);
                }

                int storageLimit = -1;
                if (EntityManager.HasComponent<StorageLimitData>(companyPrefab))
                {
                    storageLimit =
                        EntityManager
                            .GetComponentData<StorageLimitData>(companyPrefab)
                            .m_Limit;
                }

                int workers = -1;
                if (EntityManager.HasBuffer<Employee>(company))
                {
                    workers = EntityManager.GetBuffer<Employee>(company).Length;
                }

                int maxWorkers = -1;
                if (EntityManager.HasComponent<WorkProvider>(company))
                {
                    maxWorkers =
                        EntityManager
                            .GetComponentData<WorkProvider>(company)
                            .m_MaxWorkers;
                }

                int currentCustomers = -1;
                int monthlyCustomers = -1;
                int maxCustomers = -1;

                if (EntityManager.HasComponent<CompanyStatisticData>(company))
                {
                    CompanyStatisticData statistics =
                        EntityManager.GetComponentData<CompanyStatisticData>(company);

                    currentCustomers =
                        statistics.m_CurrentNumberOfCustomers;

                    monthlyCustomers =
                        statistics.m_MonthlyCustomerCount;

                    maxCustomers =
                        statistics.m_MaxNumberOfCustomers;
                }

                int pendingRestock = 0;
                int pendingRestockTrips = 0;

                if (EntityManager.HasBuffer<Game.Citizens.TripNeeded>(company))
                {
                    DynamicBuffer<Game.Citizens.TripNeeded> trips =
                        EntityManager.GetBuffer<Game.Citizens.TripNeeded>(company);

                    for (int j = 0; j < trips.Length; j++)
                    {
                        Game.Citizens.TripNeeded trip = trips[j];

                        if (trip.m_Purpose == Game.Citizens.Purpose.Shopping &&
                            trip.m_Resource == soldResource)
                        {
                            pendingRestockTrips++;
                            pendingRestock += trip.m_Data;
                        }
                    }
                }

                PropertyRenter propertyRenter =
                    EntityManager.GetComponentData<PropertyRenter>(company);

                Entity building = propertyRenter.m_Property;

                string companyName =
                    GetCompanyPrefabName(companyPrefab);

                string buildingName =
                    GetBuildingPrefabName(building);

                details.Append("[CWD-NOCUSTOMERS] ")
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
                    .Append((unsoldRatio * 100f).ToString("F1"))
                    .Append('%')
                    .Append(" stock=").Append(physicalStock)
                    .Append(" storageLimit=").Append(storageLimit)
                    .Append(" meanPriority=")
                    .Append(service.m_MeanPriority.ToString("F3"))
                    .Append(" customersCurrent=").Append(currentCustomers)
                    .Append(" customersMonthly=").Append(monthlyCustomers)
                    .Append(" customersMax=").Append(maxCustomers)
                    .Append(" workers=")
                    .Append(workers)
                    .Append('/')
                    .Append(maxWorkers)
                    .Append(" restockTrips=").Append(pendingRestockTrips)
                    .Append(" restockAmount=").Append(pendingRestock)
                    .Append(" noCustomersCounter=")
                    .Append(notifications.m_NoCustomersCounter)
                    .AppendLine();
            }

            if (!writeFullReport)
            {
                return;
            }

            m_LastReportFrame = frame;

            LogUtils.Debug(
                () =>
                    $"[CWD-NOCUSTOMERS] frame={frame} " +
                    $"commercial={commercialCount} " +
                    $"warnings={warningCount} " +
                    $">90%={over90Count} " +
                    $">95%={over95Count} " +
                    $">99%={over99Count}");

            if (details != null && details.Length > 0)
            {
                LogUtils.Debug(() => details.ToString());
            }
        }

        private string GetCompanyPrefabName(Entity prefab)
        {
            try
            {
                return m_PrefabSystem.GetPrefab<CompanyPrefab>(prefab).name
                    ?? prefab.ToString();
            }
            catch
            {
                return prefab.ToString();
            }
        }

        private string GetBuildingPrefabName(Entity building)
        {
            if (building == Entity.Null ||
                !EntityManager.HasComponent<PrefabRef>(building))
            {
                return "<none>";
            }

            Entity prefab =
                EntityManager.GetComponentData<PrefabRef>(building).m_Prefab;

            try
            {
                return m_PrefabSystem.GetPrefab<BuildingPrefab>(prefab).name
                    ?? prefab.ToString();
            }
            catch
            {
                return prefab.ToString();
            }
        }
    }
}

#endif
