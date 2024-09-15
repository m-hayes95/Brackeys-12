using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;
using UnityEngine.Playables;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private Dictionary<AudioType, AudioSource> soundTypeDictionary;
        [SerializeField] private AudioSource pigSnortSounds;
        [SerializeField] private AudioClip[] pigSoundVariants;
        [SerializeField] private AudioSource farmAmbienceTrack;
        [SerializeField] private AudioSource heavyRainTrack;
        [SerializeField] private AudioClip[] rainSounds;
        [SerializeField] private AudioSource fallingObject;
        [SerializeField] private AudioClip[] impactSounds;
        [SerializeField] private AudioSource mainMenuMusic; // Change music to clip array later
        [SerializeField] private AudioSource runningMusic; // Change music to clip array later
        [SerializeField] AudioMixerSnapshot unpausedSnap;
        [SerializeField] AudioMixerSnapshot pausedSnap;

        [SerializeField]private PlayableDirector stormTimeline;
        private int lastClipIndex;
        private int rainSoundIndex; // quick implementation >> change
        private bool doOnce;

        #region Singleton Pattern
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        #endregion

        #region Dictionary for Audio Types
        private void Start()
        {
            PopulateAudioTypeDictionary();
        }
        private void PopulateAudioTypeDictionary()
        {
            soundTypeDictionary = new Dictionary<AudioType, AudioSource>
            {
                { AudioType.FarmAmbienceTrack, farmAmbienceTrack },
                { AudioType.HeavyRainTrack, heavyRainTrack },
                { AudioType.PigSnortSound_V, pigSnortSounds },
                { AudioType.FallingObject, fallingObject },
                { AudioType.RunningMusic, runningMusic },
                { AudioType.MenuMusic, mainMenuMusic }
            };
        }
        #endregion

        #region Control Sounds
        public void PlaySound(AudioType audioType)
        {
            if (!soundTypeDictionary.ContainsKey(audioType))
            { 
                Debug.LogWarning($"Audio Type: {audioType} cannot be found. Check AudioType class for compatible names");
                return;
            }
            
            // Play variant track of single track depending on audio type chosen
            if (
                Enum.GetName(typeof(AudioType), audioType).EndsWith("V") &&
                    !soundTypeDictionary[audioType].isPlaying
                )
            {
                soundTypeDictionary[audioType].PlayOneShot(pigSoundVariants[RepeatSoundCheck(pigSoundVariants.Length, lastClipIndex)]);
            }
            if (!soundTypeDictionary[audioType].isPlaying)
                soundTypeDictionary[audioType].Play();
        }

        public void PlaySoundFadeIn(AudioType audioType, float step)
        {
            if (!soundTypeDictionary.ContainsKey(audioType))
            { 
                Debug.LogWarning($"Audio Type: {audioType} cannot be found. Check AudioType class for compatible names");
                return;
            }

            var track = soundTypeDictionary[audioType];
            if (!track.isPlaying)
            {
                track.volume = 0f;
                track.Play();
                if (track.volume < 1f)
                {
                    track.volume = Mathf.Lerp(track.volume, 1f, step);
                }
            }
        }
        public void StopSound(AudioType audioType)
        {
            if (!soundTypeDictionary.ContainsKey(audioType))
            { 
                Debug.LogWarning($"Audio Type: {audioType} cannot be found. Check AudioType class for compatible names");
                return;
            }
            // Stop variant track of single track depending on audio type chosen
            if (soundTypeDictionary[audioType].isPlaying)
                soundTypeDictionary[audioType].Stop();
        }
        
        private int RepeatSoundCheck(int range, int lastIndex)
        {
            int index = Random.Range(0, range);
            while (index == lastIndex)
            {
                index = Random.Range(0, range);
            }
            
            lastClipIndex = index;
            return index;
        }

        public void PauseAudio(bool paused, float time)
        {
            if (paused)
                pausedSnap.TransitionTo(time);
            else
                unpausedSnap.TransitionTo(time);
        }
        #endregion

        public void StartStormTrackTimeline()
        {
            //stormTimeline.Play();
            Debug.Log($"Sound played from game start script and index set to {rainSoundIndex}");
            if (rainSounds.Length > 0 && !doOnce)
            {
                PlayNextClip();
            }
        }

        private void PlayNextClip()
        {
            if (rainSoundIndex < rainSounds.Length)
            {
                heavyRainTrack.clip = rainSounds[rainSoundIndex];
                heavyRainTrack.Play();
                Debug.Log($"rain started current sound playing = {heavyRainTrack.clip.name}");
                Invoke(nameof(WaitForTrackEnd), heavyRainTrack.clip.length);
            }

            if (rainSoundIndex >= 3)
            {
                int lastTrackInArray = 2;
                heavyRainTrack.clip = rainSounds[lastTrackInArray];
                heavyRainTrack.Play();
                Debug.Log($"rain started current sound playing = {heavyRainTrack.clip.name}");
                Invoke(nameof(WaitForTrackEnd), heavyRainTrack.clip.length);
            }
        }

        private void WaitForTrackEnd()
        {
            rainSoundIndex++; 
            Debug.Log("Play next audio");
            PlayNextClip();
        }

        public void ResetStormTrackLoop()
        {
            rainSoundIndex = 0;
            doOnce = false;
        }
    }
}

