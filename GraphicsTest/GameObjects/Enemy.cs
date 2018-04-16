using GraphicsTest.Behaviors.Enemy;
using GraphicsTest.Helpers;
using OpenTK;
using TakoEngine.Maps;

namespace GraphicsTest.GameObjects
{
    public class Enemy : MapActor
    {
        public const string NAME = "Enemy";

        public Enemy()
        {
            Name = NAME;
            Position = new Vector3(5.0f, 5.0f, -1.5f);
            Scale = 0.04f * Vector3.One;
            Rotation = Vector3.Zero;
            Orientation = new Vector3(90.0f, 0.0f, 0.0f);
            ModelFilePath = FilePathHelper.BOB_LAMP_MESH_PATH;

            /*TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_02_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_02_N_NORMAL_PATH
            });

            TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_02_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_02_N_NORMAL_PATH
            });

            TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_02_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_02_N_NORMAL_PATH
            });

            TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_02_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_02_N_NORMAL_PATH
            });

            TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_02_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_02_N_NORMAL_PATH
            });

            TexturesPaths.Add(new TexturePaths()
            {
                DiffuseMapFilePath = FilePathHelper.BRICK_02_D_TEXTURE_PATH,
                NormalMapFilePath = FilePathHelper.BRICK_02_N_NORMAL_PATH
            });*/

            BehaviorFilePath = FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH;
            HasCollision = true;

            SaveBehaviorTree();
        }

        private void SaveBehaviorTree()
        {
            var behavior = new EnemyBehavior();
            behavior.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);

            /*var rootNode = new SelectorNode(
                new RepeaterNode(
                    new IsPlayerInSightNode(VIEW_ANGLE, VIEW_DISTANCE,
                        new IsAtFullAlert(FULL_ALERT_TICKS,
                            // TODO - Chase Player
                            new InlineLeafNode(c => BehaviorStatuses.Success)
                        )
                    )
                ),
                new SequenceNode(
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, 5.0f, -1.0f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(5.0f, -5.0f, -1.0f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, -5.0f, -1.0f), WALK_SPEED),
                        new TurnTowardsNode()
                    ),
                    new ParallelNode(
                        new MoveToNode(new Vector3(-5.0f, 5.0f, -1.0f), WALK_SPEED),
                        new TurnTowardsNode()
                    )
                )
            );

            rootNode.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);*/
        }
    }
}
