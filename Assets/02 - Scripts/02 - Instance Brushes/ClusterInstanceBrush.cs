using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterInstanceBrush : InstanceBrush
{
    public int numberOfTrees = 3; // Adjust the number of trees
    private float lastDrawTime;
    private int latestIndex = prefab_idx;
    
    public override void draw(float x, float z)
    {
        if (Time.time - lastDrawTime >= 0.1f)
        {
            //terrain.debug.text = " ";
            Vector3 clusterCenter = new Vector3(x, 0, z);
            
            if (terrain.vegetation.Length > 0)
            {
                for (int i = 0; i < numberOfTrees; i++)
                {
                    int randomTreeIndex = Random.Range(0, terrain.vegetation.Length);
                    prefab_idx = randomTreeIndex;
                    terrain.object_prefab = terrain.vegetation[prefab_idx];
                    
                    float angle = Random.Range(0f, 360f);
                    float localRadius = Random.Range(0f, radius);

                    float xOffset = localRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float zOffset = localRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
                    
                    spawnObject(clusterCenter.x + xOffset, clusterCenter.z + zOffset);
                    
                    //terrain.debug.text += "\n" + "Tree Type: " + terrain.vegetation[randomTreeIndex].name + "\n";
                }
                prefab_idx = latestIndex;
            }
            else
            {
                terrain.object_prefab = null;
            }
            lastDrawTime = Time.time;
            terrain.object_prefab = terrain.vegetation[latestIndex];

        }
    }
}

