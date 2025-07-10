namespace SnakeGame
{
    public class Coords
    {
        public int X { get; }
        public int Y { get; }

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coords Move(Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Coords(X, Y - 1),
                Direction.Down => new Coords(X, Y + 1),
                Direction.Left => new Coords(X - 1, Y),
                Direction.Right => new Coords(X + 1, Y),
                _ => this
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Coords other) return false;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}