using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Shaders;
using System.Runtime.InteropServices;

namespace SweetGraphicsCore.Buffers
{
    public abstract class ShaderStorageBuffer<T> : OpenGLObject
    {
        protected readonly int _size;

        public ShaderStorageBuffer(IRenderContext renderContext, string name, int binding) : base(renderContext)
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
            shaderProgram.BindShaderStorageBlock(Name, Binding);
        }

        protected override int Create() => GL.GenBuffer();
        protected override void Delete() => GL.DeleteBuffer(Handle);

        public override void Bind() => GL.BindBuffer(BufferTargetARB.ShaderStorageBuffer, Handle);
        public override void Unbind() => GL.BindBuffer(BufferTargetARB.ShaderStorageBuffer, 0);

        public void Buffer() => GL.BindBufferBase(BufferTargetARB.ShaderStorageBuffer, Binding, Handle);
        // GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
        // GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, index, _id);
        // GL.BufferData(BufferTarget.ShaderStorageBuffer, (int)EngineHelper.size.vec2, ref default_luminosity, BufferUsageHint.DynamicCopy);
        // GL.GetBufferSubData(BufferTarget.ShaderStorageBuffer, (IntPtr)0, exp_size, ref lumRead);
    }
}
