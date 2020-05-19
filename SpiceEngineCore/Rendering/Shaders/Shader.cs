using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace SpiceEngineCore.Rendering.Shaders
{
    public class Shader : IDisposable
    {
        internal int _handle;
        public ShaderType ShaderType { get; private set; }

        public Shader(ShaderType type, string code)
        {
            ShaderType = type;
            _handle = GL.CreateShader(type);

            GL.ShaderSource(_handle, code);
            GL.CompileShader(_handle);

            GL.GetShader(_handle, ShaderParameter.CompileStatus, out int statusCode);
            if (statusCode != 1)
            {
                throw new GraphicsException(ShaderType + " Shader failed to compile: " + GL.GetShaderInfoLog(_handle));
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

                GL.DeleteShader(_handle);
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
