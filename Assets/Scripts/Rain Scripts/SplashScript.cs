using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SplashScript : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] GameObject MainObject;
    [SerializeField] PolygonCollider2D hitBox;
    [SerializeField] Transform hitBoxTransform;
    [SerializeField] SpriteRenderer shadowSpriteRenderer;
    [SerializeField] SpriteRenderer splashSpriteRenderer;
    // [SerializeField] Color startColour;
    // [SerializeField] Color targetColour;
    [SerializeField] Animator animator;

    [Header("Splash Stats")]
    public int objectId;
    [SerializeField] float fallTime;
    [SerializeField] float splashTotalTime;
    [SerializeField] float splashCutoffTime;
    [SerializeField] float startFallSize = .1f;
    [SerializeField] float targetFallSize = 1;
    [SerializeField] float hitBoxStartSize = .6f;
    [SerializeField] float hitBoxEndSize = 1;
    // void Start()
    // {
    //     splashTotalTime = splashTotalTime - splashCutoffTime;
    // }

    public void EnabledObject()
    {
        MainObject.SetActive(true);
        StartCoroutine(FallRoutine());
    }

    public IEnumerator FallRoutine()
    {
        splashSpriteRenderer.enabled = false;
        shadowSpriteRenderer.enabled = true;
        animator.speed = 1 / fallTime;

        float t = 0;

        animator.speed = 1 / fallTime;
        animator.SetTrigger("Shadow");
        animator.Play("Shadow Animation");

        yield return new WaitForSeconds(fallTime);

        animator.SetTrigger("Splash");
        animator.Play("Splash Animation");
        animator.speed = 1 / splashTotalTime;
        shadowSpriteRenderer.enabled = false;
        splashSpriteRenderer.enabled = true;
        hitBox.enabled = true;

        while (t < 1)
        {
            t += Time.deltaTime / splashTotalTime;
            hitBoxTransform.transform.localScale = Vector3.Lerp(new Vector3 (hitBoxStartSize, hitBoxStartSize, 1), new Vector3 (hitBoxEndSize, hitBoxEndSize, 1), t);

            yield return null;
        }
        hitBox.enabled = false;
        Rain_Manager.instance.ReturnSplashID(objectId);
        animator.Play("Idle");
        MainObject.SetActive(false);
        yield break;
    }

    public void SplashHitPlayer()
    {
        Debug.LogError("Player Died");
    }
}
