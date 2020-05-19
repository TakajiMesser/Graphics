using SauceEditorCore.Models.Components;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SauceEditor.ViewModels.Properties
{
    public class MaterialPropertyViewModel : PropertyViewModel<MaterialComponent>, IPropertyViewModel
    {
        public string Name { get; set; }

        [ExpandableObject]
        public VectorProperty Ambient { get; set; }

        [ExpandableObject]
        public VectorProperty Diffuse { get; set; }

        [ExpandableObject]
        public VectorProperty Specular { get; set; }

        public float SpecularExponent { get; set; }

        /*public void OnPositionChanged()
        {
            AddChild(Position, (s, args) =>
            {
                if (_editorEntity != null)
                {
                    _editorEntity.Entity.Position = Position.ToVector3();
                    _editorEntity.MapEntity.Position = Position.ToVector3();
                    InvokePropertyChanged(nameof(Position));
                }
            });
        }

        public void OnRotationChanged()
        {
            AddChild(Rotation, (s, args) =>
            {
                if (_editorEntity != null && _editorEntity.Entity is IRotate rotator)
                {
                    rotator.Rotation = Quaternion.FromEulerAngles(Rotation.ToVector3().ToRadians());
                    _editorEntity.MapEntity.Rotation = Rotation.ToVector3();
                    InvokePropertyChanged(nameof(Rotation));
                }
            });
        }

        public void OnScaleChanged()
        {
            AddChild(Scale, (s, args) =>
            {
                if (_editorEntity != null && _editorEntity.Entity is IScale scaler)
                {
                    scaler.Scale = Scale.ToVector3();
                    _editorEntity.MapEntity.Scale = Scale.ToVector3();
                    InvokePropertyChanged(nameof(Scale));
                }
            });
        }

        public void UpdateFromModel(Material material)
        {
            _material = material;
            if (material == null) return; // Yikes
            
            if (editorEntity.MapEntity is MapActor mapActor)
            {
                Name = mapActor.Name;
            }

            Position = new VectorProperty(editorEntity.MapEntity.Position);

            if (editorEntity.Entity is IRotate)
            {
                Rotation = new VectorProperty(editorEntity.MapEntity.Rotation);
            }

            if (editorEntity.Entity is IScale)
            {
                Scale = new VectorProperty(editorEntity.MapEntity.Scale);
            }

            if (editorEntity.MapEntity is MapLight mapLight)
            {
                Color = mapLight.Color.ToMediaColor();
            }
        }*/
    }
}