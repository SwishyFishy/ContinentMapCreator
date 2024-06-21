using System;

namespace ContinentMapCreator
{
    public class Continent
    {
        // Allows specification of territories' geographical relation with neighbouring territories
        public enum NeighbourType { None, Self, Border }

        // Properties
        public Territory[] Territories { get; set; }
        public Lake[] Lakes { get; set; }
        public NeighbourType[,] NeighbourMatrix { get; set; }

        /*
        NeighbourMatrix has Territories.Length + Lakes.Length indices in both dimensions
        NeighbourMatrix[a, b] holds the geographical relation between Territories[a]/Lakes[a] and Territories[b]/Lakes[b]
            None: No connection
            Self: a = b
            Border: They share a border
        If the dimension is of length n, then the first Territory.Length indices of n correspond to the Territories array. 
        If a given index is greater than Territories.Length, subtract Territories.Length and use the result as an index of the Lakes array.

        A method GetAllNeighbours() can look at the Border relations of Lakes with that relation to determine 'lake-chain' connections
        */

        // Constructor
        public Continent(int territories, int lakes)
        {
            Territories = new Territory[territories];
            Lakes = new Lake[lakes];
            NeighbourMatrix = new NeighbourType[territories + lakes, territories + lakes];

            // Initialize neighbours
            for (int i = 0; i < NeighbourMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < NeighbourMatrix.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        NeighbourMatrix[i, j] = NeighbourType.Self;
                    }
                    else
                    {
                        NeighbourMatrix[i, j] = NeighbourType.None;
                    }
                }
            }
        }
    }
}
