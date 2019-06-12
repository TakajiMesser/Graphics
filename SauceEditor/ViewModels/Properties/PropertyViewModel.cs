using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditor.ViewModels.Docks;
using Component = SauceEditor.Models.Components.Component;

namespace SauceEditor.ViewModels.Properties
{
    public class PropertyViewModel : DockViewModel, /*ISideDockViewModel, */IDisplayProperties
    {
        public PropertyViewModel() : base(DockTypes.Property) { }

        public IPropertyViewModel Properties { get; set; }
        //public bool IsActive { get; set; }

        public void OnPropertiesChanged() => AddChild((ViewModel)Properties, (s, args) => InvokePropertyChanged(nameof(Properties)));

        public void InitializeProperties(Component component)
        {
            switch (component)
            {
                case MapComponent mapComponent:
                    Properties = new EntityPropertyViewModel();
                    UpdateFromModel(null);
                    break;
                case MaterialComponent materialComponent:
                    Properties = new MaterialPropertyViewModel();
                    UpdateFromModel(component);
                    break;
                case TextureComponent textureComponent:
                    Properties = new TexturePropertyViewModel();
                    UpdateFromModel(component);
                    break;
            }
        }

        public void UpdateFromEntity(EditorEntity entity)
        {
            if (Properties is EntityPropertyViewModel entityPropertyViewModel)
            {
                entityPropertyViewModel.UpdateFromModel(entity);

                if (entity != null)
                {
                    IsActive = true;
                }
            }
        }

        public void UpdateFromModel(object model)
        {
            /*if (model == null)
            {
                // Find way to disable binding?
            }
            else
            {*/
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
            //}
        }
    }
}