using System;
using System.Windows.Forms;
using System.Drawing;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    {
        private void Generate()
        {
            // Use Poisson-Disc sampling to determine a random set of territory origin points
            // https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf

            // Initialize variables
            // Dictates which pixels are too close to the border to be territories
            int minXValue = FULL_CONTINENT ? MIN_LAKE_RADIUS : 0;
            int maxXValue = FULL_CONTINENT ? pnl_MapBackground.Width - MIN_LAKE_RADIUS : 0;
            int minYValue = FULL_CONTINENT ? MIN_LAKE_RADIUS : 0;
            int maxYValue = FULL_CONTINENT ? pnl_MapBackground.Height - MIN_LAKE_RADIUS : 0;

            // Tracks the (x, y) coordinated of pixels that can be territory origins
            Point[] TerritoryOriginPoints = new Point[pnl_MapBackground.Width * pnl_MapBackground.Height];
            int numTerritoryOriginPoints = 0;

            // Tracks the indices of TerritoryOriginPoints that are exansionable
            int[] ActiveIndices = new int[TerritoryOriginPoints.Length];
            int numActiveIndices = 0;

            // Get minimum and maximum spacing of points
            int minSpacing = MIN_ORIGIN_SPACING;
            int maxSpacing = Math.Max(MIN_ORIGIN_SPACING, MIN_TERRITORY_RADIUS);

            // Get initial seed
            Random rnd = new Random();
            int x = rnd.Next(minXValue, maxXValue);
            int y = rnd.Next(minYValue, maxYValue);
            int GenerationAttempts = 30;

            TerritoryOriginPoints[numTerritoryOriginPoints] = new Point(x, y);
            ActiveIndices[numActiveIndices] = numTerritoryOriginPoints;
            numTerritoryOriginPoints = 1;
            numActiveIndices = 1;

            int nextSeed;
            int seed;
            bool placeable;

            // Apply the algorithm
            while (numActiveIndices > 0)
            {
                // Get origin point of next attempt
                nextSeed = rnd.Next(0, numActiveIndices);
                seed = ActiveIndices[nextSeed];

                for (int i = 0; i < GenerationAttempts; i++)
                {
                    // Hijack x and y to determine which quadrant of the grid around the next seed is being generated in this iteration of the loop
                    x = rnd.Next(0, 2);
                    y = rnd.Next(0, 2);

                    // Determine this iteration's point's x and y
                    x = x == 0 ? 
                        rnd.Next(TerritoryOriginPoints[seed].X - maxSpacing, TerritoryOriginPoints[seed].X - minSpacing) : 
                        rnd.Next(TerritoryOriginPoints[seed].X + minSpacing, TerritoryOriginPoints[seed].X + maxSpacing);
                    y = y == 0 ?
                        rnd.Next(TerritoryOriginPoints[seed].Y - maxSpacing, TerritoryOriginPoints[seed].Y - minSpacing) :
                        rnd.Next(TerritoryOriginPoints[seed].Y + minSpacing, TerritoryOriginPoints[seed].Y + maxSpacing);

                    placeable = true;

                    // If the new point is outside the acceptable bounds, the point is not placeable
                    if (x < minXValue || x > maxXValue || y < minYValue || y > maxYValue)
                    {
                        placeable = false;
                    }

                    // If the new point is too close to another, the point is not placeable
                    for (int j = 0; j < numTerritoryOriginPoints && placeable; j++)
                    {
                        if (Math.Sqrt(Math.Pow(TerritoryOriginPoints[j].X - x, 2) + Math.Pow(TerritoryOriginPoints[j].Y - y, 2)) < MIN_ORIGIN_SPACING)
                        {
                            placeable = false;
                        }
                    }

                    // Add the new point
                    if (placeable)
                    {
                        TerritoryOriginPoints[numTerritoryOriginPoints] = new Point(x, y);
                        ActiveIndices[numActiveIndices] = numTerritoryOriginPoints;
                        numTerritoryOriginPoints++;
                        numActiveIndices++;
                    }
                }

                // Remove the seed point from ActiveIndices
                numActiveIndices--;
                ActiveIndices[nextSeed] = ActiveIndices[numActiveIndices];
            }

            // End of Poisson-Disc algorithm
            // Result: TerritoryOriginPoints contains numTerritoryOriginPoints points, each of which is a valid point for a TerritoryOrigin

            // Generate Territories
            Territories = new Territory[Math.Min(NUM_TERRITORIES, numTerritoryOriginPoints)];
            int originIndex;
            for (int i = 0; i < Territories.Length; i++)
            {
                // Get a random TerritoryOriginPoint and make it a Territory
                originIndex = rnd.Next(0, numTerritoryOriginPoints);
                Territories[i] = new Territory(i.ToString(), TerritoryOriginPoints[originIndex], MIN_TERRITORY_RADIUS);

                // Overwrite the used TerritoryOriginPoint
                numTerritoryOriginPoints--;
                TerritoryOriginPoints[originIndex] = TerritoryOriginPoints[numTerritoryOriginPoints];
            }
        }
    }
}
