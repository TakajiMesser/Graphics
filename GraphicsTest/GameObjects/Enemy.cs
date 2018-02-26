using Graphics.Maps;
using Graphics.Scripting.BehaviorTrees;
using Graphics.Scripting.BehaviorTrees.Composites;
using Graphics.Scripting.BehaviorTrees.Decorators;
using Graphics.Scripting.BehaviorTrees.Leaves;
using GraphicsTest.Behaviors;
using GraphicsTest.Behaviors.Enemy;
using GraphicsTest.Helpers;
using OpenTK;
using Graphics.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest.GameObjects
{
    public class Enemy : MapGameObject
    {
        public const string NAME = "Enemy";

        public const float WALK_SPEED = 0.1f;
        public const float VIEW_ANGLE = 1.0472f;
        public const float VIEW_DISTANCE = 5.0f;
        public const int FULL_ALERT_TICKS = 120;

        public Enemy()
        {
            Name = NAME;
            Position = new Vector3(5.0f, 5.0f, -1.0f);
            Scale = 0.04f * Vector3.One;
            Rotation = Quaternion.FromEulerAngles(90.0f, 0.0f, 0.0f);
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
            var rootNode = new SelectorNode(
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

            rootNode.Save(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);
        }
    }
}
