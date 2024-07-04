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
            int minXValue = 0;
            int maxXValue = MAP_WIDTH;
            int minYValue = 0;
            int maxYValue = MAP_HEIGHT;

            // If FULL_CONTINENT, define an elliptical area for territory origin points
            // Creates rounder continents, rather than the rectangular bounding of the border oceans
            WorkingAreaOrigin = new Point(MAP_WIDTH / 2, MAP_HEIGHT / 2);
            WorkingAreaFocus1 = new Point(0, 0);
            WorkingAreaFocus2 = new Point(0, 0);
            workingAreaFociCoverage = 0;
            if (FULL_CONTINENT)
            {
                // Squeeze the boundaries to account for border oceans
                minXValue += MAX_OCEAN_RADIUS_INLAND;
                maxXValue -= MAX_OCEAN_RADIUS_INLAND;
                minYValue += MAX_OCEAN_RADIUS_INLAND;
                maxYValue -= MAX_OCEAN_RADIUS_INLAND;

                // Define working area focal points
                // Assume panel is wider than it is high
                int focalLength = (int)Math.Sqrt(Math.Pow(WorkingAreaOrigin.X, 2) - Math.Pow(WorkingAreaOrigin.Y, 2));
                WorkingAreaFocus1 = new Point(WorkingAreaOrigin.X - focalLength, WorkingAreaOrigin.Y);
                WorkingAreaFocus2 = new Point(WorkingAreaOrigin.X + focalLength, WorkingAreaOrigin.Y);
                
                // Define length from focal points to edge of working area
                // 2 * major radius, which is (maxX - minX) / 2
                workingAreaFociCoverage = maxXValue - minXValue;
            }

            // Tracks the indices of OriginPoints that are exansionable
            OriginPoints = new Point[MAP_WIDTH * MAP_HEIGHT];
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

                        // If FULL_CONTINENT, check that the point is within the working area
                        if (FULL_CONTINENT && 
                            Math.Sqrt(Math.Pow(x - WorkingAreaFocus1.X, 2) + Math.Pow(y - WorkingAreaFocus1.Y, 2)) +
                            Math.Sqrt(Math.Pow(x - WorkingAreaFocus2.X, 2) + Math.Pow(y - WorkingAreaFocus2.Y, 2))
                            > workingAreaFociCoverage)
                        {
                            return;
                        }

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
                Territories[i] = new Territory(i.ToString(), LAND_COLOUR, OriginPoints[originIndex], TERRITORY_RADIUS);
                
                // Overwrite the used OriginPoint
                numOriginPoints--;
                OriginPoints[originIndex] = OriginPoints[numOriginPoints];
            }
        }
     
        // Generate Lakes
        // O(territories * origins)
        private void GenerateLakes()
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
                int leastDistance = MAP_WIDTH * MAP_HEIGHT;
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
                    Lakes[numLakes] = new Lake((Territories.Length + numLakes).ToString(), WATER_COLOUR, OriginPoints[originIndex], rad1, rad2, angle);
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

        // Generate Oceans
        // O(1)
        public void GenerateOceans()
        {
            // Generate bordering oceans
            if (FULL_CONTINENT)
            {
                HorizontalOceans = new Ocean[2 * (MAP_WIDTH / MIN_OCEAN_RADIUS_COAST) + 2];
                VerticalOceans = new Ocean[2 * (MAP_HEIGHT / MIN_OCEAN_RADIUS_COAST) + 2];
                int numOceans = 0;
                int xrad1;
                int yrad1;
                int xrad2;
                int yrad2;

                // Oceans on top and bottom of panel
                for (int i = 0; i < MAP_WIDTH; i += 2 * MIN_OCEAN_RADIUS_COAST)
                {
                    // Get ocean radii
                    xrad1 = rnd.Next(MIN_OCEAN_RADIUS_COAST, MAX_OCEAN_RADIUS_COAST);
                    yrad1 = rnd.Next(MIN_OCEAN_RADIUS_INLAND, MAX_OCEAN_RADIUS_INLAND);

                    xrad2 = rnd.Next(MIN_OCEAN_RADIUS_COAST, MAX_OCEAN_RADIUS_COAST);
                    yrad2 = rnd.Next(MIN_OCEAN_RADIUS_INLAND, MAX_OCEAN_RADIUS_INLAND);

                    // Apply OCEAN_SIZE_MULTIPLIER based on proximity to panel corner
                    // Weights larger oceans at corners and smaller oceans in the middle, resulting in a more rounded continent
                    // TODO: Technically, this is bad and unstable. There's no guarantee that no ocean crosses into the WorkingArea, and there's no failsafe
                    // for if a TerritoryOrigin exists inside the space in the WorkingArea the ocean occupies. The chances are low, but not impossible.
                    // This should be fixed so that this algorithm takes the size of the WorkingArea, or the TerritoryOrigins' locations, as an input.
                    double halfWidth = MAP_WIDTH / 2.0;
                    double distanceFromCenter = Math.Abs(halfWidth - i);
                    double normalizedDistanceFromCenter = distanceFromCenter / halfWidth;
                    xrad1 = Math.Max((int)(xrad1 * normalizedDistanceFromCenter * OCEAN_SIZE_MULTIPLIER), xrad1);
                    yrad1 = Math.Max((int)(yrad1 * normalizedDistanceFromCenter * OCEAN_SIZE_MULTIPLIER), yrad1);
                    xrad2 = Math.Max((int)(xrad2 * normalizedDistanceFromCenter * OCEAN_SIZE_MULTIPLIER), xrad2);
                    yrad2 = Math.Max((int)(yrad2 * normalizedDistanceFromCenter * OCEAN_SIZE_MULTIPLIER), yrad2);

                    // Add oceans
                    HorizontalOceans[numOceans] = new Ocean(numOceans.ToString(), WATER_COLOUR, new Point(i, 0), xrad1, yrad1);
                    numOceans++;
                    HorizontalOceans[numOceans] = new Ocean(numOceans.ToString(), WATER_COLOUR, new Point(i, MAP_HEIGHT), xrad2, yrad2);
                    numOceans++;
                }
                Array.Resize(ref HorizontalOceans, numOceans);

                // Oceans on left and right of panel
                numOceans = 0;
                for (int i = 0; i < MAP_HEIGHT; i += 2 * MIN_OCEAN_RADIUS_COAST)
                {
                    // Get ocean radii
                    xrad1 = rnd.Next(MIN_OCEAN_RADIUS_INLAND, MAX_OCEAN_RADIUS_INLAND);
                    yrad1 = rnd.Next(MIN_OCEAN_RADIUS_COAST, MAX_OCEAN_RADIUS_COAST);

                    xrad2 = rnd.Next(MIN_OCEAN_RADIUS_INLAND, MAX_OCEAN_RADIUS_INLAND);
                    yrad2 = rnd.Next(MIN_OCEAN_RADIUS_COAST, MAX_OCEAN_RADIUS_COAST);

                    // Apply OCEAN_SIZE_MULTIPLIER based on proximity to panel corner
                    // Weights larger oceans at corners and smaller oceans in the middle, resulting in a more rounded continent
                    // TODO: Technically, this is bad and unstable. There's no guarantee that no ocean crosses into the WorkingArea, and there's no failsafe
                    // for if a TerritoryOrigin exists inside the space in the WorkingArea the ocean occupies. The chances are low, but not impossible.
                    // This should be fixed so that this algorithm takes the size of the WorkingArea, or the TerritoryOrigins' locations, as an input.
                    double halfHeight = MAP_HEIGHT / 2.0;
                    double distanceFromCenter = Math.Abs(halfHeight - i);
                    double normalizedDistanceFromCenter = distanceFromCenter / halfHeight;
                    xrad1 = Math.Max((int)(xrad1 * normalizedDistanceFromCenter * OCEAN_SIZE_MULTIPLIER), xrad1);
                    yrad1 = Math.Max((int)(yrad1 * normalizedDistanceFromCenter * OCEAN_SIZE_MULTIPLIER), yrad1);
                    xrad2 = Math.Max((int)(xrad2 * normalizedDistanceFromCenter * OCEAN_SIZE_MULTIPLIER), xrad2);
                    yrad2 = Math.Max((int)(yrad2 * normalizedDistanceFromCenter * OCEAN_SIZE_MULTIPLIER), yrad2);

                    // Add oceans
                    VerticalOceans[numOceans] = new Ocean(numOceans.ToString(), WATER_COLOUR, new Point(0, i), xrad1, yrad1);
                    numOceans++;
                    VerticalOceans[numOceans] = new Ocean(numOceans.ToString(), WATER_COLOUR, new Point(MAP_WIDTH, i), xrad2, yrad2);
                    numOceans++;
                }
                Array.Resize(ref VerticalOceans, numOceans);
            }
        }

        // Generate Rivers
        // O(rivers * curvature)
        private void GenerateRivers()
        {
            // Get number of rivers
            NUM_RIVERS = rnd.Next(MIN_NUM_RIVERS, MAX_NUM_RIVERS + 1);
            int sinkIndexUpperBound = 2 * NUM_LAKES + (FULL_CONTINENT ? 2 * (VerticalOceans.Length + HorizontalOceans.Length) : 0);
            int sourceIndex;
            int sinkIndex;
            Point control1;
            Point control2;

            // Loop through all rivers
            for (int i = 0; i < NUM_RIVERS; i++)
            {
                // Random source and sink
                sourceIndex = rnd.Next(0, 2 * NUM_LAKES);
                sinkIndex = rnd.Next(0, sinkIndexUpperBound);

                // Control points, with allowed distance based on curvature

            }
        }

        // Calculate territory borders and mark coastal territories as such
        // Determine neighbouring Territories
        // O(length * width * (territories + lakes + oceans))
        private void GenerateBorders()
        {
            int numBorderPoints = 0;
            TerritoryBorders = new Point[MAP_WIDTH * MAP_HEIGHT];
            NeighbourMatrix = new bool[Territories.Length, Territories.Length];

            // Loop through all points
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    Point thisPixel = new Point(x, y);
                    EvaluatePixel();

                    void EvaluatePixel()
                    {
                        int bordersWaterIndex = -1;


                        // Check if pixel is inside a lake
                        for (int i = 0; i < Lakes.Length; i++)
                        {
                            // If this point is contained within a lake, move on
                            if (Lakes[i].BoundsContains(thisPixel))
                            {
                                return;
                            }
                            // If this point is on a lake border, remember that so the territory owning this point can add WaterNeighbours
                            else if (bordersWaterIndex < 0 && Lakes[i].BorderContains(thisPixel))
                            {
                                bordersWaterIndex = i;
                            }
                        }

                        // Check if the pixel is inside an ocean
                        if (FULL_CONTINENT && 
                            Math.Sqrt(Math.Pow(thisPixel.X - WorkingAreaFocus1.X, 2) + Math.Pow(thisPixel.Y - WorkingAreaFocus1.Y, 2)) +
                            Math.Sqrt(Math.Pow(thisPixel.X - WorkingAreaFocus2.X, 2) + Math.Pow(thisPixel.Y - WorkingAreaFocus2.Y, 2))
                            > workingAreaFociCoverage)
                        { 
                            // Check if this pixel is inside an ocean
                            for (int i = 0; i < HorizontalOceans.Length; i++)
                            {
                                // If this point is contained within an ocean, move on
                                if (HorizontalOceans[i].BoundsContains(thisPixel))
                                {
                                    return;
                                }
                                // If this point is on an ocean border, remember that
                                else if (bordersWaterIndex < 0 && HorizontalOceans[i].BorderContains(thisPixel))
                                {
                                    bordersWaterIndex = i;
                                }
                            }
                            for (int i = 0; i < VerticalOceans.Length; i++)
                            {
                                // If this point is contained within an ocean, move on
                                if (VerticalOceans[i].BoundsContains(thisPixel))
                                {
                                    return;
                                }
                                // If this point is on a lake border, remember that
                                else if (bordersWaterIndex < 0 && VerticalOceans[i].BorderContains(thisPixel))
                                {
                                    bordersWaterIndex = i;
                                }
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
                        // Point is on the border of a lake or ocean and is within the bounds of at least one Territory
                        else if (bordersWaterIndex > -1 && distancesToOrigins[closestOriginIndex] <= 1)
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
