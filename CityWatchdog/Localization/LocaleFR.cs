// <copyright file="LocaleFR.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleFR.cs
// Purpose: French (fr-FR) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleFR : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleFR(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kActions), "Principal" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kMiniHudTab), "Mini-HUD" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Raccourcis" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "À propos" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "UTILISATION" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Panneau principal des notifications" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Panneau principal et affichage" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Tendances dans la barre" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Alertes Mini HUD" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "DIAGNOSTIC" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Afficher les instructions" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Affiche ou masque les instructions ci-dessous." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<Mode ville>\n" +
                    "Clique la patte en haut à gauche ou utilise Shift+N pour ouvrir le panneau principal.\n" +
                    "Fais glisser le panneau par la barre de titre. Utilise la flèche pour le réduire ou l’agrandir.\n" +
                    "<Alertes>\n" +
                    "Utilise Afficher les icônes pour masquer/afficher les alertes. Préréglages 1 et 2 : clic pour charger ; maintenir 1 seconde pour enregistrer.\n" +
                    "<Tendances>\n" +
                    "Active les tendances Population + Argent et les infos supplémentaires dans les infobulles du menu bas.\n" +
                    "<Éditeur>\n" +
                    "Dans l’Éditeur, Shift+N ouvre la petite barre City Watchdog Editor."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Afficher/masquer les icônes d’alerte" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<Raccourci> pour la même action que le bouton <[AFFICHER LES ICÔNES]> en jeu.\n" +
                    "Affiche ou masque instantanément toutes les icônes de problème.\n" +
                    "**Mode VILLE uniquement.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Afficher/masquer les icônes de problème" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Ouvrir/fermer le panneau de notifications" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<Raccourci> pour ouvrir ou fermer le\n" +
                    "<panneau de notifications> en ville.\n" +
                    "Comme cliquer l’icône City Watchdog en haut à gauche.\n" +
                    "**Dans l’ÉDITEUR, ouvre les contrôles rapides.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Ouvrir/fermer le panneau de notifications" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Panneau principal : ouvrir réduit à 1 ligne" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "Si activé [ ✓ ], City Watchdog ouvre d’abord seulement la ligne de boutons.\n" +
                    "Utilise la flèche du titre ou le bouton [0/62] pour ouvrir le panneau complet."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "Désactiver les infobulles City Watchdog" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "Masque la plupart des infobulles du panneau principal City Watchdog.\n" +
                    "Réactive-les ici ou clique l’icône patte dans la barre de titre.\n" +
                    "Affecte seulement City Watchdog. Pratique si tu préfères un panneau plus épuré avec moins d’infobulles."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Masquer/afficher les noms de rues" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<Raccourci> pour masquer ou afficher instantanément les noms de rues du jeu.\n" +
                    "Comme l’icône Noms de rues dans City Watchdog.\n" +
                    "**Fonctionne dans ÉDITEUR + VILLE.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Masquer/afficher les noms de rues" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Désactiver toutes les infobulles du jeu" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<Raccourci> pour masquer ou afficher TOUTES les infobulles du jeu au survol — bâtiments, citoyens, outils et icônes du menu bas.\n" +
                    "Cette case [x] est synchronisée avec l’icône [i] du panneau City Watchdog.\n" +
                    "N’affecte pas les infobulles propres à City Watchdog.\n" +
                    "**Fonctionne dans ÉDITEUR + VILLE.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Masquer/afficher les infobulles du jeu" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Interface de jeu agrandie" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "Si activé [ ✓ ], <toute l’interface du jeu> est plus grande — panneaux du jeu et des mods.\n" +
                    "Utilise l’option <Échelle de l’interface> du jeu sans <--developerMode>.\n" +
                    "Cette case [x] est synchronisée avec le bouton d’échelle de City Watchdog.\n" +
                    "Texte seulement : Options > Interface > <Échelle du texte>.\n" +
                    "Reste actif jusqu’à sa désactivation, même si City Watchdog est supprimé.\n" +
                    "- Désactive-le avant de désinstaller pour revenir à la taille normale.\n" +
                    "- Ou lance une fois avec <--developerMode> puis désactive Échelle de l’interface (dev)."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "Opacité du panneau CWD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Règle la transparence du fond.\n" +
                    "**S’applique au panneau principal City Watchdog et au panneau Éditeur.**\n" +
                    "Plus bas = plus transparent. Plus haut = plus sombre et opaque."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Tendances Population + Argent" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Activation recommandée>\n" +
                    "Menu du bas : affiche les tendances près des <flèches argent et population>.\n" +
                    "Fonction légère au survol <affichage seulement> ;\n" +
                    "évite d’ouvrir le panneau d’infos du jeu et peut être plus fluide."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "Fréquence d’affichage" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Choisis si les tendances du menu bas sont horaires ou mensuelles pour l’argent et la population.\n" +
                    "Mensuel utilise revenus moins dépenses et une projection population sur 24 h."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Horaire (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Mensuel (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Style d’infobulle" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Choisis le niveau de détail de l’infobulle argent.\n" +
                    "<Mini> montre seulement le Net en /h et /mo.\n" +
                    "<Compact> montre Revenus, Dépenses et Net seulement dans l’unité /h ou /mo choisie.\n" +
                    "<Données complètes> montre Revenus, Dépenses et Net en /h et /mo."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Mini" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Compact" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Données complètes" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Taille police argent" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Règle la <taille de police> des nombres de l’infobulle argent.\n" +
                    "Défaut du jeu = 100%\n" +
                    "<Défaut du mod = 120%>\n" +
                    "Survole Argent en bas de l’écran.\n" +
                    "Pour les joueurs qui lisent mal les petites infobulles."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Taille police population" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Règle la <taille de police> des nombres de population.\n" +
                    "Défaut du jeu = 100%\n" +
                    "<Défaut du mod = 120%>\n" +
                    "Survole Population en bas de l’écran."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Afficher Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Affiche un petit panneau HUD.\n" +
                    "Sert de bandeau rapide sans ouvrir tout City Watchdog ni remplir la ville d’icônes.\n" +
                    "Clique une icône pour sauter au problème. Reclique pour parcourir les autres endroits.\n" +
                    "**============================**\n" +
                    "Une façon de l’utiliser :\n" +
                    "1. Désactive toutes les icônes normales dans le panneau principal.\n" +
                    "2. Active Mini HUD pour ne voir que 5 ou 10 favoris.\n" +
                    "3. Marque avec une **Étoile bleue** les alertes à suivre dans le panneau complet.\n" +
                    "4. Mini HUD affiche les 5 ou 10 compteurs les plus élevés de cette liste.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Clic - Démarrage rapide" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Applique un <démarrage rapide> au Mini HUD :\n" +
                    "Inclut une **liste de départ d’Étoiles bleues**.\n" +
                    "En mode Favoris, Mini HUD affiche les 5 ou 10 compteurs actuels les plus élevés de ta liste.\n" +
                    "Ajoute ou retire les **Étoiles bleues** dans City Watchdog.\n" +
                    "Règle : Favoris, 5 icônes, horizontal, déplaçable, 100 %, panneau sombre et masque les zéros.\n" +
                    "Relance Démarrage rapide quand tu veux pour réinitialiser."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Mode Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Choisis les lignes utilisées par le Mini HUD.\n" +
                    "**Alertes les plus actives** montre les compteurs actuels les plus élevés.\n" +
                    "**Favoris** utilise toutes les lignes avec une **Étoile bleue** dans le panneau principal.\n" +
                    "Tu peux choisir autant de favoris que tu veux,\n" +
                    "mais Mini HUD n’affiche que les 5 ou 10 compteurs les plus élevés."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "Alertes les plus actives" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Favoris" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Nombre d’icônes" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Choisis combien d’icônes de notification Mini HUD peut afficher à la fois." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Taille des icônes" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Ajuste la taille des icônes et nombres du Mini HUD.\n" +
                    "90% = compact. 100% = défaut.\n" +
                    "Jusqu’à 130% pour mieux voir.\n" +
                    "Passe à 90% pour le rendre plus petit et discret."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Orientation" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Choisis si les icônes Mini HUD sont en ligne ou en colonne." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Horizontal" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Vertical" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "Position du HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Choisis où apparaît le Mini HUD.\n" +
                    "Déplaçable permet de le déplacer dans l’interface de ville."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Haut centre" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Haut droit" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Déplaçable" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Style sombre ou verre" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Choisis le fond du Mini HUD.\n" +
                    "Verre va de clair à blanc voilé ; il ne devient pas plus sombre.\n" +
                    "Utilise Sombre pour un HUD plus foncé style jeu."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Panneau sombre" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Panneau verre" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Opacité du mini panneau" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Règle la transparence du Mini HUD.\n" +
                    "Plus bas = plus transparent.\n" +
                    "Plus haut = plus opaque.\n" +
                    "Verre devient plus blanc/voilé. Sombre devient plus dense/foncé."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Masquer les alertes à zéro" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Si activé [ ✓ ], Mini HUD masque les lignes avec un compteur à 0." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Nom du mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Nom affiché de ce mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Version actuelle du mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Paradox Mods de Mochi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Ouvre la page Paradox Mods de l’auteur." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Rapport de débogage" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<Pas nécessaire en jeu normal.>\n" +
                    "Pour les tests et après les patchs du jeu : écrit un rapport dans <Logs/CityWatchdog.log>\n" +
                    "comparant les notifications actuelles du jeu aux icônes contrôlées par Watchdog."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Ouvrir le journal" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "Ouvre </Logs/CityWatchdog.log> s’il existe.\n" +
                    "Sinon ouvre le dossier Logs/."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
