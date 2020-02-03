using StarchUICore.Attributes.Sizes;
using StarchUICore.Attributes.Units;

namespace StarchUICore.Attributes.Positions
{
    public struct Padding
    {
        public Padding(IUnits left, IUnits top, IUnits right, IUnits bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public IUnits Left { get; }
        public IUnits Top { get; }
        public IUnits Right { get; }
        public IUnits Bottom { get; }

        public int GetWidth(int availableWidth)
        {
            var paddingLeft = Left.Constrain(availableWidth);
            var paddingRight = Right.Constrain(availableWidth);

            return paddingLeft + paddingRight;
        }

        public int GetHeight(int availableHeight)
        {
            var paddingTop = Top.Constrain(availableHeight);
            var paddingBottom = Bottom.Constrain(availableHeight);

            return paddingTop + paddingBottom;
        }

        public static Padding OnLeft(IUnits value) => new Padding(value, Unit.Auto(), Unit.Auto(), Unit.Auto());
        public static Padding OnTop(IUnits value) => new Padding(Unit.Auto(), value, Unit.Auto(), Unit.Auto());
        public static Padding OnRight(IUnits value) => new Padding(Unit.Auto(), Unit.Auto(), value, Unit.Auto());
        public static Padding OnBottom(IUnits value) => new Padding(Unit.Auto(), Unit.Auto(), Unit.Auto(), value);
        public static Padding OnAll(IUnits value) => new Padding(value, value, value, value);
        public static Padding Empty() => new Padding(Unit.Auto(), Unit.Auto(), Unit.Auto(), Unit.Auto());
    }
}
