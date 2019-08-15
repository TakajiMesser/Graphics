using OpenTK;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public interface IModelShape
    {
        Vector3 GetAveragePosition();
        void CenterAround(Vector3 position);

        void Transform(Transform transform);
        //void Translate(Vector3 translation);
        //void Rotate(Quaternion rotation);
        //void Scale(Vector3 scale);
    }
}
