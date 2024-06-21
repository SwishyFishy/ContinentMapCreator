using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

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
        static bool FULL_CONTINENT;
        static int MIN_NUM_TERRITORIES;
        static int MAX_NUM_TERRITORIES;
        static int NUM_TERRITORIES;
        static int MIN_TERRITORY_RADIUS;
        static int MAX_TERRITORY_RADIUS;
        static int TERRITORY_RADIUS;
        static int MIN_NUM_LAKES;
        static int MAX_NUM_LAKES;
        static int NUM_LAKES;
        static int MIN_LAKE_RADIUS;
        static int MAX_LAKE_RADIUS;

        const int MIN_ORIGIN_SPACING = 50;

        // Aesthetic Settings
        static bool ROUGH_BORDERS = true;
        static Font DISPLAY_FONT = new Font("Carlito", 12, FontStyle.Bold);
        static SolidBrush LAND_COLOUR = new SolidBrush(Color.FromArgb(128, 128, 64));
        static SolidBrush WATER_COLOUR = new SolidBrush(Color.FromArgb(0, 128, 192));
        static SolidBrush LOCATION_COLOUR = new SolidBrush(Color.FromArgb(0, 0, 0));
        static SolidBrush BORDER_COLOUR = new SolidBrush(Color.FromArgb(0, 64, 0));
        static float BORDER_THICKNESS = 2.0F;
        static float BORDER_OFFSET = 1.0F;
        static float LOCATION_MARKER_THICKNESS = 2.0F;
        static float LOCATION_MARKER_OFFSET = 1.0F;

        // Generation args
        Random rnd = new Random();
        bool allowPainting = false;
        Territory[] Territories;
        Lake[] Lakes;
        Point[] TerritoryBorders;
        bool[,] NeighbourMatrix;
        Point[] OriginPoints;
        int numOriginPoints;
        int originIndex;

        // Diagnostics
        Stopwatch timer = new Stopwatch();

        public form_Window()
        {
            InitializeComponent();
        }
    }
}
