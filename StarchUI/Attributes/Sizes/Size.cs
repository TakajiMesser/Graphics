using StarchUICore.Attributes.Units;

namespace StarchUICore.Attributes.Sizes
{
    public struct Size
    {
        public Size(IUnits width, IUnits height, IUnits minimumWidth, IUnits minimumHeight, IUnits maximumWidth, IUnits maximumHeight)
        {
            Width = width;
            Height = height;
            MinimumWidth = minimumWidth;
            MinimumHeight = minimumHeight;
            MaximumWidth = maximumWidth;
            MaximumHeight = maximumHeight;
        }

        public IUnits Width { get; }
        public IUnits Height { get; }

        public IUnits MinimumWidth { get; private set; }
        public IUnits MinimumHeight { get; private set; }

        public IUnits MaximumWidth { get; private set; }
        public IUnits MaximumHeight { get; private set; }

        public int GetConstrainedWidth(int availableWidth, int dockWidth)
        {
            var constrainedWidth = Width.ToDimensionPixels(availableWidth, dockWidth);

            constrainedWidth = MinimumWidth.ConstrainAsMinimum(constrainedWidth, dockWidth);
            constrainedWidth = MaximumWidth.ConstrainAsMinimum(constrainedWidth, dockWidth);

            return constrainedWidth;
        }

        public int GetConstrainedHeight(int availableHeight, int dockHeight)
        {
            var constrainedHeight = Height.ToDimensionPixels(availableHeight, dockHeight);

            constrainedHeight = MinimumHeight.ConstrainAsMinimum(constrainedHeight, dockHeight);
            constrainedHeight = MaximumHeight.ConstrainAsMinimum(constrainedHeight, dockHeight);

            return constrainedHeight;
        }

        public Size Dimension(IUnits width, IUnits height) => new Size(width, height, MinimumWidth, MinimumHeight, MaximumWidth, MaximumHeight);

        public Size DimensionMinimums(IUnits minimumWidth, IUnits minimumHeight) => new Size(Width, Height, minimumWidth, minimumHeight, MaximumWidth, MaximumHeight);

        public Size DimensionMaximums(IUnits maximumWidth, IUnits maximumHeight) => new Size(Width, Height, MinimumWidth, MinimumHeight, maximumWidth, maximumHeight);

        public static Size FromDimensions(IUnits width, IUnits height) => new Size(width, height, Unit.Auto(), Unit.Auto(), Unit.Auto(), Unit.Auto());
    }
}
