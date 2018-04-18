using Jidai.Behaviors.Player;
using Jidai.Helpers;
using OpenTK;
using TakoEngine.Maps;
using TakoEngine.Rendering.Textures;

namespace Jidai.GameObjects
{
    public class Player : MapActor
    {
        public const string NAME = "Player";

        public Player()
        {
            Name = NAME;
            Position = new Vector3(0.0f, 0.0f, -1.0f);
            Scale = Vector3.One;
            Rotation = Vector3.Zero;
            Orientation = Vector3.Zero;
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
            var behavior = new PlayerBehavior();
            behavior.Save(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);

            /*var rootNode = new SelectorNode(
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

            rootNode.Save(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);*/
        }
    }
}
