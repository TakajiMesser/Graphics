using System;

namespace StarchUICore.Attributes.Sizes
{
    public class SizeEventArgs : EventArgs
    {
        public Size OldSize { get; }
        public Size NewSize { get; }

        public SizeEventArgs(Size oldSize, Size newSize)
        {
            OldSize = oldSize;
            NewSize = newSize;
        }
    }
}
