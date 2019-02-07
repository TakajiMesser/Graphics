using DockingLibrary;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpiceEngine.Entities;
using SpiceEngine.Game;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Utilities;
using SpiceEngine.Physics;
using SpiceEngine.Rendering.Textures;
using System.IO;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Sounds;
using SpiceEngine.Scripting;
using SpiceEngine.Entities.Lights;

namespace SauceEditor.Controls.GamePanels
{
    /// <summary>
    /// Interaction logic for GamePanelManager.xaml
    /// </summary>
    public partial class GamePanelManager : DockingLibrary.DockableContent
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

        public GamePanelManager(DockManager dockManager, string mapPath) : base(dockManager)
        {
            InitializeComponent();
            Open(mapPath);

            Title = Path.GetFileNameWithoutExtension(mapPath);

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

            _scriptManager = new ScriptManager(_entityManager);

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

        private void CreateAndShowPanels()
        {
            _perspectiveView = new DockableGamePanel(MainDockManager, ViewTypes.Perspective)
            {
                Title = "Perspective",
                Focusable = true
            };
            _perspectiveView.Panel.Load += (s, args) => LoadPanels();
            _perspectiveView.EntitySelectionChanged += (s, args) =>
            {
                _xView.Panel.SelectEntities(args.Entities);
                _yView.Panel.SelectEntities(args.Entities);
                _zView.Panel.SelectEntities(args.Entities);

                if (args.Entities.Count > 0)
                {
                    _xView.Panel.UpdateEntities(args.Entities);
                    _yView.Panel.UpdateEntities(args.Entities);
                    _zView.Panel.UpdateEntities(args.Entities);
                }

                EntitySelectionChanged?.Invoke(this, args);
            };
            _perspectiveView.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.ID, args.NewID);
            //_perspectiveView.CommandExecuted += (s, args) => CommandStack.Push(args.Command);
            //_perspectiveView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            _perspectiveView.Width = MainDockManager.Width / 2.0;
            _perspectiveView.Height = MainDockManager.Height / 2.0;
            _perspectiveView.Show();

            _xView = new DockableGamePanel(MainDockManager, ViewTypes.X)
            {
                Title = "X",
                Focusable = true
            };
            _xView.Panel.Load += (s, args) => LoadPanels();
            _xView.EntitySelectionChanged += (s, args) =>
            {
                _perspectiveView.Panel.SelectEntities(args.Entities);
                _yView.Panel.SelectEntities(args.Entities);
                _zView.Panel.SelectEntities(args.Entities);

                if (args.Entities.Count > 0)
                {
                    _perspectiveView.Panel.UpdateEntities(args.Entities);
                    _yView.Panel.UpdateEntities(args.Entities);
                    _zView.Panel.UpdateEntities(args.Entities);
                }

                EntitySelectionChanged?.Invoke(this, args);
            };
            _xView.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.ID, args.NewID);
            //_xView.CommandExecuted += (s, args) => CommandStack.Push(args.Command);
            //_xView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            _xView.Width = MainDockManager.Width / 2.0;
            _xView.Height = MainDockManager.Height / 2.0;
            _xView.Show(Docks.Right);

            _yView = new DockableGamePanel(MainDockManager, ViewTypes.Y)
            {
                Title = "Y",
                Focusable = true
            };
            _yView.Panel.Load += (s, args) => LoadPanels();
            _yView.EntitySelectionChanged += (s, args) =>
            {
                _perspectiveView.Panel.SelectEntities(args.Entities);
                _xView.Panel.SelectEntities(args.Entities);
                _zView.Panel.SelectEntities(args.Entities);

                if (args.Entities.Count > 0)
                {
                    _perspectiveView.Panel.UpdateEntities(args.Entities);
                    _xView.Panel.UpdateEntities(args.Entities);
                    _zView.Panel.UpdateEntities(args.Entities);
                }

                EntitySelectionChanged?.Invoke(this, args);
            };
            _yView.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.ID, args.NewID);
            //_yView.CommandExecuted += (s, args) => CommandStack.Push(args.Command);
            //_yView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            _yView.Width = MainDockManager.Width;// / 2.0;
            _yView.Height = MainDockManager.Height / 2.0;
            _yView.Show(Docks.Bottom);

            _zView = new DockableGamePanel(MainDockManager, ViewTypes.Z)
            {
                Title = "Z",
                Focusable = true
            };
            _zView.Panel.Load += (s, args) => LoadPanels();
            _zView.EntitySelectionChanged += (s, args) =>
            {
                _perspectiveView.Panel.SelectEntities(args.Entities);
                _xView.Panel.SelectEntities(args.Entities);
                _yView.Panel.SelectEntities(args.Entities);

                if (args.Entities.Count > 0)
                {
                    _perspectiveView.Panel.UpdateEntities(args.Entities);
                    _xView.Panel.UpdateEntities(args.Entities);
                    _yView.Panel.UpdateEntities(args.Entities);
                }

                EntitySelectionChanged?.Invoke(this, args);
            };
            _zView.Panel.EntityDuplicated += (s, args) => DuplicateEntity(args.ID, args.NewID);
            //_zView.CommandExecuted += (s, args) => CommandStack.Push(args.Command);
            //_zView.Closed += (s, args) => PlayButton.Visibility = Visibility.Hidden;
            _zView.Show(Docks.Right | Docks.Bottom);
        }

        public EntityMapping LoadFromMap(Map map)
        {
            switch (map)
            {
                case Map2D map2D:
                    _physicsManager = new PhysicsManager(_entityManager, _scriptManager, map2D.Boundaries);
                    break;
                case Map3D map3D:
                    _physicsManager = new PhysicsManager(_entityManager, _scriptManager, map3D.Boundaries);
                    break;
            }

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

        public void SetView(Structure.ViewTypes view)
        {
            switch (view)
            {
                case Structure.ViewTypes.All:
                    View_All.IsSelected = true;
                    break;
                case Structure.ViewTypes.Perspective:
                    View_Perspective.IsSelected = true;
                    break;
                case Structure.ViewTypes.X:
                    View_X.IsSelected = true;
                    break;
                case Structure.ViewTypes.Y:
                    View_Y.IsSelected = true;
                    break;
                case Structure.ViewTypes.Z:
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
                    _perspectiveView.Show();
                    _xView.Show();
                    _yView.Show();
                    _zView.Show();
                    break;
                case "Perspective":
                    _zView.Hide();
                    _yView.Hide();
                    _xView.Hide();
                    _perspectiveView.Show();
                    break;
                case "X":
                    _zView.Hide();
                    _yView.Hide();
                    _perspectiveView.Hide();
                    _xView.Show();
                    break;
                case "Y":
                    _zView.Hide();
                    _xView.Hide();
                    _perspectiveView.Hide();
                    _yView.Show();
                    break;
                case "Z":
                    _yView.Hide();
                    _xView.Hide();
                    _perspectiveView.Hide();
                    _zView.Show();
                    break;
            }
        }
    }
}
