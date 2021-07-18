using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Helpers;
using System;

namespace SweetGraphicsCore.Shaders
{
    public class Shader : OpenGLObject, IDisposable
    {
        public Shader(IRenderContext renderContext, ShaderType type) : base(renderContext) => ShaderType = type;

        public ShaderType ShaderType { get; private set; }

        public void Load(string code)
        {
            base.Load();

            if (!ShaderHelper.CompileShader(this, code, out string errorLog))
            {
                Unload();
                throw new Exception(ShaderType + " Shader failed to compile: " + errorLog);
            }
        }

        protected override int Create() => GL.CreateShader(ShaderType);
        protected override void Delete() => GL.DeleteShader(Handle);

        public override void Bind() { }
        public override void Unbind() { }
    }
}
