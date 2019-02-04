using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using OpenTK.Graphics.OpenGL;

namespace SpiceEngine.Rendering.Meshes
{
    public interface IMesh3D
    {
        IEnumerable<IVertex3D> Vertices { get; }
        void Load();
        void Draw();
        IMesh3D Duplicate();
    }
}
