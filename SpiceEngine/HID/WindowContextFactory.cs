using SpiceEngine.Game;
using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.Windowing;
using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.HID;
using System;
using System.Text;

namespace SpiceEngine.HID
{
    public class WindowContextFactory : IWindowContextFactory
    {
        private static WindowContextFactory _sharedInstance;

        public static WindowContextFactory SharedInstance
        {
            get
            {
                if (_sharedInstance == null)
                {
                    _sharedInstance = new WindowContextFactory();
                }

                return _sharedInstance;
            }
        }

        private GLFWContext _glfwContext;

        public WindowContextFactory()
        {
            _glfwContext = new GLFWContext();
            _glfwContext.Initialize();
        }

        public IWindowContext CreateWindowContext(IWindowConfig configuration)
        {
            /*switch(configuration.WindowType)
            {
                case WindowTypes.Native:
                    return CreateNativeWindow(configuration);
                case WindowTypes.Frame:
                    return CreateFrameWindow(configuration);
            }*/

            throw new NotImplementedException();
        }

        /*private IWindowContext CreateNativeWindow(IWindowConfig configuration)
        {
            GLFW.WindowHint(WindowHints.Focused, 1);
            return new GameWindow(configuration);
        }*/

        /*private IWindowContext CreateFrameWindow(IWindowConfig configuration)
        {
            var titleBytes = configuration.Title != null
                ? Encoding.UTF8.GetBytes(configuration.Title)
                : new byte[0];
            var monitor = SpiceEngine.GLFWBindings.Monitoring.Monitor.None;//GLFW.GetPrimaryMonitor();
            var window = SpiceEngine.GLFWBindings.Windowing.Window.None;//new SpiceEngine.GLFWBindings.Windowing.Window();

            GLFW.WindowHint(WindowHints.Focused, 1);

            //var monitor = GLFW.GetPrimaryMonitor();
            //var videoMode = GLFW.GetVideoMode(monitor);

            //GLFW.WindowHint(Hints.RedBits, videoMode.RedBits);
            //GLFW.WindowHint(Hints.GreenBits, videoMode.GreenBits);
            //GLFW.WindowHint(Hints.BlueBits, videoMode.BlueBits);
            //GLFW.WindowHint(Hints.RefreshRate, videoMode.RefreshRate);

            var windowHandle = GLFW.CreateWindow(configuration.Size.Width, configuration.Size.Height, titleBytes, monitor, window);
            return new FrameWindow(windowHandle, configuration.UpdatesPerSecond, configuration.RendersPerSecond)
            {
                WindowSize = new Resolution(configuration.Size.Width, configuration.Size.Height)
            };
        }*/

        public IntPtr CreateWindowHandle(IWindowConfig configuration)
        {
            var titleBytes = configuration.Title != null
                ? Encoding.UTF8.GetBytes(configuration.Title)
                : new byte[0];
            var monitor = SpiceEngine.GLFWBindings.Monitoring.Monitor.None;//GLFW.GetPrimaryMonitor();
            var window = SpiceEngine.GLFWBindings.Windowing.Window.None;//new SpiceEngine.GLFWBindings.Windowing.Window();

            //var monitor = GLFW.GetPrimaryMonitor();
            //var videoMode = GLFW.GetVideoMode(monitor);

            //GLFW.WindowHint(Hints.RedBits, videoMode.RedBits);
            //GLFW.WindowHint(Hints.GreenBits, videoMode.GreenBits);
            //GLFW.WindowHint(Hints.BlueBits, videoMode.BlueBits);
            //GLFW.WindowHint(Hints.RefreshRate, videoMode.RefreshRate);

            GLFW.WindowHint(WindowHints.Visible, configuration.Visible ? 1 : 0);
            GLFW.WindowHint(WindowHints.Focused, configuration.Visible ? 1 : 0);

            var windowHandle = GLFW.CreateWindow(configuration.Size.Width, configuration.Size.Height, titleBytes, monitor, window);
            return windowHandle;
        }
    }
}
