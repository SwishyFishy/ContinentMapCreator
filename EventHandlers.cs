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
        // Resize       -> Resize and redraw panels
        private void form_Window_Load(object sender, EventArgs e)
        {
            // Set window size
            WINDOW_WIDTH = this.ClientSize.Width;
            WINDOW_HEIGHT = this.ClientSize.Height;
            this.Size = new Size(WINDOW_WIDTH, WINDOW_HEIGHT);
            this.Location = new Point(0, 0);

            // Set initial interior panel and map sizes
            pnl_SettingsBackground.Width = (int)(SETTINGS_WIDTH_PROPORTION * BASE_WINDOW_WIDTH);
            pnl_SettingsBackground.Height = (int)(SETTINGS_HEIGHT_PROPORTION * WINDOW_HEIGHT);
            pnl_SettingsBackground.Location = new Point(0, 0);

            MAP_WIDTH = (int)(MAP_WIDTH_PROPORTION * BASE_WINDOW_WIDTH);
            MAP_HEIGHT = (int)(MAP_HEIGHT_PROPORTION * BASE_WINDOW_HEIGHT);

            pnl_MapBackground.Width = MAP_WIDTH;
            pnl_MapBackground.Height = MAP_HEIGHT;
            pnl_MapBackground.Location = new Point(pnl_SettingsBackground.Width + (this.ClientSize.Width - pnl_SettingsBackground.Width - MAP_WIDTH) / 2, (this.ClientSize.Height - MAP_HEIGHT) / 2);

            // Add Controls
            pnl_SettingsBackground.Controls.Add(chb_FullContinent);
            pnl_SettingsBackground.Controls.Add(btn_TerritorySettings);
            pnl_SettingsBackground.Controls.Add(btn_LakeSettings);
            pnl_SettingsBackground.Controls.Add(btn_RiverSettings);

            pnl_TerritorySettings.Controls.Add(trk_TerritoryFrequencyBase);
            pnl_TerritorySettings.Controls.Add(trk_TerritoryFrequencyVariation);
            pnl_TerritorySettings.Controls.Add(trk_TerritoryRadiusBase);
            pnl_TerritorySettings.Controls.Add(trk_TerritoryRadiusVariation);
            pnl_SettingsBackground.Controls.Add(pnl_TerritorySettings);

            pnl_LakeSettings.Controls.Add(trk_LakeFrequencyBase);
            pnl_LakeSettings.Controls.Add(trk_LakeFrequencyVariation);
            pnl_LakeSettings.Controls.Add(trk_LakeRadiusBase);
            pnl_LakeSettings.Controls.Add(trk_LakeRadiusVariation);
            pnl_SettingsBackground.Controls.Add(pnl_LakeSettings);

            pnl_SettingsBackground.Controls.Add(trk_BorderThickness);
            pnl_SettingsBackground.Controls.Add(trk_LocationThickness);
            pnl_SettingsBackground.Controls.Add(cbo_LoadPreset);
            pnl_SettingsBackground.Controls.Add(chb_CleanBorders);
            pnl_SettingsBackground.Controls.Add(btn_FontSelector);
            pnl_SettingsBackground.Controls.Add(btn_Generate);

            // Add Tooltips
            tip_SettingsDetails.SetToolTip(chb_FullContinent, "Surround with Ocean");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryFrequency, "Number of Territories");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryFrequencyVariation, "+/- Number of Territories");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryRadius, "(Static) 1/2 maximum Territory diameter");
            tip_SettingsDetails.SetToolTip(lbl_TerritoryRadiusVariation, "+/- 1/2 maximum Territory diameter");

            tip_SettingsDetails.SetToolTip(lbl_LakeFrequency, "Number of Lakes");
            tip_SettingsDetails.SetToolTip(lbl_LakeFrequencyVariation, "+/- Number of Lakes");
            tip_SettingsDetails.SetToolTip(lbl_LakeRadius, "(Randomized twice per instance) 1/2 maximum Lake diameter");
            tip_SettingsDetails.SetToolTip(lbl_LakeRadiusVariation, "+/- 1/2 maximum Lake diameter");

            tip_SettingsDetails.SetToolTip(lbl_RiverFrequency, "Number of Rivers");
            tip_SettingsDetails.SetToolTip(lbl_RiverFrequencyVariation, "+/- Number of Rivers");
            tip_SettingsDetails.SetToolTip(lbl_RiverThickness, "(Randomized per instance) River Thickness");
            tip_SettingsDetails.SetToolTip(lbl_RiverThicknessVariation, "+/- River Thickness");
            tip_SettingsDetails.SetToolTip(lbl_RiverCurvature, "(Randomized per Instance) River Tension");
            tip_SettingsDetails.SetToolTip(lbl_RiverCurvatureVariation, "+/- River Tension");

            tip_SettingsDetails.SetToolTip(lbl_BorderThickness, "Border Thickness");
            tip_SettingsDetails.SetToolTip(lbl_LocationThickness, "Location Marker Thickness");
            tip_SettingsDetails.SetToolTip(chb_CleanBorders, "Draw borders cleanly or roughly");

            // Set control defaults
            UpdateDisplay();
            PresetDefaults();
            btn_Generate.Enabled = true;
        }
        private void form_Window_Resize(object sender, EventArgs e)
        {
            // Reset map panel
            // Do not adjust width or height. Adjust location so map is centered
            int availableX = this.ClientSize.Width - pnl_SettingsBackground.Width;
            int availableY = this.ClientSize.Height;
            pnl_MapBackground.Location = new Point(pnl_SettingsBackground.Width + (availableX - MAP_WIDTH) / 2, (availableY - MAP_HEIGHT) / 2);

            // Track changes in window state
            newWindowState = this.WindowState;
            if (newWindowState != oldWindowState)
            {
                oldWindowState = newWindowState;
                UpdateDisplay();
            }
        }
        private void form_Window_ResizeEnd(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        // Settings panels buttons
        // Click        -> Enable/Disable that settings panel
        private void btn_TerritorySettings_Click(object sender, EventArgs e)
        {
            // If territory settings are disabled, enable them and disable the rest
            if (!pnl_TerritorySettings.Enabled)
            {
                pnl_LakeSettings.Enabled = false;
                pnl_LakeSettings.Visible = false;

                pnl_TerritorySettings.Enabled = true;
                pnl_TerritorySettings.Visible = true;

                pnl_RiverSettings.Enabled = false;
                pnl_RiverSettings.Visible = false;
            }
            // Otherwise, disable them
            else
            {
                pnl_TerritorySettings.Enabled = false;
                pnl_TerritorySettings.Visible = false;
            }
        }
        private void btn_LakeSettings_Click(object sender, EventArgs e)
        {
            // If lake settings are disabled, enable them and disable the rest
            if (!pnl_LakeSettings.Enabled)
            {
                pnl_TerritorySettings.Enabled = false;
                pnl_TerritorySettings.Visible = false;

                pnl_LakeSettings.Enabled = true;
                pnl_LakeSettings.Visible = true;

                pnl_RiverSettings.Enabled = false;
                pnl_RiverSettings.Visible = false;
            }
            // Otherwise, disable them
            else
            {
                pnl_LakeSettings.Enabled = false;
                pnl_LakeSettings.Visible = false;
            }
        }
        private void btn_RiverSettings_Click(object sender, EventArgs e)
        {
            // If river settings are disabled, enable them and disable the rest
            if (!pnl_RiverSettings.Enabled)
            {
                pnl_LakeSettings.Enabled = false;
                pnl_LakeSettings.Visible = false;

                pnl_TerritorySettings.Enabled = false;
                pnl_TerritorySettings.Visible = false;

                pnl_RiverSettings.Enabled = true;
                pnl_RiverSettings.Visible = true;
            }
            // Otherwise, disable them
            else
            {
                pnl_RiverSettings.Enabled = false;
                pnl_RiverSettings.Visible = false;
            }
        }


        // Generation Settings track bars
        // Scroll       -> Update the displayed value
        private void trk_TerritoryFrequencyBase_Scroll(object sender, EventArgs e)
        {
            lbl_TerritoryFrequencyBaseDisplay.Text = trk_TerritoryFrequencyBase.Value.ToString();
        }
        private void trk_TerritoryFrequencyVariation_Scroll(object sender, EventArgs e)
        {
            lbl_TerritoryFrequencyVariationDisplay.Text = trk_TerritoryFrequencyVariation.Value.ToString();
        }
        private void trk_TerritoryRadiusBase_Scroll(object sender, EventArgs e)
        {
            lbl_TerritoryRadiusBaseDisplay.Text = trk_TerritoryRadiusBase.Value.ToString();
        }
        private void trk_TerritoryRadiusVariation_Scroll(object sender, EventArgs e)
        {
            lbl_TerritoryRadiusVariationDisplay.Text = trk_TerritoryRadiusVariation.Value.ToString();
        }
        private void trk_LakeFrequencyBase_Scroll(object sender, EventArgs e)
        {
            lbl_LakeFrequencyBaseDisplay.Text = trk_LakeFrequencyBase.Value.ToString();
        }
        private void trk_LakeFrequencyVariation_Scroll(object sender, EventArgs e)
        {
            lbl_LakeFrequencyVariationDisplay.Text = trk_LakeFrequencyVariation.Value.ToString();
        }
        private void trk_LakeRadiusBase_Scroll(object sender, EventArgs e)
        {
            lbl_LakeRadiusBaseDisplay.Text = trk_LakeRadiusBase.Value.ToString();
        }
        private void trk_LakeRadiusVariation_Scroll(object sender, EventArgs e)
        {
            lbl_LakeRadiusVariationDisplay.Text = trk_LakeRadiusVariation.Value.ToString();
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

        // cbo_LoadPresets
        // SelectedIndexChanged -> Load a map preset
        private void cbo_LoadPreset_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (cbo_LoadPreset.SelectedIndex)
            {
                case 0:
                    PresetDefaults();
                    break;
                case 1:
                    PresetArchipelago();
                    break;
                case 2:
                    PresetContinental();
                    break;
                case 3:
                    PresetCratered();
                    break;
                case 4:
                    PresetInlandSeas();
                    break;
                case 5:
                    PresetIslands();
                    break;
                case 6:
                    PresetProvinces();
                    break;
            }

            UpdateTrackbarDisplayLabels();
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
            // Hide new window tutorial labels
            lbl_TutorialSettingsPanel.Visible = false;
            lbl_TutorialSettingsHover.Visible = false;

            // Call the generation methods
            UpdateGenerationSettings();
            GenerateOrigins();
            GenerateTerritories(); 
            GenerateLakes();
            GenerateOceans();
            GenerateBorders();

            // Confirm that a map now exists for this runtime
            mapExists = true;

            // Redraw map
            UpdateDisplay();
        }

        // pnl_MapBackground
        // Paint        -> Call DrawMap() method to redraw screen when refreshed
        private void pnl_MapBackground_Paint(object sender, PaintEventArgs e)
        {
            if (paintMap)
            {
                DrawMap(e.Graphics);
            }
        }
    }
}
