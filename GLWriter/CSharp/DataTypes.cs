using System;

namespace GLWriter.CSharp
{
    public enum DataTypes
    {
        None,
        String,
        Enum,
        Struct,
        Void,
        Int,
        Short,
        Long,
        Float,
        Double,
        Char,
        Bool,
        Byte,
        SByte,
        UInt,
        UShort,
        ULong,
        IntPtr
    }

    public static class DataTypeExtensions
    {
        public static string ToText(this DataTypes dataType) => dataType switch
        {
            DataTypes.String => "string",
            DataTypes.Enum => "enum",
            DataTypes.Struct => "struct",
            DataTypes.Void => "void",
            DataTypes.Int => "int",
            DataTypes.Short => "short",
            DataTypes.Long => "long",
            DataTypes.Float => "float",
            DataTypes.Double => "double",
            DataTypes.Char => "char",
            DataTypes.Bool => "bool",
            DataTypes.Byte => "byte",
            DataTypes.SByte => "sbyte",
            DataTypes.UInt => "uint",
            DataTypes.UShort => "ushort",
            DataTypes.ULong => "ulong",
            DataTypes.IntPtr => "IntPtr",
            _ => throw new ArgumentOutOfRangeException("Could not convert data type " + dataType),
        };

        public static DataTypes ParseDataType(string text) => text switch
        {
            "string" => DataTypes.String,
            "enum" => DataTypes.Enum,
            "struct" => DataTypes.Struct,
            "void" => DataTypes.Void,
            "int" => DataTypes.Int,
            "short" => DataTypes.Short,
            "long" => DataTypes.Long,
            "float" => DataTypes.Float,
            "double" => DataTypes.Double,
            "char" => DataTypes.Char,
            "bool" => DataTypes.Bool,
            "byte" => DataTypes.Byte,
            "sbyte" => DataTypes.SByte,
            "uint" => DataTypes.UInt,
            "ushort" => DataTypes.UShort,
            "ulong" => DataTypes.ULong,
            "IntPtr" => DataTypes.IntPtr,
            _ => DataTypes.None
        };

        public static string ToCode(this DataTypes dataType) => dataType switch
        {
            DataTypes.String => "Str",
            DataTypes.Enum => "E",
            DataTypes.Struct => "St",
            DataTypes.Void => "V",
            DataTypes.Int => "I",
            DataTypes.Short => "S",
            DataTypes.Long => "L",
            DataTypes.Float => "F",
            DataTypes.Double => "D",
            DataTypes.Char => "C",
            DataTypes.Bool => "B",
            DataTypes.Byte => "By",
            DataTypes.SByte => "Sby",
            DataTypes.UInt => "Ui",
            DataTypes.UShort => "Us",
            DataTypes.ULong => "Ul",
            DataTypes.IntPtr => "P",
            _ => throw new ArgumentOutOfRangeException("Could not convert data type " + dataType),
        };
    }
}
