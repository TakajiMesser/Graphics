using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Batches;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SweetGraphicsCore.Rendering.Billboards;
using System.Collections.Generic;
using System.Linq;

namespace SweetGraphicsCore.Rendering.Batches
{
    public class BillboardBatch : Batch<IBillboard>
    {
        private Dictionary<int, int> _offsetByID = new Dictionary<int, int>();
        private Dictionary<int, int> _countByID = new Dictionary<int, int>();

        public BillboardBatch(IBillboard billboard) : base(billboard) { }

        public override IBatch Duplicate() => new BillboardBatch(_renderable);

        public override void AddEntity(int id, IRenderable renderable)
        {
            if (renderable is IBillboard billboard)
            {
                if (EntityIDs.Any())
                {
                    var offset = _renderable.Vertices.Count();
                    _offsetByID.Add(id, offset);
                    _countByID.Add(id, offset + billboard.Vertices.Count());

                    _renderable.Combine(billboard);
                }
                else
                {
                    _offsetByID.Add(id, 0);
                    _countByID.Add(id, billboard.Vertices.Count());
                }
            }

            base.AddEntity(id, renderable);
        }

        public override bool CanBatch(IRenderable renderable) => renderable is IBillboard billboard && _renderable.TextureIndex == billboard.TextureIndex;

        public override void SetUniforms(IEntityProvider entityProvider, ShaderProgram shaderProgram) { }

        public override void Transform(int entityID, Transform transform)
        {
            // TODO - This is redundant with function overloads for Mesh.Transform()
            var offset = _offsetByID.ContainsKey(entityID) ? _offsetByID[entityID] : 0;
            var count = _countByID.ContainsKey(entityID) ? _countByID[entityID] : _renderable.Vertices.Count();

            _renderable.Transform(transform, offset, count);
        }

        public override void BindTextures(ShaderProgram shaderProgram, ITextureProvider textureProvider)
        {
            // TODO - Also set texture Alpha value here
            var texture = textureProvider.RetrieveTexture(_renderable.TextureIndex);
            shaderProgram.BindTexture(texture, "mainTexture", 0);
        }
    }
}
