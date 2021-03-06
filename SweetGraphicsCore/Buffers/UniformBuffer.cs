﻿using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Rendering.Shaders;
using SweetGraphicsCore.Rendering;
using System;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Buffers
{
    public abstract class UniformBuffer<T> : IDisposable, IBindable
    {
        public string Name { get; private set; }
        protected readonly int _handle;
        protected readonly int _size;
        protected readonly int _binding;

        public UniformBuffer(string name, int binding)
        {
            Name = name;

            _handle = GL.GenBuffer();
            _size = Marshal.SizeOf<T>();
            _binding = binding;
        }

        public void Load(ShaderProgram program) => program.BindUniformBlock(Name, _binding);

        public void Buffer() => GL.BindBufferBase(BufferRangeTarget.UniformBuffer, _binding, _handle);

        public abstract void Bind();

        public void Unbind() => GL.BindBuffer(BufferTarget.UniformBuffer, 0);

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

        ~UniformBuffer()
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
