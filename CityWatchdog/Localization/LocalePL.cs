// <copyright file="LocalePL.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocalePL.cs
// Purpose: Polish (pl-PL) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocalePL : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocalePL(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "Główne" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "Mini-HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Skróty klawiszowe" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "Informacje" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "UŻYCIE" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Główny panel powiadomień" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Główny panel i widok" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Trendy na pasku" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Powiadomienia Mini HUD" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "DIAGNOSTYKA" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Pokaż instrukcje" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Pokaż lub ukryj instrukcje poniżej." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<Tryb miasta>\n" +
                    "Kliknij ikonę łapy w lewym górnym rogu albo użyj Shift+N, aby otworzyć główny panel.\n" +
                    "Przeciągaj panel za pasek tytułu. Strzałką zwiń lub rozwiń.\n" +
                    "<Powiadomienia>\n" +
                    "Użyj Pokaż ikony, aby ukrywać/pokazywać alerty. Presety 1 i 2: kliknij, aby wczytać; przytrzymaj 1 sekundę, aby zapisać.\n" +
                    "<Trendy>\n" +
                    "Włącz trendy Ludność + Pieniądze i dodatkowe dane w tooltipach dolnego menu.\n" +
                    "<Edytor>\n" +
                    "W Edytorze Shift+N otwiera mały pasek City Watchdog Editor."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Przełącz ikony powiadomień" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<Skrót> do tej samej akcji co przycisk <[POKAŻ IKONY]> w grze.\n" +
                    "Natychmiast pokazuje lub ukrywa wszystkie ikony problemów.\n" +
                    "**Tylko tryb MIASTA.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Pokaż/ukryj ikony problemów" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Otwórz/zamknij panel powiadomień" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<Skrót> do otwierania lub zamykania\n" +
                    "<panelu powiadomień> w mieście.\n" +
                    "Działa jak kliknięcie ikony City Watchdog w lewym górnym rogu.\n" +
                    "**W EDYTORZE otwiera Szybkie sterowanie Edytora.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Otwórz/zamknij panel powiadomień" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Główny panel: otwieraj zwinięty do 1 wiersza" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "Gdy włączone [ ✓ ], City Watchdog najpierw pokazuje tylko 1 wiersz przycisków.\n" +
                    "Użyj strzałki tytułu albo przycisku [0/62], aby rozwinąć pełny panel."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "Wyłącz podpowiedzi City Watchdog" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "Ukrywa większość podpowiedzi głównego panelu City Watchdog.\n" +
                    "Włącz je ponownie tutaj albo kliknij ikonę łapy na pasku tytułu.\n" +
                    "Dotyczy tylko City Watchdog. Przydatne, jeśli wolisz czystszy panel z mniejszą liczbą podpowiedzi."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Ukryj/pokaż nazwy dróg" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<Skrót> do natychmiastowego ukrywania/pokazywania nazw dróg z gry.\n" +
                    "Działa jak ikona nazw dróg w City Watchdog.\n" +
                    "**Działa w EDYTORZE + MIEŚCIE.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Ukryj/pokaż nazwy dróg" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Wyłącz wszystkie tooltipy gry" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<Skrót> do ukrywania/pokazywania WSZYSTKICH tooltipów gry po najechaniu — budynków, mieszkańców, narzędzi i ikon dolnego menu.\n" +
                    "Ta opcja [x] jest zsynchronizowana z ikoną [i] w panelu City Watchdog.\n" +
                    "Nie wpływa na tooltipy samego City Watchdog.\n" +
                    "**Działa w EDYTORZE + MIEŚCIE.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Ukryj/pokaż wszystkie tooltipy gry" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Większy interfejs gry" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "Gdy włączone [ ✓ ], <cały interfejs gry> jest większy — panele gry i modów.\n" +
                    "Używa opcji <Skalowanie interfejsu> bez <--developerMode>.\n" +
                    "Ta opcja [x] jest zsynchronizowana z przyciskiem skali w City Watchdog.\n" +
                    "Tylko tekst: Opcje > Interfejs > <Skalowanie tekstu>.\n" +
                    "Pozostaje włączone, dopóki tego nie wyłączysz, nawet po usunięciu City Watchdog.\n" +
                    "- Wyłącz przed odinstalowaniem, aby wrócić do normalnego rozmiaru.\n" +
                    "- Albo uruchom raz z <--developerMode> i wyłącz Skalowanie interfejsu (dev)."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "Przezroczystość panelu CWD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Reguluje przezroczystość tła.\n" +
                    "**Dotyczy głównego panelu City Watchdog i panelu Edytora.**\n" +
                    "Niżej = bardziej przezroczyste. Wyżej = ciemniejsze i bardziej pełne."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Trendy Ludność + Pieniądze" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Zalecane WŁ.>\n" +
                    "Dolne menu: pokazuje trendy przy <strzałkach pieniędzy i ludności>.\n" +
                    "Lekka funkcja po najechaniu <tylko wyświetlanie>;\n" +
                    "oszczędza czas i może być lżejsza niż otwieranie widoku informacji gry."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "Częstotliwość widoku" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Wybierz, czy trendy na dolnym pasku mają pokazywać wartości godzinowe czy miesięczne dla pieniędzy i ludności.\n" +
                    "Miesięczne pieniądze = dochód minus wydatki, ludność = projekcja 24 h."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Godzinowo (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Miesięcznie (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Styl tooltipa" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Wybierz ilość danych w tooltipie pieniędzy.\n" +
                    "<Mini> pokazuje tylko Netto w /h i /mo.\n" +
                    "<Kompaktowy> pokazuje Dochód, Wydatki i Netto tylko w wybranym /h albo /mo.\n" +
                    "<Pełne dane> pokazują Dochód, Wydatki i Netto w /h i /mo."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Mini" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Kompaktowy" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Pełne dane" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Rozmiar czcionki pieniędzy" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Zmienia <rozmiar czcionki> liczb w tooltipie pieniędzy.\n" +
                    "Domyślnie w grze = 100%\n" +
                    "<Domyślnie w modzie = 120%>\n" +
                    "Najedź na Pieniądze na dole ekranu.\n" +
                    "Dla osób, którym trudno czytać małe tooltipy."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Rozmiar czcionki ludności" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Zmienia <rozmiar czcionki> liczb ludności.\n" +
                    "Domyślnie w grze = 100%\n" +
                    "<Domyślnie w modzie = 120%>\n" +
                    "Najedź na Ludność na dole ekranu."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Pokaż Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Pokazuje mały panel HUD.\n" +
                    "Szybki pasek alertów bez otwierania całego City Watchdog i bez tysięcy ikon w mieście.\n" +
                    "Kliknij ikonę, aby przejść do problemu. Klikaj dalej, aby przechodzić po kolejnych miejscach.\n" +
                    "**============================**\n" +
                    "Przykład użycia:\n" +
                    "1. Wyłącz wszystkie zwykłe ikony powiadomień w głównym panelu.\n" +
                    "2. Włącz Mini HUD i pokazuj tylko 5 lub 10 ulubionych.\n" +
                    "3. W pełnym panelu oznacz śledzone pozycje **Niebieską gwiazdką**.\n" +
                    "4. Mini HUD pokaże 5 lub 10 najwyższych bieżących wartości z tej listy.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Kliknij - Szybki start" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Stosuje <szybki start> dla Mini HUD:\n" +
                    "Zawiera **startowy zestaw Niebieskich gwiazdek**.\n" +
                    "W trybie Ulubione Mini HUD pokazuje 5 lub 10 najwyższych bieżących wartości z listy.\n" +
                    "Dodawaj/usuwaj **Niebieskie gwiazdki** w panelu City Watchdog.\n" +
                    "Ustawia: Ulubione, 5 ikon, poziomo, przeciąganie, 100%, ciemny panel, ukrywanie zer.\n" +
                    "Uruchom ponownie, kiedy chcesz zresetować te ustawienia."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Tryb Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Wybierz, których wierszy alertów używa Mini HUD.\n" +
                    "**Najbardziej aktywne** pokazuje najwyższe bieżące wartości.\n" +
                    "**Ulubione** używa wszystkich wierszy z **Niebieską gwiazdką** w głównym panelu.\n" +
                    "Możesz wybrać dowolnie dużo ulubionych,\n" +
                    "ale Mini HUD pokaże tylko 5 lub 10 najwyższych wartości."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "Najbardziej aktywne" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Ulubione" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Liczba ikon" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Wybierz, ile ikon powiadomień Mini HUD może pokazywać naraz." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Rozmiar ikon" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Skaluje ikony i liczby Mini HUD.\n" +
                    "90% = kompaktowo. 100% = domyślnie.\n" +
                    "Do 130% dla lepszej widoczności.\n" +
                    "90% jeśli ma być mniejszy i mniej widoczny."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Układ" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Wybierz układ ikon Mini HUD: wiersz albo kolumna." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Poziomo" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Pionowo" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "Pozycja HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Wybierz, gdzie pojawia się Mini HUD.\n" +
                    "Przeciągany można dowolnie przesuwać w interfejsie miasta."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Góra środek" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Góra prawa" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Przeciągany" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Styl ciemny lub szkło" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Wybierz tło Mini HUD.\n" +
                    "Szkło przechodzi od przezroczystego do mlecznobiałego; nie robi się ciemniejsze.\n" +
                    "Ciemny panel daje ciemniejszy wygląd jak w grze."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Ciemny panel" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Szklany panel" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Przezroczystość mini panelu" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Reguluje przezroczystość Mini HUD.\n" +
                    "Niżej = bardziej przezroczyste.\n" +
                    "Wyżej = bardziej pełne.\n" +
                    "Szkło robi się bielsze/mleczne. Ciemny panel ciemniejszy."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Ukryj alerty z zerem" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Gdy włączone [ ✓ ], Mini HUD ukrywa wiersze powiadomień z wartością 0." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Nazwa moda" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Wyświetlana nazwa tego moda." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Wersja" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Aktualna wersja moda." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Paradox Mods Mochi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Otwiera stronę autora w Paradox Mods." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Raport debug do logu" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<Niepotrzebne podczas normalnej gry.>\n" +
                    "Dla testerów i kontroli po patchach gry: zapisuje raport do <Logs/CityWatchdog.log>\n" +
                    "porównując bieżące powiadomienia gry z ikonami sterowanymi przez Watchdog."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Otwórz log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "Otwiera </Logs/CityWatchdog.log>, jeśli istnieje.\n" +
                    "Jeśli nie, otwiera folder Logs/."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
