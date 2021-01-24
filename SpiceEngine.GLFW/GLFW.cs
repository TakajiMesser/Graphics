using SpiceEngine.GLFW.Initialization;
using SpiceEngine.GLFW.Inputs;
using SpiceEngine.GLFW.Monitoring;
using SpiceEngine.GLFW.Utilities;
using SpiceEngine.GLFW.Windowing;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SpiceEngineCore.GLFW
{
    public static class GLFW
    {
        public const string DLL_NAME =
#if Windows
            "glfw3";
#elif OSX
            "libglfw.3";
#elif Linux
            "glfw";
#endif

        public static IntPtr GetProcAddress(string procName) => GetProcAddress(Encoding.ASCII.GetBytes(procName));

        [DllImport(DLL_NAME, EntryPoint = "glfwInit", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Init();

        [DllImport(DLL_NAME, EntryPoint = "glfwTerminate", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Terminate();

        [DllImport(DLL_NAME, EntryPoint = "glfwInitHint", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitHint(InitHints hint, bool value);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetVersion(out int major, out int minor, out int revision);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetVersionString", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetVersionString();

        [DllImport(DLL_NAME, EntryPoint = "glfwGetError", CallingConvention = CallingConvention.Cdecl)]
        public static extern ErrorCodes GetErrorPrivate(out IntPtr description);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetErrorCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(ErrorCallback))]
        public static extern ErrorCallback SetErrorCallback(ErrorCallback errorHandler);




        [DllImport(DLL_NAME, EntryPoint = "glfwDefaultWindowHints", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DefaultWindowHints();

        [DllImport(DLL_NAME, EntryPoint = "glfwWindowHint", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WindowHint(WindowHints hint, int value);

        [DllImport(DLL_NAME, EntryPoint = "glfwWindowHintString", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WindowHintString(WindowHints hint, byte[] value);

        [DllImport(DLL_NAME, EntryPoint = "glfwCreateWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern Window CreateWindow(int width, int height, byte[] title, Monitor monitor, Window share);

        [DllImport(DLL_NAME, EntryPoint = "glfwDestroyWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyWindow(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool WindowShouldClose(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowShouldClose(Window window, bool close);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowTitle(Window window, byte[] title);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowIcon", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowIcon(Window window, int count, Image[] images);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetWindowPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowPosition(Window window, out int x, out int y);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowPosition(Window window, int x, int y);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowSize(Window window, out int width, out int height);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowSize(Window window, int width, int height);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetFramebufferSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetFramebufferSize(Window window, out int width, out int height);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetWindowFrameSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowFrameSize(Window window, out int left, out int top, out int right, out int bottom);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetWindowContentScale", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowContentScale(Window window, out float xScale, out float yScale);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        public static extern float GetWindowOpacity(IntPtr window);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowOpacity(IntPtr window, float opacity);

        [DllImport(DLL_NAME, EntryPoint = "glfwIconifyWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IconifyWindow(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwRestoreWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RestoreWindow(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwMaximizeWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MaximizeWindow(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwShowWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ShowWindow(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwHideWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HideWindow(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwFocusWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FocusWindow(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwRequestWindowAttention", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RequestWindowAttention(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetWindowMonitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern Monitor GetWindowMonitor(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowMonitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowMonitor(Window window, Monitor monitor, int x, int y, int width, int height, int refreshRate);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetWindowAttrib", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetWindowAttribute(Window window, int attribute);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowAttrib", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowAttribute(IntPtr window, WindowAttributes attr, bool value);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowUserPointer(Window window, IntPtr userPointer);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetWindowUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetWindowUserPointer(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowPosCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(PositionCallback))]
        public static extern PositionCallback SetWindowPositionCallback(Window window, PositionCallback positionCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowSizeCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(SizeCallback))]
        public static extern SizeCallback SetWindowSizeCallback(Window window, SizeCallback sizeCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowCloseCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(WindowCallback))]
        public static extern WindowCallback SetCloseCallback(Window window, WindowCallback closeCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowRefreshCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(WindowCallback))]
        public static extern WindowCallback SetWindowRefreshCallback(Window window, WindowCallback callback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowFocusCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(FocusCallback))]
        public static extern FocusCallback SetWindowFocusCallback(Window window, FocusCallback focusCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowIconifyCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(IconifyCallback))]
        public static extern IconifyCallback SetWindowIconifyCallback(Window window, IconifyCallback callback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowMaximizeCallback", CallingConvention = CallingConvention.Cdecl)]
        public static extern WindowMaximizedCallback SetWindowMaximizeCallback(Window window, WindowMaximizedCallback cb);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetFramebufferSizeCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(SizeCallback))]
        public static extern SizeCallback SetFramebufferSizeCallback(Window window, SizeCallback sizeCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetWindowContentScaleCallback", CallingConvention = CallingConvention.Cdecl)]
        public static extern WindowContentsScaleCallback SetWindowContentScaleCallback(Window window, WindowContentsScaleCallback cb);

        [DllImport(DLL_NAME, EntryPoint = "glfwPollEvents", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PollEvents();

        [DllImport(DLL_NAME, EntryPoint = "glfwWaitEvents", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WaitEvents();

        [DllImport(DLL_NAME, EntryPoint = "glfwWaitEventsTimeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WaitEventsTimeout(double timeout);

        [DllImport(DLL_NAME, EntryPoint = "glfwPostEmptyEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PostEmptyEvent();

        [DllImport(DLL_NAME, EntryPoint = "glfwSwapBuffers", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SwapBuffers(Window window);



        [DllImport(DLL_NAME, EntryPoint = "glfwMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MakeContextCurrent(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern Window GetCurrentContext();

        [DllImport(DLL_NAME, EntryPoint = "glfwSwapInterval", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SwapInterval(int interval);

        [DllImport(DLL_NAME, EntryPoint = "glfwExtensionSupported", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetExtensionSupported(byte[] extension);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetProcAddress", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetProcAddress(byte[] procName);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetMonitors", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetMonitors(out int count);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetPrimaryMonitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern Monitor GetPrimaryMonitor();

        [DllImport(DLL_NAME, EntryPoint = "glfwGetMonitorPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMonitorPosition(Monitor monitor, out int x, out int y);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetMonitorWorkarea", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMonitorWorkArea(IntPtr monitor, out int x, out int y, out int width, out int height);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetMonitorPhysicalSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMonitorPhysicalSize(Monitor monitor, out int width, out int height);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetMonitorContentScale", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMonitorContentScale(IntPtr monitor, out float xScale, out float yScale);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetMonitorName", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetMonitorNameInternal(Monitor monitor);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetMonitorUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetMonitorUserPointer(IntPtr monitor, IntPtr pointer);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetMonitorUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetMonitorUserPointer(IntPtr monitor);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetMonitorCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MonitorCallback))]
        public static extern MonitorCallback SetMonitorCallback(MonitorCallback monitorCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetVideoModes", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetVideoModes(Monitor monitor, out int count);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetVideoMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetVideoModeInternal(Monitor monitor);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetGamma", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetGamma(Monitor monitor, float gamma);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetGammaRamp", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetGammaRampInternal(Monitor monitor);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetGammaRamp", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetGammaRamp(Monitor monitor, GammaRamp gammaRamp);



        [DllImport(DLL_NAME, EntryPoint = "glfwGetInputMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetInputMode(Window window, InputModes mode);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetInputMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetInputMode(Window window, InputModes mode, int value);

        [DllImport(DLL_NAME, EntryPoint = "glfwRawMouseMotionSupported", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool RawMouseMotionSupported();

        [DllImport(DLL_NAME, EntryPoint = "glfwGetKeyName", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetKeyNameInternal(Keys key, int scanCode);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetKeyScancode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetKeyScanCode(Keys key);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetKey", CallingConvention = CallingConvention.Cdecl)]
        public static extern InputStates GetKey(Window window, Keys key);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetMouseButton", CallingConvention = CallingConvention.Cdecl)]
        public static extern InputStates GetMouseButton(Window window, MouseButtons button);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetCursorPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetCursorPosition(Window window, out double x, out double y);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetCursorPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCursorPosition(Window window, double x, double y);

        [DllImport(DLL_NAME, EntryPoint = "glfwCreateCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern Cursor CreateCursor(Image image, int xHotspot, int yHotspot);

        [DllImport(DLL_NAME, EntryPoint = "glfwCreateStandardCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern Cursor CreateStandardCursor(CursorShapes type);

        [DllImport(DLL_NAME, EntryPoint = "glfwDestroyCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyCursor(Cursor cursor);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCursor(Window window, Cursor cursor);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetKeyCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(KeyCallback))]
        public static extern KeyCallback SetKeyCallback(Window window, KeyCallback keyCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetCharCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(CharCallback))]
        public static extern CharCallback SetCharCallback(Window window, CharCallback charCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetCharModsCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(CharModsCallback))]
        public static extern CharModsCallback SetCharModsCallback(Window window, CharModsCallback charCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetMouseButtonCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MouseButtonCallback))]
        public static extern MouseButtonCallback SetMouseButtonCallback(Window window, MouseButtonCallback mouseCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetCursorPosCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MouseCallback))]
        public static extern MouseCallback SetCursorPositionCallback(Window window, MouseCallback mouseCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetCursorEnterCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MouseEnterCallback))]
        public static extern MouseEnterCallback SetCursorEnterCallback(Window window, MouseEnterCallback mouseCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetScrollCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MouseCallback))]
        public static extern MouseCallback SetScrollCallback(Window window, MouseCallback mouseCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetDropCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(FileDropCallback))]
        public static extern FileDropCallback SetDropCallback(Window window, FileDropCallback dropCallback);

        [DllImport(DLL_NAME, EntryPoint = "glfwJoystickPresent", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool JoystickPresent(Joysticks joystick);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetJoystickAxes", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetJoystickAxes(Joysticks joystick, out int count);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetJoystickButtons", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetJoystickButtons(Joysticks joystick, out int count);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetJoystickHats", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetJoystickHats(int joystickId, out int count);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetJoystickName", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetJoystickNameInternal(Joysticks joystick);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetJoystickGUID", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetJoystickGuidPrivate(int joystickId);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetJoystickUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetJoystickUserPointer(int joystickId, IntPtr pointer);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetJoystickUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetJoystickUserPointer(int joystickId);

        [DllImport(DLL_NAME, EntryPoint = "glfwJoystickIsGamepad", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool JoystickIsGamepad(int joystickId);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetJoystickCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(JoystickCallback))]
        public static extern JoystickCallback SetJoystickCallback(JoystickCallback callback);

        [DllImport(DLL_NAME, EntryPoint = "glfwUpdateGamepadMappings", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool UpdateGamepadMappings(byte[] mappings);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetGamepadName", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetGamepadNamePrivate(int gamepadId);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetGamepadState", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetGamepadState(int id, out GamePadState state);

        [DllImport(DLL_NAME, EntryPoint = "glfwSetClipboardString", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetClipboardString(Window window, byte[] bytes);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetClipboardString", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetClipboardStringInternal(Window window);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetTime();

        [DllImport(DLL_NAME, EntryPoint = "glfwSetTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTime(double time);

        [DllImport(DLL_NAME, EntryPoint = "glfwGetTimerValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong GetTimerValue();

        [DllImport(DLL_NAME, EntryPoint = "glfwGetTimerFrequency", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong GetTimerFrequency();
    }
}
