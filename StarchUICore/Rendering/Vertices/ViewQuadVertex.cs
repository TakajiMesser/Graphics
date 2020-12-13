using SpiceEngineCore.Rendering.Matrices;
using SweetGraphicsCore.Vertices;
using System.Runtime.InteropServices;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace StarchUICore.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ViewQuadVertex : IVertex3D, IColorVertex, ISelectionVertex
    {
        public Vector3 Position { get; private set; }
        public float BorderThickness { get; private set; }
        public Vector2 Size { get; private set; }
        public Vector2 CornerRadius { get; private set; }
        public Color4 Color { get; private set; }
        public Color4 BorderColor { get; private set; }
        public Color4 IDColor { get; private set; }

        public ViewQuadVertex(Vector3 position, float borderThickness, Vector2 size, Vector2 cornerRadius, Color4 color, Color4 borderColor, Color4 idColor)
        {
            Position = position;
            BorderThickness = borderThickness;
            Size = size;
            CornerRadius = cornerRadius;
            Color = color;
            BorderColor = borderColor;
            IDColor = idColor;
        }

        /*public ViewQuadVertex(IVertex3D vertex, Color4 selectionID)
        {
            Position = vertex.Position;
            Color = vertex is IColorVertex colorVertex ? colorVertex.Color : new Color4();
            SelectionID = selectionID;
        }*/

        public IVertex3D Transformed(Transform transform)
        {
            var modelMatrix = transform.ToMatrix();

            return new ViewQuadVertex()
            {
                Position = (new Vector4(Position, 1.0f) * modelMatrix).Xyz,
                Size = Size,
                CornerRadius = CornerRadius,
                Color = Color,
                IDColor = IDColor
            };
        }

        public ISelectionVertex Selected() => new ViewQuadVertex()
        {
            Position = Position,
            Size = Size,
            CornerRadius = CornerRadius,
            Color = Color,
            IDColor = new Color4(IDColor.R, IDColor.G, IDColor.B, 0.5f)
        };

        public ISelectionVertex Deselected() => new ViewQuadVertex()
        {
            Position = Position,
            Size = Size,
            CornerRadius = CornerRadius,
            Color = Color,
            IDColor = new Color4(IDColor.R, IDColor.G, IDColor.B, 1.0f)
        };

        public IColorVertex Colored(Color4 color) => new ViewQuadVertex()
        {
            Position = Position,
            Size = Size,
            CornerRadius = CornerRadius,
            Color = color,
            IDColor = IDColor
        };
    }
}
