using SauceEditorCore.Models.Entities;
using SpiceEngine.Rendering.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditorCore.Models.Components
{
    public class ModelComponent : Component
    {
        private EditorModel _modelShape;

        public ModelComponent(string filePath) : base(filePath) { }

        public Model Model { get; set; }

        private EditorModel GetModelShape()
        {
            // Lazy load model shape
            if (_modelShape == null)
            {
                _modelShape = EditorModel.LoadFromFile(Path);
            }

            return _modelShape;
        }

        public IEnumerable<MeshEntity> GetMeshEntities()
        {
            var modelMeshes = GetModelShape().Meshes;//.Select(m => m.Duplicated()).ToList();
            var texturePaths = Model.GetTexturePaths(Path).ToList();

            for (var i = 0; i < modelMeshes.Count; i++)
            {
                yield return new MeshEntity(modelMeshes[i], texturePaths[i]);
            }
        }

        public IEnumerable<FaceEntity> GetFaceEntities()
        {
            var modelMeshes = GetModelShape().Meshes;//.Select(m => m.Duplicated()).ToList();
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
            var modelMeshes = GetModelShape().Meshes;//.Select(m => m.Duplicated()).ToList();
            var texturePaths = Model.GetTexturePaths(Path).ToList();

            for (var i = 0; i < modelMeshes.Count; i++)
            {
                foreach (var modelFace in modelMeshes[i].Faces)
                {
                    foreach (var modelTriangle in modelFace.GetTriangles())
                    {
                        yield return new TriangleEntity(modelTriangle, texturePaths[i]);
                    }
                }
            }
        }

        public IEnumerable<VertexEntity> GetVertexEntities()
        {
            foreach (var modelMesh in GetModelShape().Meshes)//.Select(m => m.Duplicated()))
            {
                foreach (var modelFace in modelMesh.Faces)
                {
                    foreach (var modelTriangle in modelFace.GetTriangles())
                    {
                        yield return new VertexEntity(modelTriangle.VertexA);
                        yield return new VertexEntity(modelTriangle.VertexB);
                        yield return new VertexEntity(modelTriangle.VertexC);
                    }
                }
            }
        }

        public override void Save() => throw new NotImplementedException();
        public override void Load() => Model = new Model(Path);

        public static bool IsValidExtension(string extension) => extension == ".obj";
    }
}
