using OpenTK;
using SpiceEngine.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Entities.Selection
{
    public interface IDirectional
    {
        Vector3 XDirection { get; }
        Vector3 YDirection { get; }
        Vector3 ZDirection { get; }
    }
}
