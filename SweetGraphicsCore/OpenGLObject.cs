using SpiceEngine.GLFWBindings;
using SpiceEngineCore.Rendering;
using System;

namespace SweetGraphicsCore
{
    public enum GLObjectStates
    {
        None,
        Created,
        Deleted
    }

    public abstract class OpenGLObject : IDisposable
    {
        private IRenderContextProvider _contextProvider;

        public OpenGLObject(IRenderContextProvider contextProvider) => _contextProvider = contextProvider;

        public int Handle { get; protected set; }
        public GLObjectStates State { get; protected set; }

        public virtual void Load()
        {
            if (State != GLObjectStates.Created)
            {
                Handle = Create();
                State = GLObjectStates.Created;

                if (Handle == 0)
                {
                    throw new Exception("Failed to generate texture: " + GL.GetError());//GL.GetShaderInfoLog(_handle));
                }
            }
        }

        public virtual void Unload()
        {
            if (State == GLObjectStates.Created)
            {
                Delete();
                State = GLObjectStates.Deleted;
            }
        }

        protected abstract int Create();
        protected abstract void Delete();

        public abstract void Bind();
        public abstract void Unbind();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && _contextProvider.CurrentContext != null && !_contextProvider.CurrentContext.IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                //Unload();
                disposedValue = true;
            }
        }

        ~OpenGLObject()
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
