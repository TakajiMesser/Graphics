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
    /// Interaction logic for PropertyPanel.xaml
    /// </summary>
    public partial class PropertyPanel : DockingLibrary.DockableContent
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
                    Title.Content = _entity.ID;
                    if (_entity is Actor actor)
                    {
                        Title.Content += " " + actor.Name;
                    }

                    PositionTransform.SetValues(_entity.Position);
                    //RotationTransform.SetValues(_entity.Rotation.)
                    ScaleTransform.SetValues(_entity.Scale);
                }
                else
                {
                    Title.Content = "";

                    PositionTransform.IsEnabled = false;
                    RotationTransform.IsEnabled = false;
                    ScaleTransform.IsEnabled = false;
                }
            }
        }

        public event EventHandler<TransformChangedEventArgs> TransformChanged;

        public PropertyPanel()
        {
            InitializeComponent();

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
                    _entity.Rotation = Quaternion.FromEulerAngles(args.Transform);
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
    }
}
