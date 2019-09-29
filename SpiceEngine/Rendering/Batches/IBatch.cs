using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Batches
{
    public interface IBatch
    {
        IEnumerable<int> EntityIDs { get; }

        void AddEntity(int id, IRenderable renderable);
        void Transform(int entityID, Transform transform);
        void TransformTexture(int entityID, Vector3 center, Vector2 translation, float rotation, Vector2 scale);
        void UpdateVertices(int entityID, Func<IVertex3D, IVertex3D> vertexUpdate);
        void RemoveEntity(int id);

        void Load();
        void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null);

        IBatch Duplicate();
    }
}
