using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectsManager : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] Transform[] positions;
    int positionCount;
    // Start is called before the first frame update
    void Start()
    {
        positionCount = positions.Length;
    }

    public void DropObject()
    {
        Random.Range(0, positionCount);
    }


}
