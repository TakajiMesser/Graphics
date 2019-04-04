using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SauceEditor.ViewModels
{
    public class PropertiesViewModel : ViewModel
    {
        private readonly DelegateCommand _openScriptCommand;

        private Visibility _entitySelected;
        private string _entityType;
        private int _id;
        private string _name;
        private Vector3 _position;
        private Vector3? _rotation;
        private Vector3? _scale;
        private Color? _color;

        public Visibility EntitySelected
        {
            get => _entitySelected;
            set => SetProperty(ref _entitySelected, value);
        }

        public string EntityType
        {
            get => _entityType;
            set => SetProperty(ref _entityType, value);
        }

        public int ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Vector3 Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public Vector3? Rotation
        {
            get => _rotation;
            set => SetProperty(ref _rotation, value);
        }

        public Vector3? Scale
        {
            get => _scale;
            set => SetProperty(ref _scale, value);
        }

        public Color? Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public ICommand OpenScriptCommand => _openScriptCommand;

        public PropertiesViewModel()
        {
            SetValues(null);

            _openScriptCommand = new DelegateCommand(
                p =>
                {
                    _openScriptCommand.InvokeCanExecuteChanged();
                },
                p =>
                {
                    return true;
                }
            );
        }

        public void SetValues(EditorEntity editorEntity)
        {
            EntitySelected = editorEntity != null ? Visibility.Visible : Visibility.Collapsed;
            EntityType = editorEntity != null ? editorEntity.Entity.GetType().Name : "No Properties to Show";

            if (editorEntity != null)
            {
                Position = editorEntity.MapEntity.Position;

                if (editorEntity.Entity is IRotate)
                {
                    Rotation = editorEntity.MapEntity.Rotation;
                }

                if (editorEntity.Entity is IScale)
                {
                    Scale = editorEntity.MapEntity.Scale;
                }

                if (editorEntity.MapEntity is MapActor mapActor)
                {
                    Name = mapActor.Name;
                }

                if (editorEntity.MapEntity is MapLight mapLight)
                {
                    Color = mapLight.Color.ToMediaColor();
                }
            }
            else
            {
                Position = Vector3.Zero;
                Rotation = null;
                Scale = null;
                Name = null;
                Color = null;
            }
        }
    }
}