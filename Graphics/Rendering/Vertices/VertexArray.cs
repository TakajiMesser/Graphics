using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using Graphics.Helpers;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Graphics.Rendering.Vertices
{
    public class VertexArray<T> : IDisposable, IBindable where T : struct
    {
        private readonly int _handle;
        private bool _generated = false;

        public int Handle => _handle;

        public VertexArray()
        {
            if (_generated)
            {
                GL.DeleteVertexArray(_handle);
            }

            _handle = GL.GenVertexArray();
            _generated = true;
        }

        public void Load(ShaderProgram program)
        {
            Bind();
            SetVertexAttributes(program);
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

        private void SetVertexAttributes(ShaderProgram program)
        {
            // TODO - This should all either be hard-coded or determined at compile time, since doing this on the fly is pointlessly wasteful
            int stride = Marshal.SizeOf<T>();

            // TODO - Replace vertex fields with properties, since struct fields are janky
            //var properties = typeof(T).GetProperties(/*BindingFlags.Public*/);
            var fields = typeof(T).GetFields();

            for (var i = 0; i < fields.Length; i++)
            {
                GL.EnableVertexAttribArray(i);

                int size = GetSize(fields[i].FieldType);
                IntPtr offset = Marshal.OffsetOf<T>(fields[i].Name);
                VertexAttribPointerType type = GetPointerType(fields[i].FieldType);

                GL.VertexAttribPointer(i, size, type, false, stride, offset);
            }

            /*foreach (var attribute in VertexHelper.GetAttributes<T>())
            {
                int index = program.GetAttributeLocation(attribute.Name);
                attribute.Set(index);
            }*/
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
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

                GL.DeleteVertexArray(_handle);
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
