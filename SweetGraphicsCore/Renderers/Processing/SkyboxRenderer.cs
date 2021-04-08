using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Helpers;
using SweetGraphicsCore.Properties;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetGraphicsCore.Renderers.Processing
{
    public class SkyboxRenderer : Renderer
    {
        public Texture SkyTexture2D { get; protected set; }
        public Texture SkyTexture { get; protected set; }

        private ShaderProgram _program;
        private ShaderProgram _2DProgram;

        private SimpleMesh _cubeMesh;
        private SimpleMesh _squareMesh;
        private List<string> _texturePaths = new List<string>();

        public void SetTextures(IList<string> texturePaths)
        {
            _texturePaths.Clear();
            _texturePaths.AddRange(texturePaths);
        }

        protected override void LoadPrograms(IRenderContext renderContext)
        {
            _program = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.skybox_vert, Resources.skybox_frag });

            _2DProgram = ShaderHelper.LoadProgram(renderContext,
                new[] { ShaderType.VertexShader, ShaderType.FragmentShader },
                new[] { Resources.skybox2D_vert, Resources.skybox2D_frag });
        }

        protected override void LoadTextures(IRenderContext renderContext, Resolution resolution)
        {
            if (_texturePaths.Any())
            {
                SkyTexture = TextureHelper.LoadFromFile(renderContext, _texturePaths, TextureTarget.TextureCubeMap, true, true);
                SkyTexture2D = TextureHelper.LoadFromFile(renderContext, _texturePaths.First(), true, true);
            }
        }

        protected override void LoadBuffers(IRenderContext renderContext)
        {
            _cubeMesh = SimpleMesh.LoadFromFile(renderContext, FilePathHelper.CUBE_MESH_PATH, _program);
            _squareMesh = SimpleMesh.LoadFromFile(renderContext, FilePathHelper.SQUARE_MESH_PATH, _2DProgram);
        }

        protected override void Resize(Resolution resolution) { }

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
                _program.Bind();

                int oldCullFaceMode = GL.GetIntegerv(GetPName.CullFaceMode)[0];
                int oldDepthFunc = GL.GetIntegerv(GetPName.DepthFunc)[0];

                GL.Enable(EnableCap.DepthTest);
                GL.CullFace(CullFaceMode.Front);
                GL.DepthFunc(DepthFunction.Lequal);

                _program.BindTexture(SkyTexture, "mainTexture", 0);

                _program.SetCamera(camera);
                _program.SetUniform(ModelMatrix.CURRENT_NAME, Matrix4.CreateTranslation(camera.Position));
                _cubeMesh.Draw();

                GL.CullFace((CullFaceMode)oldCullFaceMode);
                GL.DepthFunc((DepthFunction)oldDepthFunc);
            }
        }

        private void Render(OrthographicCamera camera)
        {
            if (SkyTexture2D != null)
            {
                _2DProgram.Bind();

                GL.Enable(EnableCap.DepthTest);
                //GL.CullFace(CullFaceMode.Front);
                GL.DepthFunc(DepthFunction.Lequal);

                _2DProgram.BindTexture(SkyTexture2D, "mainTexture", 0);

                //camera.Draw(_2DProgram);
                //camera._viewMatrix.Set(_2DProgram);
                //camera._projectionMatrix.Set(_2DProgram);
                //var width = camera._projectionMatrix.Width;
                /*var width = 0.8f;
                var height = width / camera._projectionMatrix.Resolution.AspectRatio;
                var projection = Matrix4.CreateOrthographic(width, height, camera._projectionMatrix.ZNear, camera._projectionMatrix.ZFar);*/
                var projection = camera.CalculateProjection();
                //_2DProgram.SetUniform(ProjectionMatrix.CURRENT_NAME, projection);
                _2DProgram.SetCamera(camera);

                var originalDirection = -Vector3.UnitZ;
                var direction = (camera.LookAt - camera.Position).Normalized();

                var cosTheta = Vector3.Dot(originalDirection, direction);
                var rotationAxis = Vector3.Cross(originalDirection, direction);
                var rotation = (rotationAxis == Vector3.Zero)
                    ? Quaternion.Identity
                    : Quaternion.FromAxisAngle(rotationAxis, UnitConversions.ToDegrees((float)Math.Acos(cosTheta)));

                _2DProgram.SetUniform(ModelMatrix.CURRENT_NAME, Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(camera.Position));
                _squareMesh.Draw();
            }
        }
    }
}
