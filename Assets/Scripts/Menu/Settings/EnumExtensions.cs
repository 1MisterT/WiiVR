#nullable enable
using System;
using System.Reflection;

namespace Menu.Settings
{
    public static class EnumExtensions
    {
        public static (int width, int height) GetResolution(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo? fieldInfo = type.GetField(value.ToString());

            if (fieldInfo is null) return (0, 0);
            if (fieldInfo.GetCustomAttribute<ResolutionAttribute>() is not { } resolution) return (0, 0);
            return (resolution.width, resolution.height);
        }
    }
}