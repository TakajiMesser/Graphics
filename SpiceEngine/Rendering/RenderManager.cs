using CitrusAnimationCore.Animations;
using OpenTK.Graphics.OpenGL;
using SpiceEngine.Entities.Selection;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Batches;
using SpiceEngine.Rendering.PostProcessing;
using SpiceEngine.Utilities;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Layers;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Entities.Volumes;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Maps;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using StarchUICore;
using SweetGraphicsCore.Renderers;
using SweetGraphicsCore.Renderers.PostProcessing;
using SweetGraphicsCore.Renderers.Processing;
using SweetGraphicsCore.Rendering;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Billboards;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Selection;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngine.Rendering
{
    public enum RenderModes
    {
        Wireframe,
        Diffuse,
        Lit,
        Full
    }

    public class RenderManager : RenderableLoader, IRenderProvider, IGridRenderer
    {
        public RenderModes RenderMode { get; set; }
        public Resolution Resolution { get; private set; }
        public Resolution WindowSize { get; private set; }
        public double Frequency { get; internal set; }
        public bool RenderGrid { get; set; }

        // TODO - Make this less janky
        public bool IsInEditorMode { get; set; }

        private IAnimationProvider _animationProvider;
        private IUIProvider _uiProvider;
        private ISelectionProvider _selectionProvider;

        private TextureManager _textureManager = new TextureManager();
        private FontManager _fontManager;
        private BatchManager _batchManager;

        //private ForwardRenderer _forwardRenderer;
        private DeferredRenderer _deferredRenderer;
        private WireframeRenderer _wireframeRenderer;
        private ShadowRenderer _shadowRenderer;
        private LightRenderer _lightRenderer;
        private SkyboxRenderer _skyboxRenderer;
        private SelectionRenderer _selectionRenderer;
        private BillboardRenderer _billboardRenderer;

        private FXAARenderer _fxaaRenderer;
        private Blur _blurRenderer;
        private InvertColors _invertRenderer;
        private TextRenderer _textRenderer;
        private RenderToScreen _renderToScreen;
        private UIRenderer _uiRenderer;

        private LogManager _logManager;

        private IInvoker _invoker;
        public IInvoker Invoker
        {
            get => _invoker;
            set
            {
                _invoker = value;
                _textureManager.Invoker = value;
            }
        }

        public RenderManager(Resolution resolution, Resolution windowSize)
        {
            Resolution = resolution;
            WindowSize = windowSize;

            // TODO - We likely want to split the resolution by nCameras for splitscreen support
            Resolution.ResolutionChanged += (s, args) =>
            {
                foreach (var camera in _entityProvider.Cameras.Where(c => c.IsActive))
                {
                    camera.UpdateAspectRatio(args.AspectRatio);
                }
            };

            InitializeRenderers();

            _fontManager = new FontManager(_textureManager);
            _logManager = new LogManager(_textRenderer);
        }

        private void InitializeRenderers()
        {
            _deferredRenderer = new DeferredRenderer(_textureManager);
            _wireframeRenderer = new WireframeRenderer(_textureManager);
            _shadowRenderer = new ShadowRenderer(_textureManager);
            _lightRenderer = new LightRenderer(_textureManager);
            _skyboxRenderer = new SkyboxRenderer(_textureManager);
            _selectionRenderer = new SelectionRenderer(_textureManager);
            _billboardRenderer = new BillboardRenderer(_textureManager);

            _fxaaRenderer = new FXAARenderer(_textureManager);
            _blurRenderer = new Blur(_textureManager);
            _invertRenderer = new InvertColors(_textureManager);
            _textRenderer = new TextRenderer(_textureManager);
            _renderToScreen = new RenderToScreen(_textureManager);
            _uiRenderer = new UIRenderer(_textureManager);
        }

        public IRenderable GetRenderable(int entityID) => _componentByID[entityID];
        public IRenderable GetRenderableOrDefault(int entityID) => HasRenderable(entityID) ? GetRenderable(entityID) : default;

        public bool HasRenderable(int entityID) => _componentByID.ContainsKey(entityID);

        public override void SetEntityProvider(IEntityProvider entityProvider)
        {
            base.SetEntityProvider(entityProvider);

            /*_entityProvider.EntitiesAdded += (s, args) =>
            {
                foreach (var builder in args.Builders)
                {
                    builder.Item2.
                }
            };*/
            /*foreach (var camera in _entityProvider.Cameras.Where(c => c.IsActive))
            {
                camera.UpdateAspectRatio(Resolution.AspectRatio);
            }*/

            _batchManager = new BatchManager(_entityProvider, _textureManager);

            if (_animationProvider != null)
            {
                _batchManager.SetAnimationProvider(_animationProvider);
            }

            if (_uiProvider != null)
            {
                _batchManager.SetUIProvider(_uiProvider);
            }
        }

        public void SetAnimationProvider(IAnimationProvider animationProvider)
        {
            _animationProvider = animationProvider;
            _batchManager?.SetAnimationProvider(_animationProvider);
        }

        public void SetUIProvider(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
            _uiProvider.SetTextureProvider(_textureManager);
            _batchManager?.SetUIProvider(_uiProvider);
        }

        public void SetSelectionProvider(ISelectionProvider selectionProvider) => _selectionProvider = selectionProvider;

        public void LoadFromMap(IMap map)
        {
            if (map is Map castMap)
            {
                _skyboxRenderer.SetTextures(castMap.SkyboxTextureFilePaths);
            }
        }

        public override Task LoadBuilderAsync(int entityID, IRenderableBuilder builder) => Task.Run(() =>
        {
            var component = builder.ToRenderable();

            if (component != null)
            {
                if (component is IBillboard billboard)
                {
                    billboard.LoadTexture(_textureManager);
                }
                else if (component is ITexturedMesh texturedMesh)
                {
                    if (builder is ITexturePather texturePather)
                    {
                        var texturePaths = texturePather.TexturesPaths.FirstOrDefault();

                        if (texturePaths != null && !texturePaths.IsEmpty)
                        {
                            texturedMesh.TextureMapping = texturePaths.ToTextureMapping(_textureManager);
                        }
                    }
                }
                else if (component is IModel model)
                {
                    if (builder is ITexturePather texturePather)
                    {
                        if (builder is IModelPather modelPather && !string.IsNullOrEmpty(modelPather.ModelFilePath))
                        {
                            using (var importer = new Assimp.AssimpContext())
                            {
                                var scene = importer.ImportFile(modelPather.ModelFilePath);

                                for (var i = 0; i < model.Meshes.Count; i++)
                                {
                                    if (model.Meshes[i] is ITexturedMesh modelTexturedMesh)
                                    {
                                        var textureMapping = i < texturePather.TexturesPaths.Count
                                            ? texturePather.TexturesPaths[i].ToTextureMapping(_textureManager)
                                            : scene.Materials[scene.Meshes[i].MaterialIndex].ToTexturePaths(Path.GetDirectoryName(modelPather.ModelFilePath)).ToTextureMapping(_textureManager);

                                        modelTexturedMesh.TextureMapping = textureMapping;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (var i = 0; i < model.Meshes.Count; i++)
                            {
                                if (model.Meshes[i] is ITexturedMesh modelTexturedMesh && i < texturePather.TexturesPaths.Count)
                                {
                                    modelTexturedMesh.TextureMapping = texturePather.TexturesPaths[i].ToTextureMapping(_textureManager);
                                }
                            }
                        }
                    }
                }

                _componentAndIDQueue.Enqueue(Tuple.Create(component, entityID));
            }
        });

        protected override Task LoadInitial()
        {
            // TODO - If Invoker is null, queue up this action
            return Invoker.RunAsync(() =>
            {
                // TODO - For now, just use the first available camera
                //_camera = _entityProvider.Cameras.First();
                //try {
                //_forwardRenderer.Load(Resolution);
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
                _uiRenderer.Load(WindowSize);

                //var font = FontManager.AddFontFile(TextRenderer.FONT_PATH, 14);
                //_logManager.SetFont(font);

                var clearColor = Color4.Black;
                GL.ClearColor(clearColor.R, clearColor.G, clearColor.B, clearColor.A);
                /*} catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }*/
            });
        }

        protected override void LoadComponents()
        {
            try
            {
                base.LoadComponents();

                // TODO - If Invoker is null, queue up this action
                //Invoker.RunSync(() => _batchManager.Load());
                //Invoker.ForceUpdate();
                //Invoker.RunAsync(() => _batchManager.Load()).ContinueWith(t => Invoker.ForceUpdate());
                Invoker.RunAsync(() =>
                {
                    try
                    {
                        _batchManager.Load();
                        //_uiProvider.Load();

                        Invoker.ForceUpdate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // TODO - Can we remove this call?
        public void LoadBatcher() => _batchManager.Load();

        protected override void LoadComponent(int entityID, IRenderable component)
        {
            base.LoadComponent(entityID, component);

            if (IsInEditorMode && component is IMesh mesh)
            {
                var colorID = SelectionHelper.GetColorFromID(entityID);
                _batchManager.AddEntity(entityID, TransformToEditorMesh(mesh, colorID));
            }
            else if (IsInEditorMode && component is IModel model)
            {
                var colorID = SelectionHelper.GetColorFromID(entityID);

                for (var i = 0; i < model.Meshes.Count; i++)
                {
                    model.Meshes[i] = TransformToEditorMesh(model.Meshes[i], colorID);
                }

                _batchManager.AddEntity(entityID, component);
            }
            else if (IsInEditorMode && component is IBillboard billboard)
            {
                billboard.SetColor(SelectionHelper.GetColorFromID(entityID));
            }
            else
            {
                _batchManager.AddEntity(entityID, component);

                if (component is IElement element)
                {
                    //_uiProvider.AddElement(entityID, element);
                }
            }
        }

        private IMesh TransformToEditorMesh(IMesh mesh, Color4 colorID)
        {
            var vertices = mesh.Vertices.Select(v => new EditorVertex3D(v, colorID)).ToList();
            var triangleIndices = mesh.TriangleIndices.ToList();
            var vertexSet = new Vertex3DSet<EditorVertex3D>(mesh.Vertices.Select(v => new EditorVertex3D(v, colorID)).ToList(), mesh.TriangleIndices.ToList());

            if (mesh is ITexturedMesh texturedMesh)
            {
                return new TexturedMesh<EditorVertex3D>(vertexSet)
                {
                    Material = texturedMesh.Material,
                    TextureMapping = texturedMesh.TextureMapping
                };
            }
            else if (mesh is IColoredMesh coloredMesh)
            {
                return new ColoredMesh<EditorVertex3D>(vertexSet)
                {
                    Color = coloredMesh.Color
                };
            }
            else
            {
                return new Mesh<EditorVertex3D>(vertexSet);
            }
        }

        public void RemoveEntity(int entityID) => _batchManager.RemoveByEntityID(entityID);

        public void Duplicate(int entityID, int duplicateID) => _batchManager.DuplicateBatch(entityID, duplicateID);

        public void ResizeResolution()
        {
            if (IsLoaded)
            {
                //_forwardRenderer.ResizeTextures(Resolution);
                _deferredRenderer.Resize(Resolution);
                _wireframeRenderer.Resize(Resolution);
                _shadowRenderer.Resize(Resolution);
                _skyboxRenderer.Resize(Resolution);
                _lightRenderer.Resize(Resolution);
                _selectionRenderer.Resize(Resolution);
                _billboardRenderer.Resize(Resolution);
                _fxaaRenderer.Resize(Resolution);
                _blurRenderer.Resize(Resolution);
                _invertRenderer.Resize(Resolution);
                _textRenderer.Resize(Resolution);
                _uiRenderer.Resize(Resolution);
            }
        }

        public void ResizeWindow()
        {
            if (IsLoaded)
            {
                _renderToScreen.Resize(WindowSize);
            }
        }

        /*public void RenderEntityIDs(Volume volume)
        {
            _selectionRenderer.BindForWriting();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            //_selectionRenderer.SelectionPass();
        }*/

        public void SetSelected(IEnumerable<int> entityIDs)
        {
            foreach (var entityID in entityIDs)
            {
                // TODO - Handle light selection differently, since lights are not stored in the BatchManager
                _batchManager.UpdateVertices(entityID, v => ((EditorVertex3D)v).Selected());
            }
        }

        public void SetDeselected(IEnumerable<int> entityIDs)
        {
            foreach (var entityID in entityIDs)
            {
                _batchManager.UpdateVertices(entityID, v => ((EditorVertex3D)v).Deselected());
            }
        }

        public int GetEntityIDFromSelection(Vector2 coordinates)
        {
            // Mouse coordinates are from top-left
            var mouseCoordinates = coordinates;

            // We need to convert these to instead be from bottom-left
            var windowCoordinates = new Vector2(mouseCoordinates.X, WindowSize.Height - mouseCoordinates.Y);

            // We now need to convert these coordinates from window-size to resolution-size
            var resolutionCoordinates = new Vector2(Resolution.Width * windowCoordinates.X / WindowSize.Width, Resolution.Height * windowCoordinates.Y / WindowSize.Height);

            _selectionRenderer.BindForReading();
            return _selectionRenderer.GetEntityIDFromPoint(resolutionCoordinates);
        }

        public void RenderSelection(IEnumerable<IEntity> entities, TransformModes transformMode)
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.DepthFunc(DepthFunction.Always);

            foreach (var entity in entities)
            {
                if (entity is ILight light)
                {
                    var lightMesh = _lightRenderer.GetMeshForLight(light);
                    var camera = _entityProvider.Cameras.First(c => c.IsActive);

                    _wireframeRenderer.SelectionPass(camera, light, lightMesh);
                    _billboardRenderer.RenderSelection(camera, light);
                }
                else if (entity is Volume volume)
                {
                    // TODO - Render volumes (need to add mesh to BatchManager)
                    /*_wireframeRenderer.SelectionPass(entityProvider, camera, entity, BatchManager);
                    _billboardRenderer.RenderSelection(camera, volume, BatchManager);

                    _selectionRenderer.BindForWriting();
                    _billboardRenderer.RenderSelection(camera, volume, BatchManager);*/
                }
                else
                {
                    // TODO - Find out why selection appears to be updating ahead of entity
                    //_wireframeRenderer.SelectionPass(_entityProvider, _camera, entity, BatchManager);
                }
            }

            var lastEntity = entities.LastOrDefault();
            if (lastEntity != null)
            {
                // Render the RGB arrows over the selection
                GL.Clear(ClearBufferMask.DepthBufferBit);
                GL.DepthFunc(DepthFunction.Less);

                RenderTransform(lastEntity, transformMode);

                // Render the RGB arrows into the selection buffer as well, which means that R, G, and B are "reserved" ID colors
                _selectionRenderer.BindForWriting();
                GL.Clear(ClearBufferMask.DepthBufferBit);
                GL.DepthFunc(DepthFunction.Less);

                RenderTransform(lastEntity, transformMode);
            }
        }

        private void RenderTransform(IEntity entity, TransformModes transformMode)
        {
            var camera = _entityProvider.Cameras.First(c => c.IsActive);

            switch (transformMode)
            {
                case TransformModes.Translate:
                    if (entity is IDirectional directional)
                    {
                        _selectionRenderer.RenderTranslationArrows(camera, entity.Position, directional.XDirection, directional.YDirection, directional.ZDirection);
                    }
                    else
                    {
                        _selectionRenderer.RenderTranslationArrows(camera, entity.Position);
                    }
                    break;
                case TransformModes.Rotate:
                    /*if (entity is IDirectional directional)
                    {
                        _selectionRenderer.RenderRotationRings(_camera, entity.Position, directional.XDirection, directional.YDirection, directional.ZDirection);
                    }
                    else
                    {*/
                        _selectionRenderer.RenderRotationRings(camera, entity.Position);
                    //}
                    break;
                case TransformModes.Scale:
                    /*if (entity is IDirectional directional)
                    {
                        _selectionRenderer.RenderScaleLines(_camera, entity.Position, directional.XDirection, directional.YDirection, directional.ZDirection);
                    }
                    else
                    {*/
                        _selectionRenderer.RenderScaleLines(camera, entity.Position);
                    //}
                    break;
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

        public void SetPhysicsVolumeColor(Color4 color)
        {
            var entityIDs = _entityProvider.Volumes.Where(v => v is PhysicsVolume).Select(v => v.ID);

            foreach (var entityID in entityIDs)
            {
                var batch = _batchManager.GetBatchOrDefault(entityID);
                if (batch is MeshBatch meshBatch)
                {
                    meshBatch.UpdateVertices(entityID, v => v is IColorVertex colorVertex
                        ? (IVertex3D)colorVertex.Colored(color)
                        : v);
                }
            }
        }

        //public bool RenderWireframe { get; set; }
        public bool LogToScreen { get; set; }

        protected override void Update()
        {
            // TODO - It would be better to have separate objects run different Update() implementations than to branch every loop iteration here
            switch (RenderMode)
            {
                case RenderModes.Wireframe:
                    RenderWireframe();
                    break;
                case RenderModes.Diffuse:
                    RenderDiffuseFrame();
                    break;
                case RenderModes.Lit:
                    RenderLitFrame();
                    break;
                case RenderModes.Full:
                    RenderFullFrame();
                    break;
            }

            //if (IsInEditorMode)
            //{
                RenderEntityIDs();

                // TODO - Determine how to handle this
                /*if (SelectionManager.SelectionCount > 0)
                {
                    _renderManager.RenderSelection(SelectionManager.SelectedEntities, TransformMode);
                }*/
            //}
        }

        private void RenderEntityIDs()
        {
            // TODO - Perform check first to see if ANY selection IDs (via layers) even exist...
            _selectionRenderer.BindForWriting();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

            var camera = _entityProvider.Cameras.First(c => c.IsActive);
            _selectionRenderer.SelectionPass(camera, _batchManager, _entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Select));
            _billboardRenderer.RenderLightSelectIDs(camera, _entityProvider.Lights.Where(l => _entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Select).Contains(l.ID)));

            if (IsInEditorMode)
            {
                var vertexEntities = _entityProvider.LayerProvider.GetLayerEntityIDs("Vertices");
                if (vertexEntities.Any())
                {
                    _billboardRenderer.RenderVertexSelectIDs(camera, vertexEntities.Select(v => _entityProvider.GetEntity(v)));
                }
            }
            else
            {
                _uiRenderer.RenderSelections(_batchManager, _uiProvider);
            }
        }

        public void RenderWireframe()
        {
            _wireframeRenderer.BindForWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);
            var camera = _entityProvider.Cameras.First(c => c.IsActive);

            if (RenderGrid)
            {
                _wireframeRenderer.RenderGridLines(camera);
            }

            _wireframeRenderer.WireframePass(camera, _batchManager);

            GL.Enable(EnableCap.CullFace);
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            //_billboardRenderer.RenderLights(_camera, _entityProvider.Lights);

            GL.Disable(EnableCap.DepthTest);

            _fxaaRenderer.Render(_wireframeRenderer.FinalTexture);
            _renderToScreen.Render(_fxaaRenderer.FinalTexture);
            _logManager.RenderToScreen();
        }

        public void RenderDiffuseFrame()
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);
            var camera = _entityProvider.Cameras.First(c => c.IsActive);

            _deferredRenderer.GeometryPass(camera, _batchManager);

            _deferredRenderer.BindForDiffuseWriting();

            if (RenderGrid)
            {
                GL.Disable(EnableCap.CullFace);
                _wireframeRenderer.RenderGridLines(camera);
                GL.Enable(EnableCap.CullFace);
            }

            _skyboxRenderer.Render(camera);
            _billboardRenderer.GeometryPass(camera, _batchManager);
            _billboardRenderer.RenderLights(camera, _entityProvider.Lights);

            _deferredRenderer.BindForTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(camera, _batchManager);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.ColorTexture);
            _logManager.RenderToScreen();

            if (IsInEditorMode && _selectionProvider != null && _selectionProvider.SelectionCount > 0)
            {
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

                GL.Disable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Blend);
                GL.DepthFunc(DepthFunction.Always);

                _wireframeRenderer.SelectionPass(camera, _selectionProvider.SelectedIDs, _batchManager);
            }
        }

        public void RenderLitFrame()
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);
            var camera = _entityProvider.Cameras.First(c => c.IsActive);

            _deferredRenderer.GeometryPass(camera, _batchManager);

            RenderLights();

            _deferredRenderer.BindForLitWriting();

            _skyboxRenderer.Render(camera);
            _billboardRenderer.RenderLights(camera, _entityProvider.Lights);

            if (RenderGrid)
            {
                GL.Disable(EnableCap.CullFace);
                _wireframeRenderer.RenderGridLines(camera);
                GL.Enable(EnableCap.CullFace);
            }

            _deferredRenderer.BindForLitTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(camera, _batchManager);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.FinalTexture);
            _logManager.RenderToScreen();

            if (IsInEditorMode && _selectionProvider != null && _selectionProvider.SelectionCount > 0)
            {
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

                GL.Disable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Blend);
                GL.DepthFunc(DepthFunction.Always);

                _wireframeRenderer.SelectionPass(camera, _selectionProvider.SelectedIDs, _batchManager);
            }
        }

        public void RenderFullFrame()
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);
            var camera = _entityProvider.Cameras.First(c => c.IsActive);

            _deferredRenderer.GeometryPass(camera, _batchManager);

            RenderLights();

            _deferredRenderer.BindForLitWriting();
            GL.Viewport(0, 0, Resolution.Width, Resolution.Height);
            //_skyboxRenderer.Render(camera);

            _deferredRenderer.BindForLitTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(camera, _batchManager);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            /*if (IsInEditorMode && _selectionProvider != null && _selectionProvider.SelectionCount > 0)
            {
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

                GL.Disable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Blend);
                GL.DepthFunc(DepthFunction.Always);

                _wireframeRenderer.SelectionPass(_camera, _selectionProvider.SelectedIDs, BatchManager);
            }*/

            // Read from GBuffer's final texture, so that we can post-process it
            //GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _deferredRenderer.GBuffer._handle);
            //GL.ReadBuffer(ReadBufferMode.ColorAttachment1);
            var texture = _deferredRenderer.FinalTexture;

            GL.Disable(EnableCap.DepthTest);

            //_invertRenderer.Render(texture);
            _blurRenderer.Render(texture, _deferredRenderer.VelocityTexture, 60.0f);
            texture = _blurRenderer.FinalTexture;

            _renderToScreen.Render(texture);

            RenderUI();
            //_logManager.RenderToScreen();
        }

        private void RenderLights()
        {
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);

            var camera = _entityProvider.Cameras.First(c => c.IsActive);

            foreach (var light in _entityProvider.Lights)
            {
                var lightMesh = _lightRenderer.GetMeshForLight(light);
                _lightRenderer.StencilPass(light, camera, lightMesh);

                GL.Disable(EnableCap.Blend);
                _shadowRenderer.Render(camera, light, _batchManager);
                GL.Enable(EnableCap.Blend);

                _deferredRenderer.BindForLitWriting();
                GL.Viewport(0, 0, Resolution.Width, Resolution.Height);

                var lightProgram = _lightRenderer.GetProgramForLight(light);
                var shadowMap = (light is PointLight) ? _shadowRenderer.PointDepthCubeMap : _shadowRenderer.SpotDepthTexture;
                _lightRenderer.LightPass(_deferredRenderer, light, camera, lightMesh, shadowMap, lightProgram);
            }

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.Blend);
        }

        // TODO - Remove this test code and set fonts up more appropriately
        private bool _isFontSet = false;

        private void RenderUI()
        {
            _uiRenderer.Render(_batchManager, _uiProvider);

            //var font = FontManager.GetFont(TextRenderer.FONT_PATH);

            /*if (!_isFontSet)
            {
                var entityID = _entityProvider.GetEntity("Text 2C").ID;
                var uiElement = _uiProvider.GetUIElement(entityID);
                
                if (uiElement is Label textView)
                {
                    textView.Font = font;
                }

                _isFontSet = true;
            }*/

            _textRenderer.Render(_batchManager, _uiProvider);

            //var x = Resolution.Width - 9 * (10 + font.GlyphWidth);
            //var y = /*Resolution.Height - */(10 + font.GlyphHeight);
            //var y = 100;
            //_textRenderer.RenderText(font, "FPS: " + Frequency.ToString("0.##"), x, y);
        }
    }
}
