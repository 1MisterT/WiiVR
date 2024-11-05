using System;
using System.Diagnostics.CodeAnalysis;

namespace Menu
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum MainMenuState
    {
        Main,
        GameSelection,
        HighScore,
        Options,
        Options_Audio,
        Options_Video,
        Options_Controls
    }
}