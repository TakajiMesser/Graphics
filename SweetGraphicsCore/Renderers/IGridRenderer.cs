using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Renderers
{
    public interface IGridRenderer
    {
        bool RenderGrid { get; set; }

        void RotateGrid(float pitch, float yaw, float roll);
        void SetGridUnit(float unit);
        void SetGridLineThickness(float thickness);
        void SetGridUnitColor(Color4 color);
        void SetGridAxisColor(Color4 color);
        void SetGrid5Color(Color4 color);
        void SetGrid10Color(Color4 color);
    }
}
