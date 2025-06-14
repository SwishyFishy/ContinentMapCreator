﻿using System;
using System.Windows.Forms;
using System.Drawing;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    {
        // Window Presentation Values
        int WINDOW_WIDTH;
        int WINDOW_HEIGHT;

        const int BASE_WINDOW_WIDTH = 1600;
        const int BASE_WINDOW_HEIGHT = 900;
        const double SETTINGS_WIDTH_PROPORTION = 0.25;
        const double SETTINGS_HEIGHT_PROPORTION = 1.0;
        const double MAP_WIDTH_PROPORTION = 1.0 - SETTINGS_WIDTH_PROPORTION;
        const double MAP_HEIGHT_PROPORTION = 1.0;

        // Map Creation Settings
        bool FULL_CONTINENT;
        int MAP_WIDTH;
        int MAP_HEIGHT;

        int MIN_NUM_TERRITORIES;
        int MAX_NUM_TERRITORIES;
        int NUM_TERRITORIES;
        int MIN_TERRITORY_RADIUS;
        int MAX_TERRITORY_RADIUS;
        int TERRITORY_RADIUS;

        int MIN_NUM_LAKES;
        int MAX_NUM_LAKES;
        int NUM_LAKES;
        int MIN_LAKE_RADIUS;
        int MAX_LAKE_RADIUS;

        int MIN_NUM_RIVERS;
        int MAX_NUM_RIVERS;
        int NUM_RIVERS;
        int MIN_RIVER_CURVATURE;
        int MAX_RIVER_CURVATURE;
        int MIN_RIVER_THICKNESS;
        int MAX_RIVER_THICKNESS;

        const int MIN_ORIGIN_SPACING = 50;
        const int MIN_OCEAN_RADIUS_INLAND = 10;
        const int MAX_OCEAN_RADIUS_INLAND = MIN_ORIGIN_SPACING;
        const int MIN_OCEAN_RADIUS_COAST = 50;
        const int MAX_OCEAN_RADIUS_COAST = 100;
        const int OCEAN_SIZE_MULTIPLIER = 4;

        // Aesthetic Settings
        bool ROUGH_BORDERS = true;
        Font DISPLAY_FONT = new Font("Carlito", 12, FontStyle.Bold);
        SolidBrush LAND_COLOUR = new SolidBrush(Color.FromArgb(128, 128, 64));
        SolidBrush WATER_COLOUR = new SolidBrush(Color.FromArgb(0, 128, 192));
        SolidBrush LOCATION_COLOUR = new SolidBrush(Color.FromArgb(0, 0, 0));
        SolidBrush BORDER_COLOUR = new SolidBrush(Color.FromArgb(0, 64, 0));
        float BORDER_THICKNESS = 2.0F;
        float BORDER_OFFSET = 1.0F;
        float LOCATION_MARKER_THICKNESS = 2.0F;
        float LOCATION_MARKER_OFFSET = 1.0F;

        // Generation args
        readonly Random rnd = new Random();
        Territory[] Territories;
        Lake[] Lakes;
        Ocean[] HorizontalOceans;
        Ocean[] VerticalOceans;
        River[] Rivers;
        Point[] TerritoryBorders;
        bool[,] NeighbourMatrix;
        Point[] OriginPoints;
        int numOriginPoints;
        Point WorkingAreaOrigin;
        Point WorkingAreaFocus1;
        Point WorkingAreaFocus2;
        int workingAreaFociCoverage;

        // Display args
        FormWindowState oldWindowState;
        FormWindowState newWindowState;
        bool paintMap = false;
        bool paintSettings = true;
        bool mapExists = false;

        public form_Window()
        {
            InitializeComponent();
        }
    }
}
