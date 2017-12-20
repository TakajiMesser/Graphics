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
using Graphics.Helpers;
using Graphics.Maps;
using Graphics.Rendering.Shaders;

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
            var vertexShader = new Shader(ShaderType.VertexShader, File.ReadAllText(FilePathHelper.VERTEX_SHADER_PATH));
            var fragmentShader = new Shader(ShaderType.FragmentShader, File.ReadAllText(FilePathHelper.FRAGMENT_SHADER_PATH));

            _program = new ShaderProgram(vertexShader, fragmentShader);

            //Map.SaveTestMap();
            var loadedMap = Map.Load(FilePathHelper.MAP_PATH);

            _gameState = new GameState(_program, loadedMap, this);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Handle game logic, guaranteed to run at a fixed rate, regardless of FPS
            base.OnUpdateFrame(e);

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

            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
        }
    }
}
