using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgo : MonoBehaviour
{

    [Header("Genetic Algorithm parameters")]
    public int popSize = 100;
    public GameObject animalPrefab;

    [Header("Dynamic elements")]
    public float vegetationGrowthRate = 1.0f;
    public float currentGrowth;

    private List<GameObject> animals;
    protected Terrain terrain;
    protected CustomTerrain customTerrain;
    protected float width;
    protected float height;

    void Start()
    {
        // Retrieve terrain.
        terrain = Terrain.activeTerrain;
        customTerrain = GetComponent<CustomTerrain>();
        width = terrain.terrainData.size.x;
        height = terrain.terrainData.size.z;

        // Initialize terrain growth.
        currentGrowth = 0.0f;

        // Initialize animals array.
        animals = new List<GameObject>();
        for (int i = 0; i < popSize; i++)
        {
            GameObject animal = makeAnimal();
            animals.Add(animal);
        }
    }

    void Update()
    {
        // Keeps animal to a minimum.
        while (animals.Count < popSize / 10)
        {
            animals.Add(makeAnimal());
            
            /*foreach (GameObject animalObj in animals)
            {
                Animal animal = animalObj.GetComponent<Animal>();
                if (animal != null)
                {
                    animal.SetEnergyLoss(0.0f);
                }
            }*/
        }
        customTerrain.debug.text = "Number of animals: " + animals.Count.ToString();

        // Update grass elements/food resources.
        updateResources();
    }

    /// <summary>
    /// Method to place grass or other resource in the terrain.
    /// </summary>
    
    /*public void updateResources()
    {
        Vector2 detail_sz = customTerrain.detailSize();
        int[,] details = customTerrain.getDetails();
        currentGrowth += vegetationGrowthRate;
        while (currentGrowth > 1.0f)
        {
            int x = (int)(UnityEngine.Random.value * detail_sz.x);
            int y = (int)(UnityEngine.Random.value * detail_sz.y);
            details[y, x] = 1;
            currentGrowth -= 1.0f;
        }
        customTerrain.saveDetails();
    }*/
    
    // Grass placement based on the terrain altitude
    public void updateResources()
    {
        Vector2 detail_sz = customTerrain.detailSize();
        int[,] details = customTerrain.getDetails();
        float spawnProbability;
        currentGrowth += vegetationGrowthRate;
        while (currentGrowth > 1.0f)
        {
            float x = UnityEngine.Random.value * width;  // Use terrain width
            float y = UnityEngine.Random.value * height; // Use terrain height

            // Get the terrain altitude at the selected coordinates
            float terrainHeight = customTerrain.get(x, y);

            // Calculate the grass spawn probability based on terrain height
            if (terrainHeight == 0)
            {
                spawnProbability = 0.01f;
            }
            else
            {
                spawnProbability = CalculateSpawnProbability(terrainHeight);
            }

            float random = UnityEngine.Random.Range(0f,1f);
            // Check if grass should be spawned at this coordinate
            if (random < spawnProbability)
            {
                //customTerrain.debug.text = random.ToString();
                // Map world coordinates back to detail map coordinates
                int detailX = Mathf.FloorToInt(x / width * detail_sz.x);
                int detailY = Mathf.FloorToInt(y / height * detail_sz.y);

                details[detailY, detailX] = 1;
                currentGrowth -= 1.0f;

                // Visualize a debug ray at the spawned coordinate
                Vector3 start = new Vector3(x, terrainHeight, y);
                Vector3 end = start + Vector3.up * 10f; // Change the length here

                // Change the thickness (width) of the debug line by specifying the color
                Debug.DrawLine(start, end, Color.green, 10.0f); // Change the thickness here
            }
            else break;

            customTerrain.saveDetails();
        }
    }
    
    private float CalculateSpawnProbability(float terrainHeight)
    {
        // Adjust these parameters as needed
        float minProbability = 0.01f; // Minimum probability
        float maxProbability = 0.8f; // Maximum probability

        float normalizedHeight = terrainHeight / 110.0f;
        // Exponential function to increase the probability
        return minProbability + (maxProbability - minProbability) * Mathf.Pow(normalizedHeight, 2.0f);
    }


    /// <summary>
    /// Method to instantiate an animal prefab. It must contain the animal.cs class attached.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject makeAnimal(Vector3 position)
    {
        GameObject animal = Instantiate(animalPrefab, transform);
        animal.GetComponent<Animal>().Setup(customTerrain, this);
        animal.transform.position = position;
        animal.transform.Rotate(0.0f, UnityEngine.Random.value * 360.0f, 0.0f);
        return animal;
    }
    
    /*public void setLoss(float loss)
    {
        foreach (GameObject animalInstance in animals)
        {
            Animal animalComponent = animalInstance.GetComponent<Animal>();
            if (animalComponent != null)
            {
                animalComponent.SetEnergyLoss(loss);
                customTerrain.debug.text += "Setting the animal energy loss!";
            }
        }
    }*/

    /// <summary>
    /// If makeAnimal() is called without position, we randomize it on the terrain.
    /// </summary>
    /// <returns></returns>
    public GameObject makeAnimal()
    {
        Vector3 scale = terrain.terrainData.heightmapScale;
        float x = UnityEngine.Random.value * width;
        float z = UnityEngine.Random.value * height;
        float y = customTerrain.getInterp(x / scale.x, z / scale.z);
        return makeAnimal(new Vector3(x, y, z));
    }

    /// <summary>
    /// Method to add an animal inherited from anothed. It spawns where the parent was.
    /// </summary>
    /// <param name="parent"></param>
    public void addOffspring(Animal parent)
    {
        GameObject animal = makeAnimal(parent.transform.position);
        animal.GetComponent<Animal>().InheritBrain(parent.GetBrain(), true);
        animals.Add(animal);
    }

    /// <summary>
    /// Remove instance of an animal.
    /// </summary>
    /// <param name="animal"></param>
    public void removeAnimal(Animal animal)
    {
        animals.Remove(animal.transform.gameObject);
        Destroy(animal.transform.gameObject);
    }

}