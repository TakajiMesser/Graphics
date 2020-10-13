namespace SpiceEngineCore.Commands
{
    public interface ICommander
    {
        void RunCommand(ICommand command);
    }
}
