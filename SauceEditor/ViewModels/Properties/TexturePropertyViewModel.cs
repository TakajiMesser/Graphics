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
    public class TexturePropertyViewModel : ViewModel, IPropertyViewModel<TextureComponent>
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

        private TextureComponent _textureComponent;

        public void UpdateFromModel(TextureComponent textureComponent)
        {
            _textureComponent = textureComponent;
        }
    }
}