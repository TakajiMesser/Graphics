using SampleGameProject.Helpers;
using SpiceEngine.Maps;
using SpiceEngineCore.Maps;
using SweetGraphicsCore.Rendering.Textures;
using UmamiScriptingCore.StimResponse;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SampleGameProject.GameObjects
{
    public class Player : MapActor
    {
        public const string NAME = "Player";

        public Player()
        {
            Name = NAME;
            Position = new Vector3(0.0f, 0.0f, 2.0f);
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
            Behavior = MapBehavior.Load(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
            IsPhysical = true;

            Stimuli.Add(Stimulus.Player);
        }
    }
}
