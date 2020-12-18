using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.Batches
{
    public enum RenderTypes
    {
        OpaqueStatic,
        OpaqueAnimated,
        OpaqueBillboard,
        OpaqueView,
        OpaqueText,
        TransparentStatic,
        TransparentAnimated,
        TransparentBillboard,
        TransparentView,
        TransparentText
    }

    public interface IBatch
    {
        IEnumerable<int> EntityIDs { get; }
        int EntityCount { get; }
        bool IsLoaded { get; }

        void AddEntity(int id, IRenderable renderable);
        void Transform(int entityID, Transform transform);
        void TransformTexture(int entityID, Vector3 center, Vector2 translation, float rotation, Vector2 scale);
        void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate);
        void RemoveEntity(int id);

        void SetUniforms(IEntityProvider entityProvider, ShaderProgram program);
        bool CompareUniforms(IRenderable renderable);
        void BindTextures(ShaderProgram program, ITextureProvider textureProvider);

        void Load();
        void Draw(IEntityProvider entityProvider, ShaderProgram shaderProgram, ITextureProvider textureProvider = null);

        IBatch Duplicate();
    }
}
