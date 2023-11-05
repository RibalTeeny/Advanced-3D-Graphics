using UnityEngine;

public class RangeInstanceBrush : InstanceBrush
{
    private float[] altitudeRanges = { 2.0f, 20.0f, float.MaxValue }; // Define altitude ranges for each tree type

    public override void draw(float x, float z)
    {
        float altitude = terrain.get(x, z);

        // Find the appropriate tree type based on altitude
        int treeTypeIndex = FindTreeTypeIndex(altitude);

        // Check if a valid tree type was found
        if (treeTypeIndex >= 0 && treeTypeIndex < terrain.vegetation.Length)
        {
            // Set the prefab index and object_prefab to spawn the selected tree
            prefab_idx = treeTypeIndex;
            terrain.object_prefab = terrain.vegetation[prefab_idx];
            
            spawnObject(x, z);
        }
    }

    private int FindTreeTypeIndex(float altitude)
    {
        for (int i = 0; i < altitudeRanges.Length; i++)
        {
            if (altitude < altitudeRanges[i])
            {
                return i; // Return the index of the first altitude range that the altitude is below
            }
        }

        return terrain.vegetation.Length - 1; // Default to the last tree type if no range matches
    }
}
