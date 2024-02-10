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
                GameObject fadeUtilityObject = new GameObject("EffectUtility");
                instance = fadeUtilityObject.AddComponent<EffectUtility>();
            }
            return instance;
        }
    }
    
    private class SpriteRendererState
    {
        public Color OriginalColor { get; set; }
        public bool IsFlashing { get; set; }
    }

    private Dictionary<SpriteRenderer, SpriteRendererState> spriteRendererStates = new Dictionary<SpriteRenderer, SpriteRendererState>();

    public void FlashHitColor(List<SpriteRenderer> spriteRenderers, Color hitColor, float hitDuration)
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer == null) continue;

            // 해당 SpriteRenderer의 상태가 없으면 새로 생성
            if (!spriteRendererStates.TryGetValue(spriteRenderer, out var state))
            {
                state = new SpriteRendererState { OriginalColor = spriteRenderer.color };
                spriteRendererStates[spriteRenderer] = state;
            }

            // 이미 플래시 중이면 원래 색상으로 복원 후 다시 플래시 시작
            if (state.IsFlashing)
            {
                spriteRenderer.color = state.OriginalColor;
            }

            StartCoroutine(FlashHitColorCo(spriteRenderer, state, hitColor, hitDuration));
        }
    }

    private IEnumerator FlashHitColorCo(SpriteRenderer spriteRenderer, SpriteRendererState state, Color hitColor, float hitDuration)
    {
        state.IsFlashing = true;
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(hitDuration);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = state.OriginalColor;
        }

        state.IsFlashing = false;
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
