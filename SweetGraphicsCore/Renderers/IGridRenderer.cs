using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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
