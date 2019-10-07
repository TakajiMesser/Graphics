namespace SauceEditor.Views.Factories
{
    public interface IEntityFactory
    {
        void CreateLight();
        void CreateBrush();
        void CreateActor();
        void CreateVolume();

        void SelectEntity(int id);
        void DuplicateEntity(int id);
        void DeleteEntity(int id);
    }
}