using OpenTK;
using SauceEditor.Models;
using SauceEditor.Utilities;
using SauceEditor.ViewModels.Commands;
using SpiceEngine.Entities;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SauceEditor.ViewModels
{
    public class GamePanelManagerViewModel : ViewModel
    {
        private MapManager _mapManager;
        private GameManager _gameManager;
        private EntityMapping _entityMapping;

        private TransformModes _transformMode;

        private RelayCommand _translateCommand;
        private RelayCommand _rotateCommand;
        private RelayCommand _scaleCommand;

        private GamePanelViewModel _perspectiveViewModel;
        private GamePanelViewModel _xViewModel;
        private GamePanelViewModel _yViewModel;
        private GamePanelViewModel _zViewModel;

        public RelayCommand TranslateCommand
        {
            get
            {
                if (_translateCommand == null)
                {
                    _translateCommand = new RelayCommand(
                        p => SetTransformMode(TransformModes.Translate),
                        p => _transformMode != TransformModes.Translate
                    );
                }

                return _translateCommand;
            }
        }

        public RelayCommand RotateCommand
        {
            get
            {
                if (_rotateCommand == null)
                {
                    _rotateCommand = new RelayCommand(
                        p => SetTransformMode(TransformModes.Rotate),
                        p => _transformMode != TransformModes.Rotate
                    );
                }

                return _rotateCommand;
            }
        }

        public RelayCommand ScaleCommand
        {
            get
            {
                if (_scaleCommand == null)
                {
                    _scaleCommand = new RelayCommand(
                        p => SetTransformMode(TransformModes.Scale),
                        p => _transformMode != TransformModes.Scale
                    );
                }

                return _scaleCommand;
            }
        }

        public GamePanelViewModel PerspectiveViewModel
        {
            get => _perspectiveViewModel;
            set
            {
                _perspectiveViewModel = value;
                AddChild(_perspectiveViewModel, (s, args) => { });// Position = _positionViewModel.Transform);
            }
        }

        public GamePanelViewModel XViewModel
        {
            get => _xViewModel;
            set
            {
                _xViewModel = value;
                AddChild(_perspectiveViewModel, (s, args) => { });// Position = _positionViewModel.Transform);
            }
        }

        public GamePanelViewModel YViewModel
        {
            get => _yViewModel;
            set
            {
                _yViewModel = value;
                AddChild(_yViewModel, (s, args) => { });// Position = _positionViewModel.Transform);
            }
        }

        public GamePanelViewModel ZViewModel
        {
            get => _zViewModel;
            set
            {
                _zViewModel = value;
                AddChild(_zViewModel, (s, args) => { });// Position = _positionViewModel.Transform);
            }
        }

        public Resolution Resolution { get; set; }
        /*public TransformModes TransformMode
        {
            get => _transformMode;
            set
            {
                _transformMode = value;

                switch (_transformMode)
                {
                    case TransformModes.Translate:
                        TranslateButton.IsEnabled = false;
                        RotateButton.IsEnabled = true;
                        ScaleButton.IsEnabled = true;
                        break;
                    case TransformModes.Rotate:
                        TranslateButton.IsEnabled = true;
                        RotateButton.IsEnabled = false;
                        ScaleButton.IsEnabled = true;
                        break;
                    case TransformModes.Scale:
                        TranslateButton.IsEnabled = true;
                        RotateButton.IsEnabled = true;
                        ScaleButton.IsEnabled = false;
                        break;
                }

                _perspectiveView.Panel.TransformMode = value;
                _perspectiveView.Panel.Invalidate();

                _xView.Panel.TransformMode = value;
                _xView.Panel.Invalidate();

                _yView.Panel.TransformMode = value;
                _yView.Panel.Invalidate();

                _zView.Panel.TransformMode = value;
                _zView.Panel.Invalidate();
            }
        }*/

        public Map Map => _mapManager.Map;

        public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        public event EventHandler<CommandEventArgs> CommandExecuted;

        private void SetTransformMode(TransformModes transformMode)
        {
            _transformMode = transformMode;

            _perspectiveViewModel.Panel.TransformMode = value;
            _perspectiveViewModel.Panel.Invalidate();

            _xViewModel.Panel.TransformMode = value;
            _xViewModel.Panel.Invalidate();

            _yViewModel.Panel.TransformMode = value;
            _yViewModel.Panel.Invalidate();

            _zViewModel.Panel.TransformMode = value;
            _zViewModel.Panel.Invalidate();

            CommandManager.InvalidateRequerySuggested();
        }
    }
}