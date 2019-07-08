﻿using OpenTK;

namespace SpiceEngine.Rendering.Vertices
{
    public interface IAnimatedVertex : IVertex
    {
        Vector4 BoneIDs { get; }
        Vector4 BoneWeights { get; }
    }
}
