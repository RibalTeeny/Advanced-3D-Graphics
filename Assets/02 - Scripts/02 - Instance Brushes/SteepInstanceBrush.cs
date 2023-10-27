using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteepInstanceBrush : InstanceBrush {

    public float maxSteepnessAngle = 30.0f; // Set the maximum allowed steepness angle in degrees in the Inspector

    public override void draw(float x, float z)
    {
        // Check if the terrain is too steep at the placement location
        if (terrainIsTooSteep(x, z))
        {
            return; // Terrain is too steep, do not place the object
        }

        // If the terrain is not too steep, proceed with object placement
        spawnObject(x, z);
    }

    private bool terrainIsTooSteep(float x, float z)
    {
        Vector3 normal = terrain.getNormal(x, z);

        // Calculate the angle between the terrain normal and the up direction (in degrees)
        float steepnessAngle = Vector3.Angle(normal, Vector3.up);

        // Check if the steepness angle exceeds the maximum allowed angle
        if (steepnessAngle > maxSteepnessAngle)
        {
            return true; // Terrain is too steep
        }

        return false; // Terrain is not too steep
    }
}