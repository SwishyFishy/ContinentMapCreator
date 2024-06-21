using System;
using System.Windows.Forms;
using System.Drawing;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    {
        // Methods
        // Update the map generation settings based on user input
        private void UpdateGenerationSettings()
        {
            // FULL_CONTINENT based on chb_FullContinent
            FULL_CONTINENT = chb_FullContinent.Checked;

            // MIN_TERRITORY_RADIUS & MAX_TERRITORY_RADIUS based on nud_TerritoryRadiusBound1 & nud_TerritoryRadiusBound2
            MIN_TERRITORY_RADIUS = Math.Min((int)nud_TerritoryRadiusBound1.Value, (int)nud_TerritoryRadiusBound2.Value);
            MAX_TERRITORY_RADIUS = Math.Max((int)nud_TerritoryRadiusBound1.Value, (int)nud_TerritoryRadiusBound2.Value);

            // MIN_NUM_TERRITORIES & MAX_NUM_TERRITORIES based on nud_TerritoryCountBound1 & nud_TerritoryCountBound2
            MIN_NUM_TERRITORIES = Math.Min((int)nud_TerritoryCountBound1.Value, (int)nud_TerritoryCountBound2.Value);
            MAX_NUM_TERRITORIES = Math.Max((int)nud_TerritoryCountBound1.Value, (int)nud_TerritoryCountBound2.Value);

            // MIN_NUM_LAKES & MAX_NUM_LAKES based on nud_LakeCountBound1 & nud_LakeCountBound2
            MIN_NUM_LAKES = Math.Min((int)nud_LakeCountBound1.Value, (int)nud_LakeCountBound2.Value);
            MAX_NUM_LAKES = Math.Max((int)nud_LakeCountBound1.Value, (int)nud_LakeCountBound2.Value);

            // MIN_LAKE_RADIUS & MAX_LAKE_RADIUS based on nud_LakeRadiusBound1 & nud_LakeRadiusBound2
            MIN_LAKE_RADIUS = Math.Min((int)nud_LakeRadiusBound1.Value, (int)nud_LakeRadiusBound2.Value);
            MAX_LAKE_RADIUS = Math.Max((int)nud_LakeRadiusBound1.Value, (int)nud_LakeRadiusBound2.Value);
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

        // Use Poisson-Disc sampling to determine a random set of territory origin points
        // https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf
        // O(origins)
        private void GenerateOrigins()
        {
            // Initialize variables
            // Dictates which pixels are too close to the border to be territories
            int minXValue = FULL_CONTINENT ? MIN_LAKE_RADIUS : 0;
            int maxXValue = FULL_CONTINENT ? pnl_MapBackground.Width - MIN_LAKE_RADIUS : pnl_MapBackground.Width;
            int minYValue = FULL_CONTINENT ? MIN_LAKE_RADIUS : 0;
            int maxYValue = FULL_CONTINENT ? pnl_MapBackground.Height - MIN_LAKE_RADIUS : pnl_MapBackground.Height;

            // Tracks the indices of OriginPoints that are exansionable
            OriginPoints = new Point[pnl_MapBackground.Width * pnl_MapBackground.Height];
            numOriginPoints = 0;
            int[] ActiveIndices = new int[OriginPoints.Length];
            int numActiveIndices = 0;

            // Get minimum and maximum spacing of points
            int minSpacing = MIN_ORIGIN_SPACING;
            int maxSpacing = Math.Max(MIN_ORIGIN_SPACING, MIN_TERRITORY_RADIUS);

            // Get initial seed
            int x = rnd.Next(minXValue, maxXValue);
            int y = rnd.Next(minYValue, maxYValue);
            int GenerationAttempts = 30;

            OriginPoints[numOriginPoints] = new Point(x, y);
            ActiveIndices[numActiveIndices] = numOriginPoints;
            numOriginPoints = 1;
            numActiveIndices = 1;

            // Apply the algorithm
            while (numActiveIndices > 0)
            {
                // Get origin point of next attempt
                int nextSeed = rnd.Next(0, numActiveIndices);
                originIndex = ActiveIndices[nextSeed];

                for (int i = 0; i < GenerationAttempts; i++)
                {
                    Place();

                    void Place()
                    { 
                        // Hijack x and y to determine which quadrant of the grid around the next originIndex is being generated in this iteration of the loop
                        x = rnd.Next(0, 2);
                        y = rnd.Next(0, 2);

                        // Determine this iteration's point's x and y
                        x = x == 0 ?
                            rnd.Next(OriginPoints[originIndex].X - maxSpacing, OriginPoints[originIndex].X - minSpacing) :
                            rnd.Next(OriginPoints[originIndex].X + minSpacing, OriginPoints[originIndex].X + maxSpacing);
                        y = y == 0 ?
                            rnd.Next(OriginPoints[originIndex].Y - maxSpacing, OriginPoints[originIndex].Y - minSpacing) :
                            rnd.Next(OriginPoints[originIndex].Y + minSpacing, OriginPoints[originIndex].Y + maxSpacing);

                        // If the new point is outside the acceptable bounds, the point is not placeable
                        if (x < minXValue || x > maxXValue || y < minYValue || y > maxYValue)
                        {
                            return;
                        }

                        // If the new point is too close to another, the point is not placeable
                        for (int j = 0; j < numOriginPoints; j++)
                        {
                            if (Math.Sqrt(Math.Pow(OriginPoints[j].X - x, 2) + Math.Pow(OriginPoints[j].Y - y, 2)) < MIN_ORIGIN_SPACING)
                            {
                                return;
                            }
                        }

                        // Add the new point
                        OriginPoints[numOriginPoints] = new Point(x, y);
                        ActiveIndices[numActiveIndices] = numOriginPoints;
                        numOriginPoints++;
                        numActiveIndices++;
                    }
                }

                // Remove the seed point from ActiveIndices
                numActiveIndices--;
                ActiveIndices[nextSeed] = ActiveIndices[numActiveIndices];
            }
        }

        // Generate Territories
        // O(territories * territories)
        private void GenerateTerritories()
        {
            NUM_TERRITORIES = rnd.Next(MIN_NUM_TERRITORIES, MAX_NUM_TERRITORIES + 1);
            TERRITORY_RADIUS = rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS + 1);
            Territories = new Territory[Math.Min(NUM_TERRITORIES, numOriginPoints)];

            for (int i = 0; i < Territories.Length; i++)
            {
                // Get a random OriginPoint and make it a Territory
                originIndex = rnd.Next(0, numOriginPoints);
                Territories[i] = new Territory(i.ToString(), OriginPoints[originIndex], TERRITORY_RADIUS);
                
                // Overwrite the used OriginPoint
                numOriginPoints--;
                OriginPoints[originIndex] = OriginPoints[numOriginPoints];
            }
        }
     
        // Generate Lakes
        // O(territories * lakes)
        private void GenerateWater()
        {
            NUM_LAKES = rnd.Next(MIN_NUM_LAKES, MAX_NUM_LAKES + 1);
            Lakes = new Lake[Math.Min(NUM_LAKES, numOriginPoints)];
            int numLakes = 0;

            while (numOriginPoints > 0 && numLakes < Lakes.Length)
            {
                // Get a random OriginPoint
                originIndex = rnd.Next(0, numOriginPoints);

                // Check whether this point can be an inland lake, part of the ocean, or if it is too close to a TerritoryOrigin
                int leastDistance = pnl_MapBackground.Width * pnl_MapBackground.Height;
                for (int i = 0; i < Territories.Length; i++)
                {
                    leastDistance = Math.Min(leastDistance, Territories[i].OriginToPoint(OriginPoints[originIndex]));
                }

                // Create an inland lake
                // Give the lake random radii and a random angle
                if (leastDistance > MIN_LAKE_RADIUS && leastDistance < MAX_LAKE_RADIUS)
                {
                    int rad1 = rnd.Next(MIN_LAKE_RADIUS, Math.Min(MAX_LAKE_RADIUS, leastDistance));
                    int rad2 = rnd.Next(MIN_LAKE_RADIUS, Math.Min(MAX_LAKE_RADIUS, leastDistance));
                    double angle = rnd.NextDouble();
                    Lakes[numLakes] = new Lake((Territories.Length + numLakes).ToString(), OriginPoints[originIndex], rad1, rad2, angle);
                    numLakes++;
                }

                // Overwrite the used OriginPoint
                numOriginPoints--;
                OriginPoints[originIndex] = OriginPoints[numOriginPoints];
            }

            // Truncate the array
            Array.Resize(ref Lakes, numLakes);
            NUM_LAKES = numLakes;
        }

        // Calculate territory borders and mark coastal territories as such
        // Determine neighbouring Territories (by land and sea)
        // O(length * width * (territories + lakes))
        private void GenerateBorders()
        {
            int numBorderPoints = 0;
            TerritoryBorders = new Point[pnl_MapBackground.Width * pnl_MapBackground.Height]; 
            NeighbourMatrix = new bool[Territories.Length + Lakes.Length, Territories.Length + Lakes.Length];

            // Loop through all points
            for (int x = 0; x < pnl_MapBackground.Width; x++)
            {
                for (int y = 0; y < pnl_MapBackground.Height; y++)
                {
                    Point thisPixel = new Point(x, y);
                    EvaluatePixel();

                    void EvaluatePixel()
                    {
                        // Check if pixel is inside a lake
                        int bordersLakeIndex = -1;
                        for (int i = 0; i < Lakes.Length; i++)
                        {
                            // If this point is contained within a lake, move on
                            if (Lakes[i].LakeBoundsContains(thisPixel))
                            {
                                return;
                            }
                            // If this point is on a lake border, remember that so the territory owning this point can add WaterNeighbours
                            else if (bordersLakeIndex < 0 && Lakes[i].LakeBorderContains(thisPixel))
                            {
                                bordersLakeIndex = i;
                            }
                        }

                        // Get the distance between this point and each TerritoryOrigin
                        // As the loop progresses, track which indices in distancesToOrigins are those of the closest and second-closest territories
                        double[] distancesToOrigins = new double[Territories.Length];
                        int closestOriginIndex = -1;
                        int secondClosestOriginIndex = -1;

                        // Populate the distancesToOrigins array
                        for (int i = 0; i < Territories.Length; i++)
                        {
                            // Scale the distance to origin such that origin = 0, and border = 1.
                            distancesToOrigins[i] = (double)Territories[i].OriginToPoint(thisPixel) / (double)Territories[i].Radius;

                            // Track two closest territories
                            if (closestOriginIndex == -1)
                            {
                                closestOriginIndex = i;
                            }
                            else if (distancesToOrigins[closestOriginIndex] > distancesToOrigins[i])
                            {
                                secondClosestOriginIndex = closestOriginIndex;
                                closestOriginIndex = i;
                            }
                            else if (secondClosestOriginIndex == -1 || distancesToOrigins[secondClosestOriginIndex] > distancesToOrigins[i])
                            {
                                secondClosestOriginIndex = i;
                            }
                        }

                        // Point is too far from any Territory Origin to be land
                        if (distancesToOrigins[closestOriginIndex] > 1)
                        {
                            return;
                        }
                        // Point is on the border of a lake and is within the bounds of at least one Territory
                        else if (bordersLakeIndex > -1 && distancesToOrigins[closestOriginIndex] <= 1)
                        {
                            TerritoryBorders[numBorderPoints] = thisPixel;
                            numBorderPoints++;

                            if (!NeighbourMatrix[closestOriginIndex, Territories.Length + bordersLakeIndex])
                            {
                                NeighbourMatrix[closestOriginIndex, Territories.Length + bordersLakeIndex] = true;
                                NeighbourMatrix[Territories.Length + bordersLakeIndex, closestOriginIndex] = true;
                            }
                        }
                        // Point is exactly a Territory's Radius from its Origin, and is closer to that Territory than any other.
                        else if (distancesToOrigins[closestOriginIndex] == 1 && distancesToOrigins[secondClosestOriginIndex] > 1)
                        {
                            TerritoryBorders[numBorderPoints] = thisPixel;
                            numBorderPoints++;
                            Territories[closestOriginIndex].Coastal = true;
                        }
                        // Point is equidistant from two Territory Origins, and is within the bounds of both Territories, and is closer to those two Territories than any others
                        else if (distancesToOrigins[secondClosestOriginIndex] <= 1 &&
                            distancesToOrigins[closestOriginIndex] == distancesToOrigins[secondClosestOriginIndex])
                        {
                            TerritoryBorders[numBorderPoints] = thisPixel;
                            numBorderPoints++;

                            NeighbourMatrix[closestOriginIndex, secondClosestOriginIndex] = true;
                            NeighbourMatrix[secondClosestOriginIndex, closestOriginIndex] = true;
                        }
                    }
                }
            }

            // Shorten TerritoryBorders to actual size
            Array.Resize(ref TerritoryBorders, numBorderPoints);
        }

        // Draw the screen
        private void DrawMap(Graphics g)
        {
            Pen borderPen = new Pen(BORDER_COLOUR, BORDER_THICKNESS);
            Pen locationPen = new Pen(LOCATION_COLOUR, LOCATION_MARKER_THICKNESS);
            float xOffset;
            float yOffset;

            // Draw Ocean
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
