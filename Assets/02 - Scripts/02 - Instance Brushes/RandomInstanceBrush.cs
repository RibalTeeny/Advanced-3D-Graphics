using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstanceBrush : InstanceBrush
{
    public int tree = 0;
    
    public override void draw(float x, float z)
    {
        terrain.object_prefab = terrain.vegetation[tree];
        float randomRadius = Random.Range(0f, radius);
        float randomAngle = Random.Range(0f, 360f);
        
        // Convert polar coordinates to Cartesian coordinates
        float randomPositionX = x + randomRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float randomPositionZ = z + randomRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
        
        spawnObject(randomPositionX, randomPositionZ);
    }
}
