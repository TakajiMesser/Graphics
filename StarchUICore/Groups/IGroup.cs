using SpiceEngineCore.Entities;
using System;
using System.Collections.Generic;

namespace StarchUICore.Groups
{
    public interface IGroup : IUIItem
    {
        UILayoutTypes LayoutType { get; set; }
        IEnumerable<IUIItem> GetChildren();

        IGroup Duplicate();
    }
}
