// <copyright file="CwdSettings.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Settings/CwdSettings.cs
// Purpose: Defines shared City Watchdog settings shell, tabs, common Options UI, and key bindings.

namespace CityWatchdog
{
    using System;

    using CityWatchdog.Systems;

    using Colossal.IO.AssetDatabase;

    using CS2Shared.RiverMochi;

    using Game;
    using Game.Input;
    using Game.Modding;
    using Game.SceneFlow;
    using Game.Settings;
    using Game.UI;
    using Game.UI.Widgets;

    using Unity.Entities;

    using UnityEngine;

    [FileLocation("ModsSettings/CityWatchdog/CityWatchdog")]
    [SettingsUITabOrder(kActions, kMiniHudTab, kHotkeys, kAbout)]
    [SettingsUIGroupOrder(
        kAboutUsage,
        kNotifications,
        kHotkeyActions,
        kMoneyViewGroup,
        kMiniHudGroup,
        kAboutInfo,
        kAboutLinks,
        kAboutDiagnostics,
        kSerialize)]
    [SettingsUIShowGroupName(
        kAboutUsage,
        kNotifications,
        kHotkeyActions,
        kMoneyViewGroup,
        kMiniHudGroup,
        kAboutDiagnostics,
        kSerialize)]
    public partial class CwdSettings : ModSetting
    {
        internal static CwdSettings Instance { get; set; } = null!;

        // Tab IDs.
        internal const string kActions = "Actions";
        internal const string kMiniHudTab = "MiniHud";
        internal const string kHotkeys = "Hotkeys";
        internal const string kAbout = "About";
        internal const string kDebug = "Debug";
        internal const string kSerialize = "Serialize";

        // Keybinding action IDs.
        public const string ToggleNotificationsAction = nameof(ToggleNotificationsAction);
        public const string ToggleNotificationPanelAction = nameof(ToggleNotificationPanelAction);
        public const string ToggleRoadNamesAction = nameof(ToggleRoadNamesAction);
        public const string ToggleAllTooltipsAction = nameof(ToggleAllTooltipsAction);

        // Group IDs.
        internal const string kNotifications = "Notifications";
        internal const string kHotkeyActions = "HotkeyActions";
        internal const string kMoneyViewGroup = "MoneyViewGroup";
        internal const string kMiniHudGroup = "MiniHudGroup";
        internal const string kAboutInfo = "AboutInfo";
        internal const string kAboutLinks = "AboutLinks";
        internal const string kAboutDiagnostics = "AboutDiagnostics";
        internal const string kAboutUsage = "AboutUsage";

        // Stored panel position gets real viewport clamping in the UI.
        internal const int kPanelPositionLimit = 20000;
        internal const int kMainPanelOpacityDefault = 80;

        private const string kAboutLinksRow = "AboutLinksRow";
        private const string kDebugButtonsRow = "1DebugButtonsRow";
        private const string kUsageIconPath = "coui://ui-mods/images/NotificationIcon_PawRainbow.svg";
        private const string kUrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        private int m_MainPanelOpacity = kMainPanelOpacityDefault;

        public CwdSettings(IMod mod)
            : base(mod)
        {
            SetDefaults();
        }

        // --------------------------------------------------------------------
        // Main tab - Usage
        // --------------------------------------------------------------------

        [SettingsUISection(kActions, kAboutUsage)]
        public bool ShowUsage { get; set; }

        [SettingsUIMultilineText(kUsageIconPath)]
        [SettingsUIHideByCondition(typeof(CwdSettings), nameof(HideUsageText))]
        [SettingsUISection(kActions, kAboutUsage)]
        public string UsageText => string.Empty;

        // --------------------------------------------------------------------
        // Main tab - Main Notification Panel
        // --------------------------------------------------------------------

        [SettingsUISection(kActions, kNotifications)]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnPanelButtonsOnlyStartChanged))]
        public bool PanelButtonsOnlyStart { get; set; }

        [SettingsUISlider(min = 30, max = 100, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(kActions, kNotifications)]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnMainPanelOpacityChanged))]
        public int MainPanelOpacity
        {
            get => m_MainPanelOpacity;
            set => m_MainPanelOpacity = value <= 0
                ? kMainPanelOpacityDefault
                : Math.Clamp(value, 30, 100);
        }

        // Mirrors vanilla Interface Scaling (dev).
        [SettingsUISection(kActions, kNotifications)]
        public bool InterfaceScaling
        {
            get => GameManager.instance?.settings?.userInterface?.interfaceScaling ?? false;
            set => World.DefaultGameObjectInjectionWorld?
                .GetExistingSystemManaged<InterfaceScaleControlSystem>()?
                .SetInterfaceScaling(value);
        }

        // Can be turned OFF here. The title-bar paw can restore tooltips in-game.
        [SettingsUISection(kActions, kNotifications)]
        [SettingsUISetter(typeof(CwdSettings), nameof(OnDisableCwdTooltipsChanged))]
        public bool DisableCwdTooltips { get; set; }

        // --------------------------------------------------------------------
        // Key Bindings tab - Main panel and display
        // --------------------------------------------------------------------

        [SettingsUIKeyboardBinding(BindingKeyboard.N, ToggleNotificationPanelAction, shift: true)]
        [SettingsUISection(kHotkeys, kHotkeyActions)]
        public ProxyBinding ToggleNotificationPanelKeyboardBinding { get; set; }

        [SettingsUIKeyboardBinding(BindingKeyboard.N, ToggleNotificationsAction)]
        [SettingsUISection(kHotkeys, kHotkeyActions)]
        public ProxyBinding ToggleNotificationsKeyboardBinding { get; set; }

        [SettingsUIKeyboardBinding(BindingKeyboard.Backslash, ToggleRoadNamesAction)]
        [SettingsUISection(kHotkeys, kHotkeyActions)]
        public ProxyBinding ToggleRoadNamesKeyboardBinding { get; set; }

        [SettingsUIKeyboardBinding(BindingKeyboard.Backslash, ToggleAllTooltipsAction, shift: true)]
        [SettingsUISection(kHotkeys, kHotkeyActions)]
        public ProxyBinding ToggleAllTooltipsKeyboardBinding { get; set; }

        // Persisted across sessions but controlled from the in-game panel/hotkeys.
        [SettingsUIHidden]
        public bool HideRoadNames { get; set; }

        [SettingsUIHidden]
        public bool HideDistrictNames { get; set; }

        [SettingsUIHidden]
        public bool ShowRoadArrows { get; set; }

        // Last position of the draggable main panel.
        [SettingsUIHidden]
        public int PanelPositionX { get; set; }

        [SettingsUIHidden]
        public int PanelPositionY { get; set; }

        // Separate Editor quick-controls position.
        [SettingsUIHidden]
        public int EditorQuickControlsPositionX { get; set; }

        [SettingsUIHidden]
        public int EditorQuickControlsPositionY { get; set; }

        // bit set = collapsed.
        [SettingsUIHidden]
        public int PanelCollapsedSectionsMask { get; set; }

        // 0 = A->Z, 1 = Z->A, 2 = Active-first.
        [SettingsUIHidden]
        public int PanelSortMode { get; set; }

        // --------------------------------------------------------------------
        // About tab
        // --------------------------------------------------------------------

        [SettingsUISection(kAbout, kAboutInfo)]
        public string NameText => Mod.ModName;

        [SettingsUISection(kAbout, kAboutInfo)]
        public string VersionText =>
#if DEBUG
            Mod.ModVersion + " (DEBUG)";
#else
            Mod.ModVersion;
#endif

        [SettingsUIButtonGroup(kAboutLinksRow)]
        [SettingsUIButton]
        [SettingsUISection(kAbout, kAboutLinks)]
        public bool OpenParadox
        {
            set
            {
                if (value)
                {
                    TryOpenUrl(kUrlParadox);
                }
            }
        }

        // --------------------------------------------------------------------
        // About tab - Diagnostics
        // --------------------------------------------------------------------

        [SettingsUIButtonGroup(kDebugButtonsRow)]
        [SettingsUIButton]
        [SettingsUISection(kAbout, kAboutDiagnostics)]
        public bool WriteNotificationAuditLog
        {
            set
            {
                if (!value)
                {
                    return;
                }

                AlertIconSystem? alertIconSystem = World.DefaultGameObjectInjectionWorld?
                    .GetExistingSystemManaged<AlertIconSystem>();

                if (alertIconSystem == null)
                {
                    LogUtils.Info(() => "Notification audit skipped: AlertIconSystem is not available.");
                    return;
                }

                LogUtils.Info(() => "Notification audit requested from Options UI.");
                alertIconSystem.WriteNotificationAuditLog();
            }
        }

        [SettingsUIButtonGroup(kDebugButtonsRow)]
        [SettingsUIButton]
        [SettingsUISection(kAbout, kAboutDiagnostics)]
        public bool OpenLog
        {
            set
            {
                if (!value)
                {
                    return;
                }

                ShellOpen.OpenModLogOrLogsFolder();
            }
        }

        // --------------------------------------------------------------------
        // Conditions and helpers
        // --------------------------------------------------------------------

        private static bool IsInGame()
        {
            return GameManager.instance != null && GameManager.instance.gameMode == GameMode.Game;
        }

        public bool NotInGame => !IsInGame();

        public bool InEditor => GameManager.instance != null && GameManager.instance.gameMode == GameMode.Editor;

        public bool InMainMenu => GameManager.instance != null && GameManager.instance.gameMode == GameMode.MainMenu;

        private bool HideUsageText()
        {
            return !ShowUsage;
        }

        private static void TryOpenUrl(string url)
        {
            try
            {
                Application.OpenURL(url);
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "open-url-" + url,
                    () => $"Failed to open URL '{url}': {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        public override void SetDefaults()
        {
            ShowUsage = false;

            DisableCwdTooltips = false;
            HideRoadNames = false;
            HideDistrictNames = false;
            ShowRoadArrows = false;

            PanelButtonsOnlyStart = false;
            MainPanelOpacity = kMainPanelOpacityDefault;
            PanelPositionX = 0;
            PanelPositionY = 0;
            EditorQuickControlsPositionX = 0;
            EditorQuickControlsPositionY = 0;
            PanelCollapsedSectionsMask = 0;
            PanelSortMode = 0;

            // Population + Money trends stay in CWD.
            MoneyView = true;
            MoneyViewMode = kMoneyViewModeMonthly;
            MoneyTooltipMode = kMoneyTooltipModeFullData;
            MoneyTooltipFontScale = 120;
            PopulationTooltipFontScale = 120;

            ApplyMiniHudStarterPresetValues();

            Notification.SetDefaults();
            ResetPresets();
        }

        private static void OnPanelButtonsOnlyStartChanged(bool value) =>
            GetUISystem()?.UpdatePanelButtonsOnlyStartBinding(value);

        private static void OnMainPanelOpacityChanged(int value) =>
            GetUISystem()?.UpdateMainPanelOpacityBinding(value);

        private static void OnDisableCwdTooltipsChanged(bool value)
        {
            World.DefaultGameObjectInjectionWorld?
                .GetExistingSystemManaged<TooltipControlSystem>()?
                .SetCwdTooltipsDisabled(value);
        }

        private static CityWatchdogUISystem? GetUISystem()
        {
            return World.DefaultGameObjectInjectionWorld?
                .GetExistingSystemManaged<CityWatchdogUISystem>();
        }

        private static DropdownItem<int> CreateDropdownItem(int value)
        {
            return new DropdownItem<int>
            {
                value = value,
                displayName = value.ToString("N0"),
            };
        }

        public string GetOptionLocaleID(string localeId)
        {
            return $"Options[{id}.{localeId}]";
        }

        public string GetUILocaleID(string entryId)
        {
            return $"{Mod.ModId}.UI[{entryId}]";
        }
    }
}
