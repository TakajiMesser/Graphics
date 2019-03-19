using SpiceEngine.Game;

namespace SpiceEngine.Sounds
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
                AL.Source(_handle, ALSource3f.Position, value);
            }
        }

        public Vector3 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                AL.Source(_handle, ALSource3f.Velocity, value);
            }
        }

        public Vector3 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                AL.Source(_handle, ALSource3f.Direction, value);
            }
        }

        public ALSourceState GetState() => AL.GetSource(_handle, ALGetSourcei.SourceState);

        // TODO - determine how to structure Sound-SoundBuffer relationship
        public void Swap(SoundBuffer buffer)
        {
            AL.SourcePlay(_handle, ALSourcei.Buffer, buffer);
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
