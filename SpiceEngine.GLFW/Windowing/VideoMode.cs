using System.Runtime.InteropServices;

namespace SpiceEngine.GLFW.Windowing
{
    [StructLayout(LayoutKind.Explicit, Size = 24, Pack = 4)]
    public struct VideoMode
    {
        [FieldOffset(0)]
        public readonly int Width;

        [FieldOffset(4)]
        public readonly int Height;

        [FieldOffset(8)]
        public readonly int RedBits;

        [FieldOffset(12)]
        public readonly int GreenBits;

        [FieldOffset(16)]
        public readonly int BlueBits;

        [FieldOffset(20)]
        public readonly int RefreshRate;
    }
}
