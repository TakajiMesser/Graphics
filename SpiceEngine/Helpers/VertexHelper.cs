using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;

namespace SpiceEngine.Helpers
{
    public static class VertexHelper
    {
        private static List<VertexAttribute> _vertexAttributes;
        private static List<VertexAttribute> _jointVertexAttributes;

        public static IEnumerable<VertexAttribute> VertexAttributes => _vertexAttributes
            ?? (_vertexAttributes = new List<VertexAttribute>()
            {
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex3D>(), 0),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex3D>(), 12),
                new VertexAttribute("vNormal", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex3D>(), 28),
                new VertexAttribute("vTangent", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex3D>(), 40),
                new VertexAttribute("vUV", 2, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vertex3D>(), 52),
                //new VertexAttribute("vMaterialIndex", 1, VertexAttribPointerType.Int, UnitConversions.SizeOf<Vertex>(), 60)
            });

        public static IEnumerable<VertexAttribute> JointVertexAttributes => _jointVertexAttributes
            ?? (_jointVertexAttributes = new List<VertexAttribute>()
            {
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<JointVertex3D>(), 0),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, UnitConversions.SizeOf<JointVertex3D>(), 12),
                new VertexAttribute("vNormal", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<JointVertex3D>(), 28),
                new VertexAttribute("vTangent", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<JointVertex3D>(), 40),
                new VertexAttribute("vUV", 2, VertexAttribPointerType.Float, UnitConversions.SizeOf<JointVertex3D>(), 52),
                //new VertexAttribute("vMaterialIndex", 1, VertexAttribPointerType.Int, UnitConversions.SizeOf<JointVertex>(), 60),
                new VertexAttribute("vBoneIDs", 4, VertexAttribPointerType.Float, UnitConversions.SizeOf<JointVertex3D>(), 60),
                new VertexAttribute("vBoneWeights", 4, VertexAttribPointerType.Float, UnitConversions.SizeOf<JointVertex3D>(), 76)
            });

        public static IEnumerable<VertexAttribute> GetAttributes<T>() where T : struct
        {
            Type type = typeof(T);

            if (type == typeof(Vertex3D))
            {
                return VertexAttributes;
            }
            else if (type == typeof(JointVertex3D))
            {
                return JointVertexAttributes;
            }

            throw new NotImplementedException("Could not get attributes for type " + nameof(T));
        }
    }
}
