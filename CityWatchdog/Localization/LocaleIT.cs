// <copyright file="LocaleIT.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleIT.cs
// Purpose: Italian (it-IT) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleIT : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleIT(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "Principale" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "Mini-HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Scorciatoie" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "Info" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "USO" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Pannello principale notifiche" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Pannello principale e display" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Tendenze nella barra" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Avvisi Mini HUD" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "DIAGNOSTICA" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Mostra istruzioni" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Mostra o nasconde le istruzioni qui sotto." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<Modalità città>\n" +
                    "Usa l’icona zampa in alto a sinistra o Shift+N per aprire il pannello principale.\n" +
                    "Trascina il pannello dalla barra del titolo. Usa la freccia per chiuderlo o aprirlo.\n" +
                    "<Avvisi>\n" +
                    "Usa Mostra icone per nascondere o mostrare gli avvisi. Preset 1 e 2: clic per caricare; tieni premuto 1 secondo per salvare.\n" +
                    "<Tendenze>\n" +
                    "Attiva tendenze Popolazione + Soldi e dati extra nei tooltip della barra in basso.\n" +
                    "<Editor>\n" +
                    "Nell’Editor, Shift+N apre la piccola barra City Watchdog Editor."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Attiva/disattiva icone notifica" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<Scorciatoia> per la stessa azione del pulsante <[MOSTRA ICONE]> in gioco.\n" +
                    "Mostra o nasconde subito tutte le icone dei problemi.\n" +
                    "**Solo modalità CITTÀ.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Mostra/nascondi icone problemi" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Apri/chiudi pannello notifiche" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<Scorciatoia> per aprire o chiudere il\n" +
                    "<pannello notifiche> in città.\n" +
                    "Come cliccare l’icona City Watchdog in alto a sinistra.\n" +
                    "**Nell’EDITOR apre i Controlli rapidi Editor.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Apri/chiudi pannello notifiche" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Pannello principale: apri chiuso a 1 riga" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "Se attivo [ ✓ ], City Watchdog apre prima solo la riga di pulsanti.\n" +
                    "Usa la freccia del titolo o il pulsante [0/62] per aprire il pannello completo."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "Disattiva suggerimenti City Watchdog" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "Nasconde la maggior parte dei suggerimenti del pannello principale City Watchdog.\n" +
                    "Riattivali qui o clicca l’icona zampa nella barra del titolo.\n" +
                    "Vale solo per City Watchdog. Utile se preferisci un pannello più pulito con meno suggerimenti."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Nascondi/mostra nomi strade" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<Scorciatoia> per nascondere o mostrare subito i nomi strade del gioco.\n" +
                    "Come l’icona Nomi strade in City Watchdog.\n" +
                    "**Funziona in EDITOR + CITTÀ.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Nascondi/mostra nomi strade" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Disattiva tutti i tooltip del gioco" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<Scorciatoia> per nascondere o mostrare TUTTI i tooltip al passaggio del mouse del gioco — edifici, cittadini, strumenti e icone del menu in basso.\n" +
                    "Questa casella [x] è sincronizzata con l’icona [i] nel pannello City Watchdog.\n" +
                    "Non tocca i tooltip di City Watchdog.\n" +
                    "**Funziona in EDITOR + CITTÀ.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Nascondi/mostra tooltip del gioco" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Interfaccia di gioco più grande" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "Se attivo [ ✓ ], <tutta l’interfaccia del gioco> diventa più grande — pannelli del gioco e delle mod.\n" +
                    "Usa l’opzione <Scala interfaccia> del gioco senza <--developerMode>.\n" +
                    "Questa casella [x] è sincronizzata con il pulsante scala di City Watchdog.\n" +
                    "Solo testo: Opzioni > Interfaccia > <Scala testo>.\n" +
                    "Resta attivo finché non lo disattivi, anche se rimuovi City Watchdog.\n" +
                    "- Disattivalo prima di disinstallare per tornare alla dimensione normale.\n" +
                    "- Oppure avvia una volta con <--developerMode> e disattiva Scala interfaccia (dev)."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "Opacità pannello CWD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Regola la trasparenza dello sfondo.\n" +
                    "**Vale per il pannello principale City Watchdog e il pannello Editor.**\n" +
                    "Più basso = più trasparente. Più alto = più scuro e solido."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Tendenze Popolazione + Soldi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Consigliato ATTIVO>\n" +
                    "Barra in basso: mostra le tendenze accanto alle <frecce soldi e popolazione>.\n" +
                    "Funzione leggera al passaggio <solo visuale>;\n" +
                    "fa risparmiare tempo e può essere più leggera della vista informazioni del gioco."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "Frequenza vista" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Scegli se il testo delle tendenze in basso mostra valori orari o mensili per soldi e popolazione.\n" +
                    "Mensile usa entrate meno spese e una proiezione popolazione di 24 ore."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Orario (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Mensile (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Stile tooltip" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Scegli quanti dettagli mostra il tooltip soldi.\n" +
                    "<Mini> mostra solo Netto in /h e /mo.\n" +
                    "<Compatto> mostra Entrate, Spese e Netto solo nell’unità /h o /mo scelta.\n" +
                    "<Dati completi> mostra Entrate, Spese e Netto in /h e /mo."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Mini" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Compatto" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Dati completi" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Dimensione testo soldi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Regola la <dimensione testo> dei numeri nel tooltip soldi.\n" +
                    "Predefinito gioco = 100%\n" +
                    "<Predefinito mod = 120%>\n" +
                    "Passa su Soldi in basso.\n" +
                    "Per chi fa fatica a leggere tooltip piccoli."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Dimensione testo popolazione" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Regola la <dimensione testo> dei numeri popolazione.\n" +
                    "Predefinito gioco = 100%\n" +
                    "<Predefinito mod = 120%>\n" +
                    "Passa su Popolazione in basso."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Mostra Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Mostra un piccolo pannello HUD.\n" +
                    "Usalo come barra rapida senza aprire tutto City Watchdog o riempire la città di icone.\n" +
                    "Clicca un’icona per saltare al problema. Continua a cliccare per scorrere gli altri punti.\n" +
                    "**============================**\n" +
                    "Un modo per usarlo:\n" +
                    "1. Disattiva tutte le normali icone notifica nel pannello principale.\n" +
                    "2. Attiva Mini HUD per vedere solo 5 o 10 preferiti.\n" +
                    "3. Nel pannello completo marca con **Stella blu** gli avvisi da seguire.\n" +
                    "4. Mini HUD mostra i 5 o 10 conteggi più alti di quella lista.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Clic - Avvio rapido" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Applica un <avvio rapido> al Mini HUD:\n" +
                    "Include una **lista iniziale di Stelle blu**.\n" +
                    "In Preferiti, Mini HUD mostra i 5 o 10 conteggi attuali più alti della tua lista.\n" +
                    "Aggiungi o rimuovi **Stelle blu** in City Watchdog.\n" +
                    "Imposta: Preferiti, 5 icone, orizzontale, trascinabile, 100 %, pannello scuro e nasconde gli zero.\n" +
                    "Usa di nuovo Avvio rapido quando vuoi reimpostare."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Modalità Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Scegli quali righe usa il Mini HUD.\n" +
                    "**Più attivi** mostra i conteggi attuali più alti.\n" +
                    "**Preferiti** usa tutte le righe con **Stella blu** nel pannello principale.\n" +
                    "Puoi scegliere tutti i preferiti che vuoi,\n" +
                    "ma Mini HUD mostra solo i 5 o 10 conteggi più alti."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "Avvisi più attivi" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Preferiti" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Numero icone" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Scegli quante icone di notifica Mini HUD può mostrare insieme." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Dimensione icone" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Ridimensiona icone e numeri del Mini HUD.\n" +
                    "90% = compatto. 100% = predefinito.\n" +
                    "Fino a 130% per vedere meglio.\n" +
                    "Scendi a 90% per renderlo più piccolo e discreto."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Orientamento" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Scegli se le icone Mini HUD sono in riga o colonna." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Orizzontale" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Verticale" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "Posizione HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Scegli dove appare il Mini HUD.\n" +
                    "Trascinabile permette di spostarlo nell’interfaccia città."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Alto centro" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Alto destra" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Trascinabile" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Stile scuro o vetro" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Scegli lo sfondo del Mini HUD.\n" +
                    "Vetro va da chiaro a bianco velato; non diventa più scuro.\n" +
                    "Usa Scuro per un HUD più scuro stile gioco."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Pannello scuro" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Pannello vetro" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Opacità mini pannello" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Regola la trasparenza del Mini HUD.\n" +
                    "Più basso = più trasparente.\n" +
                    "Più alto = più solido.\n" +
                    "Vetro diventa più bianco/velato. Scuro più solido/scuro."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Nascondi avvisi a zero" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Se attivo [ ✓ ], Mini HUD nasconde le righe con conteggio 0." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Nome mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Nome visualizzato di questa mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Versione" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Versione attuale della mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Paradox Mods di Mochi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Apre la pagina Paradox Mods dell’autore." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Rapporto debug nel log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<Non serve per giocare normalmente.>\n" +
                    "Per test e controlli dopo patch del gioco: scrive un rapporto in <Logs/CityWatchdog.log>\n" +
                    "confrontando le notifiche attuali del gioco con le icone controllate da Watchdog."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Apri log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "Apre </Logs/CityWatchdog.log> se esiste.\n" +
                    "Se manca, apre la cartella Logs/."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
