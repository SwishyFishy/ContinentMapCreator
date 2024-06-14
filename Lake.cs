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
        public Point[] Vertices { get; set; }
        public Territory[] Neighbours { get; set; }

        // Constructor
        public Lake(String name, Point origin, int rad1, int rad2, double angle)
        {
            Name = name;
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
            int f1xRotation = (int)(Focus1.X * Math.Cos(angle) - Focus1.Y * Math.Sin(angle));
            int f1yRotation = (int)(Focus1.X * Math.Sin(angle) + Focus1.Y * Math.Cos(angle));

            int f2xRotation = (int)(Focus2.X * Math.Cos(angle) - Focus2.Y * Math.Sin(angle));
            int f2yRotation = (int)(Focus2.X * Math.Sin(angle) + Focus2.Y * Math.Cos(angle));

            Focus1 = new Point(f1xRotation + Origin.X, f1yRotation + Origin.Y);
            Focus2 = new Point(f2xRotation + Origin.X, f2yRotation + Origin.Y);

            // Assign the Vertices using the same parametric equations
            int v1xRotation = (int)(-MajorRadius * Math.Cos(angle) - 0 * Math.Sin(angle));
            int v1yRotation = (int)(-MajorRadius * Math.Sin(angle) + 0 * Math.Cos(angle));

            int v2xRotation = (int)(MajorRadius * Math.Cos(angle) - 0 * Math.Sin(angle));
            int v2yRotation = (int)(MajorRadius * Math.Sin(angle) + 0 * Math.Cos(angle));

            int v3xRotation = (int)(0 * Math.Cos(angle) - -MinorRadius * Math.Sin(angle));
            int v3yRotation = (int)(0 * Math.Sin(angle) + -MinorRadius * Math.Cos(angle));

            int v4xRotation = (int)(0 * Math.Cos(angle) - MinorRadius * Math.Sin(angle));
            int v4yRotation = (int)(0 * Math.Sin(angle) + MinorRadius * Math.Cos(angle));

            // Assign in clockwise order around circumference, starting with the vertex that is most western at angle = 0
            Vertices = new Point[4];
            Vertices[0] = new Point(v1xRotation + Origin.X, v1yRotation + Origin.Y);
            Vertices[1] = new Point(v3xRotation + Origin.X, v3yRotation + Origin.Y);
            Vertices[2] = new Point(v2xRotation + Origin.X, v2yRotation + Origin.Y);
            Vertices[3] = new Point(v4xRotation + Origin.X, v4yRotation + Origin.Y);

            Neighbours = new Territory[1];
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

        public void AddNeighbour(Territory neighbour)
        {
            // If neighbour is not in Neighbours, add it
            if (Array.Find(Neighbours, x => x == neighbour) == null)
            {
                Territory[] temp = new Territory[Neighbours.Length + 1];
                Array.Copy(Neighbours, temp, Neighbours.Length);
                temp[Neighbours.Length] = neighbour;
                Neighbours = temp;

                // Add neighbour to all other neighbours, and all other neighbours to neighbour
                // Neighbours[0] is always null, begin at second index
                for (int i = 1; i < Neighbours.Length - 1; i++)
                {
                    Neighbours[i].AddNeighbour(neighbour, false);
                    neighbour.AddNeighbour(Neighbours[i], false);
                }
            }
        }

        public void RemoveNeighbour(Territory neighbour)
        {
            // If neighbour is in Neighbours, remove it
            int index = Array.IndexOf(Neighbours, neighbour);
            if (index != -1)
            {
                Territory[] temp = new Territory[Neighbours.Length - 1];
                Array.Copy(Neighbours, 0, temp, 0, index);
                if (index < Neighbours.Length - 1)
                {
                    Array.Copy(Neighbours, index + 1, temp, index + 1, Neighbours.Length - index - 1);
                }
                Neighbours = temp;

                // Remove neighbour from all neighbours
                // Neighbours[0] is always null, begin at second index
                for (int i = 1; i < Neighbours.Length - 1; i++)
                {
                    Neighbours[i].RemoveNeighbour(neighbour, false);
                }
            }
        }
    }
}
