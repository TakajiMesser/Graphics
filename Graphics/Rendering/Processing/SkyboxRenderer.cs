using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Meshes;
using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Matrices;
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

namespace Graphics.Rendering.Processing
{
    public class SkyboxRenderer
    {
        public Resolution Resolution { get; private set; }
        public Texture SkyTexture { get; protected set; }
        
        internal ShaderProgram _program;
        private SimpleMesh _cubeMesh;

        public SkyboxRenderer(Resolution resolution)
        {
            Resolution = resolution;
        }

        public void Load()
        {
            LoadPrograms();
            LoadBuffers();
            LoadCubeMesh();
        }

        protected void LoadPrograms()
        {
            _program = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SKYBOX_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SKYBOX_FRAGMENT_PATH)));
        }

        public void LoadBuffers()
        {
            var texturePaths = new List<string>
            {
                FilePathHelper.SPACE_01_TEXTURE_PATH,
                FilePathHelper.SPACE_02_TEXTURE_PATH,
                FilePathHelper.SPACE_03_TEXTURE_PATH,
                FilePathHelper.SPACE_04_TEXTURE_PATH,
                FilePathHelper.SPACE_05_TEXTURE_PATH,
                FilePathHelper.SPACE_06_TEXTURE_PATH,
            };

            SkyTexture = Texture.LoadFromFile(texturePaths, TextureTarget.TextureCubeMap, true, true);
        }

        private void LoadCubeMesh()
        {
            _cubeMesh = SimpleMesh.LoadFromFile(FilePathHelper.CUBE_MESH_PATH, _program);
        }

        public void Render(Camera camera, FrameBuffer gBuffer)
        {
            _program.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, gBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);

            //GL.ClearColor(Color4.Purple);
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            int oldCullFaceMode = GL.GetInteger(GetPName.CullFaceMode);
            int oldDepthFunc = GL.GetInteger(GetPName.DepthFunc);

            //GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Lequal);

            _program.BindTexture(SkyTexture, "mainTexture", 0);

            camera.Draw(_program);
            _program.SetUniform(ModelMatrix.NAME, Matrix4.CreateTranslation(camera.Position));
            _cubeMesh.Draw();

            GL.CullFace((CullFaceMode)oldCullFaceMode);
            GL.DepthFunc((DepthFunction)oldDepthFunc);
        }

        public void Render(Camera camera, FrameBuffer gBuffer, Texture cubeMap)
        {
            _program.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, gBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);

            //GL.ClearColor(Color4.Purple);
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            int oldCullFaceMode = GL.GetInteger(GetPName.CullFaceMode);
            int oldDepthFunc = GL.GetInteger(GetPName.DepthFunc);

            //GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Lequal);

            _program.BindTexture(cubeMap, "mainTexture", 0);

            camera.Draw(_program);
            _program.SetUniform(ModelMatrix.NAME, Matrix4.CreateTranslation(camera.Position));
            _cubeMesh.Draw();

            GL.CullFace((CullFaceMode)oldCullFaceMode);
            GL.DepthFunc((DepthFunction)oldDepthFunc);
        }
    }
}
