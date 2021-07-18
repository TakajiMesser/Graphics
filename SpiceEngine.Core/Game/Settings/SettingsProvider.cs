using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Game.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private Dictionary<Type, ISubSettings> _subSettingsByType = new Dictionary<Type, ISubSettings>();

        protected List<ISubSettings> _subSettings = new List<ISubSettings>();

        public virtual void AddSubSettings<T>(T subSettings) where T : ISubSettings
        {
            _subSettingsByType.Add(typeof(T), subSettings);
            _subSettings.Add(subSettings);
        }

        public T GetSubSettings<T>() where T : ISubSettings => (T)_subSettingsByType[typeof(T)];
        public T GetSubSettingsOrDefault<T>() where T : ISubSettings => HasSubSettings<T>() ? GetSubSettings<T>() : default;

        public bool HasSubSettings<T>() where T : ISubSettings => _subSettingsByType.ContainsKey(typeof(T));
    }
}
