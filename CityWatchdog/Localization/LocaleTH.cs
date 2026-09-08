// <copyright file="LocaleTH.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleTH.cs
// Purpose: Thai (th-TH) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleTH : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleTH(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "หลัก" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "มินิ HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "ปุ่มลัด" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "เกี่ยวกับ" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "วิธีใช้" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "แผงแจ้งเตือนหลัก" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "แผงหลักและการแสดงผล" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "แนวโน้มบนแถบเมนู" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "การแจ้งเตือน มินิ HUD" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "วินิจฉัย" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "แสดงคำแนะนำ" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "แสดงหรือซ่อนคำแนะนำด้านล่าง" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<โหมดเมือง>\n" +
                    "ใช้ไอคอนอุ้งเท้ามุมซ้ายบน หรือ Shift+N เพื่อเปิดแผงหลัก\n" +
                    "ลากแผงที่แถบชื่อ ใช้ลูกศรเพื่อย่อหรือขยาย\n" +
                    "<การแจ้งเตือน>\n" +
                    "ใช้ แสดงไอคอน เพื่อซ่อนหรือแสดงการแจ้งเตือน พรีเซ็ต 1 และ 2: คลิกเพื่อโหลด; กดค้าง 1 วินาทีเพื่อบันทึก\n" +
                    "<แนวโน้ม>\n" +
                    "เปิดแนวโน้มประชากร + เงิน และข้อมูลเพิ่มในทูลทิปเมนูด้านล่าง\n" +
                    "<Editor>\n" +
                    "ใน Editor กด Shift+N เพื่อเปิดแถบ City Watchdog Editor แบบเล็ก"
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "สลับไอคอนการแจ้งเตือน" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<ปุ่มลัด>สำหรับคำสั่งเดียวกับปุ่ม <[SHOW ICONS]> ในเกม\n" +
                    "แสดงหรือซ่อนไอคอนปัญหาทั้งหมดทันที\n" +
                    "**เฉพาะโหมดเมือง**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "แสดง/ซ่อนไอคอนปัญหาทันที" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "เปิด/ปิดแผงแจ้งเตือน" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<ปุ่มลัด>สำหรับเปิดหรือปิด\n" +
                    "<แผงแจ้งเตือน>ในเมือง\n" +
                    "เหมือนคลิกไอคอน City Watchdog มุมซ้ายบน\n" +
                    "**ใน EDITOR จะเปิด แถบควบคุมด่วน Editor**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "เปิด/ปิดแผงแจ้งเตือน" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "แผงหลัก: เปิดแบบย่อเหลือ 1 แถว" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "เมื่อเปิด [ ✓ ] City Watchdog จะแสดงแค่ปุ่ม 1 แถวก่อน\n" +
                    "ใช้ลูกศรบนแถบชื่อหรือปุ่ม [0/62] เพื่อเปิดแผงเต็ม"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "ปิดทูลทิป City Watchdog" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "ซ่อนทูลทิปส่วนใหญ่ในแผงหลักของ City Watchdog\n" +
                    "เปิดกลับได้ที่นี่ หรือคลิกไอคอนอุ้งเท้าบนแถบชื่อ\n" +
                    "มีผลเฉพาะ City Watchdog เหมาะถ้าชอบแผงที่ดูโล่งและมีทูลทิปน้อยลง"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "ซ่อน/แสดงชื่อถนน" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<ปุ่มลัด>สำหรับซ่อนหรือแสดงชื่อถนนของเกมทันที\n" +
                    "เหมือนไอคอนชื่อถนนใน City Watchdog\n" +
                    "**ใช้ได้ใน EDITOR + เมือง**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "ซ่อน/แสดงชื่อถนน" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "ปิดทูลทิปเกมทั้งหมด" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<ปุ่มลัด>สำหรับซ่อนหรือแสดงทูลทิปเมื่อเอาเมาส์ชี้ของเกมทั้งหมด — อาคาร ประชาชน เครื่องมือ และไอคอนเมนูด้านล่าง\n" +
                    "ช่อง [x] นี้ซิงก์กับไอคอน [i] ในแผง City Watchdog\n" +
                    "ไม่กระทบทูลทิปของ City Watchdog เอง\n" +
                    "**ใช้ได้ใน EDITOR + เมือง**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "ซ่อน/แสดงทูลทิปเกมทั้งหมด" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "UI เกมใหญ่ขึ้น" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "เมื่อเปิด [ ✓ ] <UI เกมทั้งหมด>จะใหญ่ขึ้น — ทั้งแผงเกมและม็อด\n" +
                    "ใช้ <Interface Scaling> ของเกมโดยไม่ต้องใช้ <--developerMode>\n" +
                    "ช่อง [x] นี้ซิงก์กับปุ่มขยายในแถบชื่อ City Watchdog\n" +
                    "ถ้าต้องการเปลี่ยนเฉพาะตัวอักษร ใช้ Options > Interface > <Text Scaling>\n" +
                    "ค่าจะคงอยู่จนกว่าจะปิด แม้ลบ City Watchdog แล้ว\n" +
                    "- ปิดก่อนถอนม็อดเพื่อกลับขนาดปกติ\n" +
                    "- หรือเปิดเกมหนึ่งครั้งด้วย <--developerMode> แล้วปิด Interface Scaling (dev)"
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "ความทึบแผง CWD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "ปรับความโปร่งใสของพื้นหลัง\n" +
                    "**มีผลทั้งแผงหลัก City Watchdog และแผง Editor**\n" +
                    "ค่าน้อย = โปร่งใสมากขึ้น ค่าสูง = เข้มและทึบขึ้น"
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "แนวโน้มประชากร + เงิน" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<แนะนำให้เปิด>\n" +
                    "เมนูด้านล่าง: แสดงค่าแนวโน้มข้าง <ลูกศรเงินและประชากร>\n" +
                    "เป็นฟีเจอร์เบาๆ <แสดงผลเท่านั้น> เมื่อเอาเมาส์ชี้\n" +
                    "ช่วยลดการเปิดหน้าข้อมูลของเกม"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "หน่วยการแสดงผล" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "เลือกให้แนวโน้มบนแถบด้านล่างแสดงเป็นรายชั่วโมงหรือรายเดือน ทั้งเงินและประชากร\n" +
                    "รายเดือนใช้รายรับลบรายจ่าย และประชากรเป็นการคาดการณ์ 24 ชั่วโมง"
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "รายชั่วโมง (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "รายเดือน (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "รูปแบบทูลทิป" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "เลือกระดับรายละเอียดของทูลทิปเงิน\n" +
                    "<มินิ> แสดงเฉพาะสุทธิทั้ง /h และ /mo\n" +
                    "<กะทัดรัด> แสดงรายรับ รายจ่าย และสุทธิ เฉพาะหน่วย /h หรือ /mo ที่เลือก\n" +
                    "<ข้อมูลทั้งหมด> แสดงรายรับ รายจ่าย และสุทธิ ทั้ง /h และ /mo"
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "มินิ" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "กะทัดรัด" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "ข้อมูลทั้งหมด" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "ขนาดตัวอักษรเงิน" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "ปรับ <ขนาดตัวอักษร> ของตัวเลขในทูลทิปเงิน\n" +
                    "ค่าปกติเกม = 100%\n" +
                    "<ค่าปกติม็อด = 120%>\n" +
                    "เอาเมาส์ชี้ที่เงินด้านล่างจอ\n" +
                    "เหมาะสำหรับคนที่อ่านทูลทิปเล็กๆ ยาก"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "ขนาดตัวอักษรประชากร" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "ปรับ <ขนาดตัวอักษร> ของตัวเลขประชากร\n" +
                    "ค่าปกติเกม = 100%\n" +
                    "<ค่าปกติม็อด = 120%>\n" +
                    "เอาเมาส์ชี้ที่ประชากรด้านล่างจอ"
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "แสดง มินิ HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "แสดงแผง HUD ขนาดเล็ก\n" +
                    "ใช้เป็นแถบแจ้งเตือนเร็วๆ โดยไม่ต้องเปิด City Watchdog เต็มหรือมีไอคอนเต็มเมือง\n" +
                    "คลิกไอคอนเพื่อไปยังจุดปัญหา คลิกซ้ำเพื่อวนดูจุดอื่น\n" +
                    "**============================**\n" +
                    "ตัวอย่างการใช้:\n" +
                    "1. ปิดไอคอนแจ้งเตือนปกติทั้งหมดในแผงหลัก\n" +
                    "2. เปิด มินิ HUD เพื่อดูเฉพาะรายการโปรด 5 หรือ 10 รายการ\n" +
                    "3. ในแผงเต็ม ทำเครื่องหมาย **ดาวสีน้ำเงิน** ที่ต้องการติดตาม\n" +
                    "4. มินิ HUD จะแสดง 5 หรือ 10 ค่าปัจจุบันที่สูงสุดจากรายการนั้น\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "คลิก - ตั้งค่าเร็ว" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "ใช้ <การตั้งค่าเร็ว> สำหรับ มินิ HUD:\n" +
                    "มี **ชุดดาวสีน้ำเงินเริ่มต้น**\n" +
                    "โหมดรายการโปรดจะแสดง 5 หรือ 10 ค่าปัจจุบันที่สูงสุดจากรายการของคุณ\n" +
                    "เพิ่มหรือลบ **ดาวสีน้ำเงิน** ในแผง City Watchdog\n" +
                    "ตั้งค่า: รายการโปรด, 5 ไอคอน, แนวนอน, ลากได้, 100%, แผงมืด, ซ่อนค่า 0\n" +
                    "ใช้การตั้งค่าเร็วอีกครั้งเมื่ออยากรีเซ็ต"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "โหมด มินิ HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "เลือกแถวแจ้งเตือนที่ มินิ HUD ใช้\n" +
                    "**ใช้งานมากสุด** แสดงค่าปัจจุบันที่สูงสุด\n" +
                    "**รายการโปรด** ใช้ทุกแถวที่มี **ดาวสีน้ำเงิน** ในแผงหลัก\n" +
                    "เลือกได้กี่รายการก็ได้\n" +
                    "แต่ มินิ HUD จะแสดงแค่ 5 หรือ 10 ค่าสูงสุด"
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "แจ้งเตือนที่ใช้งานมากสุด" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "รายการโปรด" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "จำนวนไอคอน" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "เลือกจำนวนไอคอนแจ้งเตือนที่ มินิ HUD แสดงพร้อมกัน" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "ขนาดไอคอน" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "ปรับขนาดไอคอนและตัวเลข มินิ HUD\n" +
                    "90% = เล็ก 100% = ปกติ\n" +
                    "เพิ่มได้ถึง 130% เพื่อมองง่ายขึ้น\n" +
                    "ลดเป็น 90% ถ้าต้องการให้เล็กและไม่เด่น"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "แนววาง" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "เลือกให้ไอคอน มินิ HUD เรียงเป็นแถวหรือคอลัมน์" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "แนวนอน" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "แนวตั้ง" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "ตำแหน่ง HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "เลือกตำแหน่ง มินิ HUD\n" +
                    "แบบลากได้ให้ย้ายได้อิสระใน UI เมือง"
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "กลางด้านบน" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "ขวาบน" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "ลากได้" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "สไตล์มืดหรือกระจก" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "เลือกพื้นหลัง มินิ HUD\n" +
                    "กระจกจะจากใสไปเป็นสีขาวขุ่น ไม่เข้มขึ้น\n" +
                    "ใช้แผงมืดถ้าต้องการสไตล์เกมที่มืดกว่า"
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "แผงมืด" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "แผงกระจก" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "ความทึบแผง Mini" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "ปรับความโปร่งใส มินิ HUD\n" +
                    "ค่าน้อย = โปร่งใสมากขึ้น\n" +
                    "ค่าสูง = ทึบขึ้น\n" +
                    "กระจกจะขาว/ขุ่นขึ้น แผงมืดจะเข้มขึ้น"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "ซ่อนแจ้งเตือนค่า 0" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "เมื่อเปิด [ ✓ ] มินิ HUD จะซ่อนแถวแจ้งเตือนที่มีค่า 0" },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "ชื่อม็อด" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "ชื่อที่แสดงของม็อดนี้" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "เวอร์ชัน" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "เวอร์ชันปัจจุบันของม็อด" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Paradox Mods ของ Mochi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "เปิดหน้า Paradox Mods ของผู้สร้าง" },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "รายงาน Debug ลง Log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<ไม่จำเป็นสำหรับการเล่นปกติ>\n" +
                    "สำหรับผู้ทดสอบและตรวจหลังแพตช์เกม: เขียนรายงานลง <Logs/CityWatchdog.log>\n" +
                    "เพื่อเทียบการแจ้งเตือนปัจจุบันของเกมกับไอคอนที่ Watchdog ควบคุม"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "เปิด Log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "เปิด </Logs/CityWatchdog.log> ถ้ามี\n" +
                    "ถ้าไม่มี จะเปิดโฟลเดอร์ Logs/"
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
