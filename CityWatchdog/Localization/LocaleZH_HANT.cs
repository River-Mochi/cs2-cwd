// <copyright file="LocaleZH_HANT.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleZH_HANT.cs
// Purpose: Traditional Chinese (zh-HANT) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleZH_HANT : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleZH_HANT(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "主頁" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "迷你 HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "快捷鍵" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "關於" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "使用說明" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "主通知面板" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "主面板與顯示" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "選單列趨勢" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "迷你 HUD 通知" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "診斷" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "顯示說明" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "顯示或隱藏下方的使用說明。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<城市模式>\n" +
                    "點擊左上角爪子圖示，或按 Shift+N 開啟主面板。\n" +
                    "拖曳標題列移動面板。用箭頭收合或展開。\n" +
                    "<通知>\n" +
                    "用「顯示圖示」隱藏或顯示警報。預設 1/2：點擊載入；按住 1 秒儲存。\n" +
                    "<趨勢>\n" +
                    "開啟人口 + 金錢趨勢，並在底部選單提示中顯示更多資訊。\n" +
                    "<編輯器>\n" +
                    "在編輯器中按 Shift+N 開啟較小的 City Watchdog Editor 工具列。"
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "切換通知圖示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "與遊戲內 <[顯示圖示]> 按鈕相同操作的<快捷鍵>。\n" +
                    "立即顯示或隱藏所有問題圖示。\n" +
                    "**僅城市模式。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "立即顯示/隱藏問題圖示" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "開啟/關閉通知面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "在城市中開啟或關閉\n" +
                    "<通知面板>的<快捷鍵>。\n" +
                    "與左上角 City Watchdog 圖示相同。\n" +
                    "**在編輯器中會開啟編輯器快速控制。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "開啟/關閉通知面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "主面板：預設只展開 1 列" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "啟用 [ ✓ ] 後，City Watchdog 一開始只顯示 1 列按鈕。\n" +
                    "用標題列箭頭或 [0/62] 按鈕展開完整面板。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "關閉 City Watchdog 提示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "隱藏 City Watchdog 主面板的大部分提示。\n" +
                    "可在這裡重新開啟，或點擊標題列的爪印圖示恢復。\n" +
                    "只影響 City Watchdog。適合喜歡更簡潔、提示更少的面板。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "隱藏/顯示道路名稱" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "立即隱藏或顯示遊戲原本道路名稱的<快捷鍵>。\n" +
                    "與 City Watchdog 的道路名稱圖示相同。\n" +
                    "**編輯器 + 城市都可用。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "隱藏/顯示道路名稱" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "關閉所有遊戲提示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "隱藏或顯示遊戲中所有滑鼠懸停提示的<快捷鍵>——建築、市民、工具和底部選單圖示等。\n" +
                    "這個 [x] 與 City Watchdog 面板中的 [i] 圖示同步。\n" +
                    "不會影響 City Watchdog 自己的提示。\n" +
                    "**編輯器 + 城市都可用。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "隱藏/顯示所有遊戲提示" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "放大遊戲介面" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "啟用 [ ✓ ] 後，<整個遊戲介面>都會變大——遊戲和模組面板。\n" +
                    "直接使用遊戲的 <Interface Scaling>，不需要 <--developerMode>。\n" +
                    "這個 [x] 與 City Watchdog 標題列的縮放按鈕同步。\n" +
                    "只改文字大小：Options > Interface > <Text Scaling>。\n" +
                    "就算移除 City Watchdog，也會保持開啟直到你手動關閉。\n" +
                    "- 解除安裝前先關閉即可恢復正常大小。\n" +
                    "- 或用 <--developerMode> 啟動一次，再關閉 Interface Scaling (dev)。"
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "CWD 面板透明度" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "調整背景透明度。\n" +
                    "**同時影響 City Watchdog 主面板和編輯器面板。**\n" +
                    "數值低 = 更透明；數值高 = 更暗、更實。"
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "人口 + 金錢趨勢" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<建議開啟>\n" +
                    "底部選單：在<金錢和人口箭頭>旁顯示趨勢值。\n" +
                    "這是輕量的<只顯示>懸停功能；\n" +
                    "可減少打開遊戲資訊檢視的次數。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "顯示頻率" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "選擇底部趨勢按小時還是按月顯示，金錢和人口會一起切換。\n" +
                    "月度金錢 = 收入減支出；人口 = 24 小時預測。"
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "每小時 (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "每月 (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "提示樣式" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "選擇金錢提示顯示多少資訊。\n" +
                    "<極簡> 只顯示 /h 和 /mo 的淨值。\n" +
                    "<緊湊> 只按你選的 /h 或 /mo 顯示收入、支出和淨值。\n" +
                    "<完整資料> 同時顯示 /h 和 /mo 的收入、支出和淨值。"
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "極簡" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "緊湊" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "完整資料" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "金錢字體大小" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "調整金錢提示數字的<字體大小>。\n" +
                    "遊戲預設 = 100%\n" +
                    "<模組預設 = 120%>\n" +
                    "把滑鼠移到畫面底部的金錢上查看。\n" +
                    "適合覺得小提示難讀的玩家。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "人口字體大小" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "調整人口提示數字的<字體大小>。\n" +
                    "遊戲預設 = 100%\n" +
                    "<模組預設 = 120%>\n" +
                    "把滑鼠移到畫面底部的人口上查看。"
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "顯示 迷你 HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "顯示一個小型 HUD 面板。\n" +
                    "不用打開完整 City Watchdog，也不用讓城市裡滿是圖示，就能快速看警報。\n" +
                    "點擊圖示跳到問題位置。繼續點擊可輪換其他位置。\n" +
                    "**============================**\n" +
                    "一種用法：\n" +
                    "1. 在主面板關閉所有一般通知圖示。\n" +
                    "2. 開啟 迷你 HUD，只看 5 或 10 個收藏。\n" +
                    "3. 在完整面板給想追蹤的項目加 **藍色星號**。\n" +
                    "4. 迷你 HUD 會顯示這份清單裡目前數值最高的 5 或 10 項。\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "點擊 - 快速開始" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "為 迷你 HUD 套用<快速開始>：\n" +
                    "包含一組**藍色星號初始收藏**。\n" +
                    "收藏模式會顯示清單中目前數值最高的 5 或 10 項。\n" +
                    "可在 City Watchdog 面板新增或移除**藍色星號**。\n" +
                    "設定為：收藏、5 個圖示、橫向、可拖曳、100%、深色面板、隱藏 0。\n" +
                    "需要時再次執行即可重設。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "迷你 HUD 模式" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "選擇 迷你 HUD 使用哪些通知列。\n" +
                    "**最活躍警報**顯示目前數值最高的項目。\n" +
                    "**收藏**使用主面板裡所有帶**藍色星號**的列。\n" +
                    "收藏數量不限，\n" +
                    "但 迷你 HUD 只顯示最高的 5 或 10 項。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "最活躍警報" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "收藏" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "圖示數量" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "選擇 迷你 HUD 一次最多顯示多少個通知圖示。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "圖示大小" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "調整 迷你 HUD 圖示和數字大小。\n" +
                    "90% = 緊湊；100% = 預設。\n" +
                    "最多 130%，更容易看清。\n" +
                    "想更小、更低調可用 90%。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "方向" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "選擇 迷你 HUD 圖示橫向排列還是直向排列。" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "橫向" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "直向" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "HUD 位置" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "選擇 迷你 HUD 顯示在哪裡。\n" +
                    "可拖曳模式可以在城市介面中自由移動。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "頂部置中" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "右上角" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "可拖曳" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "深色或玻璃樣式" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "選擇 迷你 HUD 背景樣式。\n" +
                    "玻璃會從透明變成偏白的霧狀，不會變暗。\n" +
                    "想要較暗的遊戲風格，請用深色面板。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "深色面板" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "玻璃面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Mini 面板透明度" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "調整 迷你 HUD 透明度。\n" +
                    "數值低 = 更透明。\n" +
                    "數值高 = 更實。\n" +
                    "玻璃會更白/更霧；深色面板會更暗、更實。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "隱藏 0 警報" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "啟用 [ ✓ ] 後，迷你 HUD 會隱藏計數為 0 的通知列。" },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "模組名稱" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "此模組的顯示名稱。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "版本" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "目前模組版本。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Mochi 的 Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "開啟作者的 Paradox Mods 頁面。" },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "把除錯報告寫入日誌" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<一般遊玩不需要。>\n" +
                    "供測試人員和遊戲更新後檢查：把報告寫入 <Logs/CityWatchdog.log>，\n" +
                    "比較目前遊戲通知與 Watchdog 控制的圖示。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "開啟日誌" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "如果存在就開啟 </Logs/CityWatchdog.log>。\n" +
                    "否則開啟 Logs/ 資料夾。"
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
