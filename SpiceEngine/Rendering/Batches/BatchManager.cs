﻿using SpiceEngine.Entities;
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

        private List<MeshBatch> _brushBatches = new List<MeshBatch>();
        private List<MeshBatch> _volumeBatches = new List<MeshBatch>();
        private List<ModelBatch> _actorBatches = new List<ModelBatch>();
        private List<ModelBatch> _jointBatches = new List<ModelBatch>();

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

            _brushBatches.Add(batch);
            _batchesByEntityID.Add(entityID, batch);

            if (IsLoaded)
            {
                batch.Load();
            }
        }

        public void AddVolume(int entityID, IMesh3D mesh)
        {
            var batch = new MeshBatch(entityID, mesh);

            _volumeBatches.Add(batch);
            _batchesByEntityID.Add(entityID, batch);

            if (IsLoaded)
            {
                batch.Load();
            }
        }

        public void AddActor(int entityID, IEnumerable<IMesh3D> meshes)
        {
            var batch = new ModelBatch(entityID, meshes);

            _actorBatches.Add(batch);
            _batchesByEntityID.Add(entityID, batch);

            if (IsLoaded)
            {
                batch.Load();
            }
        }

        public void AddJoint(int entityID, IEnumerable<IMesh3D> meshes)
        {
            var batch = new ModelBatch(entityID, meshes);

            _jointBatches.Add(batch);
            _batchesByEntityID.Add(entityID, batch);

            if (IsLoaded)
            {
                batch.Load();
            }
        }

        public void Load(int entityID) => _batchesByEntityID[entityID].Load();

        public void Load()
        {
            foreach (var batch in _brushBatches)
            {
                batch.Load();
            }

            foreach (var batch in _volumeBatches)
            {
                batch.Load();
            }

            foreach (var batch in _actorBatches)
            {
                batch.Load();
            }

            foreach (var batch in _jointBatches)
            {
                batch.Load();
            }

            IsLoaded = true;
        }

        public void DrawBrushes(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            foreach (var batch in _brushBatches)
            {
                batch.Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        public void DrawBrushes(ShaderProgram shaderProgram, Action<int> batchAction, TextureManager textureManager = null)
        {
            foreach (var batch in _brushBatches)
            {
                batchAction(batch.EntityID);
                batch.Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        public void DrawVolumes(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            foreach (var batch in _volumeBatches)
            {
                batch.Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        public void DrawVolumes(ShaderProgram shaderProgram, Action<int> batchAction, TextureManager textureManager = null)
        {
            foreach (var batch in _volumeBatches)
            {
                batchAction(batch.EntityID);
                batch.Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        public void DrawActors(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            foreach (var batch in _actorBatches)
            {
                batch.Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        public void DrawActors(ShaderProgram shaderProgram, Action<int> batchAction, TextureManager textureManager = null)
        {
            foreach (var batch in _actorBatches)
            {
                batchAction(batch.EntityID);
                batch.Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        public void DrawJoints(ShaderProgram shaderProgram, TextureManager textureManager = null)
        {
            foreach (var batch in _jointBatches)
            {
                batch.Draw(_entityProvider, shaderProgram, textureManager);
            }
        }

        public void DrawJoints(ShaderProgram shaderProgram, Action<int> batchAction, TextureManager textureManager = null)
        {
            foreach (var batch in _jointBatches)
            {
                batchAction(batch.EntityID);
                batch.Draw(_entityProvider, shaderProgram, textureManager);
            }
        }
    }
}
