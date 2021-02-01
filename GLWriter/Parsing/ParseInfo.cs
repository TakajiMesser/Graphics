namespace GLWriter
{
    public class ParseInfo
    {
        public ParseInfo(string line, int startIndex = 0, Delimiters terminator = Delimiters.All, Delimiters failure = Delimiters.None)
        {
            Line = line;
            StartIndex = startIndex;
            Terminator = terminator;
            Failure = failure;
        }

        public string Line { get; set; }
        public int StartIndex { get; set; }
        public Delimiters Terminator { get; set; }
        public Delimiters Failure { get; set; }
    }
}
