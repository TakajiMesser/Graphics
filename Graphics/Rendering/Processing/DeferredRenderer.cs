using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Lighting;
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
    public class DeferredRenderer
    {
        public Resolution Resolution { get; private set; }

        public Texture PositionTexture { get; protected set; }
        public Texture ColorTexture { get; protected set; }
        public Texture NormalTexture { get; protected set; }
        public Texture DiffuseMaterialTexture { get; protected set; }
        public Texture SpecularTexture { get; protected set; }
        public Texture VelocityTexture { get; protected set; }
        public Texture FinalTexture { get; protected set; }
        public Texture DepthStencilTexture { get; protected set; }

        public FrameBuffer GBuffer { get; private set; } = new FrameBuffer();

        internal ShaderProgram _geometryProgram;
        internal ShaderProgram _stencilProgram;
        internal ShaderProgram _pointLightProgram;
        internal ShaderProgram _spotLightProgram;
        internal ShaderProgram _simpleProgram;

        private SimpleMesh _pointLightMesh;
        private SimpleMesh _spotLightMesh;

        public DeferredRenderer(Resolution resolution)
        {
            Resolution = resolution;
        }

        public void Load()
        {
            LoadPrograms();
            LoadBuffers();
            LoadPointLightMesh();
            LoadSpotLightMesh();
        }

        protected void LoadPrograms()
        {
            _geometryProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.GEOMETRY_VERTEX_SHADER_PATH)),
                //new Shader(ShaderType.TessControlShader, File.ReadAllText(FilePathHelper.GEOMETRY_TESS_CONTROL_SHADER_PATH)),
                //new Shader(ShaderType.TessEvaluationShader, File.ReadAllText(FilePathHelper.GEOMETRY_TESS_EVAL_SHADER_PATH)),
                //new Shader(ShaderType.GeometryShader, File.ReadAllText(FilePathHelper.GEOMETRY_GEOMETRY_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.GEOMETRY_FRAGMENT_SHADER_PATH))
            });

            _stencilProgram = new ShaderProgram(new[]
            {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.STENCIL_VERTEX_SHADER_PATH))
            });

            _pointLightProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.POINT_LIGHT_FRAGMENT_SHADER_PATH))
            });

            _spotLightProgram = new ShaderProgram(new[] {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.LIGHT_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SPOT_LIGHT_FRAGMENT_SHADER_PATH))
            });

            _simpleProgram = new ShaderProgram(new[]
            {
                new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.SIMPLE_VERTEX_SHADER_PATH)),
                new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.SIMPLE_FRAGMENT_SHADER_PATH))
            });
        }

        public void ResizeTextures()
        {
            PositionTexture.Resize(Resolution.Width, Resolution.Height, 0);
            PositionTexture.Bind();
            PositionTexture.ReserveMemory();

            ColorTexture.Resize(Resolution.Width, Resolution.Height, 0);
            ColorTexture.Bind();
            ColorTexture.ReserveMemory();

            NormalTexture.Resize(Resolution.Width, Resolution.Height, 0);
            NormalTexture.Bind();
            NormalTexture.ReserveMemory();

            DiffuseMaterialTexture.Resize(Resolution.Width, Resolution.Height, 0);
            DiffuseMaterialTexture.Bind();
            DiffuseMaterialTexture.ReserveMemory();

            SpecularTexture.Resize(Resolution.Width, Resolution.Height, 0);
            SpecularTexture.Bind();
            SpecularTexture.ReserveMemory();

            VelocityTexture.Resize(Resolution.Width, Resolution.Height, 0);
            VelocityTexture.Bind();
            VelocityTexture.ReserveMemory();

            FinalTexture.Resize(Resolution.Width, Resolution.Height, 0);
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            DepthStencilTexture.Resize(Resolution.Width, Resolution.Height, 0);
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();
        }

        protected void LoadBuffers()
        {
            PositionTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgb16f,
                PixelFormat = PixelFormat.Rgb,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            PositionTexture.Bind();
            PositionTexture.ReserveMemory();

            ColorTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            ColorTexture.Bind();
            ColorTexture.ReserveMemory();

            NormalTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgb16f,
                PixelFormat = PixelFormat.Rgb,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            NormalTexture.Bind();
            NormalTexture.ReserveMemory();

            DiffuseMaterialTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DiffuseMaterialTexture.Bind();
            DiffuseMaterialTexture.ReserveMemory();

            SpecularTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            SpecularTexture.Bind();
            SpecularTexture.ReserveMemory();

            VelocityTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rg16f,
                PixelFormat = PixelFormat.Rg,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            VelocityTexture.Bind();
            VelocityTexture.ReserveMemory();

            FinalTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Rgba16f,
                PixelFormat = PixelFormat.Rgba,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            FinalTexture.Bind();
            FinalTexture.ReserveMemory();

            DepthStencilTexture = new Texture(Resolution.Width, Resolution.Height, 0)
            {
                Target = TextureTarget.Texture2D,
                EnableMipMap = false,
                EnableAnisotropy = false,
                PixelInternalFormat = PixelInternalFormat.Depth32fStencil8,
                PixelFormat = PixelFormat.DepthComponent,
                PixelType = PixelType.Float,
                MinFilter = TextureMinFilter.Linear,
                MagFilter = TextureMagFilter.Linear,
                WrapMode = TextureWrapMode.Clamp
            };
            DepthStencilTexture.Bind();
            DepthStencilTexture.ReserveMemory();

            GBuffer.Clear();
            GBuffer.Add(FramebufferAttachment.ColorAttachment0, PositionTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment1, ColorTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment2, NormalTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment3, DiffuseMaterialTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment4, SpecularTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment5, VelocityTexture);
            GBuffer.Add(FramebufferAttachment.ColorAttachment6, FinalTexture);
            GBuffer.Add(FramebufferAttachment.DepthStencilAttachment, DepthStencilTexture);

            GBuffer.Bind(FramebufferTarget.Framebuffer);
            GBuffer.AttachAttachments();
            GBuffer.Unbind(FramebufferTarget.Framebuffer);
        }

        private void LoadPointLightMesh()
        {
            _pointLightMesh = SimpleMesh.LoadFromFile(FilePathHelper.SPHERE_MESH_PATH, _pointLightProgram);
        }

        private void LoadSpotLightMesh()
        {
            _spotLightMesh = SimpleMesh.LoadFromFile(FilePathHelper.CONE_MESH_PATH, _spotLightProgram);
        }

        public void GeometryPass(TextureManager textureManager, Camera camera, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects)
        {
            // Clear final texture from last frame
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _geometryProgram.Use();
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffers(7, new DrawBuffersEnum[]
            {
                DrawBuffersEnum.ColorAttachment0,
                DrawBuffersEnum.ColorAttachment1,
                DrawBuffersEnum.ColorAttachment2,
                DrawBuffersEnum.ColorAttachment3,
                DrawBuffersEnum.ColorAttachment4,
                DrawBuffersEnum.ColorAttachment5,
                DrawBuffersEnum.ColorAttachment6
            });

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);

            camera.Draw(_geometryProgram);

            foreach (var brush in brushes)
            {
                BindTextures(textureManager, brush.TextureMapping);
                brush.Draw(_geometryProgram);
            }

            foreach (var gameObject in gameObjects.Where(g => g.Mesh != null))
            {
                BindTextures(textureManager, gameObject.TextureMapping);
                gameObject.Draw(_geometryProgram);
            }

            /*GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);
            _simpleProgram.Use();
            camera.Draw(_simpleProgram);
            var modelMatrix = new ModelMatrix(new Vector3(-20.0f, 0, 3.0f), Quaternion.FromAxisAngle(Vector3.UnitZ, -1.0f) * Quaternion.FromAxisAngle(Vector3.UnitY, -1.0f), new Vector3(5.0f, 5.0f, 18.0f));
            modelMatrix.Set(_simpleProgram);
            _spotLightMesh.Draw();*/
        }

        public void LightPass(Camera camera, IEnumerable<Light> lights, IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects, ShadowRenderer shadowRenderer)
        {
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            foreach (var light in lights)
            {
                var lightMesh = GetMeshForLight(light);
                StencilPass(light, camera, lightMesh);

                GL.Disable(EnableCap.Blend);
                shadowRenderer.Render(camera, light, brushes, gameObjects);
                GL.Enable(EnableCap.Blend);

                var lightProgram = GetProgramForLight(light);
                DrawLight(light, camera, lightMesh, light is PointLight ? shadowRenderer.PointDepthCubeMap : shadowRenderer.SpotDepthTexture, lightProgram);
            }

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.Blend);
        }

        public void LightPass(Camera camera, IEnumerable<Light> lights, Texture pointShadows, Texture spotShadows)
        {
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            foreach (var light in lights)
            {
                var lightMesh = GetMeshForLight(light);
                StencilPass(light, camera, lightMesh);

                var lightProgram = GetProgramForLight(light);
                var shadowTexture = light is PointLight ? pointShadows : spotShadows;
                DrawLight(light, camera, lightMesh, shadowTexture, lightProgram);
            }

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.Blend);
        }

        public void StencilPass(Light light, Camera camera, SimpleMesh mesh)
        {
            _stencilProgram.Use();

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            GL.DepthMask(false);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.StencilFunc(StencilFunction.Always, 0, 0);
            GL.StencilOpSeparate(StencilFace.Back, StencilOp.Keep, StencilOp.IncrWrap, StencilOp.Keep);
            GL.StencilOpSeparate(StencilFace.Front, StencilOp.Keep, StencilOp.DecrWrap, StencilOp.Keep);
            
            camera.Draw(_stencilProgram);
            light.DrawForStencilPass(_stencilProgram);
            mesh.Draw();
        }

        private void DrawLight(Light light, Camera camera, SimpleMesh mesh, Texture shadowMap, ShaderProgram program)
        {
            program.Use();

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            GL.StencilFunc(StencilFunction.Notequal, 0, 0xFF);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            program.BindTexture(PositionTexture, "positionMap", 0);
            program.BindTexture(ColorTexture, "colorMap", 1);
            program.BindTexture(NormalTexture, "normalMap", 2);
            program.BindTexture(DiffuseMaterialTexture, "diffuseMaterial", 3);
            program.BindTexture(SpecularTexture, "specularMap", 4);
            program.BindTexture(shadowMap, "shadowMap", 5);

            camera.Draw(program);
            program.SetUniform("cameraPosition", camera.Position);

            light.DrawForLightPass(Resolution, program);
            mesh.Draw();
        }

        private SimpleMesh GetMeshForLight(Light light)
        {
            if (light is PointLight)
            {
                return _pointLightMesh;
            }
            else if (light is SpotLight)
            {
                return _spotLightMesh;
            }
            else
            {
                return null;
            }
        }

        private ShaderProgram GetProgramForLight(Light light)
        {
            if (light is PointLight)
            {
                return _pointLightProgram;
            }
            else if (light is SpotLight)
            {
                return _spotLightProgram;
            }
            else
            {
                return null;
            }
        }

        private void BindTextures(TextureManager textureManager, TextureMapping textureMapping)
        {
            // TODO - Order brush rendering in a way that allows us to not re-bind duplicate textures repeatedly
            // Check brush's texture mapping to see which textures we need to bind
            var mainTexture = textureManager.RetrieveTexture(textureMapping.MainTextureID);

            GL.Uniform1(_geometryProgram.GetUniformLocation("useMainTexture"), (mainTexture != null) ? 1 : 0);
            if (mainTexture != null)
            {
                _geometryProgram.BindTexture(mainTexture, "mainTexture", 0);
            }

            var normalMap = textureManager.RetrieveTexture(textureMapping.NormalMapID);

            GL.Uniform1(_geometryProgram.GetUniformLocation("useNormalMap"), (normalMap != null) ? 1 : 0);
            if (normalMap != null)
            {
                _geometryProgram.BindTexture(normalMap, "normalMap", 1);
            }

            var diffuseMap = textureManager.RetrieveTexture(textureMapping.DiffuseMapID);

            GL.Uniform1(_geometryProgram.GetUniformLocation("useDiffuseMap"), (diffuseMap != null) ? 1 : 0);
            if (diffuseMap != null)
            {
                _geometryProgram.BindTexture(diffuseMap, "diffuseMap", 2);
            }

            var specularMap = textureManager.RetrieveTexture(textureMapping.SpecularMapID);

            GL.Uniform1(_geometryProgram.GetUniformLocation("useSpecularMap"), (specularMap != null) ? 1 : 0);
            if (specularMap != null)
            {
                _geometryProgram.BindTexture(specularMap, "specularMap", 3);
            }
        }

        private IEnumerable<GameObject> PerformFrustumCulling(IEnumerable<GameObject> gameObjects)
        {
            // Don't render meshes that are not in the camera's view

            // Using the position of the gameObject, determine if we should render the mesh
            // We will also need a bounding sphere or bounding box from the mesh to determine this
            foreach (var gameObject in gameObjects)
            {
                Vector3 position = gameObject.Position;
            }

            return gameObjects;
        }

        private IEnumerable<GameObject> PerformOcclusionCulling(IEnumerable<GameObject> gameObjects)
        {
            // Don't render meshes that are obscured by closer meshes
            foreach (var gameObject in gameObjects)
            {
                Vector3 position = gameObject.Position;
            }

            return gameObjects;
        }
    }
}
