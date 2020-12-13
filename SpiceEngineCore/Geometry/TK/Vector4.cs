using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using SpiceEngineCore.Geometry.Colors;
using SpiceEngineCore.Geometry.Matrices;
using SpiceEngineCore.Geometry.Quaternions;
using SpiceEngineCore.Utilities;
using Color4 = SpiceEngineCore.Geometry.Colors.TColor4;
using Matrix2 = SpiceEngineCore.Geometry.Matrices.TMatrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrices.TMatrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrices.TMatrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternions.TQuaternion;
using Vector2 = SpiceEngineCore.Geometry.Vectors.TVector2;
using Vector3 = SpiceEngineCore.Geometry.Vectors.TVector3;
using Vector4 = SpiceEngineCore.Geometry.Vectors.TVector4;

namespace SpiceEngineCore.Geometry.Vectors
{
    /// <summary>
    /// Represents a 4D vector using four single-precision floating-point numbers.
    /// </summary>
    /// <remarks>
    /// The TVector4 structure is suitable for interoperation with unmanaged code requiring four consecutive floats.
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct TVector4 : IEquatable<TVector4>
    {
        /// <summary>
        /// The X component of the TVector4.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component of the TVector4.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z component of the TVector4.
        /// </summary>
        public float Z;

        /// <summary>
        /// The W component of the TVector4.
        /// </summary>
        public float W;

        /// <summary>
        /// Defines a unit-length TVector4 that points towards the X-axis.
        /// </summary>
        public static readonly TVector4 UnitX = new TVector4(1, 0, 0, 0);

        /// <summary>
        /// Defines a unit-length TVector4 that points towards the Y-axis.
        /// </summary>
        public static readonly TVector4 UnitY = new TVector4(0, 1, 0, 0);

        /// <summary>
        /// Defines a unit-length TVector4 that points towards the Z-axis.
        /// </summary>
        public static readonly TVector4 UnitZ = new TVector4(0, 0, 1, 0);

        /// <summary>
        /// Defines a unit-length TVector4 that points towards the W-axis.
        /// </summary>
        public static readonly TVector4 UnitW = new TVector4(0, 0, 0, 1);

        /// <summary>
        /// Defines a zero-length TVector4.
        /// </summary>
        public static readonly TVector4 Zero = new TVector4(0, 0, 0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly TVector4 One = new TVector4(1, 1, 1, 1);

        /// <summary>
        /// Defines an instance with all components set to positive infinity.
        /// </summary>
        public static readonly TVector4 PositiveInfinity = new TVector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        /// <summary>
        /// Defines an instance with all components set to negative infinity.
        /// </summary>
        public static readonly TVector4 NegativeInfinity = new TVector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        /// <summary>
        /// Defines the size of the TVector4 struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = UnitConversions.SizeOf<TVector4>();//Unsafe.SizeOf<TVector4>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TVector4"/> struct.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public TVector4(float value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TVector4"/> struct.
        /// </summary>
        /// <param name="x">The x component of the TVector4.</param>
        /// <param name="y">The y component of the TVector4.</param>
        /// <param name="z">The z component of the TVector4.</param>
        /// <param name="w">The w component of the TVector4.</param>
        public TVector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TVector4"/> struct.
        /// </summary>
        /// <param name="v">The TVector2 to copy components from.</param>
        public TVector4(TVector2 v)
        {
            X = v.X;
            Y = v.Y;
            Z = 0.0f;
            W = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TVector4"/> struct.
        /// </summary>
        /// <param name="v">The TVector3 to copy components from.</param>
        /// <remarks>
        ///  .<seealso cref="TVector4(TVector3, float)"/>
        /// </remarks>
        public TVector4(TVector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = 0.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TVector4"/> struct.
        /// </summary>
        /// <param name="v">The TVector3 to copy components from.</param>
        /// <param name="w">The w component of the new TVector4.</param>
        public TVector4(TVector3 v, float w)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TVector4"/> struct.
        /// </summary>
        /// <param name="v">The TVector4 to copy components from.</param>
        public TVector4(TVector4 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = v.W;
        }

        /// <summary>
        /// Gets or sets the value at the index of the Vector.
        /// </summary>
        /// <param name="index">The index of the component from the Vector.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than 0 or greater than 3.</exception>
        public float this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return X;
                }

                if (index == 1)
                {
                    return Y;
                }

                if (index == 2)
                {
                    return Z;
                }

                if (index == 3)
                {
                    return W;
                }

                throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
            }

            set
            {
                if (index == 0)
                {
                    X = value;
                }
                else if (index == 1)
                {
                    Y = value;
                }
                else if (index == 2)
                {
                    Z = value;
                }
                else if (index == 3)
                {
                    W = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
                }
            }
        }

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        /// <see cref="LengthFast"/>
        /// <seealso cref="LengthSquared"/>
        public float Length => (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));

        /// <summary>
        /// Gets an approximation of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property uses an approximation of the square root function to calculate vector magnitude, with
        /// an upper error bound of 0.001.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthSquared"/>
        public float LengthFast => 1.0f / MathHelper.InverseSqrtFast((X * X) + (Y * Y) + (Z * Z) + (W * W));

        /// <summary>
        /// Gets the square of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
        /// for comparisons.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthFast"/>
        public float LengthSquared => (X * X) + (Y * Y) + (Z * Z) + (W * W);

        /// <summary>
        /// Returns a copy of the TVector4 scaled to unit length.
        /// </summary>
        /// <returns>The normalized copy.</returns>
        public TVector4 Normalized()
        {
            var v = this;
            v.Normalize();
            return v;
        }

        /// <summary>
        /// Scales the TVector4 to unit length.
        /// </summary>
        public void Normalize()
        {
            var scale = 1.0f / Length;
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }

        /// <summary>
        /// Scales the TVector4 to approximately unit length.
        /// </summary>
        public void NormalizeFast()
        {
            var scale = MathHelper.InverseSqrtFast((X * X) + (Y * Y) + (Z * Z) + (W * W));
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Result of operation.</returns>
        [Pure]
        public static TVector4 Add(TVector4 a, TVector4 b)
        {
            Add(in a, in b, out a);
            return a;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <param name="result">Result of operation.</param>
        public static void Add(in TVector4 a, in TVector4 b, out TVector4 result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
            result.W = a.W + b.W;
        }

        /// <summary>
        /// Subtract one Vector from another.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>Result of subtraction.</returns>
        [Pure]
        public static TVector4 Subtract(TVector4 a, TVector4 b)
        {
            Subtract(in a, in b, out a);
            return a;
        }

        /// <summary>
        /// Subtract one Vector from another.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Result of subtraction.</param>
        public static void Subtract(in TVector4 a, in TVector4 b, out TVector4 result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
            result.W = a.W - b.W;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static TVector4 Multiply(TVector4 vector, float scale)
        {
            Multiply(in vector, scale, out vector);
            return vector;
        }

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(in TVector4 vector, float scale, out TVector4 result)
        {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
            result.Z = vector.Z * scale;
            result.W = vector.W * scale;
        }

        /// <summary>
        /// Multiplies a vector by the components a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static TVector4 Multiply(TVector4 vector, TVector4 scale)
        {
            Multiply(in vector, in scale, out vector);
            return vector;
        }

        /// <summary>
        /// Multiplies a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Multiply(in TVector4 vector, in TVector4 scale, out TVector4 result)
        {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
            result.Z = vector.Z * scale.Z;
            result.W = vector.W * scale.W;
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static TVector4 Divide(TVector4 vector, float scale)
        {
            Divide(in vector, scale, out vector);
            return vector;
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(in TVector4 vector, float scale, out TVector4 result)
        {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
            result.Z = vector.Z / scale;
            result.W = vector.W / scale;
        }

        /// <summary>
        /// Divides a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <returns>Result of the operation.</returns>
        [Pure]
        public static TVector4 Divide(TVector4 vector, TVector4 scale)
        {
            Divide(in vector, in scale, out vector);
            return vector;
        }

        /// <summary>
        /// Divide a vector by the components of a vector (scale).
        /// </summary>
        /// <param name="vector">Left operand.</param>
        /// <param name="scale">Right operand.</param>
        /// <param name="result">Result of the operation.</param>
        public static void Divide(in TVector4 vector, in TVector4 scale, out TVector4 result)
        {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
            result.Z = vector.Z / scale.Z;
            result.W = vector.W / scale.W;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>The component-wise minimum.</returns>
        [Pure]
        public static TVector4 ComponentMin(TVector4 a, TVector4 b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            a.Z = a.Z < b.Z ? a.Z : b.Z;
            a.W = a.W < b.W ? a.W : b.W;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the smallest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">The component-wise minimum.</param>
        public static void ComponentMin(in TVector4 a, in TVector4 b, out TVector4 result)
        {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
            result.Z = a.Z < b.Z ? a.Z : b.Z;
            result.W = a.W < b.W ? a.W : b.W;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>The component-wise maximum.</returns>
        [Pure]
        public static TVector4 ComponentMax(TVector4 a, TVector4 b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            a.Z = a.Z > b.Z ? a.Z : b.Z;
            a.W = a.W > b.W ? a.W : b.W;
            return a;
        }

        /// <summary>
        /// Returns a vector created from the largest of the corresponding components of the given vectors.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">The component-wise maximum.</param>
        public static void ComponentMax(in TVector4 a, in TVector4 b, out TVector4 result)
        {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
            result.Z = a.Z > b.Z ? a.Z : b.Z;
            result.W = a.W > b.W ? a.W : b.W;
        }

        /// <summary>
        /// Returns the TVector4 with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>The minimum TVector4.</returns>
        [Pure]
        public static TVector4 MagnitudeMin(TVector4 left, TVector4 right)
        {
            return left.LengthSquared < right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Returns the TVector4 with the minimum magnitude. If the magnitudes are equal, the second vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <param name="result">The magnitude-wise minimum.</param>
        public static void MagnitudeMin(in TVector4 left, in TVector4 right, out TVector4 result)
        {
            result = left.LengthSquared < right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Returns the TVector4 with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>The maximum TVector4.</returns>
        [Pure]
        public static TVector4 MagnitudeMax(TVector4 left, TVector4 right)
        {
            return left.LengthSquared >= right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Returns the TVector4 with the maximum magnitude. If the magnitudes are equal, the first vector
        /// is selected.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <param name="result">The magnitude-wise maximum.</param>
        public static void MagnitudeMax(in TVector4 left, in TVector4 right, out TVector4 result)
        {
            result = left.LengthSquared >= right.LengthSquared ? left : right;
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors.
        /// </summary>
        /// <param name="vec">Input vector.</param>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        /// <returns>The clamped vector.</returns>
        [Pure]
        public static TVector4 Clamp(TVector4 vec, TVector4 min, TVector4 max)
        {
            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            vec.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
            vec.W = vec.W < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
            return vec;
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors.
        /// </summary>
        /// <param name="vec">Input vector.</param>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        /// <param name="result">The clamped vector.</param>
        public static void Clamp(in TVector4 vec, in TVector4 min, in TVector4 max, out TVector4 result)
        {
            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            result.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
            result.W = vec.W < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
        }

        /// <summary>
        /// Scale a vector to unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <returns>The normalized copy.</returns>
        [Pure]
        public static TVector4 Normalize(TVector4 vec)
        {
            var scale = 1.0f / vec.Length;
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Scale a vector to unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <param name="result">The normalized vector.</param>
        public static void Normalize(in TVector4 vec, out TVector4 result)
        {
            var scale = 1.0f / vec.Length;
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
            result.W = vec.W * scale;
        }

        /// <summary>
        /// Scale a vector to approximately unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <returns>The normalized copy.</returns>
        [Pure]
        public static TVector4 NormalizeFast(TVector4 vec)
        {
            var scale = MathHelper.InverseSqrtFast((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z) + (vec.W * vec.W));
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Scale a vector to approximately unit length.
        /// </summary>
        /// <param name="vec">The input vector.</param>
        /// <param name="result">The normalized copy.</param>
        public static void NormalizeFast(in TVector4 vec, out TVector4 result)
        {
            var scale = MathHelper.InverseSqrtFast((vec.X * vec.X) + (vec.Y * vec.Y) + (vec.Z * vec.Z) + (vec.W * vec.W));
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
            result.W = vec.W * scale;
        }

        /// <summary>
        /// Calculate the dot product of two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <returns>The dot product of the two inputs.</returns>
        [Pure]
        public static float Dot(TVector4 left, TVector4 right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        /// <summary>
        /// Calculate the dot product of two vectors.
        /// </summary>
        /// <param name="left">First operand.</param>
        /// <param name="right">Second operand.</param>
        /// <param name="result">The dot product of the two inputs.</param>
        public static void Dot(in TVector4 left, in TVector4 right, out float result)
        {
            result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise.</returns>
        [Pure]
        public static TVector4 Lerp(TVector4 a, TVector4 b, float blend)
        {
            a.X = (blend * (b.X - a.X)) + a.X;
            a.Y = (blend * (b.Y - a.Y)) + a.Y;
            a.Z = (blend * (b.Z - a.Z)) + a.Z;
            a.W = (blend * (b.W - a.W)) + a.W;
            return a;
        }

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors.
        /// </summary>
        /// <param name="a">First input vector.</param>
        /// <param name="b">Second input vector.</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise.</param>
        public static void Lerp(in TVector4 a, in TVector4 b, float blend, out TVector4 result)
        {
            result.X = (blend * (b.X - a.X)) + a.X;
            result.Y = (blend * (b.Y - a.Y)) + a.Y;
            result.Z = (blend * (b.Z - a.Z)) + a.Z;
            result.W = (blend * (b.W - a.W)) + a.W;
        }

        /// <summary>
        /// Interpolate 3 Vectors using Barycentric coordinates.
        /// </summary>
        /// <param name="a">First input Vector.</param>
        /// <param name="b">Second input Vector.</param>
        /// <param name="c">Third input Vector.</param>
        /// <param name="u">First Barycentric Coordinate.</param>
        /// <param name="v">Second Barycentric Coordinate.</param>
        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise.</returns>
        [Pure]
        public static TVector4 BaryCentric(TVector4 a, TVector4 b, TVector4 c, float u, float v)
        {
            BaryCentric(in a, in b, in c, u, v, out var result);
            return result;
        }

        /// <summary>
        /// Interpolate 3 Vectors using Barycentric coordinates.
        /// </summary>
        /// <param name="a">First input Vector.</param>
        /// <param name="b">Second input Vector.</param>
        /// <param name="c">Third input Vector.</param>
        /// <param name="u">First Barycentric Coordinate.</param>
        /// <param name="v">Second Barycentric Coordinate.</param>
        /// <param name="result">
        /// Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c
        /// otherwise.
        /// </param>
        public static void BaryCentric
        (
            in TVector4 a,
            in TVector4 b,
            in TVector4 c,
            float u,
            float v,
            out TVector4 result
        )
        {
            Subtract(in b, in a, out var ab);
            Multiply(in ab, u, out var abU);
            Add(in a, in abU, out var uPos);

            Subtract(in c, in a, out var ac);
            Multiply(in ac, v, out var acV);
            Add(in uPos, in acV, out result);
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static TVector4 TransformRow(TVector4 vec, TMatrix4 mat)
        {
            TransformRow(in vec, in mat, out TVector4 result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransformRow(in TVector4 vec, in TMatrix4 mat, out TVector4 result)
        {
            result = new TVector4(
                (vec.X * mat.Row0.X) + (vec.Y * mat.Row1.X) + (vec.Z * mat.Row2.X) + (vec.W * mat.Row3.X),
                (vec.X * mat.Row0.Y) + (vec.Y * mat.Row1.Y) + (vec.Z * mat.Row2.Y) + (vec.W * mat.Row3.Y),
                (vec.X * mat.Row0.Z) + (vec.Y * mat.Row1.Z) + (vec.Z * mat.Row2.Z) + (vec.W * mat.Row3.Z),
                (vec.X * mat.Row0.W) + (vec.Y * mat.Row1.W) + (vec.Z * mat.Row2.W) + (vec.W * mat.Row3.W));
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <returns>The result of the operation.</returns>
        [Pure]
        public static TVector4 Transform(TVector4 vec, TQuaternion quat)
        {
            Transform(in vec, in quat, out TVector4 result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="result">The result of the operation.</param>
        public static void Transform(in TVector4 vec, in TQuaternion quat, out TVector4 result)
        {
            TQuaternion v = new TQuaternion(vec.X, vec.Y, vec.Z, vec.W);
            TQuaternion.Invert(in quat, out TQuaternion i);
            TQuaternion.Multiply(in quat, in v, out TQuaternion t);
            TQuaternion.Multiply(in t, in i, out v);

            result.X = v.X;
            result.Y = v.Y;
            result.Z = v.Z;
            result.W = v.W;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static TVector4 TransformColumn(TMatrix4 mat, TVector4 vec)
        {
            TransformColumn(in mat, in vec, out TVector4 result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransformColumn(in TMatrix4 mat, in TVector4 vec, out TVector4 result)
        {
            result = new TVector4(
                (mat.Row0.X * vec.X) + (mat.Row0.Y * vec.Y) + (mat.Row0.Z * vec.Z) + (mat.Row0.W * vec.W),
                (mat.Row1.X * vec.X) + (mat.Row1.Y * vec.Y) + (mat.Row1.Z * vec.Z) + (mat.Row1.W * vec.W),
                (mat.Row2.X * vec.X) + (mat.Row2.Y * vec.Y) + (mat.Row2.Z * vec.Z) + (mat.Row2.W * vec.W),
                (mat.Row3.X * vec.X) + (mat.Row3.Y * vec.Y) + (mat.Row3.Z * vec.Z) + (mat.Row3.W * vec.W));
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the X and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Xy
        {
            get => new TVector2(X, Y);// Unsafe.As<TVector4, TVector2>(ref this);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the X and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Xz
        {
            get => new TVector2(X, Z);
            set
            {
                X = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the X and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Xw
        {
            get => new TVector2(X, W);
            set
            {
                X = value.X;
                W = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the Y and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Yx
        {
            get => new TVector2(Y, X);
            set
            {
                Y = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the Y and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Yz
        {
            get => new TVector2(Y, Z);
            set
            {
                Y = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the Y and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Yw
        {
            get => new TVector2(Y, W);
            set
            {
                Y = value.X;
                W = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the Z and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Zx
        {
            get => new TVector2(Z, X);
            set
            {
                Z = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the Z and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Zy
        {
            get => new TVector2(Z, Y);
            set
            {
                Z = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the Z and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Zw
        {
            get => new TVector2(Z, W);
            set
            {
                Z = value.X;
                W = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the W and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Wx
        {
            get => new TVector2(W, X);
            set
            {
                W = value.X;
                X = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the W and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Wy
        {
            get => new TVector2(W, Y);
            set
            {
                W = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector2 with the W and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector2 Wz
        {
            get => new TVector2(W, Z);
            set
            {
                W = value.X;
                Z = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Xyz
        {
            get => new TVector3(X, Y, Z);//Unsafe.As<TVector4, TVector3>(ref this);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Xyw
        {
            get => new TVector3(X, Y, W);
            set
            {
                X = value.X;
                Y = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the X, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Xzy
        {
            get => new TVector3(X, Z, Y);
            set
            {
                X = value.X;
                Z = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the X, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Xzw
        {
            get => new TVector3(X, Z, W);
            set
            {
                X = value.X;
                Z = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the X, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Xwy
        {
            get => new TVector3(X, W, Y);
            set
            {
                X = value.X;
                W = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the X, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Xwz
        {
            get => new TVector3(X, W, Z);
            set
            {
                X = value.X;
                W = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Y, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Yxz
        {
            get => new TVector3(Y, X, Z);
            set
            {
                Y = value.X;
                X = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Y, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Yxw
        {
            get => new TVector3(Y, X, W);
            set
            {
                Y = value.X;
                X = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Y, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Yzx
        {
            get => new TVector3(Y, Z, X);
            set
            {
                Y = value.X;
                Z = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Y, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Yzw
        {
            get => new TVector3(Y, Z, W);
            set
            {
                Y = value.X;
                Z = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Y, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Ywx
        {
            get => new TVector3(Y, W, X);
            set
            {
                Y = value.X;
                W = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Ywz
        {
            get => new TVector3(Y, W, Z);
            set
            {
                Y = value.X;
                W = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Z, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Zxy
        {
            get => new TVector3(Z, X, Y);
            set
            {
                Z = value.X;
                X = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Z, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Zxw
        {
            get => new TVector3(Z, X, W);
            set
            {
                Z = value.X;
                X = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Z, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Zyx
        {
            get => new TVector3(Z, Y, X);
            set
            {
                Z = value.X;
                Y = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Zyw
        {
            get => new TVector3(Z, Y, W);
            set
            {
                Z = value.X;
                Y = value.Y;
                W = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Z, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Zwx
        {
            get => new TVector3(Z, W, X);
            set
            {
                Z = value.X;
                W = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the Z, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Zwy
        {
            get => new TVector3(Z, W, Y);
            set
            {
                Z = value.X;
                W = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the W, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Wxy
        {
            get => new TVector3(W, X, Y);
            set
            {
                W = value.X;
                X = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the W, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Wxz
        {
            get => new TVector3(W, X, Z);
            set
            {
                W = value.X;
                X = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the W, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Wyx
        {
            get => new TVector3(W, Y, X);
            set
            {
                W = value.X;
                Y = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the W, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Wyz
        {
            get => new TVector3(W, Y, Z);
            set
            {
                W = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the W, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Wzx
        {
            get => new TVector3(W, Z, X);
            set
            {
                W = value.X;
                Z = value.Y;
                X = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector3 with the W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector3 Wzy
        {
            get => new TVector3(W, Z, Y);
            set
            {
                W = value.X;
                Z = value.Y;
                Y = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the X, Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Xywz
        {
            get => new TVector4(X, Y, W, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                W = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the X, Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Xzyw
        {
            get => new TVector4(X, Z, Y, W);
            set
            {
                X = value.X;
                Z = value.Y;
                Y = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the X, Z, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Xzwy
        {
            get => new TVector4(X, Z, W, Y);
            set
            {
                X = value.X;
                Z = value.Y;
                W = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the X, W, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Xwyz
        {
            get => new TVector4(X, W, Y, Z);
            set
            {
                X = value.X;
                W = value.Y;
                Y = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the X, W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Xwzy
        {
            get => new TVector4(X, W, Z, Y);
            set
            {
                X = value.X;
                W = value.Y;
                Z = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Y, X, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Yxzw
        {
            get => new TVector4(Y, X, Z, W);
            set
            {
                Y = value.X;
                X = value.Y;
                Z = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Y, X, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Yxwz
        {
            get => new TVector4(Y, X, W, Z);
            set
            {
                Y = value.X;
                X = value.Y;
                W = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Y, Y, Z, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Yyzw
        {
            get => new TVector4(Y, Y, Z, W);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Y, Y, W, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Yywz
        {
            get => new TVector4(Y, Y, W, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                W = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Y, Z, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Yzxw
        {
            get => new TVector4(Y, Z, X, W);
            set
            {
                Y = value.X;
                Z = value.Y;
                X = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Y, Z, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Yzwx
        {
            get => new TVector4(Y, Z, W, X);
            set
            {
                Y = value.X;
                Z = value.Y;
                W = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Y, W, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Ywxz
        {
            get => new TVector4(Y, W, X, Z);
            set
            {
                Y = value.X;
                W = value.Y;
                X = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Y, W, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Ywzx
        {
            get => new TVector4(Y, W, Z, X);
            set
            {
                Y = value.X;
                W = value.Y;
                Z = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Z, X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Zxyw
        {
            get => new TVector4(Z, X, Y, W);
            set
            {
                Z = value.X;
                X = value.Y;
                Y = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Z, X, W, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Zxwy
        {
            get => new TVector4(Z, X, W, Y);
            set
            {
                Z = value.X;
                X = value.Y;
                W = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Z, Y, X, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Zyxw
        {
            get => new TVector4(Z, Y, X, W);
            set
            {
                Z = value.X;
                Y = value.Y;
                X = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Z, Y, W, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Zywx
        {
            get => new TVector4(Z, Y, W, X);
            set
            {
                Z = value.X;
                Y = value.Y;
                W = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Z, W, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Zwxy
        {
            get => new TVector4(Z, W, X, Y);
            set
            {
                Z = value.X;
                W = value.Y;
                X = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Z, W, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Zwyx
        {
            get => new TVector4(Z, W, Y, X);
            set
            {
                Z = value.X;
                W = value.Y;
                Y = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the Z, W, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Zwzy
        {
            get => new TVector4(Z, W, Z, Y);
            set
            {
                X = value.X;
                W = value.Y;
                Z = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the W, X, Y, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Wxyz
        {
            get => new TVector4(W, X, Y, Z);
            set
            {
                W = value.X;
                X = value.Y;
                Y = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the W, X, Z, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Wxzy
        {
            get => new TVector4(W, X, Z, Y);
            set
            {
                W = value.X;
                X = value.Y;
                Z = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the W, Y, X, and Z components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Wyxz
        {
            get => new TVector4(W, Y, X, Z);
            set
            {
                W = value.X;
                Y = value.Y;
                X = value.Z;
                Z = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the W, Y, Z, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Wyzx
        {
            get => new TVector4(W, Y, Z, X);
            set
            {
                W = value.X;
                Y = value.Y;
                Z = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the W, Z, X, and Y components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Wzxy
        {
            get => new TVector4(W, Z, X, Y);
            set
            {
                W = value.X;
                Z = value.Y;
                X = value.Z;
                Y = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the W, Z, Y, and X components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Wzyx
        {
            get => new TVector4(W, Z, Y, X);
            set
            {
                W = value.X;
                Z = value.Y;
                Y = value.Z;
                X = value.W;
            }
        }

        /// <summary>
        /// Gets or sets an OpenTK.TVector4 with the W, Z, Y, and W components of this instance.
        /// </summary>
        [XmlIgnore]
        public TVector4 Wzyw
        {
            get => new TVector4(W, Z, Y, W);
            set
            {
                X = value.X;
                Z = value.Y;
                Y = value.Z;
                W = value.W;
            }
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static TVector4 operator +(TVector4 left, TVector4 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }

        /// <summary>
        /// Subtracts two instances.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static TVector4 operator -(TVector4 left, TVector4 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        /// <summary>
        /// Negates an instance.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static TVector4 operator -(TVector4 vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            vec.W = -vec.W;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static TVector4 operator *(TVector4 vec, float scale)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="scale">The scalar.</param>
        /// <param name="vec">The instance.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static TVector4 operator *(float scale, TVector4 vec)
        {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        /// <summary>
        /// Component-wise multiplication between the specified instance by a scale vector.
        /// </summary>
        /// <param name="scale">Left operand.</param>
        /// <param name="vec">Right operand.</param>
        /// <returns>Result of multiplication.</returns>
        [Pure]
        public static TVector4 operator *(TVector4 vec, TVector4 scale)
        {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            vec.Z *= scale.Z;
            vec.W *= scale.W;
            return vec;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix.
        /// </summary>
        /// <param name="vec">The vector to transform.</param>
        /// <param name="mat">The desired transformation.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static TVector4 operator *(TVector4 vec, TMatrix4 mat)
        {
            TransformRow(in vec, in mat, out TVector4 result);
            return result;
        }

        /// <summary>
        /// Transform a Vector by the given Matrix using right-handed notation.
        /// </summary>
        /// <param name="mat">The desired transformation.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static TVector4 operator *(TMatrix4 mat, TVector4 vec)
        {
            TransformColumn(in mat, in vec, out TVector4 result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by a quaternion rotation.
        /// </summary>
        /// <param name="quat">The quaternion to rotate the vector by.</param>
        /// <param name="vec">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        [Pure]
        public static TVector4 operator *(TQuaternion quat, TVector4 vec)
        {
            Transform(in vec, in quat, out TVector4 result);
            return result;
        }

        /// <summary>
        /// Divides an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        [Pure]
        public static TVector4 operator /(TVector4 vec, float scale)
        {
            vec.X /= scale;
            vec.Y /= scale;
            vec.Z /= scale;
            vec.W /= scale;
            return vec;
        }

        /// <summary>
        /// Compares two instances for equality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left equals right; false otherwise.</returns>
        public static bool operator ==(TVector4 left, TVector4 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two instances for inequality.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns>True, if left does not equa lright; false otherwise.</returns>
        public static bool operator !=(TVector4 left, TVector4 right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a pointer to the first element of the specified instance.
        /// </summary>
        /// <param name="v">The instance.</param>
        /// <returns>A pointer to the first element of v.</returns>
        [Pure]
        public static unsafe explicit operator float*(TVector4 v)
        {
            return &v.X;
        }

        /// <summary>
        /// Returns a pointer to the first element of the specified instance.
        /// </summary>
        /// <param name="v">The instance.</param>
        /// <returns>A pointer to the first element of v.</returns>
        [Pure]
        public static explicit operator IntPtr(TVector4 v)
        {
            unsafe
            {
                return (IntPtr)(&v.X);
            }
        }

        /// <summary>
        /// Returns this TVector4 as a TColor4. The resulting struct will have RGBA mapped to XYZW, in that order.
        /// </summary>
        /// <param name="v">The TVector4 to convert.</param>
        /// <returns>The TVector4, converted to a TColor4.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static explicit operator TColor4(TVector4 v)
        {
            return new TColor4(v.X, v.Y, v.Z, v.W);//Unsafe.As<TVector4, TColor4>(ref v);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TVector4"/> struct using a tuple containing the component
        /// values.
        /// </summary>
        /// <param name="values">A tuple containing the component values.</param>
        /// <returns>A new instance of the <see cref="TVector4"/> struct with the given component values.</returns>
        [Pure]
        public static implicit operator TVector4((float X, float Y, float Z, float W) values)
        {
            return new TVector4(values.X, values.Y, values.Z, values.W);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("({0}{4} {1}{4} {2}{4} {3})", X, Y, Z, W, MathHelper.ListSeparator);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is TVector4 && Equals((TVector4)obj);
        }

        /// <inheritdoc />
        public bool Equals(TVector4 other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z &&
                   W == other.W;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = 707706286;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            return hashCode;
            //return HashCode.Combine(X, Y, Z, W);
        }

        /// <summary>
        /// Deconstructs the vector into it's individual components.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        [Pure]
        public void Deconstruct(out float x, out float y, out float z, out float w)
        {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }
    }
}
