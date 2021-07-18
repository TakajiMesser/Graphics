using SpiceEngine.GLFWBindings;
using SpiceEngine.GLFWBindings.Context;
using SpiceEngine.GLFWBindings.Windowing;
using SpiceEngineCore.Game.Settings;
using SpiceEngineCore.HID;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore;
using System;

namespace SpiceEngine.Rendering
{
    public class RenderContextFactory : IRenderContextFactory
    {
        private static RenderContextFactory _sharedInstance;

        public static RenderContextFactory SharedInstance
        {
            get
            {
                if (_sharedInstance == null)
                {
                    _sharedInstance = new RenderContextFactory();
                }

                return _sharedInstance;
            }
        }

        public IRenderContext CreateRenderContext(IRenderConfig configuration, IWindowContext windowContext)
        {
            switch (configuration.API)
            {
                case "OpenGL":
                    return CreateOpenGLContext(configuration, windowContext);
            }

            throw new NotImplementedException();
        }

        private IRenderContext CreateOpenGLContext(IRenderConfig configuration, IWindowContext windowContext)
        {
            GLFW.WindowHint(Hints.ClientAPI, (int)APIs.OpenGL);
            GLFW.WindowHint(Hints.ContextVersionMajor, configuration.MajorVersion);
            GLFW.WindowHint(Hints.ContextVersionMinor, configuration.MinorVersion);

            GLFW.WindowHint(Hints.OpenGLForwardCompat, 1);
            //GLFW.WindowHint(Hints.OpenGLDebugContext, 1);
            GLFW.WindowHint(Hints.OpenGLProfile, (int)OpenGLProfiles.Any);

            return new OpenGLContext(windowContext);
        }
    }
}
