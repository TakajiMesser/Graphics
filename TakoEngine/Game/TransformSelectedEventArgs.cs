using System;
using TakoEngine.Entities;

namespace TakoEngine.Game
{
    public enum SelectionTypes
    {
        None,
        Red,
        Green,
        Blue,
        Cyan,
        Magenta,
        Yellow
    }

    public class TransformSelectedEventArgs : EventArgs
    {
        public SelectionTypes SelectionType { get; private set; }

        public TransformSelectedEventArgs(SelectionTypes selectionType)
        {
            SelectionType = selectionType;
        }
    }
}
