using SpiceEngineCore.Components.Animations;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Cameras;
using SpiceEngineCore.Entities.Layers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using StarchUICore;
using StarchUICore.Groups;
using StarchUICore.Rendering.Batches;
using StarchUICore.Views;
using SweetGraphicsCore.Rendering.Batches;
using SweetGraphicsCore.Rendering.Billboards;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Batches
{
    public class BatchManager : IBatcher
    {
        private IEntityProvider _entityProvider;
        private ITextureProvider _textureProvider;
        private IAnimationProvider _animationProvider;
        private IUIProvider _uiProvider;

        private HashSet<int> _opaqueStaticIDs = new HashSet<int>();
        private HashSet<int> _opaqueAnimatedIDs = new HashSet<int>();
        private HashSet<int> _opaqueBillboardIDs = new HashSet<int>();
        private HashSet<int> _opaqueViewIDs = new HashSet<int>();
        private HashSet<int> _transparentStaticIDs = new HashSet<int>();
        private HashSet<int> _transparentAnimatedIDs = new HashSet<int>();
        private HashSet<int> _transparentBillboardIDs = new HashSet<int>();
        private HashSet<int> _transparentViewIDs = new HashSet<int>();

        private Dictionary<int, RenderTypes> _renderTypeByEntityID = new Dictionary<int, RenderTypes>();
        private Dictionary<int, int> _batchIndexByEntityID = new Dictionary<int, int>();
        private List<IBatch> _batches = new List<IBatch>();

        public bool IsLoaded { get; private set; } = false;

        public BatchManager(IEntityProvider entityProvider, ITextureProvider textureProvider)
        {
            _entityProvider = entityProvider;
            _textureProvider = textureProvider;
        }

        public void SetAnimationProvider(IAnimationProvider animationProvider) => _animationProvider = animationProvider;

        public void SetUIProvider(IUIProvider uiProvider) => _uiProvider = uiProvider;

        public void DuplicateBatch(int entityID, int newID)
        {
            var batch = GetBatch(entityID);

            // TODO = Handle texture mapping duplication outside of here (in RenderManager?)
            switch (batch)
            {
                case IMesh mesh:
                    AddEntity(newID, mesh.Duplicate());
                    break;
                case IModel model:
                    AddEntity(newID, model.Duplicate());
                    break;
                case IBillboard billboard:
                    AddEntity(newID, billboard.Duplicate());
                    break;
                case IElement element:
                    if (element is IView view)
                    {
                        AddEntity(newID, view.Duplicate());
                    }
                    else if (element is IGroup group)
                    {
                        AddEntity(newID, group.Duplicate());
                    }
                    break;
            }
        }

        public IBatch GetBatch(int entityID)
        {
            if (!_batchIndexByEntityID.ContainsKey(entityID)) throw new KeyNotFoundException("No batch found for entity ID " + entityID);

            var batchIndex = _batchIndexByEntityID[entityID];
            return _batches[batchIndex];
        }

        public IBatch GetBatchOrDefault(int entityID) => _batchIndexByEntityID.ContainsKey(entityID) ? _batches[_batchIndexByEntityID[entityID]] : null;

        public void RemoveByEntityID(int entityID)
        {
            GetEntityIDSet(_renderTypeByEntityID[entityID]).Remove(entityID);
            _renderTypeByEntityID.Remove(entityID);

            var batchIndex = _batchIndexByEntityID[entityID];
            _batchIndexByEntityID.Remove(entityID);

            var batch = _batches[batchIndex];
            batch.RemoveEntity(entityID);

            if (!batch.EntityIDs.Any())
            {
                // TODO - This is horribly inefficient AND not thread-safe
                // Since we are removing this batch from the list, the dictionary values need to be updated accordingly
                _batches.RemoveAt(batchIndex);

                foreach (var entityIDKey in _batchIndexByEntityID.Keys.ToList())
                {
                    var batchIndexValue = _batchIndexByEntityID[entityIDKey];

                    if (batchIndexValue >= batchIndex)
                    {
                        _batchIndexByEntityID[entityIDKey] = batchIndexValue - 1;
                    }
                }
            }
        }

        public void AddEntity(int entityID, IRenderable renderable)
        {
            var renderType = GetRenderTypeForRenderable(renderable);
            _renderTypeByEntityID.Add(entityID, renderType);

            var idSet = GetEntityIDSet(renderType);
            idSet.Add(entityID);

            var opaqueRenderType = renderType.Opaque();
            var opaqueIDSet = GetEntityIDSet(opaqueRenderType);

            var transparentRenderType = renderType.Transparent();
            var transparentIDSet = GetEntityIDSet(transparentRenderType);

            renderable.AlphaChanged += (s, args) =>
            {
                if (args.BecameTransparent)
                {
                    opaqueIDSet.Remove(entityID);
                    transparentIDSet.Add(entityID);
                    _renderTypeByEntityID[entityID] = transparentRenderType;
                }
                else if (args.BecameOpaque)
                {
                    transparentIDSet.Remove(entityID);
                    opaqueIDSet.Add(entityID);
                    _renderTypeByEntityID[entityID] = opaqueRenderType;
                }
            };

            // TODO - Should we hold off on binding this animation model until the model batch has been loaded?
            if (renderable is IAnimate animated && _animationProvider != null)
            {
                _animationProvider.AddAnimated(entityID, animated);
            }

            var batch = CreateOrGetBatch(entityID, renderable);
            batch.AddEntity(entityID, renderable);

            // TODO - Make this better. Currently I am arbitrarily centering brushes and model entities around the origin, but NOT actors...
            // Because of this, for the FIRST item in a batch with those entities, we must transform the mesh to fit around this...
            var entity = _entityProvider.GetEntity(entityID);

            if (batch.EntityCount == 1)
            {
                if (entity is IBrush)
                {
                    // Unfortunately, we have to wait for batch.AddEntity() here, since that is what sets up the offset and count in the batch...
                    entity.Transformed += (s, args) =>
                    {
                        // TODO - Don't recalculate matrix here (store in event args)
                        batch.Transform(args.ID, args.Transform);
                    };
                }
            }
            else
            {
                // Any additional entities in the batch is responsible for transforming their vertices if their model matrix changes...
                entity.Transformed += (s, args) =>
                {
                    // TODO - Don't recalculate matrix here (store in event args)
                    batch.Transform(args.ID, args.Transform);
                };

                if (entity is ITexturedEntity texturedEntity)
                {
                    texturedEntity.TextureTransformed += (s, args) =>
                    {
                        batch.TransformTexture(args.ID, entity.Position, args.Translation, args.Rotation, args.Scale);
                    };
                }
            }

            // TODO - What is this part doing? Do I need to do this for Model-Mesh entities as well?
            /*if (_entityProvider.GetEntity(entityID) is Brush brush)
            {
                batch.Transform(brush.ID, brush.GetModelMatrix());
            }*/

            //var entity = _entityProvider.GetEntity(entityID) as Entity;
            //batch.Transform(entityID, entity.GetModelMatrix());

            /*var entity = _entityProvider.GetEntity(entityID) as Entity;
            if (entity != null && !(entity is Actor) && !(entity is ILight) && !(entity is Volume))
            {
                batch.Transform(entity.ID, entity.GetModelMatrix());
            }*/

            //AddBatch(entityID, batch);
        }

        public void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate)
        {
            var batch = GetBatchOrDefault(entityID);
            batch?.UpdateVertices(entityID, vertexUpdate);
        }

        private IBatch CreateOrGetBatch(int entityID, IRenderable renderable)
        {
            var match = FindAppropriateBatch(entityID, renderable);

            if (match != null)
            {
                return match;
            }
            else
            {
                var batch = CreateBatch(renderable);
                _batches.Add(batch);

                var batchIndex = _batches.Count - 1;
                _batchIndexByEntityID.Add(entityID, batchIndex);

                //var entity = _entityProvider.GetEntity(entityID) as Entity;
                //batch.Transform(entityID, entity.GetModelMatrix());

                // TODO - For now, let's avoid this to tease out why the BatchManager is getting loaded too soon
                /*if (IsLoaded)
                {
                    batch.Load();
                }*/

                return batch;
            }
        }

        private IBatch CreateBatch(IRenderable renderable)
        {
            switch (renderable)
            {
                case IMesh mesh:
                    return new MeshBatch(mesh);
                case IModel model:
                    return new ModelBatch(model);
                case IBillboard billboard:
                    return new BillboardBatch(billboard);
                case IElement element:
                    var uiBatch = new UIBatch(element);
                    _uiProvider.OrderChanged += (s, args) => uiBatch.Reorder(args.IDs);
                    return new UIBatch(element);
            }

            throw new ArgumentOutOfRangeException("Could not handle renderable of type " + renderable.GetType());
        }

        private IBatch FindAppropriateBatch(int entityID, IRenderable renderable)
        {
            for (var i = 0; i < _batches.Count; i++)
            {
                var batch = _batches[i];

                if (batch.CompareUniforms(renderable))
                {
                    //var entity = _entityProvider.GetEntity(entityID);

                    // Any additional entities in the batch are responsible for transforming their vertices if their model matrix changes...
                    // TODO - Don't recalculate matrix here (store in event args)
                    /*entity.Transformed += (s, args) => batch.Transform(args.ID, args.Transform);

                    if (entity is ITexturedEntity texturedEntity)
                    {
                        texturedEntity.TextureTransformed += (s, args) => batch.TransformTexture(args.ID, entity.Position, args.Translation, args.Rotation, args.Scale);
                    }*/

                    _batchIndexByEntityID.Add(entityID, i);
                    return batch;
                }
            }

            return null;
        }

        /*private void AddBatch(int entityID, IBatch batch)
        {
            _batches.Add(batch);

            var batchIndex = _batches.Count - 1;
            _batchIndexByEntityID.Add(entityID, batchIndex);

            if (IsLoaded)
            {
                batch.Load();
            }
        }*/

        public void Load(int entityID)
        {
            var batch = GetBatch(entityID);

            if (!batch.IsLoaded)
            {
                Load();
            }
        }

        public void Load()
        {
            // TODO - Instead of checking every time, have a load queue
            foreach (var batch in _batches)
            {
                if (!batch.IsLoaded)
                {
                    batch.Load();
                }
            }

            IsLoaded = true;
        }

        public IBatchAction CreateBatchAction() => new BatchAction(this);

        /*private void DrawEntities(ShaderProgram shader, HashSet<int> ids, Action<int> action = null)
        {
            var batchIndices = new HashSet<int>();

            foreach (var id in _entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Render))
            {
                // TODO - Handle case where ids is NOT null, and we only want to render some of the entities within a single batch
                if ((ids == null || ids.Contains(id)))
                {
                    action?.Invoke(id);

                    var batchIndex = _batchIndexByEntityID[id];
                    if (!batchIndices.Contains(batchIndex))
                    {
                        batchIndices.Add(batchIndex);
                        _batches[batchIndex].Draw(_entityProvider, shader, _textureProvider);
                    }
                }
            }
        }

        private void DrawEntities(RenderTypes renderType, ShaderProgram shader, List<int> ids, Action<int> action = null)
        {
            var batchIndices = new HashSet<int>();
            var idSet = _entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Render).Union(GetEntityIDSet(renderType));

            foreach (var id in ids)
            {
                if (idSet.Contains(id))
                {
                    action?.Invoke(id);

                    var batchIndex = _batchIndexByEntityID[id];

                    if (!batchIndices.Contains(batchIndex))
                    {
                        batchIndices.Add(batchIndex);
                        _batches[batchIndex].Draw(_entityProvider, shader, _textureProvider);
                    }
                }
            }
        }

        private void DrawEntities(RenderTypes renderType, ShaderProgram shader, HashSet<int> ids, Action<int> action = null)
        {
            var batchIndices = new HashSet<int>();

            foreach (var id in _entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Render))
            {
                // TODO - Handle case where ids is NOT null, and we only want to render some of the entities within a single batch
                if ((ids == null || ids.Contains(id)) && GetEntityIDSet(renderType).Contains(id))
                {
                    action?.Invoke(id);

                    var batchIndex = _batchIndexByEntityID[id];

                    if (!batchIndices.Contains(batchIndex))
                    {
                        batchIndices.Add(batchIndex);
                        _batches[batchIndex].Draw(_entityProvider, shader, _textureProvider);
                    }
                }
            }
        }*/

        private IEnumerable<int> GetEntityIDs(RenderTypes? renderType, HashSet<int> idSet, List<int> idOrder)
        {
            var entityIDOrder = idOrder ?? _entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Render);
            var entityIDFilter = new HashSet<int>(_entityProvider.LayerProvider.GetEntityIDs(LayerTypes.Render));
            
            if (idSet != null)
            {
                entityIDFilter.IntersectWith(idSet);
            }

            if (renderType.HasValue)
            {
                entityIDFilter.IntersectWith(GetEntityIDSet(renderType.Value));
            }

            foreach (var entityID in entityIDOrder)
            {
                if (entityIDFilter.Contains(entityID))
                {
                    yield return entityID;
                }
            }
        }

        private void DrawEntities(ShaderProgram shader, RenderTypes? renderType, HashSet<int> idSet, List<int> idOrder, Action<int> action = null)
        {
            var batchIndices = new HashSet<int>();

            foreach (var id in GetEntityIDs(renderType, idSet, idOrder))
            {
                var batchIndex = _batchIndexByEntityID[id];
                if (!batchIndices.Contains(batchIndex))
                {
                    batchIndices.Add(batchIndex);
                    _batches[batchIndex].Draw(_entityProvider, shader, _textureProvider);
                }
            }
        }

        private HashSet<int> GetEntityIDSet(RenderTypes renderType)
        {
            switch (renderType)
            {
                case RenderTypes.OpaqueStatic:
                    return _opaqueStaticIDs;
                case RenderTypes.OpaqueAnimated:
                    return _opaqueAnimatedIDs;
                case RenderTypes.OpaqueBillboard:
                    return _opaqueBillboardIDs;
                case RenderTypes.OpaqueView:
                    return _opaqueViewIDs;
                case RenderTypes.TransparentStatic:
                    return _transparentStaticIDs;
                case RenderTypes.TransparentAnimated:
                    return _transparentAnimatedIDs;
                case RenderTypes.TransparentBillboard:
                    return _transparentBillboardIDs;
                case RenderTypes.TransparentView:
                    return _transparentViewIDs;
            }

            throw new ArgumentOutOfRangeException("Could not handle render type " + renderType);
        }

        private RenderTypes GetRenderTypeForRenderable(IRenderable renderable)
        {
            if (renderable is IView)
            {
                return renderable.IsTransparent ? RenderTypes.TransparentView : RenderTypes.OpaqueView;
            }
            else if (renderable.IsAnimated)
            {
                return renderable.IsTransparent ? RenderTypes.TransparentAnimated : RenderTypes.OpaqueAnimated;
            }
            else if (renderable is IBillboard)
            {
                return renderable.IsTransparent ? RenderTypes.TransparentBillboard : RenderTypes.OpaqueBillboard;
            }
            else
            {
                return renderable.IsTransparent ? RenderTypes.TransparentStatic : RenderTypes.OpaqueStatic;
            }
        }

        public class BatchAction : IBatchAction
        {
            private BatchManager _batchManager;
            private Queue<Action> _commandQueue = new Queue<Action>();

            private ShaderProgram _shader;
            private Action<int> _idAction;
            private RenderTypes? _renderType;

            private HashSet<int> _entityIDSet;
            private List<int> _entityIDOrder;

            public BatchAction(BatchManager batchManager) => _batchManager = batchManager;

            public ICamera Camera { get; set; }

            public IBatchAction SetShader(ShaderProgram shader)
            {
                _commandQueue.Enqueue(() =>
                {
                    _shader = shader;
                    _shader.Use();
                });
                return this;
            }

            public IBatchAction SetCamera(ICamera camera)
            {
                _commandQueue.Enqueue(() =>
                {
                    Camera = camera;
                    Camera.SetUniforms(_shader);
                });
                return this;
            }

            public IBatchAction SetCamera(ICamera camera, ILight light)
            {
                _commandQueue.Enqueue(() =>
                {
                    Camera = camera;
                    Camera.SetUniforms(_shader, light);
                });
                return this;
            }

            public IBatchAction SetUniform<T>(string name, T value) where T : struct
            {
                _commandQueue.Enqueue(() => _shader.SetUniform<T>(name, value));
                return this;
            }

            public IBatchAction SetPerIDAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _idAction = action);
                return this;
            }

            public IBatchAction SetRenderType(RenderTypes renderType)
            {
                _commandQueue.Enqueue(() => _renderType = renderType);
                return this;
            }

            public IBatchAction ClearRenderType()
            {
                _commandQueue.Enqueue(() => _renderType = null);
                return this;
            }

            public IBatchAction SetEntityIDSet(IEnumerable<int> ids)
            {
                _commandQueue.Enqueue(() => _entityIDSet = new HashSet<int>(ids));
                return this;
            }

            public IBatchAction SetEntityIDOrder(IEnumerable<int> ids)
            {
                _commandQueue.Enqueue(() => _entityIDOrder = new List<int>(ids));
                return this;
            }

            public IBatchAction ClearEntityIDs()
            {
                _commandQueue.Enqueue(() =>
                {
                    _entityIDSet = null;
                    _entityIDOrder = null;
                });
                return this;
            }

            public IBatchAction PerformAction(Action action)
            {
                _commandQueue.Enqueue(action);
                return this;
            }

            public IBatchAction SetTexture(ITexture texture, string name, int index)
            {
                _commandQueue.Enqueue(() => _shader.BindTexture(texture, name, index));
                return this;
            }

            public IBatchAction Render()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(_shader, _renderType, _entityIDSet, _entityIDOrder, _idAction));
                return this;
            }

            public void Execute()
            {
                while (_commandQueue.Any())
                {
                    Action action = _commandQueue.Dequeue();
                    action();
                }
            }
        }
    }
}
