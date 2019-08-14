using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Helpers
{
    public static class ShaderHelper
    {
        public static Shader GetDeferredVertexShader()
        {
            var deferredShader = new Shader()
            {
                Name = "deferred",
                ShaderStage = ShaderStages.Vertex,
                Version = 440
            };

            deferredShader.Uniforms.Add(new Variable("modelMatrix", VariableTypes.Uniform, DataTypes.Matrix4));
            deferredShader.Uniforms.Add(new Variable("viewMatrix", VariableTypes.Uniform, DataTypes.Matrix4));
            deferredShader.Uniforms.Add(new Variable("projectionMatrix", VariableTypes.Uniform, DataTypes.Matrix4));
            deferredShader.Uniforms.Add(new Variable("previousModelMatrix", VariableTypes.Uniform, DataTypes.Matrix4));
            deferredShader.Uniforms.Add(new Variable("previousViewMatrix", VariableTypes.Uniform, DataTypes.Matrix4));
            deferredShader.Uniforms.Add(new Variable("previousProjectionMatrix", VariableTypes.Uniform, DataTypes.Matrix4));

            deferredShader.Inputs.Add(new Variable("vPosition", VariableTypes.Input, DataTypes.Vector3) { Location = 0 });
            deferredShader.Inputs.Add(new Variable("vNormal", VariableTypes.Input, DataTypes.Vector3) { Location = 1 });
            deferredShader.Inputs.Add(new Variable("vTangent", VariableTypes.Input, DataTypes.Vector3) { Location = 2 });
            deferredShader.Inputs.Add(new Variable("vUV", VariableTypes.Input, DataTypes.Vector2) { Location = 3 });
            deferredShader.Inputs.Add(new Variable("vColor", VariableTypes.Input, DataTypes.Vector4) { Location = 4 });

            deferredShader.Outputs.Add(new Variable("fPosition", VariableTypes.Output, DataTypes.Vector3));
            deferredShader.Outputs.Add(new Variable("fClipPosition", VariableTypes.Output, DataTypes.Vector4));
            deferredShader.Outputs.Add(new Variable("fPreviousClipPosition", VariableTypes.Output, DataTypes.Vector4));
            deferredShader.Outputs.Add(new Variable("fNormal", VariableTypes.Output, DataTypes.Vector3));
            deferredShader.Outputs.Add(new Variable("fTangent", VariableTypes.Output, DataTypes.Vector3));
            deferredShader.Outputs.Add(new Variable("fColor", VariableTypes.Output, DataTypes.Vector4));
            deferredShader.Outputs.Add(new Variable("fUV", VariableTypes.Output, DataTypes.Vector2));

            var mainFunction = new Function("main", FunctionTypes.Void);
            mainFunction.Statements.Add("mat4 mvp = projectionMatrix * viewMatrix * modelMatrix;");
            mainFunction.Statements.Add("vec4 position = vec4(vPosition, 1.0);");
            mainFunction.Statements.Add("fPosition = (modelMatrix * position).xyz;");
            mainFunction.Statements.Add("fClipPosition = mvp * position;");
            mainFunction.Statements.Add("fPreviousClipPosition = previousProjectionMatrix * previousViewMatrix * previousModelMatrix * position;");
            mainFunction.Statements.Add("fNormal = (modelMatrix * vec4(vNormal, 0.0)).xyz;");
            mainFunction.Statements.Add("fTangent = (modelMatrix * vec4(vTangent, 0.0)).xyz;");
            mainFunction.Statements.Add("fColor = vColor;");
            mainFunction.Statements.Add("fUV = vUV;");
            mainFunction.Statements.Add("gl_Position = fClipPosition;");
            deferredShader.Functions.Add(mainFunction);

            return deferredShader;
        }

        /*public Function(string name, FunctionTypes functionType)
        {
            Name = name;
            FunctionType = functionType;
        }

        public string Name { get; set; }
        public FunctionTypes FunctionType { get; set; }
        public List<Statement> Statements { get; } = new List<Statement>();*/

        /*public string Name { get; set; }
        public ShaderStages ShaderStage { get; set; }
        public int Version { get; set; }

        public List<Variable> Uniforms { get; } = new List<Variable>();
        public List<Variable> Inputs { get; } = new List<Variable>();
        public List<Variable> Outputs { get; } = new List<Variable>();
        public List<Function> Functions { get; } = new List<Function>();*/

        /*public Variable(string name, VariableTypes variableType, DataTypes dataType)
        {
            Name = name;
            VariableType = variableType;
            DataType = dataType;
        }

        public string Name { get; set; }
        public VariableTypes VariableType { get; set; }
        public DataTypes DataType { get; set; }
        public int? Location { get; set; }*/
    }
}
