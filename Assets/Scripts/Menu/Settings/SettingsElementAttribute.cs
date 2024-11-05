using System;

namespace Menu.Settings
{
    public class SettingsElementAttribute : Attribute
    {
        public string objectName { get; }
        public Type type { get; }
        
        public SettingsElementAttribute(string objectName, Type type)
        {
            this.objectName = objectName;
            this.type = type;
        }
    }
}