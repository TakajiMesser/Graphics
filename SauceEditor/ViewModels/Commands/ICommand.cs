namespace SauceEditor.ViewModels.Commands
{
    public interface ICommand
    {
        void Do();
        void Undo();
    }
}
