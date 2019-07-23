﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Shaders
{
    public enum FunctionTypes
    {
        Void,
        Integer,
        Float,
        Vector2,
        Vector3,
        Vector4,
        Matrix4
    }

    public class Function
    {
        public Function(string name, FunctionTypes functionType)
        {
            Name = name;
            FunctionType = functionType;
        }

        public string Name { get; set; }
        public FunctionTypes FunctionType { get; set; }
        public List<Statement> Statements { get; } = new List<Statement>();

        public string GetText()
        {
            var builder = new StringBuilder();

            builder.Append(GetTextForFunctionType(FunctionType) + " " + Name + "()");
            builder.Append("{" + Shader.NEW_LINE);

            foreach (var statement in Statements)
            {
                builder.Append(statement.GetText() + Shader.NEW_LINE);
            }

            builder.Append("}");

            return builder.ToString();
        }

        public static string GetTextForFunctionType(FunctionTypes functionType)
        {
            switch (functionType)
            {
                case FunctionTypes.Void:
                    return "void";
                case FunctionTypes.Integer:
                    return "int";
                case FunctionTypes.Float:
                    return "float";
                case FunctionTypes.Vector2:
                    return "vec2";
                case FunctionTypes.Vector3:
                    return "vec3";
                case FunctionTypes.Vector4:
                    return "vec4";
                case FunctionTypes.Matrix4:
                    return "mat4";
            }

            throw new ArgumentOutOfRangeException("Could not handle function type " + functionType);
        }
    }
}
