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
using Graphics.Meshes;
using Graphics.GameObjects;
using Graphics.GameStates;
using System.Runtime.InteropServices;
using OpenTK.Input;
using Graphics.Physics.Collision;

namespace GraphicsTest
{
    public class GameWindow : OpenTK.GameWindow
    {
        private ShaderProgram _program;
        private GameState _gameState;
        private KeyboardState _keyState;
        private KeyboardState _previousKeyState;
        private MouseState _mouseState;
        private MouseState _previousMouseState;

        public GameWindow() : base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Console.WriteLine("GL Version: " + GL.GetString(StringName.Version));
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            _gameState.UpdateAspectRatio(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            //WindowState = WindowState.Maximized;
            Size = new System.Drawing.Size(300, 300);

            // Compile and load shaders
            var vertexShader = new Shader(ShaderType.VertexShader, File.ReadAllText(@"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Shaders\simple-vertex-shader.glsl"));
            var fragmentShader = new Shader(ShaderType.FragmentShader, File.ReadAllText(@"C:\Users\Takaji\documents\visual studio 2017\Projects\Graphics\Graphics\Shaders\simple-fragment-shader.glsl"));

            _program = new ShaderProgram(vertexShader, fragmentShader);

            var player = new Player()
            {
                Mesh = Mesh.LoadFromFile(@"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\Cube.obj", _program)
            };
            player.Collider = new BoundingSphere(player.Mesh.Vertices);
            player.Mesh.AddTestColors();
            player.Mesh.AddTestLight();
            player.Position = new Vector3(0.0f, 0.0f, -1.0f);

            _gameState = new GameState(player, new Camera("MainCamera", _program, Width, Height), _program);
            _gameState.Camera.AttachToObject(player);

            // Create simple triangle mesh to render on the display
            var triangle = new GameObject("Triangle")
            {
                Mesh = Mesh.LoadFromFile(@"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\GraphicsTest\Meshes\Triangle.obj", _program),
            };
            triangle.Collider = new BoundingSphere(triangle.Mesh.Vertices);
            triangle.Mesh.AddTestColors();
            triangle.Position = new Vector3(2.0f, 2.0f, -1.0f);

            _gameState.AddGameObject(triangle);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            base.OnUpdateFrame(e);

            HandleInput();
            _gameState.HandleInput();

            // For now, simply translate the mesh back and forth
            //var triangle = _gameState.GetByName("Triangle");
            /*if (triangle.ModelMatrix.Matrix.M41 > 1.0f)
            {
                triangle.Transform = new Transform(new Vector3(-0.01f, 0, 0), Quaternion.Identity, Vector3.One);
            }
            else if (triangle.ModelMatrix.Matrix.M41 < -1.0f)
            {
                triangle.Transform = new Transform(new Vector3(0.01f, 0, 0), Quaternion.Identity, Vector3.One);
            }*/
            //triangle.Transform = new Transform(Vector3.Zero, Quaternion.FromEulerAngles(0.0f, 0.1f, 0.0f), Vector3.One);
            //triangle.Transform = new Transform(Vector3.Zero, Quaternion.FromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), 0.1f), Vector3.One);

            _gameState.UpdateFrame();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color4.Purple);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _program.Use();
            _gameState.RenderFrame();

            GL.UseProgram(0);
            SwapBuffers();

            PollForInput();
        }

        private void HandleInput()
        {
            if (_keyState.IsKeyDown(Key.Escape))
            {
                Close();
            }
        }

        private void PollForInput()
        {
            _previousKeyState = _keyState;
            _previousMouseState = _mouseState;

            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
        }
    }
}
