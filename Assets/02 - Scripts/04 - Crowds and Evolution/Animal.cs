using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{
    static private LinkedList<Animal> animals = new LinkedList<Animal>();

    [Header("Animal parameters")]
    public float swapRate = 0.01f;
    public float mutateRate = 0.01f;
    public float swapStrength = 10.0f;
    public float mutateStrength = 0.5f;
    public float maxAngle = 10.0f;


    [Header("Reproduction parameters")]
    public float eatingReproductionProbability = 0.2f;
    public float reproduceDistance = 2;
    public float reproduceDelay = 2.0f;
    public float minEnergyToReproduce = 4;
    [TagSelector] public string maleTag = "";
    [TagSelector] public string femaleTag = "";
    private float reproduceTimer;
    private bool isMale;


    [Header("Energy parameters")]
    public float maxEnergy = 10.0f;
    public float lossEnergy = 0.1f;
    public float gainEnergy = 10.0f;
    private float energy;

    [Header("Sensor - Vision")]
    public float maxVision = 20.0f;
    public float stepAngle = 10.0f;
    public int nEyes = 5;

    [Header("Sensor - Smelling")]
    public float minSmellStrenght = 5;
    public float smellDecreaseAmount = 0.05f;
    public float emitperiod = 0.2f;
    private float emitTimer = 0;
    [Space]
    public float noseStrenght = 5f;
    [TagSelector] public string grassTag = "";

    private int[] networkStruct;
    private NeuralNet brain = null;

    // Terrain.
    private CustomTerrain terrain = null;
    private int[,] details = null;
    private Vector2 detailSize;
    private Vector2 terrainSize;

    // Animal.
    private Transform tfm;
    private float[] vision;
    private float[] nose;
    static private int nSmells = 2; // angle and strongest for grass, male & female

    // Genetic alg.
    private GeneticAlgo genetic_algo = null;

    // Renderer.
    private Material mat = null;

    void Start()
    {
        // Network: 1 input per receptor, 1 output per actuator.
        vision = new float[nEyes];
        nose = new float[nSmells];
        networkStruct = new int[] {
            nEyes + nSmells,        // Vision receptor : number of eye | Nose receptor : number of different smells
            6,
            2 };                    // Number of output : the turn angle, the additional smell strength
        energy = maxEnergy;
        tfm = transform;
        animals.AddLast(this);
        reproduceTimer = reproduceDelay;
        isMale = UnityEngine.Random.value > 0.5f;
        if (isMale)
            gameObject.tag = maleTag;
        else
            gameObject.tag = femaleTag;


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
            brain = new NeuralNet(networkStruct);
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

        // If the animal is near to a another one, reporduce with it
        if (reproduceTimer > 0)
        {
            reproduceTimer -= Time.deltaTime;
        }
        else if (energy > minEnergyToReproduce)
        {
            // Can reproduce itself, check if there is a partner near
            foreach(var animal in animals)
            {
                /*if (animal == this)
                    continue;*/

                if ((isMale && animal.CompareTag(maleTag)) || (!isMale && animal.CompareTag(femaleTag)))
                    continue;

                if(animal.CanReproduce() && Vector3.Distance(transform.position, animal.transform.position) < reproduceDistance)
                {
                    reproduceTimer = reproduceDelay;
                    genetic_algo.addOffspring(this);
                    //energy -= maxEnergy/2;   // make the reproduction costly
                    //energy += 3 * lossEnergy;
                    //if (energy > maxEnergy)
                        energy = maxEnergy;
                    Debug.Log("Reproduced by proximity");
                    break;
                }
            }
        }

        // If the animal is located in the dimensions of the terrain and over a grass position (details[dy, dx] > 0), it eats it, gain energy and spawn an offspring.
        if ((dx >= 0) && dx < (details.GetLength(1)) && (dy >= 0) && (dy < details.GetLength(0)) && details[dy, dx] > 0)
        {
            // Eat (remove) the grass and gain energy.
            details[dy, dx] = 0;
            energy += gainEnergy;
            if (energy > maxEnergy)
                energy = maxEnergy;

            if (UnityEngine.Random.value < eatingReproductionProbability)    // Maybe reproduce (kept to make the animals learn the need to feed)
            {
                genetic_algo.addOffspring(this);
                Debug.Log("Reproduced by eating");
            }
        }

        // If the energy is below 0, the animal dies.
        if (energy < 0)
        {
            energy = 0.0f;
            genetic_algo.removeAnimal(this);
            animals.Remove(this);
            Debug.Log("Dead");
        }

        // Update the color of the animal as a function of the energy that it contains.
        if (mat != null)
            mat.color = Color.white * (energy / maxEnergy);

        // 1. Update receptor.
        UpdateVision();
        UpdateNose();

        // 2. Use brain.
        float[] input = new float[vision.Length + nose.Length];
        vision.CopyTo(input, 0);
        nose.CopyTo(input, vision.Length);
        float[] output = brain.getOutput(input);

        // 3. Act using actuators.
        float angle = (output[0] * 2.0f - 1.0f) * maxAngle;
        tfm.Rotate(0.0f, angle, 0.0f);

        EmitSmell(minSmellStrenght * (1 + output[1]));
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
    private void UpdateNose()
    {
        // Reset
        float cumulate = 0;
        float angle = 0;

        SmellFactory.Smelling(isMale ? femaleTag : maleTag, gameObject, noseStrenght, ref cumulate, ref angle);
        nose[0] = cumulate/(2 * minSmellStrenght);
        nose[1] = angle;
    }

    public void Setup(CustomTerrain ct, GeneticAlgo ga)
    {
        terrain = ct;
        genetic_algo = ga;
        UpdateSetup();
    }

    private void UpdateSetup()
    {
        detailSize = terrain.detailSize();
        Vector3 gsz = terrain.terrainSize();
        terrainSize = new Vector2(gsz.x, gsz.z);
        details = terrain.getDetails();
    }

    public void InheritBrain(NeuralNet other, bool mutate)
    {
        brain = new NeuralNet(other);
        if (mutate)
            brain.mutate(swapRate, mutateRate, swapStrength, mutateStrength);
    }
    public NeuralNet GetBrain()
    {
        return brain;
    }
    public float GetHealth()
    {
        return energy / maxEnergy;
    }
    public bool CanReproduce()
    {
        return reproduceTimer < 0 && energy > minEnergyToReproduce;
    }

    private void EmitSmell(float strenght)
    {
        emitTimer += Time.deltaTime;
        if (emitTimer > emitperiod)
        {
            emitTimer -= emitperiod;
            SmellFactory.AddSmell(gameObject, strenght, smellDecreaseAmount, true, isMale ? 0 : 1 );
        }
    }
}
