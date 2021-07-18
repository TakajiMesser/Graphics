using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Vertices
{
    public class VertexArray<T> : OpenGLObject where T : IVertex
    {
        public VertexArray(IRenderContext renderContext) : base(renderContext) { }

        public override void Load()
        {
            base.Load();

            Bind();
            SetVertexAttributes();
            Unbind();
        }

        protected override int Create() => GL.GenVertexArray();
        protected override void Delete() => GL.DeleteVertexArray(Handle);

        public override void Bind() => GL.BindVertexArray(Handle);
        public override void Unbind() => GL.BindVertexArray(0);

        private void SetVertexAttributes()
        {
            // TODO - This should all either be hard-coded or determined at compile time, since doing this on the fly is pointlessly wasteful
            int stride = Marshal.SizeOf<T>();
            int offset = 0;

            var properties = typeof(T).GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                GL.EnableVertexAttribArray(i);

                var size = GetSize(properties[i].PropertyType);
                //IntPtr offset = Marshal.OffsetOf<T>(properties[i].Name);
                var type = GetPointerType(properties[i].PropertyType);

                GL.VertexAttribPointer(i, size, type, false, stride, (IntPtr)offset);
                offset += size * 4;
            }

            /*foreach (var attribute in VertexHelper.GetAttributes<T>())
            {
                int index = program.GetAttributeLocation(attribute.Name);
                attribute.Set(index);
            }*/
        }

        private int GetSize(Type type)
        {
            if (type == typeof(int) || type == typeof(float))
            {
                return 1;
            }
            else if (type == typeof(Vector2))
            {
                return 2;
            }
            else if (type == typeof(Vector3))
            {
                return 3;
            }
            else if (type == typeof(Vector4) || type == typeof(Color4))
            {
                return 4;
            }
            else
            {
                throw new NotImplementedException("Cannot handle property type " + type);
            }
        }

        private VertexAttribPointerType GetPointerType(Type type)
        {
            if (type == typeof(int))
            {
                return VertexAttribPointerType.Int;
            }
            else if (type == typeof(float)
                || type == typeof(Vector2)
                || type == typeof(Vector3)
                || type == typeof(Vector4)
                || type == typeof(Color4))
            {
                return VertexAttribPointerType.Float;
            }
            else
            {
                throw new NotImplementedException("Cannot handle property type " + type);
            }
        }
    }
}
