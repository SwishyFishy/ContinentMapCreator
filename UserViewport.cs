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
        static bool ROUGH_BORDERS = true;
        static int MIN_NUM_TERRITORIES = 12;
        static int MAX_NUM_TERRITORIES = 18;
        static int NUM_TERRITORIES = 16;
        static int MIN_TERRITORY_RADIUS = 200;
        static int MAX_TERRITORY_RADIUS = 200;

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
            tip_SettingsDetails.SetToolTip(lbl_TerritoryRadius, "Indicates the high and low bounds of 1/2 the maximum allowed distance across a territory");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryCount, "Indicates the high and low bounds of the range of number of territories");
            tip_SettingsDetails.SetToolTip(chb_CleanBorders, "Indicates whether borders should be drawn cleanly or roughly");

            // Add Controls
            pnl_SettingsBackground.Controls.Add(nud_TerritoryRadiusBound1);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryRadiusBound2);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryCountBound1);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryCountBound2);

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
        }
        private void UpdateDisplaySettings()
        {
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
            // Pick random points for territory origins
            Random rnd = new Random();
            NUM_TERRITORIES = rnd.Next(MIN_NUM_TERRITORIES, MAX_NUM_TERRITORIES + 1);
            Territories = new Territory[NUM_TERRITORIES];

            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                // Set territory origin boundaries
                int minX = 0;
                int maxX = pnl_MapBackground.Width;
                int minY = 0;
                int maxY = pnl_MapBackground.Height;

                Point origin = new Point(rnd.Next(minX, maxX), rnd.Next(minY, maxY));
                Territories[i] = new Territory(origin, rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS), rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS));
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
                    int[] distancesToFoci = new int[NUM_TERRITORIES];
                    int closestFociIndex = -1;
                    int secondClosestFociIndex = -1;
                    for (int i = 0; i < NUM_TERRITORIES; i++)
                    {
                        distancesToFoci[i] = Territories[i].SumDistanceToFoci(thisPixel);

                        // Track two closest territories
                        if (closestFociIndex == -1)
                        {
                            closestFociIndex = i;
                        }
                        else if (distancesToFoci[closestFociIndex] > distancesToFoci[i])
                        {
                            secondClosestFociIndex = closestFociIndex;
                            closestFociIndex = i;
                        }
                        else if (secondClosestFociIndex == -1)
                        {
                            secondClosestFociIndex = i;
                        }
                        else if (distancesToFoci[secondClosestFociIndex] > distancesToFoci[i])
                        {
                            secondClosestFociIndex = i;
                        }
                    }

                    // If no TerritoryFoci are close enough for that Territory to contain this point, continue
                    if (distancesToFoci[closestFociIndex] > Territories[closestFociIndex].DistanceFociToBorder)
                    {
                        continue;
                    }
                    // If only one TerritoryFoci are close enough for that Territory to contain this point, make it a border if it is on the edge
                    else if (distancesToFoci[secondClosestFociIndex] > Territories[secondClosestFociIndex].DistanceFociToBorder &&
                        distancesToFoci[closestFociIndex] == Territories[closestFociIndex].DistanceFociToBorder)
                    {
                        PointsOnBorder[numBorderPoints] = thisPixel;
                        numBorderPoints++;
                        Territories[closestFociIndex].IsCoastal = true;
                    }
                    // If two TerritoryFoci are close enough for that Territory to contain this point and neither can contain the other's Origin
                    // make it a border if it is equidistant from both Territories' outermost bounds
                    else if (Territories[closestFociIndex].DistanceFociToBorder - distancesToFoci[closestFociIndex] ==
                        Territories[secondClosestFociIndex].DistanceFociToBorder - distancesToFoci[secondClosestFociIndex])
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
                if (Territories[i].MajorAxisIsX)
                {
                    e.Graphics.FillEllipse(Brushes.Bisque,
                      Territories[i].Origin.X - Territories[i].MajorRadius,
                      Territories[i].Origin.Y - Territories[i].MinorRadius,
                      2 * Territories[i].MajorRadius,
                      2 * Territories[i].MinorRadius);
                }
                else
                {
                    e.Graphics.FillEllipse(Brushes.Bisque,
                      Territories[i].Origin.X - Territories[i].MinorRadius,
                      Territories[i].Origin.Y - Territories[i].MajorRadius,
                      2 * Territories[i].MinorRadius,
                      2 * Territories[i].MajorRadius);
                }
            }

            // Draw Territory Origins
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                e.Graphics.DrawRectangle(blackPen, Territories[i].Origin.X - 15, Territories[i].Origin.Y - 15, 30, 30);
                e.Graphics.DrawString(i.ToString(), new Font("Arial", 15), Brushes.Black, Territories[i].Origin.X - 10, Territories[i].Origin.Y - 10);
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
