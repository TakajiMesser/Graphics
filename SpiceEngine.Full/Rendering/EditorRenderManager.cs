using SpiceEngineCore.Entities.Selection;
using SpiceEngine.Game;
using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.GLEnums;
using SpiceEngine.Rendering.PostProcessing;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Layers;
using SpiceEngineCore.Entities.Lights;
using SpiceEngineCore.Entities.Volumes;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Renderers;
using SweetGraphicsCore.Renderers.Processing;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Billboards;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Selection;
using SweetGraphicsCore.Vertices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangyHIDCore.Outputs;

namespace SpiceEngine.Rendering
{
    public enum RenderModes
    {
        Wireframe,
        Diffuse,
        Lit,
        Full
    }

    public class EditorRenderManager : RenderManager, IGridRenderer
    {
        private ISelectionProvider _selectionProvider;
        private LogManager _logManager;
        private WireframeRenderer _wireframeRenderer = new WireframeRenderer();

        public EditorRenderManager(IRenderContext context, Display display, PanelCamera camera) : base(context, display)
        {
            _logManager = new LogManager(_textRenderer);

            EditorCamera = camera;
            Display.Resolution.ResolutionChanged += (s, args) => EditorCamera?.Camera.UpdateAspectRatio(args.Resolution.AspectRatio);
        }

        public PanelCamera EditorCamera { get; }
        public RenderModes RenderMode { get; set; }

        public void SetSelectionProvider(ISelectionProvider selectionProvider) => _selectionProvider = selectionProvider;

        protected async override Task LoadInitial()
        {
            await Invoker.InvokeAsync(() =>
            {
                _wireframeRenderer.Load(RenderContext, Display.Resolution);
            });
            await base.LoadInitial();
        }

        protected override void LoadComponent(int entityID, IRenderable component)
        {
            var colorID = SelectionHelper.GetColorFromID(entityID);

            switch (component)
            {
                case IModel model:
                    for (var i = 0; i < model.Meshes.Count; i++)
                    {
                        model.Meshes[i] = TransformToEditorMesh(model.Meshes[i], colorID);
                    }
                    break;
                case IMesh mesh:
                    component = TransformToEditorMesh(mesh, colorID);
                    break;
                case IBillboard billboard:
                    billboard.SetColor(colorID);
                    break;
            }

            base.LoadComponent(entityID, component);

            /*if (component is IMesh mesh)
            {
                var colorID = SelectionHelper.GetColorFromID(entityID);
                _batchManager.AddEntity(entityID, TransformToEditorMesh(mesh, colorID));
            }
            else if (component is IModel model)
            {
                var colorID = SelectionHelper.GetColorFromID(entityID);

                for (var i = 0; i < model.Meshes.Count; i++)
                {
                    model.Meshes[i] = TransformToEditorMesh(model.Meshes[i], colorID);
                }

                _batchManager.AddEntity(entityID, component);
            }
            else if (component is IBillboard billboard)
            {
                billboard.SetColor(SelectionHelper.GetColorFromID(entityID));
            }
            else
            {
                _batchManager.AddEntity(entityID, component);
            }*/
        }

        private IMesh TransformToEditorMesh(IMesh mesh, Color4 colorID)
        {
            var vertices = mesh.Vertices.Select(v => new EditorVertex3D(v, colorID)).ToList();
            var triangleIndices = mesh.TriangleIndices.ToList();
            var vertexSet = new Vertex3DSet<EditorVertex3D>(vertices, triangleIndices);

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

                    _wireframeRenderer.SelectionPass(EditorCamera.Camera, light, lightMesh);
                    _billboardRenderer.RenderSelection(EditorCamera.Camera, light);
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
            switch (transformMode)
            {
                case TransformModes.Translate:
                    if (entity is IDirectional directional)
                    {
                        _selectionRenderer.RenderTranslationArrows(EditorCamera.Camera, entity.Position, directional.XDirection, directional.YDirection, directional.ZDirection);
                    }
                    else
                    {
                        _selectionRenderer.RenderTranslationArrows(EditorCamera.Camera, entity.Position);
                    }
                    break;
                case TransformModes.Rotate:
                    /*if (entity is IDirectional directional)
                    {
                        _selectionRenderer.RenderRotationRings(_camera, entity.Position, directional.XDirection, directional.YDirection, directional.ZDirection);
                    }
                    else
                    {*/
                        _selectionRenderer.RenderRotationRings(EditorCamera.Camera, entity.Position);
                    //}
                    break;
                case TransformModes.Scale:
                    /*if (entity is IDirectional directional)
                    {
                        _selectionRenderer.RenderScaleLines(_camera, entity.Position, directional.XDirection, directional.YDirection, directional.ZDirection);
                    }
                    else
                    {*/
                        _selectionRenderer.RenderScaleLines(EditorCamera.Camera, entity.Position);
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
            var entityIDs = _entityProvider.ActiveScene.Volumes.Where(v => v is PhysicsVolume).Select(v => v.ID);

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

        protected override void RenderEntityIDs()
        {
            // TODO - Perform check first to see if ANY selection IDs (via layers) even exist...
            _selectionRenderer.BindForWriting();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Display.Resolution.Width, Display.Resolution.Height);

            _selectionRenderer.SelectionPass(EditorCamera.Camera, _batchManager, _entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Select));
            _billboardRenderer.RenderLightSelectIDs(EditorCamera.Camera, _entityProvider.ActiveScene.Lights.Where(l => _entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Select).Contains(l.ID)));

            var vertexEntities = _entityProvider.LayerProvider.GetLayerEntityIDs("Vertices");
            if (vertexEntities.Any())
            {
                _billboardRenderer.RenderVertexSelectIDs(EditorCamera.Camera, vertexEntities.Select(v => _entityProvider.GetEntity(v)));
            }
        }

        public void RenderWireframe()
        {
            _wireframeRenderer.BindForWriting();
            GL.Viewport(0, 0, Display.Resolution.Width, Display.Resolution.Height);

            if (RenderGrid)
            {
                _wireframeRenderer.RenderGridLines(EditorCamera.Camera);
            }

            _wireframeRenderer.WireframePass(EditorCamera.Camera, _batchManager);

            GL.Enable(EnableCap.CullFace);
            GL.DepthMask(true);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            _billboardRenderer.RenderLights(EditorCamera.Camera, _entityProvider.ActiveScene.Lights);

            GL.Disable(EnableCap.DepthTest);

            _fxaaRenderer.Render(_wireframeRenderer.FinalTexture);
            _renderToScreen.Render(_fxaaRenderer.FinalTexture);
            _logManager.RenderToScreen();
        }

        public void RenderDiffuseFrame()
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Display.Resolution.Width, Display.Resolution.Height);

            _deferredRenderer.GeometryPass(EditorCamera.Camera, _batchManager);

            _deferredRenderer.BindForDiffuseWriting();

            if (RenderGrid)
            {
                GL.Disable(EnableCap.CullFace);
                _wireframeRenderer.RenderGridLines(EditorCamera.Camera);
                GL.Enable(EnableCap.CullFace);
            }

            _skyboxRenderer.Render(EditorCamera.Camera);
            _billboardRenderer.GeometryPass(EditorCamera.Camera, _batchManager);
            _billboardRenderer.RenderLights(EditorCamera.Camera, _entityProvider.ActiveScene.Lights);

            _deferredRenderer.BindForTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationModeEXT.FuncAdd);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(EditorCamera.Camera, _batchManager);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.ColorTexture);
            _logManager.RenderToScreen();

            if (_selectionProvider != null && _selectionProvider.SelectionCount > 0)
            {
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

                GL.Disable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Blend);
                GL.DepthFunc(DepthFunction.Always);

                _wireframeRenderer.SelectionPass(EditorCamera.Camera, _selectionProvider.SelectedIDs, _batchManager);
            }
        }

        public void RenderLitFrame()
        {
            _deferredRenderer.BindForGeometryWriting();
            GL.Viewport(0, 0, Display.Resolution.Width, Display.Resolution.Height);

            _deferredRenderer.GeometryPass(EditorCamera.Camera, _batchManager);

            RenderLights();

            _deferredRenderer.BindForLitWriting();

            _skyboxRenderer.Render(EditorCamera.Camera);
            _billboardRenderer.RenderLights(EditorCamera.Camera, _entityProvider.ActiveScene.Lights);

            if (RenderGrid)
            {
                GL.Disable(EnableCap.CullFace);
                _wireframeRenderer.RenderGridLines(EditorCamera.Camera);
                GL.Enable(EnableCap.CullFace);
            }

            _deferredRenderer.BindForLitTransparentWriting();

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationModeEXT.FuncAdd);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);
            GL.Disable(EnableCap.CullFace);

            _deferredRenderer.TransparentGeometryPass(EditorCamera.Camera, _batchManager);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.Disable(EnableCap.DepthTest);

            _renderToScreen.Render(_deferredRenderer.FinalTexture);
            _logManager.RenderToScreen();

            if (_selectionProvider != null && _selectionProvider.SelectionCount > 0)
            {
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);

                GL.Disable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Blend);
                GL.DepthFunc(DepthFunction.Always);

                _wireframeRenderer.SelectionPass(EditorCamera.Camera, _selectionProvider.SelectedIDs, _batchManager);
            }
        }

        private void RenderLights()
        {
            GL.Enable(EnableCap.StencilTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationModeEXT.FuncAdd);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);

            foreach (var light in _entityProvider.ActiveScene.Lights)
            {
                var lightMesh = _lightRenderer.GetMeshForLight(light);
                _lightRenderer.StencilPass(light, EditorCamera.Camera, lightMesh);

                GL.Disable(EnableCap.Blend);
                _shadowRenderer.Render(EditorCamera.Camera, light, _batchManager);
                GL.Enable(EnableCap.Blend);

                _deferredRenderer.BindForLitWriting();
                GL.Viewport(0, 0, Display.Resolution.Width, Display.Resolution.Height);

                var lightProgram = _lightRenderer.GetProgramForLight(light);
                var shadowMap = (light is PointLight) ? _shadowRenderer.PointDepthCubeMap : _shadowRenderer.SpotDepthTexture;
                _lightRenderer.LightPass(_deferredRenderer, light, EditorCamera.Camera, lightMesh, shadowMap, lightProgram);
            }

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.Blend);
        }
    }
}
