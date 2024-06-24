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
            if (!lbl_TutorialSettingsPanel.Visible)
            {
                // Disable controls
                pnl_SettingsBackground.Enabled = false;

                // Redraw
                allowPainting = true;
                Refresh();
                allowPainting = false;

                // Reenable controls
                pnl_SettingsBackground.Enabled = true;
            }
        }

        // Draw the screen
        private void DrawMap(Graphics g)
        {
            Pen borderPen = new Pen(BORDER_COLOUR, BORDER_THICKNESS);
            Pen locationPen = new Pen(LOCATION_COLOUR, LOCATION_MARKER_THICKNESS);
            float xOffset;
            float yOffset;

            // Draw Ocean Backdrop
            g.FillRectangle(WATER_COLOUR, 0, 0, pnl_MapBackground.Width, pnl_MapBackground.Height);

            // Draw Territories
            for (int i = 0; i < Territories.Length; i++)
            {
                xOffset = Territories[i].Origin.X - Territories[i].Radius;
                yOffset = Territories[i].Origin.Y - Territories[i].Radius;
                g.FillEllipse(LAND_COLOUR, xOffset, yOffset, 2 * Territories[i].Radius, 2 * Territories[i].Radius);
            }

            // Draw Lakes
            for (int i = 0; i < Lakes.Length; i++)
            {
                g.FillClosedCurve(WATER_COLOUR, Lakes[i].Vertices, System.Drawing.Drawing2D.FillMode.Alternate, 0.95F);
                g.DrawString(Lakes[i].Name, DISPLAY_FONT, LOCATION_COLOUR, Lakes[i].Origin.X, Lakes[i].Origin.Y);
            }

            // Draw Oceans
            if (FULL_CONTINENT)
            {
                // Oceans at top and bottom of screen
                for (int i = 0; i < HorizontalOceans.Length; i++)
                {
                    xOffset = HorizontalOceans[i].Origin.X - HorizontalOceans[i].MajorRadius;
                    yOffset = HorizontalOceans[i].Origin.Y - HorizontalOceans[i].MinorRadius;
                    g.FillEllipse(WATER_COLOUR, xOffset, yOffset, 2 * HorizontalOceans[i].MajorRadius, 2 * HorizontalOceans[i].MinorRadius);
                }

                // Oceans at left and right of screen
                for (int i = 0; i < VerticalOceans.Length; i++)
                {
                    xOffset = VerticalOceans[i].Origin.X - VerticalOceans[i].MinorRadius;
                    yOffset = VerticalOceans[i].Origin.Y - VerticalOceans[i].MajorRadius;
                    g.FillEllipse(WATER_COLOUR, xOffset, yOffset, 2 * VerticalOceans[i].MinorRadius, 2 * VerticalOceans[i].MajorRadius);
                }
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
            for (int i = 0; i < Territories.Length; i++)
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
