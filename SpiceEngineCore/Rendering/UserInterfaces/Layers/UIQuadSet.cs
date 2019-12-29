using OpenTK.Graphics;
using SpiceEngineCore.Rendering.Textures;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.UserInterfaces.Layers
{
    public class UIQuadSet
    {
        private List<UIQuad> _quads = new List<UIQuad>();

        private UIQuad _totalQuad = new UIQuad();


        public void Add(UIQuad quad)
        {
            _quads.Add(quad);
        }

        public void Remove(UIQuad quad)
        {
            _quads.Remove(quad);
        }

        public void Clear()
        {
            _quads.Clear();
        }
    }
}
