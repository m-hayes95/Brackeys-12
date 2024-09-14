using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rain_Manager : MonoBehaviour
{
    public static Rain_Manager instance;
    [SerializeField] FallingObjectsManager fallingObjectsManager;
    [SerializeField] Transform[] rainSpawnPositions;
    [SerializeField] List<int> availableRainSplashes = new List<int>();
    [SerializeField] List<SplashScript> rainSplashScripts = new List<SplashScript>();
    [SerializeField] GameObject rainSplash;
    [SerializeField] Transform rainSplashFolder;

    public enum RainMode { Waves, SingleEndless, WavesEndless }
    
    [SerializeField] [Tooltip("Choose whether or not the game should be Endless (continue when all waves are completed), or set in Waves (Stop when all waves are completed)")]
     RainMode mode = RainMode.Waves;

    [System.Serializable]
    public struct Wave
    {
        public float duration; // Duration of the wave
        public float timePerPattern; // Time per rain pattern
        public float rainSpeed; // Animation Speed of each rain
    }

    [SerializeField] List<Wave> rainWaves = new List<Wave>();
    [SerializeField] int currentWave = 0;

    [Header("Pattern Stats")]
    [SerializeField] int randomSplashMin = 5;
    [SerializeField] int randomSplashMax = 11;
    [SerializeField] int scatterSplashMin = 8;
    [SerializeField] int scatterSplashMax = 16;

    [Header("Game Stats")]
    [SerializeField] float timeElapsed = 0; // Debug
    private float baseTimeElapsed = 0;
    [SerializeField] bool gameStarted = false;

    private void OnEnable()
    {
        CollectMudTimer.OnTimeOver += StartRain;
    }

    private void OnDisable()
    {
        CollectMudTimer.OnTimeOver -= StartRain;
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        InitializeRainSplashes();
    }

    private void InitializeRainSplashes()
    {
        int i = 0;
        foreach (var spawnPos in rainSpawnPositions)
        {
            GameObject GOBJ = Instantiate(rainSplash, spawnPos.position, Quaternion.identity, rainSplashFolder);
            GOBJ.name = "Rain Splash (" + i + ")";
            var splashScript = GOBJ.GetComponent<SplashScript>();
            splashScript.objectId = i;
            rainSplashScripts.Add(splashScript);
            availableRainSplashes.Add(i);
            i++;
        }
    }

    private void StartRain()
    {
        StartCoroutine(PreparationTimer());
    }

    IEnumerator PreparationTimer()
    {
        yield return new WaitForSeconds(3);

        gameStarted = true;

        if (mode == RainMode.SingleEndless)
        {
            StartCoroutine(EndlessWavesRoutine());
        }
        else
        {
            StartCoroutine(StandardWavesRoutine());
        }
    }

    public IEnumerator EndlessWavesRoutine()
    {
        while (gameStarted)
        {
            Wave current = rainWaves[currentWave];
            int patternNum = Random.Range(0, 2);

            if (availableRainSplashes.Count > 0)
            {
                ExecutePattern(patternNum);
            }
            yield return new WaitForSeconds(current.timePerPattern);
        }
    }

    IEnumerator StandardWavesRoutine()
    {
        while (currentWave < rainWaves.Count && gameStarted)
        {
            Wave current = rainWaves[currentWave];
            float elapsedTime = 0;
            Debug.Log("Wave " + currentWave + " Started. Duration: " + current.duration);
            while (elapsedTime < current.duration)
            {
                elapsedTime += current.timePerPattern;
                // Debug.Log(elapsedTime);

                int patternNum = Random.Range(0, 2);
                if (availableRainSplashes.Count > 0)
                {
                    ExecutePattern(patternNum);
                }
                yield return new WaitForSeconds(current.timePerPattern);
            }

            currentWave++;
            GlobalVariables.rainSpeedMultiplier = current.rainSpeed; // Update the rains speed;

            if (currentWave >= rainWaves.Count && mode == RainMode.Waves)
            {
                gameStarted = false;
                Debug.Log("Rain waves ended.");
            }
            else if (currentWave >= rainWaves.Count && mode == RainMode.WavesEndless)
            {
                currentWave = rainWaves.Count - 1; // Loop the final wave
                Debug.Log("Rain wave " + currentWave + " Looped");
            }
        }
    }

    void ExecutePattern(int patternNum)
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

    void RandomSplash()
    {
        int numberOfSelections = Mathf.Min(Random.Range(randomSplashMin, randomSplashMax), availableRainSplashes.Count);

        List<int> itemsToPickFrom = new List<int>(availableRainSplashes);

        for (int i = 0; i < numberOfSelections; i++)
        {
            int randomIndex = Random.Range(0, itemsToPickFrom.Count);
            int selectedObject = itemsToPickFrom[randomIndex];

            rainSplashScripts[selectedObject].EnabledObject();

            availableRainSplashes.Remove(selectedObject);
            itemsToPickFrom.RemoveAt(randomIndex);
        }
    }

    IEnumerator ScatterSplash()
    {
        int numberOfSelections = Mathf.Min(Random.Range(scatterSplashMin, scatterSplashMax), availableRainSplashes.Count);

        List<int> itemsToPickFrom = new List<int>(availableRainSplashes);

        for (int i = 0; i < numberOfSelections; i++)
        {
            int randomIndex = Random.Range(0, itemsToPickFrom.Count);
            int selectedObject = itemsToPickFrom[randomIndex];

            rainSplashScripts[selectedObject].EnabledObject();

            availableRainSplashes.Remove(selectedObject);
            itemsToPickFrom.RemoveAt(randomIndex);

            yield return new WaitForSeconds(.2f);
        }
    }

    void TriggerFallingObject()
    {
        fallingObjectsManager.DropObject();
    }

    public void ReturnSplashID(int ID)
    {
        availableRainSplashes.Add(ID);
    }
    public void ResetAllSplashScripts() // Reset Manager and Splash Scripts
    {
        StopAllCoroutines();
        foreach (var item in rainSplashScripts)
        {
            item.ResetSplash();
        }
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
