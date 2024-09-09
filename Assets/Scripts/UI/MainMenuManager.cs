using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // [Header("Buttons")]
    // [SerializeField] Button startButton;
    // [SerializeField] Button quitButton;
    // [SerializeField] Button controlsToggle;
    [Header("Components")]
    [SerializeField] Image controlsToggleIndicator;
    [SerializeField] Sprite indicatorSprites;
    private Color placeholderindicatorcolourA = Color.red;
    private Color placeholderindicatorcolourB = Color.green;
    [SerializeField] Animator animator;

    [SerializeField] enum controls
    {
        Keys, cusor
    } 
    controls selectedControl = controls.cusor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator StartGame()
    {

        yield break;   
    }

    // Method to handle behavior based on platform
    public void QuitGame()
    {
        #if UNITY_EDITOR // If running in the Unity Editor, stop playing
            EditorApplication.isPlaying = false;

        #elif UNITY_STANDALONE // If running as an application, quit the app
            Debug.Log("Exiting standalone application");
            Application.Quit();

        #elif UNITY_WEBGL // If running in WebGL, do nothing
            Debug.Log("WebGL platform: No action taken");
        #endif
    }

    public void SwitchControls()
    {

    }
}