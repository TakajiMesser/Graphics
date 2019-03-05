using System;

namespace SauceEditor.Models
{
    public class SettingsEventArgs : EventArgs
    {
        public EditorSettings Settings { get; private set; }

        public SettingsEventArgs(EditorSettings settings)
        {
            Settings = settings;
        }
    }
}
