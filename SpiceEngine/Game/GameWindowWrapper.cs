using SpiceEngineCore.Maps;

namespace SpiceEngine.Game
{
    public class GameWindowWrapper
    {
        private IMap _map;

        public GameWindowWrapper(IMap map) => _map = map;

        public void Start()
        {
            using (var gameWindow = new GameWindow(_map))
            {
                gameWindow.LoadAndRun();
            }
        }
    }
}
