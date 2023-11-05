using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeInstanceBrush : InstanceBrush
{
    public float minAltitude = 0.0f;
    public float maxAltitude = 20.0f;

    public override void draw(float x, float z)
    {
        float altitude = terrain.get(x, z);

        // Check if the terrain altitude is within the specified range
        if (altitude < minAltitude || altitude > maxAltitude)
        {
            return; 
        }
        
        spawnObject(x, z);
    }
}
