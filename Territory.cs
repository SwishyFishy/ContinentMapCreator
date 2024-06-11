using System;
using System.Drawing;

namespace ContinentMapCreator
{
    public class Territory
    { 
        // Properties
        public string Name { get; set; }
        public bool IsCoastal { get; set; }
        public Point Origin { get; set; }
        public int MaxRadius { get; set; }

        // Constructor
        public Territory(Point origin, int maxRadius)
        {
            Origin = origin;
            MaxRadius = maxRadius;
            IsCoastal = false;
        }

        // Methods
        public int OriginToPoint(Point point)
        {
            return (int)Math.Sqrt(Math.Pow(Origin.X - point.X, 2) + Math.Pow(Origin.Y - point.Y, 2));
        }

        public bool TerritoryEdgeContains(Point point)
        {
            return OriginToPoint(point) == MaxRadius ? true : false;
        }

        public bool TerritoryBoundsContains(Point point)
        {
            return OriginToPoint(point) <= MaxRadius ? true : false;
        }
    }
}
