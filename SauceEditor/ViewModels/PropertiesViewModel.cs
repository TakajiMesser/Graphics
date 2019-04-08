using OpenTK;
using OpenTK.Graphics;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SauceEditor.Views;
using SauceEditor.Views.Properties;
using SpiceEngine.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Utilities;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SauceEditor.ViewModels
{
    public class PropertiesViewModel : ViewModel
    {
        private RelayCommand _openScriptCommand;

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

        private TransformPanelViewModel _positionViewModel;
        private TransformPanelViewModel _rotationViewModel;
        private TransformPanelViewModel _scaleViewModel;

        public RelayCommand OpenScriptCommand
        {
            get
            {
                if (_openScriptCommand == null)
                {
                    _openScriptCommand = new RelayCommand(
                        p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath))//,
                        //p => _behaviorFilePath != null
                    );
                }

                return _openScriptCommand;
            }
        }

        public EditorEntity EditorEntity { get; set; }

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

        public string ID
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
            set
            {
                if (EditorEntity != null)
                {
                    EditorEntity.Entity.Position = value;
                    EditorEntity.MapEntity.Position = value;
                }

                SetProperty(ref _position, value);
            }
        }

        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                if (EditorEntity != null)
                {
                    var rotator = EditorEntity.Entity as IRotate;
                    rotator.Rotation = Quaternion.FromEulerAngles(value.ToRadians());
                    EditorEntity.MapEntity.Rotation = value;
                }

                SetProperty(ref _rotation, value);
            }
        }

        public Vector3 Scale
        {
            get => _scale;
            set
            {
                if (EditorEntity != null)
                {
                    var scaler = EditorEntity.Entity as IScale;
                    scaler.Scale = value;
                    EditorEntity.MapEntity.Scale = value;
                }

                SetProperty(ref _scale, value);
            }
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

        public TransformPanelViewModel PositionViewModel
        {
            get => _positionViewModel;
            set
            {
                _positionViewModel = value;
                AddChild(_positionViewModel, (s, args) => Position = _positionViewModel.Transform);
            }
        }

        public TransformPanelViewModel RotationViewModel
        {
            get => _rotationViewModel;
            set
            {
                _rotationViewModel = value;
                AddChild(_rotationViewModel, (s, args) => Rotation = _rotationViewModel.Transform);
            }
        }

        public TransformPanelViewModel ScaleViewModel
        {
            get => _scaleViewModel;
            set
            {
                _scaleViewModel = value;
                AddChild(_scaleViewModel, (s, args) => Scale = _scaleViewModel.Transform);
            }
        }

        //public event EventHandler<EntityEventArgs> EntityUpdated;
        public event EventHandler<FileEventArgs> ScriptOpened;

        public void SetValues(EditorEntity editorEntity)
        {
            EditorEntity = editorEntity;

            EntitySelected = editorEntity != null ? Visibility.Visible : Visibility.Collapsed;
            EntityType = editorEntity != null ? editorEntity.Entity.GetType().Name : "No Properties to Show";
            ID = editorEntity != null ? editorEntity.Entity.ID.ToString() : "";

            if (editorEntity != null && editorEntity.MapEntity is MapActor mapActor)
            {
                Name = mapActor.Name;
                NameRowHeight = GridLength.Auto;
                ScriptRowHeight = GridLength.Auto;
                _behaviorFilePath = mapActor.Behavior.FilePath;

                // TODO - Determine smarter way to invoke this...
                //CommandManager.InvalidateRequerySuggested();
                //OpenScriptCommand.InvokeCanExecuteChanged();
            }
            else
            {
                Name = "";
                NameRowHeight = new GridLength(0);
                ScriptRowHeight = new GridLength(0);
                _behaviorFilePath = "";

                //CommandManager.InvalidateRequerySuggested();
                //OpenScriptCommand.InvokeCanExecuteChanged();
            }

            //Position = editorEntity != null ? editorEntity.MapEntity.Position : Vector3.Zero;
            //_positionViewModel.Transform = editorEntity != null ? editorEntity.MapEntity.Position : Vector3.Zero;
            _positionViewModel.UpdateTransform(editorEntity != null ? editorEntity.MapEntity.Position : Vector3.Zero);

            if (editorEntity != null && editorEntity.Entity is IRotate)
            {
                //Rotation = editorEntity.MapEntity.Rotation;
                _rotationViewModel.UpdateTransform(editorEntity.MapEntity.Rotation);
                RotationRowHeight = GridLength.Auto;
            }
            else
            {
                //Rotation = Vector3.Zero;
                _rotationViewModel.UpdateTransform(Vector3.Zero);
                RotationRowHeight = new GridLength(0);
            }

            if (editorEntity != null && editorEntity.Entity is IScale)
            {
                //Scale = editorEntity.MapEntity.Scale;
                _scaleViewModel.UpdateTransform(editorEntity.MapEntity.Scale);
                ScaleRowHeight = GridLength.Auto;
            }
            else
            {
                //Scale = Vector3.Zero;
                _scaleViewModel.UpdateTransform(Vector3.Zero);
                ScaleRowHeight = new GridLength(0);
            }

            if (editorEntity != null && editorEntity.MapEntity is MapLight mapLight)
            {
                Color = mapLight.Color.ToMediaColor();
                ColorRowHeight = GridLength.Auto;
            }
            else
            {
                Color = Color4.White.ToMediaColor();
                ColorRowHeight = new GridLength(0);
            }

            CommandManager.InvalidateRequerySuggested();
        }
    }
}