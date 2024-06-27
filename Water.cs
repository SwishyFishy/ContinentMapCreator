using System;
using System.Drawing;

namespace ContinentMapCreator
{
    public class Ocean
    {
        // Properties
        public string Name { get; set; }
        public Brush Colour { get; set; }
        public Point Origin { get; set; }
        public int MajorRadius { get; set; }
        public int MinorRadius { get; set; }
        public Point Focus1 { get; set; }
        public Point Focus2 { get; set; }
        public int FocalLength { get; set; }

        // Constructor
        public Ocean(string name, Brush colour, Point origin, int xrad, int yrad)
        {
            Name = name;
            Colour = colour;
            Origin = origin;
            MajorRadius = Math.Max(xrad, yrad);
            MinorRadius = Math.Min(xrad, yrad);

            // Determine focal length
            FocalLength = (int)Math.Sqrt(Math.Pow(MajorRadius, 2) - Math.Pow(MinorRadius, 2));

            // Set focal points
            if (xrad > yrad)
            {
                Focus1 = new Point(Origin.X + FocalLength, Origin.Y);
                Focus2 = new Point(Origin.X - FocalLength, Origin.Y);
            }
            else
            {
                Focus1 = new Point(Origin.X, Origin.Y + FocalLength);
                Focus2 = new Point(Origin.X, Origin.Y - FocalLength);
            }
        }

        // Methods
        public bool BorderContains(Point point)
        {
            double distanceToFocus1 = Math.Sqrt(Math.Pow(point.X - Focus1.X, 2) + Math.Pow(point.Y - Focus1.Y, 2));
            double distanceToFocus2 = Math.Sqrt(Math.Pow(point.X - Focus2.X, 2) + Math.Pow(point.Y - Focus2.Y, 2));
            int sumDistance = (int)(distanceToFocus1 + distanceToFocus2);

            return sumDistance == 2 * MajorRadius ? true : false;
        }

        public bool BoundsContains(Point point)
        {
            double distanceToFocus1 = Math.Sqrt(Math.Pow(point.X - Focus1.X, 2) + Math.Pow(point.Y - Focus1.Y, 2));
            double distanceToFocus2 = Math.Sqrt(Math.Pow(point.X - Focus2.X, 2) + Math.Pow(point.Y - Focus2.Y, 2));
            int sumDistance = (int)(distanceToFocus1 + distanceToFocus2);

            return sumDistance < 2 * MajorRadius ? true : false;
        }

        public void Draw(Graphics g, bool horizontal)
        {
            if (horizontal)
            {
                int xOffset = Origin.X - MajorRadius;
                int yOffset = Origin.Y - MinorRadius;
                g.FillEllipse(Colour, xOffset, yOffset, 2 * MajorRadius, 2 * MinorRadius);
            }
            else
            {
                int xOffset = Origin.X - MinorRadius;
                int yOffset = Origin.Y - MajorRadius;
                g.FillEllipse(Colour, xOffset, yOffset, 2 * MinorRadius, 2 * MajorRadius);
            }
        }
    }

    public class Lake : Ocean
    {
        public double Angle { get; set; }
        public Point[] Vertices { get; set; }

        // Constructor
        public Lake(string name, Brush colour, Point origin, int rad1, int rad2, double angle) : base(name, colour, origin, rad1, rad2)
        {
            // Angle is measured in radians, 2 * PI * angle. angle should be passed as a double between 0.0 and 1.0
            Angle = 2 * Math.PI * angle;

            // Reassign the focus points relative to the origin, so that they can be rotated
            // Assume angle = 0, x axis is major radius
            Focus1 = new Point(FocalLength, 0);
            Focus2 = new Point(-FocalLength, 0);

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
            // Assume MajorRadius lies on the x axis at angle = 0 to match up with initial Focus1 and Focus2 assignment
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
        }

        // Methods
        public void Draw(Graphics g)
        {
            float tension = MajorRadius / MinorRadius / 10;
            g.FillClosedCurve(Colour, Vertices, System.Drawing.Drawing2D.FillMode.Alternate, 0.85F + tension);

        }
    }

    public class River
    {
        // Properties
        public Point[] Points { get; set; }
        public Brush Colour { get; set; }
        public int Curvature { get; set; }
        public int Thickness { get; set; }

        // Constructor
        public River(Point source, Point sink, Point[] curves, Brush colour, int curvature, int thickness)
        {
            // Insert points
            Points = new Point[curves.Length + 2];
            Points[0] = source;
            Array.Copy(curves, 0, Points, 1, curves.Length);
            Points[Points.Length - 1] = sink;

            // Set other properties
            Colour = colour;
            Curvature = curvature;
            Thickness = thickness;
        }

        // Methods
        public void Draw(Graphics g)
        {
            Pen pen = new Pen(Colour);
            g.DrawCurve(pen, Points, Curvature);
            pen.Dispose();
        }
    }
}
