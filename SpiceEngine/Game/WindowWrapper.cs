using SpiceEngine.Maps;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;

namespace SpiceEngine.Game
{
    public class WindowWrapper
    {
        private Window _window;

        public WindowWrapper(Configuration configuration)
        {
            _window = new Window(configuration);
        }

        public void Start(Map map)
        {
            _window.Map = map;
            _window.Start();
        }
    }
}
