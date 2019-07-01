using OpenTK;
using SpiceEngine.Rendering.Animations;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Entities.Actors
{
    public class AnimatedActor : Actor
    {
        public const int MAX_JOINTS = 100;

        private Dictionary<int, Matrix4[]> _jointTransformsByMeshIndex = new Dictionary<int, Matrix4[]>();//ArrayExtensions.Initialize(MAX_JOINTS, Matrix4.Identity);

        public Animator Animator { get; set; } = new Animator();
        public Joint RootJoint { get; set; }

        internal Matrix4 _globalInverseTransform;

        public AnimatedActor(string name) : base(name) { }

        public void SetJointTransforms(int meshIndex, Matrix4[] transforms) => _jointTransformsByMeshIndex[meshIndex] = transforms;

        public override Actor Duplicate(string name)
        {
            var animatedActor = new AnimatedActor(name);
            animatedActor.FromActor(this);

            foreach (var kvp in _jointTransformsByMeshIndex)
            {
                animatedActor._jointTransformsByMeshIndex.Add(kvp.Key, kvp.Value);
            }

            return animatedActor;
        }

        public override void SetUniforms(ShaderProgram program, ITextureProvider textureProvider, int meshIndex)
        {
            base.SetUniforms(program, textureProvider, meshIndex);

            if (_jointTransformsByMeshIndex.ContainsKey(meshIndex))
            {
                program.SetUniform("jointTransforms", _jointTransformsByMeshIndex[meshIndex]);
            }
            else
            {
                program.SetUniform("jointTransforms", ArrayExtensions.Initialize(MAX_JOINTS, Matrix4.Identity));
            }
        }

        public void UpdateAnimation()
        {
            Animator.CurrentAnimation = Animator.Animations.First();
            Animator.Tick();
        }

        // Define how this object's state will be saved, if desired
        public override void OnSaveState() { }
    }
}
