using StarchUICore.Attributes.Units;

namespace StarchUICore.Attributes.Positions
{
    public struct Position
    {
        public Position(IUnits x, IUnits y, IUnits minimumX, IUnits minimumY, IUnits maximumX, IUnits maximumY)
        {
            X = x;
            Y = y;
            MinimumX = minimumX;
            MinimumY = minimumY;
            MaximumX = maximumX;
            MaximumY = maximumY;
        }

        public IUnits X { get; private set; }
        public IUnits Y { get; private set; }

        public IUnits MinimumX { get; private set; }
        public IUnits MinimumY { get; private set; }

        public IUnits MaximumX { get; private set; }
        public IUnits MaximumY { get; private set; }

        public int GetConstrainedX(int availableX, int anchorWidth)
        {
            var constrainedX = X.ToOffsetPixels(availableX, anchorWidth);

            constrainedX = MinimumX.ConstrainAsMinimum(constrainedX, anchorWidth);
            constrainedX = MaximumX.ConstrainAsMaximum(constrainedX, anchorWidth);

            return constrainedX;
        }

        public int GetConstrainedY(int availableY, int anchorHeight)
        {
            var constrainedY = Y.ToOffsetPixels(availableY, anchorHeight);

            constrainedY = MinimumY.ConstrainAsMinimum(constrainedY, anchorHeight);
            constrainedY = MaximumY.ConstrainAsMaximum(constrainedY, anchorHeight);

            return constrainedY;
        }

        public Position Offset(IUnits x, IUnits y) => new Position(x, y, MinimumX, MinimumY, MaximumX, MaximumY);

        public Position OffsetMinimums(IUnits minimumX, IUnits minimumY) => new Position(X, Y, minimumX, minimumY, MaximumX, MaximumY);

        public Position OffsetMaximums(IUnits maximumX, IUnits maximumY) => new Position(X, Y, MinimumX, MinimumY, maximumX, maximumY);

        public static Position FromOffsets(IUnits x, IUnits y) => new Position(x, y, Unit.Auto(), Unit.Auto(), Unit.Auto(), Unit.Auto());
    }
}
