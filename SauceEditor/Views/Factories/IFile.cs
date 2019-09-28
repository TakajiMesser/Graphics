namespace SauceEditor.Views.Factories
{
    public interface IFile
    {
        void Save(string filePath);
        void Load(string filePath);
    }
}