using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinBrush : TerrainBrush {

    public float scale = 0.05f;
    public float heightMultiplier = 25.0f;
    
    public override void draw(int x, int z) {
        for (int zi = -radius; zi <= radius; zi++) {
            for (int xi = -radius; xi <= radius; xi++) {
                //terrain.set(x + xi, z + zi, Mathf.PerlinNoise(xi,zi)*100);
                float perlinX = (x + xi) * scale;
                float perlinZ = (z + zi) * scale;
                float perlinValue = Mathf.PerlinNoise(perlinX, perlinZ) * heightMultiplier;
                float currentHeight = terrain.get(x + xi, z + zi);
                terrain.set(x + xi, z + zi, perlinValue);
                terrain.debug.text = "PerlinValue: " + perlinValue;
            }
        }
    }
}
