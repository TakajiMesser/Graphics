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
using System.Runtime.InteropServices;
using OpenTK.Input;
using Graphics.Physics.Collision;
using Graphics.Helpers;
using Graphics.Maps;
using Graphics.Rendering.Shaders;
using Graphics.Scripting.BehaviorTrees;

namespace MappingTool
{
    public class GameWindow : OpenTK.GameWindow
    {
        private string _mapPath;

        private ShaderProgram _program;
        private GameState _gameState;
        private KeyboardState _keyState;
        private KeyboardState _previousKeyState;
        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private MouseDevice _mouse;
        private MouseDevice _previousMouse;

        public GameWindow(string mapPath) : base(1280, 720, GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            _mapPath = mapPath;
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
            Size = new System.Drawing.Size(1024, 768);

            // Compile and load shaders
            var vertexShader = new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.VERTEX_SHADER_PATH));
            var fragmentShader = new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.FRAGMENT_SHADER_PATH));

            _program = new ShaderProgram(vertexShader, fragmentShader);

            TestHelper.CreateTestEnemyBehavior();
            TestHelper.CreateTestPlayerBehavior();
            TestHelper.CreateTestMap();
            var loadedMap = Map.Load(_mapPath);

            _gameState = new GameState(_program, loadedMap, this);
            _gameState.Initialize();
        }

        //protected override void OnMouseEnter(EventArgs e) => CursorVisible = false;

        //protected override void OnMouseLeave(EventArgs e) => CursorVisible = true;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            HandleInput();
            _gameState.HandleInput();

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
            _previousMouse = _mouse;

            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            _mouse = Mouse;
        }
    }
}
