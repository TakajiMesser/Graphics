using OpenTK.Input;
using SpiceEngine.GLFW.Inputs;
using System;

namespace TangyHIDCore.Utilities
{
    public static class InputExtensions
    {
        public static Key ConvertToOpenTKKey(this int key)
        {
            switch ((Keys)key)
            {
                case Keys.Unknown:
                    return Key.Unknown;
                case Keys.W:
                    return Key.W;
                case Keys.A:
                    return Key.A;
                case Keys.S:
                    return Key.S;
                case Keys.D:
                    return Key.D;
                case Keys.Q:
                    return Key.Q;
                case Keys.E:
                    return Key.E;
                case Keys.F:
                    return Key.F;
                case Keys.Space:
                    return Key.Space;
                case Keys.Escape:
                    return Key.Escape;
                case Keys.LeftShift:
                    return Key.ShiftLeft;
                case Keys.LeftControl:
                    return Key.ControlLeft;
                case Keys.Up:
                    return Key.Up;
                case Keys.Left:
                    return Key.Left;
                case Keys.Down:
                    return Key.Down;
                case Keys.Right:
                    return Key.Right;
                case Keys.Num0:
                    return Key.Number0;
                case Keys.Num1:
                    return Key.Number1;
                case Keys.Num2:
                    return Key.Number2;
                case Keys.Num3:
                    return Key.Number3;
                case Keys.Num4:
                    return Key.Number4;
                case Keys.Num5:
                    return Key.Number5;
                case Keys.Num6:
                    return Key.Number6;
                case Keys.Num7:
                    return Key.Number7;
                case Keys.Num8:
                    return Key.Number8;
                case Keys.Num9:
                    return Key.Number9;
            }

            throw new ArgumentOutOfRangeException("Could not convert key " + key);
        }

        public static MouseButton? ConvertToOpenTKMouseButton(this int button)
        {
            switch ((MouseButtons)button)
            {
                case MouseButtons.Unknown:
                    return null;
                case MouseButtons.Left:
                    return MouseButton.Left;
                case MouseButtons.Right:
                    return MouseButton.Right;
                case MouseButtons.Middle:
                    return MouseButton.Middle;
                /*case MouseButtons.Button1:
                    return MouseButton.Button1;
                case MouseButtons.Button2:
                    return MouseButton.Button2;
                case MouseButtons.Button3:
                    return MouseButton.Button3;*/
                case MouseButtons.Button4:
                    return MouseButton.Button4;
                case MouseButtons.Button5:
                    return MouseButton.Button5;
                case MouseButtons.Button6:
                    return MouseButton.Button6;
                case MouseButtons.Button7:
                    return MouseButton.Button7;
                case MouseButtons.Button8:
                    return MouseButton.Button8;
            }

            throw new ArgumentOutOfRangeException("Could not convert button " + button);
        }
    }
}
