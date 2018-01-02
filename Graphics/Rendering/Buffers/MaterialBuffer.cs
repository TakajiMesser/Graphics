using Graphics.Materials;
using Graphics.Rendering.Shaders;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Buffers
{
    public class MaterialBuffer : IDisposable, IBindable
    {
        private readonly int _handle;
        private readonly int _size;
        private readonly string _name;
        private readonly ShaderProgram _program;

        private Dictionary<string, int> _indexByName = new Dictionary<string, int>();

        private List<Material> _materials = new List<Material>();
        public List<Material> Materials => _materials;

        public MaterialBuffer(string name, ShaderProgram program)
        {
            _handle = GL.GenBuffer();
            _size = Marshal.SizeOf<Material>();
            _name = name;
            _program = program;
        }

        public void AddMaterial(Material material)
        {
            _materials.Add(material);
        }

        public void AddMaterials(IEnumerable<Material> materials)
        {
            _materials.AddRange(materials);
        }

        public void Clear()
        {
            _materials.Clear();
        }

        public void Buffer()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * _materials.Count, _materials.ToArray(), BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            _program.BindUniformBlock("MaterialBlock", 1);
            //GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 1, _handle, 0, _size * _materials.Count);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 1, _handle);
        }

        public void Bind()
        {
            
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
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

                GL.DeleteBuffer(_handle);
                disposedValue = true;
            }
        }

        ~MaterialBuffer()
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
