using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Textures
{
    public interface ITexturePather
    {
        List<TexturePaths> TexturesPaths { get; }
    }
}
