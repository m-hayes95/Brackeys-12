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

    enum controls
    {
        Keys, cusor
    } 
    [SerializeField] controls selectedControl = controls.cusor;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        animator.SetTrigger("Disappear");


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
        if (selectedControl == controls.cusor)
        {
            selectedControl = controls.Keys;
            controlsToggleIndicator.color = placeholderindicatorcolourA;
        }
        else
        {
            selectedControl = controls.cusor;
            controlsToggleIndicator.color = placeholderindicatorcolourB;
        }
    }
}