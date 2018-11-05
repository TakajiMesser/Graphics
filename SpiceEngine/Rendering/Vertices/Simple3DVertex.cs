﻿using OpenTK;
using System.Runtime.InteropServices;

namespace SpiceEngine.Rendering.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Simple3DVertex : IVertex3D
    {
        public Vector3 Position { get; private set; }

        public Simple3DVertex(Vector3 position)
        {
            Position = position;
        }

        public Simple3DVertex(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }
    }
}