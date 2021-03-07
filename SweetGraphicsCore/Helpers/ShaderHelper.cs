using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Shaders;
using System;

namespace SweetGraphicsCore.Helpers
{
    public static class ShaderHelper
    {
        public static ShaderProgram LoadProgram(IRenderContextProvider contextProvider, ShaderType[] shaderTypes, string[] shaderCodes)
        {
            if (shaderTypes.Length != shaderCodes.Length) throw new ArgumentException("Number of shader types and code do not match");
            
            var program = new ShaderProgram(contextProvider);
            var shaders = new Shader[shaderTypes.Length];

            for (var i = 0; i < shaderTypes.Length; i++)
            {
                shaders[i] = new Shader(contextProvider, shaderTypes[i]);
                shaders[i].Load(shaderCodes[i]);
            }

            program.Load(shaders);
            return program;
        }

        public static bool CompileShader(Shader shader, string code, out string errorLog)
        {
            GL.ShaderSource(shader.Handle, 1, new string[] { code }, new int[] { code.Length });
            GL.CompileShader(shader.Handle);

            var statusCode = GL.GetShaderiv(shader.Handle, ShaderParameterName.CompileStatus)[0];
            if (statusCode != 1)
            {
                errorLog = string.Empty;

                var infoLogLengths = GL.GetShaderiv(shader.Handle, ShaderParameterName.InfoLogLength);
                if (infoLogLengths.Length > 0 && infoLogLengths[0] > 0)
                {
                    GL.GetShaderInfoLog(shader.Handle, infoLogLengths[0] * 2, infoLogLengths, errorLog);
                }

                return false;
            }

            errorLog = null;
            return true;
        }
    }
}
