using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Maps;
using SpiceEngine.Outputs;
using SpiceEngine.Rendering.Batches;
using SpiceEngine.Rendering.PostProcessing;
using SpiceEngine.Rendering.Processing;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpiceEngine.Rendering
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

        public BatchManager BatchManager { get; private set; }
        public TextureManager TextureManager { get; } = new TextureManager();

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

        private LogManager _logManager;

        public RenderManager(Resolution resolution, Resolution windowSize)
        {
            Resolution = resolution;
            WindowSize = windowSize;

            _logManager = new LogManager(_textRenderer);
        }

        public void LoadFromMap(Map map, IEntityProvider entityProvider, EntityMapping entityMapping)
        {
            BatchManager = new BatchManager(entityProvider);

            AddBrushes(map.Brushes, entityMapping.BrushIDs);
            //AddVolumes(map.Volumes, entityMapping.VolumeIDs);
            //AddLights(map.Lights, entityMapping.LightIDs);
            AddActors(map.Actors, entityMapping.ActorIDs);

            BatchManager.Load();

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

            GL.ClearColor(Color4.Black);

            IsLoaded = true;
        }

        public void AddBrush(MapBrush mapBrush, int entityID)
        {
            var mesh = mapBrush.ToMesh();
            var textureMapping = mapBrush.TexturesPaths.ToTextureMapping(TextureManager);
            BatchManager.AddBrush(entityID, mesh, textureMapping);
        }

        public void AddActor(MapActor mapActor, int entityID)
        {
            var meshes = mapActor.ToMeshes();

            if (mapActor.HasAnimations)
            {
                var textureMappings = new List<TextureMapping?>();

                using (var importer = new Assimp.AssimpContext())
                {
                    var scene = importer.ImportFile(mapActor.ModelFilePath);

                    for (var i = 0; i < scene.Meshes.Count; i++)
                    {
                        var textureMapping = i < mapActor.TexturesPaths.Count
                            ? mapActor.TexturesPaths[i].ToTextureMapping(TextureManager)
                            : new TexturePaths(scene.Materials[scene.Meshes[i].MaterialIndex], Path.GetDirectoryName(mapActor.ModelFilePath)).ToTextureMapping(TextureManager);

                        textureMappings.Add(textureMapping);
                    }
                }

                BatchManager.AddJoint(entityID, meshes, textureMappings);
            }
            else
            {
                var textureMappings = mapActor.TexturesPaths.Select(t => (TextureMapping?)t.ToTextureMapping(TextureManager));
                BatchManager.AddActor(entityID, meshes, textureMappings);
            }
        }

        private void AddBrushes(IList<MapBrush> mapBrushes, IList<int> brushIDs)
        {
            for (var i = 0; i < mapBrushes.Count; i++)
            {
                AddBrush(mapBrushes[i], brushIDs[i]);
            }
        }

        private void AddActors(IList<MapActor> mapActors, IList<int> actorIDs)
        {
            for (var i = 0; i < mapActors.Count; i++)
            {
                AddActor(mapActors[i], actorIDs[i]);
            }
        }

        /*private void AddVolumes(IList<MapVolume> mapVolumes, IList<int> volumeIDs)
        {
            for (var i = 0; i < mapVolumes.Count; i++)
            {
                var entityID = volumeIDs[i];
                var mesh = mapVolumes[i].ToMesh();

                BatchManager.AddVolume(entityID, mesh);
            }
        }*/

        /*private void AddLights(IList<Light> lights, IList<int> lightIDs)
        {
            foreach (var light in lights)
            {
                //BatchManager.light.ID;
            }
        }*/

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

        public void RenderEntityIDs(IEntityProvider entityProvider, Camera camera)
        {
            _selectionRenderer.BindForWriting();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _selectionRenderer.JointSelectionPass(camera, BatchManager);
            _selectionRenderer.SelectionPass(camera, BatchManager);
            _billboardRenderer.RenderLightSelections(camera, entityProvider.Lights);
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

        public void RenderSelection(IEntityProvider entityProvider, Camera camera, List<IEntity> entities, TransformModes transformMode)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.DepthFunc(DepthFunction.Always);

            for (var i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];

                if (entity is ILight light)
                {
                    var lightMesh = _lightRenderer.GetMeshForLight(light);
                    _wireframeRenderer.SelectionPass(camera, light, lightMesh);
                    _billboardRenderer.RenderSelection(camera, light);
                }
                else if (entity is Volume volume)
                {
                    _wireframeRenderer.SelectionPass(entityProvider, camera, entity, BatchManager);
                    _billboardRenderer.RenderSelection(camera, volume, BatchManager);

                    _selectionRenderer.BindForWriting();
                    _billboardRenderer.RenderSelection(camera, volume, BatchManager);
                }
                else
                {
                    // TODO - Find out why selection appears to be updating ahead of entity
                    _wireframeRenderer.SelectionPass(entityProvider, camera, entity, BatchManager);
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

        public void SetWireframeThickness(float thickness) => _wireframeRenderer.LineThickness = thickness;
        public void SetWireframeColor(Color4 color) => _wireframeRenderer.LineColor = color.ToVector4();
        public void SetSelectedWireframeThickness(float thickness) => _wireframeRenderer.SelectedLineThickness = thickness;
        public void SetSelectedWireframeColor(Color4 color) => _wireframeRenderer.SelectedLineColor = color.ToVector4();
        public void SetSelectedLightWireframeThickness(float thickness) => _wireframeRenderer.SelectedLightLineThickness = thickness;
        public void SetSelectedLightWireframeColor(Color4 color) => _wireframeRenderer.SelectedLightLineColor = color.ToVector4();

        public void SetGridUnit(float unit) => _wireframeRenderer.GridUnit = unit;
        public void SetGridLineThickness(float thickness) => _wireframeRenderer.GridLineThickness = thickness;
        public void SetGridUnitColor(Color4 color) => _wireframeRenderer.GridLineUnitColor = color.ToVector4();
        public void SetGridAxisColor(Color4 color) => _wireframeRenderer.GridLineAxisColor = color.ToVector4();
        public void SetGrid5Color(Color4 color) => _wireframeRenderer.GridLine5Color = color.ToVector4();
        public void SetGrid10Color(Color4 color) => _wireframeRenderer.GridLine10Color = color.ToVector4();

        public void RenderWireframe(IEntityProvider entityProvider, Camera camera)
        {
            _wireframeRenderer.BindForWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            if (RenderGrid)
            {
                _wireframeRenderer.RenderGridLines(camera);
            }

            _wireframeRenderer.WireframePass(camera, BatchManager);
            _wireframeRenderer.JointWireframePass(camera, BatchManager);

            GL.Enable(EnableCap.CullFace);
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            _billboardRenderer.RenderLights(camera, entityProvider.Lights);

            GL.Disable(EnableCap.DepthTest);

            _fxaaRenderer.Render(_wireframeRenderer.FinalTexture);
            _renderToScreen.Render(_fxaaRenderer.FinalTexture);
            _logManager.RenderToScreen();
        }

        public void RenderDiffuseFrame(IEntityProvider entityProvider, Camera camera)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(camera, BatchManager, TextureManager);
            _deferredRenderer.JointGeometryPass(camera, BatchManager, TextureManager);

            _deferredRenderer.BindForDiffuseWriting();

            if (RenderGrid)
            {
                GL.Disable(EnableCap.CullFace);
                _wireframeRenderer.RenderGridLines(camera);
                GL.Enable(EnableCap.CullFace);
            }

            _skyboxRenderer.Render(camera);
            _billboardRenderer.RenderLights(camera, entityProvider.Lights);

            _deferredRenderer.BindForLitTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(camera, BatchManager, TextureManager);
            _deferredRenderer.TransparentJointGeometryPass(camera, BatchManager, TextureManager);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.ColorTexture);
            _logManager.RenderToScreen();
        }

        public void RenderLitFrame(IEntityProvider entityProvider, Camera camera)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(camera, BatchManager, TextureManager);
            _deferredRenderer.JointGeometryPass(camera, BatchManager, TextureManager);

            RenderLights(entityProvider, camera);

            _deferredRenderer.BindForLitWriting();

            _skyboxRenderer.Render(camera);
            _billboardRenderer.RenderLights(camera, entityProvider.Lights);

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

            _deferredRenderer.TransparentGeometryPass(camera, BatchManager, TextureManager);
            _deferredRenderer.TransparentJointGeometryPass(camera, BatchManager, TextureManager);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.FinalTexture);
            _logManager.RenderToScreen();
        }

        public void RenderFullFrame(IEntityProvider entityProvider, Camera camera)
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            _deferredRenderer.GeometryPass(camera, BatchManager, TextureManager);
            _deferredRenderer.JointGeometryPass(camera, BatchManager, TextureManager);

            RenderLights(entityProvider, camera);

            _deferredRenderer.BindForLitWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);
            _skyboxRenderer.Render(camera);

            _deferredRenderer.BindForLitTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(camera, BatchManager, TextureManager);
            _deferredRenderer.TransparentJointGeometryPass(camera, BatchManager, TextureManager);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            // Read from GBuffer's final texture, so that we can post-process it
            //GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _deferredRenderer.GBuffer._handle);
            //GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
            var texture = _deferredRenderer.FinalTexture;

            GL.Disable(EnableCap.DepthTest);

            //_invertRenderer.Render(texture);
            _blurRenderer.Render(texture, _deferredRenderer.VelocityTexture, 60.0f);
            texture = _blurRenderer.FinalTexture;

            _renderToScreen.Render(texture);

            _textRenderer.RenderText("FPS: " + Frequency.ToString("0.##"), Resolution.Width - 9 * (10 + TextRenderer.GLYPH_WIDTH), Resolution.Height - (10 + TextRenderer.GLYPH_HEIGHT));
            _logManager.RenderToScreen();
        }

        private void RenderLights(IEntityProvider entityProvider, Camera camera)
        {
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            foreach (var light in entityProvider.Lights)
            {
                var lightMesh = _lightRenderer.GetMeshForLight(light);
                _lightRenderer.StencilPass(light, camera, lightMesh);

                GL.Disable(EnableCap.Blend);
                _shadowRenderer.Render(camera, light, BatchManager);
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
