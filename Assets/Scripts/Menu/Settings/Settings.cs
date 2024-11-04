using System;
using UnityEngine.Rendering.PostProcessing;

namespace Menu.Settings
{
    [Serializable]
    public class Settings
    {
        public AudioSettings audioSettings = new();
        public VideoSettings videoSettings = new();
        public ControlSettings controlSettings = new();
    }

    [Serializable]
    public class AudioSettings : SettingsModel
    {
        [MinMax(0f, 1f)] private double _backgroundMusic = .5;

        [SettingsElement("Slider_BackgroundMusic_Bindable")]
        public double backgroundMusic
        {
            get => _backgroundMusic;
            set => this.SetAndSafeIfChanged(ref _backgroundMusic, value);
        }
        
        [MinMax(0f, 1f)] private double _soundEffects = .5;

        [SettingsElement("Slider_SoundEffects_Bindable")]
        public double soundEffects
        {
            get => _soundEffects;
            set => this.SetAndSafeIfChanged(ref _soundEffects, value);
        }
    }

    [Serializable]
    public class VideoSettings : SettingsModel
    {
        
        private Quality _quality = Quality.Medium;

        public Quality quality
        {
            get => _quality;
            set => this.SetAndSafeIfChanged(ref _quality, value);
        }
        
        private Resolution _resolution = Resolution._1080p;

        public Resolution resolution
        {
            get => _resolution;
            set => this.SetAndSafeIfChanged(ref _resolution, value);
        }
    }

    [Serializable]
    public enum Quality
    {
        Low,
        Medium,
        High
    }

    [Serializable]
    public enum Resolution
    {
        _1080p,
        _1440p
    }

    [Serializable]
    public class ControlSettings : SettingsModel
    {
        [MinMax(0f, 1f)]
        private double _sensitivity = .5;
        
        public double sensitivity
        {
            get => _sensitivity;
            set => this.SetAndSafeIfChanged(ref _sensitivity, value);
        }

        private bool _mouseControl;

        public bool mouseControl
        {
            get => _mouseControl;
            set => this.SetAndSafeIfChanged(ref _mouseControl, value);
        }
    }
}