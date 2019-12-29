using SpiceEngineCore.Rendering.UserInterfaces.Views;

namespace SpiceEngineCore.Rendering.UserInterfaces
{
    public interface IUIProvider
    {
        void AddView(int entityID, IUIView view);
        IUIView GetView(int entityID);
        void Clear();

        void Load();
        void Draw();
    }
}
