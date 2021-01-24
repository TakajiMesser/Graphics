using SpiceEngine.GLFW.Initialization;
using System;

namespace TangyHIDCore.Utilities
{
    public class GLFWException: Exception
    {
        public GLFWException(string message, ErrorCodes errorCode) : base(message) => ErrorCode = errorCode;

        public ErrorCodes ErrorCode { get; }
    }
}
