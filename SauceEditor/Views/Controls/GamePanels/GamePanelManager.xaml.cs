using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Physics;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Scripting;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

namespace SauceEditor.Views.Controls.GamePanels
{
    /// <summary>
    /// Interaction logic for GamePanelManager.xaml
    /// </summary>
    public partial class GamePanelManager : DockPanel
    {
        public Resolution Resolution { get; set; }
        public TransformModes TransformMode
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
        }

        public event EventHandler<EntitiesEventArgs> EntitySelectionChanged;

        private TransformModes _transformMode;

        private Map _map;

        private EntityManager _entityManager = new EntityManager();
        private TextureManager _textureManager = new TextureManager();
        private PhysicsManager _physicsManager;
        private ScriptManager _scriptManager;

        private DockableGamePanel _perspectiveView;
        private DockableGamePanel _xView;
        private DockableGamePanel _yView;
        private DockableGamePanel _zView;

        public GamePanelManager(string mapPath)
        {
            InitializeComponent();
            Open(mapPath);

            TransformMode = TransformModes.Translate;
        }

        public void SetSelectedTool(SpiceEngine.Game.Tools tool)
        {
            _perspectiveView.Panel.SelectedTool = tool;
            _xView.Panel.SelectedTool = tool;
            _yView.Panel.SelectedTool = tool;
            _zView.Panel.SelectedTool = tool;
        }

        public void UpdateEntity(IEntity entity)
        {
            _perspectiveView.Panel.UpdateEntities(entity.Yield());
            _xView.Panel.UpdateEntities(entity.Yield());
            _yView.Panel.UpdateEntities(entity.Yield());
            _zView.Panel.UpdateEntities(entity.Yield());
        }

        public void UpdateEntities(IEnumerable<IEntity> entities)
        {
            _perspectiveView.Panel.UpdateEntities(entities);
            _xView.Panel.UpdateEntities(entities);
            _yView.Panel.UpdateEntities(entities);
            _zView.Panel.UpdateEntities(entities);
        }

        private void MainDock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    TransformMode = (TransformModes)((int)(TransformMode + 1) % Enum.GetValues(typeof(TransformModes)).Length);
                    break;
                case Key.Home:
                    _perspectiveView?.Panel.CenterView();
                    _xView?.Panel.CenterView();
                    _yView?.Panel.CenterView();
                    _zView?.Panel.CenterView();
                    break;
            }

            e.Handled = true;
        }

        private object _panelLock = new object();
        private bool _isGLContextLoaded = false;

        public void Open(string mapPath)
        {
            Resolution = new Resolution((int)Width, (int)Height);

            _textureManager.EnableMipMapping = true;
            _textureManager.EnableAnisotropy = true;

            _map = Map.Load(mapPath);
            CreateAndShowPanels();
        }

        private void LoadPanels()
        {
            // Wait for at least one panel to finish loading so that we can be sure the GLContext is properly loaded
            lock (_panelLock)
            {
                // Lock and check to ensure that this only happens once
                if (!_isGLContextLoaded)
                {
                    // TODO - Determine how to handle the fact that each GamePanel is its own IMouseDelta...
                    var entityMapping = LoadFromMap(_map);

                    _perspectiveView.Panel.LoadFromMap(_map, _entityManager, _textureManager, entityMapping);
                    _xView.Panel.LoadFromMap(_map, _entityManager, _textureManager, entityMapping);
                    _yView.Panel.LoadFromMap(_map, _entityManager, _textureManager, entityMapping);
                    _zView.Panel.LoadFromMap(_map, _entityManager, _textureManager, entityMapping);
                }

                _isGLContextLoaded = true;
            }
        }

        private void DuplicateEntity(int entityID, int duplicateEntityID)
        {
            _perspectiveView.Panel.Duplicate(entityID, duplicateEntityID);
            _xView.Panel.Duplicate(entityID, duplicateEntityID);
            _yView.Panel.Duplicate(entityID, duplicateEntityID);
            _zView.Panel.Duplicate(entityID, duplicateEntityID);

            _physicsManager.DuplicateBody(entityID, duplicateEntityID);
            //_scriptManager;
        }

        private DockableGamePanel CreatePanel(ViewTypes viewType, AnchorableShowStrategy showStrategy)
        {
            var gamePanel = new DockableGamePanel(viewType);
            gamePanel.Panel.Load += (s, args) => LoadPanels();
            gamePanel.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.ID, args.NewID);
            gamePanel.EntitySelectionChanged += (s, args) => OnEntitySelectionChanged(viewType, args);

            /*var anchorable = new LayoutAnchorable
            {
                Title = GetTitle(viewType),
                //FloatingHeight = 400,
                //FloatingWidth = 500,
                Content = gamePanel
            };

            anchorable.AddToLayout(MainDockingManager, showStrategy);
            anchorable.DockAsDocument();*/
            gamePanel.Anchorable.Show();

            return gamePanel;
        }

        private void OnEntitySelectionChanged(ViewTypes viewType, EntitiesEventArgs args)
        {
            if (viewType != ViewTypes.Perspective) _perspectiveView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.X) _xView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.Y) _yView.Panel.SelectEntities(args.Entities);
            if (viewType != ViewTypes.Z) _zView.Panel.SelectEntities(args.Entities);

            if (args.Entities.Count > 0)
            {
                if (viewType != ViewTypes.Perspective) _perspectiveView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.X) _xView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.Y) _yView.Panel.UpdateEntities(args.Entities);
                if (viewType != ViewTypes.Z) _zView.Panel.UpdateEntities(args.Entities);
            }

            EntitySelectionChanged?.Invoke(this, args);
        }

        private void CreateAndShowPanels()
        {
            _perspectiveView = CreatePanel(ViewTypes.Perspective, AnchorableShowStrategy.Most);
            //_perspectiveView.CommandExecuted += (s, args) => CommandStack.Push(args.Command);
            //_perspectiveView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            //_perspectiveView.Width = MainDockingManager.Width / 2.0;
            //_perspectiveView.Height = MainDockingManager.Height / 2.0;
            //_perspectiveView.Show();

            _xView = CreatePanel(ViewTypes.X, AnchorableShowStrategy.Right);
            //_xView.CommandExecuted += (s, args) => CommandStack.Push(args.Command);
            //_xView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            //_xView.Width = MainDockingManager.Width / 2.0;
            //_xView.Height = MainDockingManager.Height / 2.0;
            //_xView.Show(Docks.Right);

            _yView = CreatePanel(ViewTypes.Y, AnchorableShowStrategy.Bottom);
            //_yView.CommandExecuted += (s, args) => CommandStack.Push(args.Command);
            //_yView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            //_yView.Width = MainDockingManager.Width;// / 2.0;
            //_yView.Height = MainDockingManager.Height / 2.0;
            //_yView.Show(Docks.Bottom);

            _zView = CreatePanel(ViewTypes.Z, AnchorableShowStrategy.Right | AnchorableShowStrategy.Bottom);
            //_zView.CommandExecuted += (s, args) => CommandStack.Push(args.Command);
            //_zView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            //_zView.Show(Docks.Right | Docks.Bottom);

            //Get the main LayoutDocumentPane of your DockingManager 
            var rootPanel = MainDockingManager.Layout.RootPanel;
            rootPanel.Children.Clear();
            rootPanel.Orientation = Orientation.Vertical;

            var topPaneGroup = new LayoutAnchorablePaneGroup()
            {
                Orientation = Orientation.Horizontal,
                DockMinHeight = MainDockingManager.ActualHeight / 2.0
            };
            topPaneGroup.Children.Add(_perspectiveView);
            topPaneGroup.Children.Add(_xView);

            var bottomPaneGroup = new LayoutAnchorablePaneGroup()
            {
                Orientation = Orientation.Horizontal,
                DockMinHeight = MainDockingManager.ActualHeight / 2.0
            };
            bottomPaneGroup.Children.Add(_yView);
            bottomPaneGroup.Children.Add(_zView);

            rootPanel.Children.Add(topPaneGroup);
            rootPanel.Children.Add(bottomPaneGroup);
        }

        public EntityMapping LoadFromMap(Map map)
        {
            switch (map)
            {
                case Map2D map2D:
                    _physicsManager = new PhysicsManager(_entityManager, map2D.Boundaries);
                    break;
                case Map3D map3D:
                    _physicsManager = new PhysicsManager(_entityManager, map3D.Boundaries);
                    break;
            }

            _scriptManager = new ScriptManager(_entityManager, _physicsManager);
            _entityManager.ClearEntities();

            var lightIDs = LoadLights(map.Lights);
            var brushIDs = LoadBrushes(map.Brushes);
            var volumeIDs = LoadVolumes(map.Volumes);
            var actorIDs = LoadActors(map.Actors);

            var entityMapping = new EntityMapping(actorIDs, brushIDs, volumeIDs, lightIDs);

            _scriptManager.Load();

            return entityMapping;
        }

        private IEnumerable<int> LoadLights(IEnumerable<Light> lights)
        {
            foreach (var light in lights)
            {
                int entityID = _entityManager.AddEntity(light);
                yield return entityID;
            }
        }

        private IEnumerable<int> LoadBrushes(IEnumerable<MapBrush> mapBrushes)
        {
            foreach (var mapBrush in mapBrushes)
            {
                var brush = mapBrush.ToEntity();
                int entityID = _entityManager.AddEntity(brush);

                brush.TextureMapping = mapBrush.TexturesPaths.ToTextureMapping(_textureManager);

                var shape = mapBrush.ToShape();
                _physicsManager.AddBrush(entityID, shape, brush.Position);

                yield return entityID;
            }
        }

        private IEnumerable<int> LoadVolumes(IEnumerable<MapVolume> mapVolumes)
        {
            foreach (var mapVolume in mapVolumes)
            {
                var volume = mapVolume.ToEntity();
                int entityID = _entityManager.AddEntity(volume);

                var shape = mapVolume.ToShape();
                _physicsManager.AddVolume(entityID, shape, volume.Position);

                yield return entityID;
            }
        }

        private IEnumerable<int> LoadActors(IList<MapActor> mapActors)
        {
            foreach (var mapActor in mapActors)
            {
                var actor = mapActor.ToEntity(/*_gameManager.TextureManager*/);
                int entityID = _entityManager.AddEntity(actor);

                var meshes = mapActor.ToMeshes();

                var shape = mapActor.ToShape();
                _physicsManager.AddActor(entityID, shape, actor.Position);

                /*actor.HasCollision = mapActor.HasCollision;
                actor.Bounds = actor.Name == "Player"
                    ? (Bounds)new BoundingCircle(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)))
                    : new BoundingBox(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)));*/

                var behavior = mapActor.ToBehavior();
                _scriptManager.AddBehavior(entityID, behavior);

                _scriptManager.AddProperties(entityID, mapActor.Properties);
                _scriptManager.AddStimuli(entityID, mapActor.Stimuli);

                if (actor is AnimatedActor)
                {
                    using (var importer = new Assimp.AssimpContext())
                    {
                        var scene = importer.ImportFile(mapActor.ModelFilePath);

                        for (var i = 0; i < scene.Meshes.Count; i++)
                        {
                            var textureMapping = i < mapActor.TexturesPaths.Count
                                ? mapActor.TexturesPaths[i].ToTextureMapping(_textureManager)
                                : new TexturePaths(scene.Materials[scene.Meshes[i].MaterialIndex], Path.GetDirectoryName(mapActor.ModelFilePath)).ToTextureMapping(_textureManager);

                            actor.AddTextureMapping(i, textureMapping);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < mapActor.TexturesPaths.Count; i++)
                    {
                        actor.AddTextureMapping(i, mapActor.TexturesPaths[i].ToTextureMapping(_textureManager));
                    }
                }

                yield return entityID;
            }
        }

        private void TranslateButton_Click(object sender, RoutedEventArgs e) => TransformMode = TransformModes.Translate;

        private void RotateButton_Click(object sender, RoutedEventArgs e) => TransformMode = TransformModes.Rotate;

        private void ScaleButton_Click(object sender, RoutedEventArgs e) => TransformMode = TransformModes.Scale;

        public void SetView(Models.ViewTypes view)
        {
            switch (view)
            {
                case Models.ViewTypes.All:
                    View_All.IsSelected = true;
                    break;
                case Models.ViewTypes.Perspective:
                    View_Perspective.IsSelected = true;
                    break;
                case Models.ViewTypes.X:
                    View_X.IsSelected = true;
                    break;
                case Models.ViewTypes.Y:
                    View_Y.IsSelected = true;
                    break;
                case Models.ViewTypes.Z:
                    View_Z.IsSelected = true;
                    break;
            }
        }

        private void ViewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ViewComboBox.SelectedItem as ComboBoxItem;
            
            switch (selectedItem.Content)
            {
                case "All":
                    ((LayoutAnchorablePaneGroup)_zView.Parent).DockMinHeight = MainDockingManager.ActualHeight / 2.0;
                    ((LayoutAnchorablePaneGroup)_yView.Parent).DockMinHeight = MainDockingManager.ActualHeight / 2.0;
                    ((LayoutAnchorablePaneGroup)_perspectiveView.Parent).DockMinHeight = MainDockingManager.ActualHeight / 2.0;
                    ((LayoutAnchorablePaneGroup)_xView.Parent).DockMinHeight = MainDockingManager.ActualHeight / 2.0;

                    _perspectiveView.Anchorable.Show();
                    _xView.Anchorable.Show();
                    _yView.Anchorable.Show();
                    _zView.Anchorable.Show();
                    break;
                case "Perspective":
                    Hide(_zView);
                    Hide(_yView);
                    Hide(_xView);
                    Show(_perspectiveView);
                    break;
                case "X":
                    Hide(_zView);
                    Hide(_yView);
                    Hide(_perspectiveView);
                    Show(_xView);
                    break;
                case "Y":
                    Hide(_zView);
                    Hide(_xView);
                    Hide(_perspectiveView);
                    Show(_yView);
                    break;
                case "Z":
                    Hide(_yView);
                    Hide(_xView);
                    Hide(_perspectiveView);
                    Show(_zView);
                    break;
            }
        }

        private void Show(DockableGamePanel gamePanel)
        {
            ((LayoutAnchorablePaneGroup)gamePanel.Parent).DockMinHeight = MainDockingManager.ActualHeight;
            gamePanel.Anchorable.Show();
        }

        private void Hide(DockableGamePanel gamePanel)
        {
            ((LayoutAnchorablePaneGroup)gamePanel.Parent).DockMinHeight = 0;
            gamePanel.Anchorable.Hide();
        }
    }
}
