// <copyright file="CwdSettings.MoneyView.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Settings/CwdSettings.MoneyView.cs
// Purpose: Defines City Watchdog Population + Money trend Options settings.

namespace CityWatchdog
{
    using CityWatchdog.Systems;

    using Game.Settings;
    using Game.UI;
    using Game.UI.Widgets;

    using Unity.Entities;

    public partial class CwdSettings
    {
        internal const int kMoneyViewModeHourly = 0;
        internal const int kMoneyViewModeMonthly = 1;
        internal const int kMoneyTooltipModeFullData = 0;
        internal const int kMoneyTooltipModeCompact = 1;
        internal const int kMoneyTooltipModeMini = 2;

        // Actions tab - Population + Money trends
        // --------------------------------------------------------------------

        [SettingsUISection(kActions, kMoneyViewGroup)]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMoneyViewChanged))]
        public bool MoneyView { get; set; }

        [SettingsUIDropdown(typeof(CwdSettings), nameof(GetMoneyViewModeItems))]
        [SettingsUISection(kActions, kMoneyViewGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMoneyViewEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMoneyViewModeChanged))]
        public int MoneyViewMode { get; set; }

        [SettingsUIDropdown(typeof(CwdSettings), nameof(GetMoneyTooltipModeItems))]
        [SettingsUISection(kActions, kMoneyViewGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMoneyViewEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMoneyTooltipModeChanged))]
        public int MoneyTooltipMode { get; set; }

        // UI converts 90..130 directly into 0.90em..1.30em.
        [SettingsUISlider(min = 90, max = 130, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActions, kMoneyViewGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMoneyViewEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMoneyTooltipFontScaleChanged))]
        public int MoneyTooltipFontScale { get; set; }

        [SettingsUISlider(min = 90, max = 130, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActions, kMoneyViewGroup)]
        [SettingsUIDisableByCondition(typeof(CwdSettings), nameof(EnsureMoneyViewEnabled))]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnPopulationTooltipFontScaleChanged))]
        public int PopulationTooltipFontScale { get; set; }

        public bool EnsureMoneyViewEnabled()
        {
            return !MoneyView;
        }

        public DropdownItem<int>[] GetMoneyViewModeItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = kMoneyViewModeHourly,
                    displayName = GetOptionLocaleID("MoneyViewModeHourly"),
                },
                new DropdownItem<int>
                {
                    value = kMoneyViewModeMonthly,
                    displayName = GetOptionLocaleID("MoneyViewModeMonthly"),
                },
            };
        }

        public DropdownItem<int>[] GetMoneyTooltipModeItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = kMoneyTooltipModeMini,
                    displayName = GetOptionLocaleID("MoneyTooltipModeMini"),
                },
                new DropdownItem<int>
                {
                    value = kMoneyTooltipModeCompact,
                    displayName = GetOptionLocaleID("MoneyTooltipModeCompact"),
                },
                new DropdownItem<int>
                {
                    value = kMoneyTooltipModeFullData,
                    displayName = GetOptionLocaleID("MoneyTooltipModeFullData"),
                },
            };
        }

        private void ApplyMoneyViewDefaults()
        {
            MoneyView = true;
            MoneyViewMode = kMoneyViewModeMonthly;
            MoneyTooltipMode = kMoneyTooltipModeFullData;

            // Keep in sync with UI/src/bindings/bindings.tsx fallbacks.
            MoneyTooltipFontScale = 120;
            PopulationTooltipFontScale = 120;
        }

        private static void OnMoneyViewChanged(bool value)
        {
            World.DefaultGameObjectInjectionWorld?
                .GetExistingSystemManaged<CityWatchdogUISystem>()?
                .UpdateMoneyViewBinding(value);
        }

        private static void OnMoneyViewModeChanged(int value)
        {
            World.DefaultGameObjectInjectionWorld?
                .GetExistingSystemManaged<CityWatchdogUISystem>()?
                .UpdateMoneyViewModeBinding(value);
        }

        private static void OnMoneyTooltipModeChanged(int value)
        {
            World.DefaultGameObjectInjectionWorld?
                .GetExistingSystemManaged<CityWatchdogUISystem>()?
                .UpdateMoneyTooltipModeBinding(value);
        }

        private static void OnMoneyTooltipFontScaleChanged(int value)
        {
            World.DefaultGameObjectInjectionWorld?
                .GetExistingSystemManaged<CityWatchdogUISystem>()?
                .UpdateMoneyTooltipFontScaleBinding(value);
        }

        private static void OnPopulationTooltipFontScaleChanged(int value)
        {
            World.DefaultGameObjectInjectionWorld?
                .GetExistingSystemManaged<CityWatchdogUISystem>()?
                .UpdatePopulationTooltipFontScaleBinding(value);
        }
    }
}
