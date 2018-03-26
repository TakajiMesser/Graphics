using System.IO;

namespace TakoEngine.Helpers
{
    /// <summary>
    /// For now, this is a helper class for accessing hard-coded paths for game files
    /// </summary>
    public static class FilePathHelper
    {
        private static string SOLUTION_DIRECTORY = Directory.GetCurrentDirectory() + @"\..\..\..";//@"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\TakoEngine";

        public static string SCREENSHOT_PATH = SOLUTION_DIRECTORY + @"\GraphicsTest\Resources\Screenshots\";

        #region Textures
        public static string POINT_LIGHT_BILLBOARD_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Resources\Textures\point-light.png";
        public static string SPOT_LIGHT_BILLBOARD_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Resources\Textures\spot-light.png";
        public static string DIRECTIONAL_LIGHT_BILLBOARD_TEXTURE_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Resources\Textures\directional-light.png";
        #endregion

        #region Meshes
        public static string SQUARE_MESH_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Resources\Meshes\Square.obj";
        public static string CUBE_MESH_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Resources\Meshes\Cube.obj";
        public static string SPHERE_MESH_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Resources\Meshes\Sphere.obj";
        public static string CONE_MESH_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Resources\Meshes\Cone.obj";
        #endregion

        #region Materials
        public static string GENERIC_MATERIAL_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Resources\Meshes\GenericMaterial.mtl";
        #endregion

        #region Shaders
        public static string BILLBOARD_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Billboard\billboard.vert";
        public static string BILLBOARD_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Billboard\billboard.geom";
        public static string BILLBOARD_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Billboard\billboard.frag";
        public static string BILLBOARD_SELECTION_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Billboard\billboard-selection.vert";
        public static string BILLBOARD_SELECTION_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Billboard\billboard-selection.geom";
        public static string BILLBOARD_SELECTION_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Billboard\billboard-selection.frag";

        public static string DEFERRED_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Deferred\deferred.vert";
        public static string DEFERRED_SKINNING_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Deferred\deferred-skinning.vert";
        public static string DEFERRED_TESS_CONTROL_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Deferred\deferred.tesc";
        public static string DEFERRED_TESS_EVAL_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Deferred\deferred.tese";
        public static string DEFERRED_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Deferred\deferred.geom";
        public static string DEFERRED_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Deferred\deferred.frag";

        public static string FORWARD_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Forward\forward.vert";
        public static string FORWARD_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Forward\forward.frag";

        public static string FXAA_VERTEX_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\FXAA\fxaa.vert";
        public static string FXAA_SKINNING_VERTEX_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\FXAA\fxaa-skinning.vert";
        public static string FXAA_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\FXAA\fxaa.frag";

        public static string STENCIL_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Lighting\stencil.vert";
        public static string LIGHT_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Lighting\light.vert";
        public static string POINT_LIGHT_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Lighting\point-light.frag";
        public static string SPOT_LIGHT_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Lighting\spot-light.frag";

        public static string INVERT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Miscellaneous\invert.frag";

        public static string MY_BLUR_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\MotionBlur\myBlur.frag";
        public static string BLUR_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\MotionBlur\blur.frag";
        public static string DILATE_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\MotionBlur\dilate.frag";

        public static string RENDER_1D_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\RenderToScreen\render-1D.frag";
        public static string RENDER_2D_ARRAY_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\RenderToScreen\render-2D-array.frag";
        public static string RENDER_2D_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\RenderToScreen\render-2D.frag";
        public static string RENDER_2D_VERTEX_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\RenderToScreen\render-2D.vert";
        public static string RENDER_3D_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\RenderToScreen\render-3D.frag";
        public static string RENDER_CUBE_ARRAY_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\RenderToScreen\render-cube-array.frag";
        public static string RENDER_CUBE_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\RenderToScreen\render-cube.frag";
        public static string RENDER_CUBE_VERTEX_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\RenderToScreen\render-cube.vert";

        public static string SELECTION_VERTEX_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Selection\selection.vert";
        public static string SELECTION_SKINNING_VERTEX_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Selection\selection-skinning.vert";
        public static string SELECTION_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Selection\selection.frag";

        public static string POINT_SHADOW_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Shadows\point-shadow.vert";
        public static string POINT_SHADOW_SKINNING_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Shadows\point-shadow-skinning.vert";
        public static string POINT_SHADOW_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Shadows\point-shadow.geom";
        public static string POINT_SHADOW_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Shadows\point-shadow.frag";
        public static string SPOT_SHADOW_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Shadows\spot-shadow.vert";
        public static string SPOT_SHADOW_SKINNING_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Shadows\spot-shadow-skinning.vert";
        public static string SPOT_SHADOW_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Shadows\spot-shadow.frag";

        public static string SIMPLE_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Simple\simple.vert";
        public static string SIMPLE_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Simple\simple.frag";

        public static string SKYBOX_VERTEX_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Skybox\skybox.vert";
        public static string SKYBOX_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Skybox\skybox.frag";

        public static string TEXT_VERTEX_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Text\text.vert";
        public static string TEXT_FRAGMENT_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Text\text.frag";

        public static string WIREFRAME_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Wireframe\wireframe.vert";
        public static string WIREFRAME_SKINNING_VERTEX_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Wireframe\wireframe-skinning.vert";
        public static string WIREFRAME_GEOMETRY_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Wireframe\wireframe.geom";
        public static string WIREFRAME_FRAGMENT_SHADER_PATH = SOLUTION_DIRECTORY + @"\TakoEngine\Rendering\Shaders\Wireframe\wireframe.frag";
        
        #endregion
    }
}
