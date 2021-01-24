using SpiceEngine.GLFW.Inputs;
using System;

namespace TangyHIDCore.Inputs
{
    public class MouseButtonEventArgs : EventArgs
    {
        public MouseButtonEventArgs(MouseButtons button, InputStates state, ModifierKeys modifiers)
        {
            Button = button;
            State = state;
            Modifiers = modifiers;
        }
        
        public MouseButtons Button { get; }
        public InputStates State { get; }
        public ModifierKeys Modifiers { get; }
    }
}
