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
        static int NUM_TERRITORIES = 16;
        static int MIN_TERRITORY_RADIUS = 200;
        static int MAX_TERRITORY_RADIUS = 200;
        static int MIN_ORIGIN_SPACING = 5;

        // Aesthetic Settings
        static bool ROUGH_BORDERS = true;
        static Font DISPLAY_FONT = new Font("Carlito", 12, FontStyle.Bold);
        static Brush LAND_COLOUR = Brushes.Bisque;
        static Brush OCEAN_COLOUR = Brushes.Blue;
        static Brush NAME_COLOUR = Brushes.Black;
        static Brush BORDER_COLOUR = Brushes.Black;
        static float BORDER_THICKNESS = 3.0F;
        static float BORDER_OFFSET = 1.5F;

        // Generation args
        bool allowPainting = false;
        Territory[] Territories;
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
        }
        private void UpdateDisplaySettings()
        {
            // ROUGH_BORDERS based on chb_CleanBorders
            ROUGH_BORDERS = chb_CleanBorders.Checked ? false : true;
            DISPLAY_FONT = fntd_FontSelector.Font;

            // BORDER_OFFSET based on BORDER_THICKNESS
            BORDER_OFFSET = BORDER_THICKNESS / 2.0F;

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
            // Pick random points for territory origins
            Random rnd = new Random();
            NUM_TERRITORIES = rnd.Next(MIN_NUM_TERRITORIES, MAX_NUM_TERRITORIES + 1);
            Territories = new Territory[NUM_TERRITORIES];
            int radius = rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS);

            // TODO: Use MIN_ORIGIN_SPACING

            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                // Set territory origin boundaries
                int minX = 0;
                int maxX = pnl_MapBackground.Width;
                int minY = 0;
                int maxY = pnl_MapBackground.Height;

                // Generate random (x, y) coordinates for this origin that are sufficiently distanced from all others
                Point origin = new Point();
                bool spacingVerified = false;
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
                Territories[i] = new Territory(origin, radius);
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
                    // Get the distance between this point and each TerritoryOrigin
                    // -1 indicates that the point is too far away from that TerritoryOrigin to be inside it
                    // As the loop progresses, track which indices in distancesToOrigins are those of the closest and second-closest territories
                    Point thisPixel = new Point(x, y);
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

                    // If no TerritoryOrigins are close enough for that Territory to contain this point, continue
                    if (distancesToOrigins[closestOriginIndex] > Territories[closestOriginIndex].MaxRadius)
                    {
                        continue;
                    }
                    // If only one TerritoryOrigin is close enough for that Territory to contain this point, make it a border if it is on the edge
                    else if (
                        distancesToOrigins[closestOriginIndex] == Territories[closestOriginIndex].MaxRadius && 
                        distancesToOrigins[secondClosestOriginIndex] > Territories[secondClosestOriginIndex].MaxRadius)
                    {
                        PointsOnBorder[numBorderPoints] = thisPixel;
                        numBorderPoints++;
                        Territories[closestOriginIndex].IsCoastal = true;
                    }
                    // If two TerritoryOrigins are close enough for those Territories to contain this point, mark it a border if it is equidstant from both Origins
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

            // Draw TerritoryOrigins and Territory names
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                xOffset = Territories[i].Origin.X - BORDER_OFFSET;
                yOffset = Territories[i].Origin.Y - BORDER_OFFSET;
                e.Graphics.DrawRectangle(borderPen, xOffset, yOffset, BORDER_THICKNESS, BORDER_THICKNESS);
                e.Graphics.DrawString(i.ToString(), DISPLAY_FONT, NAME_COLOUR, Territories[i].Origin.X, Territories[i].Origin.Y);
            }

            // Draw Borders
            for (int i = 0; i < TerritoryBorders.Length; i++)
            {
                Random rnd = new Random();
                xOffset = TerritoryBorders[i].X + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0) - BORDER_OFFSET;
                yOffset = TerritoryBorders[i].Y + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0) - BORDER_OFFSET;
                e.Graphics.DrawRectangle(borderPen, xOffset, yOffset, BORDER_THICKNESS, BORDER_THICKNESS);
            }

            borderPen.Dispose();
        }

        private void btn_FontSelector_Click(object sender, EventArgs e)
        {

        }
    }
}
