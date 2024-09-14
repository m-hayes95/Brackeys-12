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
    [SerializeField] private List<GameObject> fallingObjects = new List<GameObject>();

    private List<Vector3> usedSpawnPoints = new List<Vector3>();
    private List<GameObject> usedGameObjects = new List<GameObject>();
    private int currentGameObjectsInScene;
    private float currentTime;
    private bool canSpawn;

    private void Update()
    {
        if (currentGameObjectsInScene < maxObjectsCanSpawn && canSpawn)
        {
            SpawnNextObjectTimer();
        }
    }

    #region Drop an object

    private void SpawnNextObjectTimer()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= spawnTimer)
        {
            currentTime = 0;
            currentGameObjectsInScene++;
            InstantiateNewObject();
        }
            
    }
    private void InstantiateNewObject()
    {
        Instantiate(FindFallingObject(), FindRandomPosition(), quaternion.identity);
    }
    #endregion
    
    #region Get random object and spawn position not in use

    private GameObject FindFallingObject()
    {
        GameObject newFallingObject;
        do
        {
            int randomIndex = Random.Range(0, fallingObjects.Count);
            newFallingObject = fallingObjects[randomIndex];
        } 
        while (usedGameObjects.Contains(newFallingObject));
        
        usedGameObjects.Add(newFallingObject);
        return newFallingObject;
    }

    private Vector3 FindRandomPosition()
    {
        Vector3 newPosition;
        do
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            newPosition = spawnPoints[randomIndex].transform.position;
        } 
        while (usedSpawnPoints.Contains(newPosition));
        
        usedSpawnPoints.Add(newPosition);
        return newPosition;
    }
    
    #endregion
    
}
