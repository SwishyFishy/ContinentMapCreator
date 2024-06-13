using System;
using System.Windows.Forms;
using System.Drawing;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    {
        // Window Presentation Values
        const int WINDOW_WIDTH = 1600;
        const int WINDOW_HEIGHT = 900;
        const double SETTINGS_WIDTH_PROPORTION = 0.25;
        const double MAP_WIDTH_PROPORTION = (double)1 - SETTINGS_WIDTH_PROPORTION;

        // Map Creation Settings
        static int MIN_NUM_TERRITORIES = 12;
        static int MAX_NUM_TERRITORIES = 18;
        static int NUM_TERRITORIES;
        static int MIN_TERRITORY_RADIUS = 150;
        static int MAX_TERRITORY_RADIUS = 250;
        static int TERRITORY_RADIUS;
        static int MIN_ORIGIN_SPACING = 5;
        static int MIN_NUM_LAKES = 3;
        static int MAX_NUM_LAKES = 7;
        static int NUM_LAKES;
        static int MIN_LAKE_RADIUS = 25;
        static int MAX_LAKE_RADIUS = 75;

        // Aesthetic Settings
        static bool ROUGH_BORDERS = true;
        static Font DISPLAY_FONT = new Font("Carlito", 12, FontStyle.Bold);
        static SolidBrush LAND_COLOUR = new SolidBrush(Color.FromArgb(128, 128, 64));
        static SolidBrush OCEAN_COLOUR = new SolidBrush(Color.FromArgb(0, 128, 192));
        static SolidBrush LOCATION_COLOUR = new SolidBrush(Color.FromArgb(0, 0, 0));
        static SolidBrush BORDER_COLOUR = new SolidBrush(Color.FromArgb(0, 64, 0));
        static float BORDER_THICKNESS = 2.0F;
        static float BORDER_OFFSET = 1.0F;
        static float LOCATION_MARKER_THICKNESS;
        static float LOCATION_MAKRER_OFFSET;

        // Generation args
        bool allowPainting = false;
        Territory[] Territories;
        Lake[] Lakes;
        Point[,] DefiningLakePoints;
        Point[] PointsOnBorder = new Point[WINDOW_WIDTH * WINDOW_HEIGHT];
        Point[] TerritoryBorders;

        public form_Window()
        {
            InitializeComponent();
        }

        // Methods
        // Update the map generation settings based on user input
        private void UpdateGenerationSettings()
        {
            // MIN_TERRITORY_RADIUS & MAX_TERRITORY_RADIUS based on nud_TerritoryRadiusBound1 & nud_TerritoryRadiusBound2
            MIN_TERRITORY_RADIUS = Math.Min((int)nud_TerritoryRadiusBound1.Value, (int)nud_TerritoryRadiusBound2.Value);
            MAX_TERRITORY_RADIUS = Math.Max((int)nud_TerritoryRadiusBound1.Value, (int)nud_TerritoryRadiusBound2.Value);

            // MIN_NUM_TERRITORIES & MAX_NUM_TERRITORIES based on nud_TerritoryCountBound1 & nud_TerritoryCountBound2
            MIN_NUM_TERRITORIES = Math.Min((int)nud_TerritoryCountBound1.Value, (int)nud_TerritoryCountBound2.Value);
            MAX_NUM_TERRITORIES = Math.Max((int)nud_TerritoryCountBound1.Value, (int)nud_TerritoryCountBound2.Value);

            // MIN_ORIGIN_SPACING based on nud_MinimumOriginSpacing
            MIN_ORIGIN_SPACING = (int)nud_MinimumOriginSpacing.Value;

            // MIN_NUM_LAKES & MAX_NUM_LAKES based on nud_LakeCountBound1 & nud_LakeCountBound2
            MIN_NUM_LAKES = Math.Min((int)nud_LakeFrequencyBound1.Value, (int)nud_LakeFrequencyBound2.Value); 
            MAX_NUM_LAKES = Math.Max((int)nud_LakeFrequencyBound1.Value, (int)nud_LakeFrequencyBound2.Value);

            // MIN_LAKE_RADIUS & MAX_LAKE_RADIUS based on nud_LakeRadiusBound1 & nud_LakeRadiusBound2
            MIN_LAKE_RADIUS = Math.Min((int)nud_LakeRadiusBound1.Value, (int)nud_LakeRadiusBound2.Value);
            MAX_LAKE_RADIUS = Math.Max((int)nud_LakeRadiusBound1.Value, (int)nud_LakeRadiusBound2.Value);
        }
        private void UpdateDisplay()
        {
            // BORDER_THICKNESS & BORDER_OFFSET based on trk_BorderThickness
            BORDER_THICKNESS = (float)(trk_BorderThickness.Value / 10.0);
            BORDER_OFFSET = BORDER_THICKNESS / 2.0F;

            // ROUGH_BORDERS based on chb_CleanBorders
            ROUGH_BORDERS = chb_CleanBorders.Checked ? false : true;

            // Redraw map
            if (!lbl_NewWindowPrompt.Visible)
            {
                foreach (Control control in pnl_SettingsBackground.Controls)
                {
                    control.Enabled = false;
                }
                allowPainting = true;
                Refresh();
                allowPainting = false; 
                foreach (Control control in pnl_SettingsBackground.Controls)
                {
                    control.Enabled = true;
                }
            }
        }

        // Populate TerritoryOrigins array with random points
        private void GenerateTerritoryOrigins()
        {
            // Pick random points for Territory origins
            Random rnd = new Random();
            NUM_TERRITORIES = rnd.Next(MIN_NUM_TERRITORIES, MAX_NUM_TERRITORIES + 1);
            TERRITORY_RADIUS = rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS);
            Territories = new Territory[NUM_TERRITORIES];

            // Set Territory origin boundaries
            int minX = 0;
            int maxX = pnl_MapBackground.Width;
            int minY = 0;
            int maxY = pnl_MapBackground.Height;
            Point origin = new Point();
            bool spacingVerified;

            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                spacingVerified = false;

                // Generate random (x, y) coordinates for this origin that are sufficiently distanced from all others
                while (!spacingVerified)
                {
                    spacingVerified = true;
                    origin = new Point(rnd.Next(minX, maxX), rnd.Next(minY, maxY));
                    
                    // Check that origin is sufficiently distant from all others
                    for (int j = 0; j < i; j++)
                    {
                        if (Territories[j].OriginToPoint(origin) < MIN_ORIGIN_SPACING)
                        {
                            spacingVerified = false;
                        }
                    }
                }

                // Add a new Territory at the generated origin
                Territories[i] = new Territory(origin, TERRITORY_RADIUS);
            }
        }

        // Populate Lakes array
        private void GenerateLakes()
        {
            // Pick random points for Lake origins
            Random rnd = new Random();
            NUM_LAKES = rnd.Next(MIN_NUM_LAKES, MAX_NUM_LAKES);
            Lakes = new Lake[NUM_LAKES];
            DefiningLakePoints = new Point[4, NUM_LAKES];

            // Set Lake origin boundaries
            int minX = 0;
            int maxX = pnl_MapBackground.Width;
            int minY = 0;
            int maxY = pnl_MapBackground.Height;
            Point origin = new Point();
            int rad1;
            int rad2;
            double angle;
            bool spacingVerified;

            // Repeat once per lake
            for (int i = 0; i < NUM_LAKES; i++)
            {
                spacingVerified = false;

                // Generate random (x, y) coordinates for this origin that are sufficiently distanced from all others
                while (!spacingVerified)
                {
                    spacingVerified = true;

                    // Set the lake's origin, radii, and angle above the horizontal
                    origin = new Point(rnd.Next(minX, maxX), rnd.Next(minY, maxY));
                    rad1 = rnd.Next(MIN_LAKE_RADIUS, MAX_LAKE_RADIUS);
                    rad2 = rnd.Next(MIN_LAKE_RADIUS, MAX_LAKE_RADIUS);
                    angle = rnd.NextDouble();

                    // Add a new Lake at a random origin
                    Lakes[i] = new Lake(origin, rad1, rad2, angle);

                    // Check that lake does not contain any Territory origins
                    for (int j = 0; j < NUM_TERRITORIES; j++)
                    {
                        if (Lakes[i].LakeBoundsContains(Territories[j].Origin))
                        {
                            spacingVerified = false;
                        }
                    }
                }
            }
        }

        // Calculate territory borders and mark coastal territories as such
        // For each point, add it to the OwnedPoints of its closest TerritoryOrigin
        private void GenerateTerritoryBorders()
        {
            int numBorderPoints = 0;

            // Loop through all points
            for (int x = 0; x < pnl_MapBackground.Width; x++)
            {
                for (int y = 0; y < pnl_MapBackground.Height; y++)
                {
                    bool inLake = false;
                    bool onLakeBorder = false;
                    Point thisPixel = new Point(x, y);

                    // If this point is contained within a lake, move on
                    for (int i = 0; i < NUM_LAKES; i++)
                    {
                        if (Lakes[i].LakeBoundsContains(thisPixel))
                        {
                            inLake = true;
                        }
                        else if (Lakes[i].LakeBorderContains(thisPixel))
                        {
                            onLakeBorder = true;
                        }
                    }
                    if (inLake)
                    {
                        continue;
                    }

                    // Get the distance between this point and each TerritoryOrigin
                    // -1 indicates that the point is too far away from that TerritoryOrigin to be inside it
                    // As the loop progresses, track which indices in distancesToOrigins are those of the closest and second-closest territories
                    int[] distancesToOrigins = new int[NUM_TERRITORIES];
                    int closestOriginIndex = -1;
                    int secondClosestOriginIndex = -1;

                    // Populate the distancesToOrigins array
                    for (int i = 0; i < NUM_TERRITORIES; i++)
                    {
                        distancesToOrigins[i] = Territories[i].OriginToPoint(thisPixel);

                        // Track two closest territories
                        if (closestOriginIndex == -1)
                        {
                            closestOriginIndex = i;
                        }
                        else if (distancesToOrigins[closestOriginIndex] > distancesToOrigins[i])
                        {
                            secondClosestOriginIndex = closestOriginIndex;
                            closestOriginIndex = i;
                        }
                        else if (secondClosestOriginIndex == -1 || distancesToOrigins[secondClosestOriginIndex] > distancesToOrigins[i])
                        {
                            secondClosestOriginIndex = i;
                        }
                    }

                    // Point is too far from any Territory Origin to be land
                    if (distancesToOrigins[closestOriginIndex] > Territories[closestOriginIndex].MaxRadius)
                    {
                        continue;
                    }
                    // Point is on the border of a lake and is within the bounds of at least one Territory
                    else if (onLakeBorder && distancesToOrigins[closestOriginIndex] <= Territories[closestOriginIndex].MaxRadius)
                    {
                        PointsOnBorder[numBorderPoints] = thisPixel;
                        numBorderPoints++;
                        Territories[closestOriginIndex].IsCoastal = true;
                    }
                    // Point is exactly a Territory's Radius from its Origin, and is closer to that TErritory than any other.
                    else if (distancesToOrigins[closestOriginIndex] == Territories[closestOriginIndex].MaxRadius && 
                        distancesToOrigins[secondClosestOriginIndex] > Territories[secondClosestOriginIndex].MaxRadius)
                    {
                        PointsOnBorder[numBorderPoints] = thisPixel;
                        numBorderPoints++;
                        Territories[closestOriginIndex].IsCoastal = true;
                    }
                    // Point is equidistant from two Territory Origins, and is within the bounds of both Territories, and is closer to those two Territories than any others
                    else if (distancesToOrigins[secondClosestOriginIndex] <= Territories[secondClosestOriginIndex].MaxRadius && 
                        distancesToOrigins[closestOriginIndex] == distancesToOrigins[secondClosestOriginIndex])
                    {
                        PointsOnBorder[numBorderPoints] = thisPixel;
                        numBorderPoints++;
                    }
                }
            }

            // Shorten PointsOnBorder to actual size
            TerritoryBorders = new Point[numBorderPoints];
            Array.Copy(PointsOnBorder, TerritoryBorders, numBorderPoints);
        }

        // Draw the screen
        private void Draw(PaintEventArgs e)
        {
            Pen borderPen = new Pen(BORDER_COLOUR, BORDER_THICKNESS);
            Pen locationPen = new Pen(LOCATION_COLOUR, BORDER_THICKNESS);
            float xOffset;
            float yOffset;

            // Draw Ocean
            e.Graphics.FillRectangle(OCEAN_COLOUR, 0, 0, pnl_MapBackground.Width, pnl_MapBackground.Height);

            // Draw Territories
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                xOffset = Territories[i].Origin.X - Territories[i].MaxRadius;
                yOffset = Territories[i].Origin.Y - Territories[i].MaxRadius;
                e.Graphics.FillEllipse(LAND_COLOUR, xOffset, yOffset, 2 * Territories[i].MaxRadius, 2 * Territories[i].MaxRadius);
            }

            // Draw Lakes
            for (int i = 0; i < NUM_LAKES; i++)
            {
                e.Graphics.FillClosedCurve(OCEAN_COLOUR, Lakes[i].Vertices, System.Drawing.Drawing2D.FillMode.Alternate, 0.9F);
            }

            // Draw Borders
            for (int i = 0; i < TerritoryBorders.Length; i++)
            {
                Random rnd = new Random();
                xOffset = TerritoryBorders[i].X + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0) - BORDER_OFFSET;
                yOffset = TerritoryBorders[i].Y + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0) - BORDER_OFFSET;
                e.Graphics.DrawRectangle(borderPen, xOffset, yOffset, BORDER_THICKNESS, BORDER_THICKNESS);

            }
            
            // Draw TerritoryOrigins and Territory names
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                xOffset = Territories[i].Origin.X - BORDER_OFFSET;
                yOffset = Territories[i].Origin.Y - BORDER_OFFSET;
                e.Graphics.DrawRectangle(locationPen, xOffset, yOffset, BORDER_THICKNESS, BORDER_THICKNESS);
                e.Graphics.DrawString(i.ToString(), DISPLAY_FONT, LOCATION_COLOUR, Territories[i].Origin.X, Territories[i].Origin.Y);
            }

            borderPen.Dispose();
            locationPen.Dispose();
        }
    }
}
