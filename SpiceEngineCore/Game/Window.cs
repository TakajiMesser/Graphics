using OpenTK;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;

namespace SpiceEngineCore.Game
{
    public abstract class Window : NativeWindow
    {
        private ISimulator _simulator;
        private IRenderer _renderer;

        private Configuration _configuration;
        private GameLoader _gameLoader;
        private ConcurrentQueue<Action> _mainActionQueue = new ConcurrentQueue<Action>();

        private Stopwatch _updateWatch = new Stopwatch();
        private Stopwatch _renderWatch = new Stopwatch();

        private double _msPerUpdate;
        private double _msPerRender;

        private double _msSinceUpdate;
        private double _msSinceRender;

        public Window(Configuration configuration)
        {
            _configuration = configuration;

            _msPerUpdate = 1000.0 / _configuration.UpdatesPerSecond;
            _msPerRender = 1000.0 / _configuration.RendersPerSecond;
        }

        public bool IsExiting { get; private set; }

        public void Load()
        {
            //MakeCurrent();
        }

        public void Run()
        {
            _updateWatch.Start();
            _renderWatch.Start();

            while (true)
            {
                ProcessEvents();

                if (!Exists || IsExiting)
                {
                    return;
                }

                CheckForUpdate();
                CheckForRender();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            IsExiting = true;
            base.OnClosing(e);
        }

        protected abstract void Update();
        protected abstract void Render();

        private void CheckForUpdate()
        {
            _msSinceUpdate += _updateWatch.Elapsed.TotalMilliseconds;
            _updateWatch.Restart();

            // How many updates, if any, should take place?
            if (_msSinceUpdate >= _msPerUpdate)
            {
                var nUpdates = (int)(_msSinceUpdate / _msPerUpdate);
                _msSinceUpdate -= nUpdates * _msPerUpdate;

                DispatchUpdates(nUpdates);
            }
        }

        private void CheckForRender()
        {
            _msSinceRender += _renderWatch.Elapsed.TotalMilliseconds;
            _renderWatch.Restart();

            // How many updates, if any, should take place?
            if (_msSinceRender >= _msPerRender)
            {
                var nRenders = (int)(_msSinceRender / _msPerRender);
                _msSinceRender -= nRenders * _msPerRender;

                DispatchRenders(nRenders);
            }
        }

        private void DispatchUpdates(int nUpdates)
        {
            for (var i = 0; i < nUpdates; i++)
            {
                Update();
            }
        }

        private void DispatchRenders(int nRenders)
        {
            for (var i = 0; i < nRenders; i++)
            {
                Render();
            }
        }
    }
}
