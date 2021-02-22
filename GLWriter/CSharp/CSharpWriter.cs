using System.Collections.Generic;
using System.IO;

namespace GLWriter.CSharp
{
    public class CSharpWriter
    {
        public void WriteEnumFiles(CSharpSpec spec, string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (var file in Directory.GetFiles(directoryPath))
                {
                    File.Delete(file);
                }
            }

            foreach (var enumGroup in spec.Enums)
            {
                var filePath = Path.Join(directoryPath, enumGroup.Name + ".cs");
                File.WriteAllLines(filePath, enumGroup.ToLines());
            }
        }

        public void WriteStructFiles(CSharpSpec spec, string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (var file in Directory.GetFiles(directoryPath))
                {
                    File.Delete(file);
                }
            }

            foreach (var structSpec in spec.Structs)
            {
                var filePath = Path.Join(directoryPath, structSpec.Name + ".cs");
                File.WriteAllLines(filePath, structSpec.ToLines());
            }
        }

        public void WriteFunctionFile(CSharpSpec spec, string filePath)
        {
            var lines = new List<string>();
            lines.Add("using SpiceEngineCore.Geometry;");
            lines.Add("using System;");
            lines.Add("using System.Collections.Generic;");
            lines.Add("using System.Runtime.InteropServices;");
            lines.Add("");
            lines.Add("namespace SpiceEngine.GLFW");
            lines.Add("{");
            lines.Add("    public static unsafe class GL");
            lines.Add("    {");

            foreach (var function in spec.Functions)
            {
                lines.Add("        " + function.ToFieldLine());
            }

            lines.Add("        public static void LoadFunctions()");
            lines.Add("        {");

            foreach (var function in spec.Functions)
            {
                lines.Add("            " + function.ToLoadLine());
            }

            lines.Add("        }");
            lines.Add("");

            foreach (var function in spec.Functions)
            {
                lines.Add("        " + function.ToDefinitionLine());
            }

            lines.Add("");

            for (var i = 0; i < spec.OverloadCount; i++)
            {
                var overload = spec.OverloadAt(i);

                foreach (var line in overload.ToDefinitionLines())
                {
                    lines.Add("        " + line);
                }

                lines.Add("");
            }

            lines.Add("        public static void ClearColor(Color4 color) => ClearColor(color.R, color.G, color.B, color.A);");
            lines.Add("");

            lines.Add("        public static Color4 ReadPixels(int x, int y, int width, int height, GLFWBindings.GLEnums.PixelFormat format, GLFWBindings.GLEnums.PixelType type)");
            lines.Add("        {");
            lines.Add("            var bytes = new byte[4];");
            lines.Add("");
            lines.Add("            unsafe");
            lines.Add("            {");
            lines.Add("                fixed (byte* bytesPtr = &bytes[0])");
            lines.Add("                {");
            lines.Add("                    ReadPixels(x, y, width, height, format, type, (IntPtr)bytesPtr);");
            lines.Add("                    return new Color4((int)bytes[0], (int)bytes[1], (int)bytes[2], (int)bytes[3]);");
            lines.Add("                }");
            lines.Add("            }");
            lines.Add("        }");
            lines.Add("");

            // TODO - Generate "easy" function overloads
            // Change uint parameters to int and explicitly cast for ease
            // Convert char* to string
            // If function has void return and last parameter is void* type, then convert to and return as byte[]

            // TODO - Still need to properly handle extensions instead of throwing them all out - check the "extension->supported" field

            lines.Add("        private static T GetFunctionDelegate<T>(string name) where T : Delegate");
            lines.Add("        {");
            lines.Add("            var address = GLFW.GetProcAddress(name);");
            lines.Add("            return Marshal.GetDelegateForFunctionPointer<T>(address);");
            lines.Add("        }");
            lines.Add("");

            for (var i = 0; i < spec.DelegateCount; i++)
            {
                var delegateDefinition = spec.DelegateAt(i);

                lines.Add("        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]");
                lines.Add("        " + delegateDefinition.ToDefinitionLine());

                if (i < spec.DelegateCount - 1)
                {
                    lines.Add("");
                }
            }

            lines.Add("    }");
            lines.Add("}");

            File.WriteAllLines(filePath, lines);
        }
    }
}
