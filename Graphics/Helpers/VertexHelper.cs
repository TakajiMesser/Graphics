using Graphics.Meshes;
using Graphics.Rendering.Vertices;
using Graphics.Utilities;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Helpers
{
    public static class VertexHelper
    {
        private static List<VertexAttribute> _vertexAttributes;

        public static IEnumerable<VertexAttribute> VertexAttributes => _vertexAttributes
            ?? (_vertexAttributes = new List<VertexAttribute>()
            {
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex>(), 0),
                new VertexAttribute("vNormal", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex>(), 12),
                new VertexAttribute("vTangent", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex>(), 24),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex>(), 36),
                new VertexAttribute("vUV", 2, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex>(), 52),
                new VertexAttribute("vMaterialIndex", 1, VertexAttribPointerType.Int, UnitConversions.SizeOf<Vertex>(), 60)
            });

        public static IEnumerable<VertexAttribute> GetAttributes<T>() where T : struct
        {
            Type type = typeof(T);

            if (type == typeof(Vertex))
            {
                return VertexAttributes;
            }

            throw new NotImplementedException("Could not get attributes for type " + nameof(T));
        }
    }
}
