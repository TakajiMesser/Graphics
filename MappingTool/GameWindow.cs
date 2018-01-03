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
using Graphics.Rendering.PostProcessing;

namespace MappingTool
{
    public class GameWindow : OpenTK.GameWindow
    {
        private string _mapPath;

        private GameState _gameState;
        private KeyboardState _keyState;
        private KeyboardState _previousKeyState;
        private MouseState _mouseState;
        private MouseState _previousMouseState;
        private MouseDevice _mouse;
        private MouseDevice _previousMouse;

        public GameWindow(string mapPath) : base(1280, 720, 
            GraphicsMode.Default, "My First OpenGL Game", GameWindowFlags.Default, 
            DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
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

            // Create test objects
            TestHelper.CreateTestEnemyBehavior();
            TestHelper.CreateTestPlayerBehavior();
            TestHelper.CreateTestMap();
            var loadedMap = Map.Load(_mapPath);

            _gameState = new GameState(loadedMap, this);
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
            PollForInput();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color4.Purple);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _gameState.RenderFrame();

            GL.UseProgram(0);
            SwapBuffers();
        }

        private void HandleInput()
        {
            if (_keyState.IsKeyDown(Key.Escape))
            {
                Close();
            }
            else if (_previousKeyState != null && _previousKeyState.IsKeyUp(Key.F11) && _keyState.IsKeyDown(Key.F11))
            {
                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Fullscreen;
                }
                else
                {
                    WindowState = WindowState.Normal;
                }
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
