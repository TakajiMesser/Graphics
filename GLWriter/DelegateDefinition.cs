using System.Collections.Generic;
using System.Text;

namespace GLWriter
{
    public class DelegateDefinition
    {
        public const string LINE_PREFIX = "private delegate ";
        public const string DELEGATE_NAME_PREFIX = "PFNGL";

        public string DelegateName { get; set; }
        public DataTypes ReturnType { get; set; }
        public List<DataTypes> Parameters { get; set; } = new List<DataTypes>();
        
        public string DelegateMaskName { get; set; }

        private void SetDelegateMaskName()
        {
            var builder = new StringBuilder();
            builder.Append("DEL");
            builder.Append("_");
            builder.Append(DataTypeExtensions.ToCharacter(ReturnType));
            builder.Append("_");

            foreach (var parameter in Parameters)
            {
                builder.Append(DataTypeExtensions.ToCharacter(parameter));
            }

            DelegateMaskName = builder.ToString();
        }

        public bool Parse(string line)
        {
            line = line.Trim();

            var a = 3;
            if (line == "private delegate uint PFNGLCREATEPROGRAMPROC();")
            {
                a = 4;
            }

            if (line.Length < LINE_PREFIX.Length) return false;
            var prefix = line.Substring(0, LINE_PREFIX.Length);
            if (prefix != LINE_PREFIX) return false;
            var index = LINE_PREFIX.Length;

            index = ParseReturnType(line, index);
            if (index < 0) return false;

            index = ParseDelegateName(line, index);
            if (index < 0) return false;

            index = ParseParameters(line, index);
            if (index < 0) return false;

            SetDelegateMaskName();
            return true;
        }

        private int ParseReturnType(string line, int index)
        {
            var parseInfo = new ParseInfo(line, index, Delimiters.Space, Delimiters.NonSpace);
            var parseResult = ParseResult.ParseNextToken(parseInfo);

            if (!parseResult.IsSuccess) return -1;
            var returnType = DataTypeExtensions.ParseDataType(parseResult.Token);

            if (returnType == DataTypes.None) return -1;
            ReturnType = returnType;

            return parseResult.Index + 1;
        }

        private int ParseDelegateName(string line, int index)
        {
            var parseInfo = new ParseInfo(line, index, Delimiters.StartParen, Delimiters.NonStartParen);
            var parseResult = ParseResult.ParseNextToken(parseInfo);

            if (!parseResult.IsSuccess) return -1;
            if (!parseResult.Token.StartsWith(DELEGATE_NAME_PREFIX)) return -1;
            DelegateName = parseResult.Token;

            return parseResult.Index + 1;
        }

        private int ParseParameters(string line, int index)
        {
            var currentIndex = index;

            while (currentIndex < line.Length && line.Substring(currentIndex, 1) != ")")
            {
                if (line.Substring(currentIndex, 1) == " ")
                {
                    currentIndex++;
                }

                // TODO - Handle int*args
                var parameterTypeInfo = new ParseInfo(line, currentIndex, Delimiters.Space, Delimiters.NonSpace);
                var parameterTypeResult = ParseResult.ParseNextToken(parameterTypeInfo);

                if (!parameterTypeResult.IsSuccess) return -1;
                currentIndex = parameterTypeResult.Index + 1;

                if (parameterTypeResult.Token == "/*const*/")
                {
                    parameterTypeInfo = new ParseInfo(line, currentIndex, Delimiters.Space, Delimiters.NonSpace);
                    parameterTypeResult = ParseResult.ParseNextToken(parameterTypeInfo);

                    if (!parameterTypeResult.IsSuccess) return -1;
                    currentIndex = parameterTypeResult.Index + 1;
                }

                var isOutVariable = false;

                if (parameterTypeResult.Token == "out")
                {
                    isOutVariable = true;
                    parameterTypeInfo = new ParseInfo(line, currentIndex, Delimiters.Space, Delimiters.NonSpace);
                    parameterTypeResult = ParseResult.ParseNextToken(parameterTypeInfo);

                    if (!parameterTypeResult.IsSuccess) return -1;
                    currentIndex = parameterTypeResult.Index + 1;
                }

                var parameterType = DataTypeExtensions.ParseDataType(parameterTypeResult.Token);
                if (parameterType == DataTypes.None) return -1;

                if (isOutVariable)
                {
                    parameterType = DataTypeExtensions.ToOutDataType(parameterType);
                }

                var parameterNameInfo = new ParseInfo(line, currentIndex);
                var parameterNameResult = ParseResult.ParseNextToken(parameterNameInfo);

                if (!parameterNameResult.IsSuccess) return -1;
                Parameters.Add(parameterType);

                if (line.Substring(parameterNameResult.Index, 1) == ")")
                {
                    currentIndex = parameterNameResult.Index + 1;
                    break;
                }

                currentIndex = parameterNameResult.Index + 1;
            }

            return currentIndex + 1;
        }
    }
}
