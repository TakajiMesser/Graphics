using OpenTK;
using SpiceEngine.Entities;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Batches
{
    public interface IBatch
    {
        IEnumerable<int> EntityIDs { get; }

        void AddEntity(int id, IRenderable renderable);
        void Transform(int entityID, Matrix4 matrix);
        void RemoveEntity(int id);

        void Load();
        void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null);

        IBatch Duplicate();
    }
}
