﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.Shaders
{
    public enum VariableTypes
    {
        Default,
        Constant,
        Uniform,
        Input,
        Output
    }

    public enum DataTypes
    {
        Integer,
        Float,
        Vector2,
        Vector3,
        Vector4,
        Matrix4,
        Sampler2D
    }

    public class Variable
    {
        public Variable(string name, VariableTypes variableType, DataTypes dataType)
        {
            Name = name;
            VariableType = variableType;
            DataType = dataType;
        }

        public string Name { get; set; }
        public VariableTypes VariableType { get; set; }
        public DataTypes DataType { get; set; }
        public int? Location { get; set; }

        public string GetText()
        {
            var builder = new StringBuilder();

            if (Location.HasValue)
            {
                builder.Append("layout(location = " + Location.Value + ") ");
            }

            var variableTypeText = GetTextForVariableType(VariableType);
            if (!string.IsNullOrEmpty(variableTypeText))
            {
                builder.Append(variableTypeText + " ");
            }
            
            builder.Append(GetTextForDataType(DataType) + " ");
            builder.Append(Name + ";");

            return builder.ToString();
        }

        public static string GetTextForVariableType(VariableTypes variableType)
        {
            switch (variableType)
            {
                case VariableTypes.Default:
                    return "";
                case VariableTypes.Constant:
                    return "const";
                case VariableTypes.Uniform:
                    return "uniform";
                case VariableTypes.Input:
                    return "in";
                case VariableTypes.Output:
                    return "out";
            }

            throw new ArgumentOutOfRangeException("Could not handle variable type " + variableType);
        }

        public static string GetTextForDataType(DataTypes dataType)
        {
            switch (dataType)
            {
                case DataTypes.Integer:
                    return "int";
                case DataTypes.Float:
                    return "float";
                case DataTypes.Vector2:
                    return "vec2";
                case DataTypes.Vector3:
                    return "vec3";
                case DataTypes.Vector4:
                    return "vec4";
                case DataTypes.Matrix4:
                    return "mat4";
                case DataTypes.Sampler2D:
                    return "sampler2D";
            }

            throw new ArgumentOutOfRangeException("Could not handle data type " + dataType);
        }
    }
}
