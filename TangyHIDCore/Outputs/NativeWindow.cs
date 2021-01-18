using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpiceEngineCore.Game;
using SpiceEngineCore.Rendering;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Configuration = SpiceEngineCore.Game.Settings.Configuration;

namespace TangyHIDCore.Outputs
{
    public abstract class NativeWindow// : IDisposable
    {
        private IntPtr _windowHandle;
        private IntPtr _cursorHandle;


    }
}
