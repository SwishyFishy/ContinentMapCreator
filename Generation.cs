using System;
using System.Windows.Forms;
using System.Drawing;

namespace ContinentMapCreator
{
    public partial class form_Window : Form
    {
        // Update the map generation settings based on user input
        private void UpdateGenerationSettings()
        {
            int temp;

            // FULL_CONTINENT
            FULL_CONTINENT = chb_FullContinent.Checked;

            // MIN_NUM_TERRITORIES & MAX_NUM_TERRITORIES
            temp = trk_TerritoryFrequencyBase.Value - trk_TerritoryFrequencyVariation.Value;
            MIN_NUM_TERRITORIES = temp >= trk_TerritoryFrequencyBase.Minimum ? temp : trk_TerritoryFrequencyBase.Minimum;
            MAX_NUM_TERRITORIES = Math.Min(trk_TerritoryFrequencyBase.Value + trk_TerritoryFrequencyVariation.Value, trk_TerritoryFrequencyBase.Maximum);

            // MIN_TERRITORY_RADIUS & MAX_TERRITORY_RADIUS
            temp = trk_TerritoryRadiusBase.Value - trk_TerritoryRadiusVariation.Value;
            MIN_TERRITORY_RADIUS = temp >= trk_TerritoryRadiusBase.Minimum ? temp : trk_TerritoryRadiusBase.Minimum;
            MAX_TERRITORY_RADIUS = Math.Min(trk_TerritoryRadiusBase.Value + trk_TerritoryRadiusVariation.Value, trk_TerritoryRadiusBase.Maximum);

            // MIN_NUM_LAKES & MAX_NUM_LAKES
            temp = trk_LakeFrequencyBase.Value - trk_LakeFrequencyVariation.Value;
            MIN_NUM_LAKES = temp >= trk_LakeFrequencyBase.Minimum ? temp : trk_LakeFrequencyBase.Minimum;
            MAX_NUM_LAKES = Math.Min(trk_LakeFrequencyBase.Value + trk_LakeFrequencyVariation.Value, trk_LakeFrequencyBase.Maximum);

            // MIN_LAKE_RADIUS & MAX_LAKE_RADIUS
            temp = trk_LakeRadiusBase.Value - trk_LakeRadiusVariation.Value;
            MIN_LAKE_RADIUS = temp >= trk_LakeRadiusBase.Minimum ? temp : trk_LakeRadiusBase.Minimum;
            MAX_LAKE_RADIUS = Math.Min(trk_LakeRadiusBase.Value + trk_LakeRadiusVariation.Value, trk_LakeRadiusBase.Maximum);
        }

        // Use Poisson-Disc sampling to determine a random set of territory origin points
        // https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf
        // O(origins)
        private void GenerateOrigins()
        {
            // Initialize variables
            // Dictates which pixels are too close to the border to be territories
            int minXValue = FULL_CONTINENT ? MAX_OCEAN_RADIUS_INLAND : 0;
            int maxXValue = FULL_CONTINENT ? pnl_MapBackground.Width - MAX_OCEAN_RADIUS_INLAND : pnl_MapBackground.Width;
            int minYValue = FULL_CONTINENT ? MAX_OCEAN_RADIUS_INLAND : 0;
            int maxYValue = FULL_CONTINENT ? pnl_MapBackground.Height - MAX_OCEAN_RADIUS_INLAND : pnl_MapBackground.Height;

            // Tracks the indices of OriginPoints that are exansionable
            OriginPoints = new Point[pnl_MapBackground.Width * pnl_MapBackground.Height];
            numOriginPoints = 0;
            int[] ActiveIndices = new int[OriginPoints.Length];
            int numActiveIndices = 0;
            int originIndex;

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
        // O(territories)
        private void GenerateTerritories()
        {
            NUM_TERRITORIES = rnd.Next(MIN_NUM_TERRITORIES, MAX_NUM_TERRITORIES + 1);
            TERRITORY_RADIUS = rnd.Next(MIN_TERRITORY_RADIUS, MAX_TERRITORY_RADIUS + 1);
            Territories = new Territory[Math.Min(NUM_TERRITORIES, numOriginPoints)];
            int originIndex;

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
        // O(territories * origins)
        private void GenerateWater()
        {
            NUM_LAKES = rnd.Next(MIN_NUM_LAKES, MAX_NUM_LAKES + 1);
            Lakes = new Lake[Math.Min(NUM_LAKES, numOriginPoints)];
            int numLakes = 0;
            int originIndex;

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

            // Generate bordering oceans
            if (FULL_CONTINENT)
            {
                Oceans = new Ocean[2 * (pnl_MapBackground.Width + pnl_MapBackground.Height) / MIN_OCEAN_RADIUS_COAST];

                // Oceans on top and bottom of panel
                for (int i = 0; i < pnl_MapBackground.Width; i += MIN_OCEAN_RADIUS_COAST)
                {

                }
            }
        }

        // Calculate territory borders and mark coastal territories as such
        // Determine neighbouring Territories
        // O(length * width * (territories + lakes))
        private void GenerateBorders()
        {
            int numBorderPoints = 0;
            TerritoryBorders = new Point[pnl_MapBackground.Width * pnl_MapBackground.Height];
            NeighbourMatrix = new bool[Territories.Length, Territories.Length];

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
                            if (Lakes[i].BoundsContains(thisPixel))
                            {
                                return;
                            }
                            // If this point is on a lake border, remember that so the territory owning this point can add WaterNeighbours
                            else if (bordersLakeIndex < 0 && Lakes[i].BorderContains(thisPixel))
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
                        }
                        // Point is exactly a Territory's Radius from its Origin, and is closer to that Territory than any other.
                        else if (distancesToOrigins[closestOriginIndex] == 1 && (secondClosestOriginIndex == -1 || distancesToOrigins[secondClosestOriginIndex] > 1))
                        {
                            TerritoryBorders[numBorderPoints] = thisPixel;
                            numBorderPoints++;
                            Territories[closestOriginIndex].Coastal = true;
                        }
                        // Point is equidistant from two Territory Origins, and is within the bounds of both Territories, and is closer to those two Territories than any others
                        else if (secondClosestOriginIndex > -1 && distancesToOrigins[secondClosestOriginIndex] <= 1 &&
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
    }
}
