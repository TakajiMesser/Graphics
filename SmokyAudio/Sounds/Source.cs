using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using System;

/*using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;*/

namespace SmokyAudioCore.Sounds
{
    public class Source : IDisposable
    {
        private readonly int _handle;

        private Vector3 _position;
        private Vector3 _velocity;
        private Vector3 _direction;

        public Source()
        {
            _handle = AL.GenSource();
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                AL.Source(_handle, ALSource3f.Position, ref _position);
            }
        }

        public Vector3 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                AL.Source(_handle, ALSource3f.Velocity, ref _velocity);
            }
        }

        public Vector3 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                AL.Source(_handle, ALSource3f.Direction, ref _direction);
            }
        }

        public ALSourceState GetState() => AL.GetSourceState(_handle);

        // TODO - determine how to structure Sound-SoundBuffer relationship
        public void Swap(SoundBuffer buffer)
        {
            // TODO - Determine if we need to unqueue the previous buffer first
            AL.SourceQueueBuffer(_handle, buffer.Handle);
        }

        public void Play()
        {
            AL.SourcePlay(_handle);
        }

        public void Pause()
        {
            AL.SourcePause(_handle);
        }

        public void Stop()
        {
            AL.SourceStop(_handle);
        }

        public void Reset()
        {
            AL.SourceRewind(_handle);
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

                AL.DeleteSource(_handle);
                disposedValue = true;
            }
        }

        ~Source()
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
