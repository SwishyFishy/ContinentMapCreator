using System;
using System.Windows.Forms;
using System.Drawing;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    { 
        private void UpdateTrackbarDisplayLabels()
        {
            lbl_TerritoryFrequencyBaseDisplay.Text = trk_TerritoryFrequencyBase.Value.ToString();
            lbl_TerritoryFrequencyVariationDisplay.Text = trk_TerritoryFrequencyVariation.Value.ToString();
            lbl_TerritoryRadiusBaseDisplay.Text = trk_TerritoryRadiusBase.Value.ToString();
            lbl_TerritoryRadiusVariationDisplay.Text = trk_TerritoryRadiusVariation.Value.ToString();
            lbl_LakeFrequencyBaseDisplay.Text = trk_LakeFrequencyBase.Value.ToString();
            lbl_LakeFrequencyVariationDisplay.Text = trk_LakeFrequencyVariation.Value.ToString();
            lbl_LakeRadiusBaseDisplay.Text = trk_LakeRadiusBase.Value.ToString();
            lbl_LakeRadiusVariationDisplay.Text = trk_LakeRadiusVariation.Value.ToString();
            lbl_RiverFrequencyBaseDisplay.Text = trk_RiverFrequencyBase.Value.ToString();
            lbl_RiverFrequencyVariationDisplay.Text = trk_RiverFrequencyVariation.Value.ToString();
            lbl_RiverThicknessBaseDisplay.Text = trk_RiverThicknessBase.Value.ToString();
            lbl_RiverThicknessVariationDisplay.Text = trk_RiverThicknessVariation.Value.ToString();
            lbl_RiverCurvatureBaseDisplay.Text = trk_RiverCurvatureBase.Value.ToString();
            lbl_RiverCurvatureVariationDisplay.Text = trk_RiverCurvatureVariation.Value.ToString();

        }

        private void UpdateDisplay()
        {
            // BORDER_THICKNESS & BORDER_OFFSET based on trk_BorderThickness
            BORDER_THICKNESS = (float)(trk_BorderThickness.Value / 10.0);
            BORDER_OFFSET = BORDER_THICKNESS / 2.0F;

            // LOCATION_MARKER_THICKNESS & LOCATION_MARKER_OFFSET based on trk_LocationThickness
            LOCATION_MARKER_THICKNESS = (float)(trk_LocationThickness.Value / 10.0F);
            LOCATION_MARKER_OFFSET = LOCATION_MARKER_THICKNESS / 2.0F;

            // ROUGH_BORDERS based on chb_CleanBorders
            ROUGH_BORDERS = !chb_CleanBorders.Checked;

            // Redraw map
            paintMap = true;
            Refresh();
            paintMap = false;
        }

        public override void Refresh()
        {
            if (paintMap && mapExists)
            {
                // Disable controls
                pnl_SettingsBackground.Enabled = false;

                // Draw
                pnl_MapBackground.Refresh();

                // Reenable controls
                pnl_SettingsBackground.Enabled = true;
            }

            if (paintSettings)
            {
                pnl_SettingsBackground.Refresh();
            }
        }

        // Draw the map
        private void DrawMap(Graphics g)
        {
            Pen borderPen = new Pen(BORDER_COLOUR, BORDER_THICKNESS);
            Pen locationPen = new Pen(LOCATION_COLOUR, LOCATION_MARKER_THICKNESS);
            float xOffset;
            float yOffset;

            // Draw Ocean Backdrop
            g.FillRectangle(WATER_COLOUR, 0, 0, MAP_WIDTH, MAP_HEIGHT);

            // Draw Territories
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                Territories[i].Draw(g);
            }

            // Draw Lakes
            for (int i = 0; i < NUM_LAKES; i++)
            {
                Lakes[i].Draw(g);
            }

            // Draw Oceans
            if (FULL_CONTINENT)
            {
                // Oceans at top and bottom of screen
                for (int i = 0; i < HorizontalOceans.Length; i++)
                {
                    HorizontalOceans[i].Draw(g, true);
                }

                // Oceans at left and right of screen
                for (int i = 0; i < VerticalOceans.Length; i++)
                {
                    VerticalOceans[i].Draw(g, false);
                }
            }

            // Draw Rivers
            for (int i = 0; i < NUM_RIVERS; i++)
            {
                Rivers[i].Draw(g);
            }

            // Draw Borders
            for (int i = 0; i < TerritoryBorders.Length; i++)
            {
                Random rnd = new Random();
                xOffset = TerritoryBorders[i].X + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0) - BORDER_OFFSET;
                yOffset = TerritoryBorders[i].Y + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0) - BORDER_OFFSET;
                g.DrawRectangle(borderPen, xOffset, yOffset, BORDER_THICKNESS, BORDER_THICKNESS);
            }

            // Draw TerritoryOrigins and Territory names
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                xOffset = Territories[i].Origin.X - LOCATION_MARKER_OFFSET;
                yOffset = Territories[i].Origin.Y - LOCATION_MARKER_OFFSET;
                g.DrawRectangle(locationPen, xOffset, yOffset, LOCATION_MARKER_THICKNESS, LOCATION_MARKER_THICKNESS);
                g.DrawString(Territories[i].Name, DISPLAY_FONT, LOCATION_COLOUR, Territories[i].Origin.X, Territories[i].Origin.Y);
            }

            borderPen.Dispose();
            locationPen.Dispose();
        }
    }
}
