using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class DirtyMeter : MonoBehaviour
    {
        public delegate void AllMudLost();
        public static event AllMudLost OnGameOver;
        public static DirtyMeter dirtyMeterScript;
        
        [SerializeField] private float currentMud; // Serialized for Debugging

        [SerializeField, Range(1, 100), Tooltip("Set how much mud water will remove when hit.")]
        int waterDamage;
        
        private PlayerMudPaintScript playerMudPaintScript;
        // private bool canCollectMud = true; // Replaced with Global Variables
        [SerializeField] float prevMud;
        [Header("Audio")]
        [SerializeField] AudioSource mudAudioSource;
        [SerializeField] AudioSource playerAudioSource;

        #region Event Subscription
        private void OnEnable()
        {
            HitBoxListener.OnHitPlayer += HitByWater;
            CollectMudTimer.OnTimeOver += StopMudCollection;
        }

        private void OnDisable()
        {
            HitBoxListener.OnHitPlayer -= HitByWater;
            CollectMudTimer.OnTimeOver -= StopMudCollection;
        }
        #endregion

        private void Start()
        {
            playerMudPaintScript = GetComponent<PlayerMudPaintScript>();
            dirtyMeterScript = this;
        }

        private void FixedUpdate()
        {
            if (GlobalVariables.playerCanPaint && !GlobalVariables.gamePaused)
                UpdateCurrentMud();
            else if (mudAudioSource.isPlaying)
                mudAudioSource.Stop();
        }
        private void UpdateCurrentMud()
        {
            if (!mudAudioSource.isPlaying)
                mudAudioSource.Play();
            
            currentMud = playerMudPaintScript.GetTotalMud();

            if (Mathf.Abs(prevMud - currentMud) > 0.01f)
            {
                if (!mudAudioSource.isPlaying)
                    mudAudioSource.UnPause();
            }
            else
            {
                if (mudAudioSource.isPlaying)
                    mudAudioSource.Pause();
            }

            prevMud = Mathf.Lerp(prevMud, currentMud, 0.1f);
        }

        private void HitByWater()
        {
            currentMud -= waterDamage;
            CameraShakeManager.instance.ShakeCamera(1,.2f);
            if (currentMud < 0)
            {
                currentMud = 0;
                CameraShakeManager.instance.ResetCamera();
                MainMenuManager.instance.ShowResultsMenu();
            }
        }

        private void StopMudCollection()
        {
            // canCollectMud = false;
            GlobalVariables.playerCanPaint = false;
        }
        public float GetPlayerMudTotal() // For UI and Sprite updates
        {
            return currentMud;
        }

    }
}
