using SpiceEngineCore.Rendering;
using System;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public interface IColoredMesh : IMesh
    {
        Color4 Color { get; set; }

        event EventHandler<ColorEventArgs> ColorChanged;
    }
}
