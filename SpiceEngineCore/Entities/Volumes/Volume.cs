﻿using OpenTK;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Shaders;

namespace SpiceEngineCore.Entities.Volumes
{
    /// <summary>
    /// Brushes are static geometric shapes that are baked into a scene.
    /// Unlike meshes, brushes cannot be deformed.
    /// </summary>
    public class Volume : Entity, IVolume
    {
        public Quaternion Rotation
        {
            get => _modelMatrix.Rotation;
            set => _modelMatrix.Rotation = value;
        }

        public Vector3 Scale
        {
            get => _modelMatrix.Scale;
            set => _modelMatrix.Scale = value;
        }
    }
}
