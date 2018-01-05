using Graphics.Helpers;
using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.PostProcessing
{
    public class RenderToScreen : PostProcess
    {
        public const string NAME = "RenderToScreen";

        private ShaderProgram _render1DProgram;
        private ShaderProgram _render2DProgram;
        private ShaderProgram _render2DArrayProgram;
        private ShaderProgram _render3DProgram;
        private ShaderProgram _renderCubeProgram;
        private ShaderProgram _renderCubeArrayProgram;

        private Texture _velocityTextureA;
        private int _vertexArrayHandle;

        public RenderToScreen(Resolution resolution) : base(NAME, resolution) { }

        protected override void LoadProgram()
        {
            _render1DProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_1D_PATH)));
            _render2DProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_2D_FRAGMENT_PATH)));
            _render2DArrayProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_2D_ARRAY_PATH)));
            _render3DProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_3D_PATH)));
            _renderCubeProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_FRAGMENT_PATH)));
            _renderCubeArrayProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_ARRAY_PATH)));
        }

        protected override void LoadBuffers()
        {
            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);
        }

        public void Render()
        {
            GL.BindVertexArray(_vertexArrayHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.BindVertexArray(0);
        }
    }
}
