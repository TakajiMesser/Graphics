using System;

namespace TakoEngine.Game
{
    public class CursorEventArgs : EventArgs
    {
        public bool ShowCursor { get; private set; }

        public CursorEventArgs(bool showCursor)
        {
            ShowCursor = showCursor;
        }
    }
}
