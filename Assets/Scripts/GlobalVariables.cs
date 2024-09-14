using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance;
    public static float score = 0;
    public static bool gameStarted;
    public static bool gamePaused = true;
    public static bool playerCanPaint;
    public static bool playerCanMove;
    public static float rainSpeedMultiplier = 1;
    public static float playerMuddiness = 0;
    public Rain_Manager rain_Manager;
    public PlayerMudPaintScript playerMudPaintScript;
    public CollectMudTimer collectMudTimer;

    public Transform playerTransform;
    public Animator playerAnimator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StopRoutines()
    {
        collectMudTimer.EndTimer();
        collectMudTimer.currentTime = 0; // Reset Time
        rain_Manager.StopAllCoroutines(); // Stop Rain
    }

    public void StartGame()
    {
        playerCanMove = true;
        playerCanPaint = true;
        gameStarted = true;
        gamePaused = false;
    }

    public void ResetGame()
    {
        // Reset Game Stats
        score = 0;
        gameStarted = false;
        playerCanPaint = false;
        playerCanMove = false;
        rainSpeedMultiplier = 1;
        playerMuddiness = 0;

        rain_Manager.ResetAllSplashScripts(); // Reset Rain Manager
        playerMudPaintScript.ApplyTexture(); // Reset Paint Canvas Texture
        collectMudTimer.EndTimer();

        // Reset Player Position and Rotation
        playerTransform.position = new Vector3(0,0,playerTransform.position.z);
        playerTransform.rotation = new Quaternion();
        playerAnimator.SetFloat("Speed", 0);
        playerAnimator.SetFloat("Muddiness", 0);
        playerAnimator.SetBool("Moving", false);
        //...
    }

    public float CalculatedScore()
    {
        return rain_Manager.TimeElapsed();
    }
}