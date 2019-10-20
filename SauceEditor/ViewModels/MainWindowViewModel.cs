using SauceEditor.Helpers.Builders;
using SauceEditor.Models;
using SauceEditor.Models.Components;
using SauceEditor.ViewModels.Behaviors;
using SauceEditor.ViewModels.Commands;
using SauceEditor.ViewModels.Properties;
using SauceEditor.ViewModels.Tools;
using SauceEditor.ViewModels.Trees.Entities;
using SauceEditor.ViewModels.Trees.Projects;
using SauceEditor.Views;
using SauceEditor.Views.Factories;
using SauceEditorCore.Models.Components;
using SpiceEngineCore.Utilities;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ICommand = SauceEditor.ViewModels.Commands.ICommand;
using LibraryPanelViewModel = SauceEditor.ViewModels.Libraries.LibraryPanelViewModel;

namespace SauceEditor.ViewModels
{
    public class MainWindowViewModel : ViewModel, IComponentFactory, IEntityFactory
    {
        public IWindowFactory WindowFactory { get; set; }
        //public IMainViewFactory MainViewFactory { get; set; }
        public IGameDockFactory GameDockFactory { get; set; }

        public CommandStack CommandStack { get; private set; } = new CommandStack();

        /*public DockingManager MainDock { get; set; }
        public DockingManager SideDock { get; set; }

        public List<MainDockViewModel> MainDockViewModels { get; set; } = new List<MainDockViewModel>();
        public List<ViewModel> SideDockViewModels { get; set; } = new List<ViewModel>();*/

        public IDockTracker DockTracker { get; set; }

        public string Title { get; set; }
        public bool IsPlayable { get; set; }
        public Visibility PlayVisibility { get; set; }

        public ProjectTreePanelViewModel ProjectTreePanelViewModel { get; set; }
        public LibraryPanelViewModel LibraryPanelViewModel { get; set; }
        public PropertyViewModel PropertyViewModel { get; set; }
        public EntityTreePanelViewModel EntityTreePanelViewModel { get; set; }

        public ToolsPanelViewModel ToolsPanelViewModel { get; set; }
        public ModelToolPanelViewModel ModelToolPanelViewModel { get; set; }
        public BrushToolPanelViewModel BrushToolPanelViewModel { get; set; }

        public GamePanelManagerViewModel GamePanelManagerViewModel { get; set; }
        public BehaviorViewModel BehaviorViewModel { get; set; }
        public ScriptViewModel ScriptViewModel { get; set; }

        public SettingsWindowViewModel SettingsWindowViewModel { get; set; }

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
                    p =>
                    {
                        if (DockTracker.ActiveCenterDockVM is GamePanelManagerViewModel gamePanelManagerVM)
                        {
                            WindowFactory.CreateGameWindow(gamePanelManagerVM.MapComponent.Map);
                        }
                    },
                    p => true//CurrentMainDockViewModel != null ? CurrentMainDockViewModel.IsPlayable : false
                ));
            }
        }

        public MainWindowViewModel()
        {
            Title = "Sauce Editor";
            MapBuilder.CreateTestProject();
        }

        /*public void OnGamePanelManagerViewModelChanged()
        {
            AddChild(GamePanelManagerViewModel, (s, args) =>
            {
                if (args.PropertyName == "SelectedEntities")
                {
                    // Need to check first if this is an entity map...
                    PropertyViewModel.UpdateFromModel(GamePanelManagerViewModel.SelectedEntities.LastOrDefault());
                    PropertyViewModel.IsActive = true;
                }
            });
        }*/

        public void OnPropertyViewModelChanged() => AddChild(PropertyViewModel, (s, args) => GamePanelManagerViewModel?.RequestUpdate());

        public void OnEntityTreePanelViewModelChanged()
        {
            /*if (GamePanelManagerViewModel != null)
            {
                EntityTreePanelViewModel.LayerProvider = GamePanelManagerViewModel.GameManager.EntityManager.LayerProvider;
            }*/
        }

        public void OnGamePanelManagerViewModelChanged()
        {
            GamePanelManagerViewModel.PropertyDisplayer = PropertyViewModel;

            /*if (EntityTreePanelViewModel != null)
            {
                EntityTreePanelViewModel.LayerProvider = GamePanelManagerViewModel.GameManager.EntityManager.LayerProvider;
            }*/
            
            //GamePanelManagerViewModel.EntityDisplayer = EntityTreePanelViewModel;
            //GamePanelManagerViewModel.EntityFactory = this;

            /*EntityTreePanelViewModel.UpdateFromModel(mapComponent, this);
            public void UpdateFromModel(MapComponent mapComponent, IEntityFactory entityFactory)
            {
                _roots.Add(new EntityRootViewModel(mapComponent, entityFactory));
                Roots = new ReadOnlyCollection<EntityRootViewModel>(_roots);
            }*/
        }

        /*public ICommand EntityPropertiesUpdatedCommand => _entityPropertiesUpdatedCommand ?? new RelayCommand(
            p => ScriptOpened?.Invoke(this, new FileEventArgs(_behaviorFilePath)),
            p => _behaviorFilePath != null
        );*/

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

        public void OnLibraryPanelViewModelChanged() => OpenLibraries();

        public void OpenLibraries()
        {
            var libraryManager = new SauceEditorCore.Models.Libraries.LibraryManager()
            {
                Path = SauceEditor.Helpers.FilePathHelper.RESOURCES_DIRECTORY
            };
            LibraryPanelViewModel.UpdateFromModel(libraryManager, this);
        }

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
            var mapComponent = new MapComponent(filePath);
            mapComponent.Load();

            Title = mapComponent.Name + " - SauceEditor";

            GamePanelManagerViewModel = (GamePanelManagerViewModel)GameDockFactory.CreateGamePanelManager(mapComponent);
            //EntityTreePanelViewModel.LayerProvider = GamePanelManagerViewModel.GameManager.EntityManager.LayerProvider;
            //GamePanelManagerViewModel.EntityDisplayer = EntityTreePanelViewModel;
            //GamePanelManagerViewModel.EntityFactory = this;

            // TODO - Is this okay to happen here? We probably need to wait for the GamePanel and its GameLoader process to complete...
            //EntityTreePanelViewModel.UpdateFromModel(mapComponent, this);
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
            var modelComponent = new ModelComponent(filePath);
            modelComponent.Load();

            Title = modelComponent.Name + " - SauceEditor";
            var mapComponent = MapBuilder.GenerateModelMap(modelComponent);

            GamePanelManagerViewModel = (GamePanelManagerViewModel)GameDockFactory.CreateGamePanelManager(mapComponent, modelComponent);
            GamePanelManagerViewModel.ViewType = ViewTypes.Perspective;
        }

        public void OpenBehavior(string filePath)
        {
            var behaviorComponent = new BehaviorComponent(filePath);

            Title = behaviorComponent.Name + " - SauceEditor";
            BehaviorViewModel = (BehaviorViewModel)GameDockFactory.CreateBehaviorView(behaviorComponent);
        }

        public void OpenTexture(string filePath)
        {
            var textureComponent = new TextureComponent(filePath)
            {
                TexturePaths = new SpiceEngineCore.Rendering.Textures.TexturePaths()
                {
                    DiffuseMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                    NormalMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_N_NORMAL_PATH,
                    SpecularMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_S_TEXTURE_PATH,
                    ParallaxMapFilePath = SampleGameProject.Helpers.FilePathHelper.BRICK_01_H_TEXTURE_PATH
                }
            };

            Title = textureComponent.Name + " - " + "SauceEditor";
            var mapComponent = MapBuilder.GenerateTextureMap(textureComponent);

            GamePanelManagerViewModel = (GamePanelManagerViewModel)GameDockFactory.CreateGamePanelManager(mapComponent, textureComponent);
            GamePanelManagerViewModel.ViewType = ViewTypes.Perspective;
            GamePanelManagerViewModel.PerspectiveViewModel.Panel.RenderMode = SpiceEngine.Rendering.RenderModes.Diffuse;

            CommandManager.InvalidateRequerySuggested();
        }

        public void OpenSound(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public void OpenMaterial(string filePath)
        {
            var materialComponent = new MaterialComponent(filePath);

            Title = materialComponent.Name + " - SauceEditor";
            var mapComponent = MapBuilder.GenerateMaterialMap(materialComponent);

            GamePanelManagerViewModel = (GamePanelManagerViewModel)GameDockFactory.CreateGamePanelManager(mapComponent, materialComponent);
            GamePanelManagerViewModel.ViewType = ViewTypes.Perspective;
        }

        public void OpenArchetype(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public void OpenScript(string filePath)
        {
            var scriptComponent = new ScriptComponent(filePath);

            Title = scriptComponent.Name + " - SauceEditor";
            ScriptViewModel = (ScriptViewModel)GameDockFactory.CreateScriptView(scriptComponent);
        }

        public void CreateLight()
        {

        }

        public void CreateBrush()
        {

        }

        public void CreateActor()
        {

        }

        public void CreateVolume()
        {

        }

        public void SelectEntity(int id)
        {
            GamePanelManagerViewModel.SelectEntities(id.Yield());
        }

        public void DuplicateEntity(int id)
        {

        }

        public void DeleteEntity(int id)
        {

        }
    }
}