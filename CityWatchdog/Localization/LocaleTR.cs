// <copyright file="LocaleTR.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleTR.cs
// Purpose: Turkish (tr-TR) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleTR : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleTR(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "Ana" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "Mini HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Kısayollar" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "Hakkında" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "KULLANIM" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Ana bildirim paneli" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Ana panel ve görünüm" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Menü çubuğu trendleri" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Mini HUD bildirimleri" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "TANI" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Talimatları göster" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Aşağıdaki talimatları gösterir veya gizler." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<Şehir modu>\n" +
                    "Sol üstteki pati simgesine tıkla veya Shift+N ile ana paneli aç.\n" +
                    "Paneli başlık çubuğundan sürükle. Okla daralt veya genişlet.\n" +
                    "<Bildirimler>\n" +
                    "Simgeleri Göster ile uyarıları gizle/göster. Önayar 1/2: yüklemek için tıkla; kaydetmek için 1 saniye basılı tut.\n" +
                    "<Trendler>\n" +
                    "Nüfus + Para trendlerini ve alt menü araç ipuçlarındaki ekstra bilgileri aç.\n" +
                    "<Editör>\n" +
                    "Editörde Shift+N küçük City Watchdog Editor çubuğunu açar."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Bildirim simgelerini aç/kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "Oyundaki <[SİMGELERİ GÖSTER]> düğmesiyle aynı işlev için <kısayol>.\n" +
                    "Tüm sorun simgelerini anında gösterir veya gizler.\n" +
                    "**Yalnızca ŞEHİR modu.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Sorun simgelerini anında göster/gizle" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Bildirim panelini aç/kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "Şehirde <bildirim panelini> açıp kapatan\n" +
                    "<kısayol>.\n" +
                    "Sol üstteki City Watchdog simgesiyle aynı işlev.\n" +
                    "**EDİTÖRDE Editör Hızlı Kontrollerini açar.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Bildirim panelini aç/kapat" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Ana panel: 1 satıra daraltılmış aç" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "Açıkken [ ✓ ], City Watchdog önce yalnızca 1 satır düğme gösterir.\n" +
                    "Tam paneli açmak için başlık oku veya [0/62] düğmesini kullan."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "City Watchdog ipuçlarını kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "City Watchdog ana panelindeki çoğu ipucunu gizler.\n" +
                    "Buradan tekrar açabilir veya başlık çubuğundaki pati simgesine tıklayabilirsin.\n" +
                    "Yalnızca City Watchdog'u etkiler. Daha sade ve daha az ipuçlu bir panel isteyenler için kullanışlıdır."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Yol adlarını gizle/göster" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "Oyunun yol adlarını anında gizlemek veya göstermek için <kısayol>.\n" +
                    "City Watchdog yol adı simgesiyle aynı işlev.\n" +
                    "**EDİTÖR + ŞEHİR modunda çalışır.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Yol adlarını gizle/göster" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Tüm oyun araç ipuçlarını kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "Binalar, vatandaşlar, araçlar ve alt menü simgeleri dahil oyundaki TÜM fare üstü araç ipuçlarını gizleyip gösteren <kısayol>.\n" +
                    "Bu [x], City Watchdog panelindeki [i] simgesiyle eşzamanlıdır.\n" +
                    "City Watchdog'un kendi araç ipuçlarını etkilemez.\n" +
                    "**EDİTÖR + ŞEHİR modunda çalışır.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Tüm oyun araç ipuçlarını gizle/göster" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Daha büyük oyun arayüzü" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "Açıkken [ ✓ ], <tüm oyun arayüzü> büyür — oyun + mod panelleri.\n" +
                    "<Interface Scaling> seçeneğini <--developerMode> olmadan kullanır.\n" +
                    "Bu [x], City Watchdog başlık çubuğundaki ölçek düğmesiyle eşzamanlıdır.\n" +
                    "Yalnızca metin için: Seçenekler > Arayüz > <Metin Ölçeği>.\n" +
                    "City Watchdog kaldırılsa bile sen kapatana kadar açık kalır.\n" +
                    "- Kaldırmadan önce kapat, normal boyuta dönsün.\n" +
                    "- Ya da oyunu bir kez <--developerMode> ile açıp Interface Scaling (dev) seçeneğini kapat."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "CWD panel saydamlığı" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Arka plan saydamlığını ayarlar.\n" +
                    "**City Watchdog ana paneli ve Editör paneli için geçerli.**\n" +
                    "Düşük = daha saydam. Yüksek = daha koyu ve dolu."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Nüfus + Para trendleri" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Açılması önerilir>\n" +
                    "Alt menüde <para ve nüfus oklarının> yanında trend değerlerini gösterir.\n" +
                    "Hafif bir <yalnızca gösterim> özelliğidir.\n" +
                    "Oyunun bilgi görünümünü açma ihtiyacını azaltır."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "Görünüm sıklığı" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Alt çubuk trendlerinin para ve nüfus için saatlik mi aylık mı gösterileceğini seç.\n" +
                    "Aylık para = gelir eksi gider; nüfus = 24 saatlik tahmin."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Saatlik (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Aylık (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Araç ipucu stili" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Para araç ipucunun ne kadar ayrıntı göstereceğini seç.\n" +
                    "<Mini> yalnızca Net değeri /h ve /mo ile gösterir.\n" +
                    "<Kompakt> Gelir, Gider ve Net'i yalnızca seçilen /h veya /mo biriminde gösterir.\n" +
                    "<Tam veri> Gelir, Gider ve Net'i hem /h hem /mo gösterir."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Mini" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Kompakt" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Tam veri" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Para yazı boyutu" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Para araç ipucu sayıların <yazı boyutunu> ayarlar.\n" +
                    "Oyun varsayılanı = 100%\n" +
                    "<Mod varsayılanı = 120%>\n" +
                    "Ekranın altındaki Paranın üstüne gel.\n" +
                    "Küçük araç ipuçlarını okumakta zorlanan oyuncular için."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Nüfus yazı boyutu" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Nüfus araç ipucu sayıların <yazı boyutunu> ayarlar.\n" +
                    "Oyun varsayılanı = 100%\n" +
                    "<Mod varsayılanı = 120%>\n" +
                    "Ekranın altındaki Nüfusun üstüne gel."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Mini HUD göster" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Küçük bir HUD paneli gösterir.\n" +
                    "Tam City Watchdog panelini açmadan veya şehri simgeyle doldurmadan hızlı uyarı şeridi olarak kullan.\n" +
                    "Bir simgeye tıklayınca soruna gider. Tekrar tıklayarak diğer noktaları dolaş.\n" +
                    "**============================**\n" +
                    "Bir kullanım örneği:\n" +
                    "1. Ana panelden normal bildirim simgelerini kapat.\n" +
                    "2. Mini HUD'ı açıp yalnızca 5 veya 10 favoriyi göster.\n" +
                    "3. Tam panelde takip etmek istediğin satırlara **Mavi Yıldız** koy.\n" +
                    "4. Mini HUD bu listeden en yüksek 5 veya 10 güncel değeri gösterir.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Tıkla - Hızlı Başlangıç" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Mini HUD için <hızlı başlangıç> uygular:\n" +
                    "Bir **başlangıç Mavi Yıldız listesi** içerir.\n" +
                    "Favoriler modunda listedeki en yüksek 5 veya 10 güncel değeri gösterir.\n" +
                    "City Watchdog panelinde **Mavi Yıldız** ekle veya kaldır.\n" +
                    "Ayarlar: Favoriler, 5 simge, yatay, sürüklenebilir, %100, koyu panel, sıfırları gizle.\n" +
                    "İstediğinde tekrar çalıştırıp sıfırla."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Mini HUD modu" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Mini HUD'ın hangi uyarı satırlarını kullanacağını seç.\n" +
                    "**En aktif** en yüksek güncel değerleri gösterir.\n" +
                    "**Favoriler** ana panelde **Mavi Yıldız** olan tüm satırları kullanır.\n" +
                    "İstediğin kadar favori seçebilirsin,\n" +
                    "ama Mini HUD yalnızca en yüksek 5 veya 10 değeri gösterir."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "En aktif uyarılar" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Favoriler" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Simge sayısı" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Mini HUD'ın aynı anda kaç bildirim simgesi göstereceğini seç." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Simge boyutu" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Mini HUD simgelerini ve sayılarını ölçekler.\n" +
                    "%90 = küçük. %100 = varsayılan.\n" +
                    "Daha görünür olması için %130'a kadar çıkar.\n" +
                    "Daha küçük ve az dikkat çekici olması için %90."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Yön" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Mini HUD simgelerinin satır mı sütun mu olacağını seç." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Yatay" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Dikey" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "HUD konumu" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Mini HUD'ın nerede görüneceğini seç.\n" +
                    "Sürüklenebilir seçeneği şehir arayüzünde serbestçe taşımanı sağlar."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Üst orta" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Sağ üst" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Sürüklenebilir" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Koyu veya Cam stil" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Mini HUD arka plan stilini seç.\n" +
                    "Cam, şeffaftan bulutlu beyaza gider; koyulaşmaz.\n" +
                    "Daha koyu oyun tarzı için Koyu paneli kullan."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Koyu panel" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Cam panel" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Mini panel saydamlığı" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Mini HUD saydamlığını ayarlar.\n" +
                    "Düşük = daha saydam.\n" +
                    "Yüksek = daha dolu.\n" +
                    "Cam daha beyaz/bulutlu olur. Koyu panel daha koyu ve dolu olur."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Sıfır uyarıları gizle" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Açıkken [ ✓ ], Mini HUD değeri 0 olan bildirim satırlarını gizler." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Mod adı" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Bu modun görünen adı." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Sürüm" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Geçerli mod sürümü." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Mochi'nin Paradox Mods'u" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Yazarın Paradox Mods sayfasını açar." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Debug raporunu loga yaz" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<Normal oyun için gerekmez.>\n" +
                    "Testler ve oyun yamalarından sonraki kontroller için <Logs/CityWatchdog.log> dosyasına rapor yazar\n" +
                    "ve güncel oyun bildirimlerini Watchdog'un kontrol ettiği simgelerle karşılaştırır."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Logu aç" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "</Logs/CityWatchdog.log> varsa açar.\n" +
                    "Yoksa Logs/ klasörünü açar."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
