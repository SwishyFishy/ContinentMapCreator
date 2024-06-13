using System;
using System.Drawing;

namespace ContinentMapCreator
{
    public class Lake
    {
        // Properties
        public string Name { get; set; }
        public Point Origin { get; set; }
        public int MajorRadius { get; set; }
        public int MinorRadius { get; set; }
        public double Angle { get; set; }
        public Point Focus1 { get; set; }
        public Point Focus2 { get; set; }

        // Constructor
        public Lake(Point origin, int rad1, int rad2, double angle)
        {
            Origin = origin;
            MajorRadius = Math.Max(rad1, rad2);
            MinorRadius = Math.Min(rad1, rad2);
            Angle = 2 * Math.PI * angle;

            // Determine focal points assuming angle = 0
            int focalLength = (int)Math.Sqrt(Math.Pow(MajorRadius, 2) - Math.Pow(MinorRadius, 2));

            // Assign them relative to the origin, so that they can be rotated
            Focus1 = new Point(focalLength, 0);
            Focus2 = new Point(-focalLength, 0);

            // Rotate the focal points using the following parametric equations:
            // x' = x * cos(angle) - y * sin(angle)
            // y' = x * sin(angle) + y * cos(angle)
            int f1x = (int)(Focus1.X * Math.Cos(angle) - Focus1.Y * Math.Sin(angle));
            int f1y = (int)(Focus1.X * Math.Sin(angle) + Focus1.Y * Math.Cos(angle));

            int f2x = (int)(Focus2.X * Math.Cos(angle) - Focus2.Y * Math.Sin(angle));
            int f2y = (int)(Focus2.X * Math.Sin(angle) + Focus2.Y * Math.Cos(angle));

            // Assign final focal points as sum of rotated focal points and Origin 
            Focus1 = new Point(f1x + Origin.X, f1y + Origin.Y);
            Focus2 = new Point(f2x + Origin.X, f2y + Origin.Y);
        }

        // Methods
        public bool LakeBorderContains(Point point)
        {
            double distanceToFocus1 = Math.Sqrt(Math.Pow(point.X - Focus1.X, 2) + Math.Pow(point.Y - Focus1.Y, 2));
            double distanceToFocus2 = Math.Sqrt(Math.Pow(point.X - Focus2.X, 2) + Math.Pow(point.Y - Focus2.Y, 2));
            int sumDistance = (int)(distanceToFocus1 + distanceToFocus2);

            return sumDistance == 2 * MajorRadius ? true : false;
        }

        public bool LakeBoundsContains(Point point)
        {
            double distanceToFocus1 = Math.Sqrt(Math.Pow(point.X - Focus1.X, 2) + Math.Pow(point.Y - Focus1.Y, 2));
            double distanceToFocus2 = Math.Sqrt(Math.Pow(point.X - Focus2.X, 2) + Math.Pow(point.Y - Focus2.Y, 2));
            int sumDistance = (int)(distanceToFocus1 + distanceToFocus2);

            return sumDistance < 2 * MajorRadius ? true : false;
        }
    }
}
