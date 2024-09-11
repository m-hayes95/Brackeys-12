using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject startButton;

    [Header("Main Menu Variables")]
    [SerializeField] bool mainMenuActive = true;
    [SerializeField] GameObject mainMenuObject;
    [Header("Pause Menu Variables")]
    [SerializeField] GameObject pauseMenuObject;
    [SerializeField] bool canActivatePauseMenu;
    [SerializeField] float timeScaleFadeTime = 1;
    [Header("Mud Meter Variables")]
    [SerializeField] GameObject mudMeterObject;
    [SerializeField] RectTransform mudMeterTransform;
    [SerializeField] float transitionTime = 1.5f;
    [SerializeField] Vector3 appearPos;
    [SerializeField] Vector3 disappearPos;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        eventSystem.SetSelectedGameObject(startButton);
    }
    void Update()
    {
        PauseGameInput();
    }

    public void StartGame()
    {
        StartCoroutine(GameStartRoutine());
    }

    public void QuitGame() // Main Menu Quit Game
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

    IEnumerator GameStartRoutine()
    {
        // Trigger the Game Start Sequence Here!
        animator.SetTrigger("Disappear");
        yield return new WaitForSeconds(2);
        mainMenuObject.SetActive(false);
        canActivatePauseMenu = true;
        mainMenuActive = false;
        StartCoroutine(MudMeterTransition(true));
        yield break;
    }

    void PauseGameInput() // Pause Game Using ESC key
    {
        if (Input.GetButtonDown("Cancel") && canActivatePauseMenu)
        {
            pauseMenuObject.SetActive(true);
            animator.SetBool("Is Main Menu", false);
            animator.SetTrigger("Appear");
            canActivatePauseMenu = false;
            Time.timeScale = 0;
        }
    }

    public void ResumeGame() // Pause Menu Resume Game
    {
        animator.SetTrigger("Disappear");
        StartCoroutine(ResumeGameRoutine());
    }

    IEnumerator ResumeGameRoutine()
    {
        Time.timeScale = 0;
        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / timeScaleFadeTime;
            Time.timeScale = t;
            yield return null;
        }
        Time.timeScale = 1;
        canActivatePauseMenu = true;
        pauseMenuObject.SetActive(false);
        yield break;
    }

    public void EndGame() // Pause Menu End Game
    {
        mainMenuActive = true;
        canActivatePauseMenu = false;
        StartCoroutine(EndGameSequence());
    }

    IEnumerator EndGameSequence()
    {
        // Coroutine to stop whatever is going on (Placeholder atm, unsure if we even need this)
        animator.SetTrigger("Disappear");
        Debug.Log("Triggered Exit");
        StartCoroutine(MudMeterTransition(false));

        yield return new WaitForSecondsRealtime(2);
        pauseMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
        mainMenuActive = true;
        
        Debug.Log("Triggered Menu");

        // Reset Game Stats Here or trigger an event to do so!

        animator.SetBool("Is Main Menu", true);
        animator.SetTrigger("Appear");
        Time.timeScale = 1;
        Debug.Log("Triggered Menu");

        yield break;
    }

    IEnumerator MudMeterTransition(bool appearing)
    {
        float t = 0;
        Debug.Log("Mud UI Transition Started");
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / transitionTime;
            if (appearing) // Appear Transition
            {
                mudMeterTransform.anchoredPosition = Vector3.Lerp(disappearPos, appearPos, t);
            }
            else // Disappear Transition
            {
                mudMeterTransform.anchoredPosition = Vector3.Lerp(appearPos, disappearPos, t);
            } 
            yield return null;
        }
        Debug.Log("Mud UI Transition Ended");
        yield break;
    }
}