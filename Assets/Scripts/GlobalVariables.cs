using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static float score = 0;
    public static bool gameStarted;
    public static bool gamePaused = true;
    public static bool playerCanPaint;
    public static bool playerCanMove;
    public static float rainSpeedMultiplier = 1;
    public static float playerMuddiness = 0;

    public void ResetVariables()
    {
        score = 0;
        gameStarted = false;
        playerCanPaint = false;
        playerCanMove = false;
        rainSpeedMultiplier = 1;
        playerMuddiness = 0;
        //...
    }
}