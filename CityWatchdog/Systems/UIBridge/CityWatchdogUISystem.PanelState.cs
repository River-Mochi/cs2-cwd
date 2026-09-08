// <copyright file="CityWatchdogUISystem.PanelState.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/CityWatchdogUISystem.PanelState.cs
// Purpose: Mini HUD actions and persisted panel position, layout, sorting, and setting updates.

namespace CityWatchdog.Systems
{
    using System;
    using CS2Shared.RiverMochi;
    using Game.Rendering;
    using Game.Tools;
    using Unity.Entities;

    public partial class CityWatchdogUISystem
    {
        private void JumpToMiniHudNotification(int index)
        {
            if (!m_AlertIconSystem.TryGetNextNotificationEntity(index, out Entity entity) ||
                !EntityManager.Exists(entity))
            {
                return;
            }

            ToolSystem toolSystem = World.GetOrCreateSystemManaged<ToolSystem>();
            CameraUpdateSystem cameraSystem = World.GetOrCreateSystemManaged<CameraUpdateSystem>();

            toolSystem.selected = entity;
            cameraSystem.orbitCameraController.followedEntity = entity;
            cameraSystem.orbitCameraController.TryMatchPosition(cameraSystem.activeCameraController);
            cameraSystem.activeCameraController = cameraSystem.orbitCameraController;
        }

        private void ToggleMiniHudFavorite(int index)
        {
            if (index < 0 || index >= AlertIconSystem.NotificationCountLength)
            {
                return;
            }

            if (index < 31)
            {
                CwdSettings.Instance.MiniHudFavoriteMaskLow ^= 1 << index;
            }
            else
            {
                CwdSettings.Instance.MiniHudFavoriteMaskHigh ^= 1 << (index - 31);
            }

            try
            {
                CwdSettings.Instance.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "mini-hud-favorites-save",
                    () => $"Failed to save mini HUD favorites: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }

            m_MiniHudFavoritesBinding.Update(GetMiniHudFavoriteIndexes());
        }

        private void SaveMiniHudPosition(string payload)
        {
            if (CwdSettings.Instance.MiniHudPlacement != CwdSettings.kMiniHudPlacementDraggable ||
                string.IsNullOrWhiteSpace(payload))
            {
                return;
            }

            string[] parts = payload.Split(',');
            if (parts.Length != 3 ||
                !int.TryParse(parts[0], out int orientation) ||
                !int.TryParse(parts[1], out int x) ||
                !int.TryParse(parts[2], out int y))
            {
                return;
            }

            if (orientation != CwdSettings.Instance.MiniHudOrientation)
            {
                return;
            }

            x = Math.Clamp(x, -CwdSettings.kMiniHudPositionLimit, CwdSettings.kMiniHudPositionLimit);
            y = Math.Clamp(y, -CwdSettings.kMiniHudPositionLimit, CwdSettings.kMiniHudPositionLimit);

            if (orientation == CwdSettings.kMiniHudOrientationHorizontal)
            {
                if (CwdSettings.Instance.MiniHudHorizontalPositionX == x &&
                    CwdSettings.Instance.MiniHudHorizontalPositionY == y)
                {
                    return;
                }

                CwdSettings.Instance.MiniHudHorizontalPositionX = x;
                CwdSettings.Instance.MiniHudHorizontalPositionY = y;
            }
            else if (orientation == CwdSettings.kMiniHudOrientationVertical)
            {
                if (CwdSettings.Instance.MiniHudVerticalPositionX == x &&
                    CwdSettings.Instance.MiniHudVerticalPositionY == y)
                {
                    return;
                }

                CwdSettings.Instance.MiniHudVerticalPositionX = x;
                CwdSettings.Instance.MiniHudVerticalPositionY = y;
            }
            else
            {
                return;
            }

            CwdSettings.Instance.MiniHudPositionX = x;
            CwdSettings.Instance.MiniHudPositionY = y;
            CwdSettings.Instance.MiniHudPositionOrientation = orientation;
            UpdateMiniHudPositionBinding(x, y, orientation);

            try
            {
                CwdSettings.Instance.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "mini-hud-position-save",
                    () => $"Failed to save Mini HUD position: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private void SavePanelPosition(string payload)
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

            if (CwdSettings.Instance.PanelPositionX == x && CwdSettings.Instance.PanelPositionY == y)
            {
                return;
            }

            CwdSettings.Instance.PanelPositionX = x;
            CwdSettings.Instance.PanelPositionY = y;
            m_PanelPositionXBinding.Update(x);
            m_PanelPositionYBinding.Update(y);

            try
            {
                CwdSettings.Instance.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "panel-position-save",
                    () => $"Failed to save panel position: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private void SavePanelCollapsedSections(int mask)
        {
            if (CwdSettings.Instance.PanelCollapsedSectionsMask == mask)
            {
                return;
            }

            CwdSettings.Instance.PanelCollapsedSectionsMask = mask;
            m_PanelCollapsedSectionsMaskBinding.Update(mask);

            try
            {
                CwdSettings.Instance.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "panel-collapsed-sections-save",
                    () => $"Failed to save panel collapsed sections: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private void SavePanelSortMode(int mode)
        {
            if (mode < 0 || mode > 2 || CwdSettings.Instance.PanelSortMode == mode)
            {
                return;
            }

            CwdSettings.Instance.PanelSortMode = mode;
            m_PanelSortModeBinding.Update(mode);

            try
            {
                CwdSettings.Instance.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "panel-sort-mode-save",
                    () => $"Failed to save panel sort mode: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private static int[] GetMiniHudFavoriteIndexes()
        {
            System.Collections.Generic.List<int> favorites = new();
            for (int index = 0; index < AlertIconSystem.NotificationCountLength; index++)
            {
                int mask = index < 31
                    ? CwdSettings.Instance.MiniHudFavoriteMaskLow
                    : CwdSettings.Instance.MiniHudFavoriteMaskHigh;
                int bit = index < 31 ? index : index - 31;
                if ((mask & (1 << bit)) != 0)
                {
                    favorites.Add(index);
                }
            }

            return favorites.ToArray();
        }

        public void UpdateMoneyViewBinding(bool value) => m_MoneyViewBinding?.Update(value);

        public void UpdateMoneyViewModeBinding(int value) => m_MoneyViewModeBinding?.Update(value);

        public void UpdateMoneyTooltipModeBinding(int value) => m_MoneyTooltipModeBinding?.Update(value);

        public void UpdateMoneyTooltipFontScaleBinding(int value) => m_MoneyTooltipFontScaleBinding?.Update(value);

        public void UpdatePopulationTooltipFontScaleBinding(int value) => m_PopulationTooltipFontScaleBinding?.Update(value);

        public void UpdateMiniHudEnabledBinding(bool value)
        {
            m_MiniHudEnabledBinding?.Update(value);
            if (value)
            {
                m_MiniHudCountUpdateState?.ForceUpdate();
            }
        }

        public void UpdateMiniHudModeBinding(int value) => m_MiniHudModeBinding?.Update(value);

        public void UpdateMiniHudItemCountBinding(int value) => m_MiniHudItemCountBinding?.Update(value);

        public void UpdateMiniHudScaleBinding(int value) => m_MiniHudScaleBinding?.Update(value);

        public void UpdateMiniHudOrientationBinding(int value) => m_MiniHudOrientationBinding?.Update(value);

        public void UpdateMiniHudPlacementBinding(int value) => m_MiniHudPlacementBinding?.Update(value);

        public void UpdateMiniHudHideZeroBinding(bool value) => m_MiniHudHideZeroBinding?.Update(value);

        public void UpdateMiniHudPanelStyleBinding(int value) => m_MiniHudPanelStyleBinding?.Update(value);

        public void UpdateMiniHudPanelOpacityBinding(int value) => m_MiniHudPanelOpacityBinding?.Update(value);

        public void UpdateMiniHudFavoritesBinding() => m_MiniHudFavoritesBinding?.Update(GetMiniHudFavoriteIndexes());

        private void UpdateMiniHudPositionBinding(int x, int y, int orientation)
        {
            if (orientation == CwdSettings.kMiniHudOrientationHorizontal)
            {
                m_MiniHudHorizontalPositionXBinding?.Update(x);
                m_MiniHudHorizontalPositionYBinding?.Update(y);
            }
            else if (orientation == CwdSettings.kMiniHudOrientationVertical)
            {
                m_MiniHudVerticalPositionXBinding?.Update(x);
                m_MiniHudVerticalPositionYBinding?.Update(y);
            }
        }

        public void UpdateMiniHudPositionBindings()
        {
            m_MiniHudHorizontalPositionXBinding?.Update(CwdSettings.Instance.MiniHudHorizontalPositionX);
            m_MiniHudHorizontalPositionYBinding?.Update(CwdSettings.Instance.MiniHudHorizontalPositionY);
            m_MiniHudVerticalPositionXBinding?.Update(CwdSettings.Instance.MiniHudVerticalPositionX);
            m_MiniHudVerticalPositionYBinding?.Update(CwdSettings.Instance.MiniHudVerticalPositionY);
        }

        public void UpdatePanelButtonsOnlyStartBinding(bool value) => m_PanelButtonsOnlyStartBinding?.Update(value);

        public void UpdateMainPanelOpacityBinding(int value) => m_MainPanelOpacityBinding?.Update(value);
    }
}
