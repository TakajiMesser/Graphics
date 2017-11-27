using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using Graphics;
using Graphics.Mesh;

namespace GraphicsTest
{
    public class GameWindow : OpenTK.GameWindow
    {
        private ShaderProgram _program;

        private Matrix4Uniform _modelMatrix;
        private Matrix4Uniform _viewMatrix;
        private Matrix4Uniform _projectionMatrix;

        private Mesh _mesh;

        public GameWindow() : base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            // Compile and load shaders
            var vertexShader = new Shader(ShaderType.VertexShader, File.ReadAllText(@"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Shaders\vertex-shader.vs"));
            var fragmentShader = new Shader(ShaderType.FragmentShader, File.ReadAllText(@"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Shaders\fragment-shader.fs"));

            _program = new ShaderProgram(vertexShader, fragmentShader);

            // Create simple triangle mesh to render on the display
            var vertices = new List<MeshVertex>()
            {
                new MeshVertex(new Vector3(-1, -1, -1.5f), Vector3.Zero, Color4.Lime, Vector2.Zero),
                new MeshVertex(new Vector3(-1, 1, -1.5f), Vector3.Zero, Color4.Red, Vector2.Zero),
                new MeshVertex(new Vector3(1, -1, -1.5f), Vector3.Zero, Color4.Blue, Vector2.Zero)
            };

            _mesh = new Mesh(vertices, new List<int>() { 0, 1, 2 }, _program);

            _modelMatrix = new Matrix4Uniform("modelMatrix")
            {
                Matrix = Matrix4.Identity
            };

            _viewMatrix = new Matrix4Uniform("viewMatrix")
            {
                Matrix = Matrix4.Identity//Matrix4.LookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 3) + new Vector3(0, 0, -1.0f), new Vector3(0, 1, 0))
            };

            // Specify the FOV
            _projectionMatrix = new Matrix4Uniform("projectionMatrix")
            {
                Matrix = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 2), (float)Width / Height, 1.0f, 100.0f)
            };
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            base.OnUpdateFrame(e);

            var transform = new Transform(new Vector3(0.01f, 0, 0), Quaternion.Identity, Vector3.One);
            _modelMatrix.Matrix *= transform.ToModelMatrix();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color4.Purple);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _program.Use();

            _modelMatrix.Set(_program);
            _viewMatrix.Set(_program);
            _projectionMatrix.Set(_program);

            _mesh.Draw();

            GL.UseProgram(0);

            SwapBuffers();
        }
    }
}
