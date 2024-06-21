using System;

namespace ContinentMapCreator
{
    public class Continent
    {
        // Allows specification of territories' geographical relation with neighbouring territories
        public enum NeighbourType { Self, Border, Lake }

        // Properties
        public Territory[] Territories { get; set; }
        public Lake[] Lakes { get; set; }
        public NeighbourType[,] NeighbourMatrix { get; set; }


        // Constructor
        public Continent(int territories, int lakes)
        {
            Territories = new Territory[territories];
            Lakes = new Lake[lakes];

            NeighbourMatrix = new NeighbourType[territories, territories];
        }
    }
}
