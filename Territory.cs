using System;
using System.Drawing;

namespace ContinentMapCreator
{
    public class Territory
    {
        // Properties
        public string Name { get; set; }
        public Brush Colour { get; set; }
        public Point Origin { get; set; }
        public int Radius { get; set; }
        public bool Coastal { get; set; }

        // Constructor
        public Territory(string name, Brush colour, Point origin, int radius)
        {
            Name = name;
            Colour = colour;
            Origin = origin;
            Radius = radius;
            Coastal = false;
        }

        // Methods
        public int OriginToPoint(Point point)
        {
            return (int)Math.Sqrt(Math.Pow(Origin.X - point.X, 2) + Math.Pow(Origin.Y - point.Y, 2));
        }

        public bool BorderContains(Point point)
        {
            return OriginToPoint(point) == Radius ? true : false;
        }

        public bool BoundsContains(Point point)
        {
            return OriginToPoint(point) < Radius ? true : false;
        }

        public void Draw(Graphics g)
        {
            int xOffset = Origin.X - Radius;
            int yOffset = Origin.Y - Radius;
            g.FillEllipse(Colour, xOffset, yOffset, 2 * Radius, 2 * Radius);
        }

    }
}
