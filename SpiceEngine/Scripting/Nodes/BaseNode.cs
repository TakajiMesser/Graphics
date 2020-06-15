using UmamiScriptingCore.Behaviors.Nodes;

namespace SpiceEngine.Scripting.Nodes
{
    public abstract class BaseNode : Node
    {
        /*public override BehaviorStatus Tick(BehaviorContext context)
        {

        }

        private void Initialize()
        {

        }

        public abstract BehaviorStatus DoShit()
        {
            if (context.Entity is IActor actor && ((RigidBody)context.Body).LinearVelocity.IsSignificant())
            {
                float turnAngle = (float)Math.Atan2(((RigidBody)context.Body).LinearVelocity.Y, ((RigidBody)context.Body).LinearVelocity.X);

                actor.Rotation = new Quaternion(0.0f, 0.0f, turnAngle);
                context.EulerRotation = new Vector3(context.EulerRotation.X, context.EulerRotation.Y, turnAngle);
            }

            return BehaviorStatus.Success;
        }

        public abstract BehaviorStatus Tick(BehaviorContext context);*/
    }
}
