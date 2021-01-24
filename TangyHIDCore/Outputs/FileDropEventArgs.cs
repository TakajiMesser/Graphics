using System;

namespace TangyHIDCore.Outputs
{
    public class FileDropEventArgs : EventArgs
    {
        public FileDropEventArgs(string[] paths) => Paths = paths;

        public string[] Paths { get; }
    }
}
