using GLWriter.CSharp;
using System;

namespace GLWriter
{
    [Flags]
    public enum Delimiters
    {
        None = 1,
        Space = 2,
        StartParen = 4,
        EndParen = 8,
        Comma = 16,
        Semicolon = 32,
        EndOfLine = 64,
        StartBracket = 128,
        EndBracket = 256,
        NonSpace = StartParen | EndParen | Comma | Semicolon | EndOfLine | StartBracket | EndBracket,
        NonStartBracket = Space | StartParen | EndParen | Comma | Semicolon | EndOfLine | EndBracket,
        NonEndBracket = Space | StartParen | EndParen | Comma | Semicolon | EndOfLine | StartBracket,
        NonSemicolon = Space | StartParen | EndParen | Comma | EndOfLine | StartBracket | EndBracket,
        NonStartParen = Space | EndParen | Comma | Semicolon | EndOfLine | StartBracket | EndBracket,
        All = Space | StartParen | EndParen | Comma | Semicolon | EndOfLine | StartBracket | EndBracket
    }

    public static class DelimiterExtensions
    {
        public static Delimiters ParseDelimiter(string text) => text switch
        {
            " " => Delimiters.Space,
            "(" => Delimiters.StartParen,
            ")" => Delimiters.EndParen,
            "," => Delimiters.Comma,
            ";" => Delimiters.Semicolon,
            @"\r\n" => Delimiters.EndOfLine,
            "{" => Delimiters.StartBracket,
            "}" => Delimiters.EndBracket,
            _ => Delimiters.None,
        };
    }
}
