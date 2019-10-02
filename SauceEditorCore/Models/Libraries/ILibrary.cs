namespace SauceEditorCore.Models.Libraries
{
    public interface ILibrary
    {
        string Path { get; set; }

        void Load();
    }
}
