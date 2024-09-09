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
    }
    public void PlayLightningSound()
    {
        lightningSound.Play();
    }
}
