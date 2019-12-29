using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.UserInterfaces;
using SpiceEngineCore.Rendering.Buffers;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Rendering.UserInterfaces.Views;
using SpiceEngineCore.Rendering.Vertices;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.UserInterfaces
{
    // Stores all UIControls and determines the order that they should be drawn in
    public class UIManager : IUIProvider
    {
        private IEntityProvider _entityProvider;

        private List<IUIView> _views = new List<IUIView>();
        private Dictionary<int, IUIView> _viewsByID = new Dictionary<int, IUIView>();

        private VertexBuffer<TextureVertex2D> _vertexBuffer = new VertexBuffer<TextureVertex2D>();
        private VertexArray<TextureVertex2D> _vertexArray = new VertexArray<TextureVertex2D>();

        private bool _isSetup = false;

        public UIManager(IEntityProvider entityProvider)
        {
            _entityProvider = entityProvider;
        }

        private IUIView _testView;

        public void TestDraw()
        {
            if (!_isSetup)
            {
                var testUIElement = new UIElement()
                {
                    Name = "Test Element 01"
                };
                _testView = GetTestView();

                var entityID = _entityProvider.AddEntity(testUIElement);
                AddView(entityID, _testView);
                _testView.Load();

                _isSetup = true;
            }

            _testView.Draw();
        }

        public void AddView(int entityID, IUIView view)
        {
            _views.Add(view);
            _viewsByID.Add(entityID, view);
        }

        public IUIView GetView(int entityID) => _viewsByID[entityID];

        public IUIView GetTestView()
        {
            var width = 100.0f;
            var height = 200.0f;

            var meshShape = new ModelMesh();
            meshShape.Faces.Add(ModelFace.Rectangle(width, height));
            var meshBuild = new ModelBuilder(meshShape);

            var vertices = meshBuild.GetVertices().Select(v => new ViewVertex(v.Position, Color4.AliceBlue, Color4.White)).ToList();
            var triangleIndices = meshBuild.TriangleIndices;

            var testView = new UIView(vertices, triangleIndices);
            return testView;
        }

        public void Clear()
        {
            _views.Clear();
            _viewsByID.Clear();
        }

        public void Load()
        {
            _vertexBuffer.Bind();
            _vertexArray.Load();
            _vertexBuffer.Unbind();
        }

        public void Draw()
        {
            _vertexArray.Bind();
            _vertexBuffer.Bind();

            _vertexBuffer.Buffer();

            GL.DrawArrays(PrimitiveType.Quads, 0, _vertexBuffer.Count);

            _vertexArray.Unbind();
            _vertexBuffer.Unbind();
        }
    }
}
