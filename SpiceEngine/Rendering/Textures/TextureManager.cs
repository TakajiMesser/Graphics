using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Textures
{
    public class TextureManager : IDisposable
    {
        private Dictionary<string, int> _pathsByID = new Dictionary<string, int>(); 
        private List<Texture> _textures = new List<Texture>();

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
            _textures.Add(texture);
            return _textures.Count;
        }

        public int AddTexture(string texturePath)
        {
            if (_pathsByID.ContainsKey(texturePath))
            {
                return _pathsByID[texturePath];
            }
            else
            {
                var texture = Texture.LoadFromFile(texturePath, EnableMipMapping, EnableAnisotropy);
                if (texture != null)
                {
                    int id = AddTexture(texture);

                    _pathsByID.Add(texturePath, id);
                    return id;
                }
                else
                {
                    return 0;
                }
            }
        }

        public void Clear()
        {
            // TODO - Probably need to unbind/unload textures here...
            _pathsByID.Clear();
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
