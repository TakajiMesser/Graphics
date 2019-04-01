using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SauceEditor.ViewModels.Commands;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Maps;
using System;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Controls.Properties
{
    /// <summary>
    /// Interaction logic for PropertyPanel.xaml
    /// </summary>
    public partial class PropertyPanel : LayoutAnchorable
    {
        private EditorEntity _entity;
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
        }

        public event EventHandler<EntityEventArgs> EntityUpdated;
        public event EventHandler<CommandEventArgs> CommandExecuted;
        public event EventHandler<FileEventArgs> ScriptOpened;

        public PropertyPanel()
        {
            InitializeComponent();
            ClearProperties();

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
            };
        }

        private void SetProperties()
        {
            ClearProperties();
            EntityType.Content = _entity.Entity.GetType().Name;

            PropertyGrid.Visibility = Visibility.Visible;
            //IDTextbox.Text = _mapEntity.ID.ToString();

            PositionTransform.Visibility = Visibility.Visible;
            PositionTransform.SetValues(_entity.MapEntity.Position);

            switch (_entity.MapEntity)
            {
                case MapActor mapActor:
                    PropertyGrid.RowDefinitions[1].Height = GridLength.Auto;
                    NameTextBox.Text = mapActor.Name;

                    PropertyGrid.RowDefinitions[3].Height = GridLength.Auto;
                    RotationTransform.Visibility = Visibility.Visible;
                    RotationTransform.SetValues(mapActor.Rotation);

                    PropertyGrid.RowDefinitions[4].Height = GridLength.Auto;
                    ScaleTransform.Visibility = Visibility.Visible;
                    ScaleTransform.SetValues(mapActor.Scale);

                    PropertyGrid.RowDefinitions[6].Height = GridLength.Auto;
                    ScriptButton.Visibility = Visibility.Visible;
                    break;
                case MapBrush mapBrush:
                    PropertyGrid.RowDefinitions[3].Height = GridLength.Auto;
                    RotationTransform.Visibility = Visibility.Visible;
                    RotationTransform.SetValues(mapBrush.Rotation);

                    PropertyGrid.RowDefinitions[4].Height = GridLength.Auto;
                    ScaleTransform.Visibility = Visibility.Visible;
                    ScaleTransform.SetValues(mapBrush.Scale);
                    break;
                case MapLight mapLight:
                    PropertyGrid.RowDefinitions[5].Height = GridLength.Auto;
                    ColorPick.Visibility = Visibility.Visible;
                    ColorPick.SelectedColor = mapLight.Color.ToMediaColor();
                    break;
            }
        }

        private void ClearProperties()
        {
            EntityType.Content = "No Properties to Show";

            PropertyGrid.Visibility = Visibility.Hidden;

            PropertyGrid.RowDefinitions[1].Height = new GridLength(0);

            PositionTransform.Visibility = Visibility.Hidden;

            PropertyGrid.RowDefinitions[3].Height = new GridLength(0);
            RotationTransform.Visibility = Visibility.Hidden;

            PropertyGrid.RowDefinitions[4].Height = new GridLength(0);
            ScaleTransform.Visibility = Visibility.Hidden;

            PropertyGrid.RowDefinitions[5].Height = new GridLength(0);
            ColorPick.Visibility = Visibility.Hidden;

            PropertyGrid.RowDefinitions[6].Height = new GridLength(0);
            ScriptButton.Visibility = Visibility.Hidden;
        }

        private void ScriptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_entity != null && _entity.MapEntity is MapActor mapActor)
            {
                ScriptOpened?.Invoke(this, new FileEventArgs(mapActor.Behavior.FilePath));
            }
        }
    }
}
