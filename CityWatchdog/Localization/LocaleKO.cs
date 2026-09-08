// <copyright file="LocaleKO.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleKO.cs
// Purpose: Korean (ko-KR) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleKO : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleKO(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "메인" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "미니 HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "키 설정" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "정보" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "사용법" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "메인 알림 패널" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "메인 패널 및 표시" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "메뉴 바 추세" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "미니 HUD 알림" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "진단" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "사용법 표시" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "아래 사용법을 표시하거나 숨깁니다." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<도시 모드>\n" +
                    "왼쪽 위 발바닥 아이콘 또는 Shift+N으로 메인 패널을 엽니다.\n" +
                    "제목 표시줄을 드래그해 이동하고 화살표로 접거나 펼칩니다.\n" +
                    "<알림>\n" +
                    "아이콘 표시로 경고를 숨기거나 표시합니다. 프리셋 1/2: 클릭해 불러오기, 1초 길게 눌러 저장.\n" +
                    "<추세>\n" +
                    "인구 + 자금 추세와 하단 메뉴 툴팁의 추가 정보를 표시합니다.\n" +
                    "<에디터>\n" +
                    "에디터에서는 Shift+N으로 작은 City Watchdog Editor 도구 모음을 엽니다."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "알림 아이콘 전환" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "게임의 <[아이콘 표시]> 버튼과 같은 동작의 <단축키>입니다.\n" +
                    "문제 아이콘을 즉시 표시하거나 숨깁니다.\n" +
                    "**도시 모드 전용.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "문제 아이콘 즉시 표시/숨김" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "알림 패널 열기/닫기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "도시에서 <알림 패널>을 열거나 닫는\n" +
                    "<단축키>입니다.\n" +
                    "왼쪽 위 City Watchdog 아이콘과 같은 동작입니다.\n" +
                    "**에디터에서는 빠른 컨트롤을 엽니다.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "알림 패널 열기/닫기" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "메인 패널: 1줄로 접힌 상태로 열기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "활성화 [ ✓ ]하면 City Watchdog이 처음에 버튼 1줄만 표시합니다.\n" +
                    "제목 화살표 또는 [0/62] 버튼으로 전체 패널을 펼칩니다."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "City Watchdog 툴팁 끄기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "City Watchdog 메인 패널의 대부분 툴팁을 숨깁니다.\n" +
                    "여기서 다시 켜거나 제목 표시줄의 발바닥 아이콘을 클릭하면 다시 표시됩니다.\n" +
                    "City Watchdog에만 적용됩니다. 툴팁이 적은 깔끔한 패널을 원할 때 유용합니다."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "도로 이름 숨기기/표시" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "게임 기본 도로 이름을 즉시 숨기거나 표시하는 <단축키>입니다.\n" +
                    "City Watchdog의 도로 이름 아이콘과 같습니다.\n" +
                    "**에디터 + 도시에서 작동.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "도로 이름 숨기기/표시" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "게임 툴팁 모두 끄기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "건물, 시민, 도구, 하단 메뉴 아이콘 등 게임의 모든 마우스 오버 툴팁을 숨기거나 표시하는 <단축키>입니다.\n" +
                    "이 [x]는 City Watchdog 패널의 [i] 아이콘과 동기화됩니다.\n" +
                    "City Watchdog 자체 툴팁에는 영향이 없습니다.\n" +
                    "**에디터 + 도시에서 작동.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "게임 툴팁 모두 표시/숨김" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "게임 UI 크게" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "활성화 [ ✓ ]하면 <전체 게임 UI>가 커집니다 — 게임 + 모드 패널.\n" +
                    "<Interface Scaling>을 <--developerMode> 없이 사용합니다.\n" +
                    "이 [x]는 City Watchdog 제목 표시줄의 크기 버튼과 동기화됩니다.\n" +
                    "글자만 바꾸려면 Options > Interface > <Text Scaling>.\n" +
                    "City Watchdog을 제거해도 직접 끌 때까지 유지됩니다.\n" +
                    "- 삭제 전 끄면 기본 크기로 돌아갑니다.\n" +
                    "- 또는 <--developerMode>로 한 번 실행 후 Interface Scaling (dev)을 끄세요."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "CWD 패널 투명도" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "배경 투명도를 조절합니다.\n" +
                    "**City Watchdog 메인 패널과 에디터 패널 모두 적용됩니다.**\n" +
                    "낮을수록 투명, 높을수록 어둡고 진해집니다."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "인구 + 자금 추세" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<켜기 권장>\n" +
                    "하단 메뉴의 <자금 및 인구 화살표>에 추세 값을 표시합니다.\n" +
                    "가벼운 <표시 전용> 호버 기능입니다.\n" +
                    "게임 정보 보기를 열지 않아도 되어 편합니다."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "표시 단위" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "하단 추세를 자금과 인구 모두 시간당 또는 월간 값으로 표시할지 선택합니다.\n" +
                    "월간 자금은 수입-지출, 인구는 24시간 환산입니다."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "시간당 (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "월간 (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "툴팁 스타일" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "자금 툴팁의 정보량을 선택합니다.\n" +
                    "<미니>는 순액을 /h와 /mo로 표시합니다.\n" +
                    "<간단>은 수입, 지출, 순액을 선택한 /h 또는 /mo 한 단위로 표시합니다.\n" +
                    "<전체 데이터>는 수입, 지출, 순액을 /h와 /mo 모두 표시합니다."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "미니" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "간단" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "전체 데이터" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "자금 글자 크기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "자금 툴팁 숫자의 <글자 크기>를 조절합니다.\n" +
                    "게임 기본 = 100%\n" +
                    "<모드 기본 = 120%>\n" +
                    "화면 아래 자금에 마우스를 올려 확인하세요.\n" +
                    "작은 툴팁이 보기 어려운 플레이어용."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "인구 글자 크기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "인구 툴팁 숫자의 <글자 크기>를 조절합니다.\n" +
                    "게임 기본 = 100%\n" +
                    "<모드 기본 = 120%>\n" +
                    "화면 아래 인구에 마우스를 올려 확인하세요."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "미니 HUD 표시" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "작은 HUD 패널을 표시합니다.\n" +
                    "전체 City Watchdog 패널이나 도시의 수많은 아이콘 없이 중요한 경고만 빠르게 봅니다.\n" +
                    "아이콘을 클릭하면 문제 위치로 이동합니다. 계속 클릭하면 다른 위치를 순서대로 봅니다.\n" +
                    "**============================**\n" +
                    "사용 예:\n" +
                    "1. 메인 패널에서 일반 알림 아이콘을 모두 끕니다.\n" +
                    "2. 미니 HUD를 켜고 즐겨찾기 5개 또는 10개만 봅니다.\n" +
                    "3. 전체 패널에서 추적할 항목에 **파란 별**을 표시합니다.\n" +
                    "4. 미니 HUD는 그 목록에서 현재 수치가 높은 5개 또는 10개를 표시합니다.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "클릭 - 빠른 시작" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "미니 HUD에 <빠른 시작> 설정을 적용합니다:\n" +
                    "**파란 별 즐겨찾기 시작 목록**이 포함됩니다.\n" +
                    "즐겨찾기 모드에서는 현재 수치가 높은 5개 또는 10개를 표시합니다.\n" +
                    "City Watchdog 패널에서 **파란 별**을 추가/삭제할 수 있습니다.\n" +
                    "설정: 즐겨찾기, 아이콘 5개, 가로, 드래그 가능, 100%, 어두운 패널, 0 숨김.\n" +
                    "언제든 다시 실행해 초기화할 수 있습니다."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "미니 HUD 모드" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "미니 HUD가 사용할 알림 행을 선택합니다.\n" +
                    "**가장 활발한 알림**은 현재 수치가 높은 항목을 표시합니다.\n" +
                    "**즐겨찾기**는 메인 패널에서 **파란 별**로 표시한 행을 사용합니다.\n" +
                    "즐겨찾기는 얼마든지 고를 수 있지만,\n" +
                    "미니 HUD에는 상위 5개 또는 10개만 표시됩니다."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "가장 활발한 알림" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "즐겨찾기" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "아이콘 수" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "미니 HUD에 한 번에 표시할 알림 아이콘 수를 선택합니다." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "아이콘 크기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "미니 HUD 아이콘과 숫자 크기를 조절합니다.\n" +
                    "90% = 작게, 100% = 기본.\n" +
                    "더 잘 보려면 최대 130%.\n" +
                    "덜 눈에 띄게 하려면 90%."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "방향" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "미니 HUD 아이콘을 가로줄 또는 세로줄로 배치합니다." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "가로" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "세로" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "HUD 위치" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "미니 HUD가 나타날 위치를 선택합니다.\n" +
                    "드래그 가능을 선택하면 도시 UI에서 자유롭게 옮길 수 있습니다."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "상단 중앙" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "오른쪽 위" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "드래그 가능" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "어두운/유리 스타일" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "미니 HUD 배경 스타일을 선택합니다.\n" +
                    "유리는 투명에서 흐린 흰색까지이며 더 어두워지지 않습니다.\n" +
                    "어두운 게임 스타일은 어두운 패널을 사용하세요."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "어두운 패널" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "유리 패널" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Mini 패널 투명도" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "미니 HUD 투명도를 조절합니다.\n" +
                    "낮을수록 투명.\n" +
                    "높을수록 진함.\n" +
                    "유리는 더 하얗고 흐려지고, 어두운 패널은 더 진해집니다."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "0 알림 숨기기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "활성화 [ ✓ ]하면 수치가 0인 알림 행을 미니 HUD에서 숨깁니다." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "모드 이름" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "이 모드의 표시 이름입니다." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "버전" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "현재 모드 버전입니다." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Mochi의 Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "제작자의 Paradox Mods 페이지를 엽니다." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "디버그 보고서 로그" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<일반 플레이에는 필요 없습니다.>\n" +
                    "테스터와 게임 패치 후 확인용입니다. <Logs/CityWatchdog.log>에 보고서를 쓰고\n" +
                    "현재 게임 알림과 Watchdog이 관리하는 아이콘을 비교합니다."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "로그 열기" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "</Logs/CityWatchdog.log>가 있으면 엽니다.\n" +
                    "없으면 Logs/ 폴더를 엽니다."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
