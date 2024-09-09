using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get;
        private set;
    }
    
    [SerializeField] private AudioSource lightningSound;
    [SerializeField] private AudioSource rainSound;
    [SerializeField] private AudioSource sunnySound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
            
    }
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
        rainSound.Play();
        sunnySound.Stop();
    }
    public void PlayLightningSound()
    {
        lightningSound.Play();
    }
}
