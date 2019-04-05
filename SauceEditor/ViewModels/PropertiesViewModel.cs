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
        private readonly RelayCommand _openScriptCommand;

        private EditorEntity _editorEntity;

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

        private string _behaviorFilePath;

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

        public ICommand OpenScriptCommand => _openScriptCommand ?? new RelayCommand(
            p => ScriptOpened?.Invoke(this, new FileEventArgs(mapActor.Behavior.FilePath)),
            p => _behaviorFilePath != null
        );

        public event EventHandler<FileEventArgs> ScriptOpened;

        public PropertiesViewModel()
        {
            SetValues(null);
        }

        public void SetValues(EditorEntity editorEntity)
        {
            _editorEntity = editorEntity;

            EntitySelected = editorEntity != null ? Visibility.Visible : Visibility.Collapsed;
            EntityType = editorEntity != null ? editorEntity.Entity.GetType().Name : "No Properties to Show";
            ID = editorEntity != null ? editorEntity.Entity.ID : "";

            if (editorEntity != null && editorEntity.MapEntity is MapActor mapActor)
            {
                Name = mapActor.Name;
                NameRowHeight = GridLength.Auto;
                ScriptRowHeight = GridLength.Auto;
                _behaviorFilePath = mapActor.Behavior.FilePath;

                // TODO - Determine smarter way to invoke this...
                _openScriptCommand.InvokeCanExecuteChanged();
            }
            else
            {
                Name = "";
                NameRowHeight = new GridLength(0);
                ScriptRowHeight = new GridLength(0);
                _behaviorFilePath = mapActor.Behavior.FilePath;
                _openScriptCommand.InvokeCanExecuteChanged();
            }

            Position = editorEntity != null ? editorEntity.MapEntity.Position : Vector3.Zero;

            if (editorEntity != null && editorEntity.Entity is IRotate)
            {
                Rotation = editorEntity.MapEntity.Rotation;
                RotationRowHeight = GridLength.Auto;
            }
            else
            {
                Rotation = Vector3.Zero;
                RotationRowHeight = new GridLength(0);
            }

            if (editorEntity != null && editorEntity.Entity is IScale)
            {
                Scale = editorEntity.MapEntity.Scale;
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