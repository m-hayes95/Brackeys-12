using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rain_Manager : MonoBehaviour
{
    public static Rain_Manager instance;
    [SerializeField] Transform[] rainSpawnPositions;
    [SerializeField] List<GameObject> availableRainSplashes;
    [SerializeField] GameObject rainSplash;

    [Header("Rain Wave Settings")]
    [SerializeField] int totalWaves = 10;
    [SerializeField] int currentWave = 0;
    [SerializeField] bool endlessWaves;
    [SerializeField] List<GameObject> rainSplashesInUse;

    [Header("Game Stats")]
    [SerializeField] float timeElapsed = 0;
    private float baseTimeElapsed = 0;
    [SerializeField] bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        foreach (var item in rainSpawnPositions)
        {
            // spawnVectors.Add(item.position);
            availableRainSplashes.Add(Instantiate(rainSplash, item.position, quaternion.identity));
        }

        StartCoroutine(PreparatioTimer());
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

    IEnumerator EndlessWavesRoutine()
    {
        while (true)
        {
            int patternNum = 0; // Use Later: Random.Range(0, 0);

            yield return new WaitForSeconds(1);

            switch (patternNum)
            {
                case 0:
                    RandomSplash();
                    break;
                case 1:
                    
                    break;

            }
        }
    }

    IEnumerator StandardWavesRoutine()
    {
        while (currentWave <= totalWaves)
        {
            
        }
        yield break;
    }

    void RandomSplash()
    {
        Debug.Log("Spawned Splash");
    }

    void ActivateSplash()
    {
        
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
