using SpiceEngineCore.Components;

namespace SpiceEngineCore.UserInterfaces
{
    public interface IUIElement : IComponent
    {
        bool IsEnabled { get; set; }
        bool IsVisible { get; set; }
        bool IsGone { get; set; }

        float Alpha { get; set; }

        bool IsLaidOut { get; }

        void Update(int nTicks);
    }
}
