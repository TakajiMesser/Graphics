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
        private int _vertexArrayHandle;
        private VertexBuffer<Vector3> _vertexBuffer = new VertexBuffer<Vector3>();
        private VertexIndexBuffer _indexBuffer = new VertexIndexBuffer();

        public SkyboxRenderer(Resolution resolution)
        {
            Resolution = resolution;
        }

        public void Load()
        {
            LoadPrograms();
            LoadBuffers();
        }

        protected void LoadPrograms()
        {
            _program = new ShaderProgram(new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SKYBOX_VERTEX_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SKYBOX_FRAGMENT_PATH)));
        }

        public void LoadBuffers()
        {
            LoadCube(_program);

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

        protected void LoadCube(ShaderProgram program)
        {
            _vertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.AddVertices(new[]
            {
                new Vector3(-20.0f, -20.0f, -20.0f),
                new Vector3(-20.0f, -20.0f, 20.0f),
                new Vector3(-20.0f, 20.0f, -20.0f),
                new Vector3(-20.0f, 20.0f, 20.0f),
                new Vector3(20.0f, -20.0f, -20.0f),
                new Vector3(20.0f, -20.0f, 20.0f),
                new Vector3(20.0f, 20.0f, -20.0f),
                new Vector3(20.0f, 20.0f, 20.0f)
            });
            _vertexBuffer.Bind();
            _vertexBuffer.Buffer();

            var attribute = new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, UnitConversions.SizeOf<Vector3>(), 0);
            attribute.Set(program.GetAttributeLocation("vPosition"));

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();

            _indexBuffer.AddIndices(new List<ushort>
            {
                0, 6, 4,
                0, 2, 6,
                0, 3, 2,
                0, 1, 3,
                2, 7, 6,
                2, 3, 7,
                4, 6, 7,
                4, 7, 5,
                0, 4, 5,
                0, 5, 1,
                1, 5, 7,
                1, 7, 3
            });
            _indexBuffer.Bind();
            _indexBuffer.Buffer();
            _indexBuffer.Unbind();
        }

        public void Render(Camera camera, FrameBuffer gBuffer)
        {
            _program.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, gBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);

            //GL.ClearColor(Color4.Purple);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            int oldCullFaceMode = GL.GetInteger(GetPName.CullFaceMode);
            int oldDepthFunc = GL.GetInteger(GetPName.DepthFunc);

            //GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.CullFace(CullFaceMode.Front);
            GL.DepthFunc(DepthFunction.Lequal);

            _program.BindTexture(SkyTexture, "mainTexture", 0);

            camera.Draw(_program);

            var modelMatrix = Matrix4.CreateTranslation(camera.Position);
            //var modelMatrix = Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, -0.5f));
            _program.SetUniform(ModelMatrix.NAME, modelMatrix);

            RenderCube();

            GL.CullFace((CullFaceMode)oldCullFaceMode);
            GL.DepthFunc((DepthFunction)oldDepthFunc);
        }

        protected void RenderCube()
        {
            GL.BindVertexArray(_vertexArrayHandle);
            _vertexBuffer.Bind();
            _indexBuffer.Bind();

            _indexBuffer.Draw();

            GL.BindVertexArray(0);
            _vertexBuffer.Unbind();
            _indexBuffer.Unbind();
        }
    }
}
