using Graphics.Meshes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using Graphics.Scripting.BehaviorTrees;

namespace Graphics.GameObjects
{
    public class Enemy : GameObject
    {
        public const float WALK_SPEED = 0.01f;
        public const float RUN_SPEED = 0.015f;
        public const float CREEP_SPEED = 0.005f;

        public BehaviorTree AI;

        public Enemy() : base("Enemy")
        {

        }
    }
}
