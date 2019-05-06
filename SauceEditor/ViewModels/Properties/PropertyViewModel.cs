using OpenTK;
using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Utilities;
using System.ComponentModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SauceEditor.ViewModels.Properties
{
    public class PropertyViewModel : ViewModel, ISideDockViewModel
    {
        public IPropertyViewModel Properties { get; set; }
        public bool IsActive { get; set; }

        public void UpdateFromModel(object model)
        {
            if (model == null)
            {
                // Find way to disable binding?
            }
            else
            {
                switch (model)
                {
                    case EditorEntity editorEntity:
                        ((EntityPropertyViewModel)Properties).UpdateFromModel(editorEntity);
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