using Graphics.Helpers;
using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using Graphics.Rendering.Vertices;
using Graphics.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.PostProcessing
{
    public class InvertColors : PostProcess
    {
        public const string NAME = "InvertColors";

        private ShaderProgram _invertProgram;

        public InvertColors(Resolution resolution) : base(NAME, resolution) { }

        protected override void LoadProgram()
        {
            _invertProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_2D_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.INVERT_SHADER_PATH)));
        }

        protected override void LoadBuffers()
        {
            LoadQuad(_invertProgram);
            LoadFinalTexture();

            _frameBuffer.Add(FramebufferAttachment.ColorAttachment0, FinalTexture);
            _frameBuffer.Bind(FramebufferTarget.Framebuffer);
            _frameBuffer.AttachAttachments();
            _frameBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        public void Render(Texture texture)
        {
            _invertProgram.Use();
            _frameBuffer.Draw();

            GL.ClearColor(Color4.Purple);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _invertProgram.BindTexture(texture, "sceneTexture", 0);

            RenderQuad();
        }
    }
}
