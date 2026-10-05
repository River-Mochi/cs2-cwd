// <copyright file="CommercialProblemSpotDebugSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialProblemSpotDebugSystem.cs
// Purpose: DEBUG-only logging of current NEC leisure/hotel problem buildings for Scene Explorer checks.

#if DEBUG

using System.Globalization;

using CS2Shared.RiverMochi;

using Game;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;

using Unity.Entities;

namespace CityWatchdog.Systems
{
    public partial class CommercialProblemSpotDebugSystem :
        GameSystemBase
    {
        private struct ProblemSpot
        {
            public Entity Company;
            public Entity Building;

            public Resource Resource;

            public int ServiceAvailable;
            public int MaxService;

            public bool IsSet;
        }

        public override int GetUpdateInterval(
            SystemUpdatePhase phase)
        {
            // Light read-only diagnostic. This is frequent enough to give us
            // fresh entity IDs during a five-minute 3x test without spamming
            // the log every simulation update.
            return 8192;
        }

        protected override void OnUpdate()
        {
            ComponentLookup<
                Game.Companies.ServiceCompanyData>
                serviceCompanyDatas =
                    SystemAPI.GetComponentLookup<
                        Game.Companies.ServiceCompanyData>(
                            true);

            ComponentLookup<
                IndustrialProcessData>
                industrialProcessDatas =
                    SystemAPI.GetComponentLookup<
                        IndustrialProcessData>(
                            true);

            ProblemSpot entertainment =
                default;

            ProblemSpot meals =
                default;

            ProblemSpot recreation =
                default;

            ProblemSpot lodging =
                default;

            foreach ((
                RefRO<Game.Companies.ServiceAvailable>
                    serviceRef,
                RefRO<Game.Companies.CompanyNotifications>
                    notificationsRef,
                RefRO<PrefabRef>
                    prefabRef,
                RefRO<Game.Buildings.PropertyRenter>
                    propertyRenterRef,
                Entity company) in
                SystemAPI
                    .Query<
                        RefRO<
                            Game.Companies.ServiceAvailable>,
                        RefRO<
                            Game.Companies.CompanyNotifications>,
                        RefRO<
                            PrefabRef>,
                        RefRO<
                            Game.Buildings.PropertyRenter>>()
                    .WithAll<
                        Game.Companies.CommercialCompany>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>()
                    .WithEntityAccess())
            {
                if (notificationsRef.ValueRO
                        .m_NoCustomersEntity ==
                    Entity.Null)
                {
                    continue;
                }

                Entity companyPrefab =
                    prefabRef.ValueRO
                        .m_Prefab;

                if (!serviceCompanyDatas.HasComponent(
                        companyPrefab) ||
                    !industrialProcessDatas.HasComponent(
                        companyPrefab))
                {
                    continue;
                }

                Game.Companies.ServiceCompanyData
                    serviceData =
                        serviceCompanyDatas[
                            companyPrefab];

                if (serviceData.m_MaxService <= 0)
                {
                    continue;
                }

                Resource resource =
                    industrialProcessDatas[
                        companyPrefab]
                        .m_Output
                        .m_Resource;

                ProblemSpot candidate =
                    new()
                    {
                        Company =
                            company,

                        Building =
                            propertyRenterRef.ValueRO
                                .m_Property,

                        Resource =
                            resource,

                        ServiceAvailable =
                            serviceRef.ValueRO
                                .m_ServiceAvailable,

                        MaxService =
                            serviceData.m_MaxService,

                        IsSet =
                            true,
                    };

                switch (resource)
                {
                    case Resource.Entertainment:
                        KeepHigher(
                            ref entertainment,
                            candidate);
                        break;

                    case Resource.Meals:
                        KeepHigher(
                            ref meals,
                            candidate);
                        break;

                    case Resource.Recreation:
                        KeepHigher(
                            ref recreation,
                            candidate);
                        break;

                    case Resource.Lodging:
                        KeepHigher(
                            ref lodging,
                            candidate);
                        break;
                }
            }

            LogSpot(
                entertainment);

            LogSpot(
                meals);

            LogSpot(
                recreation);

            LogSpot(
                lodging);
        }

        private static void KeepHigher(
            ref ProblemSpot current,
            ProblemSpot candidate)
        {
            if (!candidate.IsSet)
            {
                return;
            }

            if (!current.IsSet)
            {
                current =
                    candidate;

                return;
            }

            long candidateScaled =
                (long)candidate.ServiceAvailable *
                current.MaxService;

            long currentScaled =
                (long)current.ServiceAvailable *
                candidate.MaxService;

            if (candidateScaled >
                currentScaled)
            {
                current =
                    candidate;
            }
        }

        private static void LogSpot(
            ProblemSpot spot)
        {
            if (!spot.IsSet)
            {
                return;
            }

            float unusedPercent =
                100f *
                spot.ServiceAvailable /
                spot.MaxService;

            LogUtils.Info(
                "[CWD-SPOT] " +
                $"resource={spot.Resource} " +
                $"building={spot.Building.Index}:{spot.Building.Version} " +
                $"company={spot.Company.Index}:{spot.Company.Version} " +
                $"service={spot.ServiceAvailable}/{spot.MaxService} " +
                $"unused=" +
                $"{unusedPercent.ToString("F1", CultureInfo.InvariantCulture)}%");
        }
    }
}

#endif
