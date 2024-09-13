using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMudTimer : MonoBehaviour
{
    public delegate void TimeOverEvent();
    public static event TimeOverEvent OnTimeOver;
    [SerializeField, Range(0f,60f), Tooltip("Change the amount of time that the player has to collect mud (in seconds)")]
    private float totalTime;
    Coroutine coroutine;

    public float currentTime;
    private void Start()
    {
        currentTime = totalTime;
    }
    public IEnumerator Timer()
    {
        currentTime = totalTime;
        while (currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }
        OnTimeOver?.Invoke(); // Subs: RainManager & PlayerHudUpdateText & DirtyMeter
        yield break;
    }

    public void EndTimer()
    {
        StopCoroutine(coroutine);
        currentTime = 0;
    }
    public void StartTimer()
    {
        coroutine = StartCoroutine(Timer());
        currentTime = totalTime;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
