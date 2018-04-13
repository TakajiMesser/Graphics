using System;
using TakoEngine.Entities;
using TakoEngine.Rendering.Processing;

namespace TakoEngine.Game
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
