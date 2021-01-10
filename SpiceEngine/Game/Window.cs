using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using TangyHIDCore;
using TangyHIDCore.Outputs;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;

namespace SpiceEngine.Game
{
    public class Window : TangyHIDCore.Outputs.Window, IMouseTracker
    {
        private MouseState? _mouseState = null;
        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

        public Window(Configuration configuration) : base(configuration)
        {
            _fpsTimer.Elapsed += FpsTimer_Elapsed;
            Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));
        }

        public Map Map { get; set; }

        public Vector2? MouseCoordinates => _mouseState.HasValue
            ? new Vector2(_mouseState.Value.X, _mouseState.Value.Y)
            : (Vector2?)null;

        public Vector2? RelativeCoordinates => _mouseState.HasValue
            ? PointToClient(new Point(_mouseState.Value.X, _mouseState.Value.Y)).ToVector2()
            : (Vector2?)null;

        public bool IsMouseInWindow => _mouseState != null
            && (_mouseState.Value.X.IsBetween(0, Display.Window.Width)
            && _mouseState.Value.Y.IsBetween(0, Display.Window.Height));

        public Resolution WindowSize => Display.Window;

        public override async Task LoadAsync()
        {
            var renderManager = new RenderManager(Display)
            {
                Invoker = this
            };
            _fpsTimer.Start();

            var simulationManager = new SimulationManager(Display.Resolution);
            simulationManager.Load();
            simulationManager.InputManager.EscapePressed += (s, args) => Close();
            simulationManager.SetMouseTracker(this);
            simulationManager.RenderProvider = renderManager;
            simulationManager.PhysicsSystem.SetBoundaries(Map.Boundaries);

            _gameLoader.SetEntityProvider(simulationManager.EntityProvider);
            _gameLoader.AddComponentLoader(simulationManager.PhysicsSystem);
            _gameLoader.AddComponentLoader(simulationManager.BehaviorSystem);
            _gameLoader.AddComponentLoader(simulationManager.AnimationSystem);
            _gameLoader.AddComponentLoader(simulationManager.UISystem);
            _gameLoader.AddRenderableLoader(renderManager);

            _gameLoader.AddFromMap(Map);

            renderManager.SetEntityProvider(simulationManager.EntityProvider);
            renderManager.SetAnimationProvider(simulationManager.AnimationSystem);
            renderManager.SetUIProvider(simulationManager.UISystem);

            renderManager.LoadFromMap(Map);

            //_gameLoader.Load();
            _gameLoader.TimedOut += (s, args) => RunSync(() => throw new TimeoutException());
            await _gameLoader.LoadAsync();

            // Set up UIManager to track mouse selections for UI control interactions
            simulationManager.InputManager.MouseDownSelected += (s, args) => simulationManager.UISystem.RegisterSelection(renderManager.GetEntityIDFromSelection(args.MouseCoordinates));
            simulationManager.InputManager.MouseUpSelected += (s, args) => simulationManager.UISystem.RegisterDeselection(renderManager.GetEntityIDFromSelection(args.MouseCoordinates));
            //_simulationManager.BehaviorSystem.SetSelectionTracker(_renderManager);

            //_stopWatch.Stop();
            //LogWatch("Total");

            _simulator = simulationManager;
            _renderer = renderManager;

            IsLoaded = true;
        }

        protected override void Update()
        {
            _mouseState = Mouse.GetCursorState();
            base.Update();
        }

        protected override void Render()
        {
            /*if (_renderer != null && _renderer.IsLoaded)
            {
                _frequencies.Add(RenderFrequency);
            }*/
            base.Render();
        }

        private void FpsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_frequencies.Count > 0)
            {
                var total = 0.0;

                for (var i = 0; i < _frequencies.Count; i++)
                {
                    total += _frequencies[i];
                }

                _renderer.Frequency = total / _frequencies.Count;//_frequencies.Average();
                _frequencies.Clear();
            }
        }
    }
}
