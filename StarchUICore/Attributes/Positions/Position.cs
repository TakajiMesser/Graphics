using StarchUICore.Attributes.Units;

namespace StarchUICore.Attributes.Positions
{
    public struct Position
    {
        public Position(IUnits x, IUnits y)
        {
            X = x;
            Y = y;
        }

        public IUnits X { get; }
        public IUnits Y { get; }
    }
}
