using GLWriter.CSharp;
using System.Collections.Generic;

namespace GLWriter
{
    public class FunctionDefinition
    {
        public const string LINE_PREFIX = "public static ";
        public const string FUNCTION_NAME_PREFIX = "gl";
        public const string DELEGATE_NAME_PREFIX = " => ";

        public string FunctionName { get; set; }
        public DataTypes ReturnType { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
        public string FieldName { get; set; }

        public bool Parse(string line)
        {
            line = line.Trim();

            var a = 3;
            if (line == "public static void glDepthMask(bool enabled) => _glDepthMask(enabled);")
            {
                a = 4;
            }

            if (line.Length < LINE_PREFIX.Length) return false;
            var prefix = line.Substring(0, LINE_PREFIX.Length);
            if (prefix != LINE_PREFIX) return false;
            var index = LINE_PREFIX.Length;

            index = ParseReturnType(line, index);
            if (index < 0) return false;

            index = ParseFunctionName(line, index);
            if (index < 0) return false;

            index = ParseParameters(line, index);
            if (index < 0) return false;

            if (index + DELEGATE_NAME_PREFIX.Length > line.Length) return false;
            var delegatePrefix = line.Substring(index, DELEGATE_NAME_PREFIX.Length);
            if (delegatePrefix != DELEGATE_NAME_PREFIX) return false;
            index += DELEGATE_NAME_PREFIX.Length;

            index = ParseFieldName(line, index);
            if (index < 0) return false;

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

        private int ParseFunctionName(string line, int index)
        {
            var parseInfo = new ParseInfo(line, index, Delimiters.StartParen, Delimiters.NonStartParen);
            var parseResult = ParseResult.ParseNextToken(parseInfo);

            if (!parseResult.IsSuccess) return -1;
            if (!parseResult.Token.StartsWith(FUNCTION_NAME_PREFIX)) return -1;
            FunctionName = parseResult.Token.Substring(FUNCTION_NAME_PREFIX.Length);

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
                var parameter = new Parameter(parameterType, parameterNameResult.Token);
                Parameters.Add(parameter);

                if (line.Substring(parameterNameResult.Index, 1) == ")")
                {
                    currentIndex = parameterNameResult.Index;
                    break;
                }

                currentIndex = parameterNameResult.Index + 1;
            }

            return currentIndex + 1;
        }

        private int ParseFieldName(string line, int index)
        {
            var parseInfo = new ParseInfo(line, index, Delimiters.StartParen, Delimiters.NonStartParen);
            var parseResult = ParseResult.ParseNextToken(parseInfo);

            if (!parseResult.IsSuccess) return -1;
            FieldName = parseResult.Token;

            return parseResult.Index + 1;
        }
    }
}
