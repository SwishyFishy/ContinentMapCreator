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
        static bool FULL_CONTINENT = false;
        static int MIN_NUM_TERRITORIES = 12;
        static int MAX_NUM_TERRITORIES = 18;
        static int NUM_TERRITORIES;
        static int MIN_TERRITORY_RADIUS = 150;
        static int MAX_TERRITORY_RADIUS = 250;
        static int TERRITORY_RADIUS;
        static int MIN_ORIGIN_SPACING = 5;
        static int MIN_NUM_LAKES = 3;
        static int MAX_NUM_LAKES = 7;
        static int NUM_LAKES;
        static int MIN_LAKE_RADIUS = 25;
        static int MAX_LAKE_RADIUS = 75;

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
        bool allowPainting = false;
        Territory[] Territories;
        Lake[] Lakes;
        Point[] PointsOnBorder = new Point[WINDOW_WIDTH * WINDOW_HEIGHT];
        Point[] TerritoryBorders;

        public form_Window()
        {
            InitializeComponent();
        }
    }
}
