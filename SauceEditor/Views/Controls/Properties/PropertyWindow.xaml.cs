using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Lights;
using System;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Brush = SpiceEngine.Entities.Brushes.Brush;

namespace SauceEditor.Views.Controls.Properties
{
    /// <summary>
    /// Interaction logic for PropertyWindow.xaml
    /// </summary>
    public partial class PropertyWindow : LayoutAnchorable
    {
        private IEntity _entity;
        public IEntity Entity
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

        public event EventHandler<EntityUpdatedEventArgs> EntityUpdated;
        public event EventHandler<CommandEventArgs> CommandExecuted;

        public PropertyWindow()
        {
            InitializeComponent();
            ClearProperties();

            PositionTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null)
                {
                    _entity.Position = args.Transform;
                    EntityUpdated?.Invoke(this, new EntityUpdatedEventArgs(_entity));
                }
            };

            RotationTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null && _entity is Actor actor)
                {
                    actor.OriginalRotation = args.Transform;
                }
                else if (_entity != null && _entity is Brush brush)
                {
                    brush.OriginalRotation = args.Transform;
                }

                EntityUpdated?.Invoke(this, new EntityUpdatedEventArgs(_entity));
            };

            ScaleTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null && _entity is Actor actor)
                {
                    actor.Scale = args.Transform;
                }
                else if (_entity != null && _entity is Brush brush)
                {
                    brush.Scale = args.Transform;
                }

                EntityUpdated?.Invoke(this, new EntityUpdatedEventArgs(_entity));
            };

            ColorPick.SelectedColorChanged += (s, args) =>
            {
                if (_entity != null && _entity is Light light)
                {
                    light.Color = args.NewValue.Value.ToVector4();
                }

                EntityUpdated?.Invoke(this, new EntityUpdatedEventArgs(_entity));
            };
        }

        private void SetProperties()
        {
            ClearProperties();
            EntityType.Content = _entity.GetType().Name;

            PropertyGrid.Visibility = Visibility.Visible;
            IDTextbox.Text = _entity.ID.ToString();

            PositionTransform.Visibility = Visibility.Visible;
            PositionTransform.SetValues(_entity.Position);

            if (_entity is Actor actor)
            {
                PropertyGrid.RowDefinitions[1].Height = GridLength.Auto;
                NameTextBox.Text = actor.Name;

                PropertyGrid.RowDefinitions[3].Height = GridLength.Auto;
                RotationTransform.Visibility = Visibility.Visible;
                RotationTransform.SetValues(actor.OriginalRotation);

                PropertyGrid.RowDefinitions[4].Height = GridLength.Auto;
                ScaleTransform.Visibility = Visibility.Visible;
                ScaleTransform.SetValues(actor.Scale);
            }

            if (_entity is Brush brush)
            {
                PropertyGrid.RowDefinitions[3].Height = GridLength.Auto;
                RotationTransform.Visibility = Visibility.Visible;
                RotationTransform.SetValues(brush.OriginalRotation);

                PropertyGrid.RowDefinitions[4].Height = GridLength.Auto;
                ScaleTransform.Visibility = Visibility.Visible;
                ScaleTransform.SetValues(brush.Scale);
            }

            if (_entity is Light light)
            {
                PropertyGrid.RowDefinitions[5].Height = GridLength.Auto;
                ColorPick.Visibility = Visibility.Visible;
                ColorPick.SelectedColor = light.Color.ToColor();
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
        }
    }
}
