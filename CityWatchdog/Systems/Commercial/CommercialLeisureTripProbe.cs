// <copyright file="CommercialLeisureTripProbe.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Commercial/CommercialLeisureTripProbe.cs
// Purpose: DEBUG-only marker for tracing corrective leisure trips.

#if DEBUG

using Game.Economy;

using Unity.Entities;

namespace CityWatchdog.Systems
{
    internal struct CommercialLeisureTripProbe : IComponentData
    {
        public Entity Provider;
        public Entity Building;

        public Resource Resource;

        public uint StartFrame;
        public uint ArrivalFrame;

        public int StartService;
        public byte StartLeisureCounter;
        public byte Flags;



    }
}

#endif
