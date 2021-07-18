using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.Inputs;
using SpiceEngine.GLFWBindings.Monitoring;
using SpiceEngine.GLFWBindings.Utilities;
using SpiceEngine.GLFWBindings.Windowing;
using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.HID;
using SpiceEngineCore.Rendering;
using System;
using System.ComponentModel;
using System.Text;
using TangyHIDCore.Inputs;

namespace TangyHIDCore.Outputs
{
    public abstract class Window : IWindowContext, IInputTracker
    {
        protected SpiceEngine.GLFWBindings.Windowing.Window _windowHandle;
        protected Cursor _cursorHandle;

        protected PositionCallback _windowPositionCallback;
        protected SizeCallback _windowSizeCallback;
        protected SizeCallback _framebufferSizeCallback;
        protected FocusCallback _windowFocusCallback;
        protected WindowMaximizedCallback _windowMaximizeCallback;
        protected WindowContentsScaleCallback _windowContentScaleCallback;
        protected WindowCallback _windowRefreshCallback;
        protected WindowCallback _closeCallback;
        protected FileDropCallback _dropCallback;
        protected MouseCallback _cursorPositionCallback;
        protected MouseCallback _scrollCallback;
        protected MouseEnterCallback _cursorEnterCallback;
        protected KeyCallback _keyCallback;
        protected MouseButtonCallback _mouseButtonCallback;
        protected CharModsCallback _charModsCallback;

        public Window(IWindowConfig configuration, IWindowContextFactory windowFactory)
        {
            _windowHandle = (SpiceEngine.GLFWBindings.Windowing.Window)windowFactory.CreateWindowHandle(configuration);
            SetCallbacks(configuration);

            // TODO - What is this?
            Exists = true;
        }

        public Resolution WindowSize { get; set; }

        protected virtual void SetCallbacks(IWindowConfig configuration)
        {
            if (configuration.HandleInputEvents)
            {
                _dropCallback = (_, count, arrayPtr) => OnFileDrop(arrayPtr.ToStringsUTF8(count));
                _cursorPositionCallback = (_, x, y) => OnCursorPositionChanged(x, y);
                _scrollCallback = (_, x, y) => OnScrolled(x, y);
                _cursorEnterCallback = (_, entering) => OnCursorEnterChanged(entering);
                _keyCallback = (_, key, code, state, mods) => OnKey(key, code, state, mods);
                _mouseButtonCallback = (_, button, state, mod) => OnMouseButton(button, state, mod);
                _charModsCallback = (_, cp, mods) => OnCharacterInput(cp, mods);

                GLFW.SetDropCallback(_windowHandle, _dropCallback);
                GLFW.SetCursorPositionCallback(_windowHandle, _cursorPositionCallback);
                GLFW.SetScrollCallback(_windowHandle, _scrollCallback);
                GLFW.SetCursorEnterCallback(_windowHandle, _cursorEnterCallback);
                GLFW.SetKeyCallback(_windowHandle, _keyCallback);
                GLFW.SetMouseButtonCallback(_windowHandle, _mouseButtonCallback);
                GLFW.SetCharModsCallback(_windowHandle, _charModsCallback);
            }

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

        public Vector2i PointToClient(Vector2i point) => point - Position;

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

        public int X
        {
            get
            {
                GLFW.GetWindowPosition(_windowHandle, out var x, out var _);
                return x;
            }
        }

        public int Y
        {
            get
            {
                GLFW.GetWindowPosition(_windowHandle, out var _, out var y);
                return y;
            }
        }

        public int Width
        {
            get
            {
                GLFW.GetWindowSize(_windowHandle, out var width, out var _);
                return width;
            }
        }

        public int Height
        {
            get
            {
                GLFW.GetWindowSize(_windowHandle, out var _, out var height);
                return height;
            }
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
            get => GLFW.GetClipboardString(_windowHandle).ToStringUTF8();
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

        public bool IsExiting { get; private set; }
        public bool Exists { get; private set; }

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
        public event EventHandler<CancelEventArgs> Closing;
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

        public void Update()
        {
            // TODO - How do we force an update?
        }

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

        public void Close()
        {
            OnClosing();
            Dispose(true);
        }

        protected void DestroyWindow()
        {
            if (Exists)
            {
                Exists = false;
                GLFW.DestroyWindow(_windowHandle);
                OnClosed();
            }
        }

        private bool GetAttribute(WindowAttributes attribute)
        {
            var value = GLFW.GetWindowAttribute(_windowHandle, (int)attribute);
            return value == 1;
        }

        protected virtual void OnPositionChanged(double x, double y) => PositionChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnSizeChanged(int width, int height)
        {
            WindowSize.Update(width, height);
            SizeChanged?.Invoke(this, new SizeEventArgs(width, height));
        }

        protected virtual void OnFramebufferSizeChanged(int width, int height) => FramebufferSizeChanged?.Invoke(this, new SizeEventArgs(width, height));

        protected virtual void OnFocusChanged(bool isFocusing) => FocusChanged?.Invoke(this, new FocusEventArgs(isFocusing));

        protected virtual void OnMaximizeChanged(bool isMaximized) => MaximizeChanged?.Invoke(this, new MaximizeEventArgs(isMaximized));

        protected virtual void OnContentScaleChanged(float x, float y) => ContentScaleChanged?.Invoke(this, new ScaleEventArgs(x, y));

        protected virtual void OnRefreshed() => Refreshed?.Invoke(this, EventArgs.Empty);

        protected virtual void OnClosing()
        {
            var args = new CancelEventArgs();
            Closing?.Invoke(this, args);

            if (args.Cancel)
            {
                GLFW.SetWindowShouldClose(_windowHandle, false);
            }
            else
            {
                IsExiting = true;
            }

            Dispose(true);
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

        protected virtual bool ProcessEvents()
        {
            if (IsExiting)
            {
                DestroyWindow();
                return false;
            }

            // TODO - Poll for inputs?
            GLFW.PollEvents();
            return true;
        }

        #region IDisposable Support
        public bool IsDisposed { get; private set; } = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                IsDisposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~NativeWindow()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
