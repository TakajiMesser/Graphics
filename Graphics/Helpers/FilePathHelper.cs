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
using System.IO;

namespace Graphics.Helpers
{
    /// <summary>
    /// For now, this is a helper class for accessing hard-coded paths for game files
    /// </summary>
    public static class FilePathHelper
    {
        private static string SOLUTION_DIRECTORY = Directory.GetCurrentDirectory() + @"\..\..\..";//@"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics";

        public static string SCREENSHOT_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Screenshots\";

        #region Maps
        public static string MAP_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Maps\TestMap.map";
        #endregion

        #region Meshes
        public static string PLAYER_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Player.obj";
        public static string SQUARE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Square.obj";
        public static string CUBE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Cube.obj";
        public static string FLOOR_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Floor.obj";
        public static string ENEMY_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Enemy.obj";
        public static string SPHERE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Sphere.obj";
        public static string CONE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Cone.obj";
        #endregion

        #region Materials
        public static string GENERIC_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\GenericMaterial.mtl";
        #endregion

        #region Textures
        public static string BRICK_01_D_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\brick1-d.jpg";
        public static string BRICK_01_H_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\brick1-h.jpg";
        public static string BRICK_01_N_NORMAL_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\brick1-n.jpg";
        public static string BRICK_01_S_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\brick1-s.jpg";

        public static string BRICK_02_D_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\brick2-d.jpg";
        public static string BRICK_02_H_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\brick2-h.jpg";
        public static string BRICK_02_N_NORMAL_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\brick2-n.jpg";
        public static string BRICK_02_S_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\brick2-s.jpg";

        public static string GRASS_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\Grass01.png";
        public static string GRASS_N_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\Grass01_normal.png";

        public static string SPACE_01_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\Skybox\space_right1.png";
        public static string SPACE_02_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\Skybox\space_left2.png";
        public static string SPACE_03_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\Skybox\space_top3.png";
        public static string SPACE_04_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\Skybox\space_bottom4.png";
        public static string SPACE_05_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\Skybox\space_front5.png";
        public static string SPACE_06_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Textures\Skybox\space_back6.png";
        #endregion

        #region Shaders
        public static string FORWARD_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\ForwardRendering\forward-vertex-shader.glsl";
        public static string FORWARD_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\ForwardRendering\forward-fragment-shader.glsl";

        public static string SIMPLE_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Simple\simple-vertex-shader.glsl";
        public static string SIMPLE_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Simple\simple-fragment-shader.glsl";

        public static string GEOMETRY_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-vertex-shader.glsl";
        public static string GEOMETRY_TESS_CONTROL_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-tess-control-shader.glsl";
        public static string GEOMETRY_TESS_EVAL_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-tess-eval-shader.glsl";
        public static string GEOMETRY_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-shader.glsl";
        public static string GEOMETRY_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-fragment-shader.glsl";

        public static string STENCIL_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\stencil-vertex-shader.glsl";
        public static string LIGHT_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\light-vertex-shader.glsl";
        public static string POINT_LIGHT_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\point-light-fragment-shader.glsl";
        public static string SPOT_LIGHT_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\spot-light-fragment-shader.glsl";

        public static string POINT_SHADOW_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\point-shadow-vertex-shader.glsl";
        public static string POINT_SHADOW_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\point-shadow-fragment-shader.glsl";
        public static string SPOT_SHADOW_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\spot-shadow-vertex-shader.glsl";
        public static string SPOT_SHADOW_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\spot-shadow-fragment-shader.glsl";

        public static string MY_BLUR_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\MotionBlur\mb_myBlur.glsl";
        public static string BLUR_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\MotionBlur\mb_blur.glsl";
        public static string DILATE_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\MotionBlur\mb_dilate.glsl";

        public static string INVERT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Miscellaneous\invert.glsl";

        public static string RENDER_1D_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_1D.glsl";
        public static string RENDER_2D_ARRAY_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_2D_array.glsl";
        public static string RENDER_2D_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_2D_fragment.glsl";
        public static string RENDER_2D_VERTEX_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_2D_vertex.glsl";
        public static string RENDER_3D_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_3D.glsl";
        public static string RENDER_CUBE_ARRAY_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_cube_array.glsl";
        public static string RENDER_CUBE_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_cube_fragment.glsl";
        public static string RENDER_CUBE_VERTEX_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\RenderToScreen\render_cube_vertex.glsl";

        public static string SKYBOX_VERTEX_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Skybox\skybox_vert.glsl";
        public static string SKYBOX_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Skybox\skybox_frag.glsl";
        #endregion

        #region Behaviors
        public static string ENEMY_PATROL_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Behaviors\EnemyPatrol.btt";
        public static string ENEMY_SEARCH_PLAYER_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Behaviors\SearchForPlayer.btt";
        public static string ENEMY_TURN_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Behaviors\EnemyTurn.btt";

        public static string PLAYER_INPUT_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Behaviors\PlayerInput.btt";
        public static string PLAYER_MOVEMENT_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Behaviors\PlayerMovement.btt";
        public static string PLAYER_TURN_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Behaviors\PlayerTurn.btt";
        public static string PLAYER_EVADE_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Behaviors\PlayerEvade.btt";
        public static string PLAYER_COVER_BEHAVIOR_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Behaviors\PlayerCover.btt";
        #endregion
    }
}
