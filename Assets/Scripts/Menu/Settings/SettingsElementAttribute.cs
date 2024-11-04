using System;

namespace Menu.Settings
{
    public class SettingsElementAttribute : Attribute
    {
        public string objectName { get; }
        
        public SettingsElementAttribute(string objectName)
        {
            this.objectName = objectName;
        }
    }
}