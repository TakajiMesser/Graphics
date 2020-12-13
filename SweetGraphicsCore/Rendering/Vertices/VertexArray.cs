using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering.Vertices;
using SweetGraphicsCore.Rendering;
using System;
using System.Runtime.InteropServices;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Vertices
{
    public class VertexArray<T> : IDisposable, IBindable where T : IVertex
    {
        private bool _generated = false;

        public int Handle { get; }

        public VertexArray()
        {
            if (_generated)
            {
                GL.DeleteVertexArray(Handle);
            }

            Handle = GL.GenVertexArray();
            _generated = true;
        }

        public void Load()
        {
            Bind();
            SetVertexAttributes();
            Unbind();
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

        private void SetVertexAttributes(/*ShaderProgram program*/)
        {
            // TODO - This should all either be hard-coded or determined at compile time, since doing this on the fly is pointlessly wasteful
            int stride = Marshal.SizeOf<T>();
            int offset = 0;

            var properties = typeof(T).GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                GL.EnableVertexAttribArray(i);

                int size = GetSize(properties[i].PropertyType);
                //IntPtr offset = Marshal.OffsetOf<T>(properties[i].Name);
                VertexAttribPointerType type = GetPointerType(properties[i].PropertyType);

                GL.VertexAttribPointer(i, size, type, false, stride, offset);
                offset += size * 4;
            }

            /*foreach (var attribute in VertexHelper.GetAttributes<T>())
            {
                int index = program.GetAttributeLocation(attribute.Name);
                attribute.Set(index);
            }*/
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && GraphicsContext.CurrentContext != null && !GraphicsContext.CurrentContext.IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                GL.DeleteVertexArray(Handle);
                disposedValue = true;
            }
        }

        ~VertexArray()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
