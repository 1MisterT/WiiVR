using System;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Menu.Settings
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class Settings
    {
        public AudioSettings audioSettings = new();
        public VideoSettings videoSettings = new();
        public ControlSettings controlSettings = new();
    }

    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class AudioSettings : SettingsModel
    {
        [MinMax(0f, 1f)] private float _backgroundMusic = .5f;

        [SettingsElement("Slider_BackgroundMusic_Bindable", typeof(Slider))]
        public float backgroundMusic
        {
            get => _backgroundMusic;
            set
            {
                this.SetAndSafeIfChanged(ref _backgroundMusic, value);
                GameObject.Find("GameMusic").GetComponent<AudioSource>().volume = _backgroundMusic;
            }
        }

        [MinMax(0f, 1f)] private float _soundEffects = .5f;

        [SettingsElement("Slider_SoundEffects_Bindable", typeof(Slider))]
        public float soundEffects
        {
            get => _soundEffects;
            set
            {
                this.SetAndSafeIfChanged(ref _soundEffects, value);
                GameObject.Find("SoundFXManager").GetComponent<SoundFXManager>().soundFXObject.volume = _soundEffects;
            }
        }
    }

    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class VideoSettings : SettingsModel
    {
        
        private Quality _quality = Quality.Medium;

        [SettingsElement("Selector_GraphicsQuality_Bindable", typeof(TMP_Dropdown))]
        public Quality quality
        {
            get => _quality;
            set
            {
                this.SetAndSafeIfChanged(ref _quality, value);
                QualitySettings.SetQualityLevel((int)_quality, true);
            }
        }

        private Resolution _resolution = Resolution._1080p;

        [SettingsElement("Selector_Resolution_Bindable", typeof(TMP_Dropdown))]
        public Resolution resolution
        {
            get => _resolution;
            set
            {
                this.SetAndSafeIfChanged(ref _resolution, value);
                var r = _resolution.GetResolution();
                Screen.SetResolution(r.width, r.height, Screen.fullScreen);
            }
        }
    }

    [Serializable]
    public enum Quality
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
        Ultra
    }

    [Serializable]
    public enum Resolution
    {
        [Resolution(1920, 1080)]
        _1080p,
        [Resolution(2560, 1440)]
        _1440p
    }

    // not implemented weil - genickbruch? 
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class ControlSettings : SettingsModel
    {
        [MinMax(0f, 1f)]
        private float _sensitivity = .5f;
        
        [SettingsElement("Slider_Sensitivity_Bindable", typeof(Slider))]
        public float sensitivity
        {
            get => _sensitivity;
            set => this.SetAndSafeIfChanged(ref _sensitivity, value);
        }

        private bool _mouseControl;

        [SettingsElement("Toggle_MouseControl_Bindable", typeof(Toggle))]
        public bool mouseControl
        {
            get => _mouseControl;
            set => this.SetAndSafeIfChanged(ref _mouseControl, value);
        }
    }
}