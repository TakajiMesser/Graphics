using StarchUICore.Attributes.Units;
using System.Collections.Generic;

namespace StarchUICore.Groups
{
    public interface IGroup : IElement
    {
        IEnumerable<IElement> Children { get; }
        int ChildCount { get; }
        IUnits Spacing { get; set; }

        void AddChild(IElement element);
        IElement GetChildAt(int index);
        IGroup Duplicate();
    }
}
