using SpiceEngineCore.Rendering;

namespace SweetGraphicsCore.Renderers.PreProcessing
{
    public abstract class PreProcess
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public Resolution Resolution { get; set; }

        public PreProcess(string name, Resolution resolution)
        {
            Name = name;
            Resolution = resolution;
        }

        public void Load()
        {
            LoadShaders();
            LoadBuffers();
        }

        protected abstract void LoadShaders();
        protected abstract void LoadBuffers();
        //public abstract void Render();
    }
}
