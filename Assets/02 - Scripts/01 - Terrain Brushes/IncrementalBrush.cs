using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementalBrush : TerrainBrush {

    public float growthRate = 0.3f;
    public float brushRadius = 10.0f;
    public bool isIncrementing = true;

    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                float distance = Mathf.Sqrt(xi * xi + zi * zi);
                
                if (distance <= brushRadius) {
                    float currentHeight = terrain.get(x + xi, z + zi);
                    float change = isIncrementing ? growthRate : -growthRate;
                    terrain.set(x + xi, z + zi, currentHeight + change);
                }
            }
        }
    }
}
