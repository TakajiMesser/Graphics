using SauceEditor.ViewModels.Properties;
using SauceEditorCore.Models.Entities;
using System;
using System.Reflection;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit.PropertyGrid;
using System.Linq;

namespace SauceEditor.Views.Properties
{
    /// <summary>
    /// Interaction logic for PropertyPanel.xaml
    /// </summary>
    public partial class PropertyPanel : LayoutAnchorable, IDisplayProperties
    {
        /*private EditorEntity _entity;
        public EditorEntity Entity
        {
            get => _entity;
            set
            {
                _entity = value;

                if (_entity != null)
                {
                    SetProperties();
                }
                else
                {
                    ClearProperties();
                }
            }
        }*/

        //public event EventHandler<EntityEventArgs> EntityUpdated;
        //public event EventHandler<CommandEventArgs> CommandExecuted;
        public event EventHandler<FileEventArgs> ScriptOpened;

        public PropertyPanel()
        {
            InitializeComponent();

            ViewModel.PropertyDisplayer = this;
            ViewModel.UpdateFromModel(null);

            /*ViewModel.PositionViewModel = PositionTransform.ViewModel;
            ViewModel.RotationViewModel = RotationTransform.ViewModel;
            ViewModel.ScaleViewModel = ScaleTransform.ViewModel;

            ViewModel.ScriptOpened += (s, args) =>
            {
                ScriptOpened?.Invoke(s, args);
            };
            PositionTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null)
                {
                    _entity.Entity.Position = args.Transform;
                    _entity.MapEntity.Position = args.Transform;

                    EntityUpdated?.Invoke(this, new EntityEventArgs(_entity));
                }
            };

            RotationTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null)
                {
                    if (_entity.Entity is IRotate rotator)
                    {
                        //rotator.Rotation = Quaternion.FromEulerAngles(args.Transform);
                    }

                    _entity.MapEntity.Rotation = args.Transform;
                    EntityUpdated?.Invoke(this, new EntityEventArgs(_entity));
                }
            };

            ScaleTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null)
                {
                    if (_entity is IScale scaler)
                    {
                        scaler.Scale = args.Transform;
                    }

                    _entity.MapEntity.Scale = args.Transform;
                    EntityUpdated?.Invoke(this, new EntityEventArgs(_entity));
                }
            };

            ColorPick.SelectedColorChanged += (s, args) =>
            {
                if (_entity != null)
                {
                    if (_entity.Entity is ILight light)
                    {
                        light.Color = args.NewValue.Value.ToVector4();
                    }

                    if (_entity.MapEntity is MapLight mapLight)
                    {
                        mapLight.Color = args.NewValue.Value.ToColor4();
                    }

                    EntityUpdated?.Invoke(this, new EntityEventArgs(_entity));
                }
            };*/
        }

        public void UpdateFromEntity(EditorEntity entity)
        {
            if (ViewModel.Properties is EntityPropertyViewModel entityPropertyViewModel)
            {
                entityPropertyViewModel.UpdateFromModel(entity);
                ViewModel.IsVisible = entity != null;

                if (entity != null)
                {
                    ViewModel.IsActive = true;
                }
            }
        }

        public void SetPropertyVisibility(string propertyName, Visibility visibility)
        {
            foreach (var property in PropertyGrid.Properties)
            {
                if (property is PropertyItem propertyItem && propertyItem.PropertyName == propertyName)
                {
                    propertyItem.Visibility = visibility;
                }
            }
        }

        /*private void ScriptButton_Click(object sender, RoutedEventArgs e)
        {
            if (PropertiesViewModel.)
            {
                ScriptOpened?.Invoke(this, new FileEventArgs(mapActor.Behavior.FilePath));
            }
        }*/
    }
}
