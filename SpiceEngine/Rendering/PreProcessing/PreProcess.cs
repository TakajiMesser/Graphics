using SpiceEngineCore.Rendering;

namespace SpiceEngine.Rendering.PreProcessing
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
            LoadPrograms();
            LoadBuffers();
        }

        protected abstract void LoadPrograms();
        protected abstract void LoadBuffers();
        //public abstract void Render();
    }
}
