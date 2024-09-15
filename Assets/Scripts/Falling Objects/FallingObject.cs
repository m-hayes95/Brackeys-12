using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingObject : MonoBehaviour
{
    [SerializeField] float fallTime = 3;
    [SerializeField] float flashRate = 1;
    public float despawnTime;
    [SerializeField] float cameraShakeIntensity;
    [SerializeField] AudioClip impactClip;
    [SerializeField] AudioClip whistle;

    [Header("Sprites")]

    [SerializeField] SpriteRenderer mainSprite;
    [SerializeField] Color fadeStart;
    [SerializeField] Color fadeEnd;
    [SerializeField] SpriteRenderer shadowSprite;
    [SerializeField] Color shadowStartColour;
    [SerializeField] Color shadowEndColour;
    [SerializeField] Transform shadowTransform;
    [SerializeField] Vector2 shadowStartSize = new(.6f, .6f);
    [SerializeField] Vector2 shadowEndSize = new(1,1);
    [SerializeField] SpriteRenderer outlineSprite;
    
    // [SerializeField] Animator animator;
    [Header("Components")]
    [SerializeField] PolygonCollider2D polyHitBox;
    [SerializeField] Transform hitBoxTransform;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Vector2 hitBoxStartScale = new(.2f,.2f);
    [SerializeField] Vector2 hitBoxEndScale = new(1,1);
    [SerializeField] float scaleTime = .2f;
    private Coroutine coroutine;

    public void Fall()
    {
        ApplyRandomRotation();
        coroutine = StartCoroutine(LifeCycle());
    }

    public void Despawn()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);   
        }
        gameObject.SetActive(false);
    }

    IEnumerator LifeCycle()
    {
        float newDespawnTime = Mathf.Max(0, despawnTime - fallTime - scaleTime);
        float t = 0;
        shadowSprite.enabled = true;
        outlineSprite.enabled = true;

        // INSERT SOUND : Whistle
        audioSource.PlayOneShot(whistle);

        while (t < 1)
        {
            t += Time.deltaTime / fallTime;
            shadowSprite.color = Color.Lerp(shadowStartColour, shadowEndColour, t);
            shadowTransform.localScale = Vector2.Lerp(shadowStartSize, shadowEndSize, t);
            yield return null;
        }
        // Object Landed
        mainSprite.color = fadeStart;
        shadowSprite.enabled = false;
        outlineSprite.enabled = false;
        mainSprite.enabled = true;
        CameraShakeManager.instance.ShakeCamera(1, 0.2f);

        // INSERT SOUND : Crash (sound based on fallen object)
        audioSource.PlayOneShot(impactClip);

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / scaleTime;
            hitBoxTransform.localScale = Vector3.Lerp(hitBoxStartScale, hitBoxEndScale, t);
            yield return null;
        }

        yield return new WaitForSeconds(newDespawnTime * .8f); // 80%

        t = 0;
        float ft = newDespawnTime * .2f; // 20%
        while (t < 1) // Fade main sprite
        {
            t += Time.deltaTime / ft;
            mainSprite.color = Color.Lerp(fadeStart, fadeEnd, t);
            yield return null;
        }

        mainSprite.enabled = false;

        gameObject.SetActive(false);

        yield break;
    }
    private void ApplyRandomRotation()
    {
        float randomEulerAngleZ = Random.Range(0, 360);
        transform.Rotate(new Vector3(0f, 0f, randomEulerAngleZ));
    }
}
