using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Main Menu Info")]
    [SerializeField] bool mainMenuActive = true;
    [SerializeField] GameObject mainMenuObject;
    [Header("Pause Menu Info")]
    [SerializeField] GameObject pauseMenuObject;
    [SerializeField] bool canActivatePauseMenu;

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
        yield break;
    }

    void PauseGameInput() // Pause Game Using ESC key
    {
        if (Input.GetButtonDown("Cancel") && canActivatePauseMenu)
        {
            pauseMenuObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame() // Pause Menu Resume Game
    {
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void EndGame() // Pause Menu End Game
    {
        StartCoroutine(EndGameSequence());
    }

    IEnumerator EndGameSequence()
    {
        // Coroutine to stop whatever is going on (Placeholder atm, unsure if we even need this)
        yield break;
    }
}