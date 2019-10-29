using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Vertices;
using System.Collections.Generic;

namespace SpiceEngineCore.Rendering.UserInterfaces
{
    // Stores all UIControls and determines the order that they should be drawn in
    public class UIManager : IUIProvider
    {
        private List<UIControl> _controls = new List<UIControl>();

        private VertexBuffer<TextureVertex2D> _vertexBuffer = new VertexBuffer<TextureVertex2D>();
        private VertexArray<TextureVertex2D> _vertexArray = new VertexArray<TextureVertex2D>();

        public int Add(UIControl control)
        {
            _controls.Add(control);
            return _controls.Count;
        }

        public UIControl Get(int id) => _controls[id + 1];

        public void Clear() => _controls.Clear();

        public void LoadBuffers()
        {
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();
        }
    }
}
