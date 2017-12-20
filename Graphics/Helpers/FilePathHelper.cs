using Graphics.Meshes;
using Graphics.Utilities;
using Graphics.Vertices;
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
        public const string CUBE_MESH_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\Cube.obj";
        public const string TRIANGLE_MESH_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\Triangle.obj";
        public const string VERTEX_SHADER_PATH = @"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Rendering\Shaders\simple-vertex-shader.glsl";
        public const string FRAGMENT_SHADER_PATH = @"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Rendering\Shaders\simple-fragment-shader.glsl";
        public const string MAP_PATH = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Maps\TestMap.map";
    }
}
