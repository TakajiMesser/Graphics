using SpiceEngine.Entities;
using SpiceEngine.Game;

namespace SpiceEngine.Scripting
{
    /// <summary>
    /// The director class issues commands to game objects, by requesting them from the GameState
    /// 
    /// </summary>
    public class Director : Actor
    {
        private GameState _gameState;

        public Director(string name, GameState gameState) : base(name)
        {
            _gameState = gameState;
        }
    }
}
