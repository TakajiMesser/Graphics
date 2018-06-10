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
        public bool IsLoaded { get; private set; }
        public bool RenderGrid { get; set; }

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

        public void Load(List<string> texturePaths)
        {
            _skyboxRenderer.SetTextures(texturePaths);

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

            GL.ClearColor(Color4.Black);

            IsLoaded = true;
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

        public void RenderEntityIDs(Camera camera, EntityManager entityManager)
        {
            _selectionRenderer.BindForWriting();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _selectionRenderer.SelectionPass(camera, entityManager.Brushes, entityManager.Volumes, entityManager.Actors.Where(g => g.Model is SimpleModel));
            _selectionRenderer.JointSelectionPass(camera, entityManager.Actors.Where(g => g.Model is AnimatedModel));
            _billboardRenderer.RenderLightSelections(camera, entityManager.Lights);
        }

        /*public void RenderEntityIDs(Volume volume)
        {
            _selectionRenderer.BindForWriting();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            //_selectionRenderer.SelectionPass();
        }*/

        public int GetEntityIDFromPoint(Vector2 point)
        {
            _selectionRenderer.BindForReading();
            return _selectionRenderer.GetEntityIDFromPoint(point);
        }

        public void RenderSelection(Camera camera, List<IEntity> entities, TransformModes transformMode)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.DepthFunc(DepthFunction.Always);

            for (var i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];

                if (entity is Light light)
                {
                    var lightMesh = _lightRenderer.GetMeshForLight(light);
                    _wireframeRenderer.SelectionPass(camera, light, lightMesh);
                    _billboardRenderer.RenderSelection(camera, light);
                }
                else if (entity is Volume volume)
                {
                    _wireframeRenderer.SelectionPass(camera, entity);
                    _billboardRenderer.RenderSelection(camera, volume);

                    _selectionRenderer.BindForWriting();
                    _billboardRenderer.RenderSelection(camera, volume);
                }
                else
                {
                    // TODO - Find out why selection appears to be updating ahead of entity
                    _wireframeRenderer.SelectionPass(camera, entity);
                }

                if (i == entities.Count - 1)
                {
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
            }
        }

        public void RotateGrid(float pitch, float yaw, float roll) => _wireframeRenderer.GridRotation = Quaternion.FromEulerAngles(pitch, yaw, roll);

        public void RenderWireframe(Camera camera, EntityManager entityManager)
        {
            _wireframeRenderer.BindForWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            if (RenderGrid)
            {
                _wireframeRenderer.RenderGridLines(camera);
            }

            _wireframeRenderer.WireframePass(camera, entityManager.Brushes, entityManager.Volumes, entityManager.Actors.Where(g => g.Model is SimpleModel));
            _wireframeRenderer.JointWireframePass(camera, entityManager.Actors.Where(g => g.Model is AnimatedModel));

            GL.Enable(EnableCap.CullFace);
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            _billboardRenderer.RenderLights(camera, entityManager.Lights);

            GL.Disable(EnableCap.DepthTest);

            _fxaaRenderer.Render(_wireframeRenderer.FinalTexture);
            _renderToScreen.Render(_fxaaRenderer.FinalTexture);
        }

        public void RenderDiffuseFrame(Camera camera, EntityManager entityManager, TextureManager textureManager)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(textureManager, camera, entityManager.Brushes, entityManager.Actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(textureManager, camera, entityManager.Actors.Where(g => g.Model is AnimatedModel));

            _deferredRenderer.BindForDiffuseWriting();

            if (RenderGrid)
            {
                GL.Disable(EnableCap.CullFace);
                _wireframeRenderer.RenderGridLines(camera);
                GL.Enable(EnableCap.CullFace);
            }

            _skyboxRenderer.Render(camera);
            _billboardRenderer.RenderLights(camera, entityManager.Lights);

            _deferredRenderer.BindForTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(camera, entityManager.Volumes);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.ColorTexture);
        }

        public void RenderLitFrame(Camera camera, EntityManager entityManager, TextureManager textureManager)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(textureManager, camera, entityManager.Brushes, entityManager.Actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(textureManager, camera, entityManager.Actors.Where(g => g.Model is AnimatedModel));

            RenderLights(camera, entityManager.Lights, entityManager.Brushes, entityManager.Actors);

            _deferredRenderer.BindForLitWriting();

            _skyboxRenderer.Render(camera);
            _billboardRenderer.RenderLights(camera, entityManager.Lights);

            if (RenderGrid)
            {
                GL.Disable(EnableCap.CullFace);
                _wireframeRenderer.RenderGridLines(camera);
                GL.Enable(EnableCap.CullFace);
            }

            _deferredRenderer.BindForLitTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(camera, entityManager.Volumes);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.FinalTexture);
        }

        public void RenderFullFrame(Camera camera, EntityManager entityManager, TextureManager textureManager)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(textureManager, camera, entityManager.Brushes, entityManager.Actors.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(textureManager, camera, entityManager.Actors.Where(g => g.Model is AnimatedModel));

            RenderLights(camera, entityManager.Lights, entityManager.Brushes, entityManager.Actors);

            _deferredRenderer.BindForLitWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);
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

        private void RenderLights(Camera camera, IEnumerable<Light> lights, IEnumerable<Brush> brushes, IEnumerable<Actor> actors)
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

                _deferredRenderer.BindForLitWriting();
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
