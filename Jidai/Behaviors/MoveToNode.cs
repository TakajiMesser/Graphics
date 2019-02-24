using OpenTK;
using SpiceEngine.Physics.Bodies;
using SpiceEngine.Scripting.Behaviors;

namespace Jidai.Behaviors
{
    public class MoveToNode : Node
    {
        public Vector3 Destination { get; private set; }
        public float Speed { get; private set; }

        public MoveToNode(Vector3 destination, float speed)
        {
            Destination = destination;
            Speed = speed;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var difference = Destination - context.Position;

            if (difference == Vector3.Zero)
            {
                return BehaviorStatus.Success;
            }
            else if (difference.Length < Speed)
            {
                ((RigidBody3D)context.Body).ApplyImpulse(difference);
            }
            else
            {
                ((RigidBody3D)context.Body).ApplyImpulse(difference.Normalized() * Speed);
            }

            return BehaviorStatus.Running;
        }

        public override void Reset() { }
    }
}
