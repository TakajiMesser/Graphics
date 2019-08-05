using OpenTK;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public interface ITexturedShape
    {
        void Rotate(Quaternion rotation);
        void Scale(Vector3 scale);

        void TranslateTexture(float x, float y);
        void RotateTexture(float angle);
        void ScaleTexture(float x, float y);
    }
}
