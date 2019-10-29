using OpenTK;
using SpiceEngineCore.Physics.Bodies;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.Scripting.Nodes;
using SpiceEngineCore.Utilities;

namespace SampleGameProject.Behaviors
{
    public class MoveToNode : Node
    {
        public Vector3 Destination { get; private set; }
        public float Speed { get; private set; }

        public float XTolerance { get; set; } = 0.1f;
        public float YTolerance { get; set; } = 0.1f;
        public float ZTolerance { get; set; } = 0.5f;

        public MoveToNode(Vector3 destination, float speed)
        {
            Destination = destination;
            Speed = speed;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var difference = Destination - context.Position;

            var reachedDestination = difference.X < XTolerance && difference.X > -XTolerance
                && difference.Y < YTolerance && difference.Y > -YTolerance
                && difference.Z < ZTolerance && difference.Z > -ZTolerance;//!difference.IsSignificant();

            if (reachedDestination)
            {
                return BehaviorStatus.Success;
            }
            else if (difference.Length < Speed)
            {
                ((RigidBody3D)context.Body).ApplyVelocity(difference);
            }
            else
            {
                ((RigidBody3D)context.Body).ApplyVelocity(difference.Normalized() * Speed);
            }

            return BehaviorStatus.Running;
        }

        public override void Reset() { }
    }
}
