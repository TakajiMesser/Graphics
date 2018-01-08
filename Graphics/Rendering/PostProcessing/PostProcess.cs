using Graphics.Outputs;
using Graphics.Rendering.Buffers;
using Graphics.Rendering.Shaders;
using Graphics.Rendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Rendering.PostProcessing
{
    public abstract class PostProcess
    {
        public string Name { get; private set; }
        public bool Enabled { get; set; } = true;
        public Resolution Resolution { get; set; }

        public Texture FinalTexture { get; protected set; }

        //protected ShaderProgram _program;
        protected FrameBuffer _frameBuffer = new FrameBuffer();

        public PostProcess(string name, Resolution resolution)
        {
            Name = name;
            Resolution = resolution;
        }

        public void Load()
        {
            LoadProgram();
            LoadBuffers();
        }

        protected abstract void LoadProgram();
        protected abstract void LoadBuffers();

        //public abstract void Render();
        //public abstract void Render();
    }
}
