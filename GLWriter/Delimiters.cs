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

        public static string ToText(DataTypes dataType) => dataType switch
        {
            DataTypes.VOID => "void",
            DataTypes.INTEGER => "int",
            DataTypes.SHORT => "short",
            DataTypes.LONG => "long",
            DataTypes.FLOAT => "float",
            DataTypes.DOUBLE => "double",
            DataTypes.BOOL => "bool",
            DataTypes.BYTE => "byte",
            DataTypes.UINT => "uint",
            DataTypes.USHORT => "ushort",
            DataTypes.ULONG => "ulong",
            DataTypes.INTEGERPTR => "int*",
            DataTypes.SHORTPTR => "short*",
            DataTypes.LONGPTR => "long*",
            DataTypes.FLOATPTR => "float*",
            DataTypes.DOUBLEPTR => "double*",
            DataTypes.BOOLPTR => "bool*",
            DataTypes.BYTEPTR => "byte*",
            DataTypes.UINTPTR => "uint*",
            DataTypes.USHORTPTR => "ushort*",
            DataTypes.ULONGPTR => "ulong*",
            DataTypes.VOIDPTR => "void*",
            DataTypes.INTPTR => "IntPtr",
            _ => throw new ArgumentOutOfRangeException("Could not convert data type " + dataType),
        };
    }
}
