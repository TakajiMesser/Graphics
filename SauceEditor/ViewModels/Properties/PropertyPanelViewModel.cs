using PropertyChanged;
using SauceEditor.ViewModels.Docks;
using SauceEditor.Views.Custom;
using SauceEditorCore.Models.Components;
using SauceEditorCore.Models.Entities;

namespace SauceEditor.ViewModels.Properties
{
    public class PropertyPanelViewModel : DockableViewModel
    {
        public IDisplayProperties PropertyDisplayer { get; set; }

        [PropagateChanges]
        [DoNotCheckEquality]
        public IPropertyViewModel Properties { get; set; }

        public bool IsVisible { get; set; }
        //public bool IsActive { get; set; }

        //public void OnPropertiesChanged() => AddChild((ViewModel)Properties, (s, args) => InvokePropertyChanged(nameof(Properties)));

        public void InitializeProperties(Component component)
        {
            if (component is MapComponent)
            {
                var properties = new EntityPropertyViewModel();
                Properties = properties;

                properties.SetPropertyDisplayer(PropertyDisplayer);
                UpdateFromModel(null);
            }
            else if (component is MaterialComponent)
            {
                Properties = new MaterialPropertyViewModel();
                UpdateFromModel(component);
            }
            else if (component is TextureComponent)
            {
                Properties = new TexturePropertyViewModel();
                UpdateFromModel(component);
            }
        }

        public void UpdateFromModel(object model)
        {
            if (model == null)
            {
                // Find way to disable binding?
                IsVisible = false;
            }
            else
            {
                IsVisible = true;

                switch (model)
                {
                    case EditorEntity editorEntity:
                        if (Properties is EntityPropertyViewModel entityPropertyViewModel)
                        {
                            entityPropertyViewModel.UpdateFromModel(editorEntity);
                        }
                        break;
                    case MaterialComponent materialComponent:
                        ((MaterialPropertyViewModel)Properties).UpdateFromModel(materialComponent);
                        break;
                    case TextureComponent textureComponent:
                        ((TexturePropertyViewModel)Properties).UpdateFromModel(textureComponent);
                        break;
                }
            }
        }
    }
}