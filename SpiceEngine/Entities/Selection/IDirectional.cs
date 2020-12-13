using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngine.Entities.Selection
{
    public interface IDirectional
    {
        Vector3 XDirection { get; }
        Vector3 YDirection { get; }
        Vector3 ZDirection { get; }
    }
}
