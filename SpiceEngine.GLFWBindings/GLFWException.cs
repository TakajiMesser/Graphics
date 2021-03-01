using SpiceEngine.GLFWBindings.Initialization;
using System;

namespace SpiceEngine.GLFWBindings
{
    public class GLFWException: Exception
    {
        public GLFWException(string message, ErrorCodes errorCode) : base(message) => ErrorCode = errorCode;

        public ErrorCodes ErrorCode { get; }
    }
}
