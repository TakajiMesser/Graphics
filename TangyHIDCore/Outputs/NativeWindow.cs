using SpiceEngine.GLFW.Inputs;
using SpiceEngine.GLFW.Monitoring;
using SpiceEngine.GLFW.Utilities;
using SpiceEngine.GLFW.Windowing;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.GLFW;
using System;
using System.Text;
using TangyHIDCore.Inputs;
using TangyHIDCore.Utilities;

namespace TangyHIDCore.Outputs
{
    public abstract class NativeWindow : IDisposable
    {
        private SpiceEngine.GLFW.Windowing.Window _windowHandle;
        private Cursor _cursorHandle;

        private PositionCallback _windowPositionCallback;
        private SizeCallback _windowSizeCallback;
        private SizeCallback _framebufferSizeCallback;
        private FocusCallback _windowFocusCallback;
        private WindowMaximizedCallback _windowMaximizeCallback;
        private WindowContentsScaleCallback _windowContentScaleCallback;
        private WindowCallback _windowRefreshCallback;
        private WindowCallback _closeCallback;
        private FileDropCallback _dropCallback;
        private MouseCallback _cursorPositionCallback;
        private MouseCallback _scrollCallback;
        private MouseEnterCallback _cursorEnterCallback;
        private KeyCallback _keyCallback;
        private MouseButtonCallback _mouseButtonCallback;
        private CharModsCallback _charModsCallback;

        private bool disposedValue;

        private static readonly ErrorCallback _errorCallback = (code, description) => throw new GLFWException(description.ToStringUTF8(), code);

        public NativeWindow(int width, int height, string title, Monitor monitor, SpiceEngine.GLFW.Windowing.Window share)
        {
            // TODO - Do we need to keep track of the thread handle that GLFW was initialized on?
            GLFW.Init();
            GLFW.SetErrorCallback(_errorCallback);

            var titleBytes = title != null ? Encoding.UTF8.GetBytes(title) : new byte[0];
            _windowHandle = GLFW.CreateWindow(width, height, titleBytes, monitor, share);
            SetCallbacks();
        }

        //public IntPtr Handle => new IntPtr(_windowHandle);

        public Vector2i Position
        {
            get
            {
                GLFW.GetWindowPosition(_windowHandle, out var x, out var y);
                return new Vector2i(x, y);
            }
            set => GLFW.SetWindowPosition(_windowHandle, value.X, value.Y);
        }

        public Vector2i Size
        {
            get
            {
                GLFW.GetWindowSize(_windowHandle, out var width, out var height);
                return new Vector2i(width, height);
            }
            set => GLFW.SetWindowSize(_windowHandle, value.X, value.Y);
        }

        public Vector2 ContentScale
        {
            get
            {
                GLFW.GetWindowContentScale(_windowHandle, out var x, out var y);
                return new Vector2(x, y);
            }
        }

        public string ClipboardString
        {
            get => GLFW.GetClipboardStringInternal(_windowHandle).ToStringUTF8();
            set => GLFW.SetClipboardString(_windowHandle, Encoding.UTF8.GetBytes(value));
        }

        public Monitor Monitor => GLFW.GetWindowMonitor(_windowHandle);

        public Vector2d CursorPosition
        {
            get
            {
                GLFW.GetCursorPosition(_windowHandle, out var x, out var y);
                return new Vector2d(x, y);
            }
            set => GLFW.SetCursorPosition(_windowHandle, value.X, value.Y);
        }

        public bool ShouldClose => GLFW.WindowShouldClose(_windowHandle);

        public bool IsVisible => GetAttribute(WindowAttributes.Visible);

        public bool IsDecorated => GetAttribute(WindowAttributes.Decorated);

        public bool IsFloating => GetAttribute(WindowAttributes.Floating);

        public bool IsFocused => GetAttribute(WindowAttributes.Focused);

        public bool IsResizable => GetAttribute(WindowAttributes.Resizable);

        public bool IsMaximized => GetAttribute(WindowAttributes.Maximized);

        public bool IsMinimized => GetAttribute(WindowAttributes.AutoIconify);

        /*public CursorModes CursorMode
        {
            get => (CursorMode)GLFW.GetInputMode(_windowHandle, InputModes.Cursor);
            set => GLFW.SetInputMode(_windowHandle, InputModes.Cursor, (int)value);
        }*/

        public event EventHandler PositionChanged;
        public event EventHandler<SizeEventArgs> SizeChanged;
        public event EventHandler<SizeEventArgs> FramebufferSizeChanged;
        public event EventHandler<FocusEventArgs> FocusChanged;
        public event EventHandler<MaximizeEventArgs> MaximizeChanged;
        public event EventHandler<ScaleEventArgs> ContentScaleChanged;
        public event EventHandler Refreshed;
        public event EventHandler Closed;
        public event EventHandler Closing;
        public event EventHandler<FileDropEventArgs> FileDrop;
        public event EventHandler<CursorEventArgs> CursorPositionChanged;
        public event EventHandler<CursorEventArgs> Scrolled;
        public event EventHandler CursorEntered;
        public event EventHandler CursorExited;
        public event EventHandler<KeyEventArgs> KeyAction;
        public event EventHandler<KeyEventArgs> KeyPress;
        public event EventHandler<KeyEventArgs> KeyRelease;
        public event EventHandler<KeyEventArgs> KeyRepeat;
        public event EventHandler<MouseButtonEventArgs> MouseButton;
        public event EventHandler<CharacterEventArgs> CharacterInput;

        public void MakeCurrent() => GLFW.MakeContextCurrent(_windowHandle);

        public void SwapBuffers() => GLFW.SwapBuffers(_windowHandle);

        public void Maximize() => GLFW.MaximizeWindow(_windowHandle);

        public void Minimize() => GLFW.IconifyWindow(_windowHandle);

        public void Restore() => GLFW.RestoreWindow(_windowHandle);

        public void Focus() => GLFW.FocusWindow(_windowHandle);

        public void RequestAttention() => GLFW.RequestWindowAttention(_windowHandle);

        public void Fullscreen(Monitor monitor) => GLFW.SetWindowMonitor(_windowHandle, monitor, 0, 0, 0, 0, -1);

        public void SetTitle(string title) => GLFW.SetWindowTitle(_windowHandle, Encoding.UTF8.GetBytes(title));

        public void SetIcons(params Image[] images) => GLFW.SetWindowIcon(_windowHandle, images.Length, images);

        public void SetMonitor(Monitor monitor, int x, int y, int width, int height, int refreshRate) => GLFW.SetWindowMonitor(_windowHandle, monitor, x, y, width, height, refreshRate);

        //public void SetSizeLimits(int minWidth, int minHeight, int maxWidth, int maxHeight) => GLFW.SetWindowSizeLimits(_windowHandle, minWidth, minHeight, maxWidth, maxHeight);

        private bool GetAttribute(WindowAttributes attribute)
        {
            var value = GLFW.GetWindowAttribute(_windowHandle, (int)attribute);
            return value == 1;
        }

        private void SetCallbacks()
        {
            _windowPositionCallback = (_, x, y) => OnPositionChanged(x, y);
            _windowSizeCallback = (_, w, h) => OnSizeChanged(w, h);
            _framebufferSizeCallback = (_, w, h) => OnFramebufferSizeChanged(w, h);
            _windowFocusCallback = (_, focusing) => OnFocusChanged(focusing);
            _windowMaximizeCallback = (_, maximized) => OnMaximizeChanged(maximized);
            _windowContentScaleCallback = (_, x, y) => OnContentScaleChanged(x, y);
            _windowRefreshCallback = _ => OnRefreshed();
            _closeCallback = _ => OnClosing();
            _dropCallback = (_, count, arrayPtr) => OnFileDrop(arrayPtr.ToStringsUTF8(count));
            _cursorPositionCallback = (_, x, y) => OnCursorPositionChanged(x, y);
            _scrollCallback = (_, x, y) => OnScrolled(x, y);
            _cursorEnterCallback = (_, entering) => OnCursorEnterChanged(entering);
            _keyCallback = (_, key, code, state, mods) => OnKey(key, code, state, mods);
            _mouseButtonCallback = (_, button, state, mod) => OnMouseButton(button, state, mod);
            _charModsCallback = (_, cp, mods) => OnCharacterInput(cp, mods);

            GLFW.SetWindowPositionCallback(_windowHandle, _windowPositionCallback);
            GLFW.SetWindowSizeCallback(_windowHandle, _windowSizeCallback);
            GLFW.SetFramebufferSizeCallback(_windowHandle, _framebufferSizeCallback);
            GLFW.SetWindowFocusCallback(_windowHandle, _windowFocusCallback);
            GLFW.SetWindowMaximizeCallback(_windowHandle, _windowMaximizeCallback);
            GLFW.SetWindowContentScaleCallback(_windowHandle, _windowContentScaleCallback);
            GLFW.SetWindowRefreshCallback(_windowHandle, _windowRefreshCallback);
            GLFW.SetCloseCallback(_windowHandle, _closeCallback);
            GLFW.SetDropCallback(_windowHandle, _dropCallback);
            GLFW.SetCursorPositionCallback(_windowHandle, _cursorPositionCallback);
            GLFW.SetScrollCallback(_windowHandle, _scrollCallback);
            GLFW.SetCursorEnterCallback(_windowHandle, _cursorEnterCallback);
            GLFW.SetKeyCallback(_windowHandle, _keyCallback);
            GLFW.SetMouseButtonCallback(_windowHandle, _mouseButtonCallback);
            GLFW.SetCharModsCallback(_windowHandle, _charModsCallback);
        }

        protected virtual void OnPositionChanged(double x, double y) => PositionChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnSizeChanged(int width, int height) => SizeChanged?.Invoke(this, new SizeEventArgs(width, height));

        protected virtual void OnFramebufferSizeChanged(int width, int height) => FramebufferSizeChanged?.Invoke(this, new SizeEventArgs(width, height));

        protected virtual void OnFocusChanged(bool isFocusing) => FocusChanged?.Invoke(this, new FocusEventArgs(isFocusing));

        protected virtual void OnMaximizeChanged(bool isMaximized) => MaximizeChanged?.Invoke(this, new MaximizeEventArgs(isMaximized));

        protected virtual void OnContentScaleChanged(float x, float y) => ContentScaleChanged?.Invoke(this, new ScaleEventArgs(x, y));

        protected virtual void OnRefreshed() => Refreshed?.Invoke(this, EventArgs.Empty);

        protected virtual void OnClosing()
        {
            Closing?.Invoke(this, EventArgs.Empty);
            GLFW.SetWindowShouldClose(_windowHandle, false);
            OnClosed();
        }

        protected virtual void OnClosed() => Closed?.Invoke(this, EventArgs.Empty);

        protected virtual void OnFileDrop(string[] paths) => FileDrop?.Invoke(this, new FileDropEventArgs(paths));

        protected virtual void OnCursorPositionChanged(double x, double y) => CursorPositionChanged?.Invoke(this, new CursorEventArgs(x, y));

        protected virtual void OnScrolled(double x, double y) => Scrolled?.Invoke(this, new CursorEventArgs(x, y));

        protected virtual void OnCursorEnterChanged(bool isEntering)
        {
            if (isEntering)
            {
                OnCursorEntered();
            }
            else
            {
                OnCursorExited();
            }
        }

        protected virtual void OnCursorEntered() => CursorEntered?.Invoke(this, EventArgs.Empty);

        protected virtual void OnCursorExited() => CursorExited?.Invoke(this, EventArgs.Empty);

        protected virtual void OnKey(Keys key, int scanCode, InputStates state, ModifierKeys modifiers)
        {
            var args = new KeyEventArgs(key, scanCode, state, modifiers);

            if (state.HasFlag(InputStates.Press))
            {
                KeyPress?.Invoke(this, args);
            }  
            else if (state.HasFlag(InputStates.Release))
            {
                KeyRelease?.Invoke(this, args);
            }  
            else
            {
                KeyRepeat?.Invoke(this, args);
            }
            
            KeyAction?.Invoke(this, args);
        }

        protected virtual void OnMouseButton(MouseButtons button, InputStates state, ModifierKeys modifiers) => MouseButton?.Invoke(this, new MouseButtonEventArgs(button, state, modifiers));

        protected virtual void OnCharacterInput(uint codePoint, ModifierKeys mods) => CharacterInput?.Invoke(this, new CharacterEventArgs(codePoint, mods));

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~NativeWindow()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
