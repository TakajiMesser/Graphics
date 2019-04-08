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

        private GamePanelViewModel _perspectiveViewModel;
        private GamePanelViewModel _xViewModel;
        private GamePanelViewModel _yViewModel;
        private GamePanelViewModel _zViewModel;

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
    }
}