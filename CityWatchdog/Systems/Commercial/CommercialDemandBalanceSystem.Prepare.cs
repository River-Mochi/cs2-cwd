// <copyright file="CommercialDemandBalanceSystem.Prepare.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialDemandBalanceSystem.Prepare.cs
// Purpose: DEBUG-only snapshot of households immediately before vanilla creates shopping needs.

#if DEBUG

using System.Collections.Generic;

using Colossal.Serialization.Entities;

using Game;
using Game.Citizens;
using Game.Economy;
using Game.Simulation;

using Unity.Entities;

namespace CityWatchdog.Systems
{
    /// <summary>
    /// Records households whose shopping need is empty immediately before
    /// vanilla HouseholdBehaviorSystem runs.
    /// </summary>
    public partial class CommercialDemandBalancePrepareSystem : GameSystemBase
    {
        private readonly HashSet<Entity> m_EmptyNeeds = new();

        private SimulationSystem m_SimulationSystem = null!;
        private uint m_CapturedSimulationFrame = uint.MaxValue;

        internal bool HasSnapshotFor(uint simulationFrame)
        {
            return m_CapturedSimulationFrame == simulationFrame;
        }

        internal bool WasEmptyBeforeVanilla(Entity household)
        {
            return m_EmptyNeeds.Contains(household);
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 262144 /
                (HouseholdBehaviorSystem.kUpdatesPerDay * 16);
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_SimulationSystem =
                World.GetOrCreateSystemManaged<SimulationSystem>();
        }

        protected override void OnGameLoaded(Context serializationContext)
        {
            base.OnGameLoaded(serializationContext);

            m_EmptyNeeds.Clear();
            m_CapturedSimulationFrame = uint.MaxValue;
        }

        protected override void OnUpdate()
        {
            m_EmptyNeeds.Clear();

            uint simulationFrame =
                m_SimulationSystem.frameIndex;

            uint updateFrame =
                SimulationUtils.GetUpdateFrameWithInterval(
                    simulationFrame,
                    (uint)GetUpdateInterval(
                        SystemUpdatePhase.GameSimulation),
                    16);

            foreach ((
                RefRO<Game.Citizens.HouseholdNeed> needRef,
                Entity household) in
                SystemAPI
                    .Query<
                        RefRO<Game.Citizens.HouseholdNeed>>()
                    .WithAll<
                        Game.Citizens.Household,
                        Game.Citizens.HouseholdCitizen,
                        Game.Economy.Resources>()
                    .WithAll<
                        Game.Buildings.PropertyRenter>()
                    .WithNone<
                        Game.Citizens.TouristHousehold,
                        Game.Citizens.HomelessHousehold,
                        Game.Agents.MovingAway>()
                    .WithNone<
                        Game.Common.Deleted,
                        Game.Tools.Temp>()
                    .WithSharedComponentFilter(
                        new Game.Simulation.UpdateFrame(updateFrame))
                    .WithEntityAccess())
            {
                if (needRef.ValueRO.m_Resource ==
                    Resource.NoResource)
                {
                    m_EmptyNeeds.Add(household);
                }
            }

            m_CapturedSimulationFrame =
                simulationFrame;
        }
    }
}

#endif
