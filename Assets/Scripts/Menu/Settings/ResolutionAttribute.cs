using System;

namespace Menu.Settings
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ResolutionAttribute : Attribute
    {
        public int width { get; }
        public int height { get; }
        
        public ResolutionAttribute(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}