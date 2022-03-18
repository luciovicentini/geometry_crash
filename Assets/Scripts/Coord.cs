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
        public int x { get; set; }
        public int y { get; set; }


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

        internal List<List<Coord>> Get3MLineInAllDirecctions()
        {
            List<List<Coord>> listLine = new List<List<Coord>>();
            listLine.Add(GetLineUp());
            listLine.Add(GetLineDown());
            listLine.Add(GetLineLeft());
            listLine.Add(GetLineRight());
            listLine.Add(GetLineCenter(LineType.Horizontal));
            listLine.Add(GetLineCenter(LineType.Vertical));

            return listLine;
        }

        private List<Coord> GetLineRight() => CreateLine(3, LineType.Horizontal);

        private List<Coord> GetLineLeft() => AddX(-2).CreateLine(3, LineType.Horizontal);

        private List<Coord> GetLineDown() => AddY(-2).CreateLine(3, LineType.Vertical);

        private List<Coord> GetLineUp() => CreateLine(3, LineType.Vertical);

        private List<Coord> GetLineCenter(LineType type)
        {
            Coord newOrigin;

            switch (type)
            {
                case LineType.Horizontal:
                    newOrigin = AddX(-1);
                    break;
                case LineType.Vertical:
                    newOrigin = AddY(-1);
                    break;
                default:
                    newOrigin = null;
                    break;
            }

            return newOrigin.CreateLine(3, type);
        }
    }
}