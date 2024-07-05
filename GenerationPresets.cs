using System.Windows.Forms;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    {
        // Default generation values
        private void PresetDefaults()
        {
            chb_FullContinent.Checked = false;
            trk_TerritoryFrequencyBase.Value = 15;
            trk_TerritoryFrequencyVariation.Value = 3;
            trk_TerritoryRadiusBase.Value = 200;
            trk_TerritoryRadiusVariation.Value = 50;
            trk_LakeFrequencyBase.Value = 15;
            trk_LakeFrequencyVariation.Value = 5;
            trk_LakeRadiusBase.Value = 50;
            trk_LakeRadiusVariation.Value = 25;
            trk_RiverFrequencyBase.Value = 2;
            trk_RiverFrequencyVariation.Value = 3;
            trk_RiverThicknessBase.Value = 100;
            trk_RiverThicknessVariation.Value = 25;
            trk_RiverCurvatureBase.Value = 50;
            trk_RiverCurvatureVariation.Value = 25;
            UpdateTrackbarDisplayLabels();
        }

        // Settings for archipelago-style maps
        private void PresetArchipelago()
        {
            chb_FullContinent.Checked = false;
            trk_TerritoryFrequencyBase.Value = 28;
            trk_TerritoryFrequencyVariation.Value = 4;
            trk_TerritoryRadiusBase.Value = 100;
            trk_TerritoryRadiusVariation.Value = 15;
            trk_LakeFrequencyBase.Value = 64;
            trk_LakeFrequencyVariation.Value = 12;
            trk_LakeRadiusBase.Value = 85;
            trk_LakeRadiusVariation.Value = 25;
            trk_RiverFrequencyBase.Value = 0;
            trk_RiverFrequencyVariation.Value = 0;
            trk_RiverThicknessBase.Value = 50;
            trk_RiverThicknessVariation.Value = 0;
            trk_RiverCurvatureBase.Value = 0;
            trk_RiverCurvatureVariation.Value = 0;
            UpdateTrackbarDisplayLabels();
        }

        // Settings for a continental-style map
        private void PresetContinental()
        {
            chb_FullContinent.Checked = true;
            trk_TerritoryFrequencyBase.Value = 26;
            trk_TerritoryFrequencyVariation.Value = 6;
            trk_TerritoryRadiusBase.Value = 250;
            trk_TerritoryRadiusVariation.Value = 75;
            trk_LakeFrequencyBase.Value = 6;
            trk_LakeFrequencyVariation.Value = 3;
            trk_LakeRadiusBase.Value = 225;
            trk_LakeRadiusVariation.Value = 75;
            trk_RiverFrequencyBase.Value = 0;
            trk_RiverFrequencyVariation.Value = 0;
            trk_RiverThicknessBase.Value = 50;
            trk_RiverThicknessVariation.Value = 0;
            trk_RiverCurvatureBase.Value = 0;
            trk_RiverCurvatureVariation.Value = 0;
            UpdateTrackbarDisplayLabels();
        }

        // Settings for a cratered map
        private void PresetCratered()
        {
            chb_FullContinent.Checked = false; 
            trk_TerritoryFrequencyBase.Value = 23;
            trk_TerritoryFrequencyVariation.Value = 3;
            trk_TerritoryRadiusBase.Value = 165;
            trk_TerritoryRadiusVariation.Value = 15;
            trk_LakeFrequencyBase.Value = 26;
            trk_LakeFrequencyVariation.Value = 6;
            trk_LakeRadiusBase.Value = 125;
            trk_LakeRadiusVariation.Value = 50;
            trk_RiverFrequencyBase.Value = 0;
            trk_RiverFrequencyVariation.Value = 0;
            trk_RiverThicknessBase.Value = 50;
            trk_RiverThicknessVariation.Value = 0;
            trk_RiverCurvatureBase.Value = 0;
            trk_RiverCurvatureVariation.Value = 0;
            UpdateTrackbarDisplayLabels();
        }

        // Settings for a continental-style map with large inland seas
        private void PresetInlandSeas()
        {
            chb_FullContinent.Checked = true;
            trk_TerritoryFrequencyBase.Value = 15;
            trk_TerritoryFrequencyVariation.Value = 2;
            trk_TerritoryRadiusBase.Value = 200;
            trk_TerritoryRadiusVariation.Value = 50;
            trk_LakeFrequencyBase.Value = 20;
            trk_LakeFrequencyVariation.Value = 2;
            trk_LakeRadiusBase.Value = 150;
            trk_LakeRadiusVariation.Value = 100;
            trk_RiverFrequencyBase.Value = 0;
            trk_RiverFrequencyVariation.Value = 0;
            trk_RiverThicknessBase.Value = 50;
            trk_RiverThicknessVariation.Value = 0;
            trk_RiverCurvatureBase.Value = 0;
            trk_RiverCurvatureVariation.Value = 0;
            UpdateTrackbarDisplayLabels();
        }

        // Settings for multi-island-style maps
        private void PresetIslands()
        {
            chb_FullContinent.Checked = true;
            trk_TerritoryFrequencyBase.Value = 23;
            trk_TerritoryFrequencyVariation.Value = 3;
            trk_TerritoryRadiusBase.Value = 90;
            trk_TerritoryRadiusVariation.Value = 15;
            trk_LakeFrequencyBase.Value = 32;
            trk_LakeFrequencyVariation.Value = 8;
            trk_LakeRadiusBase.Value = 70;
            trk_LakeRadiusVariation.Value = 10; 
            trk_RiverFrequencyBase.Value = 0;
            trk_RiverFrequencyVariation.Value = 0;
            trk_RiverThicknessBase.Value = 50;
            trk_RiverThicknessVariation.Value = 0;
            trk_RiverCurvatureBase.Value = 0;
            trk_RiverCurvatureVariation.Value = 0;
            UpdateTrackbarDisplayLabels();
        }

        // Settins for provincial-style maps
        private void PresetProvinces() 
        {
            chb_FullContinent.Checked = false;
            trk_TerritoryFrequencyBase.Value = 28;
            trk_TerritoryFrequencyVariation.Value = 4;
            trk_TerritoryRadiusBase.Value = 250;
            trk_TerritoryRadiusVariation.Value = 50;
            trk_LakeFrequencyBase.Value = 2;
            trk_LakeFrequencyVariation.Value = 2;
            trk_LakeRadiusBase.Value = 75;
            trk_LakeRadiusVariation.Value = 25;
            trk_RiverFrequencyBase.Value = 0;
            trk_RiverFrequencyVariation.Value = 0;
            trk_RiverThicknessBase.Value = 50;
            trk_RiverThicknessVariation.Value = 0;
            trk_RiverCurvatureBase.Value = 0;
            trk_RiverCurvatureVariation.Value = 0;
            UpdateTrackbarDisplayLabels();
        }
    }
}
