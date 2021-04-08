using SpiceEngine.HID;
using SpiceEngine.Maps;
using SpiceEngine.Rendering;
using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.HID;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;

namespace SpiceEngine.Game
{
    public class WindowWrapper
    {
        //private IWindowContext _windowContext;
        private IRenderContext _renderContext;

        private GameWindow _gameWindow;

        public WindowWrapper(Configuration configuration)
        {
            //_windowContext = WindowContextFactory.SharedInstance.CreateWindowContext(configuration);
            
            _gameWindow = new GameWindow(configuration, WindowContextFactory.SharedInstance);
            _renderContext = RenderContextFactory.SharedInstance.CreateRenderContext(configuration, _gameWindow);
        }

        public void Start(IMap map)
        {
            _gameWindow.RenderContext = _renderContext;
            _gameWindow.Map = map as Map;
            _gameWindow.Start();

            // TODO - Casting is a poor way to do this, can we instead put the Start call in the IWindowContext interface and handle map loading elsewhere?
            /*if (_windowContext is GameWindow gameWindow)
            {
                gameWindow.RenderContext = _renderContext;
                gameWindow.Map = map as Map;
                gameWindow.Start();
            }*/
        }
    }
}
