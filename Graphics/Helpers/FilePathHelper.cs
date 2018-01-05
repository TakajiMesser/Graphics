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
        private const string SOLUTION_DIRECTORY = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics";

        public const string MAP_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Maps\TestMap.map";

        public const string PLAYER_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Meshes\Player.obj";
        public const string SQUARE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Meshes\Square.obj";
        public const string FLOOR_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Meshes\Floor.obj";
        public const string ENEMY_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Meshes\Enemy.obj";

        public const string GENERIC_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Meshes\GenericMaterial.mtl";

        #region Shaders
        public const string VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\simple-vertex-shader.glsl";
        public const string FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\simple-fragment-shader.glsl";
        public const string BLUR_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\MotionBlur\mb_blur.glsl";
        public const string DILATE_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\MotionBlur\mb_dilate.glsl";

        public const string RENDER_1D_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_1D.glsl";
        public const string RENDER_2D_ARRAY_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_2D_array.glsl";
        public const string RENDER_2D_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_2D_fragment.glsl";
        public const string RENDER_2D_VERTEX_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_2D_vertex.glsl";
        public const string RENDER_3D_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_3D.glsl";
        public const string RENDER_CUBE_ARRAY_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_cube_array.glsl";
        public const string RENDER_CUBE_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_cube_fragment.glsl";
        public const string RENDER_CUBE_VERTEX_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_cube_vertex.glsl";
        #endregion

        #region Behaviors
        public const string ENEMY_PATROL_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Behaviors\EnemyPatrol.btt";
        public const string ENEMY_SEARCH_PLAYER_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Behaviors\SearchForPlayer.btt";
        public const string ENEMY_TURN_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Behaviors\EnemyTurn.btt";

        public const string PLAYER_INPUT_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Behaviors\PlayerInput.btt";
        public const string PLAYER_MOVEMENT_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Behaviors\PlayerMovement.btt";
        public const string PLAYER_TURN_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Behaviors\PlayerTurn.btt";
        public const string PLAYER_EVADE_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Behaviors\PlayerEvade.btt";
        public const string PLAYER_COVER_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Behaviors\PlayerCover.btt";
        #endregion
    }
}
