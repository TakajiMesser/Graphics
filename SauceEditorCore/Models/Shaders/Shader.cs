using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Shaders
{
    public enum ShaderStages
    {
        Vertex,
        TessellationControl,
        Geometry,
        TesselationEvaluation,
        Fragment
    }

    public class Shader
    {
        public const string MAIN_FUNCTION_NAME = "main";
        public const string NEW_LINE = "\n\r";

        public Shader()
        {
            Functions.Add(new Function(MAIN_FUNCTION_NAME, FunctionTypes.Void));
        }

        public string Name { get; set; }
        public ShaderStages ShaderStage { get; set; }
        public int Version { get; set; }

        public List<Variable> Uniforms { get; } = new List<Variable>();
        public List<Variable> Inputs { get; } = new List<Variable>();
        public List<Variable> Outputs { get; } = new List<Variable>();
        public List<Function> Functions { get; } = new List<Function>();

        public string GetText()
        {
            var builder = new StringBuilder();

            builder.Append("#version " + Version + NEW_LINE);
            builder.Append(NEW_LINE);

            foreach (var uniform in Uniforms)
            {
                builder.Append(uniform.GetText() + NEW_LINE);
            }

            builder.Append(NEW_LINE);

            foreach (var input in Inputs)
            {
                builder.Append(input.GetText() + NEW_LINE);
            }

            builder.Append(NEW_LINE);

            foreach (var output in Outputs)
            {
                builder.Append(output.GetText() + NEW_LINE);
            }

            builder.Append(NEW_LINE);

            foreach (var function in Functions)
            {
                builder.Append(function.GetText() + NEW_LINE);
                builder.Append(NEW_LINE);
            }

            return builder.ToString();
        }
    }
}
