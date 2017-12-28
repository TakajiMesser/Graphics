using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Inputs;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using Graphics.Scripting.BehaviorTrees.Leaves;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Scripting.BehaviorTrees
{
    [DataContract]
    [KnownType(typeof(SelectorNode))]
    [KnownType(typeof(SequenceNode))]
    [KnownType(typeof(OrderedNode))]
    [KnownType(typeof(InverterNode))]
    [KnownType(typeof(LoopNode))]
    [KnownType(typeof(NavigateNode))]
    [KnownType(typeof(ConditionNode))]
    public class BehaviorTree
    {
        public BehaviorStatuses Status { get; private set; }
        public Dictionary<string, object> VariablesByName { get; protected set; } = new Dictionary<string, object>();

        [DataMember]
        public INode RootNode { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext c)
        {
            VariablesByName = new Dictionary<string, object>();
        }

        public void Tick()
        {
            if (Status.IsComplete())
            {
                Reset();
            }

            RootNode.Tick(VariablesByName);
            Status = RootNode.Status;
        }

        public void Reset()
        {
            Status = BehaviorStatuses.Dormant;
            RootNode.Reset();
        }

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        public static BehaviorTree Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();
                return serializer.ReadObject(reader, true) as BehaviorTree;
            }
        }

        public static void SaveTestEnemyBehavior()
        {
            // We need to create the behavior where the enemy will patrol a set of points
            var behavior = new BehaviorTree
            {
                RootNode = new SequenceNode(
                    new List<INode>()
                    {
                        new NavigateNode(new Vector3(5.0f, 5.0f, -1.0f), 0.1f),
                        new NavigateNode(new Vector3(-1.0f, -1.0f, -1.0f), 0.1f)
                    }
                )
            };

            behavior.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);
        }

        public static void SaveTestPlayerBehavior()
        {
            // We need to create the behavior where the enemy will patrol a set of points
            var behavior = new BehaviorTree
            {
                RootNode =
                    new SequenceNode(new List<INode>
                    {
                        new SelectorNode(new List<INode>
                        {
                            new LeafNode()
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
                            },
                            new LeafNode()
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
                            }
                        }),
                        new LeafNode()
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
                        }
                    })
            };

            behavior.Save(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
        }

        /*if (_nEvadeTicks == 0 && inputState.IsPressed(InputMapping.Evade))
        {
            var evadeTranslation = GetTranslation(inputState, EVADE_SPEED);

            if (evadeTranslation != Vector3.Zero)
            {
                _nEvadeTicks++;
                Scale = new Vector3(1.0f, 0.5f, 1.0f);
                _rollDirection = evadeTranslation;
                Rotation = new Quaternion(new Vector3((float)Math.Atan2(_rollDirection.Y, _rollDirection.X), 0.0f, 0.0f));
            }
        }
        else if (_nEvadeTicks > EVADE_TICK_COUNT)
        {
            _nEvadeTicks = 0;
            Scale = Vector3.One;
            _rollDirection = Vector3.Zero;
        }
        else if (_nEvadeTicks > 0)
        {
            _nEvadeTicks++;
        }

        float speed = inputState.IsHeld(InputMapping.Run)
            ? RUN_SPEED
            : inputState.IsHeld(InputMapping.Crawl)
                ? CREEP_SPEED
                : WALK_SPEED;

        var translation = (_nEvadeTicks > 0) ? _rollDirection : GetTranslation(inputState, speed);
        HandleCollisions(translation, colliders);
        

         
        var translation = new Vector3();

        if (inputState.IsHeld(InputMapping.Forward))
        {
            translation.Y += speed;
        }

        if (inputState.IsHeld(InputMapping.Left))
        {
            translation.X -= speed;
        }

        if (inputState.IsHeld(InputMapping.Backward))
        {
            translation.Y -= speed;
        }

        if (inputState.IsHeld(InputMapping.Right))
        {
            translation.X += speed;
        }

        if (inputState.IsHeld(InputMapping.In))
        {
            translation.Z += speed;
        }

        if (inputState.IsHeld(InputMapping.Out))
        {
            translation.Z -= speed;
        }

        return translation;*/
    }
}
