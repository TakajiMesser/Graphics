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

namespace Graphics.Helpers
{
    /// <summary>
    /// For now, this is a helper class for accessing hard-coded paths for game files
    /// </summary>
    public static class FilePathHelper
    {
        public const string PLAYER_MESH_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\Player.obj";
        public const string SQUARE_MESH_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\Square.obj";
        public const string FLOOR_MESH_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\Floor.obj";
        public const string ENEMY_MESH_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\Enemy.obj";

        public const string GENERIC_MATERIAL_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\GenericMaterial.mtl";

        public const string VERTEX_SHADER_PATH = @"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Rendering\Shaders\simple-vertex-shader.glsl";
        public const string FRAGMENT_SHADER_PATH = @"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Rendering\Shaders\simple-fragment-shader.glsl";
        public const string BLUR_SHADER_PATH = @"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Rendering\Shaders\MotionBlur\mb_blur.glsl";
        public const string DILATE_SHADER_PATH = @"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Rendering\Shaders\MotionBlur\mb_dilate.glsl";

        public const string MAP_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Maps\TestMap.map";

        public const string ENEMY_PATROL_BEHAVIOR_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Behaviors\EnemyPatrol.btt";
        public const string PLAYER_INPUT_BEHAVIOR_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Behaviors\PlayerInput.btt";
        public const string PLAYER_MOVEMENT_BEHAVIOR_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Behaviors\PlayerMovement.btt";
        public const string PLAYER_TURN_BEHAVIOR_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Behaviors\PlayerTurn.btt";
        public const string PLAYER_EVADE_BEHAVIOR_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Behaviors\PlayerEvade.btt";
        public const string PLAYER_COVER_BEHAVIOR_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Behaviors\PlayerCover.btt";
    }
}
