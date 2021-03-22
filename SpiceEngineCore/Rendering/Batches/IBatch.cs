using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

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

        bool CanBatch(IRenderable renderable);

        void Load(IRenderContextProvider contextProvider);
        void Draw(IShader shader, IEntityProvider entityProvider, ITextureProvider textureProvider = null);

        IBatch Duplicate();
    }
}
