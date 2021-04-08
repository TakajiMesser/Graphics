using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Shaders;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Buffers
{
    public abstract class UniformBuffer<T> : OpenGLObject
    {
        protected readonly int _size;

        public UniformBuffer(IRenderContext renderContext, string name, int binding) : base(renderContext)
        {
            Name = name;
            Binding = binding;
            _size = Marshal.SizeOf<T>();
        }

        public string Name { get; }
        public int Binding { get; }

        public void Load(ShaderProgram shaderProgram)
        {
            base.Load();
            shaderProgram.BindUniformBlock(Name, Binding);
        }

        protected override int Create() => GL.GenBuffer();
        protected override void Delete() => GL.DeleteBuffer(Handle);

        public override void Bind() => GL.BindBuffer(BufferTargetARB.UniformBuffer, Handle);
        public override void Unbind() => GL.BindBuffer(BufferTargetARB.UniformBuffer, 0);

        public virtual void Buffer() => GL.BindBufferBase(BufferTargetARB.UniformBuffer, Binding, Handle);
    }
}
