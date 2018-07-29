using SpiceEngine.Outputs;

namespace SpiceEngine.Rendering.Processing
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
