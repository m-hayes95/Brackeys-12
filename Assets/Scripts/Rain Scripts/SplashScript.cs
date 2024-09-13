using System.Collections;
using UnityEngine;

public class SplashScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject MainObject;
    [SerializeField] PolygonCollider2D hitBox;
    [SerializeField] Transform hitBoxTransform;
    [SerializeField] SpriteRenderer shadowSpriteRenderer;
    [SerializeField] SpriteRenderer splashSpriteRenderer;
    [SerializeField] Animator animator;

    [Header("Splash Stats")]
    public int objectId;
    [SerializeField] float fallTime = 2;
    [SerializeField] float splashTotalTime = 1;
    [SerializeField] float splashCutoffTime = .2f;
    [SerializeField] float startFallSize = .1f;
    [SerializeField] float targetFallSize = 1;
    [SerializeField] float hitBoxStartSize = .6f;
    [SerializeField] float hitBoxEndSize = 1;
    [SerializeField] float rainSpeedMultiplier = 1; // Debugging Purposes
    [SerializeField] float newFallTime; // Debugging Purposes
    [SerializeField] float newSplashTime; // Debugging Purposes

    private enum SplashState { Idle, Falling, Splashing }  // Enum to track the state
    [SerializeField] SplashState currentState = SplashState.Idle;    // Current state of the splash object

    void Start()
    {
        splashTotalTime -= splashCutoffTime; // Adjust splash total time
    }

    public void EnabledObject()
    {
        MainObject.SetActive(true); // Enable the main object
        hitBox.enabled = false;
        StartCoroutine(FallRoutine()); // Start the fall routine
    }

    public IEnumerator FallRoutine()
    {
        // splashSpriteRenderer.enabled = false;
        // shadowSpriteRenderer.enabled = true;

        currentState = SplashState.Falling;

        // Set current rainSpeedMultiplier
        rainSpeedMultiplier = GlobalVariables.rainSpeedMultiplier;
        newFallTime = fallTime / rainSpeedMultiplier;
        newSplashTime = splashTotalTime / rainSpeedMultiplier;
        Debug.Log("Fall Speed = " + newFallTime);

        // Set animator speed for fall animation
        animator.SetFloat("Speed", 1 / newFallTime);

        // Trigger the "Shadow" animation
        animator.ResetTrigger("Splash"); // Ensure no previous triggers are active
        animator.SetTrigger("Shadow");

        // Wait for the falling animation to complete
        Debug.Log("Wait for Fall: " + newFallTime);
        yield return new WaitForSeconds(newFallTime);

        // Trigger the splash animation
        animator.ResetTrigger("Shadow");
        animator.SetTrigger("Splash");
        animator.SetFloat("Speed", 1 / newSplashTime);
        Debug.Log("Splash Speed = " + newSplashTime);

        // Hide shadow, show splash
        // shadowSpriteRenderer.enabled = false;
        // splashSpriteRenderer.enabled = true;

        currentState = SplashState.Splashing;

        // Enable the hitbox for the splash
        hitBox.enabled = true;

        // Hitbox scaling overtime
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / newSplashTime;
            hitBoxTransform.localScale = Vector3.Lerp(new Vector3(hitBoxStartSize, hitBoxStartSize, 1), new Vector3(hitBoxEndSize, hitBoxEndSize, 1), t);
            yield return null;
        }

        hitBox.enabled = false;

        // Return the object back to the Rain Manager
        Rain_Manager.instance.ReturnSplashID(objectId);

        animator.Play("Idle");

        // Set the state back to Idle
        currentState = SplashState.Idle;
        MainObject.SetActive(false);
    }

    public void SplashHitPlayer()
    {
        Debug.LogError("Player Died");
    }

    void OnDrawGizmos() // Gizmos Debugging
    {
        if (!MainObject || !MainObject.activeInHierarchy)
            return;

        Gizmos.color = currentState == SplashState.Splashing ? Color.red : Color.white; // Red if splashing, white if falling

        Gizmos.DrawWireSphere(hitBoxTransform.position, hitBoxTransform.localScale.x);
    }

    public void ResetSplash()
    {
        currentState = SplashState.Idle;
        rainSpeedMultiplier = 1;
        newFallTime = fallTime;
        newSplashTime = splashTotalTime;
        animator.ResetTrigger("Shadow");
        animator.ResetTrigger("Splash");
        animator.Play("Idle");
        hitBox.enabled = false;
        Rain_Manager.instance.ReturnSplashID(objectId);
        MainObject.SetActive(false);
    }
}
