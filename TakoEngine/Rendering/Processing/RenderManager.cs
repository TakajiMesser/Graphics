using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using TakoEngine.Entities;
using TakoEngine.Entities.Cameras;
using TakoEngine.Entities.Lights;
using TakoEngine.Entities.Models;
using TakoEngine.Game;
using TakoEngine.Maps;
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
        public Resolution WindowSize { get; private set; }
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

        public RenderManager(Resolution resolution, Resolution windowSize)
        {
            Resolution = resolution;
            WindowSize = windowSize;
        }

        public void Load(GameState gameState, Map map)
        {
            _skyboxRenderer.SetTextures(map.SkyboxTextureFilePaths);

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
            _renderToScreen.Load(WindowSize);

            _deferredRenderer.LoadEntities(gameState.Brushes, gameState.Actors);

            GL.ClearColor(Color4.Black);
        }

        public void ResizeResolution()
        {
            _forwardRenderer.ResizeTextures(Resolution);
            _deferredRenderer.ResizeTextures(Resolution);
            _wireframeRenderer.ResizeTextures(Resolution);
            _shadowRenderer.ResizeTextures(Resolution);
            _skyboxRenderer.ResizeTextures(Resolution);
            _lightRenderer.ResizeTextures(Resolution);
            _selectionRenderer.ResizeTextures(Resolution);
            _billboardRenderer.ResizeTextures(Resolution);
            _fxaaRenderer.ResizeTextures(Resolution);
            _blurRenderer.ResizeTextures(Resolution);
            _invertRenderer.ResizeTextures(Resolution);
            _textRenderer.ResizeTextures(Resolution);
        }

        public void ResizeWindow() => _renderToScreen.ResizeTextures(WindowSize);

        public void RenderEntityIDs(GameState gameState)
        {
            _selectionRenderer.BindForWriting();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _selectionRenderer.SelectionPass(gameState.Camera, gameState.Brushes, gameState.Actors.Where(g => g.Model is SimpleModel));
            _selectionRenderer.JointSelectionPass(gameState.Camera, gameState.Actors.Where(g => g.Model is AnimatedModel));
            _billboardRenderer.RenderLightSelections(gameState.Camera, gameState.Lights);
        }

        public int GetEntityIDFromPoint(Vector2 point)
        {
            _selectionRenderer.BindForReading();
            return _selectionRenderer.GetEntityIDFromPoint(point);
        }

        public void RenderSelection(Camera camera, IEntity entity, TransformModes transformMode)
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
            }
            else
            {
                // TODO - Find out why selection appears to be updating ahead of entity
                _wireframeRenderer.SelectionPass(camera, entity);
            }

            // Render the RGB arrows over the selection
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.DepthFunc(DepthFunction.Less);

            switch (transformMode)
            {
                case TransformModes.Translate:
                    _selectionRenderer.RenderTranslationArrows(camera, entity.Position);
                    break;
                case TransformModes.Rotate:
                    _selectionRenderer.RenderRotationRings(camera, entity.Position);
                    break;
                case TransformModes.Scale:
                    _selectionRenderer.RenderScaleLines(camera, entity.Position);
                    break;
            }

            // Render the RGB arrows into the selection buffer as well, which means that R, G, and B are "reserved" ID colors
            _selectionRenderer.BindForWriting();
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.DepthFunc(DepthFunction.Less);

            switch (transformMode)
            {
                case TransformModes.Translate:
                    _selectionRenderer.RenderTranslationArrows(camera, entity.Position);
                    break;
                case TransformModes.Rotate:
                    _selectionRenderer.RenderRotationRings(camera, entity.Position);
                    break;
                case TransformModes.Scale:
                    _selectionRenderer.RenderScaleLines(camera, entity.Position);
                    break;
            }
        }

        public void RenderWireframe(GameState gameState)
        {
            _wireframeRenderer.BindForWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _wireframeRenderer.WireframePass(gameState.Camera, gameState.Brushes, gameState.Actors.Where(g => g.Model is SimpleModel));
            _wireframeRenderer.JointWireframePass(gameState.Camera, gameState.Actors.Where(g => g.Model is AnimatedModel));
            //GL.Disable(EnableCap.DepthTest);
            //GL.DepthMask(false);
            //_wireframeRenderer.RenderGridLines(gameState.Camera);

            GL.Enable(EnableCap.CullFace);
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            _billboardRenderer.RenderLights(gameState.Camera, gameState.Lights);

            GL.Disable(EnableCap.DepthTest);

            _fxaaRenderer.Render(_wireframeRenderer.FinalTexture);
            _renderToScreen.Render(_fxaaRenderer.FinalTexture);
        }

        public void RenderDiffuseFrame(GameState gameState)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(gameState.TextureManager, gameState.Camera, gameState.Brushes, gameState.Actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(gameState.TextureManager, gameState.Camera, gameState.Actors.Where(g => g.Model is AnimatedModel));

            _deferredRenderer.BindForBillboardWriting();

            _billboardRenderer.RenderLights(gameState.Camera, gameState.Lights);
            _skyboxRenderer.Render(gameState.Camera);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.ColorTexture);
            //_renderToScreen.Render(_selectionRenderer.FinalTexture);
        }

        public void RenderLitFrame(GameState gameState)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(gameState.TextureManager, gameState.Camera, gameState.Brushes, gameState.Actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(gameState.TextureManager, gameState.Camera, gameState.Actors.Where(g => g.Model is AnimatedModel));
            RenderLights(gameState.Camera, gameState.Lights, gameState.Brushes, gameState.Actors);

            _deferredRenderer.BindForBillboardWriting();

            _skyboxRenderer.Render(gameState.Camera);
            _billboardRenderer.RenderLights(gameState.Camera, gameState.Lights);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.FinalTexture);
        }

        public void RenderFullFrame(GameState gameState)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(gameState.TextureManager, gameState.Camera, gameState.Brushes, gameState.Actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(gameState.TextureManager, gameState.Camera, gameState.Actors.Where(g => g.Model is AnimatedModel));

            RenderLights(gameState.Camera, gameState.Lights, gameState.Brushes, gameState.Actors);

            _deferredRenderer.BindForBillboardWriting();

            _skyboxRenderer.Render(gameState.Camera);

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

                _deferredRenderer.BindForBillboardWriting();
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
