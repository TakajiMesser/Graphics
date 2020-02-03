using System;

namespace StarchUICore.Attributes.Positions
{
    public class PositionEventArgs : EventArgs
    {
        public Position OldPosition { get; }
        public Position NewPosition { get; }

        public PositionEventArgs(Position oldPosition, Position newPosition)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}
