// <copyright file="CwdSettings.MiniHud.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Settings/CwdSettings.MiniHud.cs
// Purpose: Defines City Watchdog Mini HUD Options settings, defaults, and bindings.

namespace CityWatchdog
{
    using System;
    using CityWatchdog.Systems;
    using CS2Shared.RiverMochi;
    using Game.Settings;
    using Game.UI;
    using Game.UI.Widgets;

    public partial class CwdSettings
    {
        internal const int kMiniHudModeTopActive = 0;
        internal const int kMiniHudModeFavorites = 1;
        internal const int kMiniHudOrientationHorizontal = 0;
        internal const int kMiniHudOrientationVertical = 1;
        internal const int kMiniHudPlacementTopCenter = 0;
        internal const int kMiniHudPlacementTopRight = 1;
        internal const int kMiniHudPlacementDraggable = 2;
        internal const int kMiniHudPanelStyleDark = 0;
        internal const int kMiniHudPanelStyleGlass = 1;
        internal const int kMiniHudPanelOpacityDefault = 30;
        internal const int kMiniHudPositionLimit = 20000;
        // Bit positions are raw countIndex values (see notificationData.ts) — NOT re-derived from
        // any enum, so they must be hand-verified against the current index table whenever items are
        // inserted/removed. Bit 24 (Leveling Building) is deliberately NOT included: it's an optional,
        // positive-status row the player opts into manually, not a recommended "problem" alert.
        private const int kMiniHudRecommendedFavoriteMaskLow =
            (1 << 0) |  // Not enough electricity
            (1 << 1) |  // Electricity bottleneck
            (1 << 6) |  // Battery depleted
            (1 << 7) |  // Electric cable not connected
            (1 << 8) |  // Power line not connected
            (1 << 9) |  // Not enough water
            (1 << 11) | // Backed up sewer
            (1 << 12) | // Water pipe not connected
            (1 << 13) | // Sewer pipe not connected
            (1 << 19) | // Collapsed
            (1 << 20) | // Abandoned
            (1 << 21) | // Condemned
            (1 << 22) | // Deactivated
            (1 << 23) | // High rent
            (1 << 25) | // Traffic jam
            (1 << 27) | // Road required / no road access
            (1 << 28) | // Track not connected
            (1 << 29);  // No car access
        private const int kMiniHudRecommendedFavoriteMaskHigh =
            (1 << 1) |  // No pedestrian access
            (1 << 5) |  // Lack of labor
            (1 << 7) |  // Weather damage
            (1 << 9) |  // Water damage
            (1 << 12) | // On fire
            (1 << 13) | // Burned down
            (1 << 14) | // Garbage piling up
            (1 << 15) | // Facility full (Garbage)
            (1 << 18) | // Facility full (Healthcare)
            (1 << 19) | // Traffic accident
            (1 << 20) | // Crime scene
            (1 << 24) | // Low supplies
            (1 << 25) | // Out of fuel
            (1 << 26) | // Oil pipe not connected
            (1 << 29) | // Pathfinding failed
            (1 << 31);  // No vehicles



        // Mini-HUD tab - Mini HUD Notifications
        // --------------------------------------------------------------------

        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudEnabledChanged))]
        public bool MiniHudEnabled { get; set; }

        [SettingsUIButton]
        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        public bool ApplyMiniHudRecommendedPreset
        {
            set
            {
                if (!value)
                {
                    return;
                }

                ApplyMiniHudStarterPresetValues();

                CityWatchdogUISystem? uiSystem = GetUISystem();
                uiSystem?.UpdateMiniHudEnabledBinding(MiniHudEnabled);
                uiSystem?.UpdateMiniHudModeBinding(MiniHudMode);
                uiSystem?.UpdateMiniHudItemCountBinding(MiniHudItemCount);
                uiSystem?.UpdateMiniHudScaleBinding(MiniHudScale);
                uiSystem?.UpdateMiniHudOrientationBinding(MiniHudOrientation);
                uiSystem?.UpdateMiniHudPlacementBinding(MiniHudPlacement);
                uiSystem?.UpdateMiniHudHideZeroBinding(MiniHudHideZero);
                uiSystem?.UpdateMiniHudPanelStyleBinding(MiniHudPanelStyle);
                uiSystem?.UpdateMiniHudPanelOpacityBinding(MiniHudPanelOpacity);
                uiSystem?.UpdateMiniHudPositionBindings();
                uiSystem?.UpdateMiniHudFavoritesBinding();

                try
                {
                    ApplyAndSave();
                }
                catch (Exception ex)
                {
                    LogUtils.WarnOnce(
                        "mini-hud-recommended-preset-save",
                        () => $"Failed to save Mini HUD recommended preset: {ex.GetType().Name}: {ex.Message}",
                        ex);
                }
            }
        }

        [SettingsUIDropdown(typeof(CwdSettings), nameof(GetMiniHudModeItems))]
        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMiniHudEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudModeChanged))]
        public int MiniHudMode { get; set; }

        [SettingsUIDropdown(typeof(CwdSettings), nameof(GetMiniHudItemCountItems))]
        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMiniHudEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudItemCountChanged))]
        public int MiniHudItemCount { get; set; }

        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMiniHudEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudHideZeroChanged))]
        public bool MiniHudHideZero { get; set; }

        [SettingsUISlider(min = 90, max = 130, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMiniHudEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudScaleChanged))]
        public int MiniHudScale { get; set; }

        [SettingsUIDropdown(typeof(CwdSettings), nameof(GetMiniHudOrientationItems))]
        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMiniHudEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudOrientationChanged))]
        public int MiniHudOrientation { get; set; }

        [SettingsUIDropdown(typeof(CwdSettings), nameof(GetMiniHudPlacementItems))]
        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMiniHudEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudPlacementChanged))]
        public int MiniHudPlacement { get; set; }

        [SettingsUIDropdown(typeof(CwdSettings), nameof(GetMiniHudPanelStyleItems))]
        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMiniHudEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudPanelStyleChanged))]
        public int MiniHudPanelStyle { get; set; }

        [SettingsUISlider(min = 30, max = 100, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kMiniHudTab, kMiniHudGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMiniHudEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMiniHudPanelOpacityChanged))]
        public int MiniHudPanelOpacity { get; set; }

        [SettingsUIHidden]
        public bool MiniHudGlassStyle { get; set; }

        // Two 31-bit masks persist the 62 row favorites without exposing 62 Options toggles.
        [SettingsUIHidden]
        public int MiniHudFavoriteMaskLow { get; set; }

        [SettingsUIHidden]
        public int MiniHudFavoriteMaskHigh { get; set; }

        [SettingsUIHidden]
        public int MiniHudPositionX { get; set; }

        [SettingsUIHidden]
        public int MiniHudPositionY { get; set; }

        [SettingsUIHidden]
        public int MiniHudPositionOrientation { get; set; }

        [SettingsUIHidden]
        public int MiniHudHorizontalPositionX { get; set; }

        [SettingsUIHidden]
        public int MiniHudHorizontalPositionY { get; set; }

        [SettingsUIHidden]
        public int MiniHudVerticalPositionX { get; set; }

        [SettingsUIHidden]
        public int MiniHudVerticalPositionY { get; set; }



        // --------------------------------------------------------------------
        // Mini HUD conditions and helpers
        // --------------------------------------------------------------------

        public bool EnsureMiniHudEnabled()
        {
            return !MiniHudEnabled;
        }


        // --------------------------------------------------------------------
        // Mini HUD dropdown data
        // --------------------------------------------------------------------

        public DropdownItem<int>[] GetMiniHudModeItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = kMiniHudModeTopActive,
                    displayName = GetOptionLocaleID("MiniHudModeTopActive"),
                },
                new DropdownItem<int>
                {
                    value = kMiniHudModeFavorites,
                    displayName = GetOptionLocaleID("MiniHudModeFavorites"),
                },
            };
        }

        public DropdownItem<int>[] GetMiniHudItemCountItems()
        {
            return new[]
            {
                CreateDropdownItem(5),
                CreateDropdownItem(10),
            };
        }

        public DropdownItem<int>[] GetMiniHudOrientationItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = kMiniHudOrientationHorizontal,
                    displayName = GetOptionLocaleID("MiniHudOrientationHorizontal"),
                },
                new DropdownItem<int>
                {
                    value = kMiniHudOrientationVertical,
                    displayName = GetOptionLocaleID("MiniHudOrientationVertical"),
                },
            };
        }

        public DropdownItem<int>[] GetMiniHudPlacementItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = kMiniHudPlacementTopCenter,
                    displayName = GetOptionLocaleID("MiniHudPlacementTopCenter"),
                },
                new DropdownItem<int>
                {
                    value = kMiniHudPlacementTopRight,
                    displayName = GetOptionLocaleID("MiniHudPlacementTopRight"),
                },
                new DropdownItem<int>
                {
                    value = kMiniHudPlacementDraggable,
                    displayName = GetOptionLocaleID("MiniHudPlacementDraggable"),
                },
            };
        }

        public DropdownItem<int>[] GetMiniHudPanelStyleItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = kMiniHudPanelStyleDark,
                    displayName = GetOptionLocaleID("MiniHudPanelStyleDark"),
                },
                new DropdownItem<int>
                {
                    value = kMiniHudPanelStyleGlass,
                    displayName = GetOptionLocaleID("MiniHudPanelStyleGlass"),
                },
            };
        }

        private void ApplyMiniHudStarterPresetValues()
        {
            MiniHudEnabled = true;
            MiniHudMode = kMiniHudModeFavorites;
            MiniHudItemCount = 5;
            MiniHudScale = 100;
            MiniHudOrientation = kMiniHudOrientationHorizontal;
            MiniHudPlacement = kMiniHudPlacementDraggable;
            MiniHudHideZero = true;
            MiniHudPanelStyle = kMiniHudPanelStyleDark;
            MiniHudPanelOpacity = kMiniHudPanelOpacityDefault;
            MiniHudGlassStyle = false;
            MiniHudPositionX = 0;
            MiniHudPositionY = 0;
            MiniHudPositionOrientation = MiniHudOrientation;
            MiniHudHorizontalPositionX = 0;
            MiniHudHorizontalPositionY = 0;
            MiniHudVerticalPositionX = 0;
            MiniHudVerticalPositionY = 0;
            SetMiniHudRecommendedFavorites();
        }


        public void NormalizeLoadedSettings()
        {
            if (MiniHudPanelStyle != kMiniHudPanelStyleDark && MiniHudPanelStyle != kMiniHudPanelStyleGlass)
            {
                MiniHudPanelStyle = kMiniHudPanelStyleDark;
            }

            MiniHudPanelOpacity = MiniHudPanelOpacity <= 0
                ? kMiniHudPanelOpacityDefault
                : Math.Clamp(MiniHudPanelOpacity, 30, 100);
            MiniHudPositionX = Math.Clamp(MiniHudPositionX, -kMiniHudPositionLimit, kMiniHudPositionLimit);
            MiniHudPositionY = Math.Clamp(MiniHudPositionY, -kMiniHudPositionLimit, kMiniHudPositionLimit);
            if (MiniHudPositionOrientation != kMiniHudOrientationHorizontal &&
                MiniHudPositionOrientation != kMiniHudOrientationVertical)
            {
                MiniHudPositionOrientation = MiniHudOrientation;
            }

            if ((MiniHudPositionX != 0 || MiniHudPositionY != 0) &&
                MiniHudHorizontalPositionX == 0 &&
                MiniHudHorizontalPositionY == 0 &&
                MiniHudVerticalPositionX == 0 &&
                MiniHudVerticalPositionY == 0)
            {
                if (MiniHudPositionOrientation == kMiniHudOrientationHorizontal)
                {
                    MiniHudHorizontalPositionX = MiniHudPositionX;
                    MiniHudHorizontalPositionY = MiniHudPositionY;
                }
                else
                {
                    MiniHudVerticalPositionX = MiniHudPositionX;
                    MiniHudVerticalPositionY = MiniHudPositionY;
                }
            }

            MiniHudHorizontalPositionX = Math.Clamp(MiniHudHorizontalPositionX, -kMiniHudPositionLimit, kMiniHudPositionLimit);
            MiniHudHorizontalPositionY = Math.Clamp(MiniHudHorizontalPositionY, -kMiniHudPositionLimit, kMiniHudPositionLimit);
            MiniHudVerticalPositionX = Math.Clamp(MiniHudVerticalPositionX, -kMiniHudPositionLimit, kMiniHudPositionLimit);
            MiniHudVerticalPositionY = Math.Clamp(MiniHudVerticalPositionY, -kMiniHudPositionLimit, kMiniHudPositionLimit);

            PanelPositionX = Math.Clamp(PanelPositionX, -kPanelPositionLimit, kPanelPositionLimit);
            PanelPositionY = Math.Clamp(PanelPositionY, -kPanelPositionLimit, kPanelPositionLimit);
            EditorQuickControlsPositionX = Math.Clamp(EditorQuickControlsPositionX, -kPanelPositionLimit, kPanelPositionLimit);
            EditorQuickControlsPositionY = Math.Clamp(EditorQuickControlsPositionY, -kPanelPositionLimit, kPanelPositionLimit);
        }

        private void SetMiniHudRecommendedFavorites()
        {
            MiniHudFavoriteMaskLow = kMiniHudRecommendedFavoriteMaskLow;
            MiniHudFavoriteMaskHigh = kMiniHudRecommendedFavoriteMaskHigh;
        }

        private static void OnMiniHudEnabledChanged(bool value) => GetUISystem()?.UpdateMiniHudEnabledBinding(value);

        private static void OnMiniHudModeChanged(int value) => GetUISystem()?.UpdateMiniHudModeBinding(value);

        private static void OnMiniHudItemCountChanged(int value) => GetUISystem()?.UpdateMiniHudItemCountBinding(value);

        private static void OnMiniHudScaleChanged(int value) => GetUISystem()?.UpdateMiniHudScaleBinding(value);

        private static void OnMiniHudOrientationChanged(int value) => GetUISystem()?.UpdateMiniHudOrientationBinding(value);

        private static void OnMiniHudPlacementChanged(int value) => GetUISystem()?.UpdateMiniHudPlacementBinding(value);

        private static void OnMiniHudHideZeroChanged(bool value) => GetUISystem()?.UpdateMiniHudHideZeroBinding(value);

        private static void OnMiniHudPanelStyleChanged(int value) => GetUISystem()?.UpdateMiniHudPanelStyleBinding(value);

        private static void OnMiniHudPanelOpacityChanged(int value) => GetUISystem()?.UpdateMiniHudPanelOpacityBinding(value);
    }
}
