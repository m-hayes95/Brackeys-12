using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScript : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] PolygonCollider2D hitBox;
    [SerializeField] SpriteRenderer shadowSpriteRenderer;

    [Header("Splash Stats")]
    [SerializeField] float fallTime;
    [SerializeField] float splashTotalTime;
    [SerializeField] float splashCutoffTime;
    [SerializeField] float startFallSize = .1f;
    [SerializeField] float targetFallSize = 1;
    void Start()
    {
        splashTotalTime = splashTotalTime - splashCutoffTime;
    }

    void OnEnable()
    {
        
    }

    public IEnumerator FallRoutine()
    {
        for (int i = 0; i < 1; i++)
        {
            float t = 0;

            switch (i)
            {
                case 0:
                    while (t < 1)
                    {
                        t += Time.deltaTime / fallTime;

                        transform.localScale = Vector3.Lerp(new Vector3 (startFallSize, startFallSize, 1), new Vector3 (targetFallSize, targetFallSize, 1), t);
                        

                        yield return null;
                    }
                break;
                case 1:
                    while (t < 1)
                    {
                        t += Time.deltaTime / splashTotalTime;

                        yield return null;
                    }
                    hitBox.enabled = false;
                    
                    yield return new WaitForSeconds(splashCutoffTime);
                    gameObject.SetActive(false);
                break;
            }
            t = 0;
        }
        yield break;
    }
}
