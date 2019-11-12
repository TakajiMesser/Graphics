using SauceEditorCore.Helpers;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Billboards;
using SpiceEngineCore.Rendering.Models;

namespace SauceEditorCore.Models.Entities
{
    public class VertexEntity : ModelEntity<ModelVertex>
    {
        public VertexEntity(ModelVertex meshVertex) : base(meshVertex) { }

        public override IRenderable ToComponent() => new Billboard(Position, FilePathHelper.VERTEX_TEXTURE_PATH);
    }
}
