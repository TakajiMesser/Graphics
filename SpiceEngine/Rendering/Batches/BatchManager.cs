using SpiceEngine.Entities;
using SpiceEngine.Entities.Actors;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Batches
{
    public class BatchManager
    {
        private IEntityProvider _entityProvider;

        private HashSet<int> _brushIDs = new HashSet<int>();
        private HashSet<int> _volumeIDs = new HashSet<int>();
        private HashSet<int> _actorIDs = new HashSet<int>();
        private HashSet<int> _jointIDs = new HashSet<int>();

        private HashSet<int> _transparentBrushIDs = new HashSet<int>();
        private HashSet<int> _transparentVolumeIDs = new HashSet<int>();
        private HashSet<int> _transparentActorIDs = new HashSet<int>();
        private HashSet<int> _transparentJointIDs = new HashSet<int>();

        private Dictionary<int, IBatch> _batchesByEntityID = new Dictionary<int, IBatch>();

        public bool IsLoaded { get; private set; } = false;

        public BatchManager(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public void DuplicateBatch(int entityID, int newID)
        {
            var batch = GetBatch(entityID);
            var entityType = _entityProvider.GetEntityType(entityID);

            switch (entityType)
            {
                case EntityTypes.Actor:
                    var actorBatch = (ModelBatch)batch;
                    var actor = _entityProvider.GetEntity(entityID) as Actor;
                    AddActor(newID, actorBatch.Model.Duplicate(), actor.TextureMappings);
                    break;
                case EntityTypes.Brush:
                    var brushBatch = (MeshBatch)batch;
                    var brush = _entityProvider.GetEntity(entityID) as Brush;
                    AddBrush(newID, brushBatch.Mesh.Duplicate(), brush.TextureMapping);
                    break;
                case EntityTypes.Volume:
                    var volumeBatch = (MeshBatch)batch;
                    AddVolume(newID, volumeBatch.Mesh.Duplicate());
                    break;
                case EntityTypes.Joint:
                    var jointBatch = (ModelBatch)batch;
                    var jointActor = _entityProvider.GetEntity(entityID) as Actor;
                    AddJoint(newID, jointBatch.Model.Duplicate(), jointActor.TextureMappings);
                    break;
            }
        }

        public IBatch GetBatch(int entityID)
        {
            if (!_batchesByEntityID.ContainsKey(entityID)) throw new KeyNotFoundException("No batch found for entity ID " + entityID);
            return _batchesByEntityID[entityID];
        }

        public void RemoveByEntityID(int entityID)
        {
            var batch = GetBatch(entityID);
            var entityType = _entityProvider.GetEntityType(entityID);

            switch (entityType)
            {
                case EntityTypes.Actor:
                    _actorIDs.Remove(entityID);
                    break;
                case EntityTypes.Brush:
                    _brushIDs.Remove(entityID);
                    break;
                case EntityTypes.Volume:
                    _volumeIDs.Remove(entityID);
                    break;
                case EntityTypes.Joint:
                    _jointIDs.Remove(entityID);
                    break;
            }

            _batchesByEntityID.Remove(entityID);
        }

        public void AddBrush(int entityID, IMesh mesh, TextureMapping? textureMapping)
        {
            var batch = new MeshBatch(entityID, mesh);

            var brush = _entityProvider.GetEntity(entityID) as Brush;
            brush.TextureMapping = textureMapping;

            mesh.AlphaChanged += (s, args) =>
            {
                if (!args.WasTransparent && args.IsTransparent)
                {
                    _brushIDs.Remove(entityID);
                    _transparentBrushIDs.Add(entityID);
                }
                else if (args.WasTransparent && !args.IsTransparent)
                {
                    _transparentBrushIDs.Remove(entityID);
                    _brushIDs.Add(entityID);
                }
            };

            if (mesh.Alpha < 1.0f)
            {
                _transparentBrushIDs.Add(entityID);
            }
            else
            {
                _brushIDs.Add(entityID);
            }

            AddBatch(entityID, batch);
        }

        public void AddVolume(int entityID, IMesh mesh)
        {
            var batch = new MeshBatch(entityID, mesh);

            mesh.AlphaChanged += (s, args) =>
            {
                if (!args.WasTransparent && args.IsTransparent)
                {
                    _volumeIDs.Remove(entityID);
                    _transparentVolumeIDs.Add(entityID);
                }
                else if (args.WasTransparent && !args.IsTransparent)
                {
                    _transparentVolumeIDs.Remove(entityID);
                    _volumeIDs.Add(entityID);
                }
            };

            if (mesh.Alpha < 1.0f)
            {
                _transparentVolumeIDs.Add(entityID);
            }
            else
            {
                _volumeIDs.Add(entityID);
            }

            AddBatch(entityID, batch);
        }

        public void AddActor(int entityID, Model model, IEnumerable<TextureMapping?> textureMappings)
        {
            var batch = new ModelBatch(entityID, model);

            var actor = _entityProvider.GetEntity(entityID) as Actor;
            actor.TextureMappings.AddRange(textureMappings);

            foreach (var mesh in model.Meshes)
            {
                mesh.AlphaChanged += (s, args) =>
                {
                    if (!args.WasTransparent && args.IsTransparent)
                    {
                        _actorIDs.Remove(entityID);
                        _transparentActorIDs.Add(entityID);
                    }
                    else if (args.WasTransparent && !args.IsTransparent)
                    {
                        _transparentActorIDs.Remove(entityID);
                        _actorIDs.Add(entityID);
                    }
                };
            }

            if (model.Meshes.Any(m => m.Alpha < 1.0f))
            {
                _transparentActorIDs.Add(entityID);
            }
            else
            {
                _actorIDs.Add(entityID);
            }

            AddBatch(entityID, batch);
        }

        public void AddJoint(int entityID, Model model, IEnumerable<TextureMapping?> textureMappings)
        {
            var batch = new ModelBatch(entityID, model);

            var actor = _entityProvider.GetEntity(entityID) as Actor;
            actor.TextureMappings.AddRange(textureMappings);

            foreach (var mesh in model.Meshes)
            {
                mesh.AlphaChanged += (s, args) =>
                {
                    if (!args.WasTransparent && args.IsTransparent)
                    {
                        _jointIDs.Remove(entityID);
                        _transparentJointIDs.Add(entityID);
                    }
                    else if (args.WasTransparent && !args.IsTransparent)
                    {
                        _transparentJointIDs.Remove(entityID);
                        _jointIDs.Add(entityID);
                    }
                };
            }

            if (model.Meshes.Any(m => m.Alpha < 1.0f))
            {
                _transparentJointIDs.Add(entityID);
            }
            else
            {
                _jointIDs.Add(entityID);
            }

            AddBatch(entityID, batch);
        }

        private void AddBatch(int entityID, IBatch batch)
        {
            _batchesByEntityID.Add(entityID, batch);

            if (IsLoaded)
            {
                batch.Load();
            }
        }

        public void Load(int entityID) => _batchesByEntityID[entityID].Load();

        public void Load()
        {
            foreach (var batch in _batchesByEntityID.Values)
            {
                batch.Load();
            }

            IsLoaded = true;
        }

        public BatchAction CreateBatchAction() => new BatchAction(this);

        private void DrawEntities(EntityTypes entityType, ShaderProgram shader, TextureManager textureManager)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (GetEntityIDSet(entityType).Contains(id))
                {
                    _batchesByEntityID[id].Draw(_entityProvider, shader, textureManager);
                }
            }
        }

        private void DrawEntitiesWithAction(EntityTypes entityType, Action<int> action, ShaderProgram shader, TextureManager textureManager)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (GetEntityIDSet(entityType).Contains(id))
                {
                    action(id);
                    _batchesByEntityID[id].Draw(_entityProvider, shader, textureManager);
                }
            }
        }

        private void DrawTransparencies(ShaderProgram shaderProgram, TextureManager textureManager)
        {
            // TODO - Sort transparent meshes from furthest from camera to closest
            var transparencyIDs = _transparentBrushIDs.Concat(_transparentActorIDs).Concat(_transparentVolumeIDs);

            foreach (var id in transparencyIDs)
            {
                _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        private void DrawTransparentJoints(ShaderProgram shaderProgram, TextureManager textureManager)
        {
            // TODO - Sort transparent meshes from furthest from camera to closest
            var transparencyIDs = _transparentJointIDs;

            foreach (var id in transparencyIDs)
            {
                _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        private HashSet<int> GetEntityIDSet(EntityTypes entityType)
        {
            switch (entityType)
            {
                case EntityTypes.Brush:
                    return _brushIDs;
                case EntityTypes.Volume:
                    return _volumeIDs;
                case EntityTypes.Actor:
                    return _actorIDs;
                case EntityTypes.Joint:
                    return _jointIDs;
            }

            throw new ArgumentOutOfRangeException("Could not handle entity type " + entityType);
        }

        public class BatchAction
        {
            private BatchManager _batchManager;
            private Queue<Action> _commandQueue = new Queue<Action>();

            public BatchAction(BatchManager batchManager) => _batchManager = batchManager;

            public ShaderProgram Shader { get; set; }
            public Camera Camera { get; set; }
            public TextureManager TextureManager { get; set; }

            public BatchAction SetShader(ShaderProgram shader)
            {
                _commandQueue.Enqueue(() =>
                {
                    Shader = shader;
                    Shader.Use();
                });
                return this;
            }

            public BatchAction SetCamera(Camera camera)
            {
                _commandQueue.Enqueue(() =>
                {
                    Camera = camera;
                    Camera.SetUniforms(Shader);
                });
                return this;
            }

            public BatchAction SetCamera(Camera camera, PointLight pointLight)
            {
                _commandQueue.Enqueue(() =>
                {
                    Camera = camera;
                    Camera.SetUniforms(Shader, pointLight);
                });
                return this;
            }

            public BatchAction SetCamera(Camera camera, SpotLight spotLight)
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

            public BatchAction PerformAction(Action action)
            {
                _commandQueue.Enqueue(action);
                return this;
            }

            public BatchAction SetTextureManager(TextureManager textureManager)
            {
                _commandQueue.Enqueue(() => TextureManager = textureManager);
                return this;
            }

            public BatchAction RenderBrushes()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(EntityTypes.Brush, Shader, TextureManager));
                return this;
            }

            public BatchAction RenderBrushesWithAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntitiesWithAction(EntityTypes.Brush, action, Shader, TextureManager));
                return this;
            }

            public BatchAction RenderVolumes()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(EntityTypes.Volume, Shader, TextureManager));
                return this;
            }

            public BatchAction RenderVolumesWithAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntitiesWithAction(EntityTypes.Volume, action, Shader, TextureManager));
                return this;
            }

            public BatchAction RenderActors()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(EntityTypes.Actor, Shader, TextureManager));
                return this;
            }

            public BatchAction RenderActorsWithAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntitiesWithAction(EntityTypes.Actor, action, Shader, TextureManager));
                return this;
            }

            public BatchAction RenderJoints()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntities(EntityTypes.Joint, Shader, TextureManager));
                return this;
            }

            public BatchAction RenderJointsWithAction(Action<int> action)
            {
                _commandQueue.Enqueue(() => _batchManager.DrawEntitiesWithAction(EntityTypes.Joint, action, Shader, TextureManager));
                return this;
            }

            public BatchAction RenderTransparencies()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawTransparencies(Shader, TextureManager));
                return this;
            }

            public BatchAction RenderTransparentJoints()
            {
                _commandQueue.Enqueue(() => _batchManager.DrawTransparentJoints(Shader, TextureManager));
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
