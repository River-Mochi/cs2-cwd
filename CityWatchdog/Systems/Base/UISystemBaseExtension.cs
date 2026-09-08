// <copyright file="UISystemBaseExtension.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Systems/UISystemBaseExtension.cs
// Purpose: Local UI binding helpers used by the React bridge.

namespace CityWatchdog.Systems
{
    using System;
    using Colossal.UI.Binding;
    using Game.UI;

    public abstract partial class UISystemBaseExtension : UISystemBase
    {
        public virtual string ModId { get; set; } = Mod.ModId;

        public BoolBinding AddBoolBindingAndTriggerBinding(string name, bool initialValue, Action<bool> callback)
        {
            BoolBinding boolBinding = AddBoolBinding(name, initialValue);
            AddBoolTriggerBinding(name, callback);
            return boolBinding;
        }

        public BoolBinding AddBoolBinding(string name, bool initialValue)
        {
            BoolBinding boolBinding = new(ModId, name, initialValue);
            AddBinding(boolBinding.ValueBinding);
            return boolBinding;
        }

        public void AddBoolTriggerBinding(string name, Action<bool> callback)
        {
            AddTriggerBinding(name, callback);
        }

        public TriggerBinding<T> AddTriggerBinding<T>(string name, Action<T> callback)
        {
            TriggerBinding<T> triggerBinding = new(ModId, name, callback);
            AddBinding(triggerBinding);
            return triggerBinding;
        }

        // No-argument trigger (React calls trigger(mod.id, name) with no payload).
        public TriggerBinding AddTriggerBinding(string name, Action callback)
        {
            TriggerBinding triggerBinding = new(ModId, name, callback);
            AddBinding(triggerBinding);
            return triggerBinding;
        }

        public ValueBinding<T> AddValueBinding<T>(string name, T initialValue)
        {
            ValueBinding<T> valueBinding = new(ModId, name, initialValue);
            AddBinding(valueBinding);
            return valueBinding;
        }
    }

    public sealed class BoolBinding
    {
        public ValueBinding<bool> ValueBinding { get; }

        public Action<bool>? OnValueChanged;

        public bool Value
        {
            get => ValueBinding.value;
            set
            {
                if (value == ValueBinding.value)
                {
                    return;
                }

                Update(value);
                OnValueChanged?.Invoke(value);
            }
        }

        public BoolBinding(string group, string name, bool initialValue)
        {
            ValueBinding = new ValueBinding<bool>(group, name, initialValue);
        }

        public void Update()
        {
            Update(!ValueBinding.value);
        }

        public void Update(bool value)
        {
            ValueBinding.Update(value);
        }
    }
}
