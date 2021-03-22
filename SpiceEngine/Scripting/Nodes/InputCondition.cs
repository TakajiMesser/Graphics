using SpiceEngine.GLFWBindings.Inputs;
using System;
using TangyHIDCore;

namespace UmamiScriptingCore.Behaviors.Nodes.Decorators
{
    public enum InputTypes
    {
        Down,
        Up,
        Pressed,
        Released,
        Held
    }

    public abstract class InputCondition
    {
        public InputCondition(InputTypes inputType, int nFramesHeld = 0)
        {
            InputType = inputType;
            FramesHeldCount = nFramesHeld;
        }

        public InputTypes InputType { get; }
        public int FramesHeldCount { get; }

        public abstract bool Condition(IInputProvider inputProvider);
    }

    public class CommandCondition : InputCondition
    {
        public CommandCondition(string command, InputTypes inputType, int nFramesHeld = 0) : base(inputType, nFramesHeld) => Command = command;

        public string Command { get; }

        public override bool Condition(IInputProvider inputProvider)
        {
            switch (InputType)
            {
                case InputTypes.Down:
                    return inputProvider.IsDown(Command);
                case InputTypes.Up:
                    return inputProvider.IsUp(Command);
                case InputTypes.Pressed:
                    return inputProvider.IsPressed(Command);
                case InputTypes.Released:
                    return inputProvider.IsReleased(Command);
                case InputTypes.Held:
                    return inputProvider.IsHeld(Command, FramesHeldCount);
            }

            throw new ArgumentException("Could not handle input type " + InputType);
        }
    }

    public class KeyCondition : InputCondition
    {
        public KeyCondition(Keys key, InputTypes inputType, int nFramesHeld = 0) : base(inputType, nFramesHeld) => Key = key;

        public Keys Key { get; }

        public override bool Condition(IInputProvider inputProvider)
        {
            switch (InputType)
            {
                case InputTypes.Down:
                    return inputProvider.IsDown(Key);
                case InputTypes.Up:
                    return inputProvider.IsUp(Key);
                case InputTypes.Pressed:
                    return inputProvider.IsPressed(Key);
                case InputTypes.Released:
                    return inputProvider.IsReleased(Key);
                case InputTypes.Held:
                    return inputProvider.IsHeld(Key, FramesHeldCount);
            }

            throw new ArgumentException("Could not handle input type " + InputType);
        }
    }

    public class MouseButtonCondition : InputCondition
    {
        public MouseButtonCondition(MouseButtons mouseButton, InputTypes inputType, int nFramesHeld = 0) : base(inputType, nFramesHeld) => MouseButton = mouseButton;

        public MouseButtons MouseButton { get; }

        public override bool Condition(IInputProvider inputProvider)
        {
            switch (InputType)
            {
                case InputTypes.Down:
                    return inputProvider.IsDown(MouseButton);
                case InputTypes.Up:
                    return inputProvider.IsUp(MouseButton);
                case InputTypes.Pressed:
                    return inputProvider.IsPressed(MouseButton);
                case InputTypes.Released:
                    return inputProvider.IsReleased(MouseButton);
                case InputTypes.Held:
                    return inputProvider.IsHeld(MouseButton, FramesHeldCount);
            }

            throw new ArgumentException("Could not handle input type " + InputType);
        }
    }

    public class GamePadButtonCondition : InputCondition
    {
        public GamePadButtonCondition(GamePadButtons gamePadButton, InputTypes inputType, int nFramesHeld = 0) : base(inputType, nFramesHeld) => GamePadButton = gamePadButton;

        public GamePadButtons GamePadButton { get; }

        public override bool Condition(IInputProvider inputProvider)
        {
            switch (InputType)
            {
                case InputTypes.Down:
                    return inputProvider.IsDown(GamePadButton);
                case InputTypes.Up:
                    return inputProvider.IsUp(GamePadButton);
                case InputTypes.Pressed:
                    return inputProvider.IsPressed(GamePadButton);
                case InputTypes.Released:
                    return inputProvider.IsReleased(GamePadButton);
                case InputTypes.Held:
                    return inputProvider.IsHeld(GamePadButton, FramesHeldCount);
            }

            throw new ArgumentException("Could not handle input type " + InputType);
        }
    }
}
