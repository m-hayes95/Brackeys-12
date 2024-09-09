using System;
using UnityEngine;
using TMPro;

public class PlayerHudUpdateText : MonoBehaviour
{
    [SerializeField, Tooltip("Add reference to Collect Mud Timer, under Timers, to this field")] 
    private CollectMudTimer collectedMudTimer;
    [SerializeField, Tooltip("Add reference for the UI timer text to this field")]
    private TextMeshProUGUI timerDisplay;

    [SerializeField, Tooltip("Switch the format of the time to 00 or 00:00")]
    private bool switchTimeFormats;
    private string timeFormat = "00";

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

    
}
