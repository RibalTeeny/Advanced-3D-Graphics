using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothBrush : TerrainBrush {

    public float smoothStrength = 1.0f;
    public float flatnessThreshold = 0.9f;
    
    public override void draw(int centerX, int centerZ) {
        for (int zOffset = -radius; zOffset <= radius; zOffset++) {
            for (int xOffset = -radius; xOffset <= radius; xOffset++) {
                int x = centerX + xOffset;
                int z = centerZ + zOffset;
                
                Vector3 normal = terrain.getNormal(x, z);

                // Check if the terrain is relatively flat (you can adjust the threshold)
                if (normal.y > flatnessThreshold) {
                    continue;
                }

                // Get the heights of the current point and its neighbors
                float centerHeight = terrain.get(x, z);
                float leftHeight = terrain.get(x - 1, z);
                float rightHeight = terrain.get(x + 1, z);
                float frontHeight = terrain.get(x, z + 1);
                float backHeight = terrain.get(x, z - 1);

                // Calculate the average height of the neighbors
                float averageHeight = (leftHeight + rightHeight + frontHeight + backHeight) / 4.0f;

                // Smooth the height if there is a significant difference
                if (Mathf.Abs(centerHeight - averageHeight) > 0.1f) {
                    // Interpolation function
                    float smoothedHeight = Mathf.Lerp(centerHeight, averageHeight, smoothStrength);
                    terrain.set(x, z, smoothedHeight);
                }
            }
        }
    }
}