using SweetGraphicsCore.Selection;
using System;

namespace SpiceEngine.Game
{
    public class TransformSelectedEventArgs : EventArgs
    {
        public SelectionTypes SelectionType { get; private set; }

        public TransformSelectedEventArgs(SelectionTypes selectionType) => SelectionType = selectionType;
    }
}
