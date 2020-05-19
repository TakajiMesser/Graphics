using System;

namespace SpiceEngine.Game
{
    public class CursorEventArgs : EventArgs
    {
        public bool ShowCursor { get; private set; }

        public CursorEventArgs(bool showCursor) => ShowCursor = showCursor;
    }
}
