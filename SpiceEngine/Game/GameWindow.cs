using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using TangyHIDCore.Outputs;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;

namespace SpiceEngine.Game
{
    public class GameWindow : Window
    {
        private Timer _fpsTimer = new Timer(1000);
        private List<double> _frequencies = new List<double>();

        public GameWindow(Configuration configuration) : base(configuration)
        {
            _fpsTimer.Elapsed += FpsTimer_Elapsed;
            //Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));
        }

        public Map Map { get; set; }

        public override async Task LoadAsync()
        {
            var renderManager = new RenderManager(this, Display)
            {
                Invoker = this
            };
            _fpsTimer.Start();

            var simulationManager = new SimulationManager(Display.Resolution);
            simulationManager.Load();
            simulationManager.InputManager.RegisterDevices(this);
            simulationManager.InputManager.EscapePressed += (s, args) => Close();
            simulationManager.RenderProvider = renderManager;
            simulationManager.PhysicsSystem.SetBoundaries(Map.Boundaries);

            _gameLoader = new SpiceEngineCore.Game.GameLoader();
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

        /*protected override void Update(double elapsedMilliseconds)
        {
            _mouseState = Mouse.GetCursorState();
            base.Update(elapsedMilliseconds);
        }

        protected override void Render(double elapsedMilliseconds)
        {
            if (_renderer != null && _renderer.IsLoaded)
            {
                _frequencies.Add(RenderFrequency);
            }
            base.Render(elapsedMilliseconds);
        }*/

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
