using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Materials
{
    public class MaterialBuffer
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

        public void Bind()
        {
            
        }

        public void Buffer()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * _materials.Count, _materials.ToArray(), BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            int blockIndex = GL.GetUniformBlockIndex(_handle, "MaterialBlock");
            GL.UniformBlockBinding(_handle, blockIndex, 0);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, _handle);
        }

        public void Set()
        {
            int location = _program.GetUniformLocation(_name);
            GL.UniformBlockBinding(location, 0, 0);
        }
    }
}
