using System;
using System.Collections.Generic;

namespace CustomUtil
{
    public enum LineType
    {
        Horizontal,
        Vertical,
    }

    public class Coord
    {
        public int x {get; set;}
        public int y {get; set;}


        public Coord()
        {
        }

        public Coord(int y, int x)
        {
            this.x = x;
            this.y = y;
        }

        public Coord AddX(int v) => new Coord(this.y, this.x + v);

        public Coord AddY(int v) => new Coord(this.y + v, this.x);

        public override string ToString()
        {
            return $"({this.y},{this.x})";
        }

        public List<Coord> CreateLine(int length, LineType type)
        {
            List<Coord> line = new List<Coord>();
            for (int i = 0; i < length; i++)
            {
                if (type == LineType.Horizontal)
                {
                    line.Add(this.AddX(i));
                }
                if (type == LineType.Vertical)
                {
                    line.Add(this.AddY(i));
                }
            }
            return line;
        }

        public static string ListDebugging(List<Coord> coords)
        {
            string result = "[";
            foreach (Coord item in coords)
            {
                result += item.ToString() + ", ";
            }
            result += "]";
            return result;
        }
    }
}