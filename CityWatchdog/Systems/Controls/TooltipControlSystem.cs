// <copyright file="TooltipControlSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/TooltipControlSystem.cs
// Purpose: Controls vanilla and CWD tooltip visibility, including the tooltip hotkey.

namespace CityWatchdog.Systems
{
    using System;

    using CS2Shared.RiverMochi;

    using Game.Input;
    using Game.UI.Tooltip;

    public partial class TooltipControlSystem : UISystemBaseExtension
    {
        private const string kDisableAllTooltipsBindingName = "DisableAllTooltips";

        private BoolBinding m_DisableAllTooltipsBinding = null!;
        private BoolBinding m_DisableCwdTooltipsBinding = null!;
        private TooltipUISystem? m_CachedTooltipUISystem;
        private ProxyAction? m_ToggleAllTooltipsAction;

        protected override void OnCreate()
        {
            base.OnCreate();

            // Global game-tooltip toggle stays session-only.
            m_DisableAllTooltipsBinding = AddBoolBindingAndTriggerBinding(
                kDisableAllTooltipsBindingName,
                false,
                OnDisableAllTooltipsToggle);

            // CWD tooltips follow the saved Options toggle.
            m_DisableCwdTooltipsBinding = AddBoolBindingAndTriggerBinding(
                nameof(CwdSettings.DisableCwdTooltips),
                CwdSettings.Instance?.DisableCwdTooltips ?? false,
                OnDisableCwdTooltipsToggle);

            m_ToggleAllTooltipsAction = EnableHotkey(CwdSettings.ToggleAllTooltipsAction);
        }

        protected override void OnUpdate()
        {
            m_ToggleAllTooltipsAction ??= EnableHotkey(CwdSettings.ToggleAllTooltipsAction);

            if (m_ToggleAllTooltipsAction?.WasReleasedThisFrame() == true)
            {
                OnDisableAllTooltipsToggle(!m_DisableAllTooltipsBinding.Value);
            }

            m_CachedTooltipUISystem ??= World.GetExistingSystemManaged<TooltipUISystem>();
            if (m_CachedTooltipUISystem == null)
            {
                return;
            }

            bool desired = m_DisableAllTooltipsBinding.Value;
            if (m_CachedTooltipUISystem.hideTooltips != desired)
            {
                m_CachedTooltipUISystem.hideTooltips = desired;
            }
        }

        private void OnDisableAllTooltipsToggle(bool value)
        {
            m_DisableAllTooltipsBinding.Update(value);
            ApplyToGame(value);
        }

        private void OnDisableCwdTooltipsToggle(bool value)
        {
            m_DisableCwdTooltipsBinding.Update(value);

            // Title-bar paw restores CWD tooltips and keeps Options in sync.
            CwdSettings? setting = CwdSettings.Instance;
            if (setting == null || setting.DisableCwdTooltips == value)
            {
                return;
            }

            setting.DisableCwdTooltips = value;

            try
            {
                setting.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "save-cwd-tooltips",
                    () => $"Could not save CWD tooltip setting: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        internal void SetCwdTooltipsDisabled(bool value)
        {
            m_DisableCwdTooltipsBinding.Update(value);
        }

        private void ApplyToGame(bool value)
        {
            m_CachedTooltipUISystem ??= World.GetExistingSystemManaged<TooltipUISystem>();

            if (m_CachedTooltipUISystem != null)
            {
                // Public game API; no patching needed.
                m_CachedTooltipUISystem.hideTooltips = value;
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
                    "tooltip-hotkey-" + actionName,
                    () => $"Keybinding '{actionName}' unavailable: {ex.GetType().Name}: {ex.Message}",
                    ex);

                return null;
            }
        }
    }
}
