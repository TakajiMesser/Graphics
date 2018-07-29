using Graphics.Meshes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using Graphics.Scripting.BehaviorTrees;
using Graphics.Physics.Collision;

namespace Graphics.GameObjects
{
    public class Enemy : GameObject
    {
        public const float WALK_SPEED = 0.1f;
        public const float RUN_SPEED = 0.15f;
        public const float CREEP_SPEED = 0.04f;
        public const float EVADE_SPEED = 0.175f;
        public const int EVADE_TICK_COUNT = 20;

        private BehaviorTree _behavior;

        public Enemy() : base("Enemy")
        {
            _behavior.Context.Add("", false);
        }

        public override void OnUpdateFrame(IEnumerable<Collider> colliders)
        {
            base.OnUpdateFrame(colliders);

            _behavior.Tick();
        }
    }
}
