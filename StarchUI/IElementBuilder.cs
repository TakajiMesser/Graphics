using SpiceEngineCore.Components;
using System.Collections.Generic;

namespace StarchUICore
{
    public interface IElementBuilder : IComponentBuilder<IElement>
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
