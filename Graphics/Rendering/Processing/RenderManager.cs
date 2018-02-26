using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Lighting;
using Graphics.Meshes;
using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Matrices;
using Graphics.Rendering.PostProcessing;
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
    public class RenderManager
    {
        public Resolution Resolution { get; private set; }

        private ForwardRenderer _forwardRenderer = new ForwardRenderer();
        private DeferredRenderer _deferredRenderer = new DeferredRenderer();
        private ShadowRenderer _shadowRenderer = new ShadowRenderer();
        private LightRenderer _lightRenderer = new LightRenderer();
        private SkyboxRenderer _skyboxRenderer = new SkyboxRenderer();

        private Blur _blurRenderer = new Blur();
        private InvertColors _invertRenderer = new InvertColors();
        private TextRenderer _textRenderer = new TextRenderer();
        private RenderToScreen _renderToScreen = new RenderToScreen();

        public RenderManager(Resolution resolution) => Resolution = resolution;

        public void Load(IEnumerable<Brush> brushes, IEnumerable<GameObject> gameObjects, IEnumerable<string> skyboxTexturePaths)
        {
            _skyboxRenderer.SetTextures(skyboxTexturePaths);

            _forwardRenderer.Load(Resolution);
            _deferredRenderer.Load(Resolution);
            _shadowRenderer.Load(Resolution);
            _lightRenderer.Load(Resolution);
            _skyboxRenderer.Load(Resolution);
            _blurRenderer.Load(Resolution);
            _invertRenderer.Load(Resolution);
            _textRenderer.Load(Resolution);
            _renderToScreen.Load(Resolution);

            foreach (var brush in brushes)
            {
                brush.Load(_deferredRenderer._geometryProgram);
            }

            foreach (var gameObject in gameObjects)
            {
                gameObject.Model.Load(_deferredRenderer._geometryProgram);
            }
        }

        public void ResizeTextures()
        {
            _forwardRenderer.ResizeTextures(Resolution);
            _deferredRenderer.ResizeTextures(Resolution);
            _shadowRenderer.ResizeTextures(Resolution);
            _lightRenderer.ResizeTextures(Resolution);
            _skyboxRenderer.ResizeTextures(Resolution);
            _blurRenderer.ResizeTextures(Resolution);
            _invertRenderer.ResizeTextures(Resolution);
            _textRenderer.ResizeTextures(Resolution);
            _renderToScreen.ResizeTextures(Resolution);
        }

        public void RenderFrame(TextureManager textureManager, Camera camera, List<Light> lights, List<Brush> brushes, List<GameObject> gameObjects, double frequency)
        {
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            //_forwardRenderer.Render(_textureManager, _camera, _brushes, _gameObjects);
            //_skyboxRenderer.Render(_camera, _forwardRenderer._frameBuffer);

            _deferredRenderer.GeometryPass(Resolution, textureManager, camera, brushes, gameObjects.Where(g => g.Model is SimpleModel));
            _deferredRenderer.JointGeometryPass(Resolution, textureManager, camera, gameObjects.Where(g => g.Model is AnimatedModel));
            //_shadowRenderer.Render(_camera, _lights, _brushes, _gameObjects);
            //_deferredRenderer.LightPass(_camera, _lights, _shadowRenderer.PointDepthCubeMap, _shadowRenderer.SpotDepthTexture);
            _deferredRenderer.LightPass(Resolution, camera, lights, brushes, gameObjects, _shadowRenderer);

            //_skyboxRenderer.SkyTexture = _shadowRenderer.PointDepthCubeMap;
            _skyboxRenderer.Render(Resolution, camera, _deferredRenderer.GBuffer);

            // Read from GBuffer's final texture, so that we can post-process it
            //GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _lightRenderer._frameBuffer._handle);
            //GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
            //var texture = _lightRenderer.FinalTexture;
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _deferredRenderer.GBuffer._handle);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment6);
            var texture = _deferredRenderer.FinalTexture;

            GL.Disable(EnableCap.DepthTest);

            //_invertRenderer.Render(texture);
            _blurRenderer.Render(Resolution, texture, _deferredRenderer.VelocityTexture, 60.0f);
            //_renderToScreen.Render(_blurRenderer.FinalTexture);

            var fontPath = Directory.GetCurrentDirectory() + @"\..\..\.." + @"\GraphicsTest\Resources\Fonts\Roboto-Regular.ttf";
            var bitmapPath = Directory.GetCurrentDirectory() + @"\..\..\.." + @"\GraphicsTest\Resources\Fonts\Roboto-Regular.png";
            _textRenderer.SaveFontBitmap(fontPath, bitmapPath, 14);

            var fontTexture = Texture.LoadFromBitmap(bitmapPath, false, false);
            _renderToScreen.Render(texture);
            _textRenderer.RenderText(fontTexture, "FPS: " + frequency.ToString("0.##"), 10, Resolution.Height - (10 + TextRenderer.GLYPH_HEIGHT));
        }
    }
}
