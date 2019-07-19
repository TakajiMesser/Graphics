using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Brushes;
using SpiceEngine.Entities.Volumes;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Batches
{
    public class MeshBatch : Batch
    {
        private Dictionary<int, int> _offsetByID = new Dictionary<int, int>();
        private Dictionary<int, int> _countByID = new Dictionary<int, int>();

        public IMesh Mesh { get; }

        public MeshBatch(IMesh mesh) => Mesh = mesh;

        public override IBatch Duplicate() => new MeshBatch(Mesh.Duplicate());

        public override void AddEntity(int id, IRenderable renderable)
        {
            if (renderable is IMesh mesh)
            {
                if (EntityIDs.Any())
                {
                    var offset = Mesh.Vertices.Count();
                    _offsetByID.Add(id, offset);
                    _countByID.Add(id, offset + mesh.Vertices.Count());

                    Mesh.Combine(mesh);
                }
                else
                {
                    _offsetByID.Add(id, 0);
                    _countByID.Add(id, mesh.Vertices.Count());
                }
            }

            base.AddEntity(id, renderable);
        }

        public override void Transform(int entityID, Matrix4 matrix)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : Mesh.Vertices.Count();

            Mesh.Transform(matrix, offset, count);
        }

        public override void TransformTexture(int entityID, Vector2 translation, float rotation, Vector2 scale)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : Mesh.Vertices.Count();

            Mesh.TransformTexture(translation, rotation, scale, offset, count);
        }

        public override void UpdateVertices(int entityID, Func<IVertex3D, IVertex3D> vertexUpdate)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : Mesh.Vertices.Count();

            Mesh.Update(vertexUpdate, offset, count);
        }

        public void AddTestColors()
        {
            /*var vertices = new List<Vertex3D>();

            for (var i = 0; i < Mesh.Vertices.Count; i++)
            {
                if (i % 3 == 0)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Lime));
                }
                else if (i % 3 == 1)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Red));
                }
                else if (i % 3 == 2)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Blue));
                }
            }

            Mesh.ClearVertices();
            Mesh.AddVertices(vertices);*/
        }

        public override void Load() => Mesh.Load();

        public override void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null)
        {
            var entity = entityProvider.GetEntity(EntityIDs.First());

            if (textureProvider != null && entity is ITextureBinder textureBinder)
            {
                // TODO - Determine when to unbind textures
                textureBinder.BindTextures(shaderProgram, textureProvider);
            }

            entity.SetUniforms(shaderProgram);

            Mesh.Draw();
        }
    }
}
