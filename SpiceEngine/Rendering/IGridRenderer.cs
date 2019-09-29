using OpenTK.Graphics;

namespace SpiceEngine.Rendering
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
