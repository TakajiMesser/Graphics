using SpiceEngineCore.Utilities;
using StarchUICore.Helpers;
using System;

namespace StarchUICore.Attributes
{
    public struct Size
    {
        public Size(int width, int height, UIUnitTypes widthUnits = UIUnitTypes.Pixels, UIUnitTypes heightUnits = UIUnitTypes.Pixels)
        {
            if (!widthUnits.IsAbsolute() && !width.IsBetweenInclusive(0, 100)) throw new ArgumentOutOfRangeException(nameof(width) + " must be between 0 and 100 percentage points");
            if (!heightUnits.IsAbsolute() && !height.IsBetweenInclusive(0, 100)) throw new ArgumentOutOfRangeException(nameof(height) + " must be between 0 and 100 percentage points");

            Width = width;
            Height = height;

            WidthUnits = widthUnits;
            HeightUnits = heightUnits;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public UIUnitTypes WidthUnits { get; private set; }
        public UIUnitTypes HeightUnits { get; private set; }
    }
}
