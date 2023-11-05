using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBrush : TerrainBrush {

    public float maxIntensity = 5.0f; // Maximum intensity of the Gaussian distribution
    public float standardDeviation = 5.0f; // Standard deviation of the Gaussian distribution
    public bool isIncrementing = true;

    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float distance = Vector2.Distance(new Vector2(xi, zi), Vector2.zero);
                float intensity = maxIntensity * GaussianDistribution(distance, standardDeviation);
               
                float change = isIncrementing ? intensity : -intensity; // Reverse intensity for decrement action
                
                float currentHeight = terrain.get(x + xi, z + zi);
                terrain.set(x + xi, z + zi, currentHeight + change);
            }
        }
    }
    
    private float GaussianDistribution(float x, float stdDev) {
        return Mathf.Exp(-(x * x) / (2 * stdDev * stdDev)) / (stdDev * Mathf.Sqrt(2 * Mathf.PI)); // Gaussian
    }
}
