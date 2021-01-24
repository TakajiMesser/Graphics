using System.Text;

namespace GLWriter
{
    public class ParseResult
    {
        public ParseResult(bool isSuccess, string token, int index, Delimiters delimiter)
        {
            IsSuccess = isSuccess;
            Token = token;
            Index = index;
            Delimiter = delimiter;
        }

        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public int Index { get; set; }
        public Delimiters Delimiter { get; set; }

        public static ParseResult ParseNextToken(ParseInfo parseInfo)
        {
            var builder = new StringBuilder();

            for (var i = parseInfo.StartIndex; i < parseInfo.Line.Length; i++)
            {
                var character = parseInfo.Line.Substring(i, 1);
                var delimiter = DelimiterExtensions.ParseDelimiter(character);

                if (delimiter != Delimiters.None)
                {
                    if (parseInfo.Failure.HasFlag(delimiter))
                    {
                        return Failure(parseInfo);
                    }
                    else if (parseInfo.Terminator.HasFlag(delimiter))
                    {
                        return Success(builder, i, delimiter);
                    }
                }

                builder.Append(character);
            }

            return Success(builder, parseInfo.Line.Length);
        }

        private static ParseResult Success(StringBuilder builder, int index, Delimiters delimiter = Delimiters.EndOfLine) => new ParseResult(true, builder.ToString(), index, delimiter);

        private static ParseResult Failure(ParseInfo parseInfo) => new ParseResult(false, "", parseInfo.StartIndex, Delimiters.None);
    }
}
