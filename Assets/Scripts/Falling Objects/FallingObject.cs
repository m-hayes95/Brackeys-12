using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingObject : MonoBehaviour
{
    [SerializeField] float fallTime;
    [SerializeField] float despawnTime;
    [SerializeField] Vector3 startScale;
    [SerializeField] private GameObject fallingVisuals;
    [SerializeField] private GameObject groundVisuals;
    [SerializeField] Animator animator;
    [SerializeField] PolygonCollider2D polyHitBox; // If applicable

    [SerializeField] bool fallenObjectsDespawn;
    private float currentTimer;

    private void Start()
    {
        //transform.localScale(new Vector3(startScale, startScale, startScale));
        ApplyRandomRotation();
        StartCoroutine(ObjectFalling(fallTime));
    }

    private void Update()
    {
        if (fallenObjectsDespawn)
            DespawnOnTimer();
    }

    private void DespawnOnTimer()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= despawnTime)
        {
            gameObject.SetActive(false);
        }
    }
    private void ApplyRandomRotation()
    {
        float randomEulerAngleZ = Random.Range(0, 360);
        transform.Rotate(new Vector3(0f, 0f, randomEulerAngleZ));
    }

    private IEnumerator ObjectFalling(float fallTime)
    {
        transform.localScale = startScale;
        Vector3 endScale = Vector3.one;
        float elapsed = 0f;
        
        while (elapsed < fallTime)
        {
            float time = elapsed / fallTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localScale = endScale;
        ObjectLanded();
    }

    private void ObjectLanded()
    {
        // play fall animation
        fallingVisuals.SetActive(false);
        groundVisuals.SetActive(true);
        // Play landed animation
    }
    
    
}
