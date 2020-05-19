using SauceEditorCore.Helpers;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Rendering.Billboards;
using SweetGraphicsCore.Rendering.Models;

namespace SauceEditorCore.Models.Entities
{
    public class VertexEntity : ModelEntity<ModelVertex>
    {
        public VertexEntity(ModelVertex meshVertex) : base(meshVertex) { }

        public override IRenderable ToComponent() => new Billboard(Position, FilePathHelper.VERTEX_TEXTURE_PATH);
    }
}
