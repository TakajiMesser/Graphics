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
                _suffix = "Ptr";

                _prefixLines.Add("fixed (char* $Ptr = $)");
                _prefixLines.Add("{");
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.Pointer)
            {
                RequiresMultipleLines = true;
                IsUnsafe = true;
                IsFixed = true;
                _suffix = "Ptr";

                _prefixLines.Add("fixed (int* $Ptr = &$[0])");
                _prefixLines.Add("{");
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.UInt && ToType.Modifier == TypeModifiers.Pointer)
            {
                RequiresMultipleLines = true;
                IsUnsafe = true;
                IsFixed = true;
                _suffix = "Ptr";

                _prefixLines.Add("var converted = Array.ConvertAll($, i => (uint)i);");
                _prefixLines.Add("fixed (uint* $Ptr = &converted[0])");
                _prefixLines.Add("{");
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.Array)
            {
                RequiresMultipleLines = true;

                _prefixLines.Add("var $s = new int[] { $ };");
                _prefixLines.Add("var n = 1;");
            }
            else if (FromType.DataType == DataTypes.Int && FromType.Modifier == TypeModifiers.Array && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.None)
            {
                RequiresMultipleLines = true;

                _prefixLines.Add("var values = $");
                _prefixLines.Add("return values[0];");
            }
            else if (FromType.DataType == DataTypes.None && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.Array)
            {
                RequiresMultipleLines = true;
                _prefix = "values";

                _prefixLines.Add("var values = new int[" + ReferenceName + "];");
                _suffixLines.Add("return values;");
            }
            else if (FromType.DataType == DataTypes.None && FromType.Modifier == TypeModifiers.None && ToType.DataType == DataTypes.Int && ToType.Modifier == TypeModifiers.None)
            {
                _prefix = "1";
            }
        }

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

        public IEnumerable<string> ToLines(string name, int nTabs)
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

        public IEnumerable<string> ToReturnLines(int nTabs)
        {
            foreach (var line in _suffixLines)
            {
                var lineBuilder = new StringBuilder();

                for (var i = 0; i < nTabs; i++)
                {
                    lineBuilder.Append("    ");
                }

                lineBuilder.Append(line);
                yield return lineBuilder.ToString();
            }
        }
    }
}
