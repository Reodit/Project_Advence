using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUtility : MonoBehaviour
{
    private static EffectUtility instance;

    public static EffectUtility Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject fadeUtilityObject = new GameObject("FadeUtility");
                instance = fadeUtilityObject.AddComponent<EffectUtility>();
            }
            return instance;
        }
    }

    public static void FadeOut(IEnumerable<SpriteRenderer> spriteRenderers, float duration)
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null) // null 체크
            {
                Instance.StartCoroutine(Instance.PerformFadeOut(spriteRenderer, duration));
            }
        }
    }

    public static void FadeIn(IEnumerable<SpriteRenderer> spriteRenderers, float duration)
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null) // null 체크
            {
                Instance.StartCoroutine(Instance.PerformFadeIn(spriteRenderer, duration));
            }
        }
    }

    private IEnumerator PerformFadeOut(SpriteRenderer spriteRenderer, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time < endTime)
        {
            float alpha = 1 - ((Time.time - startTime) / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
    }

    private IEnumerator PerformFadeIn(SpriteRenderer spriteRenderer, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time < endTime)
        {
            float alpha = (Time.time - startTime) / duration;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
    }
}
