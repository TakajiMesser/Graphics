using TakoEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Scripting
{
    /// <summary>
    /// The director class issues commands to game objects, by requesting them from the GameState
    /// 
    /// </summary>
    public class Director : GameObject
    {
        private GameState _gameState;

        public Director(string name, GameState gameState) : base(name)
        {
            _gameState = gameState;
        }
    }
}
