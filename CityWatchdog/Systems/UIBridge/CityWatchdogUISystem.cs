// <copyright file="CityWatchdogUISystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/CityWatchdogUISystem.cs
// Purpose: UI lifecycle, hotkeys, city-load refresh, and notification count updates.

namespace CityWatchdog.Systems
{
    using System;
    using Colossal.Serialization.Entities;
    using CS2Shared.RiverMochi;
    using Game;
    using Game.Input;
    using Game.SceneFlow;

   // using Game.UI;

    public partial class CityWatchdogUISystem : UISystemBaseExtension
    {
        // Only the open panel or mini HUD scans.
        // Opening the panel or loading a city refreshes right away.
        private const int kPanelCountUpdateInterval = 256;      // Lower = faster updates, more work.
        private const int kMiniHudCountUpdateInterval = 256;    // 256 is ~4 sec at normal speed.

        private readonly int[] m_LastNotificationCounts = new int[AlertIconSystem.NotificationCountLength];
        private bool m_HasLastNotificationCounts;

        private AlertIconSystem m_AlertIconSystem = null!;
        private ProxyAction? m_ToggleNotificationsAction;
        private ProxyAction? m_ToggleNotificationPanelAction;
        // Close on city load so React drops the previous city's frozen alert snapshot.
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            if (!IsRealGameLoad(purpose, mode))
            {
                return;
            }

            m_PanelVisibleBinding.Update(false);

            // Mini HUD stays mounted across loads, so refresh before the next scheduled scan.
            m_HasLastNotificationCounts = false;
            m_NotificationCountUpdateState.ForceUpdate();
            m_MiniHudCountUpdateState.ForceUpdate();
        }

        // Ignore editor/main-menu transitions and save-outs; only a real city load should reset the panel.
        private static bool IsRealGameLoad(Purpose purpose, GameMode mode)
        {
            return mode == GameMode.Game &&
                (purpose == Purpose.NewGame || purpose == Purpose.LoadGame);
        }

        protected override void OnUpdate()
        {
            RefreshKeybindActions();

            if (!IsInGame())
            {
                return;
            }

            if (m_ToggleNotificationPanelAction?.WasReleasedThisFrame() == true)
            {
                ToggleControlPanelFromHotkey();
                return;
            }

            if (m_ToggleNotificationsAction?.WasReleasedThisFrame() == true)
            {
                ToggleAllNotificationsFromHotkey();
            }

            bool shouldUpdateCounts =
                m_PanelVisibleBinding.Value
                    ? m_NotificationCountUpdateState.Advance()
                    : m_MiniHudEnabledBinding.value && m_MiniHudCountUpdateState.Advance();

            if (shouldUpdateCounts)
            {
                int[] nextCounts = m_AlertIconSystem.GetNotificationCounts();

                if (!m_HasLastNotificationCounts || !AreSameNotificationCounts(m_LastNotificationCounts, nextCounts))
                {
                    Array.Copy(nextCounts, m_LastNotificationCounts, nextCounts.Length);
                    m_HasLastNotificationCounts = true;
                    m_NotificationCountsBinding.Update(nextCounts);
                }
            }
        }

        private static bool AreSameNotificationCounts(int[] previous, int[] next)
        {
            if (previous.Length != next.Length)
            {
                return false;
            }

            for (int i = 0; i < previous.Length; i++)
            {
                if (previous[i] != next[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void InitializeKeybindActions()
        {
            m_ToggleNotificationsAction = EnableAction(CwdSettings.ToggleNotificationsAction);
            m_ToggleNotificationPanelAction = EnableAction(CwdSettings.ToggleNotificationPanelAction);
        }

        private void RefreshKeybindActions()
        {
            m_ToggleNotificationsAction ??= EnableAction(CwdSettings.ToggleNotificationsAction);

            m_ToggleNotificationPanelAction ??= EnableAction(CwdSettings.ToggleNotificationPanelAction);
        }

        private static bool IsInGame()
        {
            return GameManager.instance != null &&
                   GameManager.instance.gameMode == GameMode.Game;
        }

        private static ProxyAction? EnableAction(string actionName)
        {
            try
            {
                ProxyAction? action = CwdSettings.Instance.GetAction(actionName);
                if (action != null)
                {
                    action.shouldBeEnabled = true;
                }

                return action;
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "missing-keybind-" + actionName,
                    () => $"Keybinding action '{actionName}' is unavailable: {ex.GetType().Name}: {ex.Message}",
                    ex);
                return null;
            }
        }

        private void OnControlPanelBindingToggle(bool value)
        {
            m_PanelVisibleBinding.Update(value);
            if (value)
            {
                m_NotificationCountUpdateState.ForceUpdate();
            }
        }

        private void ToggleControlPanelFromHotkey()
        {
            bool visible = !m_PanelVisibleBinding.Value;
            m_PanelVisibleBinding.Update(visible);
            if (visible)
            {
                m_NotificationCountUpdateState.ForceUpdate();
            }
        }
    }
}
