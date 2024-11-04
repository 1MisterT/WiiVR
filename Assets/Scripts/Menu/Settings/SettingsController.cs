using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Menu.Settings
{
    public class SettingsController : MonoBehaviour
    {
        public static Settings settings;
        
        private void Start()
        {
            if (PlayerPrefs.HasKey(nameof(Settings)))
            {
                settings = JsonConvert.DeserializeObject<Settings>(PlayerPrefs.GetString(nameof(Settings)));
            }
            else
            {
                settings = new();
                PlayerPrefs.SetString(nameof(Settings), JsonConvert.SerializeObject(settings));
                PlayerPrefs.Save();
            }
            
            SubscribeToChanges(settings);
        }

        public static void SettingsChanged(string property)
        {
            PlayerPrefs.SetString(nameof(Settings), JsonConvert.SerializeObject(settings));
            PlayerPrefs.Save();
        }

        private void SubscribeToChanges(object obj)
        {
            if (obj is null) return;

            var fields = obj.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.FieldType.IsSubclassOf(typeof(SettingsModel)))
                .ToArray();
            
            SetValues(obj);
            
            if (fields.Length == 0) return;

            foreach (var field in fields)
            {
                var val = (SettingsModel) field.GetValue(obj);
                if (val is null) continue;
                val.OnChanged += SettingsChanged;
                SubscribeToChanges(val);
            }
        }

        private void SetValues(object obj)
        {
            var properties = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(pi => pi.GetCustomAttribute<SettingsElementAttribute>() is not null)
                .ToArray();

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<SettingsElementAttribute>()!;
                var go = GameObject.Find(attr.objectName);
                if (go is null) continue;

                if (attr.type == typeof(Slider))
                {
                    ValueGameObjectBinder<float, Slider>(
                        property, go, obj,
                        (input, value) => input.value = value,
                        input => input.value,
                        input => input.onValueChanged
                    );
                }
                else if (attr.type == typeof(Toggle))
                {
                    ValueGameObjectBinder<bool, Toggle>(
                        property, go, obj,
                        (input, value) => input.isOn = value,
                        input => input.isOn,
                        input => input.onValueChanged
                    );
                }
                else if (attr.type == typeof(TMP_Dropdown))
                {
                    ValueGameObjectBinder<int, TMP_Dropdown>(
                        property, go, obj,
                        (input, value) => input.value = value,
                        input => input.value,
                        input => input.onValueChanged
                    );
                }
            }
        }

        private void ValueGameObjectBinder<TValueType, TInputType>(
            PropertyInfo property, GameObject go, object obj, 
            Action<TInputType, TValueType> setter, 
            Func<TInputType, TValueType> getter,
            Func<TInputType, UnityEvent<TValueType>> @event
        )
        {
            var component = go.GetComponent<TInputType>();
            setter(component, (TValueType) property.GetValue(obj));

            void SetValue(TValueType value) => property.SetValue(obj, value);
            
            SetValue(getter(component));
            @event(component).AddListener(SetValue);
        }
    }
}