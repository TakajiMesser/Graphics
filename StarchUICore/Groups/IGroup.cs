using StarchUICore.Attributes.Units;
using System.Collections.Generic;

namespace StarchUICore.Groups
{
    public interface IGroup : IElement
    {
        IEnumerable<IElement> Children { get; }
        IUnits Spacing { get; set; }

        IGroup Duplicate();
    }
}
