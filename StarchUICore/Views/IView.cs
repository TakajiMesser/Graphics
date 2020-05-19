using StarchUICore.Layers;
using System;

namespace StarchUICore.Views
{
    public interface IView : IElement
    {
        Layer Foreground { get; set; }
        Layer Background { get; set; }

        IView Duplicate();
    }
}
