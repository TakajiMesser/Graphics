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

            Stimuli.Add(TakoEngine.Scripting.StimResponse.Stimulus.Player);

            SaveBehaviorTree();
        }

        private void SaveBehaviorTree()
        {
            var behavior = new PlayerBehavior();
            behavior.Save(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
        }
    }
}
