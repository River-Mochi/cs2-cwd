// <copyright file="LocaleEN.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleEN.cs
// Purpose: English (en-US) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleEN : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleEN(CwdSettings setting)
        {
            m_Settings = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {

            Dictionary<string, string> entries = new()
            {
                // --- Mod title ---
                { m_Settings.GetSettingsLocaleID(), Mod.ModName },

                // --- Tabs ---
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "Main" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "Mini-HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Key Bindings" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "About" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "USAGE" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Main Notification Panel" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Main Panel and Display" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Show Trends on menu bar" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Mini HUD Notifications" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "DIAGNOSTICS" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Show Instructions" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Show or hide the usage instructions below." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<City mode>\n" +
                    "Use Paw icon (top-left), or hotkey Shift+N, to open the Main Notification Panel.\n" +
                    "Drag the panel by the title bar. Use the arrow button to collapse or expand the panel.\n" +
                    "<Notification alerts>\n" +
                    "Use Show All to hide or show all Icons over buildings.\n" +
                    "Optional Presets [1] [2] only save your current [x] checkbox selection: click to load; hold 1 sec to save a new set of selected rows you want to show or hide.\n" +
                    "Blue Stars are for the Mini HUD favorites and not part of Presets [1] or [2].\n" +
                    "<Show Trends>\n" +
                    "Enable Population + Money trends and extra hover details on the bottom menu tooltips.\n" +
                    "<Editor mode>\n" +
                    "In Editor mode, press Shift+N to see the City Watchdog Editor special toolbar, a smaller toolset."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Toggle Notification Icons" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<Hotkey> for the same action as the in-game <[SHOW ALL]> button.\n" +
                    "It shows or hides all problem alert icons instantly.\n" +
                    "**CITY mode only.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Instant show/hide issue icons" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Open/Close Notification Panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<Hotkey> for opening or closing the\n" +
                    "<notification panel> in the city.\n" +
                    "Works the same as clicking the top-left City Watchdog icon.\n" +
                    "**Works in EDITOR mode — opens Editor Quick Controls.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Open/Close notification panel" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Always open big panel collapsed" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "When enabled [ ✓ ], always opens the city big CWD panel minimized to only 1-row of buttons.\n" +
                    "Use the title-bar arrow or click the button looks like [0/62] to expand and show the full panel."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "Disable City Watchdog tooltips" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "Hides most City Watchdog Main Panel tooltips.\n" +
                    "<Keep this OFF> is recommended for most players.\n" +
                    "Only affects City Watchdog — useful if you prefer a cleaner panel."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Hide/Show Road Names" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<Hotkey> to instantly hide or show the vanilla road name labels in the city.\n" +
                    "Same as clicking the Road-Name icon in the City Watchdog panel toolbar.\n" +
                    "**Works in EDITOR + CITY mode.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Hide/Show road names" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Disable All Game Tooltips" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<Hotkey> to instantly hide or show ALL game mouse hover over tooltips — buildings, cims, tools, and bottom menu icons.\n" +
                    "This toggle [x] is the Same as clicking the [i] icon in-city on the City Watchdog panel (both are synced).\n" +
                    "Does not touch this City Watchdog mod tooltips.\n" +
                    "**Works in EDITOR + CITY mode.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Hide/Show all game hover tooltips" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Bigger Game UI" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "When enabled [ ✓ ], the <whole game UI> is a little larger — game + all panels.\n" +
                    "This uses the game's own <Interface Dev Scale> option without requiring the <--developerMode> launch parameter.\n" +
                    "This [x] checkbox is synced with the scale button in the CWD title bar.\n" +
                    "Game text size only is not touched. Use Options > Interface > <Text Scaling>.\n" +
                    "This stays on until you turn it off, even if City Watchdog is removed.\n" +
                    "- Turn this off before uninstalling to return the interface to normal size.\n" +
                    "- Or launch just once with <--developerMode> and turn off Options > Interface > Interface Scaling (dev).\n" +
                    " since this option is normally only visible if the game is launched with <--developerMode>."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "CWD panel opacity" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Adjusts the background transparency.\n" +
                    "**Adjusts both the Main City Watchdog Panel and our panel in Editor**\n" +
                    "Lower values are more transparent. Higher is darker and more solid."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Population + Money trends" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Recommend Enable>\n" +
                    "Bottom game menu: Shows trend values with the game's bottom toolbar <money and population arrows>.\n" +
                    "This is a lightweight hover over toolbar feature <display only>;\n" +
                    "Saves time and possible better performance than opening game's Info view panel."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "View frequency" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Choose whether the bottom-toolbar trend text shows hourly or monthly values for money and population.\n" +
                    "Monthly uses budget income minus expenses for money, and a 24-hour projection for population."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Hourly (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Monthly (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Tooltip style" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Choose how much detail appears in the money hover tooltip.\n" +
                    "<Mini> shows minimalist Net in both /h and /mo.\n" +
                    "<Compact> shows Income, Expenses, and Net using your selected /h or /mo view.\n" +
                    "<Full Data> shows Income, Expenses, and Net in both /h and /mo."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Mini" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Compact" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Full Data" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Money font size" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Adjusts <font size> of Money View tooltip numbers.\n" +
                    "Game default = 100%\n" +
                    "<Mod default = 120%>\n" +
                    "Hover over Money at bottom of the screen.\n" +
                    "Requested by players who have hard time seeing smaller tooltips in the game."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Population font size" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Adjusts <font size> of population tooltip numbers.\n" +
                    "Game default = 100%\n" +
                    "<Mod default = 120%>\n" +
                    "Hover over Population at bottom of the screen."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Show Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Shows a small HUD panel.\n" + 
                    "Use it as a quick tiny alert strip without opening the full City Watchdog panel or the clutter of 1000's of icons all over the city.\n" +
                    "Clicking an icon jumps to the issue location on the map.\n" +
                    "Keep clicking the same icon to cycle through other issue hot spots, then back to the first one.\n" +
                    "**============================**\n" +
                    "One way this is used:\n" +
                    "1. Disable all Notification icons for the whole city with the button in the Main panel.\n" +
                    "2. Enable Mini HUD to show only your Favorite 5 or 10 alerts.\n" +
                    "3. In the full City Watchdog panel, check off each **Blue Star** that you want tracked (pick many, not just 10).\n" +
                    "4. Mini HUD shows only the top 5 or 10 from your Blue Star favorites list that have the highest counts.\n"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Click This - Quick Start" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Applies a <quick start> for Mini HUD:\n" +
                    "Includes a **starter set of Blue Star favorites**.\n" +
                    "In Favorites mode, Mini HUD shows the top 5 or 10 current counts from your **Blue Star** list.\n" +
                    "Add or remove **Blue Stars** in the City Watchdog panel.\n" +
                    "Sets: Favorites, 5 icons, horizontal, draggable, 100% size, dark panel, and hides zero counts.\n" +
                    "Run Quick Start again anytime to reset these settings."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Mini HUD Mode" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Choose which notification rows the Mini HUD uses.\n" +
                    "**Top active** alerts shows the highest current counts.\n" +
                    "**Favorites** includes all rows marked with **Blue Star** in the main City Watchdog panel.\n" +
                    "You can pick as many favorites as you want,\n" +
                    "but Mini HUD still shows only the top 5 or top 10 current counts from that **favorites blue-star** list."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "Top active alerts" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Favorites" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Icon count" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Choose how many notification icons the Mini HUD can show at once." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Icon size" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Scale Mini HUD icons + numbers.\n" +
                    "90% = compact. 100% = default.\n" +
                    "Increase up to 130% for better visibility.\n" +
                    "Decrease to 90% to make it smaller (hide it and make it less noticeable)."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Orientation" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Choose whether Mini HUD icons are arranged in a row or a column." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Horizontal" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Vertical" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "HUD placement" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Choose where the Mini HUD appears.\n" +
                    "Draggable lets you move it in the city UI."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Top center" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Top right" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Draggable" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Dark or Glass style" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Choose the Mini HUD background style.\n" +
                    "Glass panel goes from clear to a cloudy white tint; it does not get darker.\n" +
                    "Use Dark panel for a darker vanilla-style HUD."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Dark panel" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Glass panel" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Mini panel opacity" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Adjusts Mini HUD transparency.\n" +
                    "Lower values = more transparent.\n" +
                    "Higher values = more solid.\n" +
                    "Glass is more white/cloudy panel background. Dark becomes more solid/dark."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Hide zero alerts" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "When enabled [ ✓ ], the Mini HUD hides notification rows with a count of 0." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Mod name" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Display name of this mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Current mod version." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Mochi's Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Open the author's Paradox Mods page." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Debug Report to Log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<Not needed for normal gameplay.>\n" +
                    "For testers and post game-patch checks: writes a <Logs/CityWatchdog.log> report\n" +
                    "comparing live game notification prefabs with the notification icons Watchdog currently controls."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Open Log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "Opens </Logs/CityWatchdog.log> if it exists.\n" +
                    "If the log file is missing, opens the Logs/ folder instead."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
