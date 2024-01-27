using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class DefaultDamageEffect : DamageEffect
    {
        [SerializeField] private TextMeshProUGUI tmp;
        [SerializeField] private RectTransform _rectTransform;
        // 데미지 표기 시간
        public float displayTime;
        public Vector2 damageEffectOffset;
        public bool isDamageEffectRandomOffset;
        public float offsetRange;
        
        [Header("Text-Stacking Effect")]
        public float stackingDelay;
    
        [Header("Fade Effect")]
        public float fadeinTime;
        public float fadeoutTime;
        public float fadeDurationTime;

        [Header("Scale-Bounce Effect")]
        public float scaleBounceTime;
        public AnimationCurve bounceCurve;
    
        private Color _initColor;
        private Vector3 _initLocalScale;

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (!_rectTransform)
            {
                Debug.Log("failure instancing");
            }
            _initColor = tmp.color;
            _initLocalScale = tmp.transform.localScale;
            tmp.text = null;
        }

        // TODO 광역기 case
        public override void RestoreDefaults()
        {
            _rectTransform.transform.position = Vector3.zero;
            tmp.text = null;
            tmp.color = _initColor;
            tmp.transform.localScale = _initLocalScale;
            DamageEffectManager.Instance.ReturnDamageEffect(this.gameObject);
        }

        public override void PositionUpdate(Vector3 monsterTransform)
        {
            Vector2 damageEffectOffsetCopy = isDamageEffectRandomOffset
                ? damageEffectOffset + new Vector2(Random.Range(damageEffectOffset.x, offsetRange),
                    Random.Range(damageEffectOffset.y, offsetRange)) : damageEffectOffset;
            
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(
                (Vector2)monsterTransform + damageEffectOffsetCopy);

            _rectTransform.transform.position = screenPoint;
        }
        
        // 콜백으로 호출
        public override IEnumerator DisplayDamage(List<string> damages)
        {
            float elapsedTime = 0f;

            IEnumerator animation1 = UI.UIAnimations.TextStackAnimation(tmp, damages, stackingDelay);
            IEnumerator animaiton2 = UI.UIAnimations.ScaleBounceCoroutine(tmp,
                bounceCurve, scaleBounceTime, UI.UIAnimations.FadeCoroutine(tmp,
                    fadeoutTime, fadeinTime, fadeDurationTime));
        
            StartCoroutine(animation1);
            StartCoroutine(animaiton2);

            while (displayTime > elapsedTime)
            {
                elapsedTime += Time.deltaTime;
            
                yield return null;
            }

            StopCoroutine(animation1);
            StopCoroutine(animaiton2);
            RestoreDefaults();
            yield return null;
        }

    }
}
