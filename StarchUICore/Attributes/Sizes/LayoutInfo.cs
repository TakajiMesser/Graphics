using StarchUICore.Attributes.Positions;

namespace StarchUICore.Attributes.Sizes
{
    public struct LayoutInfo
    {
        public LayoutInfo(MeasuredSize size, LocatedPosition position)
        {
            Size = size;
            Position = position;
        }

        public MeasuredSize Size { get; }
        public LocatedPosition Position { get; }
    }
}
