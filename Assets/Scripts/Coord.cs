using System;

namespace CustomUtil
{
    public class Coord
    {
        int x;
        int y;

        public Coord()
        {
        }

        public Coord(int y, int x)
        {
            this.x = x;
            this.y = y;
        }

        public int GetX() => x;
        public int GetY() => y;

        public void SetX(int x) => this.x = x;
        public void SetY(int y) => this.y = y;

        public Coord AddX(int v)
        {
            return new Coord(this.y, this.x + v);
        }

        internal Coord AddY(int v)
        {
            return new Coord(this.y + v, this.x);
        }

        public override string ToString()
        {
            return $"({this.GetY()},{this.GetX()})";
        }
    }
}