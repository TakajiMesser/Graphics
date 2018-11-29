using OpenTK;
using System.Collections.Generic;
using System.Linq;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Models;
using SpiceEngine.Game;
using SpiceEngine.Inputs;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.StimResponse;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Animations;
using SpiceEngine.Utilities;

namespace SpiceEngine.Entities
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

        public override void SetUniforms(ShaderProgram program, TextureManager textureManager, int meshIndex)
        {
            base.SetUniforms(program, textureManager, meshIndex);

            if (_jointTransformsByMeshIndex.ContainsKey(meshIndex))
            {
                program.SetUniform("jointTransforms", _jointTransformsByMeshIndex[meshIndex]);
            }
        }

        public override void OnUpdateFrame(IEnumerable<Bounds> colliders)
        {
            base.OnUpdateFrame(colliders);

            Animator.CurrentAnimation = Animator.Animations.First();
            Animator.Tick();
        }

        // Define how this object's state will be saved, if desired
        public override void OnSaveState() { }
    }
}
