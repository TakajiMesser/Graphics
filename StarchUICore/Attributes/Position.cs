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

        public override bool Equals(object obj) => obj is Position position && X == position.X && Y == position.Y && XUnits == position.XUnits && YUnits == position.YUnits;
    }
}
