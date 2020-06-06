using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using SpiceEngineCore.Game;
using System;
using System.Collections.Generic;

namespace SmokyAudioCore.Sounds
{
    public class SoundManager : UpdateManager, IDisposable
    {
        private IntPtr _device = IntPtr.Zero;
        private ContextHandle _context;

        private Listener _listener;

        private Dictionary<int, Sound> _soundByID = new Dictionary<int, Sound>();
        private Dictionary<int, SoundBuffer> _bufferByID = new Dictionary<int, SoundBuffer>();
        private int _nextAvailableID = 1;

        public SoundManager()
        {
            _device = Alc.OpenDevice(null);
            _context = Alc.CreateContext(_device, (int[])null);

            Alc.MakeContextCurrent(_context);

            // Create a listener
            _listener = new Listener();
        }

        public void AddSounds(IEnumerable<Sound> sounds)
        {
            foreach (var sound in sounds)
            {
                AddSound(sound);
            }
        }

        public int AddSound(Sound sound)
        {
            // Assign a unique ID
            if (sound.ID == 0)
            {
                int id = GetUniqueID();
                _soundByID.Add(id, sound);
                sound.ID = id;
            }

            return sound.ID;
        }

        public void Load()
        {
            // For now, just give each sound its own buffer
            foreach (var sound in _soundByID.Values)
            {
                var buffer = new SoundBuffer();
                buffer.Buffer(sound);

                _bufferByID.Add(sound.ID, buffer);
            }
        }

        public void PlaySound(int soundID)
        {
            var buffer = _bufferByID[soundID];
            var source = new Source();

            // Default to playing the sound at the listener's location
            //source.Position;
        }

        protected override void Update()
        {

        }

        private int GetUniqueID()
        {
            int id = _nextAvailableID;
            _nextAvailableID++;

            return id;
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

                if (_context != null && _context != ContextHandle.Zero)
                {
                    Alc.MakeContextCurrent(ContextHandle.Zero);
                    Alc.DestroyContext(_context);
                    _context = ContextHandle.Zero;
                }

                if (_device != IntPtr.Zero)
                {
                    Alc.CloseDevice(_device);
                }

                disposedValue = true;
            }
        }

        ~SoundManager()
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
