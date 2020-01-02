using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.UserInterfaces.Groups
{
    public interface IUIGroup : IUIItem
    {
        IEnumerable<IUIItem> GetChildren();

        IUIGroup Duplicate();
    }
}
