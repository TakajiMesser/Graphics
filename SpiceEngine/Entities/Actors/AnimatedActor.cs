using OpenTK;
using SpiceEngineCore.Rendering.Animations;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Utilities;
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

        public void SetJointTransforms(int meshIndex, Matrix4[] transforms) => _jointTransformsByMeshIndex[meshIndex] = transforms;

        public override void SetUniforms(ShaderProgram program)
        {
            base.SetUniforms(program);

            if (_jointTransformsByMeshIndex.ContainsKey(_meshIndex))
            {
                program.SetUniform("jointTransforms", _jointTransformsByMeshIndex[_meshIndex]);
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
