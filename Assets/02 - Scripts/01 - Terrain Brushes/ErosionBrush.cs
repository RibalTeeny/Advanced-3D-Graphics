using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErosionBrush : TerrainBrush
{
    public int numCraters = 1; // Number of craters to create
    public float minCraterRadius = 1f; // Minimum radius of a crater
    public float maxCraterRadius = 5f; // Maximum radius of a crater
    public float minDepth = 0.5f; // Minimum depth of a crater
    public float maxDepth = 1f; // Maximum depth of a crater

    public override void draw(int x, int z)
    {
        for (int i = 0; i < numCraters; i++)
        {
            // Generate random crater parameters
            float craterX = Random.Range(x - radius, x + radius);
            float craterZ = Random.Range(z - radius, z + radius);
            float craterRadius = Random.Range(minCraterRadius, maxCraterRadius);
            float craterDepth = Random.Range(minDepth, maxDepth);

            // Apply the crater effect to the terrain
            createCrater(craterX, craterZ, craterRadius, craterDepth);
        }

        terrain.save();
    }

    private void createCrater(float x, float z, float craterRadius, float depth)
    {
        for (int zi = (int)(z - craterRadius); zi <= (int)(z + craterRadius); zi++)
        {
            for (int xi = (int)(x - craterRadius); xi <= (int)(x + craterRadius); xi++)
            {
                float distance = Mathf.Sqrt((xi - x) * (xi - x) + (zi - z) * (zi - z));
                if (distance <= craterRadius)
                {
                    float normalizedDepth = 1f - Mathf.Clamp01(distance / craterRadius); // Depth decreases with distance from the center
                    float currentHeight = terrain.get(xi, zi);
                    float newHeight = currentHeight - depth * normalizedDepth;

                    terrain.set(xi, zi, newHeight);
                }
            }
        }
    }
}