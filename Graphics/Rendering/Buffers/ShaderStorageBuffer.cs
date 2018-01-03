using Graphics.Lighting;
using Graphics.Rendering.Shaders;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Buffers
{
    public abstract class ShaderStorageBuffer<T> : IDisposable, IBindable
    {
        protected readonly int _handle;
        protected readonly int _size;
        protected readonly int _binding;

        public ShaderStorageBuffer(string name, int binding, ShaderProgram program)
        {
            _handle = GL.GenBuffer();
            _size = Marshal.SizeOf<T>();
            _binding = binding;

            program.BindShaderStorageBlock(name, binding);
        }

        public void Buffer()
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, _binding, _handle);
        }

        public abstract void Bind();
        // GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
        // GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, index, _id);
        // GL.BufferData(BufferTarget.ShaderStorageBuffer, (int)EngineHelper.size.vec2, ref default_luminosity, BufferUsageHint.DynamicCopy);
        // GL.GetBufferSubData(BufferTarget.ShaderStorageBuffer, (IntPtr)0, exp_size, ref lumRead);

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
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

                GL.DeleteBuffer(_handle);
                disposedValue = true;
            }
        }

        ~ShaderStorageBuffer()
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
