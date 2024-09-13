using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rain_Manager : MonoBehaviour
{
    public static Rain_Manager instance;
    [SerializeField] Transform[] rainSpawnPositions;
    [SerializeField] List<int> availableRainSplashes;
    [SerializeField] List<SplashScript> rainSplashScripts;
    public int availableRainSplashesCount = 0;
    [SerializeField] GameObject rainSplash;
    [SerializeField] Transform rainSplashFolder;

    [Header("Rain Wave Settings")]
    [SerializeField] int totalWaves = 10;
    [SerializeField] int currentWave = 0;
    [SerializeField] bool endlessWaves;
    [SerializeField] List<int> rainSplashesInUse;

    [Header("Pattern Stats")]
    [SerializeField] int randomSplashMin = 5;
    [SerializeField] int randomSplashMax = 11;
    [SerializeField] int scatterSplashMin = 8;
    [SerializeField] int scatterSplashMax = 16;
    [Header("Game Stats")]
    [SerializeField] float timeElapsed = 0;
    private float baseTimeElapsed = 0;
    [SerializeField] bool gameStarted;

    private void OnEnable()
    {
        CollectMudTimer.OnTimeOver += StartRain;
    }

    private void OnDisable()
    {
        CollectMudTimer.OnTimeOver -= StartRain;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        int i = 0;
        foreach (var item in rainSpawnPositions)
        {
            // spawnVectors.Add(item.position);
            GameObject GOBJ = Instantiate(rainSplash, item.position, quaternion.identity, rainSplashFolder);
            GOBJ.name = "Rain Splash (" + i + ")";
            GOBJ.GetComponent<SplashScript>().objectId = i;
            rainSplashScripts.Add(GOBJ.GetComponent<SplashScript>());
            availableRainSplashes.Add(i);
            i++;
        }
        availableRainSplashesCount = availableRainSplashes.Count;
    }

    private void StartRain()
    {
        StartCoroutine(PreparatioTimer());
    }

    public void ReturnSplashID(int ID) // Sent from the Rain Splash Object
    {
        availableRainSplashes.Add(ID);
    }

    IEnumerator PreparatioTimer()
    {
        yield return new WaitForSeconds(3);

        if (endlessWaves)
        {
            gameStarted = true;
            StartCoroutine(EndlessWavesRoutine());
        }
        else
        {
            gameStarted = true;
            StartCoroutine(StandardWavesRoutine());
        }

        yield break;
    }

    public IEnumerator EndlessWavesRoutine()
    {
        while (gameStarted)
        {
            int patternNum = Random.Range(0, 2);

            yield return new WaitForSeconds(1 / GlobalVariables.rainSpeedMultiplier);
            if (availableRainSplashesCount > 0)
            {
                switch (patternNum)
                {
                    case 0:
                        RandomSplash();
                        Debug.Log("Random Splash");
                        break;
                    case 1:
                        StartCoroutine(ScatterSplash());
                        Debug.Log("Scatter Splash");
                        break;

                }
            }
            yield return null;
        }
    }

    IEnumerator StandardWavesRoutine()
    {
        while (currentWave <= totalWaves && gameStarted)
        {
            yield return null;
        }
        yield break;
    }

    void RandomSplash()
    {
        int numberOfSelections = Mathf.Min(Random.Range(randomSplashMin, randomSplashMax), availableRainSplashesCount);

        // Create a copy of the available object list to pick unique random objects
        List<int> itemsToPickFrom = new List<int>(availableRainSplashes);

        // Randomly select objects from the available list
        for (int i = 0; i < numberOfSelections; i++)
        {
            // Ensure we are picking from the available objects
            int randomIndex = Random.Range(0, itemsToPickFrom.Count);
            int selectedObject = itemsToPickFrom[randomIndex];

            // Trigger the TurnMeOn event
            rainSplashScripts[selectedObject].EnabledObject();

            // Remove the selected object from the available list to prevent immediate reselection
            availableRainSplashes.Remove(selectedObject);
            itemsToPickFrom.RemoveAt(randomIndex);
        }

    }

    IEnumerator ScatterSplash()
    {
        int numberOfSelections = Mathf.Min(Random.Range(5, 11), availableRainSplashesCount);

        // Create a copy of the available object list to pick unique random objects
        List<int> itemsToPickFrom = new List<int>(availableRainSplashes);

        // Randomly select objects from the available list
        for (int i = 0; i < numberOfSelections; i++)
        {
            // Ensure we are picking from the available objects
            int randomIndex = Random.Range(0, itemsToPickFrom.Count);
            int selectedObject = itemsToPickFrom[randomIndex];

            // Trigger the TurnMeOn event
            rainSplashScripts[selectedObject].EnabledObject();

            // Remove the selected object from the available list to prevent immediate reselection
            availableRainSplashes.Remove(selectedObject);
            itemsToPickFrom.RemoveAt(randomIndex);

            yield return new WaitForSeconds(.2f);
        }

        yield break;
    }

    void FixedUpdate()
    {
        if (gameStarted)
        {
            baseTimeElapsed += Time.fixedDeltaTime;
            timeElapsed = Mathf.Round(baseTimeElapsed * 100) / 100;
        }
    }
}

