using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingObjectsSpawner : MonoBehaviour
{
    [SerializeField] private float spawnTimer;
    [SerializeField] private int maxObjectsCanSpawn;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] GameObject[] prefabs;
    [SerializeField] List<GameObject> fallingObjects;
    [SerializeField] List<FallingObject> fallingObjectScripts;
    [SerializeField] Transform folder;

    // Use HashSet for O(1) lookups instead of List
    private HashSet<Vector3> usedSpawnPoints = new HashSet<Vector3>();
    private HashSet<GameObject> usedGameObjects = new HashSet<GameObject>();

    private int currentGameObjectsInScene;
    private float currentTime;
    private bool canSpawn = false;

    void Start()
    {
        foreach (var item in prefabs)
        {
            GameObject spawnedObject = Instantiate(item, Vector3.zero, quaternion.identity, folder);
            fallingObjects.Add(spawnedObject);
            fallingObjectScripts.Add(spawnedObject.GetComponent<FallingObject>());
            spawnedObject.SetActive(false);
        }
    }

    #region Drop an object
    public void DropObject(int length, float duration)
    {
        usedGameObjects.Clear();
        usedSpawnPoints.Clear();

        // Ensure we don't exceed the maxObjectsCanSpawn limit
        int objectsToSpawn = Mathf.Min(length, maxObjectsCanSpawn - currentGameObjectsInScene);

        for (int i = 0; i < objectsToSpawn; i++)
        {
            int objectNum = FindFallingObject();
            Vector3 spawnPosition = FindRandomPosition();

            // Activate and position the object
            GameObject fallingObject = fallingObjects[objectNum];
            fallingObject.SetActive(true);
            fallingObject.transform.position = spawnPosition;

            // Set despawn time and trigger fall
            FallingObject fallingObjectScript = fallingObjectScripts[objectNum];
            fallingObjectScript.despawnTime = duration;
            fallingObjectScript.Fall();
        }

        // Update the current game objects count in the scene
        currentGameObjectsInScene += objectsToSpawn;
    }

    private int FindFallingObject()
    {
        // Find a random inactive object
        int randomIndex = Random.Range(0, fallingObjects.Count);
        while (usedGameObjects.Contains(fallingObjects[randomIndex]))
        {
            randomIndex = Random.Range(0, fallingObjects.Count);
        }

        usedGameObjects.Add(fallingObjects[randomIndex]); // Mark as used

        return randomIndex;
    }

    private Vector3 FindRandomPosition()
    {
        // Find a random unused spawn point
        Vector3 newPosition;
        do
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            newPosition = spawnPoints[randomIndex].position;
        } 
        while (usedSpawnPoints.Contains(newPosition)); // Repeat if position is already used

        usedSpawnPoints.Add(newPosition); // Mark spawn point as used
        return newPosition;
    }
    #endregion

    public void ClearOnScreenObjects() // Clear all Fallen Objects
    {
        foreach (FallingObject item in fallingObjectScripts)
        {
            item.Despawn();
        }

        usedGameObjects.Clear();
        usedSpawnPoints.Clear();

        currentGameObjectsInScene = 0;
    }
}
