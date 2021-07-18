using SpiceEngine.GLFWBindings.Inputs;
using System;

namespace TangyHIDCore.Inputs
{
    public class CharacterEventArgs : EventArgs
    {
        public CharacterEventArgs(uint codePoint, ModifierKeys modifiers)
        {
            CodePoint = codePoint;
            Modifiers = modifiers;
        }
        
        public uint CodePoint { get; }
        public ModifierKeys Modifiers { get; }
    }
}
