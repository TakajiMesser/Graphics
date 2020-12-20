using Assimp;
using CitrusAnimationCore.Bones;
using SweetGraphicsCore.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SpiceEngine.Utilities
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
                    matrix4[j, i] = matrix[i + 1, j + 1];
                }
            }
            
            return matrix4;
        }

        public static Quaternion ToQuaternion(this Assimp.Quaternion quaternion) => new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        public static SpiceEngineCore.Rendering.Materials.Material ToMaterial(this Material material) => new SpiceEngineCore.Rendering.Materials.Material()
        {
            Ambient = new Vector3(material.ColorAmbient.R, material.ColorAmbient.G, material.ColorAmbient.B),
            Diffuse = new Vector3(material.ColorDiffuse.R, material.ColorDiffuse.G, material.ColorDiffuse.B),
            Specular = new Vector3(material.ColorSpecular.R, material.ColorSpecular.G, material.ColorSpecular.B),
            SpecularExponent = 1.0f - material.Shininess
        };

        public static TexturePaths ToTexturePaths(this Material material, string directoryPath) => new TexturePaths()
        {
            DiffuseMapFilePath = !string.IsNullOrEmpty(material.TextureDiffuse.FilePath) ? Path.Combine(directoryPath, material.TextureDiffuse.FilePath) : null,
            NormalMapFilePath = !string.IsNullOrEmpty(material.TextureNormal.FilePath) ? Path.Combine(directoryPath, material.TextureNormal.FilePath) : null,
            SpecularMapFilePath = !string.IsNullOrEmpty(material.TextureSpecular.FilePath) ? Path.Combine(directoryPath, material.TextureSpecular.FilePath) : null,
        };

        public static CitrusAnimationCore.Bones.Bone ToBone(this Node node)
        {
            var bone = new CitrusAnimationCore.Bones.Bone
            {
                Name = node.Name
            };

            foreach (var childNode in node.Children)
            {
                bone.Children.Add(childNode.ToBone());
            }

            return bone;
        }

        public static Joint ToJoint(this Node node, List<Mesh> meshes)
        {
            var joint = new Joint(node.Name)
            {
                Transform = node.Transform.ToMatrix4()
            };

            for (var i = 0; i < meshes.Count; i++)
            {
                var offsets = meshes[i].Bones.Where(b => b.Name == node.Name).Select(b => b.OffsetMatrix);

                if (offsets.Count() > 1)
                {
                    throw new InvalidOperationException("What the fuck do I do here?");
                }
                else if (offsets.Count() == 1)
                {
                    joint.OffsetsByMeshIndex.Add(i, offsets.First().ToMatrix4());
                }
            }

            foreach (var child in node.Children)
            {
                var childJoint = child.ToJoint(meshes);

                if (childJoint != null)
                {
                    joint.Children.Add(childJoint);
                }
            }

            return joint;
        }
    }
}
