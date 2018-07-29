using System;
using SpiceEngine.Entities;
using SpiceEngine.Rendering.Processing;

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
