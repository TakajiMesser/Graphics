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

            var wall = MapBrush.Rectangle(new Vector3(10.0f, 0.0f, -0.5f), 5.0f, 20.0f);
            wall.HasCollision = true;
            map.Brushes.Add(wall);

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
                    new GameProperty("ENTER_COVER_SPEED", typeof(float), 0.12f, true),
                    new GameProperty("COVER_DISTANCE", typeof(float), 3.0f, true),
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
                MeshFilePath = FilePathHelper.TRIANGLE_MESH_PATH,
                BehaviorFilePath = FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH
            };
        }

        #endregion

        #region BehaviorTrees

        public static void CreateTestEnemyBehavior()
        {
            // We need to create the behavior where the enemy will patrol a set of points
            var behavior = new BehaviorTree
            {
                RootNode = new SequenceNode(
                    new NavigateNode(new Vector3(5.0f, 5.0f, -1.0f), 0.1f),
                    new NavigateNode(new Vector3(-1.0f, -1.0f, -1.0f), 0.1f)
                )
            };

            behavior.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);
        }

        public static void CreateTestPlayerBehavior()
        {
            CreatePlayerMovementNode();
            CreatePlayerTurnNode();
            CreatePlayerEvadeNode();
            CreatePlayerCoverNode();

            var behavior = new BehaviorTree
            {
                RootNode = new SelectorNode(
                    Node.Load(FilePathHelper.PLAYER_COVER_BEHAVIOR_PATH),
                    new SequenceNode(
                        new SelectorNode(
                            Node.Load(FilePathHelper.PLAYER_EVADE_BEHAVIOR_PATH),
                            Node.Load(FilePathHelper.PLAYER_MOVEMENT_BEHAVIOR_PATH)
                        ),
                        Node.Load(FilePathHelper.PLAYER_TURN_BEHAVIOR_PATH)
                    )
                )
            };

            behavior.Save(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
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
                    var coverDistance = (float)v["COVER_DISTANCE"];
                    var inputState = (InputState)v["InputState"];
                    var inputMapping = (InputMapping)v["InputMapping"];
                    var position = (Vector3)v["Position"];
                    var colliders = (IEnumerable<Collider>)v["Colliders"];
                    var nEvadeTicks = v.ContainsKey("nEvadeTicks") ? (int)v["nEvadeTicks"] : 0;

                    if (inputState.IsPressed(inputMapping.Cover))
                    {
                        // TODO - Filter gameobjects and brushes based on "coverable" property
                        var filteredColliders = colliders.Where(c => c.AttachedObject.GetType() == typeof(Brush));

                        if (Raycast.TryCircleCast(new Circle(position, coverDistance), filteredColliders, out RaycastHit hit))
                        {
                            var vectorBetween = hit.Intersection - position;
                            v["coverDirection"] = vectorBetween.Xy;
                            v["coverDistance"] = vectorBetween.Length;

                            float turnAngle = (float)Math.Atan2(vectorBetween.Y, vectorBetween.X);
                            v["Rotation"] = new Quaternion(new Vector3(turnAngle + (float)Math.PI, 0.0f, 0.0f));

                            return BehaviorStatuses.Success;
                        }
                    }
                    else if (inputState.IsHeld(inputMapping.Cover))
                    {
                        if (v.ContainsKey("coverDirection"))
                        {
                            if (v.ContainsKey("coverDistance"))
                            {
                                if ((float)v["coverDistance"] > 0.0f)
                                {
                                    v["coverDistance"] = (float)v["coverDistance"] - enterCoverSpeed;

                                    var coverDirection = (Vector2)v["coverDirection"];
                                    v["Translation"] = new Vector3(coverDirection.X, coverDirection.Y, 0) * enterCoverSpeed;
                                }
                                else
                                {
                                    // Handle movement while in cover here
                                }
                            }

                            return BehaviorStatuses.Success;
                        }
                    }
                    else if (inputState.IsReleased(inputMapping.Cover))
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
                    var inputState = (InputState)v["InputState"];
                    var inputMapping = (InputMapping)v["InputMapping"];
                    var nEvadeTicks = v.ContainsKey("nEvadeTicks") ? (int)v["nEvadeTicks"] : 0;

                    if (nEvadeTicks == 0 && inputState.IsPressed(inputMapping.Evade))
                    {
                        var evadeTranslation = new Vector3();

                        if (inputState.IsHeld(inputMapping.Forward))
                        {
                            evadeTranslation.Y += evadeSpeed;
                        }

                        if (inputState.IsHeld(inputMapping.Left))
                        {
                            evadeTranslation.X -= evadeSpeed;
                        }

                        if (inputState.IsHeld(inputMapping.Backward))
                        {
                            evadeTranslation.Y -= evadeSpeed;
                        }

                        if (inputState.IsHeld(inputMapping.Right))
                        {
                            evadeTranslation.X += evadeSpeed;
                        }

                        if (inputState.IsHeld(inputMapping.In))
                        {
                            evadeTranslation.Z += evadeSpeed;
                        }

                        if (inputState.IsHeld(inputMapping.Out))
                        {
                            evadeTranslation.Z -= evadeSpeed;
                        }

                        if (evadeTranslation != Vector3.Zero)
                        {
                            nEvadeTicks++;

                            v["evadeTranslation"] = evadeTranslation;
                            v["Scale"] = new Vector3(1.0f, 0.5f, 1.0f);
                            v["Rotation"] = new Quaternion(new Vector3((float)Math.Atan2(evadeTranslation.Y, evadeTranslation.X), 0.0f, 0.0f));
                            v["Translation"] = v["evadeTranslation"];
                            v["nEvadeTicks"] = nEvadeTicks;

                            return BehaviorStatuses.Success;
                        }
                    }
                    else if (nEvadeTicks > evadeTickCount)
                    {
                        nEvadeTicks = 0;
                        v["nEvadeTicks"] = nEvadeTicks;
                        v["Scale"] = Vector3.One;

                        return BehaviorStatuses.Failure;
                    }
                    else if (nEvadeTicks > 0)
                    {
                        nEvadeTicks++;
                        v["nEvadeTicks"] = nEvadeTicks;
                        v["Translation"] = v["evadeTranslation"];

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
                    var inputState = (InputState)v["InputState"];
                    var inputMapping = (InputMapping)v["InputMapping"];

                    var speed = inputState.IsHeld(inputMapping.Run)
                        ? runSpeed
                        : inputState.IsHeld(inputMapping.Crawl)
                            ? creepSpeed
                            : walkSpeed;

                    var translation = new Vector3();

                    if (inputState.IsHeld(inputMapping.Forward))
                    {
                        translation.Y += speed;
                    }

                    if (inputState.IsHeld(inputMapping.Left))
                    {
                        translation.X -= speed;
                    }

                    if (inputState.IsHeld(inputMapping.Backward))
                    {
                        translation.Y -= speed;
                    }

                    if (inputState.IsHeld(inputMapping.Right))
                    {
                        translation.X += speed;
                    }

                    if (inputState.IsHeld(inputMapping.In))
                    {
                        translation.Z += speed;
                    }

                    if (inputState.IsHeld(inputMapping.Out))
                    {
                        translation.Z -= speed;
                    }

                    v["Translation"] = translation;
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
                    var inputState = (InputState)v["InputState"];
                    var camera = (Camera)v["Camera"];
                    var nEvadeTicks = v.ContainsKey("nEvadeTicks") ? (int)v["nEvadeTicks"] : 0;

                    // Compare current position to location of mouse, and set rotation to face the mouse
                    if (nEvadeTicks == 0 && inputState.IsMouseInWindow)
                    {
                        var clipSpacePosition = camera.ViewProjectionMatrix * new Vector4(0.0f, 0.0f, 0.0f, 1.0f);//Position, 1.0f);
                        var screenCoordinates = new Vector2()
                        {
                            X = ((clipSpacePosition.X + 1.0f) / 2.0f) * inputState.WindowWidth,
                            Y = ((1.0f - clipSpacePosition.Y) / 2.0f) * inputState.WindowHeight,
                        };

                        var vectorBetween = inputState.MouseCoordinates - screenCoordinates;
                        float turnAngle = -(float)Math.Atan2(vectorBetween.Y, vectorBetween.X);

                        v["Rotation"] = new Quaternion(new Vector3(turnAngle, 0.0f, 0.0f));
                    }

                    return BehaviorStatuses.Success;
                }
            };

            node.Save(FilePathHelper.PLAYER_TURN_BEHAVIOR_PATH);
        }

        #endregion
    }
}
