using Newtonsoft.Json;
using SavoryPhysicsCore;
using SavoryPhysicsCore.Bodies;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace TowerWarfare.Resources.Behaviors.Nodes
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
            else if (difference.Length < Speed)
            {
                var body = context.GetComponent<IBody>() as RigidBody;
                body.ApplyVelocity(difference);
            }
            else
            {
                var body = context.GetComponent<IBody>() as RigidBody;
                body.ApplyVelocity(difference.Normalized() * Speed);
            }

            return BehaviorStatus.Running;
        }

        public override void Reset() { }
    }
}
