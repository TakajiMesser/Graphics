using Newtonsoft.Json;
using OpenTK;
using SavoryPhysicsCore;
using SavoryPhysicsCore.Bodies;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

namespace SampleGameProject.Resources.Behaviors.Nodes
{
    public class MoveToNode : Node
    {
        public Vector3 Destination { get; private set; }
        public float Speed { get; private set; }

        [JsonIgnore]
        public float XTolerance { get; set; } = 0.1f;

        [JsonIgnore]
        public float YTolerance { get; set; } = 0.1f;

        [JsonIgnore]
        public float ZTolerance { get; set; } = 0.5f;

        public MoveToNode(Vector3 destination, float speed)
        {
            Destination = destination;
            Speed = speed;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            var difference = Destination - context.GetPosition();

            var reachedDestination = difference.X < XTolerance && difference.X > -XTolerance
                && difference.Y < YTolerance && difference.Y > -YTolerance
                && difference.Z < ZTolerance && difference.Z > -ZTolerance;//!difference.IsSignificant();

            if (reachedDestination)
            {
                return BehaviorStatus.Success;
            }
            else
            {
                var body = context.GetComponent<IBody>() as RigidBody;
                var velocity = difference.Length < Speed
                    ? difference
                    : difference.Normalized() * Speed;

                body.ApplyVelocity(velocity);

                return BehaviorStatus.Running;
            }
        }

        public override void Reset() { }
    }
}
