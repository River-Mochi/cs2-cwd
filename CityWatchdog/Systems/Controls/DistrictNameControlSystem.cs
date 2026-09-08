// <copyright file="DistrictNameControlSystem.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/DistrictNameControlSystem.cs
// Purpose: Hides district names only.

namespace CityWatchdog.Systems
{
    using System;
    using System.Reflection;
    using CS2Shared.RiverMochi;
    using Game.Areas;
    using Game.Rendering;

    public partial class DistrictNameControlSystem : UISystemBaseExtension
    {
        private AreaBufferSystem? m_CachedAreaBufferSystem;
        private FieldInfo? m_HasNameMeshField;
        private object? m_DistrictAreaTypeData;
        private bool m_ReflectionReady;
        private bool m_ReflectionFailed;
        private BoolBinding m_HideDistrictNamesBinding = null!;
        private bool m_CurrentlyHiding;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_CurrentlyHiding = CwdSettings.Instance?.HideDistrictNames ?? false;
            m_HideDistrictNamesBinding = AddBoolBindingAndTriggerBinding(
                nameof(CwdSettings.HideDistrictNames),
                m_CurrentlyHiding,
                OnHideDistrictNamesToggle);
        }

        // Restore vanilla's ready flag on unload so disabling CWD cannot leave names hidden.
        protected override void OnDestroy()
        {
            if (m_CurrentlyHiding)
            {
                SetHasNameMesh(true);
            }

            base.OnDestroy();
        }

        public void SyncFromSettings()
        {
            bool value = CwdSettings.Instance?.HideDistrictNames ?? false;
            if (m_HideDistrictNamesBinding.Value != value)
            {
                m_HideDistrictNamesBinding.Update(value);
            }

            ApplySetting(value);
        }

        protected override void OnUpdate()
        {
            if (!m_ReflectionReady && !m_ReflectionFailed)
            {
                InitializeReflection();
            }

            bool settingValue = CwdSettings.Instance?.HideDistrictNames ?? false;
            if (settingValue != m_CurrentlyHiding)
            {
                if (m_HideDistrictNamesBinding.Value != settingValue)
                {
                    m_HideDistrictNamesBinding.Update(settingValue);
                }

                ApplySetting(settingValue);
            }

            if (m_CurrentlyHiding && m_ReflectionReady)
            {
                SuppressDistrictNameMesh();
            }
        }

        private void OnHideDistrictNamesToggle(bool value)
        {
            m_HideDistrictNamesBinding.Update(value);

            CwdSettings? setting = CwdSettings.Instance;
            if (setting != null)
            {
                setting.HideDistrictNames = value;
                TryPersist(setting);
            }

            ApplySetting(value);
        }

        private void ApplySetting(bool hide)
        {
            m_CurrentlyHiding = hide;
            if (!m_CurrentlyHiding)
            {
                SetHasNameMesh(true);
            }
        }

        private void SuppressDistrictNameMesh()
        {
            try
            {
                // AreaBufferSystem prepared this mesh earlier in the frame. Read first, then
                // clear only District ready flag so vanilla skips the label draw.
                m_CachedAreaBufferSystem?.GetNameMesh(AreaType.District, out _, out _);
                SetHasNameMesh(false);
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "district-name-render-filter",
                    () => $"District-name rendering filter failed: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        // GetNameMesh is public, but the per-area ready flag is not. Reflection is limited
        // to this one flag so boundaries and other area labels remain untouched.
        private void InitializeReflection()
        {
            try
            {
                m_CachedAreaBufferSystem = World.GetExistingSystemManaged<AreaBufferSystem>();
                // Rendering system may not exist yet during startup, so retry next update.
                if (m_CachedAreaBufferSystem == null)
                {
                    return;
                }

                Type bufferType = typeof(AreaBufferSystem);
                Type? areaTypeDataType = bufferType.GetNestedType("AreaTypeData", BindingFlags.NonPublic);
                if (areaTypeDataType == null)
                {
                    LogUtils.WarnOnce(
                        "district-name-reflect",
                        () => "Cannot find AreaBufferSystem.AreaTypeData; district-name toggle disabled.");
                    m_ReflectionFailed = true;
                    return;
                }

                FieldInfo? arrayField = bufferType.GetField(
                    "m_AreaTypeData",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                m_HasNameMeshField = areaTypeDataType.GetField(
                    "m_HasNameMesh",
                    BindingFlags.Instance | BindingFlags.Public);

                if (arrayField == null ||
                    m_HasNameMeshField == null ||
                    m_HasNameMeshField.FieldType != typeof(bool))
                {
                    LogUtils.WarnOnce(
                        "district-name-reflect",
                        () => "District-name toggle disabled because CS2 area rendering internals changed.");
                    m_ReflectionFailed = true;
                    return;
                }

                int districtIndex = (int)AreaType.District;
                if (arrayField.GetValue(m_CachedAreaBufferSystem) is not Array array || districtIndex < 0 || districtIndex >= array.Length)
                {
                    LogUtils.WarnOnce(
                        "district-name-reflect",
                        () => "m_AreaTypeData does not contain a District entry; district-name toggle disabled.");
                    m_ReflectionFailed = true;
                    return;
                }

                m_DistrictAreaTypeData = array.GetValue(districtIndex);
                if (m_DistrictAreaTypeData == null)
                {
                    LogUtils.WarnOnce(
                        "district-name-reflect",
                        () => "The District AreaTypeData entry is null; district-name toggle disabled.");
                    m_ReflectionFailed = true;
                    return;
                }

                m_ReflectionReady = true;
#if DEBUG
                LogUtils.Debug(() => $"District reflection OK: AreaTypeData found, District index={districtIndex}, m_HasNameMesh bool field found.");
#endif
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "district-name-init",
                    () => $"District-name reflection initialization failed: {ex.GetType().Name}: {ex.Message}",
                    ex);
                m_ReflectionFailed = true;
            }
        }

        private void SetHasNameMesh(bool value)
        {
            if (m_DistrictAreaTypeData == null || m_HasNameMeshField == null)
            {
                return;
            }

            try
            {
                m_HasNameMeshField.SetValue(m_DistrictAreaTypeData, value);
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "district-name-set-mesh",
                    () => $"Setting district m_HasNameMesh failed: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }

        private static void TryPersist(CwdSettings setting)
        {
            try
            {
                setting.ApplyAndSave();
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "district-name-save",
                    () => $"Failed to persist HideDistrictNames: {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }
    }
}
