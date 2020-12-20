using SpiceEngineCore.Geometry.Vectors;
using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry.Colors
{
    /// <summary>
    /// Represents a color with 4 floating-point components (R, G, B, A).
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct TColor4 : IEquatable<TColor4>
    {
        /// <summary>
        /// The red component of this TColor4 structure.
        /// </summary>
        public float R;

        /// <summary>
        /// The green component of this TColor4 structure.
        /// </summary>
        public float G;

        /// <summary>
        /// The blue component of this TColor4 structure.
        /// </summary>
        public float B;

        /// <summary>
        /// The alpha component of this TColor4 structure.
        /// </summary>
        public float A;

        /// <summary>
        /// Initializes a new instance of the <see cref="TColor4"/> struct.
        /// </summary>
        /// <param name="r">The red component of the new TColor4 structure.</param>
        /// <param name="g">The green component of the new TColor4 structure.</param>
        /// <param name="b">The blue component of the new TColor4 structure.</param>
        /// <param name="a">The alpha component of the new TColor4 structure.</param>
        public TColor4(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TColor4"/> struct.
        /// </summary>
        /// <param name="r">The red component of the new TColor4 structure.</param>
        /// <param name="g">The green component of the new TColor4 structure.</param>
        /// <param name="b">The blue component of the new TColor4 structure.</param>
        /// <param name="a">The alpha component of the new TColor4 structure.</param>
        public TColor4(byte r, byte g, byte b, byte a)
        {
            R = r / (float)byte.MaxValue;
            G = g / (float)byte.MaxValue;
            B = b / (float)byte.MaxValue;
            A = a / (float)byte.MaxValue;
        }

        /// <summary>
        /// Converts this color to an integer representation with 8 bits per channel.
        /// </summary>
        /// <returns>A <see cref="int"/> that represents this instance.</returns>
        /// <remarks>
        /// This method is intended only for compatibility with System.Drawing. It compresses the color into 8 bits per
        /// channel, which means color information is lost.
        /// </remarks>
        public int ToArgb()
        {
            var value =
                ((uint)(A * byte.MaxValue) << 24) |
                ((uint)(R * byte.MaxValue) << 16) |
                ((uint)(G * byte.MaxValue) << 8) |
                (uint)(B * byte.MaxValue);

            return unchecked((int)value);
        }

        /// <summary>
        /// Compares the specified TColor4 structures for equality.
        /// </summary>
        /// <param name="left">The left-hand side of the comparison.</param>
        /// <param name="right">The right-hand side of the comparison.</param>
        /// <returns>True if left is equal to right; false otherwise.</returns>
        [Pure]
        public static bool operator ==(TColor4 left, TColor4 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares the specified TColor4 structures for inequality.
        /// </summary>
        /// <param name="left">The left-hand side of the comparison.</param>
        /// <param name="right">The right-hand side of the comparison.</param>
        /// <returns>True if left is not equal to right; false otherwise.</returns>
        [Pure]
        public static bool operator !=(TColor4 left, TColor4 right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Converts the specified System.Drawing.Color to a TColor4 structure.
        /// </summary>
        /// <param name="color">The System.Drawing.Color to convert.</param>
        /// <returns>A new TColor4 structure containing the converted components.</returns>
        [Pure]
        public static implicit operator TColor4(Color color)
        {
            return new TColor4(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Converts the specified TColor4 to a System.Drawing.Color structure.
        /// </summary>
        /// <param name="color">The TColor4 to convert.</param>
        /// <returns>A new System.Drawing.Color structure containing the converted components.</returns>
        [Pure]
        public static explicit operator Color(TColor4 color)
        {
            return Color.FromArgb(
                (int)(color.A * byte.MaxValue),
                (int)(color.R * byte.MaxValue),
                (int)(color.G * byte.MaxValue),
                (int)(color.B * byte.MaxValue));
        }

        /// <summary>
        /// Returns this TColor4 as a TVector4. The resulting struct will have XYZW mapped to RGBA, in that order.
        /// </summary>
        /// <param name="c">The TColor4 to convert.</param>
        /// <returns>The TColor4, converted into a TVector4.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static explicit operator TVector4(TColor4 c)
        {
            return new TVector4(c.R, c.G, c.B, c.A);// Unsafe.As<TColor4, TVector4>(ref c);
        }

        /// <summary>
        /// Compares whether this TColor4 structure is equal to the specified object.
        /// </summary>
        /// <param name="obj">An object to compare to.</param>
        /// <returns>True obj is a TColor4 structure with the same components as this TColor4; false otherwise.</returns>
        [Pure]
        public override bool Equals(object obj)
        {
            return obj is TColor4 && Equals((TColor4)obj);
        }

        /// <summary>
        /// Calculates the hash code for this TColor4 structure.
        /// </summary>
        /// <returns>A System.Int32 containing the hashcode of this TColor4 structure.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1960784236;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            return hashCode;
            //return HashCode.Combine(R, G, B, A);
        }

        /// <summary>
        /// Creates a System.String that describes this TColor4 structure.
        /// </summary>
        /// <returns>A System.String that describes this TColor4 structure.</returns>
        public override string ToString()
        {
            var ls = MathHelper.ListSeparator;
            return $"{{(R{ls} G{ls} B{ls} A) = ({R}{ls} {G}{ls} {B}{ls} {A})}}";
        }

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 255, 255, 0).
        /// </summary>
        public static TColor4 Transparent => new TColor4(255, 255, 255, 0);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (240, 248, 255, 255).
        /// </summary>
        public static TColor4 AliceBlue => new TColor4(240, 248, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (250, 235, 215, 255).
        /// </summary>
        public static TColor4 AntiqueWhite => new TColor4(250, 235, 215, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 255, 255, 255).
        /// </summary>
        public static TColor4 Aqua => new TColor4(0, 255, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (127, 255, 212, 255).
        /// </summary>
        public static TColor4 Aquamarine => new TColor4(127, 255, 212, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (240, 255, 255, 255).
        /// </summary>
        public static TColor4 Azure => new TColor4(240, 255, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (245, 245, 220, 255).
        /// </summary>
        public static TColor4 Beige => new TColor4(245, 245, 220, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 228, 196, 255).
        /// </summary>
        public static TColor4 Bisque => new TColor4(255, 228, 196, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 0, 0, 255).
        /// </summary>
        public static TColor4 Black => new TColor4(0, 0, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 235, 205, 255).
        /// </summary>
        public static TColor4 BlanchedAlmond => new TColor4(255, 235, 205, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 0, 255, 255).
        /// </summary>
        public static TColor4 Blue => new TColor4(0, 0, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (138, 43, 226, 255).
        /// </summary>
        public static TColor4 BlueViolet => new TColor4(138, 43, 226, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (165, 42, 42, 255).
        /// </summary>
        public static TColor4 Brown => new TColor4(165, 42, 42, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (222, 184, 135, 255).
        /// </summary>
        public static TColor4 BurlyWood => new TColor4(222, 184, 135, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (95, 158, 160, 255).
        /// </summary>
        public static TColor4 CadetBlue => new TColor4(95, 158, 160, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (127, 255, 0, 255).
        /// </summary>
        public static TColor4 Chartreuse => new TColor4(127, 255, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (210, 105, 30, 255).
        /// </summary>
        public static TColor4 Chocolate => new TColor4(210, 105, 30, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 127, 80, 255).
        /// </summary>
        public static TColor4 Coral => new TColor4(255, 127, 80, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (100, 149, 237, 255).
        /// </summary>
        public static TColor4 CornflowerBlue => new TColor4(100, 149, 237, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 248, 220, 255).
        /// </summary>
        public static TColor4 Cornsilk => new TColor4(255, 248, 220, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (220, 20, 60, 255).
        /// </summary>
        public static TColor4 Crimson => new TColor4(220, 20, 60, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 255, 255, 255).
        /// </summary>
        public static TColor4 Cyan => new TColor4(0, 255, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 0, 139, 255).
        /// </summary>
        public static TColor4 DarkBlue => new TColor4(0, 0, 139, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 139, 139, 255).
        /// </summary>
        public static TColor4 DarkCyan => new TColor4(0, 139, 139, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (184, 134, 11, 255).
        /// </summary>
        public static TColor4 DarkGoldenrod => new TColor4(184, 134, 11, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (169, 169, 169, 255).
        /// </summary>
        public static TColor4 DarkGray => new TColor4(169, 169, 169, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 100, 0, 255).
        /// </summary>
        public static TColor4 DarkGreen => new TColor4(0, 100, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (189, 183, 107, 255).
        /// </summary>
        public static TColor4 DarkKhaki => new TColor4(189, 183, 107, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (139, 0, 139, 255).
        /// </summary>
        public static TColor4 DarkMagenta => new TColor4(139, 0, 139, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (85, 107, 47, 255).
        /// </summary>
        public static TColor4 DarkOliveGreen => new TColor4(85, 107, 47, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 140, 0, 255).
        /// </summary>
        public static TColor4 DarkOrange => new TColor4(255, 140, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (153, 50, 204, 255).
        /// </summary>
        public static TColor4 DarkOrchid => new TColor4(153, 50, 204, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (139, 0, 0, 255).
        /// </summary>
        public static TColor4 DarkRed => new TColor4(139, 0, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (233, 150, 122, 255).
        /// </summary>
        public static TColor4 DarkSalmon => new TColor4(233, 150, 122, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (143, 188, 139, 255).
        /// </summary>
        public static TColor4 DarkSeaGreen => new TColor4(143, 188, 139, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (72, 61, 139, 255).
        /// </summary>
        public static TColor4 DarkSlateBlue => new TColor4(72, 61, 139, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (47, 79, 79, 255).
        /// </summary>
        public static TColor4 DarkSlateGray => new TColor4(47, 79, 79, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 206, 209, 255).
        /// </summary>
        public static TColor4 DarkTurquoise => new TColor4(0, 206, 209, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (148, 0, 211, 255).
        /// </summary>
        public static TColor4 DarkViolet => new TColor4(148, 0, 211, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 20, 147, 255).
        /// </summary>
        public static TColor4 DeepPink => new TColor4(255, 20, 147, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 191, 255, 255).
        /// </summary>
        public static TColor4 DeepSkyBlue => new TColor4(0, 191, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (105, 105, 105, 255).
        /// </summary>
        public static TColor4 DimGray => new TColor4(105, 105, 105, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (30, 144, 255, 255).
        /// </summary>
        public static TColor4 DodgerBlue => new TColor4(30, 144, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (178, 34, 34, 255).
        /// </summary>
        public static TColor4 Firebrick => new TColor4(178, 34, 34, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 250, 240, 255).
        /// </summary>
        public static TColor4 FloralWhite => new TColor4(255, 250, 240, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (34, 139, 34, 255).
        /// </summary>
        public static TColor4 ForestGreen => new TColor4(34, 139, 34, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 0, 255, 255).
        /// </summary>
        public static TColor4 Fuchsia => new TColor4(255, 0, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (220, 220, 220, 255).
        /// </summary>
        public static TColor4 Gainsboro => new TColor4(220, 220, 220, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (248, 248, 255, 255).
        /// </summary>
        public static TColor4 GhostWhite => new TColor4(248, 248, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 215, 0, 255).
        /// </summary>
        public static TColor4 Gold => new TColor4(255, 215, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (218, 165, 32, 255).
        /// </summary>
        public static TColor4 Goldenrod => new TColor4(218, 165, 32, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (128, 128, 128, 255).
        /// </summary>
        public static TColor4 Gray => new TColor4(128, 128, 128, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 128, 0, 255).
        /// </summary>
        public static TColor4 Green => new TColor4(0, 128, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (173, 255, 47, 255).
        /// </summary>
        public static TColor4 GreenYellow => new TColor4(173, 255, 47, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (240, 255, 240, 255).
        /// </summary>
        public static TColor4 Honeydew => new TColor4(240, 255, 240, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 105, 180, 255).
        /// </summary>
        public static TColor4 HotPink => new TColor4(255, 105, 180, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (205, 92, 92, 255).
        /// </summary>
        public static TColor4 IndianRed => new TColor4(205, 92, 92, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (75, 0, 130, 255).
        /// </summary>
        public static TColor4 Indigo => new TColor4(75, 0, 130, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 255, 240, 255).
        /// </summary>
        public static TColor4 Ivory => new TColor4(255, 255, 240, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (240, 230, 140, 255).
        /// </summary>
        public static TColor4 Khaki => new TColor4(240, 230, 140, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (230, 230, 250, 255).
        /// </summary>
        public static TColor4 Lavender => new TColor4(230, 230, 250, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 240, 245, 255).
        /// </summary>
        public static TColor4 LavenderBlush => new TColor4(255, 240, 245, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (124, 252, 0, 255).
        /// </summary>
        public static TColor4 LawnGreen => new TColor4(124, 252, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 250, 205, 255).
        /// </summary>
        public static TColor4 LemonChiffon => new TColor4(255, 250, 205, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (173, 216, 230, 255).
        /// </summary>
        public static TColor4 LightBlue => new TColor4(173, 216, 230, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (240, 128, 128, 255).
        /// </summary>
        public static TColor4 LightCoral => new TColor4(240, 128, 128, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (224, 255, 255, 255).
        /// </summary>
        public static TColor4 LightCyan => new TColor4(224, 255, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (250, 250, 210, 255).
        /// </summary>
        public static TColor4 LightGoldenrodYellow => new TColor4(250, 250, 210, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (144, 238, 144, 255).
        /// </summary>
        public static TColor4 LightGreen => new TColor4(144, 238, 144, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (211, 211, 211, 255).
        /// </summary>
        public static TColor4 LightGray => new TColor4(211, 211, 211, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 182, 193, 255).
        /// </summary>
        public static TColor4 LightPink => new TColor4(255, 182, 193, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 160, 122, 255).
        /// </summary>
        public static TColor4 LightSalmon => new TColor4(255, 160, 122, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (32, 178, 170, 255).
        /// </summary>
        public static TColor4 LightSeaGreen => new TColor4(32, 178, 170, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (135, 206, 250, 255).
        /// </summary>
        public static TColor4 LightSkyBlue => new TColor4(135, 206, 250, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (119, 136, 153, 255).
        /// </summary>
        public static TColor4 LightSlateGray => new TColor4(119, 136, 153, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (176, 196, 222, 255).
        /// </summary>
        public static TColor4 LightSteelBlue => new TColor4(176, 196, 222, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 255, 224, 255).
        /// </summary>
        public static TColor4 LightYellow => new TColor4(255, 255, 224, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 255, 0, 255).
        /// </summary>
        public static TColor4 Lime => new TColor4(0, 255, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (50, 205, 50, 255).
        /// </summary>
        public static TColor4 LimeGreen => new TColor4(50, 205, 50, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (250, 240, 230, 255).
        /// </summary>
        public static TColor4 Linen => new TColor4(250, 240, 230, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 0, 255, 255).
        /// </summary>
        public static TColor4 Magenta => new TColor4(255, 0, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (128, 0, 0, 255).
        /// </summary>
        public static TColor4 Maroon => new TColor4(128, 0, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (102, 205, 170, 255).
        /// </summary>
        public static TColor4 MediumAquamarine => new TColor4(102, 205, 170, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 0, 205, 255).
        /// </summary>
        public static TColor4 MediumBlue => new TColor4(0, 0, 205, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (186, 85, 211, 255).
        /// </summary>
        public static TColor4 MediumOrchid => new TColor4(186, 85, 211, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (147, 112, 219, 255).
        /// </summary>
        public static TColor4 MediumPurple => new TColor4(147, 112, 219, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (60, 179, 113, 255).
        /// </summary>
        public static TColor4 MediumSeaGreen => new TColor4(60, 179, 113, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (123, 104, 238, 255).
        /// </summary>
        public static TColor4 MediumSlateBlue => new TColor4(123, 104, 238, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 250, 154, 255).
        /// </summary>
        public static TColor4 MediumSpringGreen => new TColor4(0, 250, 154, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (72, 209, 204, 255).
        /// </summary>
        public static TColor4 MediumTurquoise => new TColor4(72, 209, 204, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (199, 21, 133, 255).
        /// </summary>
        public static TColor4 MediumVioletRed => new TColor4(199, 21, 133, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (25, 25, 112, 255).
        /// </summary>
        public static TColor4 MidnightBlue => new TColor4(25, 25, 112, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (245, 255, 250, 255).
        /// </summary>
        public static TColor4 MintCream => new TColor4(245, 255, 250, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 228, 225, 255).
        /// </summary>
        public static TColor4 MistyRose => new TColor4(255, 228, 225, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 228, 181, 255).
        /// </summary>
        public static TColor4 Moccasin => new TColor4(255, 228, 181, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 222, 173, 255).
        /// </summary>
        public static TColor4 NavajoWhite => new TColor4(255, 222, 173, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 0, 128, 255).
        /// </summary>
        public static TColor4 Navy => new TColor4(0, 0, 128, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (253, 245, 230, 255).
        /// </summary>
        public static TColor4 OldLace => new TColor4(253, 245, 230, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (128, 128, 0, 255).
        /// </summary>
        public static TColor4 Olive => new TColor4(128, 128, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (107, 142, 35, 255).
        /// </summary>
        public static TColor4 OliveDrab => new TColor4(107, 142, 35, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 165, 0, 255).
        /// </summary>
        public static TColor4 Orange => new TColor4(255, 165, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 69, 0, 255).
        /// </summary>
        public static TColor4 OrangeRed => new TColor4(255, 69, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (218, 112, 214, 255).
        /// </summary>
        public static TColor4 Orchid => new TColor4(218, 112, 214, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (238, 232, 170, 255).
        /// </summary>
        public static TColor4 PaleGoldenrod => new TColor4(238, 232, 170, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (152, 251, 152, 255).
        /// </summary>
        public static TColor4 PaleGreen => new TColor4(152, 251, 152, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (175, 238, 238, 255).
        /// </summary>
        public static TColor4 PaleTurquoise => new TColor4(175, 238, 238, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (219, 112, 147, 255).
        /// </summary>
        public static TColor4 PaleVioletRed => new TColor4(219, 112, 147, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 239, 213, 255).
        /// </summary>
        public static TColor4 PapayaWhip => new TColor4(255, 239, 213, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 218, 185, 255).
        /// </summary>
        public static TColor4 PeachPuff => new TColor4(255, 218, 185, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (205, 133, 63, 255).
        /// </summary>
        public static TColor4 Peru => new TColor4(205, 133, 63, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 192, 203, 255).
        /// </summary>
        public static TColor4 Pink => new TColor4(255, 192, 203, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (221, 160, 221, 255).
        /// </summary>
        public static TColor4 Plum => new TColor4(221, 160, 221, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (176, 224, 230, 255).
        /// </summary>
        public static TColor4 PowderBlue => new TColor4(176, 224, 230, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (128, 0, 128, 255).
        /// </summary>
        public static TColor4 Purple => new TColor4(128, 0, 128, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 0, 0, 255).
        /// </summary>
        public static TColor4 Red => new TColor4(255, 0, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (188, 143, 143, 255).
        /// </summary>
        public static TColor4 RosyBrown => new TColor4(188, 143, 143, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (65, 105, 225, 255).
        /// </summary>
        public static TColor4 RoyalBlue => new TColor4(65, 105, 225, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (139, 69, 19, 255).
        /// </summary>
        public static TColor4 SaddleBrown => new TColor4(139, 69, 19, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (250, 128, 114, 255).
        /// </summary>
        public static TColor4 Salmon => new TColor4(250, 128, 114, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (244, 164, 96, 255).
        /// </summary>
        public static TColor4 SandyBrown => new TColor4(244, 164, 96, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (46, 139, 87, 255).
        /// </summary>
        public static TColor4 SeaGreen => new TColor4(46, 139, 87, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 245, 238, 255).
        /// </summary>
        public static TColor4 SeaShell => new TColor4(255, 245, 238, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (160, 82, 45, 255).
        /// </summary>
        public static TColor4 Sienna => new TColor4(160, 82, 45, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (192, 192, 192, 255).
        /// </summary>
        public static TColor4 Silver => new TColor4(192, 192, 192, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (135, 206, 235, 255).
        /// </summary>
        public static TColor4 SkyBlue => new TColor4(135, 206, 235, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (106, 90, 205, 255).
        /// </summary>
        public static TColor4 SlateBlue => new TColor4(106, 90, 205, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (112, 128, 144, 255).
        /// </summary>
        public static TColor4 SlateGray => new TColor4(112, 128, 144, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 250, 250, 255).
        /// </summary>
        public static TColor4 Snow => new TColor4(255, 250, 250, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 255, 127, 255).
        /// </summary>
        public static TColor4 SpringGreen => new TColor4(0, 255, 127, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (70, 130, 180, 255).
        /// </summary>
        public static TColor4 SteelBlue => new TColor4(70, 130, 180, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (210, 180, 140, 255).
        /// </summary>
        public static TColor4 Tan => new TColor4(210, 180, 140, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (0, 128, 128, 255).
        /// </summary>
        public static TColor4 Teal => new TColor4(0, 128, 128, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (216, 191, 216, 255).
        /// </summary>
        public static TColor4 Thistle => new TColor4(216, 191, 216, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 99, 71, 255).
        /// </summary>
        public static TColor4 Tomato => new TColor4(255, 99, 71, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (64, 224, 208, 255).
        /// </summary>
        public static TColor4 Turquoise => new TColor4(64, 224, 208, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (238, 130, 238, 255).
        /// </summary>
        public static TColor4 Violet => new TColor4(238, 130, 238, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (245, 222, 179, 255).
        /// </summary>
        public static TColor4 Wheat => new TColor4(245, 222, 179, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 255, 255, 255).
        /// </summary>
        public static TColor4 White => new TColor4(255, 255, 255, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (245, 245, 245, 255).
        /// </summary>
        public static TColor4 WhiteSmoke => new TColor4(245, 245, 245, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (255, 255, 0, 255).
        /// </summary>
        public static TColor4 Yellow => new TColor4(255, 255, 0, 255);

        /// <summary>
        /// Gets the system color with (R, G, B, A) = (154, 205, 50, 255).
        /// </summary>
        public static TColor4 YellowGreen => new TColor4(154, 205, 50, 255);

        /// <summary>
        /// Converts sRGB color values to RGB color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// </returns>
        /// <param name="srgb">
        /// Color value to convert in sRGB.
        /// </param>
        [Pure]
        public static TColor4 FromSrgb(TColor4 srgb)
        {
            float r, g, b;

            if (srgb.R <= 0.04045f)
            {
                r = srgb.R / 12.92f;
            }
            else
            {
                r = (float)Math.Pow((srgb.R + 0.055f) / (1.0f + 0.055f), 2.4f);
            }

            if (srgb.G <= 0.04045f)
            {
                g = srgb.G / 12.92f;
            }
            else
            {
                g = (float)Math.Pow((srgb.G + 0.055f) / (1.0f + 0.055f), 2.4f);
            }

            if (srgb.B <= 0.04045f)
            {
                b = srgb.B / 12.92f;
            }
            else
            {
                b = (float)Math.Pow((srgb.B + 0.055f) / (1.0f + 0.055f), 2.4f);
            }

            return new TColor4(r, g, b, srgb.A);
        }

        /// <summary>
        /// Converts RGB color values to sRGB color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// </returns>
        /// <param name="rgb">Color value to convert.</param>
        [Pure]
        public static TColor4 ToSrgb(TColor4 rgb)
        {
            float r, g, b;

            if (rgb.R <= 0.0031308)
            {
                r = 12.92f * rgb.R;
            }
            else
            {
                r = ((1.0f + 0.055f) * (float)Math.Pow(rgb.R, 1.0f / 2.4f)) - 0.055f;
            }

            if (rgb.G <= 0.0031308)
            {
                g = 12.92f * rgb.G;
            }
            else
            {
                g = ((1.0f + 0.055f) * (float)Math.Pow(rgb.G, 1.0f / 2.4f)) - 0.055f;
            }

            if (rgb.B <= 0.0031308)
            {
                b = 12.92f * rgb.B;
            }
            else
            {
                b = ((1.0f + 0.055f) * (float)Math.Pow(rgb.B, 1.0f / 2.4f)) - 0.055f;
            }

            return new TColor4(r, g, b, rgb.A);
        }

        /// <summary>
        /// Converts HSL color values to RGB color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// </returns>
        /// <param name="hsl">
        /// Color value to convert in hue, saturation, lightness (HSL).
        /// The X element is Hue (H), the Y element is Saturation (S), the Z element is Lightness (L), and the W element is
        /// Alpha (which is copied to the output's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </param>
        [Pure]
        public static TColor4 FromHsl(TVector4 hsl)
        {
            var hue = hsl.X * 360.0f;
            var saturation = hsl.Y;
            var lightness = hsl.Z;

            var c = (1.0f - Math.Abs((2.0f * lightness) - 1.0f)) * saturation;

            var h = hue / 60.0f;
            var x = c * (1.0f - Math.Abs((h % 2.0f) - 1.0f));

            float r, g, b;
            if (h >= 0.0f && h < 1.0f)
            {
                r = c;
                g = x;
                b = 0.0f;
            }
            else if (h >= 1.0f && h < 2.0f)
            {
                r = x;
                g = c;
                b = 0.0f;
            }
            else if (h >= 2.0f && h < 3.0f)
            {
                r = 0.0f;
                g = c;
                b = x;
            }
            else if (h >= 3.0f && h < 4.0f)
            {
                r = 0.0f;
                g = x;
                b = c;
            }
            else if (h >= 4.0f && h < 5.0f)
            {
                r = x;
                g = 0.0f;
                b = c;
            }
            else if (h >= 5.0f && h < 6.0f)
            {
                r = c;
                g = 0.0f;
                b = x;
            }
            else
            {
                r = 0.0f;
                g = 0.0f;
                b = 0.0f;
            }

            var m = lightness - (c / 2.0f);
            if (m < 0)
            {
                m = 0;
            }
            return new TColor4(r + m, g + m, b + m, hsl.W);
        }

        /// <summary>
        /// Converts RGB color values to HSL color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// The X element is Hue (H), the Y element is Saturation (S), the Z element is Lightness (L), and the W element is
        /// Alpha (a copy of the input's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </returns>
        /// <param name="rgb">Color value to convert.</param>
        [Pure]
        public static TVector4 ToHsl(TColor4 rgb)
        {
            var max = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
            var min = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
            var diff = max - min;

            var h = 0.0f;
            if (diff == 0)
            {
                h = 0.0f;
            }
            else if (max == rgb.R)
            {
                h = ((rgb.G - rgb.B) / diff) % 6;
                if (h < 0)
                {
                    h += 6;
                }
            }
            else if (max == rgb.G)
            {
                h = ((rgb.B - rgb.R) / diff) + 2.0f;
            }
            else if (max == rgb.B)
            {
                h = ((rgb.R - rgb.G) / diff) + 4.0f;
            }

            var hue = h / 6.0f;
            if (hue < 0.0f)
            {
                hue += 1.0f;
            }

            var lightness = (max + min) / 2.0f;

            var saturation = 0.0f;
            if ((1.0f - Math.Abs((2.0f * lightness) - 1.0f)) != 0)
            {
                saturation = diff / (1.0f - Math.Abs((2.0f * lightness) - 1.0f));
            }

            return new TVector4(hue, saturation, lightness, rgb.A);
        }

        /// <summary>
        /// Converts HSV color values to RGB color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// </returns>
        /// <param name="hsv">
        /// Color value to convert in hue, saturation, value (HSV).
        /// The X element is Hue (H), the Y element is Saturation (S), the Z element is Value (V), and the W element is Alpha
        /// (which is copied to the output's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </param>
        [Pure]
        public static TColor4 FromHsv(TVector4 hsv)
        {
            var hue = hsv.X * 360.0f;
            var saturation = hsv.Y;
            var value = hsv.Z;

            var c = value * saturation;

            var h = hue / 60.0f;
            var x = c * (1.0f - Math.Abs((h % 2.0f) - 1.0f));

            float r, g, b;
            if (h >= 0.0f && h < 1.0f)
            {
                r = c;
                g = x;
                b = 0.0f;
            }
            else if (h >= 1.0f && h < 2.0f)
            {
                r = x;
                g = c;
                b = 0.0f;
            }
            else if (h >= 2.0f && h < 3.0f)
            {
                r = 0.0f;
                g = c;
                b = x;
            }
            else if (h >= 3.0f && h < 4.0f)
            {
                r = 0.0f;
                g = x;
                b = c;
            }
            else if (h >= 4.0f && h < 5.0f)
            {
                r = x;
                g = 0.0f;
                b = c;
            }
            else if (h >= 5.0f && h < 6.0f)
            {
                r = c;
                g = 0.0f;
                b = x;
            }
            else
            {
                r = 0.0f;
                g = 0.0f;
                b = 0.0f;
            }

            var m = value - c;
            return new TColor4(r + m, g + m, b + m, hsv.W);
        }

        /// <summary>
        /// Converts RGB color values to HSV color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// The X element is Hue (H), the Y element is Saturation (S), the Z element is Value (V), and the W element is Alpha
        /// (a copy of the input's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </returns>
        /// <param name="rgb">Color value to convert.</param>
        [Pure]
        public static TVector4 ToHsv(TColor4 rgb)
        {
            var max = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
            var min = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
            var diff = max - min;

            var h = 0.0f;
            if (diff == 0)
            {
                h = 0.0f;
            }
            else if (max == rgb.R)
            {
                h = ((rgb.G - rgb.B) / diff) % 6.0f;
                if (h < 0)
                {
                    h += 6f;
                }
            }
            else if (max == rgb.G)
            {
                h = ((rgb.B - rgb.R) / diff) + 2.0f;
            }
            else if (max == rgb.B)
            {
                h = ((rgb.R - rgb.G) / diff) + 4.0f;
            }

            var hue = h * 60.0f / 360.0f;

            var saturation = 0.0f;
            if (max != 0.0f)
            {
                saturation = diff / max;
            }

            return new TVector4(hue, saturation, max, rgb.A);
        }

        /// <summary>
        /// Converts XYZ color values to RGB color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// </returns>
        /// <param name="xyz">
        /// Color value to convert with the trisimulus values of X, Y, and Z in the corresponding element, and the W element
        /// with Alpha (which is copied to the output's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </param>
        /// <remarks>Uses the CIE XYZ colorspace.</remarks>
        [Pure]
        public static TColor4 FromXyz(TVector4 xyz)
        {
            var r = (0.41847f * xyz.X) + (-0.15866f * xyz.Y) + (-0.082835f * xyz.Z);
            var g = (-0.091169f * xyz.X) + (0.25243f * xyz.Y) + (0.015708f * xyz.Z);
            var b = (0.00092090f * xyz.X) + (-0.0025498f * xyz.Y) + (0.17860f * xyz.Z);
            return new TColor4(r, g, b, xyz.W);
        }

        /// <summary>
        /// Converts RGB color values to XYZ color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value with the trisimulus values of X, Y, and Z in the corresponding element, and the W
        /// element with Alpha (a copy of the input's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </returns>
        /// <param name="rgb">Color value to convert.</param>
        /// <remarks>Uses the CIE XYZ colorspace.</remarks>
        [Pure]
        public static TVector4 ToXyz(TColor4 rgb)
        {
            var x = ((0.49f * rgb.R) + (0.31f * rgb.G) + (0.20f * rgb.B)) / 0.17697f;
            var y = ((0.17697f * rgb.R) + (0.81240f * rgb.G) + (0.01063f * rgb.B)) / 0.17697f;
            var z = ((0.00f * rgb.R) + (0.01f * rgb.G) + (0.99f * rgb.B)) / 0.17697f;
            return new TVector4(x, y, z, rgb.A);
        }

        /// <summary>
        /// Converts YCbCr color values to RGB color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// </returns>
        /// <param name="ycbcr">
        /// Color value to convert in Luma-Chrominance (YCbCr) aka YUV.
        /// The X element contains Luma (Y, 0.0 to 1.0), the Y element contains Blue-difference chroma (U, -0.5 to 0.5), the Z
        /// element contains the Red-difference chroma (V, -0.5 to 0.5), and the W element contains the Alpha (which is copied
        /// to the output's Alpha value).
        /// </param>
        /// <remarks>Converts using ITU-R BT.601/CCIR 601 W(r) = 0.299 W(b) = 0.114 U(max) = 0.436 V(max) = 0.615.</remarks>
        [Pure]
        public static TColor4 FromYcbcr(TVector4 ycbcr)
        {
            var r = (1.0f * ycbcr.X) + (0.0f * ycbcr.Y) + (1.402f * ycbcr.Z);
            var g = (1.0f * ycbcr.X) + (-0.344136f * ycbcr.Y) + (-0.714136f * ycbcr.Z);
            var b = (1.0f * ycbcr.X) + (1.772f * ycbcr.Y) + (0.0f * ycbcr.Z);
            return new TColor4(r, g, b, ycbcr.W);
        }

        /// <summary>
        /// Converts RGB color values to YUV color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value in Luma-Chrominance (YCbCr) aka YUV.
        /// The X element contains Luma (Y, 0.0 to 1.0), the Y element contains Blue-difference chroma (U, -0.5 to 0.5), the Z
        /// element contains the Red-difference chroma (V, -0.5 to 0.5), and the W element contains the Alpha (a copy of the
        /// input's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </returns>
        /// <param name="rgb">Color value to convert.</param>
        /// <remarks>Converts using ITU-R BT.601/CCIR 601 W(r) = 0.299 W(b) = 0.114 U(max) = 0.436 V(max) = 0.615.</remarks>
        [Pure]
        public static TVector4 ToYcbcr(TColor4 rgb)
        {
            var y = (0.299f * rgb.R) + (0.587f * rgb.G) + (0.114f * rgb.B);
            var u = (-0.168736f * rgb.R) + (-0.331264f * rgb.G) + (0.5f * rgb.B);
            var v = (0.5f * rgb.R) + (-0.418688f * rgb.G) + (-0.081312f * rgb.B);
            return new TVector4(y, u, v, rgb.A);
        }

        /// <summary>
        /// Converts HCY color values to RGB color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// </returns>
        /// <param name="hcy">
        /// Color value to convert in hue, chroma, luminance (HCY).
        /// The X element is Hue (H), the Y element is Chroma (C), the Z element is luminance (Y), and the W element is Alpha
        /// (which is copied to the output's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </param>
        [Pure]
        public static TColor4 FromHcy(TVector4 hcy)
        {
            var hue = hcy.X * 360.0f;
            var y = hcy.Y;
            var luminance = hcy.Z;

            var h = hue / 60.0f;
            var x = y * (1.0f - Math.Abs((h % 2.0f) - 1.0f));

            float r, g, b;
            if (h >= 0.0f && h < 1.0f)
            {
                r = y;
                g = x;
                b = 0.0f;
            }
            else if (h >= 1.0f && h < 2.0f)
            {
                r = x;
                g = y;
                b = 0.0f;
            }
            else if (h >= 2.0f && h < 3.0f)
            {
                r = 0.0f;
                g = y;
                b = x;
            }
            else if (h >= 3.0f && h < 4.0f)
            {
                r = 0.0f;
                g = x;
                b = y;
            }
            else if (h >= 4.0f && h < 5.0f)
            {
                r = x;
                g = 0.0f;
                b = y;
            }
            else if (h >= 5.0f && h < 6.0f)
            {
                r = y;
                g = 0.0f;
                b = x;
            }
            else
            {
                r = 0.0f;
                g = 0.0f;
                b = 0.0f;
            }

            var m = luminance - (0.30f * r) + (0.59f * g) + (0.11f * b);
            return new TColor4(r + m, g + m, b + m, hcy.W);
        }

        /// <summary>
        /// Converts RGB color values to HCY color values.
        /// </summary>
        /// <returns>
        /// Returns the converted color value.
        /// The X element is Hue (H), the Y element is Chroma (C), the Z element is luminance (Y), and the W element is Alpha
        /// (a copy of the input's Alpha value).
        /// Each has a range of 0.0 to 1.0.
        /// </returns>
        /// <param name="rgb">Color value to convert.</param>
        [Pure]
        public static TVector4 ToHcy(TColor4 rgb)
        {
            var max = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
            var min = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
            var diff = max - min;

            var h = 0.0f;
            if (max == rgb.R)
            {
                h = ((rgb.G - rgb.B) / diff) % 6.0f;
            }
            else if (max == rgb.G)
            {
                h = ((rgb.B - rgb.R) / diff) + 2.0f;
            }
            else if (max == rgb.B)
            {
                h = ((rgb.R - rgb.G) / diff) + 4.0f;
            }

            var hue = h * 60.0f / 360.0f;

            var luminance = (0.30f * rgb.R) + (0.59f * rgb.G) + (0.11f * rgb.B);

            return new TVector4(hue, diff, luminance, rgb.A);
        }

        /// <summary>
        /// Compares whether this TColor4 structure is equal to the specified TColor4.
        /// </summary>
        /// <param name="other">The TColor4 structure to compare to.</param>
        /// <returns>True if both TColor4 structures contain the same components; false otherwise.</returns>
        [Pure]
        public bool Equals(TColor4 other)
        {
            return
                R == other.R &&
                G == other.G &&
                B == other.B &&
                A == other.A;
        }
    }
}
