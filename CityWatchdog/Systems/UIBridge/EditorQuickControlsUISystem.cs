// <copyright file="EditorQuickControlsUISystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/UIBridge/EditorQuickControlsUISystem.cs
// Purpose: Editor-only quick-controls panel lifecycle, hotkey, and draggable position.

namespace CityWatchdog.Systems
{
    using System;

    using Colossal.UI.Binding;

    using CS2Shared.RiverMochi;

    using Game;
    using Game.Input;
    // Editor-only quick controls; no city simulation or AlertIconSystem access.
    public sealed partial class EditorQuickControlsUISystem : UISystemBaseExtension
    {
        private const string kEnabledBindingName = "EditorQuickControlsEnabled";
        private const string kPositionChangedBindingName = "EditorQuickControlsPositionChanged";

        private BoolBinding m_EnabledBinding = null!;
        private ValueBinding<int> m_PositionXBinding = null!;
        private ValueBinding<int> m_PositionYBinding = null!;
        private ProxyAction? m_TogglePanelAction;

        public override GameMode gameMode => GameMode.Editor;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_EnabledBinding = AddBoolBindingAndTriggerBinding(
                kEnabledBindingName,
                false,
                OnEnabledChanged);
            m_PositionXBinding = AddValueBinding(
                nameof(CwdSettings.EditorQuickControlsPositionX),
                CwdSettings.Instance.EditorQuickControlsPositionX);
            m_PositionYBinding = AddValueBinding(
                nameof(CwdSettings.EditorQuickControlsPositionY),
                CwdSettings.Instance.EditorQuickControlsPositionY);
            AddTriggerBinding<string>(
                kPositionChangedBindingName,
                SavePosition);

            m_TogglePanelAction = EnableAction();
        }

        protected override void OnUpdate()
        {
            m_TogglePanelAction ??= EnableAction();

            if (m_TogglePanelAction?.WasReleasedThisFrame() == true)
            {
                m_EnabledBinding.Update(!m_EnabledBinding.Value);
            }
        }

        private void OnEnabledChanged(bool enabled)
        {
            m_EnabledBinding.Update(enabled);
        }

        private void SavePosition(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                return;
            }

            string[] parts = payload.Split(',');
            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out int x) ||
                !int.TryParse(parts[1], out int y))
            {
                return;
            }

            x = Math.Clamp(x, -CwdSettings.kPanelPositionLimit, CwdSettings.kPanelPositionLimit);
            y = Math.Clamp(y, -CwdSettings.kPanelPositionLimit, CwdSettings.kPanelPositionLimit);

            CwdSettings settings = CwdSettings.Instance;
            if (settings.EditorQuickControlsPositionX == x &&
                settings.EditorQuickControlsPositionY == y)
            {
                return;
            }

            settings.EditorQuickControlsPositionX = x;
            settings.EditorQuickControlsPositionY = y;
            m_PositionXBinding.Update(x);
            m_PositionYBinding.Update(y);

            try
            {
                settings.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "editor-quick-controls-position-save",
                    () => $"Failed to save Editor Quick Controls position: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private static ProxyAction? EnableAction()
        {
            try
            {
                ProxyAction? action = CwdSettings.Instance.GetAction(
                    CwdSettings.ToggleNotificationPanelAction);
                if (action != null)
                {
                    action.shouldBeEnabled = true;
                }

                return action;
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "editor-quick-controls-hotkey",
                    () => $"Editor Quick Controls hotkey is unavailable: {ex.GetType().Name}: {ex.Message}",
                    ex);
                return null;
            }
        }
    }
}
