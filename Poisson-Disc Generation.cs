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
            int maxXValue = FULL_CONTINENT ? pnl_MapBackground.Width - MIN_LAKE_RADIUS : pnl_MapBackground.Width;
            int minYValue = FULL_CONTINENT ? MIN_LAKE_RADIUS : 0;
            int maxYValue = FULL_CONTINENT ? pnl_MapBackground.Height - MIN_LAKE_RADIUS : pnl_MapBackground.Height;

            // Tracks the (x, y) coordinated of pixels that can be territory origins
            // Tracks an array of tuples that store the index of, and distance to, each other origin
            Point[] OriginPoints = new Point[pnl_MapBackground.Width * pnl_MapBackground.Height];
            int numOriginPoints = 0;

            // Tracks the indices of OriginPoints that are exansionable
            int[] ActiveIndices = new int[OriginPoints.Length];
            int numActiveIndices = 0;

            // Get minimum and maximum spacing of points
            int minSpacing = MIN_ORIGIN_SPACING;
            int maxSpacing = Math.Max(MIN_ORIGIN_SPACING, MIN_TERRITORY_RADIUS);

            // Get initial seed
            Random rnd = new Random();
            int x = rnd.Next(minXValue, maxXValue);
            int y = rnd.Next(minYValue, maxYValue);
            int GenerationAttempts = 30;
            int originIndex;

            OriginPoints[numOriginPoints] = new Point(x, y);
            ActiveIndices[numActiveIndices] = numOriginPoints;
            numOriginPoints = 1;
            numActiveIndices = 1;

            // Apply the algorithm
            // O(n) for n possible origins
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

            // End of Poisson-Disc algorithm
            // Result: OriginPoints contains numOriginPoints points, each of which is a valid point for a TerritoryOrigin

            NUM_TERRITORIES = rnd.Next(MIN_NUM_TERRITORIES, MAX_NUM_TERRITORIES + 1);
            TERRITORY_RADIUS = rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS + 1);
            NUM_LAKES = rnd.Next(MIN_NUM_LAKES, MAX_NUM_LAKES + 1);

            // Generate Territories
            // O(n) for n territories
            Territories = new Territory[NUM_TERRITORIES];
            for (int i = 0; i < Territories.Length; i++)
            {
                // Get a random OriginPoint and make it a Territory
                originIndex = rnd.Next(0, numOriginPoints);
                Territories[i] = new Territory(i.ToString(), OriginPoints[originIndex], MIN_TERRITORY_RADIUS);

                // Overwrite the used OriginPoint
                numOriginPoints--;
                OriginPoints[originIndex] = OriginPoints[numOriginPoints];
            }

            // Generate Lakes
            // O(n * m) for n inland lakes and m territories
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
        }
    }
}