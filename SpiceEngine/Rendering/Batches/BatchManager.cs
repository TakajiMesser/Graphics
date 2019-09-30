using SpiceEngine.Entities;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Utilities;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Batches
{
    public enum RenderTypes
    {
        OpaqueStatic,
        OpaqueAnimated,
        OpaqueBillboard,
        TransparentStatic,
        TransparentAnimated,
        TransparentBillboard
    }

    public class BatchManager
    {
        private IEntityProvider _entityProvider;
        private ITextureProvider _textureProvider;

        private HashSet<int> _opaqueStaticIDs = new HashSet<int>();
        private HashSet<int> _opaqueAnimatedIDs = new HashSet<int>();
        private HashSet<int> _opaqueBillboardIDs = new HashSet<int>();
        private HashSet<int> _transparentStaticIDs = new HashSet<int>();
        private HashSet<int> _transparentAnimatedIDs = new HashSet<int>();
        private HashSet<int> _transparentBillboardIDs = new HashSet<int>();

        private Dictionary<int, RenderTypes> _renderTypeByEntityID = new Dictionary<int, RenderTypes>();
        private Dictionary<int, int> _batchIndexByEntityID = new Dictionary<int, int>();
        private List<IBatch> _batches = new List<IBatch>();

        public bool IsLoaded { get; private set; } = false;

        public BatchManager(IEntityProvider entityProvider, ITextureProvider textureProvider)
        {
            _entityProvider = entityProvider;
            _textureProvider = textureProvider;
        }

        public void DuplicateBatch(int entityID, int newID)
        {
            var batch = GetBatch(entityID);
            
            // TODO = Handle texture mapping duplication outside of here (in RenderManager?)
            switch (batch)
            {
                case IMesh mesh:
                    AddEntity(newID, mesh.Duplicate());
                    break;
                case Model model:
                    AddEntity(newID, model.Duplicate());
                    break;
                case TextureID textureID:
                    AddEntity(newID, textureID.Duplicate());
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

            var batch = CreateOrGetBatch(entityID, renderable);
            batch.AddEntity(entityID, renderable);

            // TODO - Make this better. Currently I am arbitrarily centering brushes and model entities around the origin, but NOT actors...
            // Because of this, for the FIRST item in a batch with those entities, we must transform the mesh to fit around this...
            var entity = _entityProvider.GetEntity(entityID);

            if (batch.EntityCount == 1)
            {
                if (entity is Brush)
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

        public void UpdateVertices(int entityID, Func<IVertex3D, IVertex3D> vertexUpdate)
        {
            var batch = GetBatchOrDefault(entityID);
            batch?.UpdateVertices(entityID, vertexUpdate);
        }

        private Batch CreateOrGetBatch(int entityID, IRenderable renderable)
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

        private Batch CreateBatch(IRenderable renderable)
        {
            switch (renderable)
            {
                case IMesh mesh:
                    return new MeshBatch(mesh);
                case Model model:
                    return new ModelBatch(model);
                case TextureID textureID:
                    return new BillboardBatch(textureID);
            }

            throw new ArgumentOutOfRangeException("Could not handle renderable of type " + renderable.GetType());
        }

        private Batch FindAppropriateBatch(int entityID, IRenderable renderable)
        {
            if (renderable is IMesh || renderable is TextureID)
            {
                var entityA = _entityProvider.GetEntity(entityID);

                for (var i = 0; i < _batches.Count; i++)
                {
                    if (_batches[i] is MeshBatch meshBatch)
                    {
                        var entityB = _entityProvider.GetEntity(meshBatch.EntityIDs.First());

                        if (entityA.CompareUniforms(entityB))
                        {
                            /*// Any additional entities in the batch is responsible for transforming their vertices if their model matrix changes...
                            entityA.Transformed += (s, args) =>
                            {
                                // TODO - Don't recalculate matrix here (store in event args)
                                meshBatch.Transform(args.ID, args.Transform);
                            };

                            if (entityA is ITexturedEntity texturedEntity)
                            {
                                texturedEntity.TextureTransformed += (s, args) =>
                                {
                                    meshBatch.TransformTexture(args.ID, entityA.Position, args.Translation, args.Rotation, args.Scale);
                                };
                            }*/

                            _batchIndexByEntityID.Add(entityID, i);
                            return meshBatch;
                        }
                    }
                    else if (_batches[i] is BillboardBatch billboardBatch)
                    {
                        var entityB = _entityProvider.GetEntity(billboardBatch.EntityIDs.First());

                        if (entityA.CompareUniforms(entityB))
                        {
                            entityA.Transformed += (s, args) =>
                            {
                                billboardBatch.Transform(args.ID, args.Transform);
                            };

                            _batchIndexByEntityID.Add(entityID, i);
                            return billboardBatch;
                        }
                    }
                }

                /*foreach (var meshBatch in _batches.OfType<MeshBatch>())
                {
                    var entityB = _entityProvider.GetEntity(meshBatch.EntityIDs.First());

                    if (entityA.CompareUniforms(entityB))
                    {
                        entityA.Transformed += (s, args) =>
                        {
                            meshBatch.Transform(args.ID, args.Transform);
                        };

                        return meshBatch;
                    }
                }*/
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

        public void Load(int entityID) => GetBatch(entityID).Load();

        public void Load()
        {
            foreach (var batch in _batches)
            {
                batch.Load();
            }

            IsLoaded = true;
        }

        public BatchAction CreateBatchAction() => new BatchAction(this);

        private void DrawEntities(ShaderProgram shader, HashSet<int> ids, Action<int> action = null)
        {
            var batchIndices = new HashSet<int>();

            foreach (var id in _entityProvider.EntityRenderIDs)
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

        private void DrawEntities(RenderTypes renderType, ShaderProgram shader, HashSet<int> ids, Action<int> action = null)
        {
            var batchIndices = new HashSet<int>();

            foreach (var id in _entityProvider.EntityRenderIDs)
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
                case RenderTypes.TransparentStatic:
                    return _transparentStaticIDs;
                case RenderTypes.TransparentAnimated:
                    return _transparentAnimatedIDs;
                case RenderTypes.TransparentBillboard:
                    return _transparentBillboardIDs;
            }

            throw new ArgumentOutOfRangeException("Could not handle render type " + renderType);
        }

        private RenderTypes GetRenderTypeForRenderable(IRenderable renderable)
        {
            if (renderable.IsAnimated)
            {
                return renderable.IsTransparent ? RenderTypes.TransparentAnimated : RenderTypes.OpaqueAnimated;
            }
            else if (renderable is TextureID)
            {
                return renderable.IsTransparent ? RenderTypes.TransparentBillboard : RenderTypes.OpaqueBillboard;
            }
            else
            {
                return renderable.IsTransparent ? RenderTypes.TransparentStatic : RenderTypes.OpaqueStatic;
            }
        }

        public class BatchAction
        {
            private BatchManager _batchManager;
            private Queue<Action> _commandQueue = new Queue<Action>();
            private HashSet<int> _entityIDs;

            public BatchAction(BatchManager batchManager) => _batchManager = batchManager;

            public ShaderProgram Shader { get; set; }
            public ICamera Camera { get; set; }

            public BatchAction SetShader(ShaderProgram shader)
            {
                _commandQueue.Enqueue(() =>
                {
                    Shader = shader;
                    Shader.Use();
                });
                return this;
            }

            public BatchAction SetCamera(ICamera camera)
            {
                _commandQueue.Enqueue(() =>
                {
                    Camera = camera;
                    Camera.SetUniforms(Shader);
                });
                return this;
            }

            public BatchAction SetCamera(ICamera camera, PointLight pointLight)
            {
                _commandQueue.Enqueue(() =>
                {
                    Camera = camera;
                    Camera.SetUniforms(Shader, pointLight);
                });
                return this;
            }

            public BatchAction SetCamera(ICamera camera, SpotLight spotLight)
            {
                _commandQueue.Enqueue(() =>
                {
                    Camera = camera;
                    Camera.SetUniforms(Shader, spotLight);
                });
                return this;
            }

            public BatchAction SetUniform<T>(string name, T value) where T : struct
            {
                _commandQueue.Enqueue(() => Shader.SetUniform<T>(name, value));
                return this;
            }

            public BatchAction SetEntityIDs(IEnumerable<int> ids)
            {
                _commandQueue.Enqueue(() => _entityIDs = new HashSet<int>(ids));
                return this;
            }

            public BatchAction ClearEntityIDs()
            {
                _commandQueue.Enqueue(() => _entityIDs = null);
                return this;
            }

            public BatchAction PerformAction(Action action)
            {
                _commandQueue.Enqueue(action);
                return this;
            }

            public BatchAction SetTexture(ITexture texture, string name, int index)
            {
                _commandQueue.Enqueue(() => Shader.BindTexture(texture, name, index));
                return this;
            }

            public BatchAction RenderEntities()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(Shader, _entityIDs));
                return this;
            }

            public BatchAction RenderOpaqueStatic()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.OpaqueStatic, Shader, _entityIDs));
                return this;
            }

            public BatchAction RenderOpaqueAnimated()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.OpaqueAnimated, Shader, _entityIDs));
                return this;
            }

            public BatchAction RenderOpaqueBillboard()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.OpaqueBillboard, Shader, _entityIDs));
                return this;
            }

            public BatchAction RenderTransparentStatic()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.TransparentStatic, Shader, _entityIDs));
                return this;
            }

            public BatchAction RenderTransparentAnimated()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.TransparentAnimated, Shader, _entityIDs));
                return this;
            }

            public BatchAction RenderTransparentBillboard()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.TransparentBillboard, Shader, _entityIDs));
                return this;
            }

            public BatchAction RenderOpaqueStaticWithAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.OpaqueStatic, Shader, _entityIDs, action));
                return this;
            }

            public BatchAction RenderOpaqueAnimatedWithAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.OpaqueAnimated, Shader, _entityIDs, action));
                return this;
            }

            public BatchAction RenderTransparentStaticWithAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.TransparentStatic, Shader, _entityIDs, action));
                return this;
            }

            public BatchAction RenderTransparentAnimatedWithAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(RenderTypes.TransparentAnimated, Shader, _entityIDs, action));
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
