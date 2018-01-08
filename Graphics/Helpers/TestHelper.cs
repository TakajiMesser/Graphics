using Graphics.Meshes;
using Graphics.Utilities;
using Graphics.Rendering.Vertices;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Graphics.Maps;
using OpenTK;
using Graphics.GameObjects;
using Graphics.Scripting.BehaviorTrees;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Leaves;
using Graphics.Physics.Raycasting;
using Graphics.Inputs;
using Graphics.Physics.Collision;
using Graphics.Lighting;
using Graphics.Scripting.BehaviorTrees.Decorators;

namespace Graphics.Helpers
{
    /// <summary>
    /// For now, this is a helper class for creating test objects
    /// </summary>
    public static class TestHelper
    {
        #region Maps

        public static void CreateTestMap()
        {
            var map = new Map()
            {
                Camera = CreateCameraObject()
            };

            map.GameObjects.Add(CreatePlayerObject());
            map.GameObjects.Add(CreateEnemyObject());

            map.Brushes.Add(MapBrush.Rectangle(new Vector3(0.0f, 0.0f, -2.0f), 50.0f, 50.0f));

            var wall = MapBrush.Rectangle(new Vector3(10.0f, 0.0f, -0.5f), 5.0f, 10.0f);
            wall.HasCollision = true;
            wall.TextureFilePath = FilePathHelper.BRICK_01_TEXTURE_PATH;
            map.Brushes.Add(wall);

            var wall2 = MapBrush.Rectangle(new Vector3(-10.0f, 0.0f, -0.5f), 5.0f, 10.0f);
            wall2.HasCollision = true;
            wall2.TextureFilePath = FilePathHelper.BRICK_01_TEXTURE_PATH;
            map.Brushes.Add(wall2);

            map.Lights.Add(new Light()
            {
                Position = new Vector3(0.0f, 0.0f, 0.0f),
                Radius = 30.0f,
                Color = new Vector3(1.0f, 1.0f, 1.0f),
                Intensity = 0.5f
            });
            map.Lights.Add(new Light()
            {
                Position = new Vector3(0.0f, 20.0f, 0.0f),
                Radius = 30.0f,
                Color = new Vector3(1.0f, 1.0f, 1.0f),
                Intensity = 0.25f
            });

            map.Save(FilePathHelper.MAP_PATH);
        }

        private static MapCamera CreateCameraObject()
        {
            return new MapCamera()
            {
                Name = "MainCamera",
                AttachedGameObjectName = "Player",
                Position = Vector3.Zero
            };
        }

        private static MapGameObject CreatePlayerObject()
        {
            return new MapGameObject()
            {
                Name = "Player",
                Position = new Vector3(0.0f, 0.0f, -1.0f),
                Scale = Vector3.One,
                Rotation = Quaternion.Identity,
                MeshFilePath = FilePathHelper.PLAYER_MESH_PATH,
                BehaviorFilePath = FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH,
                Properties = new List<GameProperty>
                {
                    new GameProperty("WALK_SPEED", typeof(float), 0.1f, true),
                    new GameProperty("RUN_SPEED", typeof(float), 0.15f, true),
                    new GameProperty("CREEP_SPEED", typeof(float), 0.04f, true),
                    new GameProperty("EVADE_SPEED", typeof(float), 0.175f, true),
                    new GameProperty("COVER_SPEED", typeof(float), 0.1f, true),
                    new GameProperty("ENTER_COVER_SPEED", typeof(float), 0.12f, true),
                    new GameProperty("COVER_DISTANCE", typeof(float), 5.0f, true),
                    new GameProperty("EVADE_TICK_COUNT", typeof(int), 20, true)
                }
            };
        }

        private static MapGameObject CreateEnemyObject()
        {
            return new MapGameObject()
            {
                Name = "Enemy",
                Position = new Vector3(5.0f, 5.0f, -1.0f),
                Scale = Vector3.One,
                Rotation = Quaternion.Identity,
                MeshFilePath = FilePathHelper.ENEMY_MESH_PATH,
                BehaviorFilePath = FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH,
                Properties = new List<GameProperty>
                {
                    new GameProperty("WALK_SPEED", typeof(float), 0.1f, true),
                    new GameProperty("VIEW_ANGLE", typeof(float), 0.5236f, true)
                }
            };
        }

        #endregion

        #region BehaviorTrees

        public static void CreateTestEnemyBehavior()
        {
            CreateCheckPlayerInSightBehavior();
            CreateTurnTowardsBehavior();

            var rootNode = new SelectorNode(
                new RepeaterNode(
                    Node.Load(FilePathHelper.ENEMY_SEARCH_PLAYER_BEHAVIOR_PATH)
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
                Behavior = (v) =>
                {
                    var turnAngle = v.Rotation.X;//2.0f * (float)Math.Acos(v.Rotation.Z);
                    var player = v.Colliders.FirstOrDefault(c => c.AttachedObject.GetType() == typeof(GameObject) && ((GameObject)c.AttachedObject).Name == "Player");

                    if (player != null)
                    {
                        var playerPosition = ((GameObject)player.AttachedObject).Position;

                        var playerDirection = playerPosition - v.Position;
                        float playerAngle = (float)Math.Atan2(playerDirection.Y, playerDirection.X);

                        var angleDifference = (playerAngle - turnAngle + Math.PI) % (2 * Math.PI) - Math.PI;
                        if (angleDifference < -Math.PI)
                        {
                            angleDifference += (float)(2 * Math.PI);
                        }

                        var viewAngle = (float)v["VIEW_ANGLE"];
                        if (Math.Abs(angleDifference) <= viewAngle / 2.0f)
                        {
                            return BehaviorStatuses.Success;
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
                Behavior = (v) =>
                {
                    if (v.Translation != Vector3.Zero)
                    {
                        float turnAngle = (float)Math.Atan2(v.Translation.Y, v.Translation.X);
                        v.Rotation = new Vector3(turnAngle, v.Rotation.Y, v.Rotation.Z);
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
                Behavior = (v) =>
                {
                    var enterCoverSpeed = (float)v["ENTER_COVER_SPEED"];
                    var evadeSpeed = (float)v["EVADE_SPEED"];
                    var evadeTickCount = (int)v["EVADE_TICK_COUNT"];
                    var coverSpeed = (float)v["COVER_SPEED"];
                    var coverDistance = (float)v["COVER_DISTANCE"];
                    var nEvadeTicks = v.ContainsKey("nEvadeTicks") ? (int)v["nEvadeTicks"] : 0;

                    if (v.InputState.IsPressed(v.InputMapping.Cover))
                    {
                        // TODO - Filter gameobjects and brushes based on "coverable" property
                        var filteredColliders = v.Colliders.Where(c => c.AttachedObject.GetType() == typeof(Brush));

                        if (Raycast.TryCircleCast(new Circle(v.Position, coverDistance), filteredColliders, out RaycastHit hit))
                        {
                            var vectorBetween = hit.Intersection - v.Position;
                            v["coverDirection"] = vectorBetween.Xy;
                            v["coverDistance"] = vectorBetween.Length;

                            float turnAngle = (float)Math.Atan2(vectorBetween.Y, vectorBetween.X);
                            v.Rotation = new Vector3(turnAngle + (float)Math.PI, v.Rotation.Y, v.Rotation.Z);

                            return BehaviorStatuses.Success;
                        }
                    }
                    else if (v.InputState.IsHeld(v.InputMapping.Cover))
                    {
                        if (v.ContainsKey("coverDirection"))
                        {
                            if (v.ContainsKey("coverDistance"))
                            {
                                if ((float)v["coverDistance"] > 0.0f)
                                {
                                    v["coverDistance"] = (float)v["coverDistance"] - enterCoverSpeed;

                                    var coverDirection = (Vector2)v["coverDirection"];
                                    v.Translation = new Vector3(coverDirection.X, coverDirection.Y, 0) * enterCoverSpeed;
                                }

                                if ((float)v["coverDistance"] < 2.0f)
                                {
                                    // Handle movement while in cover here
                                    var translation = new Vector3();

                                    if (v.InputState.IsHeld(v.InputMapping.Forward))
                                    {
                                        translation.Y += coverSpeed;
                                    }

                                    if (v.InputState.IsHeld(v.InputMapping.Left))
                                    {
                                        translation.X -= coverSpeed;
                                    }

                                    if (v.InputState.IsHeld(v.InputMapping.Backward))
                                    {
                                        translation.Y -= coverSpeed;
                                    }

                                    if (v.InputState.IsHeld(v.InputMapping.Right))
                                    {
                                        translation.X += coverSpeed;
                                    }

                                    if (translation != Vector3.Zero)
                                    {
                                        var filteredColliders = v.Colliders.Where(c => c.AttachedObject.GetType() == typeof(Brush));

                                        var coverDirection = (Vector2)v["coverDirection"];
                                        if (Raycast.TryRaycast(new Ray3(v.Position + translation, new Vector3(coverDirection.X, coverDirection.Y, 0.0f), 1.0f), filteredColliders, out RaycastHit hit))
                                        {
                                            var vectorBetween = hit.Intersection - (v.Position + translation);

                                            translation.X += vectorBetween.X;
                                            translation.Y += vectorBetween.Y;

                                            v.Translation = translation;
                                        }
                                    }
                                }
                            }

                            return BehaviorStatuses.Success;
                        }
                    }
                    else if (v.InputState.IsReleased(v.InputMapping.Cover))
                    {
                        if (v.ContainsKey("coverDirection"))
                        {
                            v.Remove("coverDirection");
                        }

                        if (v.ContainsKey("coverDistance"))
                        {
                            v.Remove("coverDistance");
                        }

                        return BehaviorStatuses.Success;
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
                Behavior = (v) =>
                {
                    var evadeSpeed = (float)v["EVADE_SPEED"];
                    var evadeTickCount = (int)v["EVADE_TICK_COUNT"];
                    var nEvadeTicks = v.ContainsKey("nEvadeTicks") ? (int)v["nEvadeTicks"] : 0;

                    if (nEvadeTicks == 0 && v.InputState.IsPressed(v.InputMapping.Evade))
                    {
                        var evadeTranslation = new Vector3();

                        if (v.InputState.IsHeld(v.InputMapping.Forward))
                        {
                            evadeTranslation.Y += evadeSpeed;
                        }

                        if (v.InputState.IsHeld(v.InputMapping.Left))
                        {
                            evadeTranslation.X -= evadeSpeed;
                        }

                        if (v.InputState.IsHeld(v.InputMapping.Backward))
                        {
                            evadeTranslation.Y -= evadeSpeed;
                        }

                        if (v.InputState.IsHeld(v.InputMapping.Right))
                        {
                            evadeTranslation.X += evadeSpeed;
                        }

                        if (v.InputState.IsHeld(v.InputMapping.In))
                        {
                            evadeTranslation.Z += evadeSpeed;
                        }

                        if (v.InputState.IsHeld(v.InputMapping.Out))
                        {
                            evadeTranslation.Z -= evadeSpeed;
                        }

                        if (evadeTranslation != Vector3.Zero)
                        {
                            nEvadeTicks++;

                            v.Rotation = new Vector3((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), v.Rotation.Y, v.Rotation.Z);
                            v.Scale = new Vector3(1.0f, 0.5f, 1.0f);
                            v.Translation = evadeTranslation;

                            v["evadeTranslation"] = evadeTranslation;
                            v["nEvadeTicks"] = nEvadeTicks;

                            return BehaviorStatuses.Success;
                        }
                    }
                    else if (nEvadeTicks > evadeTickCount)
                    {
                        nEvadeTicks = 0;
                        v["nEvadeTicks"] = nEvadeTicks;
                        v.Scale = Vector3.One;

                        return BehaviorStatuses.Failure;
                    }
                    else if (nEvadeTicks > 0)
                    {
                        nEvadeTicks++;
                        v["nEvadeTicks"] = nEvadeTicks;
                        v.Translation = (Vector3)v["evadeTranslation"];

                        return BehaviorStatuses.Success;
                    }

                    v["nEvadeTicks"] = nEvadeTicks;
                    return BehaviorStatuses.Failure;
                }
            };

            node.Save(FilePathHelper.PLAYER_EVADE_BEHAVIOR_PATH);
        }

        private static void CreatePlayerMovementNode()
        {
            var node = new LeafNode()
            {
                Behavior = (v) =>
                {
                    var runSpeed = (float)v["RUN_SPEED"];
                    var creepSpeed = (float)v["CREEP_SPEED"];
                    var walkSpeed = (float)v["WALK_SPEED"];

                    var speed = v.InputState.IsHeld(v.InputMapping.Run)
                        ? runSpeed
                        : v.InputState.IsHeld(v.InputMapping.Crawl)
                            ? creepSpeed
                            : walkSpeed;

                    var translation = new Vector3();

                    if (v.InputState.IsHeld(v.InputMapping.Forward))
                    {
                        translation.Y += speed;
                    }

                    if (v.InputState.IsHeld(v.InputMapping.Left))
                    {
                        translation.X -= speed;
                    }

                    if (v.InputState.IsHeld(v.InputMapping.Backward))
                    {
                        translation.Y -= speed;
                    }

                    if (v.InputState.IsHeld(v.InputMapping.Right))
                    {
                        translation.X += speed;
                    }

                    if (v.InputState.IsHeld(v.InputMapping.In))
                    {
                        translation.Z += speed;
                    }

                    if (v.InputState.IsHeld(v.InputMapping.Out))
                    {
                        translation.Z -= speed;
                    }

                    v.Translation = translation;
                    return BehaviorStatuses.Success;
                }
            };

            node.Save(FilePathHelper.PLAYER_MOVEMENT_BEHAVIOR_PATH);
        }

        private static void CreatePlayerTurnNode()
        {
            var node = new LeafNode()
            {
                Behavior = (v) =>
                {
                    var nEvadeTicks = v.ContainsKey("nEvadeTicks") ? (int)v["nEvadeTicks"] : 0;

                    // Compare current position to location of mouse, and set rotation to face the mouse
                    if (nEvadeTicks == 0 && v.InputState.IsMouseInWindow)
                    {
                        var clipSpacePosition = v.Camera.ViewProjectionMatrix * new Vector4(0.0f, 0.0f, 0.0f, 1.0f);//Position, 1.0f);
                        var screenCoordinates = new Vector2()
                        {
                            X = ((clipSpacePosition.X + 1.0f) / 2.0f) * v.InputState.WindowWidth,
                            Y = ((1.0f - clipSpacePosition.Y) / 2.0f) * v.InputState.WindowHeight,
                        };

                        var vectorBetween = v.InputState.MouseCoordinates - screenCoordinates;
                        float turnAngle = -(float)Math.Atan2(vectorBetween.Y, vectorBetween.X);

                        v.Rotation = new Vector3(turnAngle, v.Rotation.Y, v.Rotation.Z);
                    }

                    return BehaviorStatuses.Success;
                }
            };

            node.Save(FilePathHelper.PLAYER_TURN_BEHAVIOR_PATH);
        }

        #endregion
    }
}
