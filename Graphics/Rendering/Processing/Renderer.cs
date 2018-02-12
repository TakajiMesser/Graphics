using Graphics.GameObjects;
using Graphics.Helpers;
using Graphics.Lighting;
using Graphics.Meshes;
using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Matrices;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using Graphics.Rendering.Vertices;
using Graphics.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.Processing
{
    public abstract class Renderer
    {
        public void Load(Resolution resolution)
        {
            LoadPrograms();
            LoadTextures(resolution);
            LoadBuffers();
        }

        protected abstract void LoadPrograms();
        protected abstract void LoadTextures(Resolution resolution);
        protected abstract void LoadBuffers();

        public abstract void ResizeTextures(Resolution resolution);
    }
}
