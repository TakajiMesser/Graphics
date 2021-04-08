using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.Utilities;
using SpiceEngineCore.Game;
using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.HID;
using System.Text;

namespace SpiceEngine.Game
{
    public class GLFWContext : IGameContext
    {
        private static readonly ErrorCallback _errorCallback = (code, description) => throw new GLFWException(description.ToStringUTF8(), code);

        public void Initialize()
        {
            GLFW.Init();
            GLFW.SetErrorCallback(_errorCallback);
        }

        public SpiceEngine.GLFWBindings.Windowing.Window CreateWindow(IWindowConfig configuration)
        {
            var titleBytes = configuration.Title != null
                ? Encoding.UTF8.GetBytes(configuration.Title)
                : new byte[0];
            var monitor = SpiceEngine.GLFWBindings.Monitoring.Monitor.None;//GLFW.GetPrimaryMonitor();
            var window = SpiceEngine.GLFWBindings.Windowing.Window.None;//new SpiceEngine.GLFWBindings.Windowing.Window();

            var windowHandle = GLFW.CreateWindow(configuration.Size.Width, configuration.Size.Height, titleBytes, monitor, window);
            return windowHandle;
        }
    }
}
