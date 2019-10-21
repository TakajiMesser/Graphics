using SauceEditor.Utilities;
using SauceEditor.Views.Custom;
using System.ComponentModel;
using System.Reflection;

namespace SauceEditor.ViewModels.Properties
{
    public abstract class PropertyViewModel<T> : ViewModel
    {
        private IDisplayProperties _propertyDisplayer;

        [Browsable(false)]
        public T Model { get; private set; }

        [Browsable(false)]
        public bool HasValue => Model != null;

        public void SetPropertyDisplayer(IDisplayProperties propertyDisplayer)
        {
            _propertyDisplayer = propertyDisplayer;
            InitializeProperties();
        }

        public void UpdateFromModel(T model)
        {
            Model = model;
            UpdatePropertiesFromModel(model);
        }

        protected virtual void UpdatePropertiesFromModel(T model) { }

        public virtual void OnPropertyChanged(string propertyName)
        {
            var propertyDisplayer = _propertyDisplayer;

            if (propertyDisplayer != null)
            {
                var propertyInfo = GetType().GetProperty(propertyName);

                if (propertyInfo != null)
                {
                    HandleProperty(propertyDisplayer, propertyInfo);
                }
            }

            InvokePropertyChanged(propertyName);
        }

        private void InitializeProperties()
        {
            var propertyDisplayer = _propertyDisplayer;

            if (propertyDisplayer != null)
            {
                foreach (var propertyInfo in GetType().GetProperties())
                {
                    HandleProperty(propertyDisplayer, propertyInfo);
                }
            }
        }

        private void HandleProperty(IDisplayProperties propertyDisplayer, PropertyInfo propertyInfo)
        {
            if (propertyInfo.HasCustomAttribute<HideIfNullPropertyAttribute>())
            {
                var propertyValue = propertyInfo.GetValue(this);

                var visibility = propertyValue != null
                    ? System.Windows.Visibility.Visible
                    : System.Windows.Visibility.Collapsed;

                propertyDisplayer.SetPropertyVisibility(propertyInfo.Name, visibility);
            }

            /*var attribute = propertyInfo.GetCustomAttributes(typeof(HideIfNullPropertyAttribute), true).FirstOrDefault();

            if (attribute is HideIfNullPropertyAttribute)
            {
                var propertyValue = propertyInfo.GetValue(this);

                var visibility = propertyValue != null
                    ? System.Windows.Visibility.Visible
                    : System.Windows.Visibility.Collapsed;

                propertyDisplayer.SetPropertyVisibility(propertyInfo.Name, visibility);
            }*/
        }
    }
}