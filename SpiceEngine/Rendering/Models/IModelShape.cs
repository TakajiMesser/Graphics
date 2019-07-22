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
        void Translate(float x, float y, float z);

        void TranslateTexture(float x, float y);
        void RotateTexture(float angle);
        void ScaleTexture(float x, float y);
    }
}
