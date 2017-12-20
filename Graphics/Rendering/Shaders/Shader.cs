using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Graphics.Rendering.Shaders
{
    public class Shader : IDisposable
    {
        public int Handle { get; private set; }
        public ShaderType ShaderType { get; private set; }

        public Shader(ShaderType type, string code)
        {
            ShaderType = type;
            Handle = GL.CreateShader(type);

            GL.ShaderSource(Handle, code);
            GL.CompileShader(Handle);

            GL.GetShader(Handle, ShaderParameter.CompileStatus, out int statusCode);
            if (statusCode != 1)
            {
                throw new ApplicationException("Shader failed to compile: " + GL.GetShaderInfoLog(Handle));
            }
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

                GL.DeleteShader(Handle);
                disposedValue = true;
            }
        }

        ~Shader() {
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
