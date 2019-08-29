using OpenTK.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Textures
{
    public class TextureManager : ITextureProvider, IDisposable
    {
        private ConcurrentDictionary<string, int> _idByPath = new ConcurrentDictionary<string, int>(); 
        private List<Texture> _textures = new List<Texture>();

        private object _textureLock = new object();

        public bool EnableMipMapping { get; set; } = true;
        public bool EnableAnisotropy { get; set; } = true;

        public TextureManager() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <returns>Returns a lookup ID for this texture</returns>
        public int AddTexture(Texture texture)
        {
            lock (_textureLock)
            {
                _textures.Add(texture);
                return _textures.Count;
            }
        }

        public int AddTexture(string texturePath)
        {
            return _idByPath.GetOrAdd(texturePath, p =>
            {
                var texture = Texture.LoadFromFile(texturePath, EnableMipMapping, EnableAnisotropy);
                if (texture != null)
                {
                    return AddTexture(texture);
                }

                return 0;
            });

            /*if (_idByPath.ContainsKey(texturePath))
            {
                return _idByPath[texturePath];
            }
            else
            {
                var texture = Texture.LoadFromFile(texturePath, EnableMipMapping, EnableAnisotropy);
                if (texture != null)
                {
                    int id = AddTexture(texture);

                    _idByPath.Add(texturePath, id);
                    return id;
                }
                else
                {
                    return 0;
                }
            }*/
        }

        public void Clear()
        {
            // TODO - Probably need to unbind/unload textures here...
            _idByPath.Clear();
            _textures.Clear();
        }

        public Texture RetrieveTexture(int id) => (id > 0 && id <= _textures.Count) ? _textures[id - 1] : null;

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

                //GL.DeleteTexture(_handle);
                disposedValue = true;
            }
        }

        ~TextureManager()
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
