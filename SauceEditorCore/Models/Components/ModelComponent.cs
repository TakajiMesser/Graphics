using SauceEditorCore.Models.Entities;
using SpiceEngine.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Text;

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

        public IEnumerable<ShapeEntity> GetShapeEntities()
        {
            foreach (var meshShape in GetModelShape().Meshes)
            {
                yield return new ShapeEntity(meshShape);
            }
        }

        public IEnumerable<FaceEntity> GetFaceEntities()
        {
            foreach (var meshShape in GetModelShape().Meshes)
            {
                foreach (var meshFace in meshShape.Faces)
                {
                    yield return new FaceEntity(meshFace);
                }
            }
        }

        public IEnumerable<TriangleEntity> GetTriangleEntities()
        {
            foreach (var meshShape in GetModelShape().Meshes)
            {
                foreach (var meshFace in meshShape.Faces)
                {
                    foreach (var meshTriangle in meshFace.GetMeshTriangles())
                    {
                        yield return new TriangleEntity(meshTriangle);
                    }
                }
            }
        }

        public IEnumerable<VertexEntity> GetVertexEntities()
        {
            foreach (var meshShape in GetModelShape().Meshes)
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
