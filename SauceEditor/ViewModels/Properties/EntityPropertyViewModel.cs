using OpenTK;
using PropertyChanged;
using SauceEditor.Utilities;
using SauceEditor.Views.Custom;
using SauceEditorCore.Models.Entities;
using SpiceEngine.Maps;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Utilities;
using System.ComponentModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SauceEditor.ViewModels.Properties
{
    public class EntityPropertyViewModel : PropertyViewModel<EditorEntity>, IPropertyViewModel
    {
        public string ID { get; private set; }

        [HideIfNullProperty]
        public string Name { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        [PropagateChanges]
        [DoNotCheckEquality]
        public VectorProperty Position { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        [PropagateChanges]
        [DoNotCheckEquality]
        [HideIfNullProperty]
        public VectorProperty Rotation { get; set; }

        [Category("Transforms")]
        [ExpandableObject]
        [PropagateChanges]
        [DoNotCheckEquality]
        [HideIfNullProperty]
        public VectorProperty Scale { get; set; }

        [HideIfNullProperty]
        public Color? Color { get; set; }

        public void OnPositionChanged()
        {
            if (Model != null)
            {
                Model.Entity.Position = Position.ToVector3();
                Model.MapEntity.Position = Position.ToVector3();
            }
        }

        public void OnRotationChanged()
        {
            if (Model != null && Model.Entity is IRotate rotator)
            {
                rotator.Rotation = Quaternion.FromEulerAngles(Rotation.ToVector3().ToRadians());

                if (Model.MapEntity is IMapEntity3D mapEntity)
                {
                    mapEntity.Rotation = Rotation.ToVector3();
                }
            }
        }

        public void OnScaleChanged()
        {
            if (Model != null && Model.Entity is IScale scaler)
            {
                scaler.Scale = Scale.ToVector3();

                if (Model.MapEntity is IMapEntity3D mapEntity)
                {
                    mapEntity.Scale = Scale.ToVector3();
                }
            }
        }

        protected override void UpdatePropertiesFromModel(EditorEntity editorEntity)
        {
            if (editorEntity == null) return; // Yikes

            ID = editorEntity.Entity.ID.ToString();

            if (editorEntity.MapEntity is MapActor mapActor)
            {
                Name = mapActor.Name;
            }
            else
            {
                Name = null;
            }

            Position = new VectorProperty(editorEntity.MapEntity.Position);

            if (editorEntity.MapEntity is IMapEntity3D mapEntity)
            {
                if (editorEntity.Entity is IRotate)
                {
                    Rotation = new VectorProperty(mapEntity.Rotation);
                }

                if (editorEntity.Entity is IScale)
                {
                    Scale = new VectorProperty(mapEntity.Scale);
                }
            }

            if (editorEntity.MapEntity is MapLight mapLight)
            {
                Color = mapLight.Color.ToMediaColor();
            }
            else
            {
                Color = null;
            }
        }
    }
}