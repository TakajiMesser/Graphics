using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Vertices;
using System;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public class ColoredMesh<T> : Mesh<T>, IColoredMesh, IDisposable where T : IVertex3D, IColorVertex
    {
        private static Color4 DEFAULT_COLOR = new Color4(0.2f, 0.2f, 0.2f, 0.5f);

        private Color4 _color;

        public Color4 Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    if (value.A >= 1.0f) throw new ArgumentOutOfRangeException("Alpha value of color must be less than 1.0");
                    Update(v => (IVertex3D)((IColorVertex)v).Colored(value));

                    _color = value;
                    ColorChanged?.Invoke(this, new ColorEventArgs(value));
                }
            }
        }

        public event EventHandler<ColorEventArgs> ColorChanged;

        public ColoredMesh(Vertex3DSet<T> vertexSet) : base(vertexSet.Updated(v => (T)((IColorVertex)v).Colored(DEFAULT_COLOR)))
        {
            // TODO - Fix this shit...
            Alpha = 1.0f;
            _color = DEFAULT_COLOR;
        }
    }
}
