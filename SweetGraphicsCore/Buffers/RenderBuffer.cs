using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Buffers
{
    public class RenderBuffer : OpenGLObject
    {
        public RenderBuffer(IRenderContextProvider contextProvider, int width, int height, RenderbufferTarget target = RenderbufferTarget.Renderbuffer, InternalFormat format = InternalFormat.Rgba) : base(contextProvider)
        {
            Width = width;
            Height = height;
            Target = target;
            Format = format;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public RenderbufferTarget Target { get; private set; }
        public InternalFormat Format { get; set; }

        public override void Load()
        {
            base.Load();

            Bind();
            ReserveMemory();
        }

        protected override int Create() => GL.GenRenderbuffer();
        protected override void Delete() => GL.DeleteRenderbuffer(Handle);

        public override void Bind() => GL.BindRenderbuffer(Target, Handle);
        public override void Unbind() => GL.BindRenderbuffer(Target, 0);

        public void ReserveMemory() => GL.RenderbufferStorage(Target, Format, Width, Height);

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;

            Bind();
            ReserveMemory();
        }
    }
}
