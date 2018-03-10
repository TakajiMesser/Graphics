using TakoEngine.Maps;
using TakoEngine.Rendering.Textures;
using TakoEngine.Scripting.BehaviorTrees;
using TakoEngine.Scripting.BehaviorTrees.Composites;
using TakoEngine.Scripting.BehaviorTrees.Decorators;
using TakoEngine.Scripting.BehaviorTrees.Leaves;
using GraphicsTest.Behaviors.Player;
using GraphicsTest.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest.GameObjects
{
    public class Player : MapGameObject
    {
        public const string NAME = "Player";

        public const float WALK_SPEED = 0.1f;
        public const float RUN_SPEED = 0.15f;
        public const float CREEP_SPEED = 0.04f;
        public const float EVADE_SPEED = 0.175f;
        public const float COVER_SPEED = 0.1f;
        public const float ENTER_COVER_SPEED = 0.12f;
        public const float COVER_DISTANCE = 5.0f;
        public const int EVADE_TICK_COUNT = 20;

        public Player()
        {
            Name = NAME;
            Position = new Vector3(0.0f, 0.0f, -1.0f);
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;
            ModelFilePath = FilePathHelper.PLAYER_MESH_PATH;

            TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH,
                SpecularMapFilePath = FilePathHelper.BRICK_01_S_TEXTURE_PATH
            });

            TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH,
                SpecularMapFilePath = FilePathHelper.BRICK_01_S_TEXTURE_PATH
            });

            //ParallaxMapFilePath = FilePathHelper.BRICK_01_H_TEXTURE_PATH,
            BehaviorFilePath = FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH;
            HasCollision = true;

            SaveBehaviorTree();
        }

        private void SaveBehaviorTree()
        {
            var rootNode = new SelectorNode(
                new SelectorNode(
                    new SelectorNode(
                        new EvadeNode(EVADE_SPEED, EVADE_TICK_COUNT),
                        new SelectorNode(
                            new InlineConditionNode(c => c.InputState.IsPressed(c.InputMapping.Cover),
                                new TakeCoverNode(COVER_SPEED, ENTER_COVER_SPEED, COVER_DISTANCE)
                            ),
                            new InlineConditionNode(c => c.InputState.IsHeld(c.InputMapping.Cover),
                                new CoverNode(COVER_SPEED, ENTER_COVER_SPEED, COVER_DISTANCE)
                            ),
                            new InlineConditionNode(c => c.InputState.IsReleased(c.InputMapping.Cover),
                                new InlineLeafNode(c =>
                                {
                                    c.RemoveVariableIfExists("coverDirection");
                                    c.RemoveVariableIfExists("coverDistance");

                                    return BehaviorStatuses.Success;
                                })
                            )
                        )
                    ),
                    new SequenceNode(
                        new MoveNode(RUN_SPEED, CREEP_SPEED, WALK_SPEED),
                        new TurnNode()
                    )
                )
            );

            rootNode.Save(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
        }
    }
}
