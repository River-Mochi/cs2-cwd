// <copyright file="LocalePT_BR.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: src/Localization/LocalePT_BR.cs
// Purpose: Portuguese (pt-BR) for City Watchdog Options UI menu.

namespace CityWatchdog
{
    using System.Collections.Generic; // Dictionary and KeyValuePair
    using Colossal;                   // IDictionarySource

    public sealed class LocalePT_BR : IDictionarySource
    {
        private readonly CwdSettings m_Settings;

        public LocalePT_BR(CwdSettings setting)
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
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kHotkeys), "Atalhos" },
                { m_Settings.GetOptionTabLocaleID(CwdSettings.kAbout), "Sobre" },

                // --- Groups, ordered by Options menu location ---
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutUsage), "USO" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kNotifications), "Painel principal de notificações" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kHotkeyActions), "Painel principal e exibição" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMoneyViewGroup), "Tendências na barra" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kMiniHudGroup), "Alertas Mini HUD" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutInfo), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutLinks), "" },
                { m_Settings.GetOptionGroupLocaleID(CwdSettings.kAboutDiagnostics), "DIAGNÓSTICO" },

                // --------------------------------------------------------------------
                // Main tab - Usage
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ShowUsage)), "Mostrar instruções" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ShowUsage)), "Mostra ou esconde as instruções abaixo." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.UsageText)),
                    "<Modo cidade>\n" +
                    "Use o ícone da pata no canto superior esquerdo ou Shift+N para abrir o painel principal.\n" +
                    "Arraste o painel pela barra de título. Use a seta para recolher ou expandir.\n" +
                    "<Alertas>\n" +
                    "Use Mostrar ícones para esconder ou mostrar alertas. Presets 1 e 2: clique para carregar; segure 1 segundo para salvar.\n" +
                    "<Tendências>\n" +
                    "Ative tendências de População + Dinheiro e dados extras nos tooltips do menu inferior.\n" +
                    "<Editor>\n" +
                    "No Editor, Shift+N abre a barra menor City Watchdog Editor."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.UsageText)), "" },

                // --------------------------------------------------------------------
                // Main tab - Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)), "Alternar ícones de notificação" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationsKeyboardBinding)),
                    "<Atalho> para a mesma ação do botão <[MOSTRAR ÍCONES]> no jogo.\n" +
                    "Mostra ou esconde na hora todos os ícones de problema.\n" +
                    "**Só no modo CIDADE.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationsAction), "Mostrar/esconder ícones de problema" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)), "Abrir/fechar painel de notificações" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleNotificationPanelKeyboardBinding)),
                    "<Atalho> para abrir ou fechar o\n" +
                    "<painel de notificações> na cidade.\n" +
                    "Igual a clicar no ícone City Watchdog no canto superior esquerdo.\n" +
                    "**No EDITOR abre os Controles rápidos do Editor.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleNotificationPanelAction), "Abrir/fechar painel de notificações" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)), "Painel principal: abrir recolhido em 1 linha" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PanelButtonsOnlyStart)),
                    "Quando ativado [ ✓ ], o City Watchdog abre primeiro só a linha de botões.\n" +
                    "Use a seta do título ou o botão [0/62] para abrir o painel completo."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.DisableCwdTooltips)), "Desativar dicas do City Watchdog" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.DisableCwdTooltips)),
                    "Oculta a maioria das dicas do painel principal do City Watchdog.\n" +
                    "Ative novamente aqui ou clique no ícone da pata na barra de título.\n" +
                    "Afeta só o City Watchdog. É útil se você prefere um painel mais limpo e com menos dicas."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)), "Esconder/mostrar nomes de ruas" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleRoadNamesKeyboardBinding)),
                    "<Atalho> para esconder ou mostrar na hora os nomes de ruas do jogo.\n" +
                    "Igual ao ícone de nomes de ruas no City Watchdog.\n" +
                    "**Funciona no EDITOR + CIDADE.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleRoadNamesAction), "Esconder/mostrar nomes de ruas" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)), "Desativar todos os tooltips do jogo" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ToggleAllTooltipsKeyboardBinding)),
                    "<Atalho> para esconder ou mostrar TODOS os tooltips do jogo ao passar o mouse — prédios, cidadãos, ferramentas e ícones do menu inferior.\n" +
                    "Esta caixa [x] fica sincronizada com o ícone [i] no painel City Watchdog.\n" +
                    "Não afeta os tooltips do próprio City Watchdog.\n" +
                    "**Funciona no EDITOR + CIDADE.**"
                },
                { m_Settings.GetBindingKeyLocaleID(CwdSettings.ToggleAllTooltipsAction), "Esconder/mostrar tooltips do jogo" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.InterfaceScaling)), "Interface do jogo maior" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.InterfaceScaling)),
                    "Quando ativado [ ✓ ], <toda a interface do jogo> fica maior — painéis do jogo e mods.\n" +
                    "Usa a opção <Interface Scaling> do jogo sem <--developerMode>.\n" +
                    "Esta caixa [x] fica sincronizada com o botão de escala do City Watchdog.\n" +
                    "Só texto: Opções > Interface > <Escala do texto>.\n" +
                    "Continua ligado até você desligar, mesmo se remover o City Watchdog.\n" +
                    "- Desligue antes de desinstalar para voltar ao tamanho normal.\n" +
                    "- Ou abra uma vez com <--developerMode> e desligue Interface Scaling (dev)."
                },


                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MainPanelOpacity)), "Opacidade do painel CWD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MainPanelOpacity)),
                    "Ajusta a transparência do fundo.\n" +
                    "**Vale para o painel principal do City Watchdog e o painel do Editor.**\n" +
                    "Menor = mais transparente. Maior = mais escuro e sólido."
                },

                // --------------------------------------------------------------------
                // Main tab - In-City Info Viewer
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyView)), "Tendências População + Dinheiro" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyView)),
                    "<Recomendado ATIVAR>\n" +
                    "Menu inferior: mostra tendências junto às <setas de dinheiro e população>.\n" +
                    "Recurso leve ao passar o mouse <só exibição>;\n" +
                    "economiza tempo e pode ser mais leve que abrir a tela de informações do jogo."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyViewMode)), "Frequência da exibição" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyViewMode)),
                    "Escolha se a tendência da barra inferior mostra valores por hora ou por mês para dinheiro e população.\n" +
                    "Mensal usa renda menos despesas e uma projeção de população de 24 horas."
                },
                { m_Settings.GetOptionLocaleID("MoneyViewModeHourly"), "Por hora (/h)" },
                { m_Settings.GetOptionLocaleID("MoneyViewModeMonthly"), "Mensal (/mo)" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipMode)), "Estilo do tooltip" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipMode)),
                    "Escolha quanto detalhe aparece no tooltip de dinheiro.\n" +
                    "<Mini> mostra só o Líquido em /h e /mo.\n" +
                    "<Compacto> mostra Renda, Despesas e Líquido só na unidade /h ou /mo escolhida.\n" +
                    "<Dados completos> mostra Renda, Despesas e Líquido em /h e /mo."
                },

                { m_Settings.GetOptionLocaleID("MoneyTooltipModeMini"), "Mini" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeCompact"), "Compacto" },
                { m_Settings.GetOptionLocaleID("MoneyTooltipModeFullData"), "Dados completos" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)), "Tamanho da fonte de dinheiro" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MoneyTooltipFontScale)),
                    "Ajusta o <tamanho da fonte> dos números do tooltip de dinheiro.\n" +
                    "Padrão do jogo = 100%\n" +
                    "<Padrão do mod = 120%>\n" +
                    "Passe o mouse sobre Dinheiro na parte de baixo.\n" +
                    "Para quem tem dificuldade de ler tooltips pequenos."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)), "Tamanho da fonte de população" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.PopulationTooltipFontScale)),
                    "Ajusta o <tamanho da fonte> dos números de população.\n" +
                    "Padrão do jogo = 100%\n" +
                    "<Padrão do mod = 120%>\n" +
                    "Passe o mouse sobre População na parte de baixo."
                },

                // --------------------------------------------------------------------
                // Mini-HUD tab - Mini HUD Notifications
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudEnabled)), "Mostrar Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudEnabled)),
                    "Mostra um pequeno painel HUD.\n" +
                    "Use como uma faixa rápida de alertas sem abrir todo o City Watchdog nem encher a cidade de ícones.\n" +
                    "Clique num ícone para ir ao problema. Continue clicando para passar pelos outros locais.\n" +
                    "**============================**\n" +
                    "Uma forma de usar:\n" +
                    "1. Desative todos os ícones normais no painel principal.\n" +
                    "2. Ative o Mini HUD para ver só 5 ou 10 favoritos.\n" +
                    "3. No painel completo, marque com **Estrela azul** o que quer acompanhar.\n" +
                    "4. O Mini HUD mostra os 5 ou 10 maiores valores atuais dessa lista.\n" +
                    ""
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)), "Clique - Início rápido" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.ApplyMiniHudRecommendedPreset)),
                    "Aplica um <início rápido> ao Mini HUD:\n" +
                    "Inclui uma **lista inicial de Estrelas azuis**.\n" +
                    "No modo Favoritos, o Mini HUD mostra os 5 ou 10 maiores valores atuais da lista.\n" +
                    "Adicione ou remova **Estrelas azuis** no City Watchdog.\n" +
                    "Define: Favoritos, 5 ícones, horizontal, arrastável, 100%, painel escuro e esconde zeros.\n" +
                    "Use Início rápido de novo quando quiser resetar."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudMode)), "Modo Mini HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudMode)),
                    "Escolha quais linhas o Mini HUD usa.\n" +
                    "**Mais ativos** mostra os maiores valores atuais.\n" +
                    "**Favoritos** usa todas as linhas com **Estrela azul** no painel principal.\n" +
                    "Você pode marcar quantos favoritos quiser,\n" +
                    "mas o Mini HUD mostra só os 5 ou 10 maiores valores."
                },
                { m_Settings.GetOptionLocaleID("MiniHudModeTopActive"), "Alertas mais ativos" },
                { m_Settings.GetOptionLocaleID("MiniHudModeFavorites"), "Favoritos" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Quantidade de ícones" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudItemCount)), "Escolha quantos ícones de notificação o Mini HUD pode mostrar de uma vez." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudScale)), "Tamanho dos ícones" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudScale)),
                    "Ajusta o tamanho dos ícones e números do Mini HUD.\n" +
                    "90% = compacto. 100% = padrão.\n" +
                    "Até 130% para enxergar melhor.\n" +
                    "Use 90% para ficar menor e mais discreto."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Orientação" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudOrientation)), "Escolha se os ícones do Mini HUD ficam em linha ou coluna." },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationHorizontal"), "Horizontal" },
                { m_Settings.GetOptionLocaleID("MiniHudOrientationVertical"), "Vertical" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPlacement)), "Posição do HUD" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPlacement)),
                    "Escolha onde o Mini HUD aparece.\n" +
                    "Arrastável deixa você mover pela interface da cidade."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopCenter"), "Centro superior" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementTopRight"), "Canto superior direito" },
                { m_Settings.GetOptionLocaleID("MiniHudPlacementDraggable"), "Arrastável" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelStyle)), "Estilo escuro ou vidro" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelStyle)),
                    "Escolha o fundo do Mini HUD.\n" +
                    "Vidro vai de transparente a branco nublado; não fica mais escuro.\n" +
                    "Use Escuro para um HUD mais escuro no estilo do jogo."
                },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleDark"), "Painel escuro" },
                { m_Settings.GetOptionLocaleID("MiniHudPanelStyleGlass"), "Painel de vidro" },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)), "Opacidade do mini painel" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudPanelOpacity)),
                    "Ajusta a transparência do Mini HUD.\n" +
                    "Menor = mais transparente.\n" +
                    "Maior = mais sólido.\n" +
                    "Vidro fica mais branco/nublado. Escuro fica mais sólido/escuro."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Esconder alertas zerados" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.MiniHudHideZero)), "Quando ativado [ ✓ ], o Mini HUD esconde linhas de notificação com valor 0." },

                // --------------------------------------------------------------------
                // About tab
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.NameText)), "Nome do mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.NameText)), "Nome exibido deste mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.VersionText)), "Versão" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.VersionText)), "Versão atual do mod." },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenParadox)), "Paradox Mods da Mochi" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenParadox)), "Abre a página do autor no Paradox Mods." },

                // --------------------------------------------------------------------
                // About tab - Diagnostics
                // --------------------------------------------------------------------

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)), "Relatório de debug no log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.WriteNotificationAuditLog)),
                    "<Não é necessário no jogo normal.>\n" +
                    "Para testes e checagens após patches do jogo: grava um relatório em <Logs/CityWatchdog.log>\n" +
                    "comparando as notificações atuais do jogo com os ícones controlados pelo Watchdog."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(CwdSettings.OpenLog)), "Abrir log" },
                { m_Settings.GetOptionDescLocaleID(nameof(CwdSettings.OpenLog)),
                    "Abre </Logs/CityWatchdog.log> se existir.\n" +
                    "Se não existir, abre a pasta Logs/."
                },
            };

            return entries;
        }

        public void Unload()
        {
        }
    }
}
