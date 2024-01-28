using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = System.Numerics.Vector3;

namespace UI
{
    public static class UIAnimations
    {
        public static IEnumerator FadeCoroutine(Graphic uiElement, 
            float startAlpha, float endAlpha, float duration, IEnumerator nextCoroutine = null)
        {
            float elapsedTime = 0f;
            Color initialColor = uiElement.color;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                uiElement.color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);
                yield return null;
            }
            
            uiElement.color = new Color(initialColor.r, initialColor.g, initialColor.b, endAlpha);

            if (nextCoroutine != null)
            {
                yield return nextCoroutine;
            }
        }
    
        // TODO 애니메이션 바운드 추가
        public static IEnumerator ScaleBounceCoroutine(Graphic uiElement, 
            AnimationCurve bounceCurve, float duration, IEnumerator nextCoroutine = null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                float time = elapsedTime / duration;
                float curveValue = bounceCurve.Evaluate(time);
                uiElement.transform.localScale = UnityEngine.Vector3.one * curveValue;
                
                yield return null;
            }
            
            if (nextCoroutine != null)
            {
                yield return nextCoroutine;
            }
        }
    
        public static IEnumerator TextStackAnimation(Graphic text, 
            List<string> stackText, float stackingDelay, IEnumerator nextCoroutine = null)
        {
            if (text is Text)
            {
                Text textComponent = text as Text;
        
                for (int i = 0; i < stackText.Count; i++)
                {
                    textComponent.text += stackText[i] + "\n";
                    yield return new WaitForSeconds(stackingDelay);
                }
            }
        
            else if (text is TextMeshProUGUI)
            {
                TextMeshProUGUI tmpTextComponent = text as TextMeshProUGUI;
        
                for (int i = 0; i < stackText.Count; i++)
                {
                    tmpTextComponent.text += stackText[i] + "\n";
                    yield return new WaitForSeconds(stackingDelay);
                }
            }
        
            else
            {
                throw new System.InvalidOperationException("The provided Graphic is neither a Text nor a TextMeshProUGUI.");
            }
        
            if (nextCoroutine != null)
            {
                yield return nextCoroutine;
            }
        }
    }
}
