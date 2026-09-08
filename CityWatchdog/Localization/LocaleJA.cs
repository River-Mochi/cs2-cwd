// <copyright file="LocaleJA.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleJA.cs
// Purpose: Japanese (ja-JP) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleJA : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleJA(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "メイン" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "ミニ HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "キー設定" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "情報" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "使い方" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "メイン通知パネル" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "メインパネルと表示" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "メニューバーのトレンド" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "ミニ HUD 通知" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "診断" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "使い方を表示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "下の使い方を表示／非表示にします。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<都市モード>\n" +
                    "左上の肉球アイコンか Shift+N でメインパネルを開きます。\n" +
                    "タイトルバーをドラッグして移動。矢印で折りたたみ／展開します。\n" +
                    "<通知>\n" +
                    "「アイコン表示」で警告を表示／非表示。プリセット1/2はクリックで読込、1秒長押しで保存。\n" +
                    "<トレンド>\n" +
                    "人口＋資金のトレンドと、下部メニューの追加ツールチップ情報を表示します。\n" +
                    "<エディター>\n" +
                    "エディターでは Shift+N で小さい City Watchdog Editor ツールバーを開きます。"
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "通知アイコン切替" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "ゲーム内の <[アイコン表示]> と同じ操作の<ホットキー>です。\n" +
                    "問題アイコンをすぐ表示／非表示にします。\n" +
                    "**都市モードのみ。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "問題アイコンを即表示／非表示" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "通知パネルを開く／閉じる" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "都市で<通知パネル>を開閉する\n" +
                    "<ホットキー>です。\n" +
                    "左上の City Watchdog アイコンと同じ動作です。\n" +
                    "**エディターではクイックコントロールを開きます。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "通知パネルを開く／閉じる" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "メインパネル：1行だけで開く" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "有効 [ ✓ ] にすると、City Watchdog は最初にボタン1行だけで開きます。\n" +
                    "タイトルの矢印か [0/62] ボタンで全体を展開します。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "City Watchdogのツールチップを無効化" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "City Watchdogメインパネルのほとんどのツールチップを非表示にします。\n" +
                    "ここで戻すか、タイトルバーの足あとアイコンをクリックすると再び表示できます。\n" +
                    "City Watchdogだけに影響します。ツールチップを減らしてすっきり表示したい時に便利です。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "道路名を表示／非表示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "ゲーム標準の道路名をすぐ表示／非表示にする<ホットキー>です。\n" +
                    "City Watchdog の道路名アイコンと同じ動作です。\n" +
                    "**エディター＋都市で使用可。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "道路名を表示／非表示" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "ゲームの全ツールチップを無効化" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "建物、市民、ツール、下部メニューなど、ゲームのマウスオーバー用ツールチップをすべて表示／非表示にする<ホットキー>です。\n" +
                    "この [x] は City Watchdog パネルの [i] と同期します。\n" +
                    "City Watchdog 自体のツールチップには影響しません。\n" +
                    "**エディター＋都市で使用可。**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "ゲームの全ツールチップを表示／非表示" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "ゲームUIを大きく" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "有効 [ ✓ ] にすると、<ゲーム全体のUI>（ゲーム＋MODパネル）が大きくなります。\n" +
                    "<Interface Scaling> を <--developerMode> なしで使います。\n" +
                    "この [x] は City Watchdog タイトルバーの拡大ボタンと同期します。\n" +
                    "文字だけなら Options > Interface > <Text Scaling>。\n" +
                    "City Watchdog を外しても、オフにするまで残ります。\n" +
                    "- アンインストール前にオフにすると通常サイズへ戻ります。\n" +
                    "- または <--developerMode> で一度起動し、Interface Scaling (dev) をオフにします。"
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "CWD パネル透明度" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "背景の透明度を調整します。\n" +
                    "**City Watchdog メインパネルとエディターパネルの両方に反映。**\n" +
                    "低いほど透明、高いほど濃くなります。"
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "人口＋資金トレンド" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<有効推奨>\n" +
                    "下部メニューの<資金と人口の矢印>にトレンド値を表示します。\n" +
                    "軽い<表示だけ>のホバー機能です。\n" +
                    "ゲームの情報ビューを開く手間を減らせます。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "表示単位" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "下部バーのトレンドを、資金と人口ともに毎時か毎月で表示します。\n" +
                    "月表示は資金が収入－支出、人口は24時間換算です。"
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "毎時 (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "毎月 (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "ツールチップ表示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "資金ツールチップの情報量を選びます。\n" +
                    "<ミニ> は純額を /h と /mo で表示。\n" +
                    "<コンパクト> は収入・支出・純額を選んだ /h または /mo だけ表示。\n" +
                    "<全データ> は収入・支出・純額を /h と /mo の両方で表示。"
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "ミニ" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "コンパクト" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "全データ" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "資金フォントサイズ" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "資金ツールチップ数値の<文字サイズ>を調整します。\n" +
                    "ゲーム標準 = 100%\n" +
                    "<MOD標準 = 120%>\n" +
                    "画面下の資金にカーソルを合わせて確認できます。\n" +
                    "小さいツールチップが見づらい方向け。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "人口フォントサイズ" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "人口ツールチップ数値の<文字サイズ>を調整します。\n" +
                    "ゲーム標準 = 100%\n" +
                    "<MOD標準 = 120%>\n" +
                    "画面下の人口にカーソルを合わせて確認できます。"
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "ミニ HUD を表示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "小さい HUD パネルを表示します。\n" +
                    "フルパネルや大量の街中アイコンを出さずに、必要な警告だけ確認できます。\n" +
                    "アイコンをクリックすると問題地点へ移動。続けてクリックすると他の地点を順番に表示します。\n" +
                    "**============================**\n" +
                    "使い方の例：\n" +
                    "1. メインパネルで通常の通知アイコンを全部オフ。\n" +
                    "2. ミニ HUD を有効にしてお気に入り5件または10件だけ表示。\n" +
                    "3. フルパネルで追いたい項目に **青い星** を付ける。\n" +
                    "4. ミニ HUD はその中から現在値が高い5件または10件を表示。\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "クリック - クイック開始" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "ミニ HUD に<クイック開始>設定を適用します：\n" +
                    "**青い星のお気に入り初期セット**を含みます。\n" +
                    "お気に入りモードでは、その中から現在値が高い5件または10件を表示。\n" +
                    "**青い星**は City Watchdog パネルで追加／削除できます。\n" +
                    "設定：お気に入り、5アイコン、横向き、ドラッグ可、100%、暗いパネル、0件非表示。\n" +
                    "いつでも再実行してリセットできます。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "ミニ HUD モード" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "ミニ HUD に使う通知行を選びます。\n" +
                    "**アクティブ上位**は現在値が高い項目を表示。\n" +
                    "**お気に入り**はメインパネルで **青い星** を付けた行を使います。\n" +
                    "お気に入りはいくつでも選べますが、\n" +
                    "ミニ HUD に出るのは上位5件または10件です。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "アクティブ上位" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "お気に入り" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "アイコン数" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "ミニ HUD に同時表示する通知アイコン数を選びます。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "アイコンサイズ" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "ミニ HUD のアイコンと数字を拡大／縮小します。\n" +
                    "90% = 小さめ、100% = 標準。\n" +
                    "見やすくするなら最大130%。\n" +
                    "目立たせたくない場合は90%。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "向き" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "ミニ HUD のアイコンを横並びか縦並びにします。" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "横" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "縦" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "HUD 位置" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "ミニ HUD の表示場所を選びます。\n" +
                    "ドラッグ可能なら都市UI上で自由に移動できます。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "上中央" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "右上" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "ドラッグ可能" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "暗色／ガラス" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "ミニ HUD の背景スタイルを選びます。\n" +
                    "ガラスは透明から白っぽい曇りまでで、暗くはなりません。\n" +
                    "暗い背景ならダークパネルを使います。"
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "ダークパネル" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "ガラスパネル" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Mini パネル透明度" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "ミニ HUD の透明度を調整します。\n" +
                    "低いほど透明。\n" +
                    "高いほど不透明。\n" +
                    "ガラスは白く曇り、ダークは濃くなります。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "0件を非表示" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "有効 [ ✓ ] にすると、件数0の通知行を ミニ HUD から隠します。" },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "MOD名" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "このMODの表示名です。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "バージョン" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "現在のMODバージョンです。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Mochi の Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "作者の Paradox Mods ページを開きます。" },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "デバッグレポートをログへ" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<通常プレイでは不要です。>\n" +
                    "テスターやゲーム更新後の確認用。<Logs/CityWatchdog.log> にレポートを書き、\n" +
                    "現在のゲーム通知と Watchdog が管理するアイコンを比較します。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "ログを開く" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "</Logs/CityWatchdog.log> があれば開きます。\n" +
                    "なければ Logs/ フォルダーを開きます。"
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
