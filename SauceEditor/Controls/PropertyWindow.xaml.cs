using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TakoEngine.Entities;
using Brush = TakoEngine.Entities.Brush;

namespace SauceEditor.Controls
{
    /// <summary>
    /// Interaction logic for PropertyWindow.xaml
    /// </summary>
    public partial class PropertyWindow : DockingLibrary.DockableContent
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

        public event EventHandler<TransformChangedEventArgs> TransformChanged;

        public PropertyWindow()
        {
            InitializeComponent();
            ClearProperties();

            PositionTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null)
                {
                    _entity.Position = args.Transform;
                }
                
                TransformChanged?.Invoke(this, args);
            };

            RotationTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null)
                {
                    _entity.OriginalRotation = args.Transform;
                }

                TransformChanged?.Invoke(this, args);
            };

            ScaleTransform.TransformChanged += (s, args) =>
            {
                if (_entity != null)
                {
                    _entity.Scale = args.Transform;
                }

                TransformChanged?.Invoke(this, args);
            };
        }

        private void SetProperties()
        {
            ClearProperties();
            EntityType.Content = _entity.GetType().Name;

            PropertyGrid.Visibility = Visibility.Visible;
            IDTextbox.Text = _entity.ID.ToString();

            if (_entity is Actor actor)
            {
                PropertyGrid.RowDefinitions[1].Height = GridLength.Auto;
                NameTextBox.Text = actor.Name;
            }

            PositionTransform.Visibility = Visibility.Visible;
            PositionTransform.SetValues(_entity.Position);

            if (_entity is Actor || _entity is Brush)
            {
                RotationTransform.Visibility = Visibility.Visible;
                RotationTransform.SetValues(_entity.OriginalRotation);

                ScaleTransform.Visibility = Visibility.Visible;
                ScaleTransform.SetValues(_entity.Scale);
            }
        }

        private void ClearProperties()
        {
            EntityType.Content = "No Properties to Show";

            PropertyGrid.Visibility = Visibility.Hidden;

            PropertyGrid.RowDefinitions[1].Height = new GridLength(0);

            PositionTransform.Visibility = Visibility.Hidden;
            RotationTransform.Visibility = Visibility.Hidden;
            ScaleTransform.Visibility = Visibility.Collapsed;
        }
    }
}
