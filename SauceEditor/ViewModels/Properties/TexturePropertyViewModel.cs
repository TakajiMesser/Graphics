using SauceEditorCore.Models.Components;
using System.ComponentModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SauceEditor.ViewModels.Properties
{
    public class TexturePropertyViewModel : PropertyViewModel<TextureComponent>, IPropertyViewModel
    {
        public string ID { get; private set; }
        public string Name { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        public VectorProperty Position { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        public VectorProperty Rotation { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        public VectorProperty Scale { get; set; }

        public Color Color { get; set; }
    }
}