using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningFlash : MonoBehaviour
{
    [SerializeField]private float minIntensity;
    [SerializeField]private float maxIntensity;
    [SerializeField]private float lerpStep;
    [SerializeField]private float lightningTimeOnScreen;
    private Light2D lightning;
    [SerializeField]private float maxTime;
    private float currentTime;
    private bool doOnce;
    private bool isRaining;
    
    private void OnEnable()
    {
        CollectMudTimer.OnTimeOver += RainStarted;
    }
    private void OnDisable()
    {
        CollectMudTimer.OnTimeOver -= RainStarted;
    }

    private void RainStarted()
    {
        isRaining = true;
    }

    private void Start()
    {
        lightning = GetComponent<Light2D>();
        currentTime = maxTime;
        Debug.Log(lightning.intensity);
    }
    private void Update()
    {
        if (isRaining)
            Timer();
    }
    private void Timer()
    {
        if (currentTime > 0)
            currentTime -= Time.deltaTime;
        else if (!doOnce)
        {
            doOnce = true;
            SoundManager.Instance.PlayLightningSound();
            CreateLightningEffect();
            StartCoroutine(ResetTime());
        }
            
    }

    private IEnumerator ResetTime()
    {
        float seconds = Random.Range(8f, 20f);
        yield return new WaitForSeconds(seconds);
        currentTime = maxTime;
        doOnce = false;
    }
    
    private void CreateLightningEffect()
    {
        lightning.intensity = maxIntensity;
        Invoke(nameof(ResetLightningEffect), lightningTimeOnScreen);
    }
    private void ResetLightningEffect()
    {
        lightning.intensity = minIntensity;
    }
}
