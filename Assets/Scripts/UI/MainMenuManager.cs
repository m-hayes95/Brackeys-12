using System.Collections;
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
    [SerializeField] CollectMudTimer mudTimerScript;
    [SerializeField] PlayerMudPaintScript mudPaintScript;
    [SerializeField] Rain_Manager rainManagerScript;
    [SerializeField] Transform playerTransform;

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
    [SerializeField] float mudMeterTransitionTime = 1.5f;
    [SerializeField] Vector3 appearPos;
    [SerializeField] Vector3 disappearPos;
    [Header("Cloud Transition Variables")]
    [SerializeField] RectTransform cloudTransform;
    [SerializeField] float cloudTransitionTime = 2;
    [SerializeField] Vector3 cloudStartPos;
    [SerializeField] Vector3 cloudCentrePos = new();
    [SerializeField] Vector3 cloudEndPos;

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
        GlobalVariables.playerCanMove = true;
        GlobalVariables.playerCanPaint = true;

        StartCoroutine(MudMeterTransition(true));

        StartCoroutine(mudTimerScript.StartTimer()); // Start the Mud Timer Coroutine
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
        Time.timeScale = 1;
        StartCoroutine(EndGameSequence());
    }

    IEnumerator EndGameSequence()
    {
        animator.SetTrigger("Disappear");
        Debug.Log("Triggered Exit");
        StopCoroutine(mudTimerScript.StartTimer()); // Stop Mud Timer
        mudTimerScript.currentTime = 0; // Reset Time
        rainManagerScript.StopAllCoroutines(); // Stop Rain
        StartCoroutine(MudMeterTransition(false));
        float t = 0;
        bool variablesReset = false;
        while (t < 1) // Move Cloud
        {
            t += Time.unscaledDeltaTime / (cloudTransitionTime / 2);
            cloudTransform.anchoredPosition = Vector3.Lerp(cloudStartPos, cloudCentrePos, t);
            if (!variablesReset) // Reset Game Variables
            {
                variablesReset = true;
                mudPaintScript.ApplyTexture(); // Consider changing this soon to an event variable?
                GlobalVariables.score = 0;
                GlobalVariables.playerCanMove = false;
                GlobalVariables.playerCanPaint = false;
                playerTransform.position = new Vector3();
                playerTransform.rotation = new Quaternion();
            }
            else yield return null;
        }
        t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / (cloudTransitionTime / 2);
            cloudTransform.anchoredPosition = Vector3.Lerp(cloudCentrePos, cloudEndPos, t);
            yield return null;
        }
        cloudTransform.anchoredPosition = cloudEndPos;
        pauseMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
        mainMenuActive = true;

        animator.SetBool("Is Main Menu", true);
        animator.SetTrigger("Appear");
        Time.timeScale = 1;
        yield break;
    }

    IEnumerator MudMeterTransition(bool appearing)
    {
        float t = 0;
        Debug.Log("Mud UI Transition Started");
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / mudMeterTransitionTime;
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