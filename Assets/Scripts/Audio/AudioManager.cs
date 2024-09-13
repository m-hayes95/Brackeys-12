using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private Dictionary<AudioType, AudioSource[]> soundVariantsDictionary;
        private Dictionary<AudioType, AudioSource> singleSoundDictionary;
        [SerializeField] private AudioSource[] pigSoundsVar;
        [SerializeField] private AudioSource[] mudSoundsVar;
        [SerializeField] private AudioSource peacefulTrack;
        [SerializeField] private AudioSource stormTrack;
        [SerializeField] private AudioSource mudSound; // Used for testing single sound effect

        #region Singleton Pattern
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        #endregion

        #region Dictionaries for Audio Types
        private void Start()
        {
            PopulateSoundVariantsDictionaries();
            PopulateSingleDictionaries();
        }

        private void PopulateSoundVariantsDictionaries()
        {
            soundVariantsDictionary = new Dictionary<AudioType, AudioSource[]>
            {
                { AudioType.PigSound_V, pigSoundsVar },
                { AudioType.MudSound_V, mudSoundsVar }
            };
        }
        private void PopulateSingleDictionaries()
        {
            singleSoundDictionary = new Dictionary<AudioType, AudioSource>
            {
                { AudioType.PeacefulTrack, peacefulTrack },
                { AudioType.StormTrack, stormTrack }
            };
        }
        #endregion

        #region Control Sounds
        public void PlaySound(AudioType audioType)
        {
            /*
            if (!soundVariantsDictionary.ContainsKey(audioType) || !singleSoundDictionary.ContainsKey(audioType))
            { 
                Debug.LogWarning($"Audio Type: {audioType} cannot be found. Check AudioType class for compatible names");
                return;
            }
            */
            // Play variant track of single track depending on audio type chosen
            if (Enum.GetName(typeof(AudioType), audioType).EndsWith("V"))
            {
                Debug.Log("Playing random variant sound");
                AudioSource[] variants = soundVariantsDictionary[audioType];
                //variants[4].Play(); // playing random variants does not work and playing directly here plays it twice and then stops playing
                RepeatSoundCheck(variants, variants.Length);
            }
            else if (!singleSoundDictionary[audioType].isPlaying)
            {
                Debug.Log($"Played single Sound {audioType}");
                singleSoundDictionary[audioType].Play();
            }
                
        }

        public void StopSound(AudioType audioType)
        {
            /*
            
            if (!soundVariantsDictionary.ContainsKey(audioType) || !singleSoundDictionary.ContainsKey(audioType))
            { 
                Debug.LogWarning($"Audio Type: {audioType} cannot be found. Check AudioType class for compatible names");
                return;
            }
            */
            // Stop variant track of single track depending on audio type chosen
            if (Enum.GetName(typeof(AudioType), audioType).EndsWith("V"))
            {
                Debug.Log($"Stoped Playing Sound variant {audioType}");
                AudioSource[] variants = soundVariantsDictionary[audioType];
                foreach (AudioSource source in variants)
                {
                    source.Stop();
                }
            }
            else if (singleSoundDictionary[audioType].isPlaying)
            {
                Debug.Log($"Stoped Playing Sound {audioType}");
                singleSoundDictionary[audioType].Stop();
            }
                
        }
        #endregion
        private int RepeatSoundCheck(AudioSource[] soundVariations, int range)
        {
            int index = Random.Range(0, range);
            if (range == 1)
                return index;
            while (soundVariations[index].isPlaying)
            {
                index = Random.Range(0, range);
            }
            return index;
        }
        /*
        public void PlayPigSounds()
        {
            int index = RepeatSoundCheck(pigSoundsVar, pigSoundsVar.Length);
            pigSoundsVar[index].Play();
        }

        public void PlayMovingSound()
        {
            if (!mudSound.isPlaying)
            {
                mudSound.Play();
            }
        }

        private void StopSound(AudioSource sound)
        {
            if (sound.isPlaying)
                sound.Stop();
        }
        */
        
        
    }
}

