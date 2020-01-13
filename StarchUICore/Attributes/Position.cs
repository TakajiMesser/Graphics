namespace StarchUICore.Attributes
{
    public struct Position
    {
        public Position(int x, int y, UIUnitTypes xUnits = UIUnitTypes.Pixels, UIUnitTypes yUnits = UIUnitTypes.Pixels)
        {
            X = x;
            Y = y;

            XUnits = xUnits;
            YUnits = yUnits;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public UIUnitTypes XUnits { get; private set; }
        public UIUnitTypes YUnits { get; private set; }
    }
}
