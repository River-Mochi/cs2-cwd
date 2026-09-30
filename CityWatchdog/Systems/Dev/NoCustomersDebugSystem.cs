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

using System.Collections.Generic;
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

        private readonly Dictionary<Entity, CompanySnapshot> m_PreviousSnapshots = new();

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

            // Systems survive city loads, but city entities do not.
            m_PreviousSnapshots.Clear();
            m_ScanCount = 0;
        }

        protected override void OnUpdate()
        {
            m_ScanCount++;

            bool writeBaseline = m_ScanCount == 1;

            int commercialCount = 0;
            int warningCount = 0;
            int over90Count = 0;
            int over95Count = 0;
            int over99Count = 0;

            Dictionary<Resource, ResourceSummary> resourceSummaries = new();
            HashSet<Entity> seenCompanies = new();

            StringBuilder baseline = new(4096);
            StringBuilder events = new(2048);

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
                RefRO<Game.Companies.ServiceAvailable> serviceRef,
                RefRO<Game.Companies.CompanyNotifications> notificationsRef,
                RefRO<Game.Prefabs.PrefabRef> companyPrefabRef,
                RefRO<Game.Buildings.PropertyRenter> propertyRenterRef,
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
                seenCompanies.Add(company);

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

                Resource soldResource =
                    process.m_Output.m_Resource;

                float unsoldRatio =
                    (float)service.m_ServiceAvailable /
                    serviceData.m_MaxService;

                bool over90 = unsoldRatio > 0.90f;
                bool hasWarning =
                    notifications.m_NoCustomersEntity != Entity.Null;

                if (hasWarning)
                {
                    warningCount++;
                }

                if (over90)
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

                CompanySnapshot snapshot = new(
                    hasWarning,
                    over90,
                    soldResource,
                    service.m_ServiceAvailable,
                    serviceData.m_MaxService,
                    physicalStock,
                    currentCustomers,
                    monthlyCustomers,
                    buyRequestResource,
                    buyRequestAmount,
                    pendingRestockTrips,
                    pendingRestockAmount);

                UpdateResourceSummary(
                    resourceSummaries,
                    snapshot);

                Entity building =
                    propertyRenterRef.ValueRO.m_Property;

                if (writeBaseline && hasWarning)
                {
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

                    AppendBaselineRow(
                        baseline,
                        company,
                        companyPrefab,
                        building,
                        soldResource,
                        service,
                        serviceData,
                        physicalStock,
                        storageLimit,
                        currentCustomers,
                        monthlyCustomers,
                        maxCustomers,
                        workers,
                        maxWorkers,
                        process,
                        buyRequestResource,
                        buyRequestAmount,
                        pendingRestockTrips,
                        pendingRestockAmount,
                        notifications,
                        prefabRefs);
                }

                if (m_PreviousSnapshots.TryGetValue(
                    company,
                    out CompanySnapshot previous))
                {
                    AppendTransitionIfNeeded(
                        events,
                        company,
                        companyPrefab,
                        building,
                        previous,
                        snapshot,
                        prefabRefs);
                }

                m_PreviousSnapshots[company] = snapshot;
            }

            RemoveStaleCompanies(seenCompanies);

            LogUtils.Info(
                $"[CWD-NOCUSTOMERS] scan={m_ScanCount} " +
                $"commercial={commercialCount} " +
                $"warnings={warningCount} " +
                $">90%={over90Count} " +
                $">95%={over95Count} " +
                $">99%={over99Count}");

            LogResourceSummaries(resourceSummaries);

            if (baseline.Length > 0)
            {
                LogUtils.Info(baseline.ToString());
            }

            if (events.Length > 0)
            {
                LogUtils.Info(events.ToString());
            }
        }

        private void AppendBaselineRow(
            StringBuilder baseline,
            Entity company,
            Entity companyPrefab,
            Entity building,
            Resource soldResource,
            Game.Companies.ServiceAvailable service,
            Game.Companies.ServiceCompanyData serviceData,
            int physicalStock,
            int storageLimit,
            int currentCustomers,
            int monthlyCustomers,
            int maxCustomers,
            int workers,
            int maxWorkers,
            Game.Prefabs.IndustrialProcessData process,
            Resource buyRequestResource,
            int buyRequestAmount,
            int pendingRestockTrips,
            int pendingRestockAmount,
            Game.Companies.CompanyNotifications notifications,
            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefs)
        {
            float unsoldRatio =
                (float)service.m_ServiceAvailable /
                serviceData.m_MaxService;

            baseline
                .Append("[CWD-NOCUSTOMERS-BASELINE] ")
                .Append("company=").Append(company)
                .Append(" companyPrefab=")
                .Append(GetCompanyPrefabName(companyPrefab))
                .Append(" building=").Append(building)
                .Append(" buildingPrefab=")
                .Append(GetBuildingPrefabName(building, prefabRefs))
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

        private void AppendTransitionIfNeeded(
            StringBuilder events,
            Entity company,
            Entity companyPrefab,
            Entity building,
            CompanySnapshot previous,
            CompanySnapshot current,
            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefs)
        {
            bool warningChanged =
                previous.HasWarning != current.HasWarning;

            bool thresholdChanged =
                previous.Over90 != current.Over90;

            bool buyRequestChanged =
                previous.BuyRequestResource != current.BuyRequestResource ||
                previous.BuyRequestAmount != current.BuyRequestAmount;

            bool restockChanged =
                previous.RestockTrips != current.RestockTrips ||
                previous.RestockAmount != current.RestockAmount;

            if (!warningChanged &&
                !thresholdChanged &&
                !buyRequestChanged &&
                !restockChanged)
            {
                return;
            }

            StringBuilder eventNames = new();

            if (warningChanged)
            {
                eventNames.Append(
                    current.HasWarning
                        ? "WARNING_ON"
                        : "WARNING_CLEARED");
            }

            if (thresholdChanged)
            {
                AppendEventSeparator(eventNames);

                eventNames.Append(
                    current.Over90
                        ? "OVER90_ENTER"
                        : "OVER90_EXIT");
            }

            if (buyRequestChanged)
            {
                AppendEventSeparator(eventNames);

                if (previous.BuyRequestResource == Resource.NoResource &&
                    current.BuyRequestResource != Resource.NoResource)
                {
                    eventNames.Append("BUY_REQUEST_ON");
                }
                else if (previous.BuyRequestResource != Resource.NoResource &&
                         current.BuyRequestResource == Resource.NoResource)
                {
                    eventNames.Append("BUY_REQUEST_OFF");
                }
                else
                {
                    eventNames.Append("BUY_REQUEST_CHANGED");
                }
            }

            if (restockChanged)
            {
                AppendEventSeparator(eventNames);
                eventNames.Append("RESTOCK_CHANGED");
            }

            events
                .Append("[CWD-NOCUSTOMERS-EVENT] ")
                .Append("scan=").Append(m_ScanCount)
                .Append(" event=").Append(eventNames)
                .Append(" company=").Append(company)
                .Append(" companyPrefab=")
                .Append(GetCompanyPrefabName(companyPrefab))
                .Append(" building=").Append(building)
                .Append(" buildingPrefab=")
                .Append(GetBuildingPrefabName(building, prefabRefs))
                .Append(" resource=").Append(current.Resource)
                .Append(" warning=")
                .Append(previous.HasWarning)
                .Append("->")
                .Append(current.HasWarning)
                .Append(" over90=")
                .Append(previous.Over90)
                .Append("->")
                .Append(current.Over90)
                .Append(" service=")
                .Append(previous.ServiceAvailable)
                .Append("->")
                .Append(current.ServiceAvailable)
                .Append('/')
                .Append(current.MaxService)
                .Append(" stock=")
                .Append(previous.PhysicalStock)
                .Append("->")
                .Append(current.PhysicalStock)
                .Append(" customersCurrent=")
                .Append(previous.CurrentCustomers)
                .Append("->")
                .Append(current.CurrentCustomers)
                .Append(" customersMonthly=")
                .Append(previous.MonthlyCustomers)
                .Append("->")
                .Append(current.MonthlyCustomers)
                .Append(" buyRequest=")
                .Append(previous.BuyRequestResource)
                .Append(':')
                .Append(previous.BuyRequestAmount)
                .Append("->")
                .Append(current.BuyRequestResource)
                .Append(':')
                .Append(current.BuyRequestAmount)
                .Append(" restock=")
                .Append(previous.RestockTrips)
                .Append('/')
                .Append(previous.RestockAmount)
                .Append("->")
                .Append(current.RestockTrips)
                .Append('/')
                .Append(current.RestockAmount)
                .AppendLine();
        }

        private static void AppendEventSeparator(
            StringBuilder eventNames)
        {
            if (eventNames.Length > 0)
            {
                eventNames.Append('|');
            }
        }

        private static void UpdateResourceSummary(
            Dictionary<Resource, ResourceSummary> summaries,
            CompanySnapshot snapshot)
        {
            summaries.TryGetValue(
                snapshot.Resource,
                out ResourceSummary summary);

            summary.CompanyCount++;

            if (snapshot.HasWarning)
            {
                summary.WarningCount++;
            }

            if (snapshot.Over90)
            {
                summary.Over90Count++;
            }

            summary.TotalServiceAvailable +=
                snapshot.ServiceAvailable;

            summary.TotalMaxService +=
                snapshot.MaxService;

            summary.TotalPhysicalStock +=
                snapshot.PhysicalStock;

            if (snapshot.CurrentCustomers >= 0)
            {
                summary.TotalCurrentCustomers +=
                    snapshot.CurrentCustomers;
            }

            if (snapshot.MonthlyCustomers >= 0)
            {
                summary.TotalMonthlyCustomers +=
                    snapshot.MonthlyCustomers;
            }

            summaries[snapshot.Resource] = summary;
        }

        private void LogResourceSummaries(
            Dictionary<Resource, ResourceSummary> summaries)
        {
            if (summaries.Count == 0)
            {
                return;
            }

            List<Resource> resources =
                new(summaries.Keys);

            resources.Sort(
                (left, right) =>
                    string.CompareOrdinal(
                        left.ToString(),
                        right.ToString()));

            StringBuilder report = new(2048);

            for (int i = 0; i < resources.Count; i++)
            {
                Resource resource = resources[i];
                ResourceSummary summary = summaries[resource];

                float warningPercent =
                    summary.CompanyCount > 0
                        ? 100f *
                          summary.WarningCount /
                          summary.CompanyCount
                        : 0f;

                float unsoldPercent =
                    summary.TotalMaxService > 0
                        ? 100f *
                          summary.TotalServiceAvailable /
                          summary.TotalMaxService
                        : 0f;

                report
                    .Append("[CWD-NOCUSTOMERS-RESOURCE] ")
                    .Append("scan=").Append(m_ScanCount)
                    .Append(" resource=").Append(resource)
                    .Append(" companies=").Append(summary.CompanyCount)
                    .Append(" warnings=").Append(summary.WarningCount)
                    .Append(" warningRate=")
                    .Append(
                        warningPercent.ToString(
                            "F1",
                            CultureInfo.InvariantCulture))
                    .Append('%')
                    .Append(" over90=").Append(summary.Over90Count)
                    .Append(" service=")
                    .Append(summary.TotalServiceAvailable)
                    .Append('/')
                    .Append(summary.TotalMaxService)
                    .Append(" unsold=")
                    .Append(
                        unsoldPercent.ToString(
                            "F1",
                            CultureInfo.InvariantCulture))
                    .Append('%')
                    .Append(" stock=")
                    .Append(summary.TotalPhysicalStock)
                    .Append(" customersCurrent=")
                    .Append(summary.TotalCurrentCustomers)
                    .Append(" customersMonthly=")
                    .Append(summary.TotalMonthlyCustomers)
                    .AppendLine();
            }

            LogUtils.Info(report.ToString());
        }

        private void RemoveStaleCompanies(
            HashSet<Entity> seenCompanies)
        {
            if (m_PreviousSnapshots.Count == 0)
            {
                return;
            }

            List<Entity> staleCompanies = new();

            foreach (Entity company in m_PreviousSnapshots.Keys)
            {
                if (!seenCompanies.Contains(company))
                {
                    staleCompanies.Add(company);
                }
            }

            for (int i = 0; i < staleCompanies.Count; i++)
            {
                m_PreviousSnapshots.Remove(
                    staleCompanies[i]);
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
            ComponentLookup<Game.Prefabs.PrefabRef> prefabRefs)
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

        private readonly struct CompanySnapshot
        {
            public CompanySnapshot(
                bool hasWarning,
                bool over90,
                Resource resource,
                int serviceAvailable,
                int maxService,
                int physicalStock,
                int currentCustomers,
                int monthlyCustomers,
                Resource buyRequestResource,
                int buyRequestAmount,
                int restockTrips,
                int restockAmount)
            {
                HasWarning = hasWarning;
                Over90 = over90;
                Resource = resource;
                ServiceAvailable = serviceAvailable;
                MaxService = maxService;
                PhysicalStock = physicalStock;
                CurrentCustomers = currentCustomers;
                MonthlyCustomers = monthlyCustomers;
                BuyRequestResource = buyRequestResource;
                BuyRequestAmount = buyRequestAmount;
                RestockTrips = restockTrips;
                RestockAmount = restockAmount;
            }

            public bool HasWarning { get; }

            public bool Over90 { get; }

            public Resource Resource { get; }

            public int ServiceAvailable { get; }

            public int MaxService { get; }

            public int PhysicalStock { get; }

            public int CurrentCustomers { get; }

            public int MonthlyCustomers { get; }

            public Resource BuyRequestResource { get; }

            public int BuyRequestAmount { get; }

            public int RestockTrips { get; }

            public int RestockAmount { get; }
        }

        private struct ResourceSummary
        {
            public int CompanyCount;
            public int WarningCount;
            public int Over90Count;

            public long TotalServiceAvailable;
            public long TotalMaxService;
            public long TotalPhysicalStock;

            public long TotalCurrentCustomers;
            public long TotalMonthlyCustomers;
        }
    }
}

#endif
