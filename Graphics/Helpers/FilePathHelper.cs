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

        #region Meshes
        public static string SQUARE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Square.obj";
        public static string CUBE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Cube.obj";
        public static string SPHERE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Sphere.obj";
        public static string CONE_MESH_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\Cone.obj";
        #endregion

        #region Materials
        public static string GENERIC_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\GenericMaterial.mtl";
        public static string SHINY_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Meshes\ShinyMaterial.mtl";
        #endregion

        #region Shaders
        public static string FORWARD_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\ForwardRendering\forward-vertex-shader.glsl";
        public static string FORWARD_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\ForwardRendering\forward-fragment-shader.glsl";

        public static string SIMPLE_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Simple\simple-vertex-shader.glsl";
        public static string SIMPLE_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Simple\simple-fragment-shader.glsl";

        public static string WIREFRAME_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Wireframe\wireframe-vertex.glsl";
        public static string WIREFRAME_SKINNING_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Wireframe\wireframe-skinning-vertex.glsl";
        public static string WIREFRAME_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Wireframe\wireframe-geometry.glsl";
        public static string WIREFRAME_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Wireframe\wireframe-fragment.glsl";

        public static string GEOMETRY_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-vertex-shader.glsl";
        public static string GEOMETRY_SKINNING_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-skinning-vertex-shader.glsl";
        public static string GEOMETRY_TESS_CONTROL_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-tess-control-shader.glsl";
        public static string GEOMETRY_TESS_EVAL_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-tess-eval-shader.glsl";
        public static string GEOMETRY_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-shader.glsl";
        public static string GEOMETRY_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Geometry\geometry-fragment-shader.glsl";

        public static string STENCIL_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\stencil-vertex-shader.glsl";
        public static string LIGHT_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\light-vertex-shader.glsl";
        public static string POINT_LIGHT_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\point-light-fragment-shader.glsl";
        public static string SPOT_LIGHT_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Lighting\spot-light-fragment-shader.glsl";

        public static string POINT_SHADOW_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Shadows\point-shadow-vertex-shader.glsl";
        public static string POINT_SHADOW_SKINNING_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Shadows\point-shadow-skinning-vertex-shader.glsl";
        public static string POINT_SHADOW_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Shadows\point-shadow-geometry-shader.glsl";
        public static string POINT_SHADOW_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Shadows\point-shadow-fragment-shader.glsl";
        public static string SPOT_SHADOW_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Shadows\spot-shadow-vertex-shader.glsl";
        public static string SPOT_SHADOW_SKINNING_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Shadows\spot-shadow-skinning-vertex-shader.glsl";
        public static string SPOT_SHADOW_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Shadows\spot-shadow-fragment-shader.glsl";

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

        public static string TEXT_VERTEX_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Text\text_vertex.glsl";
        public static string TEXT_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Text\text_fragment.glsl";

        public static string SKYBOX_VERTEX_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Skybox\skybox_vert.glsl";
        public static string SKYBOX_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\Graphics\Rendering\Shaders\Skybox\skybox_frag.glsl";
        #endregion
    }
}
