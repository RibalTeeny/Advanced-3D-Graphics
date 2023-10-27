using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBrush : TerrainBrush {
    private float[,] heightData; // Store the height data within the brush radius
    public bool isCaptured = false;

    // Override the draw method to apply the stored height data
    public override void draw(int x, int z)
    {
        if (isCaptured)
        {
            int startX = x - radius;
            int startZ = z - radius;

            int size = radius * 2 + 1;

            for (int zi = 0; zi < size; zi++)
            {
                for (int xi = 0; xi < size; xi++)
                {
                    int xn = startX + xi;
                    int zn = startZ + zi;
                    terrain.set(xn, zn, heightData[xi, zi]); // Apply the stored height data
                }
            }
        }
        
        else
        {
            int startX = x - radius;
            int startZ = z - radius;

            int size = radius * 2 + 1;
            heightData = new float[size, size];

            for (int zi = 0; zi < size; zi++)
            {
                for (int xi = 0; xi < size; xi++)
                {
                    int xn = startX + xi;
                    int zn = startZ + zi;
                    heightData[xi, zi] = terrain.get(xn, zn); // Store the height data
                }
            }

            isCaptured = true;
        }
    }
}
