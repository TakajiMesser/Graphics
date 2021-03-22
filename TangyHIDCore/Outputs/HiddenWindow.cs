using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.Windowing;
using SpiceEngineCore.Game.Settings;

namespace TangyHIDCore.Outputs
{
    public class HiddenWindow : NativeWindow
    {
        public HiddenWindow(Configuration configuration) : base(configuration) { }

        protected override void SetWindowHints(Configuration configuration)
        {
            base.SetWindowHints(configuration);

            // TODO - Are these two calls redundant?
            GLFW.WindowHint(WindowHints.Visible, 0);
            //uGLFW.HideWindow(_windowHandle);
        }

        protected override void SetCallbacks()
        {
            _windowPositionCallback = (_, x, y) => OnPositionChanged(x, y);
            _windowSizeCallback = (_, w, h) => OnSizeChanged(w, h);
            _framebufferSizeCallback = (_, w, h) => OnFramebufferSizeChanged(w, h);
            _windowFocusCallback = (_, focusing) => OnFocusChanged(focusing);
            _windowMaximizeCallback = (_, maximized) => OnMaximizeChanged(maximized);
            _windowContentScaleCallback = (_, x, y) => OnContentScaleChanged(x, y);
            _windowRefreshCallback = _ => OnRefreshed();
            _closeCallback = _ => OnClosing();

            GLFW.SetWindowPositionCallback(_windowHandle, _windowPositionCallback);
            GLFW.SetWindowSizeCallback(_windowHandle, _windowSizeCallback);
            GLFW.SetFramebufferSizeCallback(_windowHandle, _framebufferSizeCallback);
            GLFW.SetWindowFocusCallback(_windowHandle, _windowFocusCallback);
            GLFW.SetWindowMaximizeCallback(_windowHandle, _windowMaximizeCallback);
            GLFW.SetWindowContentScaleCallback(_windowHandle, _windowContentScaleCallback);
            GLFW.SetWindowRefreshCallback(_windowHandle, _windowRefreshCallback);
            GLFW.SetCloseCallback(_windowHandle, _closeCallback);
        }
    }
}
