﻿using Graphics.Lighting;
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
    public class ShaderStorageBuffer : IDisposable, IBindable
    {
        private readonly int _handle;
        private readonly int _size;

        private List<Light> _lights = new List<Light>();

        public ShaderStorageBuffer(ShaderProgram program)
        {
            _handle = GL.GenBuffer();

            // Size of SSBO is more complicated...
            // _size = Marshal.SizeOf<Light>();
        }

        public void Buffer()
        {
            //GL.BufferData(BufferRangeTarget.ShaderStorageBuffer, _size, (IntPtr)0, _handle);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
        }

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
