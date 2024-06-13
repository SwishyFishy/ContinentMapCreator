using System.Windows.Forms;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    {
        // Default generation values
        private void PresetDefaults()
        {
            chb_FullContinent.Checked = false;
            nud_TerritoryCountBound1.Value = 12;
            nud_TerritoryCountBound2.Value = 18;
            nud_TerritoryRadiusBound1.Value = 150;
            nud_TerritoryRadiusBound2.Value = 250;
            nud_MinimumOriginSpacing.Value = 50;
            nud_LakeCountBound1.Value = 10;
            nud_LakeCountBound2.Value = 20;
            nud_LakeRadiusBound1.Value = 25;
            nud_LakeRadiusBound2.Value = 75;
        }

        // Settings for a continental-scale map
        private void PresetContinental()
        {
            chb_FullContinent.Checked = true;
            nud_TerritoryCountBound1.Value = 20;
            nud_TerritoryCountBound2.Value = 25;
            nud_TerritoryRadiusBound1.Value = 125;
            nud_TerritoryRadiusBound2.Value = 150;
            nud_MinimumOriginSpacing.Value = 25;
            nud_LakeCountBound1.Value = 20;
            nud_LakeCountBound2.Value = 32;
            nud_LakeRadiusBound1.Value = 75;
            nud_LakeRadiusBound2.Value = 175;
        }

        // Settings for archipelago-style maps
        private void PresetArchipelago() 
        {
            chb_FullContinent.Checked = true;
            nud_TerritoryCountBound1.Value = 20;
            nud_TerritoryCountBound2.Value = 25;
            nud_TerritoryRadiusBound1.Value = 50;
            nud_TerritoryRadiusBound2.Value = 75;
            nud_MinimumOriginSpacing.Value = 100;
            nud_LakeCountBound1.Value = 32;
            nud_LakeCountBound2.Value = 32;
            nud_LakeRadiusBound1.Value = 50;
            nud_LakeRadiusBound2.Value = 75;
        }

        // Settins for provincial-style maps
        private void PresetProvinces() 
        {
            chb_FullContinent.Checked = false;
            nud_TerritoryCountBound1.Value = 25;
            nud_TerritoryCountBound2.Value = 32;
            nud_TerritoryRadiusBound1.Value = 200;
            nud_TerritoryRadiusBound2.Value = 300;
            nud_MinimumOriginSpacing.Value = 25;
            nud_LakeCountBound1.Value = 0;
            nud_LakeCountBound2.Value = 5;
            nud_LakeRadiusBound1.Value = 25;
            nud_LakeRadiusBound2.Value = 100;
        }
    }
}
