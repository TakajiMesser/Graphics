﻿using System;
using System.Collections.Generic;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngineCore.Utilities
{
    public static class VectorExtensions
    {
        public static float AngleBetween(this Vector2 vectorA, Vector2 vectorB)
        {
            var cosAngle = Vector2.Dot(vectorA, vectorB) / (vectorA.Length * vectorB.Length);
            return (float)Math.Acos(cosAngle);
        }

        public static float AngleBetween(this Vector3 vectorA, Vector3 vectorB)
        {
            var cosAngle = Vector3.Dot(vectorA, vectorB) / (vectorA.Length * vectorB.Length);
            return (float)Math.Acos(cosAngle);
        }

        public static bool IsSignificant(this Vector2 vector) => vector.X >= MathExtensions.EPSILON || vector.X <= -MathExtensions.EPSILON
            || vector.Y >= MathExtensions.EPSILON || vector.Y <= -MathExtensions.EPSILON;

        public static bool IsSignificant(this Vector3 vector) => vector.X >= MathExtensions.EPSILON || vector.X <= -MathExtensions.EPSILON
            || vector.Y >= MathExtensions.EPSILON || vector.Y <= -MathExtensions.EPSILON
            || vector.Z >= MathExtensions.EPSILON || vector.Z <= -MathExtensions.EPSILON;

        public static Color4 ToColor4(this Vector4 vector) => new Color4(vector.X, vector.Y, vector.Z, vector.W);

        public static Vector3 ToRadians(this Vector3 vector) => new Vector3(UnitConversions.ToRadians(vector.X), UnitConversions.ToRadians(vector.Y), UnitConversions.ToRadians(vector.Z));

        public static Vector3 ToDegrees(this Vector3 vector) => new Vector3(UnitConversions.ToDegrees(vector.X), UnitConversions.ToDegrees(vector.Y), UnitConversions.ToDegrees(vector.Z));

        public static Vector3 Average(this IEnumerable<Vector3> vertices) => new Vector3(vertices.Average(v => v.X), vertices.Average(v => v.Y), vertices.Average(v => v.Z));
    }
}
