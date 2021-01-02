using SavoryPhysicsCore;
using SavoryPhysicsCore.Bodies;
using SpiceEngine.Helpers;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Utilities;
using System.Linq;
using TangyHIDCore;
using UmamiScriptingCore;
using UmamiScriptingCore.Behaviors;
using UmamiScriptingCore.Behaviors.Nodes;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SampleGameProject.Behaviors.Player
{
    public class CoverNode : Node
    {
        public float CoverSpeed { get; set; }
        public float EnterCoverSpeed { get; set; }
        public float CoverDistance { get; set; }

        public CoverNode(float coverSpeed, float enterCoverSpeed, float coverDistance)
        {
            CoverSpeed = coverSpeed;
            EnterCoverSpeed = enterCoverSpeed;
            CoverDistance = coverDistance;
        }

        public override BehaviorStatus Tick(BehaviorContext context)
        {
            if (context.ContainsVariable("coverDirection"))
            {
                if (context.ContainsVariable("coverDistance"))
                {
                    if (context.GetVariable<float>("coverDistance") > 0.0f)
                    {
                        context.SetVariable("coverDistance", context.GetVariable<float>("coverDistance") - EnterCoverSpeed);

                        var coverDirection = context.GetVariable<Vector2>("coverDirection");

                        var body = context.GetComponent<IBody>() as RigidBody;
                        body.ApplyVelocity(new Vector3(coverDirection.X, coverDirection.Y, 0) * EnterCoverSpeed);
                    }

                    if (context.GetVariable<float>("coverDistance") < 0.1f)
                    {
                        // Handle movement while in cover here
                        var inputProvider = context.SystemProvider.GetGameSystem<IInputProvider>();
                        var translation = GeometryHelper.GetHeldTranslation(context.SystemProvider.EntityProvider.ActiveCamera, CoverSpeed, inputProvider);

                        if (translation.IsSignificant())
                        {
                            var filteredColliders = context.SystemProvider.GetGameSystem<IPhysicsProvider>().GetCollisionBodies(context.GetEntity().ID).Where(b => context.GetEntity(b.EntityID) is IBrush);

                            // Calculate the furthest point along the bounds of our object, since we should attempt to raycast from there
                            var shape = context.GetComponent<IBody>().Shape;
                            /*var borderPoint = shape.GetFurthestPoint(context.Entity.Position, translation);
                            
                            // TODO - Dynamically determine how far the raycast should be
                            var boundWidth = 2.0f;

                            var coverDirection = context.GetVariable<Vector2>("coverDirection");
                            if (Raycast.TryRaycast(new Ray3(borderPoint, new Vector3(coverDirection.X, coverDirection.Y, 0.0f), boundWidth), filteredColliders, context.GetEntityProvider(), out RaycastHit hit))
                            {
                                var vectorBetween = hit.Intersection - borderPoint;

                                translation.X += vectorBetween.X;
                                translation.Y += vectorBetween.Y;

                                context.Translation = translation;
                            }*/
                        }
                    }
                }

                return BehaviorStatus.Success;
            }

            return BehaviorStatus.Failure;
        }

        public override void Reset() { }
    }
}
