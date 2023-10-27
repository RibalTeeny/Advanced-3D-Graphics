using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceInstanceBrush : InstanceBrush {

    public float minimumDistance = 5.0f;
    public override void draw(float x, float z) {
        // Check if there's an object within the minimum distance
        if (!objectExistsWithinRadius(x, z, minimumDistance))
        {
            // If no object exists within the minimum distance, spawn the object
            spawnObject(x, z);
        }
    }

    private bool objectExistsWithinRadius(float x, float z, float radius)
    {
        int treeInstancesCount = terrain.getObjectCount();

        for (int i = 0; i < treeInstancesCount; i++)
        {
            Vector3 treePosition = terrain.getObjectLoc(i);
            float distance = Vector2.Distance(new Vector2(x, z), new Vector2(treePosition.x, treePosition.z));

            // Prevent overlap within the specified radius
            if (distance < radius)
            {
                return true; // An object already exists within the radius
            }
        }

        return false; // No object found within the radius
    }
}