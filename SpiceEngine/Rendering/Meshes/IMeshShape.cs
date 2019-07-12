using OpenTK;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public interface IMeshShape
    {
        Vector3 GetAveragePosition();
        void CenterAround(Vector3 position);
    }
}
