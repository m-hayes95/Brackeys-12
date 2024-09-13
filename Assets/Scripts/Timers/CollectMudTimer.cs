using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMudTimer : MonoBehaviour
{
    public delegate void TimeOverEvent();
    public static event TimeOverEvent OnTimeOver;
    [SerializeField, Range(0f,60f), Tooltip("Change the amount of time that the player has to collect mud (in seconds)")]
    private float totalTime;
    private bool doOnce;

    public float currentTime;
    private void Start()
    {
        currentTime = totalTime;
    }
    public IEnumerator StartTimer()
    {
        while (true)
        {
            if (currentTime >= 0)
                currentTime -= Time.deltaTime;
            else if (!doOnce)
            {
                TimeLimitReached();
                yield break;
            }
            yield return null;
        }
    }

    private void TimeLimitReached()
    {
        //Debug.Log("Time Over");
        doOnce = true;
        OnTimeOver?.Invoke(); // Subs: RainManager & PlayerHudUpdateText & DirtyMeter
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
