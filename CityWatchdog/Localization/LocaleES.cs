// <copyright file="LocaleES.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocaleES.cs
// Purpose: Spanish (es-ES) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocaleES : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocaleES(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Atajos" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "Acerca de" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "USO" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Panel principal de notificaciones" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Panel principal y pantalla" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Tendencias en la barra" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Avisos Mini HUD" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "DIAGNÓSTICO" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Mostrar instrucciones" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Muestra u oculta las instrucciones de abajo." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<Modo ciudad>\n" +
                    "Usa el icono de la pata arriba a la izquierda o Shift+N para abrir el panel principal.\n" +
                    "Arrastra el panel por la barra de título. Usa la flecha para contraerlo o expandirlo.\n" +
                    "<Alertas>\n" +
                    "Usa Mostrar iconos para ocultar o mostrar alertas. Preajustes 1 y 2: clic para cargar; mantén 1 segundo para guardar.\n" +
                    "<Tendencias>\n" +
                    "Activa tendencias de Población + Dinero y datos extra en las ayudas de la barra inferior.\n" +
                    "<Editor>\n" +
                    "En el Editor, Shift+N abre la barra pequeña de City Watchdog Editor."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Alternar iconos de notificación" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<Atajo> para la misma acción que <[MOSTRAR ICONOS]> en el juego.\n" +
                    "Muestra u oculta al instante todos los iconos de problemas.\n" +
                    "**Solo modo CIUDAD.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Mostrar/ocultar iconos de problemas" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Abrir/cerrar panel de notificaciones" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<Atajo> para abrir o cerrar el\n" +
                    "<panel de notificaciones> en la ciudad.\n" +
                    "Igual que pulsar el icono de City Watchdog arriba a la izquierda.\n" +
                    "**En EDITOR abre los Controles rápidos del Editor.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Abrir/cerrar panel de notificaciones" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Panel principal: abrir contraído a 1 fila" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "Si está activo [ ✓ ], City Watchdog abre primero solo la fila de botones.\n" +
                    "Usa la flecha del título o el botón [0/62] para abrir el panel completo."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "Desactivar ayudas de City Watchdog" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "Oculta la mayoría de las ayudas del panel principal de City Watchdog.\n" +
                    "Vuelve a activarlas aquí o pulsa el icono de la pata en la barra de título.\n" +
                    "Solo afecta a City Watchdog. Es útil si prefieres un panel más limpio y con menos ayudas."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Ocultar/mostrar nombres de calles" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<Atajo> para ocultar o mostrar al instante los nombres de calles del juego.\n" +
                    "Igual que el icono de nombres de calles en City Watchdog.\n" +
                    "**Funciona en EDITOR + CIUDAD.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Ocultar/mostrar nombres de calles" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Desactivar todas las ayudas del juego" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<Atajo> para ocultar o mostrar TODAS las ayudas al pasar el ratón del juego — edificios, ciudadanos, herramientas e iconos de la barra inferior.\n" +
                    "Esta casilla [x] está sincronizada con el icono [i] del panel de City Watchdog.\n" +
                    "No afecta a las ayudas propias de City Watchdog.\n" +
                    "**Funciona en EDITOR + CIUDAD.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Ocultar/mostrar ayudas del juego" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Interfaz del juego más grande" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "Al activarlo [ ✓ ], <toda la interfaz del juego> se hace más grande — paneles del juego y mods.\n" +
                    "Usa la opción del juego <Escala de interfaz> sin <--developerMode>.\n" +
                    "Esta casilla [x] está sincronizada con el botón de escala de City Watchdog.\n" +
                    "Solo texto: Opciones > Interfaz > <Escala de texto>.\n" +
                    "Sigue activo hasta que lo apagues, aunque quites City Watchdog.\n" +
                    "- Apágalo antes de desinstalar para volver al tamaño normal.\n" +
                    "- O inicia una vez con <--developerMode> y desactiva Escala de interfaz (dev)."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "Opacidad del panel CWD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Ajusta la transparencia del fondo.\n" +
                    "**Afecta al panel principal de City Watchdog y al panel del Editor.**\n" +
                    "Menor = más transparente. Mayor = más oscuro y sólido."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Tendencias de Población + Dinero" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Recomendado ACTIVAR>\n" +
                    "Barra inferior: muestra tendencias junto a las <flechas de dinero y población>.\n" +
                    "Es una función ligera <solo visual> al pasar el ratón;\n" +
                    "ahorra tiempo y puede rendir mejor que abrir la vista de información del juego."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "Frecuencia de vista" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Elige si la tendencia de la barra inferior muestra valores por hora o por mes para dinero y población.\n" +
                    "Mensual usa ingresos menos gastos y una proyección de población de 24 horas."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Por hora (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Mensual (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Estilo de ayuda" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Elige cuánto detalle muestra la ayuda de dinero.\n" +
                    "<Mini> muestra solo Neto en /h y /mo.\n" +
                    "<Compacto> muestra Ingresos, Gastos y Neto solo en la unidad /h o /mo elegida.\n" +
                    "<Datos completos> muestra Ingresos, Gastos y Neto en /h y /mo."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Mini" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Compacto" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Datos completos" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Tamaño de fuente de dinero" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Ajusta el <tamaño de fuente> de los números de la ayuda de dinero.\n" +
                    "Predeterminado del juego = 100%\n" +
                    "<Predeterminado del mod = 120%>\n" +
                    "Pasa el ratón sobre Dinero abajo.\n" +
                    "Para jugadores a los que les cuesta leer ayudas pequeñas."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Tamaño de fuente de población" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Ajusta el <tamaño de fuente> de los números de población.\n" +
                    "Predeterminado del juego = 100%\n" +
                    "<Predeterminado del mod = 120%>\n" +
                    "Pasa el ratón sobre Población abajo."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Mostrar Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Muestra un panel HUD pequeño.\n" +
                    "Úsalo como tira rápida de alertas sin abrir todo City Watchdog ni llenar la ciudad de iconos.\n" +
                    "Pulsa un icono para saltar al problema. Sigue pulsando para recorrer otros puntos.\n" +
                    "**============================**\n" +
                    "Una forma de usarlo:\n" +
                    "1. Desactiva todos los iconos normales desde el panel principal.\n" +
                    "2. Activa Mini HUD para ver solo 5 o 10 favoritos.\n" +
                    "3. Marca con **Estrella azul** lo que quieras seguir en el panel completo.\n" +
                    "4. Mini HUD muestra los 5 o 10 conteos más altos de esa lista.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Clic - Inicio rápido" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Aplica un <inicio rápido> al Mini HUD:\n" +
                    "Incluye una **lista inicial de Estrellas azules**.\n" +
                    "En Favoritos, Mini HUD muestra los 5 o 10 conteos actuales más altos de tu lista.\n" +
                    "Añade o quita **Estrellas azules** en City Watchdog.\n" +
                    "Configura: Favoritos, 5 iconos, horizontal, arrastrable, 100 %, panel oscuro y oculta ceros.\n" +
                    "Vuelve a usar Inicio rápido cuando quieras restablecerlo."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Modo Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Elige qué filas usa el Mini HUD.\n" +
                    "**Más activas** muestra los conteos actuales más altos.\n" +
                    "**Favoritos** usa todas las filas con **Estrella azul** en el panel principal.\n" +
                    "Puedes elegir tantos favoritos como quieras,\n" +
                    "pero Mini HUD solo muestra los 5 o 10 conteos más altos."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "Alertas más activas" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Favoritos" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Cantidad de iconos" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Elige cuántos iconos de notificación puede mostrar Mini HUD a la vez." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Tamaño de iconos" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Escala los iconos y números del Mini HUD.\n" +
                    "90% = compacto. 100% = normal.\n" +
                    "Hasta 130% para ver mejor.\n" +
                    "Baja a 90% para hacerlo más pequeño y discreto."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Orientación" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Elige si los iconos del Mini HUD van en fila o columna." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Horizontal" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Vertical" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "Posición del HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Elige dónde aparece el Mini HUD.\n" +
                    "Arrastrable permite moverlo por la interfaz de la ciudad."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Arriba centro" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Arriba derecha" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Arrastrable" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Estilo oscuro o cristal" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Elige el fondo del Mini HUD.\n" +
                    "Cristal va de claro a blanco nublado; no se oscurece.\n" +
                    "Usa Oscuro para un HUD más oscuro estilo juego."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Panel oscuro" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Panel cristal" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Opacidad del Mini panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Ajusta la transparencia del Mini HUD.\n" +
                    "Menor = más transparente.\n" +
                    "Mayor = más sólido.\n" +
                    "Cristal se vuelve más blanco/nublado. Oscuro más sólido/oscuro."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Ocultar alertas en cero" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Si está activo [ ✓ ], Mini HUD oculta las filas con conteo 0." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Nombre del mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Nombre visible de este mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Versión" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Versión actual del mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Paradox Mods de Mochi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Abre la página de Paradox Mods del autor." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Informe de depuración" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<No hace falta para jugar normalmente.>\n" +
                    "Para pruebas y revisiones tras parches del juego: escribe un informe en <Logs/CityWatchdog.log>\n" +
                    "comparando las notificaciones actuales del juego con los iconos que controla Watchdog."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Abrir registro" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "Abre </Logs/CityWatchdog.log> si existe.\n" +
                    "Si no existe, abre la carpeta Logs/."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
