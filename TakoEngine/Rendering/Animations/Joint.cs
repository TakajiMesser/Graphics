using TakoEngine.Utilities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Rendering.Animations
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

                        var jointIndex = transform.JointIndices.FirstOrDefault(i => i.MeshIndex == kvp.Key);
                        if (jointIndex != null)
                        {
                            meshTransform.TransformsByBoneIndex.Add(jointIndex.BoneIndex, kvp.Value);
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

        public static Joint CreateJoint(Assimp.Node node, List<Assimp.Mesh> meshes)
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
                var childJoint = CreateJoint(child, meshes);

                if (childJoint != null)
                {
                    joint.Children.Add(childJoint);
                }
            }

            return joint;
        }
    }
}
