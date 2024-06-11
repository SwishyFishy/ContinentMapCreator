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
        static Font DISPLAY_FONT = new Font("Carlito", 12);

        // Generation args
        bool allowPainting = false;
        Territory[] Territories;
        Point[] PointsOnBorder = new Point[WINDOW_WIDTH * WINDOW_HEIGHT];
        Point[] TerritoryBorders;

        public form_Window()
        {
            InitializeComponent();
        }

        // Event Handlers
        // form_Window
        // Load         -> Set window proportions and load initial assets
        private void form_Window_Load(object sender, EventArgs e)
        {
            // Set window size
            Size windowSize = new Size(WINDOW_WIDTH, WINDOW_HEIGHT);
            this.Size = windowSize;
            this.MaximumSize = windowSize;
            this.MinimumSize = windowSize;
            this.Location = new Point(75, 75);

            // Set interior panel sizes
            pnl_SettingsBackground.Width = (int)(SETTINGS_WIDTH_PROPORTION * this.ClientSize.Width);
            pnl_SettingsBackground.Height = this.ClientSize.Height;
            pnl_SettingsBackground.Location = new Point(0, 0);

            pnl_MapBackground.Width = (int)(MAP_WIDTH_PROPORTION * this.ClientSize.Width);
            pnl_MapBackground.Height = this.ClientSize.Height;
            pnl_MapBackground.Location = new Point(pnl_SettingsBackground.Width, 0);

            // Add Tooltips
            tip_SettingsDetails.SetToolTip(lbl_TerritoryRadius, "Bounds 1/2 the maximum allowed distance across a territory.");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryCount, "Bounds the number of territories.");
            tip_SettingsDetails.SetToolTip(lbl_OriginSpacing, "Minimum distance between territory origin points. Higher values generate more uniform maps");

            tip_SettingsDetails.SetToolTip(chb_CleanBorders, "Draw borders cleanly or roughly");

            // Add Controls
            pnl_SettingsBackground.Controls.Add(nud_TerritoryRadiusBound1);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryRadiusBound2);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryCountBound1);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryCountBound2);
            pnl_SettingsBackground.Controls.Add(nud_MinimumOriginSpacing);

            pnl_SettingsBackground.Controls.Add(chb_CleanBorders);
            pnl_SettingsBackground.Controls.Add(btn_Generate);

            // Set control defaults
            UpdateDisplaySettings();
            btn_Generate.Enabled = true;
        }

        // btn_Generate
        // Click        -> Generate new map
        private void btn_Generate_Click(object sender, EventArgs e)
        {
            lbl_NewWindowPrompt.Visible = false;
            foreach (Control control in pnl_SettingsBackground.Controls)
            {
                control.Enabled = false;
            }
            UpdateGenerationSettings();
            GenerateTerritoryOrigins();
            GenerateTerritoryBorders();
            allowPainting = true;
            Refresh();
            allowPainting = false; 
            foreach (Control control in pnl_SettingsBackground.Controls)
            {
                control.Enabled = true;
            }
        }

        // chb_CleanBorders
        // Check        -> Redraw
        private void chb_CleanBorders_CheckChanged(object sender, EventArgs e)
        {
            UpdateDisplaySettings();
        }

        // pnl_MapBackground
        // Paint        -> Call Draw() method to redraw screen when refreshed
        // DoubleClick  -> Regenerate new territory origins
        private void pnl_MapBackground_Paint(object sender, PaintEventArgs e)
        {
            if (allowPainting)
            {
                Draw(e);
            }
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
            // Create pens
            Pen blackPen = new Pen(Brushes.Black);
            blackPen.Width = 3.0F;

            // Draw Ocean
            e.Graphics.FillRectangle(Brushes.Blue, 0, 0, pnl_MapBackground.Width, pnl_MapBackground.Height);

            // Draw Territories
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                e.Graphics.FillEllipse(Brushes.Bisque, 
                    Territories[i].Origin.X - Territories[i].MaxRadius,
                    Territories[i].Origin.Y - Territories[i].MaxRadius,
                    2 * Territories[i].MaxRadius,
                    2 * Territories[i].MaxRadius);
            }

            // Draw Territory Origins
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                e.Graphics.DrawRectangle(blackPen, Territories[i].Origin.X - 15, Territories[i].Origin.Y - 15, 30, 30);
                e.Graphics.DrawString(i.ToString(), DISPLAY_FONT, Brushes.Black, Territories[i].Origin.X - 10, Territories[i].Origin.Y - 10);
            }

            // Draw Borders
            for (int i = 0; i < TerritoryBorders.Length; i++)
            {
                Random rnd = new Random();
                e.Graphics.DrawRectangle(blackPen, TerritoryBorders[i].X + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0), 
                    TerritoryBorders[i].Y + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0), 1, 1);
            }

            blackPen.Dispose();
        }
    }
}
