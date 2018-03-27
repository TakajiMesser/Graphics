using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Entities.Models;
using TakoEngine.Outputs;
using TakoEngine.Rendering.PostProcessing;
using TakoEngine.Rendering.Textures;

namespace TakoEngine.Rendering.Processing
{
    public enum RenderModes
    {
        Wireframe,
        Diffuse,
        Lit,
        Full
    }

    public class RenderManager
    {
        public Resolution Resolution { get; private set; }
        public double Frequency { get; internal set; }

        private ForwardRenderer _forwardRenderer = new ForwardRenderer();
        private DeferredRenderer _deferredRenderer = new DeferredRenderer();
        private WireframeRenderer _wireframeRenderer = new WireframeRenderer();
        private ShadowRenderer _shadowRenderer = new ShadowRenderer();
        private LightRenderer _lightRenderer = new LightRenderer();
        private SkyboxRenderer _skyboxRenderer = new SkyboxRenderer();
        private SelectionRenderer _selectionRenderer = new SelectionRenderer();
        private BillboardRenderer _billboardRenderer = new BillboardRenderer();

        private FXAARenderer _fxaaRenderer = new FXAARenderer();
        private Blur _blurRenderer = new Blur();
        private InvertColors _invertRenderer = new InvertColors();
        private TextRenderer _textRenderer = new TextRenderer();
        private RenderToScreen _renderToScreen = new RenderToScreen();

        public RenderManager(Resolution resolution) => Resolution = resolution;

        public void Load(IEnumerable<Brush> brushes, IEnumerable<Actor> actors, IEnumerable<string> skyboxTexturePaths)
        {
            _skyboxRenderer.SetTextures(skyboxTexturePaths);

            _forwardRenderer.Load(Resolution);
            _deferredRenderer.Load(Resolution);
            _wireframeRenderer.Load(Resolution);
            _shadowRenderer.Load(Resolution);
            _lightRenderer.Load(Resolution);
            _skyboxRenderer.Load(Resolution);
            _selectionRenderer.Load(Resolution);
            _billboardRenderer.Load(Resolution);
            _fxaaRenderer.Load(Resolution);
            _blurRenderer.Load(Resolution);
            _invertRenderer.Load(Resolution);
            _textRenderer.Load(Resolution);
            _renderToScreen.Load(Resolution);

            foreach (var brush in brushes)
            {
                brush.Load(_deferredRenderer._geometryProgram);
            }

            foreach (var actor in actors)
            {
                actor.Model.Load(_deferredRenderer._geometryProgram);
            }

            GL.ClearColor(Color4.Black);
        }

        public void Resize()
        {
            _forwardRenderer.ResizeTextures(Resolution);
            _deferredRenderer.ResizeTextures(Resolution);
            _wireframeRenderer.ResizeTextures(Resolution);
            _shadowRenderer.ResizeTextures(Resolution);
            _lightRenderer.ResizeTextures(Resolution);
            _skyboxRenderer.ResizeTextures(Resolution);
            _selectionRenderer.ResizeTextures(Resolution);
            _billboardRenderer.ResizeTextures(Resolution);
            _fxaaRenderer.ResizeTextures(Resolution);
            _blurRenderer.ResizeTextures(Resolution);
            _invertRenderer.ResizeTextures(Resolution);
            _textRenderer.ResizeTextures(Resolution);
            _renderToScreen.ResizeTextures(Resolution);
        }

        public void RenderEntityIDs(Camera camera, List<Brush> brushes, List<Actor> actors, List<Light> lights)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _selectionRenderer.GBuffer._handle);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _selectionRenderer.SelectionPass(camera, brushes, actors.Where(g => g.Model is SimpleModel));
            _selectionRenderer.JointSelectionPass(camera, actors.Where(g => g.Model is AnimatedModel));
            _billboardRenderer.RenderLightSelections(camera, lights);
        }

        public int GetEntityIDFromPoint(Vector2 point) => _selectionRenderer.GetEntityIDFromPoint(point);

        public void RenderSelection(Camera camera, IEntity entity)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.DepthFunc(DepthFunction.Always);

            if (entity is Light light)
            {
                var lightMesh = _lightRenderer.GetMeshForLight(light);
                _wireframeRenderer.SelectionPass(camera, light, lightMesh);
                _billboardRenderer.RenderSelection(camera, light);
                // Let Billboard Renderer know to render this light with a special texture
            }
            else
            {
                _wireframeRenderer.SelectionPass(camera, entity);
            }
        }

        public void RenderWireframe(TextureManager textureManager, Camera camera, List<Brush> brushes, List<Actor> actors, List<Light> lights)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _wireframeRenderer.GBuffer._handle);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _wireframeRenderer.WireframePass(camera, brushes, actors.Where(g => g.Model is SimpleModel));
            _wireframeRenderer.JointWireframePass(camera, actors.Where(g => g.Model is AnimatedModel));

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            _billboardRenderer.RenderLights(camera, lights);

            GL.Disable(EnableCap.DepthTest);

            _fxaaRenderer.Render(_wireframeRenderer.FinalTexture);
            _renderToScreen.Render(_fxaaRenderer.FinalTexture);
        }

        public void RenderDiffuseFrame(TextureManager textureManager, Camera camera, List<Brush> brushes, List<Actor> actors, List<Light> lights)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _deferredRenderer.GBuffer._handle);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            _deferredRenderer.GeometryPass(textureManager, camera, brushes, actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(textureManager, camera, actors.Where(g => g.Model is AnimatedModel));

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _deferredRenderer.GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment1);

            _skyboxRenderer.Render(camera);
            _billboardRenderer.RenderLights(camera, lights);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.ColorTexture);
        }

        public void RenderLitFrame(TextureManager textureManager, Camera camera, List<Light> lights, List<Brush> brushes, List<Actor> actors)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _deferredRenderer.GBuffer._handle);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            _deferredRenderer.GeometryPass(textureManager, camera, brushes, actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(textureManager, camera, actors.Where(g => g.Model is AnimatedModel));
            RenderLights(camera, lights, brushes, actors);

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _deferredRenderer.GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);

            _skyboxRenderer.Render(camera);
            _billboardRenderer.RenderLights(camera, lights);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.FinalTexture);
        }

        public void RenderFullFrame(TextureManager textureManager, Camera camera, List<Light> lights, List<Brush> brushes, List<Actor> actors)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _deferredRenderer.GBuffer._handle);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            _deferredRenderer.GeometryPass(textureManager, camera, brushes, actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(textureManager, camera, actors.Where(g => g.Model is AnimatedModel));

            RenderLights(camera, lights, brushes, actors);

            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _deferredRenderer.GBuffer._handle);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment6);

            _skyboxRenderer.Render(camera);

            // Read from GBuffer's final texture, so that we can post-process it
            //GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _deferredRenderer.GBuffer._handle);
            //GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
            var texture = _deferredRenderer.FinalTexture;

            GL.Disable(EnableCap.DepthTest);

            //_invertRenderer.Render(texture);
            _blurRenderer.Render(texture, _deferredRenderer.VelocityTexture, 60.0f);
            texture = _blurRenderer.FinalTexture;

            _renderToScreen.Render(texture);
            _textRenderer.RenderText("FPS: " + Frequency.ToString("0.##"), 10, Resolution.Height - (10 + TextRenderer.GLYPH_HEIGHT));
        }

        private void RenderLights(Camera camera, List<Light> lights, List<Brush> brushes, List<Actor> actors)
        {
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            foreach (var light in lights)
            {
                var lightMesh = _lightRenderer.GetMeshForLight(light);
                _lightRenderer.StencilPass(light, camera, lightMesh);

                GL.Disable(EnableCap.Blend);
                _shadowRenderer.Render(camera, light, brushes, actors);
                GL.Enable(EnableCap.Blend);

                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, _deferredRenderer.GBuffer._handle);
                GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

                var lightProgram = _lightRenderer.GetProgramForLight(light);
                var shadowMap = (light is PointLight) ? _shadowRenderer.PointDepthCubeMap : _shadowRenderer.SpotDepthTexture;
                _lightRenderer.LightPass(Resolution, _deferredRenderer, light, camera, lightMesh, shadowMap, lightProgram);
            }

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.Blend);
        }
    }
}
