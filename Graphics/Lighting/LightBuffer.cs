using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Lighting
{
    public class LightBuffer
    {
        private readonly int _handle;
        private readonly int _size;
        private readonly string _name;
        private readonly ShaderProgram _program;

        private Dictionary<string, int> _indexByName = new Dictionary<string, int>();

        private List<Light> _lights = new List<Light>();
        public List<Light> Lights => _lights;

        public LightBuffer(string name, ShaderProgram program)
        {
            _handle = GL.GenBuffer();
            _size = Marshal.SizeOf<Light>();
            _name = name;
            _program = program;
        }

        public void AddLight(Light light)
        {
            _lights.Add(light);
        }

        public void AddLights(IEnumerable<Light> lights)
        {
            _lights.AddRange(lights);
        }

        public void Clear()
        {
            _lights.Clear();
        }

        public void Bind()
        {

        }

        public void Buffer()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, _size * _lights.Count, _lights.ToArray(), BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            int blockIndex = GL.GetUniformBlockIndex(_handle, "LightBlock");
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
