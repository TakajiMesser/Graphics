using OpenTK;
using OpenTK.Graphics;
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
        public Color4 SelectionID { get; private set; }

        public ViewVertex(Vector3 position, Color4 color, Color4 selectionID)
        {
            Position = position;
            Color = color;
            SelectionID = selectionID;
        }

        public ViewVertex(IVertex3D vertex, Color4 selectionID)
        {
            Position = vertex.Position;
            Color = vertex is IColorVertex colorVertex ? colorVertex.Color : new Color4();
            SelectionID = selectionID;
        }

        public IVertex3D Transformed(Transform transform)
        {
            var modelMatrix = transform.ToMatrix();

            return new ViewVertex()
            {
                Position = (new Vector4(Position, 1.0f) * modelMatrix).Xyz,
                Color = Color,
                SelectionID = SelectionID
            };
        }

        public ISelectionVertex Selected() => new ViewVertex()
        {
            Position = Position,
            Color = Color,
            SelectionID = new Color4(SelectionID.R, SelectionID.G, SelectionID.B, 0.5f)
        };

        public ISelectionVertex Deselected() => new ViewVertex()
        {
            Position = Position,
            Color = Color,
            SelectionID = new Color4(SelectionID.R, SelectionID.G, SelectionID.B, 1.0f)
        };

        public IColorVertex Colored(Color4 color) => new ViewVertex()
        {
            Position = Position,
            Color = color,
            SelectionID = SelectionID
        };
    }
}
