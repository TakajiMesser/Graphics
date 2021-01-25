﻿using System;
using System.Collections.Generic;
using System.IO;

namespace GLWriter
{
    class Program
    {
        public const string SOURCE_PATH = @"D:\GitHub\glfw-net\Examples\HelloTriangle\OpenGL.cs";
        public const string DESTINATION_PATH = @"D:\GitHub\Spice-Engine\SpiceEngine.GLFW\GL.cs";

        static void Main(string[] args)
        {
            Console.WriteLine("Reading...");
            var definitions = ReadFromSource(SOURCE_PATH);
            Console.WriteLine("Processing...");
            definitions.Process();
            Console.WriteLine("Writing...");
            WriteToDestination(DESTINATION_PATH, definitions);
            Console.WriteLine("Finished...");
        }

        private static DefinitionSet ReadFromSource(string filePath)
        {
            var definitionSet = new DefinitionSet();
            var lines = File.ReadAllLines(filePath);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                var functionDefinition = new FunctionDefinition();
                var fieldDefinition = new FieldDefinition();
                var delegateDefinition = new DelegateDefinition();

                if (functionDefinition.Parse(line))
                {
                    definitionSet.AddDefinition(functionDefinition);
                }
                else if (fieldDefinition.Parse(line))
                {
                    definitionSet.AddDefinition(fieldDefinition);
                }
                else if (delegateDefinition.Parse(line))
                {
                    definitionSet.AddDefinition(delegateDefinition);
                }
            }

            return definitionSet;
        }

        private static void WriteToDestination(string filePath, DefinitionSet definitions)
        {
            var lines = new List<string>();
            lines.Add("using System;");
            lines.Add("using System.Runtime.InteropServices;");
            lines.Add("");
            lines.Add("namespace SpiceEngineCore.GLFW");
            lines.Add("{");
            lines.Add("    public static unsafe class GL");
            lines.Add("    {");

            foreach (var definition in definitions.GetFieldDefinitions())
            {
                lines.Add("        " + definition);
            }

            lines.Add("        public static void LoadFunctions()");
            lines.Add("        {");

            foreach (var definition in definitions.GetLoadDefinitions())
            {
                lines.Add("            " + definition);
            }

            lines.Add("        }");
            lines.Add("");

            foreach (var definition in definitions.GetFunctionDefinitions())
            {
                lines.Add("        " + definition);
            }

            lines.Add("        private static T GetFunctionDelegate<T>(string name) where T : Delegate");
            lines.Add("        {");
            lines.Add("            var address = GLFWContext.GetProcAddress(name);");
            lines.Add("            return Marshal.GetDelegateForFunctionPointer<T>(address);");
            lines.Add("        }");
            lines.Add("");

            foreach (var definition in definitions.GetDelegateDefinitions())
            {
                lines.Add("        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]");
                lines.Add("        " + definition);
                lines.Add("");
            }

            lines.Add("    }");
            lines.Add("}");
            lines.Add("");

            File.WriteAllLines(filePath, lines);
        }
    }
}