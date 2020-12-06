using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Geometry.Matrices;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using SweetGraphicsCore.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SweetGraphicsCore.Rendering.Batches
{
    public class MeshBatch : Batch<IMesh>
    {
        private Dictionary<int, int> _offsetByID = new Dictionary<int, int>();
        private Dictionary<int, int> _countByID = new Dictionary<int, int>();

        public MeshBatch(IMesh mesh) : base(mesh) { }

        public override IBatch Duplicate() => new MeshBatch(_renderable.Duplicate());

        public override void AddEntity(int id, IRenderable renderable)
        {
            if (renderable is IMesh mesh)
            {
                if (EntityIDs.Any())
                {
                    var offset = _renderable.Vertices.Count();
                    _offsetByID.Add(id, offset);
                    _countByID.Add(id, offset + mesh.Vertices.Count());

                    _renderable.Combine(mesh);
                }
                else
                {
                    _offsetByID.Add(id, 0);
                    _countByID.Add(id, mesh.Vertices.Count());
                }
            }

            base.AddEntity(id, renderable);
        }

        public override void Transform(int entityID, Transform transform)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : _renderable.Vertices.Count();

            _renderable.Transform(transform, offset, count);
        }

        public override void TransformTexture(int entityID, Vector3 center, Vector2 translation, float rotation, Vector2 scale)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : _renderable.Vertices.Count();

            _renderable.TransformTexture(center, translation, rotation, scale, offset, count);
        }

        public override void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : _renderable.Vertices.Count();

            _renderable.Update(vertexUpdate, offset, count);
        }

        /*public void AddTestColors()
        {
            var vertices = new List<Vertex3D>();

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
            Mesh.AddVertices(vertices);
        }*/

        public override bool CanBatch(IRenderable renderable)
        {
            if (renderable is IMesh)
            {
                if (_renderable is ITexturedMesh texturedRenderable)
                {
                    if (renderable is ITexturedMesh texturedMesh)
                    {
                        return texturedRenderable.Material.Equals(texturedMesh.Material)
                            && texturedRenderable.TextureMapping.Equals(texturedMesh.TextureMapping);
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (_renderable is IColoredMesh coloredRenderable)
                {
                    if (renderable is IColoredMesh coloredMesh)
                    {
                        return coloredRenderable.Color.Equals(coloredMesh.Color);
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;

            /*public override bool CompareUniforms(IEntity entity) =>
                entity is ITextureBinder textureBinder
                && CurrentMaterial.Equals(textureBinder.CurrentMaterial)
                && TextureMappings.Equals(textureBinder.TextureMappings);*/
        }

        public override IEnumerable<IUniform> GetUniforms(IBatcher batcher)
        {
            var entity = batcher.GetEntitiesForBatch(this).First();

            // TODO - This is janky to set this uniform based on entity type...
            if (entity is IBrush)
            {
                yield return new Uniform<Matrix4>(ModelMatrix.CURRENT_NAME, Matrix4.Identity);
                yield return new Uniform<Matrix4>(ModelMatrix.PREVIOUS_NAME, Matrix4.Identity);
            }
            else
            {
                yield return new Uniform<Matrix4>(ModelMatrix.CURRENT_NAME, entity.WorldMatrix.CurrentValue);
                yield return new Uniform<Matrix4>(ModelMatrix.PREVIOUS_NAME, entity.WorldMatrix.PreviousValue);
            }

            if (_renderable is ITexturedMesh texturedMesh)
            {
                yield return new Uniform<Vector3>(Material.AMBIENT_NAME, texturedMesh.Material.Ambient);
                yield return new Uniform<Vector3>(Material.DIFFUSE_NAME, texturedMesh.Material.Diffuse);
                yield return new Uniform<Vector3>(Material.SPECULAR_NAME, texturedMesh.Material.Specular);
                yield return new Uniform<float>(Material.SPECULAR_EXPONENT_NAME, texturedMesh.Material.SpecularExponent);
            }
        }

        public override IEnumerable<TextureBinding> GetTextureBindings(ITextureProvider textureProvider)
        {
            if (_renderable is ITexturedMesh texturedMesh && texturedMesh.TextureMapping.HasValue)
            {
                var diffuseTexture = textureProvider.RetrieveTexture(texturedMesh.TextureMapping.Value.DiffuseIndex);
                yield return new TextureBinding(TextureMapping.DIFFUSE_NAME, diffuseTexture);

                var normalTexture = textureProvider.RetrieveTexture(texturedMesh.TextureMapping.Value.NormalIndex);
                yield return new TextureBinding(TextureMapping.NORMAL_NAME, normalTexture);

                var specularTexture = textureProvider.RetrieveTexture(texturedMesh.TextureMapping.Value.SpecularIndex);
                yield return new TextureBinding(TextureMapping.SPECULAR_NAME, specularTexture);

                var parallaxTexture = textureProvider.RetrieveTexture(texturedMesh.TextureMapping.Value.ParallaxIndex);
                yield return new TextureBinding(TextureMapping.PARALLAX_NAME, parallaxTexture);
            }
        }
    }
}
