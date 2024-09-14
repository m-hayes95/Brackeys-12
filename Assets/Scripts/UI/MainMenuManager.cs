using System.Collections;
using Audio;
using Player;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject startButton;
    [SerializeField] CollectMudTimer mudTimerScript;
    [SerializeField] PlayerMudPaintScript mudPaintScript;
    [SerializeField] Rain_Manager rainManagerScript;
    [SerializeField] Transform playerTransform;
    [SerializeField] AudioSource audioSource;

    [Header("Main Menu Variables")]
    [SerializeField] bool mainMenuActive = true;
    [SerializeField] GameObject mainMenuObject;
    // [SerializeField] AudioClip menuEnterSound;
    // [SerializeField] AudioClip menuExitSound;
    [Header("Pause Menu Variables")]
    [SerializeField] GameObject pauseMenuObject;
    [SerializeField] bool canActivatePauseMenu;
    [SerializeField] float timeScaleFadeTime = 1;
    [SerializeField] AudioClip pauseSound;
    [SerializeField] AudioClip unpauseSound;
    [Header("Mud Meter Variables")]
    [SerializeField] GameObject mudMeterObject;
    [SerializeField] RectTransform mudMeterTransform;
    [SerializeField] Image mudMeterFillImage;
    [SerializeField] float mudMeterTransitionTime = 1.5f;
    [SerializeField] Vector3 appearPos;
    [SerializeField] Vector3 disappearPos;
    [Header("Cloud Transition Variables")]
    [SerializeField] RectTransform cloudTransform;
    [SerializeField] float cloudTransitionTime = 2;
    [SerializeField] Vector3 cloudStartPos;
    [SerializeField] Vector3 cloudCentrePos = new();
    [SerializeField] Vector3 cloudEndPos;
    [Header("Results Menu Variables")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject resultsObject;

    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        eventSystem.SetSelectedGameObject(startButton);
    }
    void Update()
    {
        PauseGameInput();
        if (GlobalVariables.gameStarted)
        {
            mudMeterFillImage.fillAmount = Mathf.InverseLerp(0 , 100, DirtyMeter.dirtyMeterScript.GetPlayerMudTotal());
        }
    }

    public void StartGame()
    {
        AudioManager.Instance.PauseAudio(false, .5f);
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

        GlobalVariables.instance.StartGame();

        StartCoroutine(MudMeterTransition(true));

        mudTimerScript.StartTimer(); // Start the Mud Timer
        yield break;
    }

    void PauseGameInput() // Pause Game Using ESC key
    {
        if (Input.GetButtonDown("Cancel") && canActivatePauseMenu)
        {
            pauseMenuObject.SetActive(true);
            audioSource.PlayOneShot(pauseSound);
            animator.SetBool("Is Main Menu", false);
            animator.SetTrigger("Appear");
            canActivatePauseMenu = false;
            Time.timeScale = 0;
            AudioManager.Instance.PauseAudio(true, .5f);
        }
    }

    public void ResumeGame() // Pause Menu Resume Game
    {
        animator.SetTrigger("Disappear");
        audioSource.PlayOneShot(unpauseSound);
        StartCoroutine(ResumeGameRoutine());
    }

    IEnumerator ResumeGameRoutine()
    {
        Time.timeScale = 0;
        float t = 0;
        AudioManager.Instance.PauseAudio(false, .5f);
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
        GlobalVariables.gameStarted = false;
        Time.timeScale = 1;
        StartCoroutine(EndGameSequence());
    }

    IEnumerator EndGameSequence()
    {
        animator.SetTrigger("Disappear");
        Debug.Log("Triggered Exit");

        StartCoroutine(MudMeterTransition(false));
        float t = 0;
        while (t < 1) // Move Cloud
        {
            t += Time.unscaledDeltaTime / (cloudTransitionTime / 2);
            cloudTransform.anchoredPosition = Vector3.Lerp(cloudStartPos, cloudCentrePos, t);

            yield return null;
        }
        GlobalVariables.instance.ResetGame(); // Reset Game
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
        animator.ResetTrigger("Disappear");
        animator.SetTrigger("Appear");
        Time.timeScale = 1;
        yield break;
    }

    IEnumerator MudMeterTransition(bool appearing)
    {
        float t = 0;
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
        yield break;
    }

    public void ShowResultsMenu()
    {
        resultsObject.SetActive(true);
        Time.timeScale = 0;
        canActivatePauseMenu = false;
        scoreText.SetText("Score: " + Mathf.Round(GlobalVariables.instance.CalculatedScore()) * 69f); // 69 is a placeholder to make the score bigger than it already is
        animator.SetTrigger("Results Appear");
    }
    public void HideResultsMenu()
    {
        StartCoroutine(HideResultsMenuRoutine());
    }

    IEnumerator HideResultsMenuRoutine()
    {
        animator.SetTrigger("Disappear");
        yield return new WaitForSecondsRealtime(2);
        resultsObject.SetActive(false);
        StartCoroutine(EndGameSequence());
        yield break;
    }
}