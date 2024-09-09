using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHudUpdateText : MonoBehaviour
{
    [SerializeField, Tooltip("Add reference to Collect Mud Timer, under Timers, to this field")] 
    private CollectMudTimer collectedMudTimer;
    [SerializeField, Tooltip("Add reference for the UI timer text to this field")]
    private TextMeshProUGUI timerDisplay;

    [SerializeField] private TextMeshProUGUI stormStartText;

    [SerializeField, Tooltip("Switch the format of the time to 00 or 00:00")]
    private bool switchTimeFormats;

    [SerializeField] private float timeUntilHideText;
    private string timeFormat = "00";
    
    private void OnEnable()
    {
        CollectMudTimer.OnTimeOver += ShowStormStartedText;
    }

    private void OnDisable()
    {
        CollectMudTimer.OnTimeOver -= ShowStormStartedText;
    }
    private void Start()
    {
        if (switchTimeFormats)
            ChangeTimeFormat();
    }
    private void ChangeTimeFormat()
    {
        timeFormat = "00:00";
    }

    private void Update()
    {
        DisplayPlayEncounterTimer();
    }

    private void DisplayPlayEncounterTimer()
    {
        timerDisplay.text = collectedMudTimer.GetCurrentTime().ToString(timeFormat);
    }

    private void ShowStormStartedText()
    {
        stormStartText.gameObject.SetActive(true);
        timerDisplay.gameObject.SetActive(false);
        StartCoroutine(HideStormStartedText());
    }

    private IEnumerator HideStormStartedText()
    {
        yield return new WaitForSeconds(timeUntilHideText);
        stormStartText.gameObject.SetActive(false);
    }

    
}
