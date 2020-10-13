using SpiceEngineCore.Commands;
using SpiceEngineCore.Entities;

namespace SpiceEngineCore.Game
{
    // Holds ALL GameSystems for the current simulation
    // Holds a command stack
    // Any attempts to alter Components should route to GameManager
    // by pushing a new Command
    // Has the current GameState
    public class GameManager : SystemProvider, ICommander
    {
        private CommandStack _commands;
        private int _currentTick = 0;

        public virtual void Load()
        {
            EntityProvider = new EntityManager();
            _commands = new CommandStack();
        }

        public override void AddGameSystem<T>(T gameSystem)
        {
            gameSystem.SetSystemProvider(this);
            gameSystem.SetCommander(this);

            base.AddGameSystem<T>(gameSystem);
        }

        public void Update()
        {
            foreach (var system in _gameSystems)
            {
                system.Tick();
            }
        }

        public void RunCommand(ICommand command)
        {
            _commands.Push(command);
            command.Do();
        }
    }
}
