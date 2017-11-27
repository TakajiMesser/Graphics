using Graphics.Mesh;
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
        public static int SizeOf<T>() where T : struct
        {
            return Marshal.SizeOf<T>();
        }

        private static List<VertexAttribute> _colorVertex3Attributes;
        private static List<VertexAttribute> _meshVertexAttributes;

        public static IEnumerable<VertexAttribute> GetAttributes<T>() where T : struct
        {
            Type type = typeof(T);

            if (type == typeof(ColorVertex3))
            {
                return ColorVertex3Attributes;
            }
            else if (type == typeof(MeshVertex))
            {
                return MeshVertexAttributes;
            }

            throw new NotImplementedException("Could not get attributes for type " + nameof(T));
        }

        public static IEnumerable<VertexAttribute> ColorVertex3Attributes => _colorVertex3Attributes
            ?? (_colorVertex3Attributes = new List<VertexAttribute>()
            {
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, SizeOf<ColorVertex3>(), 0),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, SizeOf<ColorVertex3>(), 12)
            });

        public static IEnumerable<VertexAttribute> MeshVertexAttributes => _meshVertexAttributes
            ?? (_meshVertexAttributes = new List<VertexAttribute>()
            {
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, SizeOf<MeshVertex>(), 0),
                new VertexAttribute("vNormal", 3, VertexAttribPointerType.Float, SizeOf<MeshVertex>(), 12),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, SizeOf<MeshVertex>(), 24)
            });
    }
}
