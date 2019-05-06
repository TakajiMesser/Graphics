using SauceEditor.Helpers.Builders;
using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditor.ViewModels.Behaviors;
using SauceEditor.ViewModels.Commands;
using SauceEditor.ViewModels.Properties;
using SauceEditor.ViewModels.Trees;
using SauceEditor.Views.Factories;
using SpiceEngine.Maps;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock;
using ICommand = SauceEditor.ViewModels.Commands.ICommand;

namespace SauceEditor.ViewModels
{
    public class MainWindowViewModel : ViewModel, IComponentFactory
    {
        public IWindowFactory WindowFactory { get; set; }
        public IMainViewFactory MainViewFactory { get; set; }

        public CommandStack CommandStack { get; private set; } = new CommandStack();

        public DockingManager MainDock { get; set; }
        public DockingManager SideDock { get; set; }

        public List<ViewModel> MainDockViewModels { get; set; } = new List<ViewModel>();
        public List<ViewModel> SideDockViewModels { get; set; } = new List<ViewModel>();

        public string Title { get; set; }
        public bool IsPlayable { get; set; }
        public Visibility PlayVisibility { get; set; }

        public IMainDockViewModel CurrentMainDockViewModel { get; set; }

        public ProjectTreePanelViewModel ProjectTreePanelViewModel { get; set; }
        public ToolsPanelViewModel ToolsPanelViewModel { get; set; }
        public PropertyViewModel PropertyViewModel { get; set; }

        public GamePanelManagerViewModel GamePanelManagerViewModel { get; set; }
        public BehaviorViewModel BehaviorViewModel { get; set; }
        public ScriptViewModel ScriptViewModel { get; set; }

        public SettingsWindowViewModel SettingsWindowViewModel { get; set; }

        public EditorSettings Settings { get; set; }

        private RelayCommand _undoCommand;
        public RelayCommand UndoCommand
        {
            get
            {
                return _undoCommand ?? (_undoCommand = new RelayCommand(
                    p => CommandStack.Undo(),
                    p => CommandStack.CanUndo
                ));
            }
        }

        private RelayCommand _redoCommand;
        public RelayCommand RedoCommand
        {
            get
            {
                return _redoCommand ?? (_redoCommand = new RelayCommand(
                    p => CommandStack.Redo(),
                    p => CommandStack.CanRedo
                ));
            }
        }

        private RelayCommand _playCommand;
        public RelayCommand PlayCommand
        {
            get
            {
                return _playCommand ?? (_playCommand = new RelayCommand(
                    p => WindowFactory.CreateGameWindow(CurrentMainDockViewModel.Map),
                    p => CurrentMainDockViewModel != null ? CurrentMainDockViewModel.IsPlayable : false
                ));
            }
        }

        public MainWindowViewModel()
        {
            Title = "Sauce Editor";

            LoadOrCreateSettings();
            MapBuilder.CreateTestProject();
        }

        public void OnGamePanelManagerViewModelChanged()
        {
            AddChild(GamePanelManagerViewModel, (s, args) =>
            {
                // We are "listening" to IsActive changing here
                if (args.PropertyName == "IsActive" && GamePanelManagerViewModel.IsActive)
                {
                    // We want to set the property panel's view model here
                    // The issue is that we STILL need a way to tell what "type" of map we have loaded, and we aren't storing that anywhere...
                    PropertyViewModel.Properties = new EntityPropertyViewModel();
                }
                else if (args.PropertyName == "SelectedEntities")
                {
                    // Need to check first if this is an entity map...
                    PropertyViewModel.UpdateFromModel(GamePanelManagerViewModel.SelectedEntities.LastOrDefault());
                    PropertyViewModel.IsActive = true;
                }
            });
        }

        public void OnPropertyViewModelChanged() => AddChild(PropertyViewModel, (s, args) => GamePanelManagerViewModel?.RequestUpdate());

        /*public ICommand EntityPropertiesUpdatedCommand => _entityPropertiesUpdatedCommand ?? new RelayCommand(
            p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath)),
            p => _behaviorFilePath != null
        );*/

        private void LoadOrCreateSettings()
        {
            if (File.Exists(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH))
            {
                Settings = EditorSettings.Load(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            }
            else
            {
                Settings = new EditorSettings();
                Settings.Save(SauceEditor.Helpers.FilePathHelper.SETTINGS_PATH);
            }
        }

        private void CommandExecuted(ICommand command)
        {
            CommandStack.Push(command);

            // TODO - Also need to enable/disable menu items for undo/redo
            /*if (CommandStack.CanUndo)
            {
                UndoButton.IsEnabled = true;
            }
            else
            {
                UndoButton.IsEnabled = false;
            }

            if (CommandStack.CanRedo)
            {
                RedoButton.IsEnabled = true;
            }
            else
            {
                RedoButton.IsEnabled = false;
            }*/
        }

        public void CreateProject() => throw new System.NotImplementedException();
        public void CreateMap() => throw new System.NotImplementedException();
        public void CreateModel() => throw new System.NotImplementedException();
        public void CreateBehavior() => throw new System.NotImplementedException();
        public void CreateTexture() => throw new System.NotImplementedException();
        public void CreateSound() => throw new System.NotImplementedException();
        public void CreateMaterial() => throw new System.NotImplementedException();
        public void CreateArchetype() => throw new System.NotImplementedException();
        public void CreateScript() => throw new System.NotImplementedException();

        public void OpenProject(string filePath)
        {
            var project = Project.Load(filePath);
            ProjectTreePanelViewModel.UpdateFromModel(project, this);
            //_projectTree.OpenProject(filePath);
            //_projectTree.IsActive = true;
            //SideDockManager.ActiveContent = _projectTree;

            Title = Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";
        }

        public void OpenMap(string filePath)
        {
            var mapComponent = new MapComponent()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Path = filePath,
                Map = Map.Load(filePath)
            };

            GamePanelManagerViewModel = (GamePanelManagerViewModel)MainViewFactory.CreateGamePanelManager(mapComponent);
            GamePanelManagerViewModel.Component = mapComponent;
            //_propertyPanel.EntityUpdated += (s, args) => _gamePanelManager.ViewModel.UpdateEntity(args.Entity);
            /*_propertyPanel.ScriptOpened += (s, args) =>
            {
                if (_propertyPanel.ViewModel.EditorEntity != null && _propertyPanel.ViewModel.EditorEntity.Entity is Actor actor && _propertyPanel.ViewModel.EditorEntity.MapEntity is MapActor mapActor)
                {
                    /*_scriptView = new ScriptView(filePath, actor, mapActor);
                    _scriptView.Saved += (sender, e) =>
                    {
                        //mapActor.Behavior.e.Script;
                    };*

                    OpenBehavior(args.FileName);
                }
                else
                {
                    // TODO - Throw some error to the user here?
                }
            };*/
        }

        public void OpenModel(string filePath)
        {
            Title = Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            var modelComponent = new ModelComponent()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Path = filePath
            };

            var mapComponent = MapBuilder.GenerateModelMap(modelComponent);

            GamePanelManagerViewModel = (GamePanelManagerViewModel)MainViewFactory.CreateGamePanelManager(mapComponent);
            GamePanelManagerViewModel.ViewType = ViewTypes.Perspective;
            GamePanelManagerViewModel.Component = modelComponent;
        }

        public void OpenBehavior(string filePath)
        {
            Title = Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            var behaviorComponent = new BehaviorComponent()
            {
                Name = "Test Behavior",
                Path = filePath
            };

            BehaviorViewModel = (BehaviorViewModel)MainViewFactory.CreateBehaviorView(behaviorComponent);
        }

        public void OpenTexture(string filePath)
        {
            Title = Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            var textureComponent = new TextureComponent()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Path = filePath,
                TexturePaths = new SpiceEngine.Rendering.Textures.TexturePaths()
                {
                    DiffuseMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                    NormalMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_N_NORMAL_PATH,
                    SpecularMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_S_TEXTURE_PATH,
                    ParallaxMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_H_TEXTURE_PATH
                }
            };

            var mapComponent = MapBuilder.GenerateTextureMap(textureComponent);

            GamePanelManagerViewModel = (GamePanelManagerViewModel)MainViewFactory.CreateGamePanelManager(mapComponent);
            GamePanelManagerViewModel.ViewType = ViewTypes.Perspective;
            GamePanelManagerViewModel.Component = textureComponent;
            GamePanelManagerViewModel.PerspectiveViewModel.Panel.RenderMode = SpiceEngine.Rendering.RenderModes.Diffuse;

            CommandManager.InvalidateRequerySuggested();
        }

        public void OpenSound(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public void OpenMaterial(string filePath)
        {
            Title = Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            var materialComponent = new MaterialComponent()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Path = filePath
            };

            var mapComponent = MapBuilder.GenerateMaterialMap(materialComponent);

            GamePanelManagerViewModel = (GamePanelManagerViewModel)MainViewFactory.CreateGamePanelManager(mapComponent);
            GamePanelManagerViewModel.ViewType = ViewTypes.Perspective;
            GamePanelManagerViewModel.Component = materialComponent;
        }

        public void OpenArchetype(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public void OpenScript(string filePath)
        {
            Title = Path.GetFileNameWithoutExtension(filePath) + " - " + "SauceEditor";

            var scriptComponent = new ScriptComponent()
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Path = filePath
            };

            ScriptViewModel = (ScriptViewModel)MainViewFactory.CreateScriptView(scriptComponent);
        }
    }
}