using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Models;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;

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

        public BatchManager(IEntityProvider entityProvider)
        {
            _entityProvider = entityProvider;
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
            
            switch (batch)
            {
                case MeshBatch meshBatch:
                    _brushBatches.Remove(meshBatch);
                    _volumeBatches.Remove(meshBatch);
                    break;
                case ModelBatch modelBatch:
                    _actorBatches.Remove(modelBatch);
                    _jointBatches.Remove(modelBatch);
                    break;
            }
        }

        public void AddBrush(int entityID, IMesh3D mesh)
        {
            var batch = new MeshBatch(entityID, mesh);

            _brushBatches.Add(batch);
            _batchesByEntityID.Add(entityID, batch);
        }

        public void AddVolume(int entityID, IMesh3D mesh)
        {
            var batch = new MeshBatch(entityID, mesh);

            _volumeBatches.Add(batch);
            _batchesByEntityID.Add(entityID, batch);
        }

        public void AddActor(int entityID, IEnumerable<IMesh3D> meshes)
        {
            var batch = new ModelBatch(entityID, meshes);

            _actorBatches.Add(batch);
            _batchesByEntityID.Add(entityID, batch);
        }

        public void AddJoint(int entityID, IEnumerable<IMesh3D> meshes)
        {
            var batch = new ModelBatch(entityID, meshes);

            _jointBatches.Add(batch);
            _batchesByEntityID.Add(entityID, batch);
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
