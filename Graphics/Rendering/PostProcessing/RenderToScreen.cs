﻿using Graphics.Helpers;
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
    public class RenderToScreen : PostProcess
    {
        public const string NAME = "RenderToScreen";

        private ShaderProgram _render1DProgram;
        private ShaderProgram _render2DProgram;
        private ShaderProgram _render2DArrayProgram;
        private ShaderProgram _render3DProgram;
        private ShaderProgram _renderCubeProgram;
        private ShaderProgram _renderCubeArrayProgram;

        public RenderToScreen(Resolution resolution) : base(NAME, resolution) { }

        protected override void LoadProgram()
        {
            _render1DProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_1D_PATH)));
            _render2DProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_2D_VERTEX_PATH)), 
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_2D_FRAGMENT_PATH)));
            _render2DArrayProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_2D_ARRAY_PATH)));
            _render3DProgram = new ShaderProgram(new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_3D_PATH)));
            _renderCubeProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_FRAGMENT_PATH)));
            _renderCubeArrayProgram = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.RENDER_CUBE_ARRAY_PATH)));
        }

        protected override void LoadBuffers()
        {
            LoadQuad(_render2DProgram);
        }

        public void Render()
        {
            RenderQuad();
        }

        public override void ResizeTextures() { }

        public void Render(Texture texture, int channel = -1)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, texture.Width, texture.Height);

            switch (texture.Target)
            {
                case TextureTarget.Texture1D:
                    _render1DProgram.Use();
                    _render1DProgram.BindTexture(texture, "textureSampler", 0);
                    break;
                case TextureTarget.Texture2D:
                    _render2DProgram.Use();
                    _render2DProgram.BindTexture(texture, "textureSampler", 0);

                    int channelLocation = _render2DProgram.GetUniformLocation("channel");
                    GL.Uniform1(channelLocation, channel);
                    break;
                case TextureTarget.Texture3D:
                    break;
                case TextureTarget.Texture2DArray:
                    break;
                case TextureTarget.TextureCubeMapArray:
                    break;
                default:
                    throw new NotImplementedException("Cannot render texture target type " + texture.Target);
            }

            Render();
        }
    }
}