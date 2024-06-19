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
            FULL_CONTINENT = chb_FullContinent.Checked ? true : false;

            // MIN_TERRITORY_RADIUS & MAX_TERRITORY_RADIUS based on nud_TerritoryRadiusBound1 & nud_TerritoryRadiusBound2
            MIN_TERRITORY_RADIUS = Math.Min((int)nud_TerritoryRadiusBound1.Value, (int)nud_TerritoryRadiusBound2.Value);
            MAX_TERRITORY_RADIUS = Math.Max((int)nud_TerritoryRadiusBound1.Value, (int)nud_TerritoryRadiusBound2.Value);

            // MIN_NUM_TERRITORIES & MAX_NUM_TERRITORIES based on nud_TerritoryCountBound1 & nud_TerritoryCountBound2
            MIN_NUM_TERRITORIES = Math.Min((int)nud_TerritoryCountBound1.Value, (int)nud_TerritoryCountBound2.Value);
            MAX_NUM_TERRITORIES = Math.Max((int)nud_TerritoryCountBound1.Value, (int)nud_TerritoryCountBound2.Value);

            // MIN_ORIGIN_SPACING based on nud_MinimumOriginSpacing
            MIN_ORIGIN_SPACING = (int)nud_MinimumOriginSpacing.Value;

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
            ROUGH_BORDERS = chb_CleanBorders.Checked ? false : true;

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

        // Populate TerritoryOrigins array with random points
        /*private void GenerateTerritoryOrigins()
        {
            // Pick random points for Territory origins
            Random rnd = new Random();
            NUM_TERRITORIES = rnd.Next(MIN_NUM_TERRITORIES, MAX_NUM_TERRITORIES + 1);
            TERRITORY_RADIUS = rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS);
            Territories = new Territory[NUM_TERRITORIES];

            // Set Territory origin boundaries
            int minX = FULL_CONTINENT ? TERRITORY_RADIUS : 0;
            int maxX = FULL_CONTINENT ? pnl_MapBackground.Width - TERRITORY_RADIUS : pnl_MapBackground.Width;
            int minY = FULL_CONTINENT ? TERRITORY_RADIUS : 0;
            int maxY = FULL_CONTINENT ? pnl_MapBackground.Height - TERRITORY_RADIUS : pnl_MapBackground.Height;
            Point origin = new Point();
            bool spacingVerified;

            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                spacingVerified = false;

                // Generate random (x, y) coordinates for this origin that are sufficiently distanced from all others
                while (!spacingVerified)
                {
                    spacingVerified = true;
                    origin = new Point(rnd.Next(minX, maxX), rnd.Next(minY, maxY));

                    // Check that origin is sufficiently distant from all others
                    for (int j = 0; j < i; j++)
                    {
                        if (Territories[j].OriginToPoint(origin) < MIN_ORIGIN_SPACING)
                        {
                            spacingVerified = false;
                        }
                    }
                }

                // Add a new Territory at the generated origin
                Territories[i] = new Territory(i.ToString(), origin, TERRITORY_RADIUS);
            }
        }*/

        // Populate Lakes array
        /*private void GenerateWater()
        {
            Random rnd = new Random();
            NUM_LAKES = rnd.Next(MIN_NUM_LAKES, MAX_NUM_LAKES);
            Lakes = new Lake[NUM_LAKES];

            // Inland lakes
            // Set Lake origin boundaries
            int minX = 0;
            int maxX = pnl_MapBackground.Width;
            int minY = 0;
            int maxY = pnl_MapBackground.Height;
            Point origin;
            int rad1;
            int rad2;
            double angle;
            bool spacingVerified;

            // Repeat once per lake
            for (int i = 0; i < NUM_LAKES; i++)
            {
                spacingVerified = false;

                // Generate random (x, y) coordinates for this origin that are sufficiently distanced from all others
                while (!spacingVerified)
                {
                    spacingVerified = true;

                    // Set the lake's origin, radii, and angle above the horizontal
                    origin = new Point(rnd.Next(minX, maxX), rnd.Next(minY, maxY));
                    rad1 = rnd.Next(MIN_LAKE_RADIUS, MAX_LAKE_RADIUS);
                    rad2 = rnd.Next(MIN_LAKE_RADIUS, MAX_LAKE_RADIUS);
                    angle = rnd.NextDouble();

                    // Add a new Lake at a random origin
                    Lakes[i] = new Lake((100 + i).ToString(), origin, rad1, rad2, angle);

                    // Check that lake does not contain any Territory origins
                    for (int j = 0; j < NUM_TERRITORIES; j++)
                    {
                        if (Lakes[i].LakeBoundsContains(Territories[j].Origin))
                        {
                            spacingVerified = false;
                        }
                    }
                }
            }
        }*/

        // Use Poisson-Disc sampling to determine a random set of territory origin points
        // https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf
        // O(n) for n possible origins
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

                    bool placeable = true;

                    // If the new point is outside the acceptable bounds, the point is not placeable
                    if (x < minXValue || x > maxXValue || y < minYValue || y > maxYValue)
                    {
                        placeable = false;
                    }

                    // If the new point is too close to another, the point is not placeable
                    for (int j = 0; j < numOriginPoints && placeable; j++)
                    {
                        if (Math.Sqrt(Math.Pow(OriginPoints[j].X - x, 2) + Math.Pow(OriginPoints[j].Y - y, 2)) < MIN_ORIGIN_SPACING)
                        {
                            placeable = false;
                        }
                    }

                    // Add the new point
                    if (placeable)
                    {
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
        // O(n) for n territories
        private void GenerateTerritories()
        {
            NUM_TERRITORIES = rnd.Next(MIN_NUM_TERRITORIES, MAX_NUM_TERRITORIES + 1);
            TERRITORY_RADIUS = rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS + 1);
            Territories = new Territory[Math.Min(NUM_TERRITORIES, numOriginPoints)];

            for (int i = 0; i < Territories.Length; i++)
            {
                // Get a random OriginPoint and make it a Territory
                originIndex = rnd.Next(0, numOriginPoints);
                Territories[i] = new Territory(i.ToString(), OriginPoints[originIndex], MIN_TERRITORY_RADIUS);

                // Overwrite the used OriginPoint
                numOriginPoints--;
                OriginPoints[originIndex] = OriginPoints[numOriginPoints];
            }
        }

        // Generate Lakes
        // O(n * m) for n inland lakes and m territories
        private void GenerateWater()
        {
            NUM_LAKES = rnd.Next(MIN_NUM_LAKES, MAX_NUM_LAKES + 1);
            Lakes = new Lake[NUM_LAKES];
            Oceans = new Lake[numOriginPoints];
            int numLakes = 0;
            int numOceans = 0;

            while (numOriginPoints > 0)
            {
                // Get a random OriginPoint
                originIndex = rnd.Next(0, numOriginPoints);

                // Check whether this point can be an inland lake, part of the ocean, or if it is too close to a TerritoryOrigin
                bool lake = false;
                bool ocean = false;
                int distance = 0;
                for (int j = 0; j < Territories.Length; j++)
                {
                    // Get distance from this point to the jth TerritoryOrigin 
                    distance = Territories[j].OriginToPoint(OriginPoints[originIndex]);

                    // If the distance between the TerritoryOrigin and this point is less than the minimum lake radius, the point is too close
                    if (distance < MIN_LAKE_RADIUS)
                    {
                        lake = false;
                        ocean = false;
                        break;
                    }
                    // If the distance between the TerritoryOrigin and this point is greater than the sum of the territory radius and maxkimum lke radius, it can be an ocean
                    else if (!lake && distance > MIN_TERRITORY_RADIUS + MAX_LAKE_RADIUS)
                    {
                        ocean = true;
                    }
                    // If the distance between the TerritoryOrigin and this point is enough for overlap, but not enough for the lake to contain the origin, it can be an inland lake
                    else
                    {
                        lake = true;
                        ocean = false;

                        // If there are enough inland lakes, and this point can't be an ocean, break
                        if (numLakes >= Lakes.Length)
                        {
                            lake = false;
                            break;
                        }
                    }
                }

                // Create an inland lake
                // Give the lake random radii and a random angle
                if (lake)
                {
                    int rad1 = rnd.Next(MIN_LAKE_RADIUS, MAX_LAKE_RADIUS);
                    int rad2 = rnd.Next(MIN_LAKE_RADIUS, MAX_LAKE_RADIUS);
                    double angle = rnd.NextDouble();
                    Lakes[numLakes] = new Lake(numLakes.ToString(), OriginPoints[originIndex], rad1, rad2, angle);
                    numLakes++;
                }
                // Create an ocean
                // Radius is the last measured distance between TerritoryOrigin and this point
                // Angle is irrelevant because the lake is circular
                else if (ocean)
                {
                    Oceans[numOceans] = new Lake((100 + numOceans).ToString(), OriginPoints[originIndex], distance, distance, 0.0);
                    numOceans++;
                }

                // Overwrite the used OriginPoint
                numOriginPoints--;
                OriginPoints[originIndex] = OriginPoints[numOriginPoints];
            }

            // Truncate the Oceans array
            Array.Resize(ref Oceans, numOceans);
        }

        // Calculate territory borders and mark coastal territories as such
        // For each point, add it to the OwnedPoints of its closest TerritoryOrigin
        private void GenerateBorders()
        {
            int numBorderPoints = 0;
            PointsOnBorder = new Point[WINDOW_WIDTH * WINDOW_HEIGHT];

            // Loop through all points
            for (int x = 0; x < pnl_MapBackground.Width; x++)
            {
                for (int y = 0; y < pnl_MapBackground.Height; y++)
                {
                    bool inLake = false;
                    int bordersLakeIndex = -1;
                    Point thisPixel = new Point(x, y);

                    for (int i = 0; i < NUM_LAKES; i++)
                    {
                        // If this point is contained within a lake, move on
                        if (Lakes[i].LakeBoundsContains(thisPixel))
                        {
                            inLake = true;
                        }
                        // If this point is on a lake border, remember that so the territory owning this point can add WaterNeighbours
                        else if (Lakes[i].LakeBorderContains(thisPixel))
                        {
                            bordersLakeIndex = i;
                        }
                    }
                    if (inLake)
                    {
                        continue;
                    }

                    // Get the distance between this point and each TerritoryOrigin
                    // As the loop progresses, track which indices in distancesToOrigins are those of the closest and second-closest territories
                    int[] distancesToOrigins = new int[NUM_TERRITORIES];
                    int closestOriginIndex = -1;
                    int secondClosestOriginIndex = -1;

                    // Populate the distancesToOrigins array
                    for (int i = 0; i < NUM_TERRITORIES; i++)
                    {
                        distancesToOrigins[i] = Territories[i].OriginToPoint(thisPixel);

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
                    if (distancesToOrigins[closestOriginIndex] > Territories[closestOriginIndex].MaxRadius)
                    {
                        continue;
                    }
                    // Point is on the border of a lake and is within the bounds of at least one Territory
                    else if (bordersLakeIndex > -1 && distancesToOrigins[closestOriginIndex] <= Territories[closestOriginIndex].MaxRadius)
                    {
                        PointsOnBorder[numBorderPoints] = thisPixel;
                        numBorderPoints++;
                        Lakes[bordersLakeIndex].AddNeighbour(Territories[closestOriginIndex]);
                    }
                    // Point is exactly a Territory's Radius from its Origin, and is closer to that Territory than any other.
                    else if (distancesToOrigins[closestOriginIndex] == Territories[closestOriginIndex].MaxRadius &&
                        distancesToOrigins[secondClosestOriginIndex] > Territories[secondClosestOriginIndex].MaxRadius)
                    {
                        PointsOnBorder[numBorderPoints] = thisPixel;
                        numBorderPoints++;
                    }
                    // Point is equidistant from two Territory Origins, and is within the bounds of both Territories, and is closer to those two Territories than any others
                    else if (distancesToOrigins[secondClosestOriginIndex] <= Territories[secondClosestOriginIndex].MaxRadius &&
                        distancesToOrigins[closestOriginIndex] == distancesToOrigins[secondClosestOriginIndex])
                    {
                        PointsOnBorder[numBorderPoints] = thisPixel;
                        numBorderPoints++;
                        Territories[closestOriginIndex].AddNeighbour(Territories[secondClosestOriginIndex], true);
                        Territories[secondClosestOriginIndex].AddNeighbour(Territories[closestOriginIndex], true);
                    }
                }
            }

            // Shorten PointsOnBorder to actual size
            TerritoryBorders = new Point[numBorderPoints];
            Array.Copy(PointsOnBorder, TerritoryBorders, numBorderPoints);
        }

        // Draw the screen
        private void Draw(PaintEventArgs e)
        {
            Pen borderPen = new Pen(BORDER_COLOUR, BORDER_THICKNESS);
            Pen locationPen = new Pen(LOCATION_COLOUR, LOCATION_MARKER_THICKNESS);
            float xOffset;
            float yOffset;

            // Draw Oceans
            //e.Graphics.FillRectangle(WATER_COLOUR, 0, 0, pnl_MapBackground.Width, pnl_MapBackground.Height);
            for (int i = 0; i < Oceans.Length; i++)
            {
                xOffset = Oceans[i].Origin.X - Oceans[i].MajorRadius;
                yOffset = Oceans[i].Origin.Y - Oceans[i].MajorRadius;

                e.Graphics.FillEllipse(WATER_COLOUR, xOffset, yOffset, 2 * Oceans[i].MajorRadius, 2 * Oceans[i].MajorRadius);
            }

            // Draw Territories
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                xOffset = Territories[i].Origin.X - Territories[i].MaxRadius;
                yOffset = Territories[i].Origin.Y - Territories[i].MaxRadius;
                e.Graphics.FillEllipse(LAND_COLOUR, xOffset, yOffset, 2 * Territories[i].MaxRadius, 2 * Territories[i].MaxRadius);
            }

            // Draw Lakes
            for (int i = 0; i < NUM_LAKES; i++)
            {
                e.Graphics.FillClosedCurve(WATER_COLOUR, Lakes[i].Vertices, System.Drawing.Drawing2D.FillMode.Alternate, 0.95F);
            }

            // Draw Borders
            for (int i = 0; i < TerritoryBorders.Length; i++)
            {
                Random rnd = new Random();
                xOffset = TerritoryBorders[i].X + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0) - BORDER_OFFSET;
                yOffset = TerritoryBorders[i].Y + (ROUGH_BORDERS ? rnd.Next(-3, 3) : 0) - BORDER_OFFSET;
                e.Graphics.DrawRectangle(borderPen, xOffset, yOffset, BORDER_THICKNESS, BORDER_THICKNESS);

            }

            // Draw TerritoryOrigins and Territory names
            for (int i = 0; i < NUM_TERRITORIES; i++)
            {
                xOffset = Territories[i].Origin.X - LOCATION_MARKER_OFFSET;
                yOffset = Territories[i].Origin.Y - LOCATION_MARKER_OFFSET;
                e.Graphics.DrawRectangle(locationPen, xOffset, yOffset, LOCATION_MARKER_THICKNESS, LOCATION_MARKER_THICKNESS);
                e.Graphics.DrawString(Territories[i].Name, DISPLAY_FONT, LOCATION_COLOUR, Territories[i].Origin.X, Territories[i].Origin.Y);
                for (int j = 1; j < Territories[i].WaterNeighbours.Length; j++)
                {
                    e.Graphics.DrawString(Territories[i].WaterNeighbours[j].Name, DISPLAY_FONT, LOCATION_COLOUR, Territories[i].Origin.X, Territories[i].Origin.Y + 15 * j);
                }
            }

            borderPen.Dispose();
            locationPen.Dispose();
        }
    }
}
