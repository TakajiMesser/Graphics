namespace SpiceEngineCore.Commands
{
    public interface ICommand
    {
        void Do();
        void Undo();
    }
}
