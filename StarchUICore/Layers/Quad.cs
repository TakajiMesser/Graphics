using StarchUICore.Attributes;

namespace StarchUICore.Layers
{
    public struct Quad
    {
        public Quad(Position position, Size size)
        {
            Position = position;
            Size = size;
        }

        public Quad(int x, int y, int width, int height)
        {
            Position = new Position(x, y);
            Size = new Size(width, height);
        }

        public Position Position { get; set; }
        public Size Size { get; set; }

        public int MinX => Position.X;
        public int MinY => Position.Y;
        public int MaxX => Position.X + Size.Width;
        public int MaxY => Position.Y + Size.Height;

        public Position TopLeft => new Position(MinX, MinY);
        public Position TopRight => new Position(MaxX, MinY);
        public Position BottomLeft => new Position(MinX, MaxY);
        public Position BottomRight => new Position(MaxX, MaxY);

        public bool Overlaps(Quad quad) => (MinX <= quad.MinX && MaxX >= quad.MinX || MaxX >= quad.MaxX && MinX <= quad.MaxX)
            && MinY <= quad.MinY && MaxY >= quad.MinY || MaxY >= quad.MaxY && MinY <= quad.MaxY;

        public bool Contains(Quad quad) => MinX <= quad.MinX && MaxX >= quad.MaxX && MinY <= quad.MinY && MaxY >= quad.MaxY;

        public Quad Combine(Quad quad)
        {
            var minX = MinX <= quad.MinX ? MinX : quad.MinX;
            var minY = MinY <= quad.MinY ? MinY : quad.MinY;
            var maxX = MaxX >= quad.MaxX ? MaxX : quad.MaxX;
            var maxY = MaxY >= quad.MaxY ? MaxY : quad.MaxY;

            return new Quad(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
