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
        private string _id;
        private string _name;
        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;
        private Color _color;
        
        private GridLength _nameRowHeight;
        private GridLength _rotationRowHeight;
        private GridLength _scaleRowHeight;
        private GridLength _colorRowHeight;
        private GridLength _scriptRowHeight;

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

        public Vector3 Rotation
        {
            get => _rotation;
            set => SetProperty(ref _rotation, value);
        }

        public Vector3 Scale
        {
            get => _scale;
            set => SetProperty(ref _scale, value);
        }

        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public GridLength NameRowHeight
        {
            get => _nameRowHeight;
            set => SetProperty(ref _nameRowHeight, value);
        }

        public GridLength RotationRowHeight
        {
            get => _rotationRowHeight;
            set => SetProperty(ref _rotationRowHeight, value);
        }

        public GridLength ScaleRowHeight
        {
            get => _scaleRowHeight;
            set => SetProperty(ref _scaleRowHeight, value);
        }

        public GridLength ColorRowHeight
        {
            get => _colorRowHeight;
            set => SetProperty(ref _colorRowHeight, value);
        }

        public GridLength ScriptRowHeight
        {
            get => _scriptRowHeight;
            set => SetProperty(ref _scriptRowHeight, value);
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
            ID = editorEntity != null ? editorEntity.Entity.ID : "";

            if (editorEntity != null && editorEntity.MapEntity is MapActor mapActor)
            {
                Name = mapActor.Name;
                NameRowHeight = GridLength.Auto;
                ScriptRowHeight = GridLength.Auto;
            }
            else
            {
                Name = "";
                NameRowHeight = new GridLength(0);
                ScriptRowHeight = new GridLength(0);
            }

            Position = editorEntity != null ? editorEntity.MapEntity.Position : Vector3.Zero;

            if (editorEntity != null && editorEntity.MapEntity is IRotate rotator)
            {
                Rotation = rotator.Rotation;
                RotationRowHeight = GridLength.Auto;
            }
            else
            {
                Rotation = Vector3.Zero;
                RotationRowHeight = new GridLength(0);
            }

            if (editorEntity != null && editorEntity.MapEntity is IScale scaler)
            {
                Scale = scaler.Scale;
                ScaleRowHeight = GridLength.Auto;
            }
            else
            {
                Scale = Vector3.Zero;
                ScaleRowHeight = new GridLength(0);
            }

            if (editorEntity != null && editorEntity.MapEntity is MapLight mapLight)
            {
                Color = mapLight.Color.ToMediaColor();
                ColorRowHeight = GridLength.Auto;
            }
            else
            {
                Color = Color.White;
                ColorRowHeight = new GridLength(0);
            }
        }
    }
}