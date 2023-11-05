using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{

    [Header("Animal parameters")]
    public float swapRate = 0.01f;
    public float mutateRate = 0.01f;
    public float swapStrength = 10.0f;
    public float mutateStrength = 0.5f;
    public float maxAngle = 10.0f;

    [Header("Energy parameters")]
    public float maxEnergy = 50.0f;
    public float lossEnergy = 0.1f;
    public float gainEnergy = 10.0f;
    private float energy;

    [Header("Sensor - Vision")]
    public float maxVision = 20.0f;
    public float stepAngle = 10.0f;
    public int nEyes = 5;
    
    /*[Header("Smell parameters")]
    public float smellRadius = 5.0f; // Adjust the radius as needed
    //public int numSmellSectors = 8; // You can adjust the number of sectors
    private bool smellData; // Array to store smell data for each sector*/

    private int[] networkStruct;
    private SimpleNeuralNet brain = null;

    // Terrain.
    private CustomTerrain terrain = null;
    private int[,] details = null;
    private Vector2 detailSize;
    private Vector2 terrainSize;

    // Animal.
    private Transform tfm;
    private float[] vision;

    // Genetic alg.
    private GeneticAlgo genetic_algo = null;

    // Renderer.
    private Material mat = null;

    void Start()
    {
        // Network: 1 input per receptor, 1 output per actuator.
        vision = new float[nEyes];
        networkStruct = new int[] { nEyes, 5, 1 };
        energy = maxEnergy;
        tfm = transform;

        // Renderer used to update animal color.
        // It needs to be updated for more complex models.
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
            mat = renderer.material;
    }

    void Update()
    {
        // In case something is not initialized...
        if (brain == null)
            brain = new SimpleNeuralNet(networkStruct);
        if (terrain == null)
            return;
        if (details == null)
        {
            UpdateSetup();
            return;
        }

        // Retrieve animal location in the heighmap
        int dx = (int)((tfm.position.x / terrainSize.x) * detailSize.x);
        int dy = (int)((tfm.position.z / terrainSize.y) * detailSize.y);

        // For each frame, we lose lossEnergy
        energy -= lossEnergy;

        // If the animal is located in the dimensions of the terrain and over a grass position (details[dy, dx] > 0), it eats it, gain energy and spawn an offspring.
        if ((dx >= 0) && dx < (details.GetLength(1)) && (dy >= 0) && (dy < details.GetLength(0)) && details[dy, dx] > 0)
        {
            // Eat (remove) the grass and gain energy.
            details[dy, dx] = 0;
            energy += gainEnergy;
            if (energy > maxEnergy)
                energy = maxEnergy;

            genetic_algo.addOffspring(this);
        }

        // If the energy is below 0, the animal dies.
        if (energy < 0)
        {
            energy = 0.0f;
            genetic_algo.removeAnimal(this);
        }

        // Update the color of the animal as a function of the energy that it contains.
        if (mat != null)
            mat.color = Color.white * (energy / maxEnergy);

        // 1. Update receptor.
        UpdateVision();
        //UpdateSmell();

        // 2. Use brain.
        float[] output = brain.getOutput(vision);
        //float[] output = brain.getOutput(vision, smellData); // Pass smell data to the brain.


        // 3. Act using actuators.
        float angle = (output[0] * 2.0f - 1.0f) * maxAngle;
        tfm.Rotate(0.0f, angle, 0.0f);
    }

    /// <summary>
    /// Calculate distance to the nearest food resource, if there is any.
    /// </summary>
    private void UpdateVision()
    {
        float startingAngle = -((float)nEyes / 2.0f) * stepAngle;
        Vector2 ratio = detailSize / terrainSize;

        for (int i = 0; i < nEyes; i++)
        {
            Quaternion rotAnimal = tfm.rotation * Quaternion.Euler(0.0f, startingAngle + (stepAngle * i), 0.0f);
            Vector3 forwardAnimal = rotAnimal * Vector3.forward;
            float sx = tfm.position.x * ratio.x;
            float sy = tfm.position.z * ratio.y;
            vision[i] = 1.0f;
            
            Vector3 rayStart = tfm.position;
            Vector3 rayDirection = forwardAnimal.normalized * maxVision;

            Debug.DrawRay(rayStart, rayDirection, Color.red);

            // Interate over vision length.
            for (float distance = 1.0f; distance < maxVision; distance += 0.5f)
            {
                // Position where we are looking at.
                float px = (sx + (distance * forwardAnimal.x * ratio.x));
                float py = (sy + (distance * forwardAnimal.z * ratio.y));

                if (px < 0)
                    px += detailSize.x;
                else if (px >= detailSize.x)
                    px -= detailSize.x;
                if (py < 0)
                    py += detailSize.y;
                else if (py >= detailSize.y)
                    py -= detailSize.y;

                if ((int)px >= 0 && (int)px < details.GetLength(1) && (int)py >= 0 && (int)py < details.GetLength(0) && details[(int)py, (int)px] > 0)
                {
                    vision[i] = distance / maxVision;
                    break;
                }
            }
        }
    }
    
    /*private void UpdateSmell()
    {
        smellData = false;

        // Iterate through the entire environment represented by the "details" array.
        for (int row = 0; row < details.GetLength(0); row++)
        {
            for (int col = 0; col < details.GetLength(1); col++)
            {
                // Calculate the position within the environment.
                float px = col * terrainSize.x / details.GetLength(1);
                float py = row * terrainSize.y / details.GetLength(0);

                // Check if the position represents a food source and is within the detection radius.
                if (details[row, col] > 0 &&
                    Vector3.Distance(tfm.position, new Vector3(px, 0, py)) <= smellRadius)
                {
                    // Set a boolean value to true or perform any other desired action.
                    smellData = true;
                    break;  // Exit the inner loop if a food source is detected.
                }
            }

            if (smellData)
            {
                break;  // Exit the outer loop if a food source is detected.
            }
        }
        //UpdateDetectionRadius();
    }*/
    
    /*private void UpdateDetectionRadius()
    {
        // Get the LineRenderer component from your GameObject or child object
        LineRenderer lineRenderer = GetComponent<LineRenderer>(); // Adjust this if the LineRenderer is on a child object

        // Set the number of points for the circle (for smooth appearance)
        int numPoints = 50; // Adjust as needed for the level of detail

        // Clear any previous positions
        lineRenderer.positionCount = numPoints;

        // Calculate the angle between each point
        float angleIncrement = 360.0f / numPoints;

        // Determine the color based on whether food is detected
        Color circleColor = smellData ? Color.green : Color.red; // Adjust colors as desired

        // Update the circle's color
        lineRenderer.startColor = circleColor;
        lineRenderer.endColor = circleColor;

        // Update the positions to draw the circle
        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * angleIncrement;
            float x = tfm.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * smellRadius;
            float z = tfm.position.z + Mathf.Sin(Mathf.Deg2Rad * angle) * smellRadius;
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
    }*/

    public void Setup(CustomTerrain ct, GeneticAlgo ga)
    {
        terrain = ct;
        genetic_algo = ga;
        UpdateSetup();
    }

    public void SetEnergyLoss(float lossNew)
    {
        lossEnergy = lossNew;
    }

    private void UpdateSetup()
    {
        detailSize = terrain.detailSize();
        Vector3 gsz = terrain.terrainSize();
        terrainSize = new Vector2(gsz.x, gsz.z);
        details = terrain.getDetails();
    }

    public void InheritBrain(SimpleNeuralNet other, bool mutate)
    {
        brain = new SimpleNeuralNet(other);
        if (mutate)
            //brain.mutate(swapRate, mutateRate, swapStrength, mutateStrength);
            brain.mutate();
    }
    public SimpleNeuralNet GetBrain()
    {
        return brain;
    }
    public float GetHealth()
    {
        return energy / maxEnergy;
    }

}