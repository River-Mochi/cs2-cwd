// <copyright file="Mod.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Mod.cs
// Purpose: Mod entrypoint; registers settings, localization, systems, keybindings, and the dedicated mod log.

namespace CityWatchdog
{
    using System;
    using System.Reflection;

    using CityWatchdog.Systems;

    using Colossal.IO.AssetDatabase;
    using Colossal.Localization;
    using Colossal.Logging;

    using CS2Shared.RiverMochi;

    using Game;
    using Game.Modding;
    using Game.SceneFlow;

    public sealed class Mod : IMod
    {
        public const string ModName = "City Watchdog";
        public const string ModId = "CityWatchdog";
        public const string ModTag = "[CWD]";

        public static readonly string ModVersion =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.1.0";

        public static readonly ILog s_Log =
            LogManager.GetLogger(ModId).SetShowsErrorsInUI(false);

        internal static CwdSettings? Settings { get; private set; }

        private static bool s_BannerLogged;

        public void OnLoad(UpdateSystem updateSystem)
        {
            LogUtils.Configure(ModId, s_Log);
            ShellOpen.Configure(s_Log, ModId, ModTag);

            if (!s_BannerLogged)
            {
                s_BannerLogged = true;

#if DEBUG
                LogUtils.Info($"{ModName} v{ModVersion} DEBUG loaded");
#else
                LogUtils.Info($"{ModName} v{ModVersion} loaded");
#endif
            }

            if (GameManager.instance == null)
            {
                LogUtils.Warn($"{ModTag} GameManager.instance is null; {ModName} cannot initialize.");
                return;
            }

            CwdSettings setting = new(this);
            Settings = CwdSettings.Instance = setting;

            try
            {
                LocalizationManager? localizationManager =
                    GameManager.instance.localizationManager;

                if (localizationManager == null)
                {
                    LogUtils.Warn(
                        $"{ModTag} LocalizationManager is null; locale sources were not registered.");
                }
                else
                {
                    // Options UI strings.
                    localizationManager.AddSource("en-US", new LocaleEN(setting));
                    localizationManager.AddSource("fr-FR", new LocaleFR(setting));
                    localizationManager.AddSource("es-ES", new LocaleES(setting));
                    localizationManager.AddSource("de-DE", new LocaleDE(setting));
                    localizationManager.AddSource("it-IT", new LocaleIT(setting));
                    localizationManager.AddSource("ja-JP", new LocaleJA(setting));
                    localizationManager.AddSource("ko-KR", new LocaleKO(setting));
                    localizationManager.AddSource("pl-PL", new LocalePL(setting));
                    localizationManager.AddSource("pt-BR", new LocalePT_BR(setting));
                    localizationManager.AddSource("pt-PT", new LocalePT_PT(setting));
                    localizationManager.AddSource("zh-HANS", new LocaleZH_HANS(setting));
                    localizationManager.AddSource("zh-HANT", new LocaleZH_HANT(setting));
                    localizationManager.AddSource("th-TH", new LocaleTH(setting));
                    localizationManager.AddSource("vi-VN", new LocaleVI(setting));
                    localizationManager.AddSource("tr-TR", new LocaleTR(setting));
                }
            }
            catch (Exception ex)
            {
                LogUtils.Error(
                    $"{ModTag} Options localization registration failed: " +
                    $"{ex.GetType().Name}: {ex.Message}",
                    ex);
            }

            // Custom in-city React UI strings from embedded lang/*.json.
            InCityLocalization.LoadEmbeddedJsonTranslations(ModId, ModTag);

            try
            {
                AssetDatabase.global.LoadSettings(
                    ModId,
                    setting,
                    new CwdSettings(this));
            }
            catch (Exception ex)
            {
                LogUtils.Error(
                    $"{ModTag} Settings load failed: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }

            setting.NormalizeLoadedSettings();

            try
            {
                setting.RegisterInOptionsUI();
            }
            catch (Exception ex)
            {
                LogUtils.Error(
                    $"{ModTag} Options UI registration failed: " +
                    $"{ex.GetType().Name}: {ex.Message}",
                    ex);
            }

            try
            {
                setting.RegisterKeyBindings();
            }
            catch (Exception ex)
            {
                LogUtils.Error(
                    $"{ModTag} Keybinding registration failed: " +
                    $"{ex.GetType().Name}: {ex.Message}",
                    ex);
            }

            try
            {
                updateSystem.UpdateAt<CityWatchdogUISystem>(SystemUpdatePhase.UIUpdate);
                updateSystem.UpdateAt<EditorQuickControlsUISystem>(SystemUpdatePhase.UIUpdate);
                updateSystem.UpdateAt<TooltipControlSystem>(SystemUpdatePhase.UIUpdate);
                updateSystem.UpdateAt<RoadNameControlSystem>(SystemUpdatePhase.UIUpdate);
                updateSystem.UpdateAt<DistrictNameControlSystem>(SystemUpdatePhase.Rendering);
                updateSystem.UpdateAt<RoadArrowControlSystem>(SystemUpdatePhase.UIUpdate);
                updateSystem.UpdateAt<InterfaceScaleControlSystem>(SystemUpdatePhase.UIUpdate);
                updateSystem.UpdateAt<AlertIconSystem>(SystemUpdatePhase.ModificationEnd);
            }
            catch (Exception ex)
            {
                LogUtils.Error(
                    $"{ModTag} System scheduling failed: " +
                    $"{ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        public void OnDispose()
        {
            Settings?.UnregisterInOptionsUI();
            Settings = null;
        }
    }
}
