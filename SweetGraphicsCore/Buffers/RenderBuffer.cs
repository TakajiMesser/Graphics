using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using System;

namespace SweetGraphicsCore.Buffers
{
    public class RenderBuffer : OpenGLObject
    {
        public RenderBuffer(IRenderContextProvider contextProvider, RenderbufferTarget target, int width, int height) : base(contextProvider)
        {
            Handle = GL.GenRenderbuffer();

            Target = target;
            Width = width;
            Height = height;
        }

        public RenderbufferTarget Target { get; private set; }
        public InternalFormat Storage { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected override int Create() => GL.GenRenderbuffer();
        protected override void Delete() => GL.DeleteRenderbuffer(Handle);

        public override void Bind() => GL.BindRenderbuffer(Target, Handle);
        public override void Unbind() => GL.BindRenderbuffer(Target, 0);

        public void Load(IntPtr pixels)
        {
            //Specify(pixels);
            //SetTextureParameters();
        }

        public void ReserveMemory() => GL.RenderbufferStorage(Target, Storage, Width, Height);
    }
}
