// <copyright file="InterfaceScaleControlSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/InterfaceScaleControlSystem.cs
// Purpose: Toggles the vanilla interface-scaling flag from CWD and applies it immediately.

namespace CityWatchdog.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Colossal.UI.Binding;

    using CS2Shared.RiverMochi;

    using Game.SceneFlow;
    using Game.Settings;
    using Game.UI;
    using Game.UI.Menu;

    public partial class InterfaceScaleControlSystem : UISystemBaseExtension
    {
        private const string kInterfaceScaleBindingName = "InterfaceScaleEnabled";
        private const string kVanillaInterfaceScalePath = "options.interfaceScaling";

        private BoolBinding m_InterfaceScaleEnabledBinding = null!;
        private InterfaceSettings? m_SubscribedUi;
        private IUpdateBinding? m_CachedScalingBinding;
        private bool m_ScalingBindingLookupFailed;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_InterfaceScaleEnabledBinding = AddBoolBindingAndTriggerBinding(
                kInterfaceScaleBindingName,
                GetUI()?.interfaceScaling ?? false,
                SetInterfaceScaling);
        }

        protected override void OnUpdate()
        {
            // InterfaceSettings may not exist at OnCreate. Hook it once, then use its event.
            if (m_SubscribedUi != null)
            {
                return;
            }

            InterfaceSettings? ui = GetUI();
            if (ui == null)
            {
                return;
            }

            m_InterfaceScaleEnabledBinding.Update(ui.interfaceScaling);
            ui.onSettingsApplied += OnVanillaSettingsApplied;
            m_SubscribedUi = ui;
        }

        private static InterfaceSettings? GetUI() => GameManager.instance?.settings?.userInterface;

        private void OnVanillaSettingsApplied(Setting _)
        {
            InterfaceSettings? ui = GetUI();
            if (ui != null && m_InterfaceScaleEnabledBinding.Value != ui.interfaceScaling)
            {
                m_InterfaceScaleEnabledBinding.Update(ui.interfaceScaling);
            }
        }

        // Vanilla owns and saves this value; CWD only passes changes through.
        public void SetInterfaceScaling(bool enable)
        {
            InterfaceSettings? ui = GetUI();
            if (ui == null)
            {
                return;
            }

            ui.interfaceScaling = enable;

            try
            {
                ui.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "interface-scale-apply",
                    () => $"Failed to apply interface scaling: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }

            // OptionsUISystem normally pushes this binding only while Options is open.
            PushVanillaInterfaceScalingBinding();
            m_InterfaceScaleEnabledBinding.Update(ui.interfaceScaling);
        }

        private void PushVanillaInterfaceScalingBinding()
        {
            try
            {
                // Update() re-reads vanilla getter and pushes the changed value to the UI.
                ResolveScalingBinding()?.Update();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "interface-scale-binding-push",
                    () => $"Failed to push interfaceScaling binding: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private IUpdateBinding? ResolveScalingBinding()
        {
            if (m_CachedScalingBinding != null || m_ScalingBindingLookupFailed)
            {
                return m_CachedScalingBinding;
            }

            OptionsUISystem? options = World.GetExistingSystemManaged<OptionsUISystem>();
            if (options == null)
            {
                return null;
            }

            // AddUpdateBinding stores its bindings in this private UISystemBase list.
            FieldInfo? field = typeof(UISystemBase).GetField(
                "m_UpdateBindings",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (field?.GetValue(options) is not IEnumerable<IUpdateBinding> bindings)
            {
                m_ScalingBindingLookupFailed = true;
                LogUtils.WarnOnce(
                    "interface-scale-binding-missing",
                    () => "Could not read OptionsUISystem update bindings; UI scale will apply when Options is opened.");
                return null;
            }

            foreach (IUpdateBinding candidate in bindings)
            {
                if (candidate is BindingBase namedBinding &&
                    namedBinding.path == kVanillaInterfaceScalePath)
                {
                    m_CachedScalingBinding = candidate;
                    return m_CachedScalingBinding;
                }
            }

            m_ScalingBindingLookupFailed = true;
            LogUtils.WarnOnce(
                "interface-scale-binding-notfound",
                () => "options.interfaceScaling binding not found; UI scale will apply when Options is opened.");
            return null;
        }

        protected override void OnDestroy()
        {
            if (m_SubscribedUi != null)
            {
                m_SubscribedUi.onSettingsApplied -= OnVanillaSettingsApplied;
                m_SubscribedUi = null;
            }

            base.OnDestroy();
        }
    }
}
