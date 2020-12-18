using SpiceEngineCore.Geometry.Colors;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering.Matrices;
using SweetGraphicsCore.Vertices;
using System.Runtime.InteropServices;

namespace StarchUICore.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ViewVertex : IVertex3D, IColorVertex, ISelectionVertex
    {
        public Vector3 Position { get; private set; }
        public Color4 Color { get; private set; }
        public Color4 IDColor { get; private set; }

        public ViewVertex(Vector3 position, Color4 color, Color4 idColor)
        {
            Position = position;
            Color = color;
            IDColor = idColor;
        }

        public ViewVertex(IVertex3D vertex, Color4 idColor)
        {
            Position = vertex.Position;
            Color = vertex is IColorVertex colorVertex ? colorVertex.Color : new Color4();
            IDColor = idColor;
        }

        public IVertex3D Transformed(Transform transform)
        {
            var modelMatrix = transform.ToMatrix();

            return new ViewVertex()
            {
                Position = (new Vector4(Position, 1.0f) * modelMatrix).Xyz,
                Color = Color,
                IDColor = IDColor
            };
        }

        public ISelectionVertex Selected() => new ViewVertex()
        {
            Position = Position,
            Color = Color,
            IDColor = new Color4(IDColor.R, IDColor.G, IDColor.B, 0.5f)
        };

        public ISelectionVertex Deselected() => new ViewVertex()
        {
            Position = Position,
            Color = Color,
            IDColor = new Color4(IDColor.R, IDColor.G, IDColor.B, 1.0f)
        };

        public IColorVertex Colored(Color4 color) => new ViewVertex()
        {
            Position = Position,
            Color = color,
            IDColor = IDColor
        };
    }
}
