using System.Collections.Generic;
using System.Text;

namespace GLWriter.CSharp
{
    public class Conversion
    {
        private string _prefix;
        private string _suffix;
        private List<string> _prefixLines = new List<string>();
        private List<string> _suffixLines = new List<string>();

        public Conversion(CSharpType fromType, CSharpType toType)
        {
            FromType = fromType;
            ToType = toType;
        }

        public CSharpType FromType { get; }
        public CSharpType ToType { get; }

        public string ReferenceName { get; set; }

        public bool RequiresMultipleLines { get; private set; }
        public bool ContainsReturn { get; private set; }
        public bool IsUnsafe { get; private set; }
        public bool IsFixed { get; private set; }

        public void Process()
        {
            if (FromType.DataType == DataTypes.UInt && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.None)
            {
                _prefix = "(int)";
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.UInt && ToType.Modifier == TypeModifiers.None)
            {
                _prefix = "(uint)";
            }
            else if (FromType.DataType == DataTypes.Char && FromType.Modifier == TypeModifiers.Pointer && ToType.DataType == DataTypes.String && ToType.Modifier == TypeModifiers.None)
            {
                _prefix = "Marshal.PtrToStringUTF8((IntPtr)";
                _suffix = ")";
            }
            else if (FromType.DataType == DataTypes.IntPtr && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Void && ToType.Modifier == TypeModifiers.Pointer)
            {
                _suffix = ".ToPointer()";
            }
            else if (FromType.DataType == DataTypes.Void && FromType.Modifier == TypeModifiers.Pointer && ToType.DataType == DataTypes.IntPtr && ToType.Modifier == TypeModifiers.None)
            {
                _prefix = "new IntPtr(";
                _suffix = ")";
            }
            else if ((FromType.DataType == DataTypes.Int || FromType.DataType == DataTypes.Long) && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.IntPtr && ToType.Modifier == TypeModifiers.None)
            {
                _prefix = "new IntPtr(";
                _suffix = ")";
            }
            else if (FromType.DataType == DataTypes.UInt && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.UInt && ToType.Modifier == TypeModifiers.Pointer)
            {
                _prefix = "&";
            }
            else if (FromType.DataType == DataTypes.String && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Char && ToType.Modifier == TypeModifiers.Pointer)
            {
                RequiresMultipleLines = true;
                IsUnsafe = true;
                IsFixed = true;
                _prefix = "(char*)";
                _suffix = "Ptr";

                _prefixLines.Add("var $Bytes = Encoding.UTF8.GetBytes($);");
                _prefixLines.Add("fixed (byte* $Ptr = &$Bytes[0])");
                _prefixLines.Add("{");
                _suffixLines.Add("}");
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.Pointer)
            {
                RequiresMultipleLines = true;
                IsUnsafe = true;
                IsFixed = true;
                _suffix = "Ptr";

                _prefixLines.Add("fixed (int* $Ptr = &$[0])");
                _prefixLines.Add("{");
                _suffixLines.Add("}");
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.UInt && ToType.Modifier == TypeModifiers.Pointer)
            {
                RequiresMultipleLines = true;
                IsUnsafe = true;
                IsFixed = true;
                _prefix = "(uint*)";
                _suffix = "Ptr";

                _prefixLines.Add("fixed (int* $Ptr = &$[0])");
                _prefixLines.Add("{");
                _suffixLines.Add("}");
            }
            else if (FromType.DataType == DataTypes.Float && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.Float && ToType.Modifier == TypeModifiers.Pointer)
            {
                RequiresMultipleLines = true;
                IsUnsafe = true;
                IsFixed = true;
                _suffix = "Ptr";

                _prefixLines.Add("fixed (float* $Ptr = &$[0])");
                _prefixLines.Add("{");
                _suffixLines.Add("}");
            }
            else if (FromType.DataType == DataTypes.Enum && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.Enum && ToType.Modifier == TypeModifiers.Pointer)
            {
                RequiresMultipleLines = true;
                IsUnsafe = true;
                IsFixed = true;
                _suffix = "Ptr";

                _prefixLines.Add("fixed (SpiceEngine.GLFWBindings.GLEnums." + ToType.Group + "* $Ptr = &$[0])");
                _prefixLines.Add("{");
                _suffixLines.Add("}");
            }
            else if (FromType.DataType == DataTypes.Enum && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Enum && ToType.Modifier == TypeModifiers.Array)
            {
                RequiresMultipleLines = true;

                _suffix = "s";
                _prefixLines.Add("var $s = new SpiceEngine.GLFWBindings.GLEnums." + ToType.Group + "[] { $ };");
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.Array)
            {
                RequiresMultipleLines = true;

                _suffix = "s";
                _prefixLines.Add("var $s = new int[] { $ };");
            }
            else if (FromType.DataType == DataTypes.Float && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Float && ToType.Modifier == TypeModifiers.Array)
            {
                RequiresMultipleLines = true;

                _suffix = "s";
                _prefixLines.Add("var $s = new float[] { $ };");
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.None)
            {
                RequiresMultipleLines = true;
                ContainsReturn = true;

                _prefixLines.Add("var values = $");
                _prefixLines.Add("return values[0];");
            }
            else if (FromType.DataType == DataTypes.String && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.Char && ToType.Modifier == TypeModifiers.DoublePointer)
            {
                RequiresMultipleLines = true;
                _prefix = "(char**)";
                _suffix = "Ptr";

                _prefixLines.Add("var ptrs = new List<IntPtr>();");
                _prefixLines.Add("var size = Marshal.SizeOf(typeof(IntPtr));");
                _prefixLines.Add("var $Ptr = Marshal.AllocHGlobal(size * $.Length);");
                _prefixLines.Add("");
                _prefixLines.Add("for (var i = 0; i < $.Length; i++)");
                _prefixLines.Add("{");
                _prefixLines.Add("    var $SinglePtr = Marshal.StringToHGlobalAnsi($[i]);");
                _prefixLines.Add("    ptrs.Add($SinglePtr);");
                _prefixLines.Add("    Marshal.WriteIntPtr($Ptr, i * size, $SinglePtr);");
                _prefixLines.Add("}");
                _prefixLines.Add("");

                _suffixLines.Add("");
                _suffixLines.Add("Marshal.FreeHGlobal($Ptr);");
                _suffixLines.Add("");
                _suffixLines.Add("foreach (var ptr in ptrs)");
                _suffixLines.Add("{");
                _suffixLines.Add("    Marshal.FreeHGlobal(ptr);");
                _suffixLines.Add("}");
            }
            else if (FromType.DataType == DataTypes.None && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.Array)
            {
                RequiresMultipleLines = true;
                ContainsReturn = true;
                _prefix = "values";

                _prefixLines.Add("var values = new int[" + ReferenceName + "];");
                _suffixLines.Add("return values;");
            }
            else if (FromType.DataType == DataTypes.None && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Float && ToType.Modifier == TypeModifiers.Array)
            {
                RequiresMultipleLines = true;
                ContainsReturn = true;
                _prefix = "values";

                _prefixLines.Add("var values = new float[" + ReferenceName + "];");
                _suffixLines.Add("return values;");
            }
            else if (FromType.DataType == DataTypes.None && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.None)
            {
                _prefix = "1";
            }
        }

        /*public static void ShaderSource(int shader, string @string)
        {
            unsafe
            {
                var length = @string.Length;
                ShaderSource((uint)shader, 1, new string[] { @string }, &length);
            }
        }

        public static void ShaderSource(int shader, string[] @string)
        {

        }

        public static void ShaderSource(int shader, int count, char** @string, int[] length)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    _glShaderSource((uint)shader, count, @string, lengthPtr);
                }
            }
        }

        public static void GetShaderInfoLog(Int32 shader, out string info)
        {
            unsafe
            {
                int length;
                GL.GetShader(shader, ShaderParameter.InfoLogLength, out length);
                if (length == 0)
                {
                    info = String.Empty;
                    return;
                }
                GL.GetShaderInfoLog((UInt32)shader, length * 2, &length, out info);
            }
        }*/

        public string ToText(string name)
        {
            var builder = new StringBuilder();

            if (_prefix != null)
            {
                builder.Append(_prefix);
            }

            builder.Append(name);

            if (_suffix != null)
            {
                builder.Append(_suffix);
            }

            return builder.ToString();
        }

        public IEnumerable<string> ToPrefixLines(string name, int nTabs)
        {
            foreach (var line in _prefixLines)
            {
                var lineBuilder = new StringBuilder();

                for (var i = 0; i < nTabs; i++)
                {
                    lineBuilder.Append("    ");
                }

                lineBuilder.Append(line.Replace("$", name));
                yield return lineBuilder.ToString();
            }
        }

        public bool RequiresReturnLines => _suffixLines.Count > 0;

        public IEnumerable<string> ToSuffixLines(string name, int nTabs)
        {
            foreach (var line in _suffixLines)
            {
                var lineBuilder = new StringBuilder();

                for (var i = 0; i < nTabs; i++)
                {
                    lineBuilder.Append("    ");
                }

                lineBuilder.Append(line.Replace("$", name));
                yield return lineBuilder.ToString();
            }
        }
    }
}
