using OpenTK;
using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Rendering.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace StarchUICore.Builders
{
    public class UIBuilder
    {
        public static Vertex3DSet<ViewVertex> Rectangle(float width, float height)
        {
            /*var meshShape = new ModelMesh();
            meshShape.Faces.Add(ModelFace.Rectangle(width, height));
            var meshBuild = new ModelBuilder(meshShape);

            var vertices = meshBuild.GetVertices().Select(v => new ViewVertex(v.Position, Color4.Aqua, Color4.PaleVioletRed)).ToList();
            var triangleIndices = meshBuild.TriangleIndices;*/

            var vertices = new List<ViewVertex>
            {
                new ViewVertex(new Vector3(0.0f, 0.0f, 0.0f), Color4.Aqua, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(0.0f, height, 0.0f), Color4.Aqua, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(width, height, 0.0f), Color4.Aqua, Color4.PaleVioletRed),
                new ViewVertex(new Vector3(width, 0.0f, 0.0f), Color4.Aqua, Color4.PaleVioletRed)
            };

            var triangleIndices = new List<int>{ 2, 1, 0, 3, 2, 0 };

            return new Vertex3DSet<ViewVertex>(vertices, triangleIndices);
        }
    }
}
