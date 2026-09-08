// <copyright file="RoadNameControlSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/RoadNameControlSystem.cs
// Purpose: Hides road names w/out blocking road arrows.

namespace CityWatchdog.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using CS2Shared.RiverMochi;
    using Game.Input;
    using Game.Rendering;
    using Game.Tools;
    using UnityEngine;
    using UnityEngine.Rendering;

    public partial class RoadNameControlSystem : UISystemBaseExtension
    {
        private const string kAggregateRenderMethodName = "Render";

        private BoolBinding m_HideRoadNamesBinding = null!;
        private AggregateRenderSystem? m_CachedAggregateRenderSystem;
        private ToolSystem? m_CachedToolSystem;
        private Action<ScriptableRenderContext, List<Camera>>? m_CachedRenderDelegate;
        private bool m_CurrentlyUnsubscribed;
        private ProxyAction? m_ToggleAction;

        protected override void OnCreate()
        {
            base.OnCreate();

            bool initial = CwdSettings.Instance?.HideRoadNames ?? false;
            m_HideRoadNamesBinding = AddBoolBindingAndTriggerBinding(
                nameof(CwdSettings.HideRoadNames),
                initial,
                OnHideRoadNamesToggle);

            m_ToggleAction = EnableHotkey(CwdSettings.ToggleRoadNamesAction);
        }

        protected override void OnDestroy()
        {
            // Restore vanilla rendering on mod unload so the game is clean.
            if (m_CurrentlyUnsubscribed && m_CachedRenderDelegate != null)
            {
                try
                {
                    RenderPipelineManager.beginContextRendering += m_CachedRenderDelegate;
                }
                catch (Exception ex)
                {
                    LogUtils.WarnOnce(
                        "road-name-restore",
                        () => $"Failed to re-subscribe AggregateRenderSystem.Render on destroy: {ex.GetType().Name}: {ex.Message}",
                        ex);
                }
                m_CurrentlyUnsubscribed = false;
            }
            base.OnDestroy();
        }

        public void SyncFromSettings()
        {
            bool value = CwdSettings.Instance?.HideRoadNames ?? false;
            if (m_HideRoadNamesBinding.Value != value)
            {
                m_HideRoadNamesBinding.Update(value);
            }
            ApplyToGame();
        }

        protected override void OnUpdate()
        {
            m_ToggleAction ??= EnableHotkey(CwdSettings.ToggleRoadNamesAction);

            if (m_ToggleAction?.WasReleasedThisFrame() == true)
            {
                bool current = CwdSettings.Instance?.HideRoadNames ?? false;
                OnHideRoadNamesToggle(!current);
            }

            // Re-evaluate every frame because two of the inputs (tool active, arrows-force setting)
            // can change w/out us being notified. Subscribe/unsubscribe writes themselves are
            // idempotent — only run on transition.
            ApplyToGame();
        }

        private void OnHideRoadNamesToggle(bool value)
        {
            m_HideRoadNamesBinding.Update(value);

            CwdSettings? setting = CwdSettings.Instance;
            if (setting != null)
            {
                setting.HideRoadNames = value;
                TryPersist(setting);
            }

            ApplyToGame();
        }

        private void ApplyToGame()
        {
            if (m_CachedAggregateRenderSystem == null)
            {
                m_CachedAggregateRenderSystem = World.GetExistingSystemManaged<AggregateRenderSystem>();
                if (m_CachedAggregateRenderSystem == null)
                {
                    return;
                }
            }

            // Road-name text is baked into GPU textures, so changing localization would not
            // reliably hide existing labels. Control vanilla render callback instead.
            if (m_CachedRenderDelegate == null)
            {
                m_CachedRenderDelegate = BuildRenderDelegate(m_CachedAggregateRenderSystem);
                if (m_CachedRenderDelegate == null)
                {
                    LogUtils.WarnOnce(
                        "road-name-render-delegate",
                        () => "Could not bind a delegate to AggregateRenderSystem.Render; road-name toggle disabled.");
                    return;
                }
            }

            CwdSettings? setting = CwdSettings.Instance;
            bool hideRequested = setting?.HideRoadNames ?? false;
            bool arrowsForced = setting?.ShowRoadArrows ?? false;
            bool toolWantsArrows = NetToolWantsArrows();

            // Only suppress vanilla Render when road names are config hidden AND nothing else needs
            // the arrows path. When arrows are forced or a net tool is active, vanilla naturally
            // skips the names loop, so we let it run — gives us arrows + no names for free.
            bool shouldBeUnsubscribed = hideRequested && !arrowsForced && !toolWantsArrows;

            if (shouldBeUnsubscribed && !m_CurrentlyUnsubscribed)
            {
                RenderPipelineManager.beginContextRendering -= m_CachedRenderDelegate;
                m_CurrentlyUnsubscribed = true;
            }
            else if (!shouldBeUnsubscribed && m_CurrentlyUnsubscribed)
            {
                RenderPipelineManager.beginContextRendering += m_CachedRenderDelegate;
                m_CurrentlyUnsubscribed = false;
            }
        }

        private bool NetToolWantsArrows()
        {
            if (m_CachedToolSystem == null)
            {
                m_CachedToolSystem = World.GetExistingSystemManaged<ToolSystem>();
                if (m_CachedToolSystem == null)
                {
                    return false;
                }
            }
            return m_CachedToolSystem.activeTool != null && m_CachedToolSystem.activeTool.requireNetArrows;
        }

        private static Action<ScriptableRenderContext, List<Camera>>? BuildRenderDelegate(AggregateRenderSystem system)
        {
            MethodInfo? method = typeof(AggregateRenderSystem).GetMethod(
                kAggregateRenderMethodName,
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (method == null)
            {
                return null;
            }

            try
            {
                return (Action<ScriptableRenderContext, List<Camera>>)Delegate.CreateDelegate(
                    typeof(Action<ScriptableRenderContext, List<Camera>>),
                    system,
                    method);
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "road-name-delegate-create",
                    () => $"Delegate.CreateDelegate failed for AggregateRenderSystem.Render: {ex.GetType().Name}: {ex.Message}",
                    ex);
                return null;
            }
        }

        private static ProxyAction? EnableHotkey(string actionName)
        {
            try
            {
                ProxyAction? action = CwdSettings.Instance?.GetAction(actionName);
                if (action != null)
                {
                    action.shouldBeEnabled = true;
                }
                return action;
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "road-name-hotkey-" + actionName,
                    () => $"Keybinding '{actionName}' unavailable: {ex.GetType().Name}: {ex.Message}",
                    ex);
                return null;
            }
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
                    "road-name-save",
                    () => $"Failed to persist HideRoadNames: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }
    }
}
