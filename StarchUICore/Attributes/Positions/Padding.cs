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

        public int GetWidth(int availableWidth, int parentWidth)
        {
            var paddingLeft = Left is AutoUnits ? 0 : Left.ToDimensionPixels(availableWidth, parentWidth);
            var paddingRight = Right is AutoUnits ? 0 : Right.ToDimensionPixels(availableWidth, parentWidth);

            return paddingLeft + paddingRight;
        }

        public int GetHeight(int availableHeight, int parentHeight)
        {
            var paddingTop = Top is AutoUnits ? 0 : Top.ToDimensionPixels(availableHeight, parentHeight);
            var paddingBottom = Bottom is AutoUnits ? 0 : Bottom.ToDimensionPixels(availableHeight, parentHeight);

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
