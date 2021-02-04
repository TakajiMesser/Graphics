using System;

namespace GLWriter.CSharp
{
    public enum DataTypes
    {
        None,
        STRING,
        ENUM,
        STRUCT,
        VOID,
        INTEGER,
        SHORT,
        LONG,
        FLOAT,
        DOUBLE,
        CHAR,
        BOOL,
        BYTE,
        SBYTE,
        UINT,
        USHORT,
        ULONG,
        ENUMPTR,
        STRUCTPTR,
        INTEGERPTR,
        SHORTPTR,
        LONGPTR,
        FLOATPTR,
        DOUBLEPTR,
        CHARPTR,
        CHARPTRPTR,
        BOOLPTR,
        BOOLPTRPTR,
        BYTEPTR,
        BYTEPTRPTR,
        SBYTEPTR,
        UBYTEPTR,
        UINTPTR,
        USHORTPTR,
        ULONGPTR,
        VOIDPTR,
        VOIDPTRPTR,
        INTPTR,
        INTPTRPTR,
        OUTINTEGER,
        OUTINTPTR,
    }

    public static class DataTypeExtensions
    {
        public static DataTypes ToPtrType(this DataTypes dataType) => dataType switch
        {
            DataTypes.ENUM => DataTypes.ENUMPTR,
            DataTypes.STRUCT => DataTypes.STRUCTPTR,
            DataTypes.INTEGER => DataTypes.INTEGERPTR,
            DataTypes.SHORT => DataTypes.SHORTPTR,
            DataTypes.LONG => DataTypes.LONGPTR,
            DataTypes.FLOAT => DataTypes.FLOATPTR,
            DataTypes.DOUBLE => DataTypes.DOUBLEPTR,
            DataTypes.CHAR => DataTypes.CHARPTR,
            DataTypes.BOOL => DataTypes.BOOLPTR,
            DataTypes.BYTE => DataTypes.BYTEPTR,
            DataTypes.SBYTE => DataTypes.SBYTEPTR,
            DataTypes.UINT => DataTypes.UINTPTR,
            DataTypes.USHORT => DataTypes.USHORTPTR,
            DataTypes.ULONG => DataTypes.ULONGPTR,
            DataTypes.VOID => DataTypes.VOIDPTR,
            DataTypes.INTPTR => DataTypes.INTPTRPTR,
            _ => throw new ArgumentOutOfRangeException("Could not convert data type " + dataType),
        };

        public static DataTypes ToPtrPtrType(this DataTypes dataType) => dataType switch
        {
            DataTypes.BOOL => DataTypes.BOOLPTRPTR,
            DataTypes.BYTE => DataTypes.BYTEPTRPTR,
            DataTypes.CHAR => DataTypes.CHARPTRPTR,
            DataTypes.VOID => DataTypes.VOIDPTRPTR,
            _ => throw new ArgumentOutOfRangeException("Could not convert data type " + dataType),
        };

        public static DataTypes ToOutDataType(this DataTypes dataType) => dataType switch
        {
            DataTypes.INTEGER => DataTypes.OUTINTEGER,
            DataTypes.INTPTR => DataTypes.OUTINTPTR,
            _ => throw new ArgumentOutOfRangeException("Could not convert data type " + dataType),
        };

        public static DataTypes ParseDataType(string text) => text switch
        {
            "void" => DataTypes.VOID,
            "int" => DataTypes.INTEGER,
            "short" => DataTypes.SHORT,
            "long" => DataTypes.LONG,
            "float" => DataTypes.FLOAT,
            "double" => DataTypes.DOUBLE,
            "char" => DataTypes.CHAR,
            "bool" => DataTypes.BOOL,
            "byte" => DataTypes.BYTE,
            "sbyte" => DataTypes.SBYTE,
            "uint" => DataTypes.UINT,
            "ushort" => DataTypes.USHORT,
            "ulong" => DataTypes.ULONG,
            "int*" => DataTypes.INTEGERPTR,
            "short*" => DataTypes.SHORTPTR,
            "long*" => DataTypes.LONGPTR,
            "float*" => DataTypes.FLOATPTR,
            "double*" => DataTypes.DOUBLEPTR,
            "char*" => DataTypes.CHARPTR,
            "char**" => DataTypes.CHARPTRPTR,
            "bool*" => DataTypes.BOOLPTR,
            "bool**" => DataTypes.BOOLPTRPTR,
            "byte*" => DataTypes.BYTEPTR,
            "sbyte*" => DataTypes.SBYTEPTR,
            "ubyte*" => DataTypes.UBYTEPTR,
            "byte**" => DataTypes.BYTEPTRPTR,
            "uint*" => DataTypes.UINTPTR,
            "ushort*" => DataTypes.USHORTPTR,
            "ulong*" => DataTypes.ULONGPTR,
            "void*" => DataTypes.VOIDPTR,
            "void**" => DataTypes.VOIDPTRPTR,
            "IntPtr" => DataTypes.INTPTR,
            _ => DataTypes.None
        };

        public static string ToText(DataTypes dataType, string group)
        {
            if (dataType == DataTypes.ENUM)
            {
                return "SpiceEngine.GLFWBindings.GLEnums." + group;
            }
            else if (dataType == DataTypes.ENUMPTR)
            {
                return "SpiceEngine.GLFWBindings.GLEnums." + group + "*";
            }
            else if (dataType == DataTypes.STRUCT)
            {
                return "SpiceEngine.GLFWBindings.GLStructs." + group;
            }
            else if (dataType == DataTypes.STRUCTPTR)
            {
                return "SpiceEngine.GLFWBindings.GLStructs." + group + "*";
            }
            else
            {
                return ToText(dataType);
            }
        }

        public static string ToText(DataTypes dataType) => dataType switch
        {
            DataTypes.VOID => "void",
            DataTypes.INTEGER => "int",
            DataTypes.SHORT => "short",
            DataTypes.LONG => "long",
            DataTypes.FLOAT => "float",
            DataTypes.DOUBLE => "double",
            DataTypes.CHAR => "char",
            DataTypes.BOOL => "bool",
            DataTypes.BYTE => "byte",
            DataTypes.SBYTE => "sbyte",
            DataTypes.UINT => "uint",
            DataTypes.USHORT => "ushort",
            DataTypes.ULONG => "ulong",
            DataTypes.INTEGERPTR => "int*",
            DataTypes.SHORTPTR => "short*",
            DataTypes.LONGPTR => "long*",
            DataTypes.FLOATPTR => "float*",
            DataTypes.DOUBLEPTR => "double*",
            DataTypes.CHARPTR => "char*",
            DataTypes.CHARPTRPTR => "char**",
            DataTypes.BOOLPTR => "bool*",
            DataTypes.BOOLPTRPTR => "bool**",
            DataTypes.BYTEPTR => "byte*",
            DataTypes.BYTEPTRPTR => "byte**",
            DataTypes.SBYTEPTR => "sbyte*",
            DataTypes.UBYTEPTR => "ubyte*",
            DataTypes.UINTPTR => "uint*",
            DataTypes.USHORTPTR => "ushort*",
            DataTypes.ULONGPTR => "ulong*",
            DataTypes.VOIDPTR => "void*",
            DataTypes.VOIDPTRPTR => "void**",
            DataTypes.INTPTR => "IntPtr",
            DataTypes.INTPTRPTR => "IntPtr*",
            DataTypes.OUTINTEGER => "out int",
            DataTypes.OUTINTPTR => "out IntPtr",
            _ => throw new ArgumentOutOfRangeException("Could not convert data type " + dataType),
        };

        public static string ToCharacter(DataTypes dataType, string group)
        {
            if (dataType == DataTypes.ENUM)
            {
                return "E" + group + "E";
            }
            else if (dataType == DataTypes.ENUMPTR)
            {
                return "Ep" + group + "Ep";
            }
            else if (dataType == DataTypes.STRUCT)
            {
                return "St" + group + "St";
            }
            else if (dataType == DataTypes.STRUCTPTR)
            {
                return "Stp" + group + "Stp";
            }
            else
            {
                return ToCharacter(dataType);
            }
        }

        public static string ToCharacter(DataTypes dataType) => dataType switch
        {
            DataTypes.VOID => "V",
            DataTypes.INTEGER => "I",
            DataTypes.SHORT => "S",
            DataTypes.LONG => "L",
            DataTypes.FLOAT => "F",
            DataTypes.DOUBLE => "D",
            DataTypes.CHAR => "C",
            DataTypes.BOOL => "B",
            DataTypes.BYTE => "By",
            DataTypes.SBYTE => "Sby",
            DataTypes.UINT => "Ui",
            DataTypes.USHORT => "Us",
            DataTypes.ULONG => "Ul",
            DataTypes.INTEGERPTR => "Ip",
            DataTypes.SHORTPTR => "Sp",
            DataTypes.LONGPTR => "Lp",
            DataTypes.FLOATPTR => "Fp",
            DataTypes.DOUBLEPTR => "Dp",
            DataTypes.CHARPTR => "Cp",
            DataTypes.CHARPTRPTR => "Cpp",
            DataTypes.BOOLPTR => "Bp",
            DataTypes.BOOLPTRPTR => "Bpp",
            DataTypes.BYTEPTR => "Byp",
            DataTypes.BYTEPTRPTR => "Bypp",
            DataTypes.SBYTEPTR => "Sbyp",
            DataTypes.UBYTEPTR => "Ubyp",
            DataTypes.UINTPTR => "Uip",
            DataTypes.USHORTPTR => "Usp",
            DataTypes.ULONGPTR => "Ulp",
            DataTypes.VOIDPTR => "Vp",
            DataTypes.VOIDPTRPTR => "Vpp",
            DataTypes.INTPTR => "P",
            DataTypes.INTPTRPTR => "Pp",
            DataTypes.OUTINTEGER => "Oi",
            DataTypes.OUTINTPTR => "Op",
            _ => throw new ArgumentOutOfRangeException("Could not convert data type " + dataType),
        };
    }
}
