using StarchUICore.Attributes.Units;

namespace StarchUICore.Attributes.Sizes
{
    public struct Size
    {
        public Size(IUnits width, IUnits height)
        {
            Width = width;
            Height = height;
        }

        public IUnits Width { get; }
        public IUnits Height { get; }

        public static Size Auto() => new Size(Unit.Auto(), Unit.Auto());
    }
}
