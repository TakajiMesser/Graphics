using SpiceEngineCore.UserInterfaces;
using System.Collections.Generic;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IUIElementBuilder : IComponentBuilder<IUIElement>
    {
        string Name { get; }
        List<string> ChildElementNames { get; }

        string RelativeHorizontalAnchorElementName { get; }
        string RelativeVerticalAnchorElementName { get; }
        string RelativeHorizontalDockElementName { get; }
        string RelativeVerticalDockElementName { get; }

        string FontFilePath { get; }
        int FontSize { get; }
    }
}
