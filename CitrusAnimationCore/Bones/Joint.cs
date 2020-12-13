using CitrusAnimationCore.Animations;
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

namespace CitrusAnimationCore.Bones
{
    public class Joint
    {
        public string Name { get; private set; }
        public List<Joint> Children { get; private set; } = new List<Joint>();

        /// <summary>
        /// The matrices that transform the bind pose from mesh-space to bone-space.
        /// </summary>
        public Dictionary<int, Matrix4> OffsetsByMeshIndex { get; private set; } = new Dictionary<int, Matrix4>();

        /// <summary>
        /// The inverse should transform the bind pose from bone-space to mesh-space.
        /// </summary>
        public Dictionary<int, Matrix4> InverseOffsetsByMeshIndex { get; set; } = new Dictionary<int, Matrix4>();

        /// <summary>
        /// Transforms from bone-space to parent-space
        /// </summary>
        public Matrix4 Transform { get; set; }

        /// <summary>
        /// Transforms from parent-space to bone-space
        /// </summary>
        public Matrix4 InverseTransform { get; set; }

        /// <summary>
        /// The transform from the bind pose to the current pose, in model-space
        /// </summary>
        public Dictionary<int, Matrix4> AnimatedTransformsByMeshIndex { get; set; } = new Dictionary<int, Matrix4>();

        public void CalculateInverseBindTransforms(int meshIndex, Matrix4 parentBindTransform)
        {
            var bindTransform = parentBindTransform * Transform;
            InverseTransform = bindTransform.Inverted();
            //InverseTransform = Transform.Inverted();

            if (OffsetsByMeshIndex.ContainsKey(meshIndex))
            {
                InverseOffsetsByMeshIndex.Add(meshIndex, OffsetsByMeshIndex[meshIndex].Inverted());
            }

            foreach (var child in Children)
            {
                child.CalculateInverseBindTransforms(meshIndex, bindTransform);
            }
        }

        public Joint(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Do some cool shit, yo.
        /// </summary>
        /// <param name="meshIndex"></param>
        /// <param name="keyFrame"></param>
        /// <param name="parentTransform"></param>
        /// <param name="globalInverseTransform"></param>
        public void ApplyKeyFrameTransforms(int meshIndex, KeyFrame keyFrame, Matrix4 parentTransform, Matrix4 globalInverseTransform)
        {
            var transform = keyFrame.Transforms.FirstOrDefault(t => t.Name == Name);
            var boneTransform = (transform != null) ? transform.Transform : Matrix4.Identity;

            // Convert transform from bone-space to model-space
            var modelTransform = boneTransform * parentTransform;

            if (OffsetsByMeshIndex.ContainsKey(meshIndex))
            {
                AnimatedTransformsByMeshIndex[meshIndex] = OffsetsByMeshIndex[meshIndex] * modelTransform * globalInverseTransform;
            }

            foreach (var child in Children)
            {
                child.ApplyKeyFrameTransforms(meshIndex, keyFrame, modelTransform, globalInverseTransform);
            }
        }

        public List<MeshTransforms> GetMeshTransforms(KeyFrame keyFrame)
        {
            var meshTransforms = new List<MeshTransforms>();

            foreach (var transform in keyFrame.Transforms)
            {
                var joint = GetJoint(transform.Name);
                if (joint != null)
                {
                    foreach (var kvp in joint.AnimatedTransformsByMeshIndex)
                    {
                        if (!meshTransforms.Any(m => m.MeshIndex == kvp.Key))
                        {
                            meshTransforms.Add(new MeshTransforms(kvp.Key));
                        }

                        var meshTransform = meshTransforms.First(m => m.MeshIndex == kvp.Key);

                        var jointIndex = transform.GetJointIndex(kvp.Key);
                        if (jointIndex.HasValue)
                        {
                            meshTransform.TransformsByBoneIndex.Add(jointIndex.Value.BoneIndex, kvp.Value);
                        }
                    }
                }
            }

            return meshTransforms;
        }

        public Joint GetJoint(string name)
        {
            if (Name == name)
            {
                return this;
            }
            else
            {
                foreach (var child in Children)
                {
                    var joint = child.GetJoint(name);
                    if (joint != null)
                    {
                        return joint;
                    }
                }
            }

            return null;
        }
    }
}
