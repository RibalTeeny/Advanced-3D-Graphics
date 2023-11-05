using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinBrush : TerrainBrush {

    public float scale = 0.05f;
    public float heightMultiplier = 60.0f;
    [Range(0.0f, 2.0f)] public float frequency = 0.2f;
    [Range(0.0f, 1.6f)] public float powValue = 1.0f;
    [Range(0.0f, 2.0f)] public float terraceValue = 0.5f;
    public int customRadius = 50;
    public bool terraceOn = true;
    
    public override void draw(int x, int z) {
        //for (int zi = -radius; zi <= radius; zi++) {
        //  for (int xi = -radius; xi <= radius; xi++) {
        float perlinValue;
        for (int zi = -customRadius; zi <= customRadius; zi++) {
            for (int xi = -customRadius; xi <= customRadius; xi++) {
                //terrain.set(x + xi, z + zi, Mathf.PerlinNoise(xi,zi)*100);
                float perlinX = (x + xi) * scale;
                float perlinZ = (z + zi) * scale;
                
                // Power function from a sum of three Perlin Noises generated with different parameters, the heightMultiplier is applied in the end
                if (!terraceOn)
                {
                    perlinValue = Mathf.Pow(((1 * Mathf.PerlinNoise(frequency * perlinX, frequency * perlinZ) +
                                                                + 0.5f * Mathf.PerlinNoise(frequency*2 * perlinX, frequency*2 * perlinZ)
                                                                + 0.25f * Mathf.PerlinNoise(frequency*4 * perlinX, frequency*4 * perlinZ))
                                                               * heightMultiplier) / (1.0f + 0.5f + 0.25f), powValue);
                }
                
                // Step function created using Mathf.Round
                else {
                    perlinValue = Mathf.Round(((1 * Mathf.PerlinNoise(frequency * perlinX, frequency * perlinZ) +
                                                            + 0.5f * Mathf.PerlinNoise(frequency*2 * perlinX, frequency*2 * perlinZ)
                                                            + 0.25f * Mathf.PerlinNoise(frequency*4 * perlinX, frequency*4 * perlinZ))
                                                           * heightMultiplier) / (1.0f + 0.5f + 0.25f) * terraceValue) / terraceValue;
                }
                
                //float currentHeight = terrain.get(x + xi, z + zi);
                terrain.set(x + xi, z + zi, perlinValue);
                //terrain.debug.text = "PerlinValue: " + perlinValue;
            }
        }
    }
}