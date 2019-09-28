namespace SauceEditor.Views.Factories
{
    public interface IComponentFactory
    {
        void CreateProject();
        void CreateMap();
        void CreateModel();
        void CreateBehavior();
        void CreateTexture();
        void CreateSound();
        void CreateMaterial();
        void CreateArchetype();
        void CreateScript();

        void OpenProject(string filePath);
        void OpenMap(string filePath);
        void OpenModel(string filePath);
        void OpenBehavior(string filePath);
        void OpenTexture(string filePath);
        void OpenSound(string filePath);
        void OpenMaterial(string filePath);
        void OpenArchetype(string filePath);
        void OpenScript(string filePath);
    }
}