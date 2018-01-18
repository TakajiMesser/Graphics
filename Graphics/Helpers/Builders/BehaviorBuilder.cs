using Graphics.GameObjects;
using Graphics.Physics.Raycasting;
using Graphics.Scripting.BehaviorTrees;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using Graphics.Scripting.BehaviorTrees.Leaves;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Helpers.Builders
{
    public static class BehaviorBuilder
    {
        public static void CreateTestEnemyBehavior()
        {
            CreateCheckPlayerInSightBehavior();
            CreateTurnTowardsBehavior();

            var rootNode = new SelectorNode(
                new RepeaterNode(
                    new SequenceNode(
                        Node.Load(FilePathHelper.ENEMY_SEARCH_PLAYER_BEHAVIOR_PATH),
                        new LeafNode()
                        {
                            Behavior = (c) =>
                            {
                                // Check if we are at full alertness
                                return BehaviorStatuses.Success;
                            }
                        },
                        new LeafNode()
                        {
                            Behavior = (c) =>
                            {
                                // Chase player
                                return BehaviorStatuses.Success;
                            }
                        }
                    )
                ),
                new SequenceNode(
                    new ParallelNode(
                        new NavigateNode(new Vector3(5.0f, 5.0f, -1.0f), 0.1f),
                        Node.Load(FilePathHelper.ENEMY_TURN_BEHAVIOR_PATH)
                    ),
                    new ParallelNode(
                        new NavigateNode(new Vector3(5.0f, -5.0f, -1.0f), 0.1f),
                        Node.Load(FilePathHelper.ENEMY_TURN_BEHAVIOR_PATH)
                    ),
                    new ParallelNode(
                        new NavigateNode(new Vector3(-5.0f, -5.0f, -1.0f), 0.1f),
                        Node.Load(FilePathHelper.ENEMY_TURN_BEHAVIOR_PATH)
                    ),
                    new ParallelNode(
                        new NavigateNode(new Vector3(-5.0f, 5.0f, -1.0f), 0.1f),
                        Node.Load(FilePathHelper.ENEMY_TURN_BEHAVIOR_PATH)
                    )
                )
            );

            rootNode.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);
        }

        private static void CreateCheckPlayerInSightBehavior()
        {
            var node = new LeafNode()
            {
                Behavior = (context) =>
                {
                    var player = context.Colliders.FirstOrDefault(c => c.AttachedObject.GetType() == typeof(GameObject) && ((GameObject)c.AttachedObject).Name == "Player");

                    if (player != null)
                    {
                        var playerPosition = ((GameObject)player.AttachedObject).Position;

                        var playerDirection = playerPosition - context.Position;
                        float playerAngle = (float)Math.Atan2(playerDirection.Y, playerDirection.X);

                        var angleDifference = (playerAngle - context.Rotation.X + Math.PI) % (2 * Math.PI) - Math.PI;
                        if (angleDifference < -Math.PI)
                        {
                            angleDifference += (float)(2 * Math.PI);
                        }

                        var viewAngle = context.GetProperty<float>("VIEW_ANGLE");
                        if (Math.Abs(angleDifference) <= viewAngle / 2.0f)
                        {
                            // Perform a raycast to see if any other colliders obstruct our view of the player
                            // TODO - Filter colliders by their ability to obstruct vision
                            var viewDistance = context.GetProperty<float>("VIEW_DISTANCE");
                            if (Raycast.TryRaycast(new Ray3(context.Position, playerDirection, viewDistance), context.Colliders, out RaycastHit hit))
                            {
                                if (hit.Collider.AttachedObject.GetType() == typeof(GameObject) && ((GameObject)hit.Collider.AttachedObject).Name == "Player")
                                {
                                    return BehaviorStatuses.Success;
                                }
                            }
                        }
                    }

                    return BehaviorStatuses.Failure;
                }
            };

            node.Save(FilePathHelper.ENEMY_SEARCH_PLAYER_BEHAVIOR_PATH);
        }

        private static void CreateTurnTowardsBehavior()
        {
            var node = new LeafNode()
            {
                Behavior = (context) =>
                {
                    if (context.Translation != Vector3.Zero)
                    {
                        float turnAngle = (float)Math.Atan2(context.Translation.Y, context.Translation.X);

                        context.QRotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                        context.Rotation = new Vector3(turnAngle, context.Rotation.Y, context.Rotation.Z);
                    }

                    return BehaviorStatuses.Success;
                }
            };

            node.Save(FilePathHelper.ENEMY_TURN_BEHAVIOR_PATH);
        }

        public static void CreateTestPlayerBehavior()
        {
            CreatePlayerMovementNode();
            CreatePlayerTurnNode();
            CreatePlayerEvadeNode();
            CreatePlayerCoverNode();

            var rootNode = new SelectorNode(
                Node.Load(FilePathHelper.PLAYER_COVER_BEHAVIOR_PATH),
                new SequenceNode(
                    new SelectorNode(
                        Node.Load(FilePathHelper.PLAYER_EVADE_BEHAVIOR_PATH),
                        Node.Load(FilePathHelper.PLAYER_MOVEMENT_BEHAVIOR_PATH)
                    ),
                    Node.Load(FilePathHelper.PLAYER_TURN_BEHAVIOR_PATH)
                )
            );

            rootNode.Save(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
        }

        private static void CreatePlayerCoverNode()
        {
            var node = new LeafNode()
            {
                Behavior = (context) =>
                {
                    var enterCoverSpeed = context.GetProperty<float>("ENTER_COVER_SPEED");
                    var coverSpeed = context.GetProperty<float>("COVER_SPEED");
                    var coverDistance = context.GetProperty<float>("COVER_DISTANCE");
                    var nEvadeTicks = context.ContainsVariable("nEvadeTicks") ? context.GetVariable<int>("nEvadeTicks") : 0;

                    if (nEvadeTicks == 0)
                    {
                        if (context.InputState.IsPressed(context.InputMapping.Cover))
                        {
                            // TODO - Filter gameobjects and brushes based on "coverable" property
                            var filteredColliders = context.Colliders.Where(c => c.AttachedObject.GetType() == typeof(Brush));

                            if (Raycast.TryCircleCast(new Circle(context.Position, coverDistance), filteredColliders, out RaycastHit hit))
                            {
                                var vectorBetween = hit.Intersection - context.Position;
                                context.SetVariable("coverDirection", vectorBetween.Xy);
                                context.SetVariable("coverDistance", vectorBetween.Length);

                                float turnAngle = (float)Math.Atan2(vectorBetween.Y, vectorBetween.X);
                                context.QRotation = new Quaternion(turnAngle + (float)Math.PI, 0.0f, 0.0f);
                                context.Rotation = new Vector3(turnAngle + (float)Math.PI, context.Rotation.Y, context.Rotation.Z);

                                return BehaviorStatuses.Success;
                            }
                        }
                        else if (context.InputState.IsHeld(context.InputMapping.Cover))
                        {
                            if (context.ContainsVariable("coverDirection"))
                            {
                                if (context.ContainsVariable("coverDistance"))
                                {
                                    if (context.GetVariable<float>("coverDistance") > 0.0f)
                                    {
                                        context.SetVariable("coverDistance", context.GetVariable<float>("coverDistance") - enterCoverSpeed);

                                        var coverDirection = context.GetVariable<Vector2>("coverDirection");
                                        context.Translation = new Vector3(coverDirection.X, coverDirection.Y, 0) * enterCoverSpeed;
                                    }

                                    if (context.GetVariable<float>("coverDistance") < 0.1f)
                                    {
                                        // Handle movement while in cover here
                                        var translation = context.GetTranslation(coverSpeed);

                                        if (translation != Vector3.Zero)
                                        {
                                            var filteredColliders = context.Colliders.Where(c => c.AttachedObject.GetType() == typeof(Brush));

                                            // Calculate the furthest point along the bounds of our object, since we should attempt to raycast from there
                                            var borderPoint = context.Bounds.GetBorder(translation);

                                            var coverDirection = context.GetVariable<Vector2>("coverDirection");
                                            if (Raycast.TryRaycast(new Ray3(borderPoint, new Vector3(coverDirection.X, coverDirection.Y, 0.0f), 1.0f), filteredColliders, out RaycastHit hit))
                                            {
                                                var vectorBetween = hit.Intersection - borderPoint;

                                                translation.X += vectorBetween.X;
                                                translation.Y += vectorBetween.Y;

                                                context.Translation = translation;
                                            }
                                        }
                                    }
                                }

                                return BehaviorStatuses.Success;
                            }
                        }
                        else if (context.InputState.IsReleased(context.InputMapping.Cover))
                        {
                            context.RemoveVariableIfExists("coverDirection");
                            context.RemoveVariableIfExists("coverDistance");

                            return BehaviorStatuses.Success;
                        }
                    }

                    return BehaviorStatuses.Failure;
                }
            };

            node.Save(FilePathHelper.PLAYER_COVER_BEHAVIOR_PATH);
        }

        private static void CreatePlayerEvadeNode()
        {
            var node = new LeafNode()
            {
                Behavior = (context) =>
                {
                    var evadeSpeed = context.GetProperty<float>("EVADE_SPEED");
                    var evadeTickCount = context.GetProperty<int>("EVADE_TICK_COUNT");
                    var nEvadeTicks = context.ContainsVariable("nEvadeTicks") ? context.GetVariable<int>("nEvadeTicks") : 0;

                    if (nEvadeTicks == 0 && context.InputState.IsPressed(context.InputMapping.Evade))
                    {
                        var evadeTranslation = context.GetTranslation(evadeSpeed);

                        if (evadeTranslation != Vector3.Zero)
                        {
                            nEvadeTicks++;

                            var angle = (float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X);

                            context.QRotation = new Quaternion(0.0f, 0.0f, (float)Math.Sin(angle / 2), (float)Math.Cos(angle / 2));
                            context.Rotation = new Vector3((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), context.Rotation.Y, context.Rotation.Z);
                            //context.Scale = new Vector3(1.0f, 0.5f, 1.0f);
                            context.Translation = evadeTranslation;

                            context.SetVariable("evadeTranslation", evadeTranslation);
                            context.SetVariable("nEvadeTicks", nEvadeTicks);

                            return BehaviorStatuses.Success;
                        }
                    }
                    else if (nEvadeTicks > evadeTickCount)
                    {
                        nEvadeTicks = 0;
                        context.SetVariable("nEvadeTicks", nEvadeTicks);
                        context.QRotation = Quaternion.Identity;
                        context.Rotation = new Vector3(context.Rotation.X, 0.0f, context.Rotation.Z);
                        context.Scale = Vector3.One;

                        return BehaviorStatuses.Failure;
                    }
                    else if (nEvadeTicks > 0)
                    {
                        var evadeTranslation = context.GetVariable<Vector3>("evadeTranslation");

                        context.QRotation = Quaternion.FromAxisAngle(Vector3.Cross(evadeTranslation.Normalized(), -Vector3.UnitZ), 2.0f * (float)Math.PI / evadeTickCount * nEvadeTicks);
                        context.QRotation *= new Quaternion((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), 0.0f, 0.0f);
                        context.Rotation = new Vector3(context.Rotation.X, 2.0f * (float)Math.PI / evadeTickCount * nEvadeTicks, context.Rotation.Z);
                        nEvadeTicks++;
                        context.SetVariable("nEvadeTicks", nEvadeTicks);
                        context.Translation = evadeTranslation;

                        return BehaviorStatuses.Success;
                    }

                    context.SetVariable("nEvadeTicks", nEvadeTicks);
                    return BehaviorStatuses.Failure;
                }
            };

            node.Save(FilePathHelper.PLAYER_EVADE_BEHAVIOR_PATH);
        }

        private static void CreatePlayerMovementNode()
        {
            var node = new LeafNode()
            {
                Behavior = (context) =>
                {
                    var runSpeed = context.GetProperty<float>("RUN_SPEED");
                    var creepSpeed = context.GetProperty<float>("CREEP_SPEED");
                    var walkSpeed = context.GetProperty<float>("WALK_SPEED");

                    var speed = context.InputState.IsHeld(context.InputMapping.Run)
                        ? runSpeed
                        : context.InputState.IsHeld(context.InputMapping.Crawl)
                            ? creepSpeed
                            : walkSpeed;

                    var translation = context.GetTranslation(speed);

                    if (context.InputState.IsHeld(context.InputMapping.In))
                    {
                        translation.Z += speed;
                    }

                    if (context.InputState.IsHeld(context.InputMapping.Out))
                    {
                        translation.Z -= speed;
                    }

                    if (context.InputState.IsHeld(context.InputMapping.ItemSlot1))
                    {
                        context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y + 0.1f, context.Rotation.Z);
                    }

                    if (context.InputState.IsHeld(context.InputMapping.ItemSlot2))
                    {
                        context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y - 0.1f, context.Rotation.Z);
                    }

                    if (context.InputState.IsHeld(context.InputMapping.ItemSlot3))
                    {
                        context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y, context.Rotation.Z + 0.1f);
                    }

                    if (context.InputState.IsHeld(context.InputMapping.ItemSlot4))
                    {
                        context.Rotation = new Vector3(context.Rotation.X, context.Rotation.Y, context.Rotation.Z - 0.1f);
                    }

                    context.Translation = translation;
                    return BehaviorStatuses.Success;
                }
            };

            node.Save(FilePathHelper.PLAYER_MOVEMENT_BEHAVIOR_PATH);
        }

        private static void CreatePlayerTurnNode()
        {
            var node = new LeafNode()
            {
                Behavior = (context) =>
                {
                    var nEvadeTicks = context.ContainsVariable("nEvadeTicks") ? context.GetVariable<int>("nEvadeTicks") : 0;

                    // Compare current position to location of mouse, and set rotation to face the mouse
                    if (!context.InputState.IsHeld(context.InputMapping.ItemWheel) && nEvadeTicks == 0 && context.InputState.IsMouseInWindow)
                    {
                        var clipSpacePosition = context.Camera.ViewProjectionMatrix * new Vector4(0.0f, 0.0f, 0.0f, 1.0f);//new Vector4(context.Position, 1.0f);
                        var screenCoordinates = new Vector2()
                        {
                            X = ((clipSpacePosition.X + 1.0f) / 2.0f) * context.InputState.WindowWidth,
                            Y = ((1.0f - clipSpacePosition.Y) / 2.0f) * context.InputState.WindowHeight,
                        };

                        var vectorBetween = context.InputState.MouseCoordinates - screenCoordinates;
                        float turnAngle = -(float)Math.Atan2(vectorBetween.Y, vectorBetween.X);

                        // Need to add the angle that the camera's Up vector is turned from Vector3.UnitY
                        turnAngle += (float)Math.Atan2(context.Camera._viewMatrix.Up.Y, context.Camera._viewMatrix.Up.X) - (float)Math.PI / 2.0f;

                        context.QRotation = new Quaternion(turnAngle, 0.0f, 0.0f);
                        context.Rotation = new Vector3(turnAngle, context.Rotation.Y, context.Rotation.Z);
                    }

                    return BehaviorStatuses.Success;
                }
            };

            node.Save(FilePathHelper.PLAYER_TURN_BEHAVIOR_PATH);
        }
    }
}
