using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Utilities;
using System.ComponentModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SauceEditor.ViewModels.Properties
{
    public class EntityProperties : ViewModel
    {
        public string ID { get; private set; }
        public string Name { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        public TransformProperty Position { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        public TransformProperty Rotation { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        public TransformProperty Scale { get; set; }

        public Color Color { get; set; }

        private EditorEntity _editorEntity;

        public void OnPositionChanged()
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

        public void UpdateFromModel(EditorEntity editorEntity)
        {
            _editorEntity = editorEntity;
            if (editorEntity == null) return; // Yikes

            ID = editorEntity.Entity.ID.ToString();
            
            if (editorEntity.MapEntity is MapActor mapActor)
            {
                Name = mapActor.Name;
            }

            Position = new TransformProperty(editorEntity.MapEntity.Position);

            if (editorEntity.Entity is IRotate)
            {
                Rotation = new TransformProperty(editorEntity.MapEntity.Rotation);
            }

            if (editorEntity.Entity is IScale)
            {
                Scale = new TransformProperty(editorEntity.MapEntity.Scale);
            }

            if (editorEntity.MapEntity is MapLight mapLight)
            {
                Color = mapLight.Color.ToMediaColor();
            }
        }
    }
}