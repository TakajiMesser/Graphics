using SpiceEngine.Entities;
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

        public BatchManager(IEntityProvider entityProvider)
        {
            _entityProvider = entityProvider;
        }

        public void DuplicateBatch(int entityID, int newID)
        {
            var batch = GetBatch(entityID);
            var entityType = _entityProvider.GetEntityType(entityID);

            switch (entityType)
            {
                case EntityTypes.Actor:
                    var actorBatch = (ModelBatch)batch;
                    AddActor(newID, actorBatch.Meshes.Select(m => m.Duplicate()));
                    break;
                case EntityTypes.Brush:
                    var brushBatch = (MeshBatch)batch;
                    AddBrush(newID, brushBatch.Mesh.Duplicate());
                    break;
                case EntityTypes.Volume:
                    var volumeBatch = (MeshBatch)batch;
                    AddVolume(newID, volumeBatch.Mesh.Duplicate());
                    break;
                case EntityTypes.Joint:
                    var jointBatch = (ModelBatch)batch;
                    AddJoint(newID, jointBatch.Meshes.Select(m => m.Duplicate()));
                    break;
            }
        }

        public IBatch GetBatch(int entityID)
        {
            if (_batchesByEntityID.ContainsKey(entityID))
            {
                return _batchesByEntityID[entityID];
            }

            throw new KeyNotFoundException("No batch found for entity ID " + entityID);
        }

        public void RemoveByEntityID(int entityID)
        {
            var batch = GetBatch(entityID);
            var entityType = _entityProvider.GetEntityType(entityID);

            switch (entityType)
            {
                case EntityTypes.Actor:
                    _actorBatches.Remove((ModelBatch)batch);
                    break;
                case EntityTypes.Brush:
                    _brushBatches.Remove((MeshBatch)batch);
                    break;
                case EntityTypes.Volume:
                    _volumeBatches.Remove((MeshBatch)batch);
                    break;
                case EntityTypes.Joint:
                    _jointBatches.Remove((ModelBatch)batch);
                    break;
            }

            _batchesByEntityID.Remove(entityID);
        }

        public void AddBrush(int entityID, IMesh3D mesh)
        {
            var batch = new MeshBatch(entityID, mesh);

            if (mesh is Mesh3D mesh3D)
            {
                mesh3D.AlphaChanged += (s, args) =>
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
            }

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

        public void AddVolume(int entityID, IMesh3D mesh)
        {
            var batch = new MeshBatch(entityID, mesh);

            if (mesh is Mesh3D mesh3D)
            {
                mesh3D.AlphaChanged += (s, args) =>
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
            }

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

        public void AddActor(int entityID, IEnumerable<IMesh3D> meshes)
        {
            var batch = new ModelBatch(entityID, meshes);

            foreach (var mesh in meshes)
            {
                if (mesh is Mesh3D mesh3D)
                {
                    mesh3D.AlphaChanged += (s, args) =>
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
            }

            if (meshes.Any(m => m.Alpha < 1.0f))
            {
                _transparentActorIDs.Add(entityID);
            }
            else
            {
                _actorIDs.Add(entityID);
            }

            AddBatch(entityID, batch);
        }

        public void AddJoint(int entityID, IEnumerable<IMesh3D> meshes)
        {
            var batch = new ModelBatch(entityID, meshes);

            foreach (var mesh in meshes)
            {
                if (mesh is Mesh3D mesh3D)
                {
                    mesh3D.AlphaChanged += (s, args) =>
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
            }

            if (meshes.Any(m => m.Alpha < 1.0f))
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

        public void DrawBrushes(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (_brushIDs.Contains(id))
                {
                    _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
                }
            }
        }

        public void DrawBrushes(ShaderProgram shaderProgram, Action<int> batchAction, TextureManager textureManager = null)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (_brushIDs.Contains(id))
                {
                    batchAction(id);
                    _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
                }
            }
        }

        public void DrawVolumes(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (_volumeIDs.Contains(id))
                {
                    _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
                }
            }
        }

        public void DrawVolumes(ShaderProgram shaderProgram, Action<int> batchAction, TextureManager textureManager = null)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (_volumeIDs.Contains(id))
                {
                    batchAction(id);
                    _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
                }
            }
        }

        public void DrawActors(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (_actorIDs.Contains(id))
                {
                    _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
                }
            }
        }

        public void DrawActors(ShaderProgram shaderProgram, Action<int> batchAction, TextureManager textureManager = null)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (_actorIDs.Contains(id))
                {
                    batchAction(id);
                    _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
                }
            }
        }

        public void DrawJoints(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (_jointIDs.Contains(id))
                {
                    _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
                }
            }
        }

        public void DrawJoints(ShaderProgram shaderProgram, Action<int> batchAction, TextureManager textureManager = null)
        {
            foreach (var id in _entityProvider.EntityRenderIDs)
            {
                if (_jointIDs.Contains(id))
                {
                    batchAction(id);
                    _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
                }
            }
        }

        public void DrawTransparencies(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            // TODO - Sort transparent meshes from furthest from camera to closest
            var transparencyIDs = _transparentBrushIDs.Concat(_transparentActorIDs).Concat(_transparentVolumeIDs);

            foreach (var id in transparencyIDs)
            {
                _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        public void DrawTransparentJoints(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            // TODO - Sort transparent meshes from furthest from camera to closest
            var transparencyIDs = _transparentJointIDs;

            foreach (var id in transparencyIDs)
            {
                _batchesByEntityID[id].Draw(_entityProvider, shaderProgram, textureManager);
            }
        }
    }
}
