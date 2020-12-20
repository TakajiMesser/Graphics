using SampleGameProject.Helpers;
using SpiceEngine.Maps;
using SpiceEngineCore.Maps;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SampleGameProject.GameObjects
{
    public class Enemy : MapActor
    {
        public const string NAME = "Enemy";

        public Enemy()
        {
            Name = NAME;
            Position = new Vector3(5.0f, 5.0f, 10.5f);
            Offset = new Vector3();
            Scale = 0.04f * Vector3.One;
            Rotation = Vector3.Zero;
            Orientation = new Vector3(0.0f, 0.0f, 90.0f);
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

            //BehaviorFilePath = FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH;
            Behavior = MapBehavior.Load(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);
            IsPhysical = true;

            //SaveBehaviorTree();
        }

        private void SaveBehaviorTree()
        {
            //var behavior = new EnemyBehavior();
            //behavior.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);

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
