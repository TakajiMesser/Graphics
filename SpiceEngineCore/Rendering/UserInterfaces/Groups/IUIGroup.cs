using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.UserInterfaces.Groups
{
    public interface IUIGroup : IRenderable
    {
        IEnumerable<ViewVertex> Vertices { get; }
        IEnumerable<int> TriangleIndices { get; }
        float Alpha { get; set; }

        void Combine(IUIGroup group);

        void Transform(Transform transform);
        void Transform(Transform transform, int offset, int count);

        void Update(Func<IVertex, IVertex> vertexUpdate);
        void Update(Func<IVertex, IVertex> vertexUpdate, int offset, int count);

        IUIGroup Duplicate();
    }
}
