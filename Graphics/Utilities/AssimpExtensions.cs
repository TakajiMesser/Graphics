using Assimp;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Utilities
{
    public static class AssimpExtensions
    {
        public static Vector3 ToVector3(this Vector3D vector) => new Vector3(vector.X, vector.Y, vector.Z);

        public static Vector2 ToVector2(this Vector3D vector) => new Vector2(vector.X, vector.Y);

        public static Matrix4 ToMatrix4(this Matrix4x4 matrix)
        {
            var matrix4 = Matrix4.Identity;

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    matrix4[i, j] = matrix[i + 1, j + 1];
                }
            }
            //matrix4.Invert();
            matrix4.Transpose();
            
            return matrix4;
        }

        public static OpenTK.Quaternion ToQuaternion(this Assimp.Quaternion quaternion) => new OpenTK.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}
