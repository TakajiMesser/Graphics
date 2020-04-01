/*using SpiceEngineCore.Maps;
using SpiceEngineCore.Scripting.Meters;
using SpiceEngineCore.Scripting.Scripts;
using SpiceEngineCore.Scripting.StimResponse;*/
using OpenTK;
using SpiceEngine.Maps;
using System.Collections.Generic;
using TowerWarfare.Helpers;
using UmamiScriptingCore.Behaviors.StimResponse;
using UmamiScriptingCore.Scripts;

namespace TowerWarfare.Builders
{
    public static class BehaviorBuilder
    {
        public const float CAMERA_MOVE_SPEED = 0.02f;
        public const float CAMERA_TURN_SPEED = 0.001f;
        public const float CAMERA_ZOOM_SPEED = 1.0f;

        public const float ENEMY_WALK_SPEED = 0.1f;
        public const float ENEMY_CHASE_SPEED = 0.15f;
        public const float ENEMY_VIEW_ANGLE = 1.0472f;
        public const float ENEMY_VIEW_DISTANCE = 5.0f;
        public const int ENEMY_FULL_ALERT_TICKS = 120;

        public static void GenerateCameraBehavior(string filePath)
        {
            var behavior = new MapBehavior
            {
                RootNode = GenerateCameraRootNode()
            };

            behavior.Save(filePath);
        }

        private static MapNode GenerateCameraRootNode() => new MapNode()
        {
            NodeType = MapNode.NodeTypes.Repeater,
            Children = new List<MapNode>()
            {
                new MapNode(CAMERA_MOVE_SPEED, CAMERA_TURN_SPEED, CAMERA_ZOOM_SPEED)
                {
                    NodeType = MapNode.NodeTypes.Node,
                    Script = new Script()
                    {
                        Name = "CameraNode",
                        SourcePath = FilePathHelper.CAMERA_NODE_PATH
                    }
                }
            }
        };

        public static void GenerateButtonScript(string filePath)
        {

        }

        public static void GenerateEnemyBehavior(string filePath)
        {
            var behavior = new MapBehavior
            {
                RootNode = GenerateEnemyRootNode(),
                //Responses = GenerateEnemyResponses().ToList()
            };

            behavior.Save(filePath);
        }

        private static MapNode GenerateEnemyRootNode() => new MapNode()
        {
            NodeType = MapNode.NodeTypes.Repeater,
            Children = new List<MapNode>()
            {
                new MapNode()
                {
                    NodeType = MapNode.NodeTypes.Sequence,
                    Children = new List<MapNode>()
                    {
                        new MapNode()
                        {
                            NodeType = MapNode.NodeTypes.Parallel,
                            Children = new List<MapNode>()
                            {
                                new MapNode(new Vector3(5.0f, 5.0f, -0.5f), ENEMY_WALK_SPEED)
                                {
                                    NodeType = MapNode.NodeTypes.Node,
                                    Script = new Script()
                                    {
                                        Name = "MoveToNode",
                                        SourcePath = FilePathHelper.MOVE_TO_NODE_PATH
                                    }
                                },
                                new MapNode()
                                {
                                    NodeType = MapNode.NodeTypes.Node,
                                    Script = new Script()
                                    {
                                        Name = "TurnTowardsNode",
                                        SourcePath = FilePathHelper.TURN_TOWARDS_NODE_PATH
                                    }
                                }
                            }
                        },
                        new MapNode()
                        {
                            NodeType = MapNode.NodeTypes.Parallel,
                            Children = new List<MapNode>()
                            {
                                new MapNode(new Vector3(5.0f, -5.0f, -0.5f), ENEMY_WALK_SPEED)
                                {
                                    NodeType = MapNode.NodeTypes.Node,
                                    Script = new Script()
                                    {
                                        Name = "MoveToNode",
                                        SourcePath = FilePathHelper.MOVE_TO_NODE_PATH
                                    }
                                },
                                new MapNode()
                                {
                                    NodeType = MapNode.NodeTypes.Node,
                                    Script = new Script()
                                    {
                                        Name = "TurnTowardsNode",
                                        SourcePath = FilePathHelper.TURN_TOWARDS_NODE_PATH
                                    }
                                }
                            }
                        },
                        new MapNode()
                        {
                            NodeType = MapNode.NodeTypes.Parallel,
                            Children = new List<MapNode>()
                            {
                                new MapNode(new Vector3(-5.0f, -5.0f, -0.5f), ENEMY_WALK_SPEED)
                                {
                                    NodeType = MapNode.NodeTypes.Node,
                                    Script = new Script()
                                    {
                                        Name = "MoveToNode",
                                        SourcePath = FilePathHelper.MOVE_TO_NODE_PATH
                                    }
                                },
                                new MapNode()
                                {
                                    NodeType = MapNode.NodeTypes.Node,
                                    Script = new Script()
                                    {
                                        Name = "TurnTowardsNode",
                                        SourcePath = FilePathHelper.TURN_TOWARDS_NODE_PATH
                                    }
                                }
                            }
                        },
                        new MapNode()
                        {
                            NodeType = MapNode.NodeTypes.Parallel,
                            Children = new List<MapNode>()
                            {
                                new MapNode(new Vector3(-5.0f, 5.0f, -0.5f), ENEMY_WALK_SPEED)
                                {
                                    NodeType = MapNode.NodeTypes.Node,
                                    Script = new Script()
                                    {
                                        Name = "MoveToNode",
                                        SourcePath = FilePathHelper.MOVE_TO_NODE_PATH
                                    }
                                },
                                new MapNode()
                                {
                                    NodeType = MapNode.NodeTypes.Node,
                                    Script = new Script()
                                    {
                                        Name = "TurnTowardsNode",
                                        SourcePath = FilePathHelper.TURN_TOWARDS_NODE_PATH
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        /*private static IEnumerable<Response> GenerateEnemyResponses()
        {
            _alertMeter.AddTrigger(new Trigger("Alert", 120)
            {
                ResetOnTrigger = false
            });
            _alertMeter.Triggered += AlertMeter_Triggered;

            var playerResponse = new Response(Stimulus.Player)
            {
                TriggerOnContact = true,
                TriggerOnSight = true,
                SightAngle = 50.0f,
                SightDistance = 20.0f
            };
            playerResponse.Triggered += PlayerResponse_Triggered;

            yield return playerResponse;
        }*/
    }
}
