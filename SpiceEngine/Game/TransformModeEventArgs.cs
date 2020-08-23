using SweetGraphicsCore.Selection;
using System;

namespace SpiceEngine.Game
{
    public class TransformModeEventArgs : EventArgs
    {
        public TransformModes TransformMode { get; private set; }

        public TransformModeEventArgs(TransformModes transformMode)
        {
            TransformMode = transformMode;
        }
    }
}
