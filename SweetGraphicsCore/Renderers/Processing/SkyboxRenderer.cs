using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Renderers.Shaders;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using Matrix4 = SpiceEngineCore.Geometry.Matrices.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternions.Quaternion;
using Vector3 = SpiceEngineCore.Geometry.Vectors.Vector3;

namespace SweetGraphicsCore.Renderers.Processing
{
    public class SkyboxRenderer : Renderer
    {
        private ShaderProgram _program;
        private ShaderProgram _2DProgram;

        private SimpleMesh _cubeMesh;
        private SimpleMesh _squareMesh;
        private List<string> _texturePaths = new List<string>();

        public SkyboxRenderer(ITextureProvider textureProvider) : base(textureProvider) { }

        public Texture SkyTexture2D { get; protected set; }
        public Texture SkyTexture { get; protected set; }

        public void SetTextures(IList<string> texturePaths)
        {
            _texturePaths.Clear();
            _texturePaths.AddRange(texturePaths);
        }

        protected override void LoadShaders()
        {
            _program = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.skybox_vert),
                new Shader(ShaderType.FragmentShader, Resources.skybox_frag)
            );

            _2DProgram = new ShaderProgram(
                new Shader(ShaderType.VertexShader, Resources.skybox2D_vert),
                new Shader(ShaderType.FragmentShader, Resources.skybox2D_frag)
            );
        }

        protected override void LoadTextures(Resolution resolution)
        {
            if (_texturePaths.Any())
            {
                SkyTexture = TextureHelper.LoadFromFile(_texturePaths, TextureTarget.TextureCubeMap, true, true);
                SkyTexture2D = TextureHelper.LoadFromFile(_texturePaths.First(), true, true);
            }
        }

        public override void Resize(Resolution resolution)
        {
            
        }

        protected override void LoadBuffers()
        {
            _cubeMesh = SimpleMesh.LoadFromFile(FilePathHelper.CUBE_MESH_PATH, _program);
            _squareMesh = SimpleMesh.LoadFromFile(FilePathHelper.SQUARE_MESH_PATH, _2DProgram);
        }

        public void Render(ICamera camera)
        {
            switch (camera)
            {
                case PerspectiveCamera p:
                    Render(p);
                    break;
                case OrthographicCamera o:
                    Render(o);
                    break;
            }
        }

        private void Render(PerspectiveCamera camera)
        {
            if (SkyTexture != null)
            {
                _program.Use();

                int oldCullFaceMode = GL.GetInteger(GetPName.CullFaceMode);
                int oldDepthFunc = GL.GetInteger(GetPName.DepthFunc);

                GL.Enable(EnableCap.DepthTest);
                GL.CullFace(CullFaceMode.Front);
                GL.DepthFunc(DepthFunction.Lequal);

                _program.BindTexture(SkyTexture, "mainTexture", 0);

                _program.SetCamera(camera);
                _program.SetUniform(ModelMatrix.CURRENT_NAME, Matrix4.FromTranslation(camera.Position));
                _cubeMesh.Draw();

                GL.CullFace((CullFaceMode)oldCullFaceMode);
                GL.DepthFunc((DepthFunction)oldDepthFunc);
            }
        }

        private void Render(OrthographicCamera camera)
        {
            if (SkyTexture2D != null)
            {
                _2DProgram.Use();

                GL.Enable(EnableCap.DepthTest);
                //GL.CullFace(CullFaceMode.Front);
                GL.DepthFunc(DepthFunction.Lequal);

                _2DProgram.BindTexture(SkyTexture2D, "mainTexture", 0);

                //camera.Draw(_2DProgram);
                _2DProgram.SetUniform(ViewMatrix.CURRENT_NAME, camera.ViewMatrix);
                //camera._projectionMatrix.Set(_2DProgram);
                //var width = camera._projectionMatrix.Width;
                /*var width = 0.8f;
                var height = width / camera._projectionMatrix.Resolution.AspectRatio;
                var projection = Matrix4.CreateOrthographic(width, height, camera._projectionMatrix.ZNear, camera._projectionMatrix.ZFar);*/
                var projection = camera.CalculateProjection();
                _2DProgram.SetUniform(ProjectionMatrix.CURRENT_NAME, projection);

                var originalDirection = -Vector3.UnitZ;
                var direction = (camera.LookAt - camera.Position).Normalized();

                var cosTheta = Vector3.Dot(originalDirection, direction);
                var rotationAxis = Vector3.Cross(originalDirection, direction);
                var rotation = (rotationAxis == Vector3.Zero)
                    ? Quaternion.Identity
                    : new Quaternion(rotationAxis, UnitConversions.ToDegrees((float)Math.Acos(cosTheta)));

                _2DProgram.SetUniform(ModelMatrix.CURRENT_NAME, Matrix4.FromQuaternion(rotation) * Matrix4.FromTranslation(camera.Position));
                _squareMesh.Draw();
            }
        }
    }
}
