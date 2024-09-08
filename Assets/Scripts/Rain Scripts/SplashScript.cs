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
    [SerializeField] Color startColour;
    [SerializeField] Color targetColour;

    [Header("Splash Stats")]
    public int objectId;
    [SerializeField] float fallTime;
    [SerializeField] float splashTotalTime;
    [SerializeField] float splashCutoffTime;
    [SerializeField] float startFallSize = .1f;
    [SerializeField] float targetFallSize = 1;
    [SerializeField] float hitBoxStartSize = .6f;
    [SerializeField] float hitBoxEndSize = 1;
    void Start()
    {
        splashTotalTime = splashTotalTime - splashCutoffTime;
    }

    public void EnabledObject()
    {
        MainObject.SetActive(true);
        StartCoroutine(FallRoutine());
    }

    public IEnumerator FallRoutine()
    {
        splashSpriteRenderer.enabled = false;
        shadowSpriteRenderer.enabled = true;
        for (int i = 0; i < 2; i++)
        {
            float t = 0;

            switch (i)
            {
                case 0:
                    while (t < 1)
                    {
                        t += Time.deltaTime / fallTime;

                        transform.localScale = Vector3.Lerp(new Vector3 (startFallSize, startFallSize, 1), new Vector3 (targetFallSize, targetFallSize, 1), t);
                        shadowSpriteRenderer.color = Color.Lerp(startColour, targetColour, t);
                        yield return null;
                    }
                    shadowSpriteRenderer.enabled = false;
                    splashSpriteRenderer.enabled = true;
                    hitBox.enabled = true;
                break;
                case 1:
                    while (t < 1)
                    {
                        t += Time.deltaTime / splashTotalTime;
                        transform.localScale = Vector3.Lerp(new Vector3 (hitBoxStartSize, hitBoxStartSize, 1), new Vector3 (hitBoxEndSize, hitBoxEndSize, 1), t);

                        yield return null;
                    }
                    hitBox.enabled = false;
                    
                    yield return new WaitForSeconds(splashCutoffTime);
                    // yield return new WaitForEndOfFrame();
                    Rain_Manager.instance.ReturnSplashID(objectId);
                break;
            }
            t = 0;
        }
        MainObject.SetActive(false);
        yield break;
    }

    public void SplashHitPlayer()
    {
        Debug.LogError("Player Died");
    }
}
