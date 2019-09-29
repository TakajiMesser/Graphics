using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using System;

namespace SpiceEngineCore.Sounds
{
    public class SoundBuffer : IDisposable
    {
        public int Handle { get; }

        public SoundBuffer() => Handle = AL.GenBuffer();

        public void Buffer(Sound sound)
        {
            AL.BufferData(Handle, sound.Format, sound.Data, sound.Data.Length, sound.SampleRate);
            var error = AL.GetError();
            if (error > 0)
            {
                throw new AudioException("Sound " + sound.ID + " failed to buffer: " + error.ToString());
            }

            //GL.BufferData(BufferTarget.ArrayBuffer, _vertexSize * _vertices.Count, _vertices.ToArray(), BufferUsageHint.StreamDraw);
            /*var handle = GCHandle.Alloc(_vertices.ToArray(), GCHandleType.Pinned);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexSize * _vertices.Count, handle.AddrOfPinnedObject(), BufferUsageHint.StreamDraw);
            handle.Free();*/
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

                AL.DeleteBuffer(Handle);
                disposedValue = true;
            }
        }

        ~SoundBuffer()
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
