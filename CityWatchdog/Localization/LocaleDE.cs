// <copyright file="LocaleDE.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleDE.cs
// Purpose: German (de-DE) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleDE : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleDE(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "Hauptseite" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "Mini-HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Tastenkürzel" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "Über" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "NUTZUNG" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Haupt-Benachrichtigungsfenster" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Hauptfenster und Anzeige" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Trends in der Menüleiste" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Mini-HUD-Warnungen" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "DIAGNOSE" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Anleitung anzeigen" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Anleitung unten anzeigen oder ausblenden." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<Stadtmodus>\n" +
                    "Pfoten-Icon oben links oder Shift+N öffnet das Hauptfenster.\n" +
                    "Fenster an der Titelleiste ziehen. Mit dem Pfeil ein-/ausklappen.\n" +
                    "<Warnmeldungen>\n" +
                    "Mit Icons anzeigen Warnungen ein-/ausblenden. Preset 1/2: klicken zum Laden; 1 Sekunde halten zum Speichern.\n" +
                    "<Trends>\n" +
                    "Bevölkerungs- und Geldtrends plus Extra-Infos in den Tooltips der unteren Leiste.\n" +
                    "<Editor>\n" +
                    "Im Editor öffnet Shift+N die kleinere City-Watchdog-Editorleiste."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Benachrichtigungs-Icons umschalten" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<Tastenkürzel> wie der <[ICONS ANZEIGEN]>-Button im Spiel.\n" +
                    "Zeigt oder versteckt sofort alle Problem-Icons.\n" +
                    "**Nur STADTMODUS.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Problem-Icons sofort zeigen/verstecken" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Benachrichtigungsfenster öffnen/schließen" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<Tastenkürzel> zum Öffnen/Schließen des\n" +
                    "<Benachrichtigungsfensters> in der Stadt.\n" +
                    "Wie ein Klick auf das City-Watchdog-Icon oben links.\n" +
                    "**Im EDITOR öffnet es die Editor-Schnellleiste.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Benachrichtigungsfenster öffnen/schließen" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Hauptfenster: nur 1 Zeile eingeklappt öffnen" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "Wenn aktiv [ ✓ ], öffnet City Watchdog zuerst nur die 1-zeilige Buttonleiste.\n" +
                    "Mit dem Titelleisten-Pfeil oder dem [0/62]-Button das volle Fenster öffnen."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "City-Watchdog-Tooltips deaktivieren" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "Blendet die meisten Tooltips im City-Watchdog-Hauptfenster aus.\n" +
                    "Hier wieder einschalten oder das Pfoten-Icon in der Titelleiste klicken.\n" +
                    "Betrifft nur City Watchdog. Praktisch, wenn du ein aufgeräumteres Fenster ohne viele Tooltips möchtest."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Straßennamen aus/ein" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<Tastenkürzel> blendet die normalen Straßennamen sofort aus/ein.\n" +
                    "Wie das Straßennamen-Icon in City Watchdog.\n" +
                    "**Funktioniert in EDITOR + STADT.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Straßennamen aus/ein" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Alle Spiel-Tooltips deaktivieren" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<Tastenkürzel> blendet ALLE Spiel-Hover-Tooltips aus/ein — Gebäude, Bürger, Tools und untere Menü-Icons.\n" +
                    "Dieses [x] ist mit dem [i]-Icon im City-Watchdog-Fenster synchronisiert.\n" +
                    "City-Watchdog-eigene Tooltips bleiben unberührt.\n" +
                    "**Funktioniert in EDITOR + STADT.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Alle Spiel-Hover-Tooltips aus/ein" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Größere Spieloberfläche" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "Wenn aktiv [ ✓ ], wird die <gesamte Spieloberfläche> größer — Spiel- und Mod-Fenster.\n" +
                    "Nutzt die Spieloption <Interface-Skalierung> ohne <--developerMode>.\n" +
                    "Dieses [x] ist mit dem Skalierungsbutton in der City-Watchdog-Titelleiste synchronisiert.\n" +
                    "Nur Textgröße: Optionen > Oberfläche > <Textskalierung>.\n" +
                    "Bleibt aktiv, bis du es ausschaltest, auch wenn City Watchdog entfernt wird.\n" +
                    "- Vor dem Deinstallieren ausschalten, um die normale Größe wiederherzustellen.\n" +
                    "- Oder einmal mit <--developerMode> starten und Interface-Skalierung (dev) ausschalten."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "CWD-Fensterdeckkraft" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Passt die Hintergrundtransparenz an.\n" +
                    "**Gilt für das City-Watchdog-Hauptfenster und das Editor-Fenster.**\n" +
                    "Niedriger = transparenter. Höher = dunkler/solider."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Bevölkerungs- + Geldtrends" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Empfohlen EIN>\n" +
                    "Untere Menüleiste: zeigt Trends bei den <Geld- und Bevölkerungspfeilen>.\n" +
                    "Leichte Hover-Anzeige <nur Darstellung>;\n" +
                    "spart Zeit und kann besser laufen als das Infofenster des Spiels."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "Anzeigeintervall" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Wähle Stunden- oder Monatswerte für Geld und Bevölkerung in der unteren Leiste.\n" +
                    "Monatlich nutzt Einnahmen minus Ausgaben und für Bevölkerung eine 24-Stunden-Projektion."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Stündlich (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Monatlich (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Tooltip-Stil" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Wähle, wie viele Details der Geld-Tooltip zeigt.\n" +
                    "<Mini> zeigt nur Netto in /h und /mo.\n" +
                    "<Kompakt> zeigt Einnahmen, Ausgaben und Netto nur in der gewählten /h- oder /mo-Ansicht.\n" +
                    "<Volle Daten> zeigt Einnahmen, Ausgaben und Netto in /h und /mo."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Mini" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Kompakt" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Volle Daten" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Geld-Schriftgröße" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Ändert die <Schriftgröße> der Geld-Tooltip-Zahlen.\n" +
                    "Spielstandard = 100%\n" +
                    "<Mod-Standard = 120%>\n" +
                    "Unten über Geld fahren.\n" +
                    "Für Spieler, denen kleine Tooltips schwer lesbar sind."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Bevölkerungs-Schriftgröße" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Ändert die <Schriftgröße> der Bevölkerungs-Tooltip-Zahlen.\n" +
                    "Spielstandard = 100%\n" +
                    "<Mod-Standard = 120%>\n" +
                    "Unten über Bevölkerung fahren."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Mini HUD anzeigen" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Zeigt ein kleines HUD-Fenster.\n" +
                    "Schnelle Warnleiste ohne das volle City-Watchdog-Fenster oder tausende Icons in der Stadt.\n" +
                    "Icon anklicken: springt zum Problem. Wieder klicken: weitere Treffer durchgehen.\n" +
                    "**============================**\n" +
                    "Eine mögliche Nutzung:\n" +
                    "1. Alle normalen Benachrichtigungs-Icons im Hauptfenster deaktivieren.\n" +
                    "2. Mini HUD aktivieren und nur 5 oder 10 Favoriten anzeigen.\n" +
                    "3. Im Hauptfenster die gewünschten **Blauen Sterne** markieren.\n" +
                    "4. Mini HUD zeigt die 5 oder 10 höchsten aktuellen Werte aus dieser Favoritenliste.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Klick – Schnellstart" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Wendet einen <Schnellstart> fürs Mini HUD an:\n" +
                    "Mit einer **Startauswahl Blauer Sterne**.\n" +
                    "Im Favoritenmodus zeigt Mini HUD die 5 oder 10 höchsten aktuellen Werte deiner **Blauen-Sterne-Liste**.\n" +
                    "**Blaue Sterne** im City-Watchdog-Fenster hinzufügen/entfernen.\n" +
                    "Setzt: Favoriten, 5 Icons, horizontal, verschiebbar, 100 %, dunkles Panel, Nullwerte aus.\n" +
                    "Schnellstart jederzeit erneut ausführen."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Mini-HUD-Modus" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Wähle, welche Warnzeilen das Mini HUD nutzt.\n" +
                    "**Aktivste Warnungen** zeigt die höchsten aktuellen Werte.\n" +
                    "**Favoriten** nutzt alle Zeilen mit **Blauem Stern** im Hauptfenster.\n" +
                    "Du kannst beliebig viele Favoriten wählen,\n" +
                    "Mini HUD zeigt aber nur die 5 oder 10 höchsten Werte daraus."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "Aktivste Warnungen" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Favoriten" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Icon-Anzahl" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Wähle, wie viele Benachrichtigungs-Icons Mini HUD gleichzeitig zeigt." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Icon-Größe" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Skaliert Mini-HUD-Icons und Zahlen.\n" +
                    "90% = kompakt. 100% = Standard.\n" +
                    "Bis 130% für bessere Sicht.\n" +
                    "Auf 90% verkleinern, wenn es weniger auffallen soll."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Ausrichtung" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Wähle, ob Mini-HUD-Icons in einer Reihe oder Spalte stehen." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Horizontal" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Vertikal" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "HUD-Position" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Wähle, wo Mini HUD erscheint.\n" +
                    "Verschiebbar lässt es frei in der Stadtoberfläche bewegen."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Oben mittig" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Oben rechts" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Verschiebbar" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Dunkel- oder Glasstil" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Wähle den Mini-HUD-Hintergrund.\n" +
                    "Glas geht von klar zu weißlich-wolkig; es wird nicht dunkler.\n" +
                    "Dunkles Panel für einen dunkleren Spielstil."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Dunkles Panel" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Glas-Panel" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Mini-Panel-Deckkraft" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Passt die Mini-HUD-Transparenz an.\n" +
                    "Niedriger = transparenter.\n" +
                    "Höher = solider.\n" +
                    "Glas wird weißlicher/wolkiger. Dunkel wird solider/dunkler."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Null-Warnungen ausblenden" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Wenn aktiv [ ✓ ], blendet Mini HUD Benachrichtigungszeilen mit Wert 0 aus." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Mod-Name" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Anzeigename dieses Mods." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Aktuelle Mod-Version." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Mochis Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Öffnet die Paradox-Mods-Seite des Autors." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Debug-Bericht ins Log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<Für normales Spielen nicht nötig.>\n" +
                    "Für Tester und Prüfungen nach Spiel-Patches: schreibt einen Bericht nach <Logs/CityWatchdog.log>\n" +
                    "und vergleicht aktuelle Spiel-Benachrichtigungen mit den von Watchdog gesteuerten Icons."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Log öffnen" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "Öffnet </Logs/CityWatchdog.log>, falls vorhanden.\n" +
                    "Falls nicht, wird der Logs/-Ordner geöffnet."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
