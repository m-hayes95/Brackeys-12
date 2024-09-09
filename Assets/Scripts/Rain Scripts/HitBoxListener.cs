using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitBoxListener : MonoBehaviour
{
    [SerializeField] UnityEvent triggerEvent;
    [SerializeField] string listeningTag = "Player";

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(listeningTag))
        {
            triggerEvent.Invoke();
        }
    }
}
