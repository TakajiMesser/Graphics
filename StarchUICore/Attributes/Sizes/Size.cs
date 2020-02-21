using SpiceEngineCore.Utilities;
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

        public int? ConstrainWidth(int availableWidth, int? dockWidth)
        {
            if (dockWidth.HasValue)
            {
                var constrainedWidth = Width.ToDimensionPixels(availableWidth, dockWidth.Value);

                if (!(MinimumWidth is AutoUnits))
                {
                    constrainedWidth = constrainedWidth.ClampBottom(MinimumWidth.ToDimensionPixels(availableWidth, dockWidth.Value));
                }

                if (!(MaximumWidth is AutoUnits))
                {
                    constrainedWidth = constrainedWidth.ClampTop(MaximumWidth.ToDimensionPixels(availableWidth, dockWidth.Value));
                }

                return constrainedWidth;
            }
            else
            {
                return null;
            }
        }

        public int? ConstrainHeight(int availableHeight, int? dockHeight)
        {
            if (dockHeight.HasValue)
            {
                var constrainedHeight = Height.ToDimensionPixels(availableHeight, dockHeight.Value);

                if (!(MinimumHeight is AutoUnits))
                {
                    constrainedHeight = constrainedHeight.ClampBottom(MinimumWidth.ToDimensionPixels(availableHeight, dockHeight.Value));
                }

                if (!(MaximumHeight is AutoUnits))
                {
                    constrainedHeight = constrainedHeight.ClampTop(MaximumWidth.ToDimensionPixels(availableHeight, dockHeight.Value));
                }

                return constrainedHeight;
            }
            else
            {
                return null;
            }
        }

        //public static Size Auto() => new Size(Unit.Auto(), Unit.Auto());
        public static Size FromDimensions(IUnits width, IUnits height) => new Size(width, height, Unit.Auto(), Unit.Auto(), Unit.Auto(), Unit.Auto());
    }
}
