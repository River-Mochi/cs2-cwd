// <copyright file="RoadArrowControlSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/RoadArrowControlSystem.cs
// Purpose: Shows one-way road arrows while the default tool is active.

namespace CityWatchdog.Systems
{
    using System;
    using System.Reflection;
    using CS2Shared.RiverMochi;
    using Game.Tools;

    public partial class RoadArrowControlSystem : UISystemBaseExtension
    {
        private const string kArrowsPropertyName = "requireNetArrows";

        private BoolBinding m_ShowRoadArrowsBinding = null!;
        private DefaultToolSystem? m_VanillaDefaultTool;
        private PropertyInfo? m_ArrowsRequiredProperty;

        // When we set the flag to true we save the previous value (vanilla default is false).
        // Used to put the flag back where we found it on toggle-off and on system destroy.
        private bool m_OriginalFlagCaptured;
        private bool m_OriginalFlagValue;
        private bool m_ArrowsCurrentlyForced;

        protected override void OnCreate()
        {
            base.OnCreate();

            bool initial = CwdSettings.Instance?.ShowRoadArrows ?? false;
            m_ShowRoadArrowsBinding = AddBoolBindingAndTriggerBinding(
                nameof(CwdSettings.ShowRoadArrows),
                initial,
                OnShowRoadArrowsToggle);
        }

        protected override void OnDestroy()
        {
            // Restore vanilla default-tool flag on mod unload so the game is clean.
            if (m_ArrowsCurrentlyForced)
            {
                WriteArrowsFlag(m_OriginalFlagCaptured && m_OriginalFlagValue);
                m_ArrowsCurrentlyForced = false;
            }
            base.OnDestroy();
        }

        public void SyncFromSettings()
        {
            bool value = CwdSettings.Instance?.ShowRoadArrows ?? false;
            if (m_ShowRoadArrowsBinding.Value != value)
            {
                m_ShowRoadArrowsBinding.Update(value);
            }
            ApplyToGame(value);
        }

        protected override void OnUpdate()
        {
            // Reapply each tick so any code path that resets DefaultToolSystem.requireNetArrows
            // gets corrected. The write itself is idempotent — only fires when the value differs.
            ApplyToGame(CwdSettings.Instance?.ShowRoadArrows ?? false);
        }

        private void OnShowRoadArrowsToggle(bool value)
        {
            m_ShowRoadArrowsBinding.Update(value);

            CwdSettings? setting = CwdSettings.Instance;
            if (setting != null)
            {
                setting.ShowRoadArrows = value;
                TryPersist(setting);
            }

            ApplyToGame(value);
        }

        private void ApplyToGame(bool show)
        {
            if (show && !m_ArrowsCurrentlyForced)
            {
                CaptureOriginalFlag();
                if (WriteArrowsFlag(true))
                {
                    m_ArrowsCurrentlyForced = true;
                }
            }
            else if (!show && m_ArrowsCurrentlyForced)
            {
                WriteArrowsFlag(m_OriginalFlagCaptured && m_OriginalFlagValue);
                m_ArrowsCurrentlyForced = false;
            }
        }

        private void CaptureOriginalFlag()
        {
            if (m_OriginalFlagCaptured)
            {
                return;
            }
            if (!ResolveReflectionTargets())
            {
                return;
            }

            try
            {
                m_OriginalFlagValue = (bool)(m_ArrowsRequiredProperty!.GetValue(m_VanillaDefaultTool) ?? false);
                m_OriginalFlagCaptured = true;
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "road-arrow-read",
                    () => $"Failed to read DefaultToolSystem.{kArrowsPropertyName}: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private bool WriteArrowsFlag(bool target)
        {
            if (!ResolveReflectionTargets())
            {
                return false;
            }

            try
            {
                m_ArrowsRequiredProperty!.SetValue(m_VanillaDefaultTool, target);
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "road-arrow-write",
                    () => $"Failed to write DefaultToolSystem.{kArrowsPropertyName}={target}: {ex.GetType().Name}: {ex.Message}",
                    ex);
                return false;
            }
        }

        private bool ResolveReflectionTargets()
        {
            if (m_VanillaDefaultTool == null)
            {
                m_VanillaDefaultTool = World.GetExistingSystemManaged<DefaultToolSystem>();
                if (m_VanillaDefaultTool == null)
                {
                    return false;
                }
            }

            if (m_ArrowsRequiredProperty == null)
            {
                // Property is public, but its setter is internal. Reflection keeps the hook
                // limited to this one vanilla property w/out Harmony.
                m_ArrowsRequiredProperty = typeof(ToolBaseSystem).GetProperty(
                    kArrowsPropertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (m_ArrowsRequiredProperty == null || !m_ArrowsRequiredProperty.CanWrite)
                {
                    LogUtils.WarnOnce(
                        "road-arrow-prop",
                        () => $"ToolBaseSystem.{kArrowsPropertyName} not found or not writable; show-road-arrows toggle disabled.");
                    return false;
                }
            }

            return true;
        }

        private static void TryPersist(CwdSettings setting)
        {
            try
            {
                setting.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "road-arrow-save",
                    () => $"Failed to persist ShowRoadArrows: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }
    }
}
