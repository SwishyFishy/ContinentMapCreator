using System;
using System.Windows.Forms;
using System.Drawing;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    {
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

            // Add Controls
            pnl_SettingsBackground.Controls.Add(nud_TerritoryRadiusBound1);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryRadiusBound2);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryCountBound1);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryCountBound2);
            pnl_SettingsBackground.Controls.Add(nud_MinimumOriginSpacing);

            pnl_SettingsBackground.Controls.Add(chb_CleanBorders);
            pnl_SettingsBackground.Controls.Add(btn_FontSelector);
            pnl_SettingsBackground.Controls.Add(btn_Generate);

            // Add Tooltips
            tip_SettingsDetails.SetToolTip(lbl_TerritoryRadius, "Bounds 1/2 the maximum allowed distance across a territory.");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryCount, "Bounds the number of territories.");
            tip_SettingsDetails.SetToolTip(lbl_OriginSpacing, "Minimum distance between territory origin points. Higher values generate more uniform maps.");

            tip_SettingsDetails.SetToolTip(chb_CleanBorders, "Draw borders cleanly or roughly.");
            tip_SettingsDetails.SetToolTip(btn_FontSelector, "Select font for location names.");

            // Set control defaults
            UpdateDisplay();
            btn_Generate.Enabled = true;
        }

        // chb_CleanBorders
        // Check        -> Redraw
        private void chb_CleanBorders_CheckChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        // btn_OceanColourSelector
        // Click        -> Open clrd_ColourSelector and set OCEAN_COLOUR then redraw
        private void btn_OceanColourSelector_Click(object sender, EventArgs e)
        {
            clrd_ColourSelector.ShowDialog();
            OCEAN_COLOUR = new SolidBrush(clrd_ColourSelector.Color);
            UpdateDisplay();
        }

        // btn_LandColourSelector
        // Click        -> Open clrd_ColourSelector and set LAND_COLOUR then redraw
        private void btn_LandColourSelector_Click(object sender, EventArgs e)
        {
            clrd_ColourSelector.ShowDialog();
            LAND_COLOUR = new SolidBrush(clrd_ColourSelector.Color);
            UpdateDisplay();
        }

        // btn_BorderColourSelector
        // Click        -> Open clrd_ColourSelector and set BORDER_COLOUR then redraw
        private void btn_BorderColourSelector_Click(object sender, EventArgs e)
        {
            clrd_ColourSelector.ShowDialog();
            BORDER_COLOUR = new SolidBrush(clrd_ColourSelector.Color);
            UpdateDisplay();
        }

        // btn_FontSelector
        // Click        -> Open fntd_FontSelector and set DISPLAY_FONT then redraw
        private void btn_FontSelector_Click(object sender, EventArgs e)
        {
            fntd_FontSelector.ShowDialog();
            DISPLAY_FONT = fntd_FontSelector.Font;
            UpdateDisplay();
        }

        // btn_FontColourSelector
        // Click        -> Open clrd_ColourSelector and set LOCATION_COLOUR then redraw
        private void btn_FontColourSelector_Click(object sender, EventArgs e)
        {
            clrd_ColourSelector.ShowDialog();
            LOCATION_COLOUR = new SolidBrush(clrd_ColourSelector.Color);
            UpdateDisplay();
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
    }
}
