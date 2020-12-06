using SpiceEngineCore.Geometry.Quaternions;
using SpiceEngineCore.Geometry.Vectors;
using System;
using System.Runtime.InteropServices;

namespace SpiceEngineCore.Geometry.Matrices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4 : IEquatable<Matrix4>
    {
        public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
        {
            M00 = m00;
            M01 = m01;
            M02 = m02;
            M03 = m03;
            M10 = m10;
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M20 = m20;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M30 = m30;
            M31 = m31;
            M32 = m32;
            M33 = m33;
        }

        public float M00 { get; private set; }
        public float M01 { get; private set; }
        public float M02 { get; private set; }
        public float M03 { get; private set; }
        public float M10 { get; private set; }
        public float M11 { get; private set; }
        public float M12 { get; private set; }
        public float M13 { get; private set; }
        public float M20 { get; private set; }
        public float M21 { get; private set; }
        public float M22 { get; private set; }
        public float M23 { get; private set; }
        public float M30 { get; private set; }
        public float M31 { get; private set; }
        public float M32 { get; private set; }
        public float M33 { get; private set; }

        // TODO - Consider actually storing these matrices as arrays, and having properties mask index access
        public float[] Values => new[] { M00, M01, M02, M03, M10, M11, M12, M13, M20, M21, M22, M23, M30, M31, M32, M33 };

        public float Determinant => M00 * M11 * M22 * M33 - M00 * M11 * M23 * M32 + M00 * M12 * M23 * M31 - M00 * M12 * M21 * M33
            + M00 * M13 * M21 * M32 - M00 * M13 * M22 * M31 - M01 * M12 * M23 * M30 + M01 * M12 * M20 * M33
            - M01 * M13 * M20 * M32 + M01 * M13 * M22 * M30 - M01 * M10 * M22 * M33 + M01 * M10 * M23 * M32
            + M02 * M13 * M20 * M31 - M02 * M13 * M21 * M30 + M02 * M10 * M21 * M33 - M02 * M10 * M23 * M31
            + M02 * M11 * M23 * M30 - M02 * M11 * M20 * M33 - M03 * M10 * M21 * M32 + M03 * M10 * M22 * M31
            - M03 * M11 * M22 * M30 + M03 * M11 * M20 * M32 - M03 * M12 * M20 * M31 + M03 * M12 * M21 * M30;

        public float Trace => M00 + M11 + M22 + M33;

        public Matrix4 Transposed() => new Matrix4(M00, M10, M20, M30, M01, M11, M21, M31, M02, M12, M22, M32, M03, M13, M23, M33);

        /*
         M00    M01     M02     M03
         M10    M11     M12     M13
         M20    M21     M22     M23
         M30    M31     M32     M33
         */

        public Matrix4 Inverted()
        {
            inverseM00 = m[5] * m[10] * m[15] -
             m[5] * m[11] * m[14] -
             m[9] * m[6] * m[15] +
             m[9] * m[7] * m[14] +
             m[13] * m[6] * m[11] -
             m[13] * m[7] * m[10];

            inv[4] = -m[4] * m[10] * m[15] +
                      m[4] * m[11] * m[14] +
                      m[8] * m[6] * m[15] -
                      m[8] * m[7] * m[14] -
                      m[12] * m[6] * m[11] +
                      m[12] * m[7] * m[10];

            inv[8] = m[4] * m[9] * m[15] -
                     m[4] * m[11] * m[13] -
                     m[8] * m[5] * m[15] +
                     m[8] * m[7] * m[13] +
                     m[12] * m[5] * m[11] -
                     m[12] * m[7] * m[9];

            inv[12] = -m[4] * m[9] * m[14] +
                       m[4] * m[10] * m[13] +
                       m[8] * m[5] * m[14] -
                       m[8] * m[6] * m[13] -
                       m[12] * m[5] * m[10] +
                       m[12] * m[6] * m[9];

            inv[1] = -m[1] * m[10] * m[15] +
                      m[1] * m[11] * m[14] +
                      m[9] * m[2] * m[15] -
                      m[9] * m[3] * m[14] -
                      m[13] * m[2] * m[11] +
                      m[13] * m[3] * m[10];

            inv[5] = m[0] * m[10] * m[15] -
                     m[0] * m[11] * m[14] -
                     m[8] * m[2] * m[15] +
                     m[8] * m[3] * m[14] +
                     m[12] * m[2] * m[11] -
                     m[12] * m[3] * m[10];

            inv[9] = -m[0] * m[9] * m[15] +
                      m[0] * m[11] * m[13] +
                      m[8] * m[1] * m[15] -
                      m[8] * m[3] * m[13] -
                      m[12] * m[1] * m[11] +
                      m[12] * m[3] * m[9];

            inv[13] = m[0] * m[9] * m[14] -
                      m[0] * m[10] * m[13] -
                      m[8] * m[1] * m[14] +
                      m[8] * m[2] * m[13] +
                      m[12] * m[1] * m[10] -
                      m[12] * m[2] * m[9];

            inv[2] = m[1] * m[6] * m[15] -
                     m[1] * m[7] * m[14] -
                     m[5] * m[2] * m[15] +
                     m[5] * m[3] * m[14] +
                     m[13] * m[2] * m[7] -
                     m[13] * m[3] * m[6];

            inv[6] = -m[0] * m[6] * m[15] +
                      m[0] * m[7] * m[14] +
                      m[4] * m[2] * m[15] -
                      m[4] * m[3] * m[14] -
                      m[12] * m[2] * m[7] +
                      m[12] * m[3] * m[6];

            inv[10] = m[0] * m[5] * m[15] -
                      m[0] * m[7] * m[13] -
                      m[4] * m[1] * m[15] +
                      m[4] * m[3] * m[13] +
                      m[12] * m[1] * m[7] -
                      m[12] * m[3] * m[5];

            inv[14] = -m[0] * m[5] * m[14] +
                       m[0] * m[6] * m[13] +
                       m[4] * m[1] * m[14] -
                       m[4] * m[2] * m[13] -
                       m[12] * m[1] * m[6] +
                       m[12] * m[2] * m[5];

            inv[3] = -m[1] * m[6] * m[11] +
                      m[1] * m[7] * m[10] +
                      m[5] * m[2] * m[11] -
                      m[5] * m[3] * m[10] -
                      m[9] * m[2] * m[7] +
                      m[9] * m[3] * m[6];

            inv[7] = m[0] * m[6] * m[11] -
                     m[0] * m[7] * m[10] -
                     m[4] * m[2] * m[11] +
                     m[4] * m[3] * m[10] +
                     m[8] * m[2] * m[7] -
                     m[8] * m[3] * m[6];

            inv[11] = -m[0] * m[5] * m[11] +
                       m[0] * m[7] * m[9] +
                       m[4] * m[1] * m[11] -
                       m[4] * m[3] * m[9] -
                       m[8] * m[1] * m[7] +
                       m[8] * m[3] * m[5];

            inv[15] = m[0] * m[5] * m[10] -
                      m[0] * m[6] * m[9] -
                      m[4] * m[1] * m[10] +
                      m[4] * m[2] * m[9] +
                      m[8] * m[1] * m[6] -
                      m[8] * m[2] * m[5];

            det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];

            if (det == 0)
                return false;

            det = 1.0 / det;

            for (i = 0; i < 16; i++)
                invOut[i] = inv[i] * det;

            return true;






            if (Determinant == 0)
            {
                return this;
            }
            else
            {
                int[] columnIndices = { 0, 0, 0, 0 };
                int[] rowIndices = { 0, 0, 0, 0 };
                int[] pivotIndices = { -1, -1, -1, -1 };


            }

            // convert the matrix to an array for easy looping
            float[,] inverse =
            {
                { mat.Row0.X, mat.Row0.Y, mat.Row0.Z, mat.Row0.W },
                { mat.Row1.X, mat.Row1.Y, mat.Row1.Z, mat.Row1.W },
                { mat.Row2.X, mat.Row2.Y, mat.Row2.Z, mat.Row2.W },
                { mat.Row3.X, mat.Row3.Y, mat.Row3.Z, mat.Row3.W }
            };
            var icol = 0;
            var irow = 0;
            for (var i = 0; i < 4; i++)
            {
                // Find the largest pivot value
                var maxPivot = 0.0f;
                for (var j = 0; j < 4; j++)
                {
                    if (pivotIdx[j] != 0)
                    {
                        for (var k = 0; k < 4; ++k)
                        {
                            if (pivotIdx[k] == -1)
                            {
                                var absVal = Math.Abs(inverse[j, k]);
                                if (absVal > maxPivot)
                                {
                                    maxPivot = absVal;
                                    irow = j;
                                    icol = k;
                                }
                            }
                            else if (pivotIdx[k] > 0)
                            {
                                result = mat;
                                return;
                            }
                        }
                    }
                }

                ++pivotIdx[icol];

                // Swap rows over so pivot is on diagonal
                if (irow != icol)
                {
                    for (var k = 0; k < 4; ++k)
                    {
                        var f = inverse[irow, k];
                        inverse[irow, k] = inverse[icol, k];
                        inverse[icol, k] = f;
                    }
                }

                rowIdx[i] = irow;
                colIdx[i] = icol;

                var pivot = inverse[icol, icol];

                // check for singular matrix
                if (pivot == 0.0f)
                {
                    throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
                }

                // Scale row so it has a unit diagonal
                var oneOverPivot = 1.0f / pivot;
                inverse[icol, icol] = 1.0f;
                for (var k = 0; k < 4; ++k)
                {
                    inverse[icol, k] *= oneOverPivot;
                }

                // Do elimination of non-diagonal elements
                for (var j = 0; j < 4; ++j)
                {
                    // check this isn't on the diagonal
                    if (icol != j)
                    {
                        var f = inverse[j, icol];
                        inverse[j, icol] = 0.0f;
                        for (var k = 0; k < 4; ++k)
                        {
                            inverse[j, k] -= inverse[icol, k] * f;
                        }
                    }
                }
            }

            for (var j = 3; j >= 0; --j)
            {
                var ir = rowIdx[j];
                var ic = colIdx[j];
                for (var k = 0; k < 4; ++k)
                {
                    var f = inverse[k, ir];
                    inverse[k, ir] = inverse[k, ic];
                    inverse[k, ic] = f;
                }
            }

            result.Row0.X = inverse[0, 0];
            result.Row0.Y = inverse[0, 1];
            result.Row0.Z = inverse[0, 2];
            result.Row0.W = inverse[0, 3];
            result.Row1.X = inverse[1, 0];
            result.Row1.Y = inverse[1, 1];
            result.Row1.Z = inverse[1, 2];
            result.Row1.W = inverse[1, 3];
            result.Row2.X = inverse[2, 0];
            result.Row2.Y = inverse[2, 1];
            result.Row2.Z = inverse[2, 2];
            result.Row2.W = inverse[2, 3];
            result.Row3.X = inverse[3, 0];
            result.Row3.Y = inverse[3, 1];
            result.Row3.Z = inverse[3, 2];
            result.Row3.W = inverse[3, 3];
        }

        public override string ToString() => "|" + M00 + "," + M01 + "," + M02 + "," + M03 + "|"
            + Environment.NewLine + "|" + M10 + "," + M11 + "," + M12 + "," + M13 + "|"
            + Environment.NewLine + "|" + M20 + "," + M21 + "," + M22 + "," + M23 + "|"
            + Environment.NewLine + "|" + M30 + "," + M31 + "," + M32 + "," + M33 + "|"; 

        public static Matrix4 operator +(Matrix4 left, Matrix4 right) => new Matrix4(left.M00 + right.M00, left.M01 + right.M01, left.M02 + right.M02, left.M03 + right.M03, left.M10 + right.M10, left.M11 + right.M11, left.M12 + right.M12, left.M13 + right.M13, left.M20 + right.M20, left.M21 + right.M21, left.M22 + right.M22, left.M23 + right.M23, left.M30 + right.M30, left.M31 + right.M31, left.M32 + right.M32, left.M33 + right.M33);

        public static Matrix4 operator -(Matrix4 left, Matrix4 right) => new Matrix4(left.M00 - right.M00, left.M01 - right.M01, left.M02 - right.M02, left.M03 - right.M03, left.M10 - right.M10, left.M11 - right.M11, left.M12 - right.M12, left.M13 - right.M13, left.M20 - right.M20, left.M21 - right.M21, left.M22 - right.M22, left.M23 - right.M23, left.M30 - right.M30, left.M31 - right.M31, left.M32 - right.M32, left.M33 - right.M33);

        public static Matrix4 operator *(Matrix4 left, Matrix4 right) => new Matrix4(left.M00 * right.M00 + left.M01 * right.M10 + left.M02 * right.M20 + left.M03 * right.M30, left.M00 * right.M01 + left.M01 * right.M11 + left.M02 * right.M21 + left.M03 * right.M31, left.M00 * right.M02 + left.M01 * right.M12 + left.M02 * right.M22 + left.M03 * right.M32, left.M00 * right.M03 + left.M01 * right.M13 + left.M02 * right.M23 + left.M03 * right.M33, left.M10 * right.M00 + left.M11 * right.M10 + left.M12 * right.M20 + left.M13 * right.M30, left.M10 * right.M01 + left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31, left.M10 * right.M02 + left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32, left.M10 * right.M03 + left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33, left.M20 * right.M00 + left.M21 * right.M10 + left.M22 * right.M20 + left.M23 * right.M30, left.M20 * right.M01 + left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31, left.M20 * right.M02 + left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32, left.M20 * right.M03 + left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33, left.M30 * right.M00 + left.M31 * right.M10 + left.M32 * right.M20 + left.M33 * right.M30, left.M30 * right.M01 + left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31, left.M30 * right.M02 + left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32, left.M30 * right.M03 + left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33);

        public static bool operator ==(Matrix4 left, Matrix4 right) => left.Equals(right);

        public static bool operator !=(Matrix4 left, Matrix4 right) => !(left == right);

        public static Matrix4 FromTranslation(Vector3 vector) => new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, vector.X, vector.Y, vector.Z, 0, 0, 0, 0, 1);

        public static Matrix4 FromScale(Vector3 scale) => new Matrix4(scale.X, 0, 0, 0, 0, scale.Y, 0, 0, 0, 0, scale.Z, 0, 0, 0, 0, 1);

        public static Matrix4 FromQuaternion(Quaternion quaternion)
        {
            var axisAngle = quaternion.ToAxisAngle();

            var axis = axisAngle.Xyz.Normalized();
            var cos = (float)Math.Cos(-axisAngle.W);
            var sin = (float)Math.Sin(-axisAngle.W);
            var t = 1f - cos;

            float tXX = t * axis.X * axis.X;
            float tXY = t * axis.X * axis.Y;
            float tXZ = t * axis.X * axis.Z;
            float tYY = t * axis.Y * axis.Y;
            float tYZ = t * axis.Y * axis.Z;
            float tZZ = t * axis.Z * axis.Z;

            float sinX = sin * axis.X;
            float sinY = sin * axis.Y;
            float sinZ = sin * axis.Z;

            return new Matrix4(
                tXX + cos, tXY - sinZ, tXZ + sinY, 0,
                tXY + sinZ, tYY + cos, tYZ - sinX, 0,
                tXZ - sinY, tYZ + sinX, tZZ + cos, 0,
                0, 0, 0, 1);
        }

        public static Matrix4 Orthographic(float width, float height, float depthNear, float depthFar)
        {
            var left = -width / 2;
            var right = width / 2;
            var bottom = -height / 2;
            var top = height / 2;

            var invRL = 1.0f / (right - left);
            var invTB = 1.0f / (top - bottom);
            var invFN = 1.0f / (depthFar - depthNear);

            return new Matrix4(
                2 * invRL, 0, 0, 0,
                0, 2 * invTB, 0, 0,
                0, 0, -2 * invFN, 0,
                -(right + left) * invRL, -(top + bottom) * invTB, -(depthFar + depthNear) * invFN, 1
            );
        }

        public static Matrix4 Perspective(float fovy, float aspect, float depthNear, float depthFar)
        {
            if (fovy <= 0 || fovy > Math.PI) throw new ArgumentOutOfRangeException(nameof(fovy));
            if (aspect <= 0) throw new ArgumentOutOfRangeException(nameof(aspect));
            if (depthNear <= 0 || depthNear >= depthFar) throw new ArgumentOutOfRangeException(nameof(depthNear));
            if (depthFar <= 0) throw new ArgumentOutOfRangeException(nameof(depthFar));

            var top = depthNear * (float)Math.Tan(0.5f * fovy);
            var bottom = -top;
            var left = bottom * aspect;
            var right = top * aspect;

            var x = 2.0f * depthNear / (right - left);
            var y = 2.0f * depthNear / (top - bottom);
            var a = (right + left) / (right - left);
            var b = (top + bottom) / (top - bottom);
            var c = -(depthFar + depthNear) / (depthFar - depthNear);
            var d = -(2.0f * depthFar * depthNear) / (depthFar - depthNear);

            return new Matrix4(
                x, 0, 0, 0,
                0, y, 0, 0,
                a, b, c, -1,
                0, 0, d, 0);
        }

        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            var z = (eye - target).Normalized();
            var x = Vector3.Cross(up, z).Normalized();
            var y = Vector3.Cross(z, x).Normalized();

            return new Matrix4(
                x.X, y.X, z.X, 0,
                x.Y, y.Y, z.Y, 0,
                x.Z, y.Z, z.Z, 0,
                -((x.X * eye.X) + (x.Y * eye.Y) + (x.Z * eye.Z)), -((y.X * eye.X) + (y.Y * eye.Y) + (y.Z * eye.Z)), -((z.X * eye.X) + (z.Y * eye.Y) + (z.Z * eye.Z)), 1);
        }

        public static Matrix4 Identity => new Matrix4(
            1.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f);

        public override bool Equals(object obj) => obj is Matrix4 matrix && Equals(matrix);

        public bool Equals(Matrix4 other) => M00 == other.M00
            && M01 == other.M01
            && M02 == other.M02
            && M03 == other.M03
            && M10 == other.M10
            && M11 == other.M11
            && M12 == other.M12
            && M13 == other.M13
            && M20 == other.M20
            && M21 == other.M21
            && M22 == other.M22
            && M23 == other.M23
            && M30 == other.M30
            && M31 == other.M31
            && M32 == other.M32
            && M33 == other.M33;

        public override int GetHashCode()
        {
            var hashCode = 1197848312;
            hashCode = hashCode * -1521134295 + M00.GetHashCode();
            hashCode = hashCode * -1521134295 + M01.GetHashCode();
            hashCode = hashCode * -1521134295 + M02.GetHashCode();
            hashCode = hashCode * -1521134295 + M03.GetHashCode();
            hashCode = hashCode * -1521134295 + M10.GetHashCode();
            hashCode = hashCode * -1521134295 + M11.GetHashCode();
            hashCode = hashCode * -1521134295 + M12.GetHashCode();
            hashCode = hashCode * -1521134295 + M13.GetHashCode();
            hashCode = hashCode * -1521134295 + M20.GetHashCode();
            hashCode = hashCode * -1521134295 + M21.GetHashCode();
            hashCode = hashCode * -1521134295 + M22.GetHashCode();
            hashCode = hashCode * -1521134295 + M23.GetHashCode();
            hashCode = hashCode * -1521134295 + M30.GetHashCode();
            hashCode = hashCode * -1521134295 + M31.GetHashCode();
            hashCode = hashCode * -1521134295 + M32.GetHashCode();
            hashCode = hashCode * -1521134295 + M33.GetHashCode();
            return hashCode;
        }
    }
}
