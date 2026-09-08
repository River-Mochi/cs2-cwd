// <copyright file="LocaleZH_HANS.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleZH_HANS.cs
// Purpose: Simplified Chinese (zh-HANS) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleZH_HANS : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleZH_HANS(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "主页" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "迷你 HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "快捷键" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "关于" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "使用说明" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "主通知面板" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "主面板与显示" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "菜单栏趋势" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "迷你 HUD 通知" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "诊断" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "显示说明" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "显示或隐藏下面的使用说明。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<城市模式>\n" +
                    "点击左上角爪子图标，或按 Shift+N 打开主面板。\n" +
                    "拖动标题栏移动面板。用箭头折叠或展开。\n" +
                    "<通知>\n" +
                    "用“显示图标”隐藏或显示警报。预设 1/2：点击加载；按住 1 秒保存。\n" +
                    "<趋势>\n" +
                    "开启人口 + 金钱趋势，并在底部菜单提示中显示更多信息。\n" +
                    "<编辑器>\n" +
                    "在编辑器中按 Shift+N 打开较小的 City Watchdog Editor 工具栏。"
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "切换通知图标" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "与游戏内 <[显示图标]> 按钮相同操作的<快捷键>。\n" +
                    "立即显示或隐藏所有问题图标。\n" +
                    "**仅城市模式。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "立即显示/隐藏问题图标" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "打开/关闭通知面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "在城市中打开或关闭\n" +
                    "<通知面板>的<快捷键>。\n" +
                    "与左上角 City Watchdog 图标相同。\n" +
                    "**在编辑器中会打开编辑器快速控制。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "打开/关闭通知面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "主面板：默认只展开 1 行" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "启用 [ ✓ ] 后，City Watchdog 首先只显示 1 行按钮。\n" +
                    "用标题栏箭头或 [0/62] 按钮展开完整面板。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "关闭 City Watchdog 提示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "隐藏 City Watchdog 主面板的大部分提示。\n" +
                    "可在这里重新开启，或点击标题栏的爪印图标恢复。\n" +
                    "只影响 City Watchdog。适合喜欢更简洁、提示更少的面板。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "隐藏/显示道路名称" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "立即隐藏或显示游戏原本道路名称的<快捷键>。\n" +
                    "与 City Watchdog 的道路名称图标相同。\n" +
                    "**编辑器 + 城市均可用。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "隐藏/显示道路名称" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "关闭所有游戏提示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "隐藏或显示游戏中所有鼠标悬停提示的<快捷键>——建筑、市民、工具和底部菜单图标等。\n" +
                    "这个 [x] 与 City Watchdog 面板中的 [i] 图标同步。\n" +
                    "不会影响 City Watchdog 自己的提示。\n" +
                    "**编辑器 + 城市均可用。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "隐藏/显示所有游戏提示" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "放大游戏界面" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "启用 [ ✓ ] 后，<整个游戏界面>都会变大——游戏和模组面板。\n" +
                    "直接使用游戏的 <Interface Scaling>，无需 <--developerMode>。\n" +
                    "这个 [x] 与 City Watchdog 标题栏的缩放按钮同步。\n" +
                    "只改文字大小：Options > Interface > <Text Scaling>。\n" +
                    "即使移除 City Watchdog，也会保持开启，直到你手动关闭。\n" +
                    "- 卸载前先关闭即可恢复正常大小。\n" +
                    "- 或用 <--developerMode> 启动一次，再关闭 Interface Scaling (dev)。"
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "CWD 面板透明度" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "调整背景透明度。\n" +
                    "**同时影响 City Watchdog 主面板和编辑器面板。**\n" +
                    "数值低 = 更透明；数值高 = 更暗、更实。"
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "人口 + 金钱趋势" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<建议开启>\n" +
                    "底部菜单：在<金钱和人口箭头>旁显示趋势值。\n" +
                    "这是轻量的<仅显示>悬停功能；\n" +
                    "可减少打开游戏信息视图的次数。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "显示频率" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "选择底部趋势按小时还是按月显示，金钱和人口都会一起切换。\n" +
                    "月度金钱 = 收入减支出；人口 = 24 小时预测。"
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "每小时 (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "每月 (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "提示样式" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "选择金钱提示显示多少信息。\n" +
                    "<极简> 只显示 /h 和 /mo 的净值。\n" +
                    "<紧凑> 只按你选的 /h 或 /mo 显示收入、支出和净值。\n" +
                    "<完整数据> 同时显示 /h 和 /mo 的收入、支出和净值。"
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "极简" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "紧凑" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "完整数据" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "金钱字体大小" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "调整金钱提示数字的<字体大小>。\n" +
                    "游戏默认 = 100%\n" +
                    "<模组默认 = 120%>\n" +
                    "把鼠标移到屏幕底部的金钱上查看。\n" +
                    "适合觉得小提示难读的玩家。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "人口字体大小" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "调整人口提示数字的<字体大小>。\n" +
                    "游戏默认 = 100%\n" +
                    "<模组默认 = 120%>\n" +
                    "把鼠标移到屏幕底部的人口上查看。"
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "显示 迷你 HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "显示一个小型 HUD 面板。\n" +
                    "不用打开完整 City Watchdog，也不用让城市里满是图标，就能快速看警报。\n" +
                    "点击图标跳到问题位置。继续点击可轮换其他位置。\n" +
                    "**============================**\n" +
                    "一种用法：\n" +
                    "1. 在主面板关闭所有普通通知图标。\n" +
                    "2. 开启 迷你 HUD，只看 5 或 10 个收藏。\n" +
                    "3. 在完整面板给想跟踪的项目加 **蓝色星标**。\n" +
                    "4. 迷你 HUD 会显示这份列表里当前数值最高的 5 或 10 项。\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "点击 - 快速开始" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "为 迷你 HUD 应用<快速开始>：\n" +
                    "包含一组**蓝色星标初始收藏**。\n" +
                    "收藏模式会显示列表中当前数值最高的 5 或 10 项。\n" +
                    "可在 City Watchdog 面板添加或移除**蓝色星标**。\n" +
                    "设置为：收藏、5 个图标、横向、可拖动、100%、深色面板、隐藏 0。\n" +
                    "需要时再次运行即可重置。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "迷你 HUD 模式" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "选择 迷你 HUD 使用哪些通知行。\n" +
                    "**最活跃警报**显示当前数值最高的项目。\n" +
                    "**收藏**使用主面板里所有带**蓝色星标**的行。\n" +
                    "收藏数量不限，\n" +
                    "但 迷你 HUD 只显示最高的 5 或 10 项。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "最活跃警报" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "收藏" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "图标数量" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "选择 迷你 HUD 一次最多显示多少个通知图标。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "图标大小" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "调整 迷你 HUD 图标和数字大小。\n" +
                    "90% = 紧凑；100% = 默认。\n" +
                    "最多 130%，更容易看清。\n" +
                    "想更小、更低调可用 90%。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "方向" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "选择 迷你 HUD 图标横向排列还是纵向排列。" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "横向" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "纵向" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "HUD 位置" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "选择 迷你 HUD 显示在哪里。\n" +
                    "可拖动模式可以在城市界面中自由移动。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "顶部居中" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "右上角" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "可拖动" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "深色或玻璃样式" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "选择 迷你 HUD 背景样式。\n" +
                    "玻璃会从透明变成偏白的雾状，不会变暗。\n" +
                    "想要更接近游戏的深色 HUD，请用深色面板。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "深色面板" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "玻璃面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Mini 面板透明度" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "调整 迷你 HUD 透明度。\n" +
                    "数值低 = 更透明。\n" +
                    "数值高 = 更实。\n" +
                    "玻璃会更白/更雾；深色面板会更暗、更实。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "隐藏 0 警报" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "启用 [ ✓ ] 后，迷你 HUD 会隐藏计数为 0 的通知行。" },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "模组名称" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "此模组的显示名称。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "版本" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "当前模组版本。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Mochi 的 Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "打开作者的 Paradox Mods 页面。" },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "把调试报告写入日志" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<正常游玩不需要。>\n" +
                    "供测试人员和游戏补丁后检查：把报告写入 <Logs/CityWatchdog.log>，\n" +
                    "比较当前游戏通知与 Watchdog 控制的图标。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "打开日志" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "如果存在则打开 </Logs/CityWatchdog.log>。\n" +
                    "否则打开 Logs/ 文件夹。"
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
