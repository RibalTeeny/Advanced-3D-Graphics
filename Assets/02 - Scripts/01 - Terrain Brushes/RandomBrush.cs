using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBrush : TerrainBrush {

    public float minHeight = 1.0f; // Minimum height deviation
    public float maxHeight = 10.0f; // Maximum height deviation
    public float maxRadiusDeviation = 5.0f; // Maximum radius deviation
    public bool addFlag = true;

    public override void draw(int x, int z) {
        int brushRadius = radius + Random.Range(-Mathf.RoundToInt(maxRadiusDeviation), Mathf.RoundToInt(maxRadiusDeviation));
        float randomHeight = Random.Range(minHeight, maxHeight);

        for (int zi = -brushRadius; zi <= brushRadius; zi++) {
            for (int xi = -brushRadius; xi <= brushRadius; xi++) {
                float currentHeight = terrain.get(x + xi, z + zi);
                
                // Apply a falloff based on distance from the center of the brush
                float distance = Mathf.Sqrt(xi * xi + zi * zi);
                if (distance <= brushRadius) {
                    float normalizedDistance = distance / brushRadius;
                    float heightChange = Mathf.Lerp(maxHeight, minHeight, normalizedDistance);
                    
                    // Switching between addition to the current height and overwriting
                    if (addFlag == true){
                        terrain.set(x + xi, z + zi, currentHeight + heightChange + randomHeight);
                    } else
                    {
                        terrain.set(x + xi, z + zi, heightChange + randomHeight);
                    }
                }
            }
        }
    }
}