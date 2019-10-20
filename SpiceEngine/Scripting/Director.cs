using SpiceEngine.Entities.Actors;
using SpiceEngine.Game;

namespace SpiceEngine.Scripting
{
    /// <summary>
    /// The director class issues commands to game objects, by requesting them from the GameState
    /// 
    /// </summary>
    public class Director : Actor
    {
        private GameManager _gameManager;

        public Director(string name, GameManager gameManager)
        {
            Name = name;
            _gameManager = gameManager;
        }
    }
}
