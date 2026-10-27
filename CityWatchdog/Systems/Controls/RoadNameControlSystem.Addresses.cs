// <copyright file="RoadNameControlSystem.Addresses.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/Controls/RoadNameControlSystem.Addresses.cs
// Purpose: Optional Addresses & Names compatibility via ECS Enabled (A&N v0.4.0.1+).

namespace CityWatchdog.Systems
{
    using System;
    using System.Reflection;
    using CS2Shared.RiverMochi;
    using Unity.Entities;

    public partial class RoadNameControlSystem
    {
        private const string kAddressesRendererTypeName = "BelzontAdr.AdrRoadNameLabelRenderSystem";
        private const float kAddressesRetrySeconds = 5f;

        private ComponentSystemBase? m_AddressesRenderer;
        private bool m_AddressesDisabledByCwd;
        private float m_NextAddressesLookup;

        private void ApplyAddressesRenderer(bool hideRequested)
        {
            if (!hideRequested)
            {
                RestoreAddressesRenderer();
                return;
            }

            if (m_AddressesRenderer == null)
            {
                TryDiscoverAddressesRenderer();
            }

            if (m_AddressesRenderer == null)
            {
                return;
            }

            try
            {
                if (m_AddressesRenderer.Enabled)
                {
                    m_AddressesRenderer.Enabled = false;

                    if (!m_AddressesDisabledByCwd)
                    {
                        m_AddressesDisabledByCwd = true;
                        LogUtils.Info("Road names: disabled Addresses & Names ECS renderer.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "road-name-addresses-disable",
                    () => $"Could not disable Addresses & Names ECS renderer: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private void TryDiscoverAddressesRenderer()
        {
            if (UnityEngine.Time.realtimeSinceStartup < m_NextAddressesLookup)
            {
                return;
            }

            // Only retry missing systems once every 5 seconds. No A&N DLL reference.
            m_NextAddressesLookup = UnityEngine.Time.realtimeSinceStartup + kAddressesRetrySeconds;

            try
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type? rendererType = assembly.GetType(kAddressesRendererTypeName, false);
                    if (rendererType == null)
                    {
                        continue;
                    }

                    if (!typeof(ComponentSystemBase).IsAssignableFrom(rendererType))
                    {
                        return;
                    }

                    // Do not instantiate the other mod's system.
                    m_AddressesRenderer = World.GetExistingSystemManaged(rendererType);
                    if (m_AddressesRenderer != null)
                    {
                        LogUtils.Info(() =>
                            $"Road names: found Addresses & Names ECS renderer (Enabled={m_AddressesRenderer.Enabled}).");
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "road-name-addresses-discover",
                    () => $"Could not locate Addresses & Names ECS renderer: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private void RestoreAddressesRenderer()
        {
            // Never turn on a renderer CWD did not previously disable.
            if (!m_AddressesDisabledByCwd || m_AddressesRenderer == null)
            {
                return;
            }

            try
            {
                m_AddressesRenderer.Enabled = true;
                m_AddressesDisabledByCwd = false;
                LogUtils.Info("Road names: restored Addresses & Names ECS renderer Enabled=true.");
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "road-name-addresses-restore",
                    () => $"Could not restore Addresses & Names ECS renderer: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }
    }
}
