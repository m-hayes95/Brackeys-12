using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private Dictionary<AudioType, AudioSource> soundTypeDictionary;
        [SerializeField] private AudioSource pigSnortSounds;
        [SerializeField] private AudioClip[] pigSoundVariants;
        [SerializeField] private AudioSource peacefulTrack;
        [SerializeField] private AudioSource stormTrack;
        [SerializeField] private AudioSource mudSound; // Used for testing single sound effect
        [SerializeField] AudioMixerSnapshot unpausedSnap;
        [SerializeField] AudioMixerSnapshot pausedSnap;
        private int lastClipIndex;

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
            PopulateAudioTypeDictionarie();
        }
        private void PopulateAudioTypeDictionarie()
        {
            soundTypeDictionary = new Dictionary<AudioType, AudioSource>
            {
                { AudioType.PeacefulTrack, peacefulTrack },
                { AudioType.StormTrack, stormTrack },
                { AudioType.PigSnortSound_V, pigSnortSounds }
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
    }
}

