using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Menu.Settings
{
    public class SettingsController : MonoBehaviour
    {
        public static Settings settings;
        
        private void Start()
        {
            if (PlayerPrefs.HasKey(nameof(Settings)))
            {
                settings = JsonUtility.FromJson<Settings>(PlayerPrefs.GetString(nameof(Settings)));
            }
            else
            {
                settings = new();
                PlayerPrefs.SetString(nameof(Settings), JsonUtility.ToJson(settings));
                PlayerPrefs.Save();
            }
            
            SubscribeToChanges(settings);
        }

        public static void SettingsChanged(string property)
        {
            Debug.Log($"PropertyChanged: {property}");
            PlayerPrefs.SetString(nameof(Settings), JsonUtility.ToJson(settings));
            PlayerPrefs.Save();
        }

        private void SubscribeToChanges(object obj)
        {
            if (obj is null) return;

            var fields = settings.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetType().IsSubclassOf(typeof(SettingsModel)))
                .ToArray();
            
            if (fields.Length == 0) return;

            foreach (var field in fields)
            {
                var val = (SettingsModel) field.GetValue(obj);
                if (val is null) continue;
                val.OnChanged += SettingsChanged;
                SubscribeToChanges(val);
            }
        }
    }
}