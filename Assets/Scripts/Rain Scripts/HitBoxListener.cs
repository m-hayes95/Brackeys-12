using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class HitBoxListener : MonoBehaviour
{
    // Changed to delegate by Mike
    public delegate void PlayerCollisionEvent();
    public static event PlayerCollisionEvent OnHitPlayer;
    //[SerializeField] UnityEvent triggerEvent;
    //[SerializeField] string listeningTag = "Player";

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            //triggerEvent.Invoke();
            OnHitPlayer?.Invoke();
        }
    }
}
