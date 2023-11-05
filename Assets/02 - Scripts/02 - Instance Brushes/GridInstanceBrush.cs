using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInstanceBrush : InstanceBrush
{
    public int gridSize = 50; // Adjust the grid size
    private float gridStep = 0;
    
    public float rotationAngle = 45.0f;

    public override void draw(float x, float z)
    {
        if (gridSize > 0)
        {
            gridStep = terrain.terrainSize().x / gridSize;
        }
    
        Vector2 rotatedCoords = RotateCoordinates(new Vector2(x, z), new Vector2(terrain.transform.position.x, terrain.transform.position.z), rotationAngle);

        float roundedX = Mathf.Round(rotatedCoords.x / gridStep) * gridStep;
        float roundedZ = Mathf.Round(rotatedCoords.y / gridStep) * gridStep;

        // Apply the reverse rotation transformation to the rounded coordinates
        Vector2 finalCoords = RotateCoordinates(new Vector2(roundedX, roundedZ), new Vector2(terrain.transform.position.x, terrain.transform.position.z), -rotationAngle);

        float distance = Vector2.Distance(new Vector2(x, z), finalCoords);
        if (distance <= radius)
        {
            if (!objectExistsAtLocation(finalCoords.x, finalCoords.y))
            {
                spawnObject(finalCoords.x, finalCoords.y);
            }
        }
    
    }

    private Vector2 RotateCoordinates(Vector2 point, Vector2 pivot, float angle)
    {
        float s = Mathf.Sin(angle * Mathf.Deg2Rad);
        float c = Mathf.Cos(angle * Mathf.Deg2Rad);

        // Translate the point to the origin
        point -= pivot;

        // Rotate the point
        float xNew = point.x * c - point.y * s;
        float yNew = point.x * s + point.y * c;

        // Translate the point back
        point.x = xNew + pivot.x;
        point.y = yNew + pivot.y;

        return point;
    }
    
    private bool objectExistsAtLocation(float x, float z)
    {
        int treeInstancesCount = terrain.getObjectCount();

        for (int i = 0; i < treeInstancesCount; i++)
        {
            Vector3 treePosition = terrain.getObjectLoc(i);
            float distance = Vector2.Distance(new Vector2(x, z), new Vector2(treePosition.x, treePosition.z));

            // Prevent overlap in a set distance
            if (distance < 1.0f)
            {
                return true; // A tree already exists here
            }
        }

        return false;
    }
}