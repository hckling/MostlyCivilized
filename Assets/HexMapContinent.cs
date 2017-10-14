using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class HexMapContinent : HexMap
    {
        public override void GenerateMap()
        {
            base.GenerateMap();

            int numContinents = 2;
            int continentSpacing = 20;

            for (int c = 0; c < numContinents; c++)
            {
                int numberOfSplats = Random.Range(3, 6);

                for (int i = 0; i < numberOfSplats; i++)
                {
                    int range = Random.Range(3, 8);
                    int row = Random.Range(range, NumberOfRows - range);
                    int col = Random.Range(20, NumberOfColumns - 20) + (c * continentSpacing);

                    ElevateArea(col, row, range);
                }
            }            

            // Add lumpiness - Perlin noise?
            // Set mesh of tiles to correct type
            // Set materials to correct type based on elevation

            UpdateHexVisuals();
        }

        private void ElevateArea(int col, int row, int range, float centerHeight = 0.5f)
        {
            Hex centerHex = GetHexAt(col, row);

            List<Hex> areaHexes = GetHexesWithinRangeOf(centerHex, range);

            foreach(Hex h in areaHexes)
            {
                if (h.Elevation < 0)
                    h.Elevation = 0;

                h.Elevation += centerHeight * Mathf.Lerp(1f, 0.25f, Hex.Distance(h, centerHex) / range);
            }
        }
    }
}
