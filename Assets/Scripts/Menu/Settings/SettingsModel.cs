using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Menu.Settings
{
    public class SettingsModel
    {
        public delegate void SettingsChangedEvent(string property);

        public event SettingsChangedEvent OnChanged;
        public void RaiseSettingsChanged(string property) => OnChanged?.Invoke(property);
    }

    public static class SettingsModelExtensions
    {
        public static TRet SetAndSafeIfChanged<TObj, TRet>(this TObj model, ref TRet backingField, TRet newValue,
            [CallerMemberName] [CanBeNull] string propertyName = null) where TObj : SettingsModel
        {
            if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));
            
            if (EqualityComparer<TRet>.Default.Equals(backingField, newValue))
            {
                return newValue;
            }

            backingField = newValue;
            model.RaiseSettingsChanged(propertyName);
            return newValue;
        }
    }
}