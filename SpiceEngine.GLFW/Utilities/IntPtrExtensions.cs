using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SpiceEngine.GLFW.Windowing
{
    public static class IntPtrExtensions
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
    }
}
