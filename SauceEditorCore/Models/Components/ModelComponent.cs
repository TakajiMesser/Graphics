using SauceEditorCore.Models.Entities;
using SpiceEngine.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditorCore.Models.Components
{
    public class ModelComponent : Component
    {
        private ModelShape _modelShape;

        public ModelComponent(string filePath) : base(filePath) { }

        public Model Model { get; set; }

        private ModelShape GetModelShape()
        {
            // Lazy load model shape
            if (_modelShape == null)
            {
                _modelShape = ModelShape.LoadFromFile(Path);
            }

            return _modelShape;
        }

        public IEnumerable<MeshEntity> GetMeshEntities()
        {
            var modelMeshes = GetModelShape().Meshes.Select(m => m.Duplicated()).ToList();
            var texturePaths = Model.GetTexturePaths(Path).ToList();

            for (var i = 0; i < modelMeshes.Count; i++)
            {
                yield return new MeshEntity(modelMeshes[i], texturePaths[i]);
            }
        }

        public IEnumerable<FaceEntity> GetFaceEntities()
        {
            var modelMeshes = GetModelShape().Meshes.Select(m => m.Duplicated()).ToList();
            var texturePaths = Model.GetTexturePaths(Path).ToList();

            for (var i = 0; i < modelMeshes.Count; i++)
            {
                foreach (var modelFace in modelMeshes[i].Faces)
                {
                    yield return new FaceEntity(modelFace, texturePaths[i]);
                }
            }
        }

        public IEnumerable<TriangleEntity> GetTriangleEntities()
        {
            var meshShapes = GetModelShape().Meshes.Select(m => m.Duplicated()).ToList();
            var texturePaths = Model.GetTexturePaths(Path).ToList();

            for (var i = 0; i < meshShapes.Count; i++)
            {
                foreach (var meshFace in meshShapes[i].Faces)
                {
                    foreach (var meshTriangle in meshFace.GetMeshTriangles())
                    {
                        yield return new TriangleEntity(meshTriangle, texturePaths[i]);
                    }
                }
            }
        }

        public IEnumerable<VertexEntity> GetVertexEntities()
        {
            foreach (var meshShape in GetModelShape().Meshes.Select(m => m.Duplicated()))
            {
                foreach (var meshFace in meshShape.Faces)
                {
                    foreach (var meshTriangle in meshFace.GetMeshTriangles())
                    {
                        yield return new VertexEntity(meshTriangle.VertexA);
                        yield return new VertexEntity(meshTriangle.VertexB);
                        yield return new VertexEntity(meshTriangle.VertexC);
                    }
                }
            }
        }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => Model = new Model(Path);
    }
}
