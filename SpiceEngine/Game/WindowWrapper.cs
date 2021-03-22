using SpiceEngine.Maps;
using SpiceEngineCore.Maps;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;

namespace SpiceEngine.Game
{
    public class WindowWrapper
    {
        private GameWindow _window;

        public WindowWrapper(Configuration configuration) => _window = new GameWindow(configuration);

        public void Start(IMap map)
        {
            _window.Map = map as Map;
            _window.Start();
        }
    }
}
