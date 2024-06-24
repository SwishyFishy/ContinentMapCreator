using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

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
            pnl_SettingsBackground.Controls.Add(trk_TerritoryFrequencyBase);
            pnl_SettingsBackground.Controls.Add(trk_TerritoryFrequencyVariation);
            pnl_SettingsBackground.Controls.Add(trk_TerritoryRadiusBase);
            pnl_SettingsBackground.Controls.Add(trk_TerritoryRadiusVariation);
            pnl_SettingsBackground.Controls.Add(trk_LakeFrequencyBase);
            pnl_SettingsBackground.Controls.Add(trk_LakeFrequencyVariation);
            pnl_SettingsBackground.Controls.Add(trk_LakeRadiusBase);
            pnl_SettingsBackground.Controls.Add(trk_LakeRadiusVariation);

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

            tip_SettingsDetails.SetToolTip(lbl_BorderThickness, "Border pixel thickness");
            tip_SettingsDetails.SetToolTip(lbl_LocationThickness, "Location Marker pixel thickness");
            tip_SettingsDetails.SetToolTip(chb_CleanBorders, "Draw borders cleanly or roughly");

            // Set control defaults
            UpdateDisplay();
            PresetDefaults();
            btn_Generate.Enabled = true;
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
            // Disable controls and hide new window tutorial labels
            pnl_SettingsBackground.Enabled = false;
            lbl_TutorialSettingsPanel.Visible = false;
            lbl_TutorialSettingsHover.Visible = false;

            // Call the generation methods
            UpdateGenerationSettings();
            GenerateOrigins();
            GenerateTerritories(); 
            GenerateWater(); 
            GenerateBorders(); 

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
                DrawMap(e.Graphics);
            }
        }
    }
}
