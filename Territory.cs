using System;
using System.Drawing;

namespace ContinentMapCreator
{
    public class Territory
    { 
        // Properties
        //public int MaxRadiusX { get; set; }
        //public int MaxRadiusY { get; set; }
        public string Name { get; set; }
        public bool IsCoastal { get; set; }
        public Point Origin { get; set; }
        public int MajorRadius { get; set; }
        public int MinorRadius { get; set; }
        public Point Focus1 { get; set; }
        public Point Focus2 { get; set; }
        public int DistanceFociToBorder { get; set; }
        public bool MajorAxisIsX { get; set; }

        // Constructor
        public Territory(Point origin, int rad1, int rad2)
        {
            //MaxRadiusX = rad1;
            //MaxRadiusY = rad2;

            Origin = origin;
            IsCoastal = false;

            // Major and minor radii set as the larger and smaller supplied radius
            MajorRadius = Math.Max(rad1, rad2);
            MinorRadius = Math.Min(rad1, rad2);

            // The distance (c) to the foci is c^2 = a^2 - b^2, where a is the major radus and b is the minor radius
            int focusDistance = (int)Math.Sqrt(Math.Pow(MajorRadius, 2) - Math.Pow(MinorRadius, 2));

            // Choose at random whether the major radius is the x or y axis
            // For whichever axis chosen, place the foci along that axis at +/- focusDistacne from the origin
            // Set the sum of distance between both foci and a point on the border using a vertex
            if (new Random().Next(0, 2) == 0)
            {
                Focus1 = new Point(Origin.X - focusDistance, Origin.Y);
                Focus2 = new Point(Origin.X + focusDistance, Origin.Y);
                DistanceFociToBorder = 2 * MajorRadius;
                MajorAxisIsX = true;
            }
            else
            {
                Focus1 = new Point(Origin.X, Origin.Y - focusDistance);
                Focus2 = new Point(Origin.X, Origin.Y + focusDistance);
                DistanceFociToBorder = 2 * MajorRadius;
                MajorAxisIsX = false;
            }
        }

        // Support Methods
        public int SumDistanceToFoci(Point point)
        {
            double distanceFocus1 = Math.Sqrt(Math.Pow(Focus1.X - point.X, 2) + Math.Pow(Focus1.Y - point.Y, 2));
            double distanceFocus2 = Math.Sqrt(Math.Pow(Focus2.X - point.X, 2) + Math.Pow(Focus2.Y - point.Y, 2));
            double focusDistance = distanceFocus1 + distanceFocus2;

            return (int)focusDistance;
        }

        // Methods
        public bool IsOnTerritoryEdge(Point point)
        {
            int focusDistance = SumDistanceToFoci(point);
            if (focusDistance == DistanceFociToBorder)
            {
                return true;
            }

            return false;
        }

        public bool IsWithinTerritoryBounds(Point point)
        {
            int focusDistance = SumDistanceToFoci(point);
            if (focusDistance <= DistanceFociToBorder)
            {
                return true;
            }

            return false;
        }

        // Returns distance from origin to supplied point
        public int DistanceFromOrigin(Point point)
        {
            return (int)Math.Sqrt(Math.Pow(Origin.X - point.X, 2) + Math.Pow(Origin.Y - point.Y, 2));
        }
    }
}
