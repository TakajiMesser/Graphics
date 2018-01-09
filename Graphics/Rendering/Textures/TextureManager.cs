using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Graphics.Rendering.Buffers;
using OpenTK.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Graphics.Rendering.Textures
{
    public class TextureManager : IDisposable
    {
        private Dictionary<string, int> _pathsByID = new Dictionary<string, int>(); 
        private List<Texture> _textures = new List<Texture>();

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
                var texture = Texture.LoadFromFile(texturePath);
                int id = AddTexture(texture);

                _pathsByID.Add(texturePath, id);
                return id;
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
