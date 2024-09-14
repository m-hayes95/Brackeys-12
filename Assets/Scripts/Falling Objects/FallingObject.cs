using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] float fallTime = 2;
    [SerializeField] float lifeTime = 1;
    [SerializeField] float fallSpeedMultiplier = 1; // Debugging Purposes
    [SerializeField] float newFallTime; // Debugging Purposes
    [SerializeField] float newLifeTime; // Debugging Purposes
    [SerializeField] Animator animator;
    [SerializeField] PolygonCollider2D polyHitBox; // If applicable
    [SerializeField] CircleCollider2D circleHitBox; // If applicable
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
