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
            pnl_SettingsBackground.Controls.Add(chb_FullContinent);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryCountBound1);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryCountBound2);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryRadiusBound1);
            pnl_SettingsBackground.Controls.Add(nud_TerritoryRadiusBound2);
            pnl_SettingsBackground.Controls.Add(nud_MinimumOriginSpacing);
            pnl_SettingsBackground.Controls.Add(nud_LakeCountBound1);
            pnl_SettingsBackground.Controls.Add(nud_LakeCountBound2);
            pnl_SettingsBackground.Controls.Add(nud_LakeRadiusBound1);
            pnl_SettingsBackground.Controls.Add(nud_LakeRadiusBound2);

            pnl_SettingsBackground.Controls.Add(trk_BorderThickness);
            pnl_SettingsBackground.Controls.Add(trk_LocationThickness);
            pnl_SettingsBackground.Controls.Add(chb_CleanBorders);
            pnl_SettingsBackground.Controls.Add(btn_FontSelector);
            pnl_SettingsBackground.Controls.Add(btn_Generate);

            // Add Tooltips
            tip_SettingsDetails.SetToolTip(chb_FullContinent, "Surround generated land with ocean.");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryCount, "Bounds the number of territories.");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryRadius, "Bounds 1/2 the maximum allowed distance across a territory.");
            tip_SettingsDetails.SetToolTip(lbl_OriginSpacing, "Minimum distance between territory origin points. Higher values generate more uniform maps.");
            tip_SettingsDetails.SetToolTip(lbl_LakeCount, "Bounds the number of lakes.");
            tip_SettingsDetails.SetToolTip(lbl_LakeRadius, "Bounds 1/2 the maximum allowed distance across a lake. Randomized per instance.");

            tip_SettingsDetails.SetToolTip(lbl_BorderThickness, "Number of pixels across each border.");
            tip_SettingsDetails.SetToolTip(lbl_LocationThickness, "Number of pixels across each location marker.");
            tip_SettingsDetails.SetToolTip(chb_CleanBorders, "Draw borders cleanly or roughly.");

            // Set control defaults
            UpdateDisplay();
            PresetDefaults();
            btn_Generate.Enabled = true;
        }


        // trk_BorderThickness
        // Scroll       -> Update the displayed value
        // MouseUp      -> Redraw
        private void trk_BorderThickness_Scroll(object sender, EventArgs e)
        {
            lbl_BorderThicknessDisplay.Text = (trk_BorderThickness.Value / 10.0F).ToString();
        }
        private void trk_BorderThickness_MouseUp(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        // trk_LocationThickness
        // Scroll       -> Update the displayed value
        // MouseUp      -> Redraw
        private void trk_LocationThickness_Scroll(object sender, EventArgs e)
        {
            lbl_LocationThicknessDisplay.Text = (trk_LocationThickness.Value / 10.0F).ToString();
        }
        private void trk_LocationThickness_MouseUp(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        // chb_CleanBorders
        // Check        -> Redraw
        private void chb_CleanBorders_CheckChanged(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        // btn_OceanColourSelector
        // Click        -> Open clrd_ColourSelector and set WATER_COLOUR then redraw
        private void btn_OceanColourSelector_Click(object sender, EventArgs e)
        {
            clrd_ColourSelector.ShowDialog();
            WATER_COLOUR = new SolidBrush(clrd_ColourSelector.Color);
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
            // Disable controls and hide new window tutorial labels
            pnl_SettingsBackground.Enabled = false;
            lbl_TutorialSettingsPanel.Visible = false;
            lbl_TutorialSettingsHover.Visible = false;

            // Call the generation methods
            UpdateGenerationSettings();
            GenerateTerritoryOrigins();
            GenerateWater();
            GenerateTerritoryBorders();

            // Redraw the screen
            allowPainting = true;
            Refresh();
            allowPainting = false;

            // Reenable controls
            pnl_SettingsBackground.Enabled = true;
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
