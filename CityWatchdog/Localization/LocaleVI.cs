// <copyright file="LocaleVI.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleVI.cs
// Purpose: Vietnamese (vi-VN) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleVI : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleVI(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "Chính" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "HUD nhỏ" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Phím tắt" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "Giới thiệu" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "CÁCH DÙNG" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Bảng thông báo chính" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Bảng chính và hiển thị" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Xu hướng trên thanh menu" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Thông báo HUD nhỏ" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "CHẨN ĐOÁN" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Hiện hướng dẫn" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Hiện hoặc ẩn hướng dẫn bên dưới." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<Chế độ thành phố>\n" +
                    "Dùng biểu tượng bàn chân góc trên trái hoặc Shift+N để mở bảng chính.\n" +
                    "Kéo bảng bằng thanh tiêu đề. Dùng mũi tên để thu gọn hoặc mở rộng.\n" +
                    "<Thông báo>\n" +
                    "Dùng Hiện biểu tượng để ẩn/hiện cảnh báo. Cài đặt sẵn 1/2: nhấp để tải; giữ 1 giây để lưu.\n" +
                    "<Xu hướng>\n" +
                    "Bật xu hướng Dân số + Tiền và thông tin thêm trong tooltip menu dưới.\n" +
                    "<Editor>\n" +
                    "Trong Editor, Shift+N mở thanh City Watchdog Editor nhỏ hơn."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Bật/tắt biểu tượng thông báo" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<Phím tắt> cho cùng thao tác với nút <[SHOW ICONS]> trong game.\n" +
                    "Hiện hoặc ẩn ngay tất cả biểu tượng vấn đề.\n" +
                    "**Chỉ chế độ THÀNH PHỐ.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Hiện/ẩn ngay biểu tượng vấn đề" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Mở/đóng bảng thông báo" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<Phím tắt> để mở hoặc đóng\n" +
                    "<bảng thông báo> trong thành phố.\n" +
                    "Giống nhấp biểu tượng City Watchdog góc trên trái.\n" +
                    "**Trong EDITOR sẽ mở Điều khiển nhanh Editor.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Mở/đóng bảng thông báo" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Bảng chính: mở thu gọn còn 1 hàng" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "Khi bật [ ✓ ], City Watchdog ban đầu chỉ hiện 1 hàng nút.\n" +
                    "Dùng mũi tên tiêu đề hoặc nút [0/62] để mở bảng đầy đủ."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "Tắt gợi ý City Watchdog" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "Ẩn phần lớn gợi ý trong bảng chính City Watchdog.\n" +
                    "Bật lại tại đây hoặc bấm biểu tượng dấu chân trên thanh tiêu đề.\n" +
                    "Chỉ ảnh hưởng City Watchdog. Hữu ích nếu bạn thích bảng gọn hơn và ít gợi ý."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Ẩn/hiện tên đường" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<Phím tắt> để ẩn hoặc hiện ngay tên đường mặc định của game.\n" +
                    "Giống biểu tượng Tên đường trong City Watchdog.\n" +
                    "**Dùng được trong EDITOR + THÀNH PHỐ.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Ẩn/hiện tên đường" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Tắt tất cả tooltip của game" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<Phím tắt> để ẩn hoặc hiện TẤT CẢ tooltip khi rê chuột của game — tòa nhà, cư dân, công cụ và biểu tượng menu dưới.\n" +
                    "Ô [x] này đồng bộ với biểu tượng [i] trong bảng City Watchdog.\n" +
                    "Không ảnh hưởng tooltip riêng của City Watchdog.\n" +
                    "**Dùng được trong EDITOR + THÀNH PHỐ.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Ẩn/hiện tất cả tooltip game" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Giao diện game lớn hơn" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "Khi bật [ ✓ ], <toàn bộ giao diện game> sẽ lớn hơn — bảng của game + mod.\n" +
                    "Dùng tùy chọn <Interface Scaling> của game mà không cần <--developerMode>.\n" +
                    "Ô [x] này đồng bộ với nút phóng to trên thanh tiêu đề City Watchdog.\n" +
                    "Chỉ đổi chữ: Options > Interface > <Text Scaling>.\n" +
                    "Giữ nguyên cho đến khi bạn tắt, kể cả khi gỡ City Watchdog.\n" +
                    "- Tắt trước khi gỡ mod để trở về kích thước thường.\n" +
                    "- Hoặc chạy game một lần với <--developerMode> rồi tắt Interface Scaling (dev)."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "Độ trong suốt bảng CWD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Chỉnh độ trong suốt nền.\n" +
                    "**Áp dụng cho bảng chính City Watchdog và bảng Editor.**\n" +
                    "Thấp = trong hơn. Cao = tối và đặc hơn."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Xu hướng Dân số + Tiền" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Khuyên bật>\n" +
                    "Menu dưới: hiện giá trị xu hướng cạnh <mũi tên tiền và dân số>.\n" +
                    "Tính năng nhẹ <chỉ hiển thị> khi rê chuột;\n" +
                    "giúp bạn đỡ phải mở bảng thông tin của game."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "Đơn vị hiển thị" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Chọn xu hướng ở thanh dưới theo giờ hay theo tháng cho cả tiền và dân số.\n" +
                    "Theo tháng dùng thu nhập trừ chi phí; dân số là dự báo 24 giờ."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Theo giờ (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Theo tháng (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Kiểu tooltip" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Chọn mức chi tiết trong tooltip tiền.\n" +
                    "<Tối giản> chỉ hiện Ròng theo /h và /mo.\n" +
                    "<Gọn> hiện Thu nhập, Chi phí và Ròng chỉ theo đơn vị /h hoặc /mo đã chọn.\n" +
                    "<Đầy đủ> hiện Thu nhập, Chi phí và Ròng theo cả /h và /mo."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Tối giản" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Gọn" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Đầy đủ" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Cỡ chữ tiền" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Chỉnh <cỡ chữ> số trong tooltip tiền.\n" +
                    "Mặc định game = 100%\n" +
                    "<Mặc định mod = 120%>\n" +
                    "Rê chuột lên Tiền ở dưới màn hình.\n" +
                    "Dành cho người khó đọc tooltip nhỏ."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Cỡ chữ dân số" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Chỉnh <cỡ chữ> số dân số.\n" +
                    "Mặc định game = 100%\n" +
                    "<Mặc định mod = 120%>\n" +
                    "Rê chuột lên Dân số ở dưới màn hình."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Hiện HUD nhỏ" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Hiện một bảng HUD nhỏ.\n" +
                    "Dùng như dải cảnh báo nhanh mà không cần mở toàn bộ City Watchdog hay đầy biểu tượng trong thành phố.\n" +
                    "Nhấp biểu tượng để nhảy đến vấn đề. Nhấp tiếp để chuyển qua các vị trí khác.\n" +
                    "**============================**\n" +
                    "Một cách dùng:\n" +
                    "1. Tắt tất cả biểu tượng thông báo thường trong bảng chính.\n" +
                    "2. Bật HUD nhỏ để chỉ xem 5 hoặc 10 mục yêu thích.\n" +
                    "3. Trong bảng đầy đủ, đánh **Sao xanh** vào các mục muốn theo dõi.\n" +
                    "4. HUD nhỏ hiện 5 hoặc 10 giá trị hiện tại cao nhất từ danh sách đó.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Nhấp - Bắt đầu nhanh" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Áp dụng <bắt đầu nhanh> cho HUD nhỏ:\n" +
                    "Có **danh sách Sao xanh ban đầu**.\n" +
                    "Ở chế độ Yêu thích, HUD nhỏ hiện 5 hoặc 10 giá trị hiện tại cao nhất trong danh sách.\n" +
                    "Thêm hoặc bỏ **Sao xanh** trong bảng City Watchdog.\n" +
                    "Thiết lập: Yêu thích, 5 biểu tượng, ngang, kéo được, 100%, bảng tối, ẩn số 0.\n" +
                    "Chạy lại Bắt đầu nhanh bất cứ lúc nào để đặt lại."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Chế độ HUD nhỏ" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Chọn các hàng cảnh báo HUD nhỏ sẽ dùng.\n" +
                    "**Hoạt động nhiều nhất** hiện các giá trị hiện tại cao nhất.\n" +
                    "**Yêu thích** dùng tất cả hàng có **Sao xanh** trong bảng chính.\n" +
                    "Bạn có thể chọn bao nhiêu mục yêu thích cũng được,\n" +
                    "nhưng HUD nhỏ chỉ hiện 5 hoặc 10 giá trị cao nhất."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "Cảnh báo hoạt động nhiều nhất" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Yêu thích" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Số biểu tượng" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Chọn số biểu tượng thông báo HUD nhỏ có thể hiện cùng lúc." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Cỡ biểu tượng" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Chỉnh cỡ biểu tượng và số của HUD nhỏ.\n" +
                    "90% = nhỏ gọn. 100% = mặc định.\n" +
                    "Tăng tới 130% để dễ nhìn hơn.\n" +
                    "Giảm về 90% để nhỏ và ít nổi bật."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Hướng" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Chọn biểu tượng HUD nhỏ xếp theo hàng hay cột." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Ngang" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Dọc" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "Vị trí HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Chọn vị trí HUD nhỏ xuất hiện.\n" +
                    "Kéo được cho phép di chuyển tự do trong giao diện thành phố."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Giữa phía trên" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Góc trên phải" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Kéo được" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Kiểu Tối hoặc Kính" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Chọn kiểu nền HUD nhỏ.\n" +
                    "Kính đi từ trong suốt đến trắng mờ; không tối hơn.\n" +
                    "Dùng bảng Tối nếu muốn kiểu HUD tối hơn."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Bảng tối" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Bảng kính" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Độ trong suốt bảng Mini" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Chỉnh độ trong suốt HUD nhỏ.\n" +
                    "Thấp = trong hơn.\n" +
                    "Cao = đặc hơn.\n" +
                    "Kính sẽ trắng/mờ hơn. Tối sẽ đậm/tối hơn."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Ẩn cảnh báo bằng 0" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Khi bật [ ✓ ], HUD nhỏ ẩn các hàng thông báo có giá trị 0." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Tên mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Tên hiển thị của mod này." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Phiên bản" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Phiên bản mod hiện tại." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Paradox Mods của Mochi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Mở trang Paradox Mods của tác giả." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Ghi báo cáo debug vào log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<Không cần cho chơi bình thường.>\n" +
                    "Dành cho tester và kiểm tra sau patch game: ghi báo cáo vào <Logs/CityWatchdog.log>\n" +
                    "so sánh thông báo hiện tại của game với các biểu tượng Watchdog đang điều khiển."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Mở log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "Mở </Logs/CityWatchdog.log> nếu có.\n" +
                    "Nếu không có, mở thư mục Logs/."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
