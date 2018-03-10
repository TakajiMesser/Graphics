using TakoEngine.GameObjects;
using TakoEngine.Helpers;
using TakoEngine.Lighting;
using TakoEngine.Meshes;
using TakoEngine.Outputs;
using TakoEngine.Rendering.Buffers;
using TakoEngine.Rendering.Matrices;
using TakoEngine.Rendering.Shaders;
using TakoEngine.Rendering.Textures;
using TakoEngine.Rendering.Vertices;
using TakoEngine.Utilities;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoEngine.Rendering.Processing
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
