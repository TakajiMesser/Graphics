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
    public class LightBuffer : IDisposable, IBindable
    {
        private readonly int _handle;
        private readonly int _size;
        private readonly string _name;
        private readonly ShaderProgram _program;

        private List<Light> _lights = new List<Light>();

        public LightBuffer(string name, ShaderProgram program)
        {
            _handle = GL.GenBuffer();
            _size = Marshal.SizeOf<Light>();
            _name = name;
            _program = program;
        }

        public void AddLight(Light light)
        {
            _lights.Add(light);
        }

        public void AddLights(IEnumerable<Light> lights)
        {
            _lights.AddRange(lights);
        }

        public void Clear()
        {
            _lights.Clear();
        }

        public void Buffer()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * _lights.Count, _lights.ToArray(), BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            _program.BindUniformBlock("LightBlock", 0);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, _handle);
        }

        public void Bind()
        {
            
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
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

        ~LightBuffer()
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
