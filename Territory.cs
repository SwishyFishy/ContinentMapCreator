using System;
using System.Drawing;

namespace ContinentMapCreator
{
    public class Territory
    { 
        // Properties
        public string Name { get; set; }
        public Point Origin { get; set; }
        public int MaxRadius { get; set; }
        public Territory[] LandNeighbours { get; set; }
        public Territory[] WaterNeighbours { get; set; }

        // Constructor
        public Territory(string name, Point origin, int maxRadius)
        {
            Name = name;
            Origin = origin;
            MaxRadius = maxRadius;
            LandNeighbours = new Territory[1];
            WaterNeighbours = new Territory[1];
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

        public void AddNeighbour(Territory neighbour, bool byLand)
        {
            // Add to LandNeighbours
            if (byLand)
            {
                // If neighbour is not in LandNeighbours, add it
                if (Array.Find(LandNeighbours, x => x == neighbour) == null)
                {
                    Territory[] temp = new Territory[LandNeighbours.Length + 1];
                    Array.Copy(LandNeighbours, temp, LandNeighbours.Length);
                    temp[LandNeighbours.Length] = neighbour;
                    LandNeighbours = temp;

                    // If neighbour is in WaterNeighbours, remove it
                    if (Array.Find(WaterNeighbours, x => x == neighbour) != null)
                    {
                        RemoveNeighbour(neighbour, false);
                    }
                }
            }
            // Add to WaterNeighbours
            else
            {
                // If neighbour is not in LandNeighbours or WaterNeighbours, add it
                if (Array.Find(WaterNeighbours, x => x == neighbour) == null && Array.Find(LandNeighbours, x => x == neighbour) == null)
                {
                    Territory[] temp = new Territory[WaterNeighbours.Length + 1];
                    Array.Copy(WaterNeighbours, temp, WaterNeighbours.Length);
                    temp[WaterNeighbours.Length] = neighbour;
                    WaterNeighbours = temp;
                }
            }
        }

        public void RemoveNeighbour(Territory neighbour, bool byLand)
        {
            // Remove from LandNeighbours
            if (byLand)
            {
                // If neighbour is in LandNeighbours, remove it
                int index = Array.IndexOf(LandNeighbours, neighbour);
                if (index != -1)
                {
                    Territory[] temp = new Territory[LandNeighbours.Length - 1];
                    Array.Copy(LandNeighbours, temp, index);
                    if (index < LandNeighbours.Length - 1)
                    {
                        Array.Copy(LandNeighbours, index + 1, temp, index, LandNeighbours.Length - index - 1);
                    }
                    LandNeighbours = temp;
                }
            }
            // Remove from WaterNeighbours
            else
            {
                // If neighbour is in WaterNeighbours, remove it
                int index = Array.IndexOf(WaterNeighbours, neighbour);
                if (index != -1)
                {
                    Territory[] temp = new Territory[WaterNeighbours.Length - 1];
                    Array.Copy(WaterNeighbours, temp, index);
                    if (index < WaterNeighbours.Length - 1)
                    {
                        Array.Copy(WaterNeighbours, index + 1, temp, index, WaterNeighbours.Length - index - 1);
                    }
                    WaterNeighbours = temp;
                }
            }
        }
    }
}
