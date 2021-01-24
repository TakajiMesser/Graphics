using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SpiceEngine.GLFW.Utilities
{
    public static class InteropExtensions
    {
        public static string ToStringUTF8(this IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                var length = 0;

                while (Marshal.ReadByte(ptr, length) != 0)
                {
                    length++;
                }
                
                var buffer = new byte[length];
                Marshal.Copy(ptr, buffer, 0, length);

                return Encoding.UTF8.GetString(buffer);
            }

            return "";
        }

        public static string[] ToStringsUTF8(this IntPtr pointer, int count)
        {
            var values = new string[count];
            var offset = 0;

            for (var i = 0; i < count; i++)
            {
                var currentPointer = Marshal.ReadIntPtr(pointer + offset);
                values[i] = currentPointer.ToStringUTF8();
                offset += IntPtr.Size;
            }

            return values;
        }

        public static IntPtr ToIntPtr(this string str)
        {
            return Marshal.StringToCoTaskMemAuto(str);
        }

        /*public static unsafe IntPtr StringToCoTaskMemUTF8(string str)
        {
            return Marshal.StringToCoTaskMemUTF8(str);
        }

        public static unsafe string PtrToStringUTF8(byte* ptr)
        {
            return Marshal.PtrToStringUTF8((IntPtr)ptr);
        }*/
    }
}
