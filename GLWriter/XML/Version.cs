using System;

namespace GLWriter.XML
{
    public struct Version : IComparable<Version>
    {
        public Version(string api, int major, int minor)
        {
            API = api;
            Major = major;
            Minor = minor;
        }

        public Version(string api, string number)
        {
            API = api;

            var separatorIndex = number.IndexOf(".");
            var major = number.Substring(0, separatorIndex);
            var minor = number.Substring(separatorIndex + 1);

            Major = int.Parse(major);
            Minor = int.Parse(minor);
        }

        public string API { get; }
        public int Major { get; }
        public int Minor { get; }

        public int CompareTo(Version other)
        {
            if (Major < other.Major)
            {
                return -1;
            }
            else if (Major > other.Major)
            {
                return 1;
            }
            else if (Minor < other.Minor)
            {
                return -1;
            }
            else if (Minor > other.Minor)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        // TODO - What is "glcore"?
        public static Version GL(int major, int minor) => new Version("gl", major, minor);
        public static Version GL(string number) => new Version("gl", number);

        public static Version GLES1(int major, int minor) => new Version("gles1", major, minor);
        public static Version GLES1(string number) => new Version("gles1", number);

        public static Version GLES2(int major, int minor) => new Version("gles2", major, minor);
        public static Version GLES2(string number) => new Version("gles2", number);
    }
}
