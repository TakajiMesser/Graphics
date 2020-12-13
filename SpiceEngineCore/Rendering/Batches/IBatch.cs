using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
        IRenderable Renderable { get; }
        IEnumerable<int> EntityIDs { get; }
        int EntityCount { get; }
        bool IsLoaded { get; }

        void AddEntity(int id, IRenderable renderable);
        void Transform(int entityID, Transform transform);
        void TransformTexture(int entityID, Vector3 center, Vector2 translation, float rotation, Vector2 scale);
        void UpdateVertices(int entityID, Func<IVertex, IVertex> vertexUpdate);
        void RemoveEntity(int id);
        
        bool CanBatch(IRenderable renderable);

        void Load();
        void Draw();

        IEnumerable<IUniform> GetUniforms(IBatcher batcher);
        IEnumerable<TextureBinding> GetTextureBindings(ITextureProvider textureProvider);

        /*void SetUniforms(IRender renderer, IEntityProvider entityProvider);
        void BindTextures(IRender renderer, ITextureProvider textureProvider);
        void Draw(IRender renderer, IEntityProvider entityProvider, ITextureProvider textureProvider = null);*/

        IBatch Duplicate();
    }
}
