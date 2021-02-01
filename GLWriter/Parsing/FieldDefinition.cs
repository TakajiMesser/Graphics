namespace GLWriter
{
    public class FieldDefinition
    {
        public const string LINE_PREFIX = "private static ";
        public const string DELEGATE_NAME_PREFIX = "PFNGL";
        public const string FIELD_NAME_PREFIX = "_gl";

        public string DelegateName { get; set; }
        public string FieldName { get; set; }

        public bool Parse(string line)
        {
            line = line.Trim();

            if (line.Length < LINE_PREFIX.Length) return false;
            var prefix = line.Substring(0, LINE_PREFIX.Length);
            if (prefix != LINE_PREFIX) return false;
            var index = LINE_PREFIX.Length;

            index = ParseDelegateName(line, index);
            if (index < 0) return false;

            index = ParseFieldName(line, index);
            if (index < 0) return false;

            return true;
        }

        private int ParseDelegateName(string line, int index)
        {
            var parseInfo = new ParseInfo(line, index, Delimiters.Space, Delimiters.NonSpace);
            var parseResult = ParseResult.ParseNextToken(parseInfo);

            if (!parseResult.IsSuccess) return -1;
            if (!parseResult.Token.StartsWith(DELEGATE_NAME_PREFIX)) return -1;
            DelegateName = parseResult.Token;//parseResult.Token.Substring(DELEGATE_NAME_PREFIX.Length);

            return parseResult.Index + 1;
        }

        private int ParseFieldName(string line, int index)
        {
            var parseInfo = new ParseInfo(line, index, Delimiters.Semicolon, Delimiters.NonSemicolon);
            var parseResult = ParseResult.ParseNextToken(parseInfo);

            if (!parseResult.IsSuccess) return -1;
            if (!parseResult.Token.StartsWith(FIELD_NAME_PREFIX)) return -1;
            FieldName = parseResult.Token;

            return parseResult.Index + 1;
        }
    }
}
