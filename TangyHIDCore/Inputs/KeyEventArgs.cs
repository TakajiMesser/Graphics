using SpiceEngine.GLFW.Inputs;
using System;

namespace TangyHIDCore.Inputs
{
    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(Keys key, int scanCode, InputStates state, ModifierKeys modifiers)
        {
            Key = key;
            ScanCode = scanCode;
            State = state;
            Modifiers = modifiers;
        }
        
        public Keys Key { get; }
        public int ScanCode { get; }
        public InputStates State { get; }
        public ModifierKeys Modifiers { get; }
    }
}
