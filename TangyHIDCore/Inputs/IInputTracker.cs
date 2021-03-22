using SpiceEngineCore.Rendering;
using System;
using System.ComponentModel;
using TangyHIDCore.Outputs;

namespace TangyHIDCore.Inputs
{
    public interface IInputTracker
    {
        Resolution WindowSize { get; }

        //event EventHandler PositionChanged;
        //event EventHandler<SizeEventArgs> SizeChanged;
        //event EventHandler<SizeEventArgs> FramebufferSizeChanged;
        //event EventHandler<FocusEventArgs> FocusChanged;
        //event EventHandler<MaximizeEventArgs> MaximizeChanged;
        //event EventHandler<ScaleEventArgs> ContentScaleChanged;
        //event EventHandler Refreshed;
        //event EventHandler Closed;
        //event EventHandler<CancelEventArgs> Closing;
        //event EventHandler<FileDropEventArgs> FileDrop;
        event EventHandler<CursorEventArgs> CursorPositionChanged;
        event EventHandler<CursorEventArgs> Scrolled;
        event EventHandler CursorEntered;
        event EventHandler CursorExited;
        event EventHandler<KeyEventArgs> KeyAction;
        event EventHandler<KeyEventArgs> KeyPress;
        event EventHandler<KeyEventArgs> KeyRelease;
        //event EventHandler<KeyEventArgs> KeyRepeat;
        event EventHandler<MouseButtonEventArgs> MouseButton;
        //event EventHandler<CharacterEventArgs> CharacterInput;
    }
}
