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

        private RelayCommand _translateCommand;
        private RelayCommand _rotateCommand;
        private RelayCommand _scaleCommand;

        public RelayCommand TranslateCommand
        {
            get
            {
                if (_translateCommand == null)
                {
                    _translateCommand = new RelayCommand(
                        p => TransformMode = TransformModes.Translate,
                        p => TransformMode != TransformModes.Translate
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
                        p => TransformMode = TransformModes.Rotate,
                        p => TransformMode != TransformModes.Rotate
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
                        p => TransformMode = TransformModes.Scale,
                        p => TransformMode != TransformModes.Scale
                    );
                }

                return _scaleCommand;
            }
        }

        public TransformModes TransformMode { get; set; }
        public ViewModels.ViewTypes ViewType { get; set; }

        public GamePanelViewModel PerspectiveViewModel { get; set; }
        public GamePanelViewModel XViewModel { get; set; }
        public GamePanelViewModel YViewModel { get; set; }
        public GamePanelViewModel ZViewModel { get; set; }

        public Resolution Resolution { get; set; }

        public Map Map => _mapManager.Map;

        public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;
        public event EventHandler<CommandEventArgs> CommandExecuted;

        public void OnTransformModeChanged()
        {
            PerspectiveViewModel.Panel.TransformMode = TransformMode;
            XViewModel.Panel.TransformMode = TransformMode;
            YViewModel.Panel.TransformMode = TransformMode;
            ZViewModel.Panel.TransformMode = TransformMode;

            CommandManager.InvalidateRequerySuggested();
        }

        public void OnViewTypeChanged()
        {

        }

        public void RequestUpdate()
        {
            PerspectiveViewModel.Panel.Invalidate();
            XViewModel.Panel.Invalidate();
            YViewModel.Panel.Invalidate();
            ZViewModel.Panel.Invalidate();
        }

        public void SetSelectedTool(SpiceEngine.Game.Tools tool)
        {
            PerspectiveViewModel.Panel.SelectedTool = tool;
            XViewModel.Panel.SelectedTool = tool;
            YViewModel.Panel.SelectedTool = tool;
            ZViewModel.Panel.SelectedTool = tool;
        }

        public void UpdateEntity(EditorEntity entity)
        {
            PerspectiveViewModel.Panel.UpdateEntities(entity.Entity.Yield());
            XViewModel.Panel.UpdateEntities(entity.Entity.Yield());
            YViewModel.Panel.UpdateEntities(entity.Entity.Yield());
            ZViewModel.Panel.UpdateEntities(entity.Entity.Yield());
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            PerspectiveViewModel.Panel.UpdateEntities(entities);
            XViewModel.Panel.UpdateEntities(entities);
            YViewModel.Panel.UpdateEntities(entities);
            ZViewModel.Panel.UpdateEntities(entities);
        }

        public void CenterView()
        {
            PerspectiveViewModel?.Panel.CenterView();
            XViewModel?.Panel.CenterView();
            YViewModel?.Panel.CenterView();
            ZViewModel?.Panel.CenterView();
        }
    }
}