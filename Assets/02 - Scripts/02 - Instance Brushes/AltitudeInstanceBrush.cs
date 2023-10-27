using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltitudeInstanceBrush : InstanceBrush
{
    public float minAltitude = 0.0f; // Set the minimum allowed altitude in the Inspector
    public float maxAltitude = 20.0f; // Set the maximum allowed altitude in the Inspector

    public override void draw(float x, float z)
    {
        // Get the altitude of the terrain at the placement location
        float altitude = terrain.get(x, z);

        // Check if the terrain altitude is within the specified range
        if (altitude < minAltitude || altitude > maxAltitude)
        {
            return; // Terrain altitude is outside the allowed range, do not place the object
        }

        // If the terrain altitude is within the allowed range, proceed with object placement
        spawnObject(x, z);
    }
}
