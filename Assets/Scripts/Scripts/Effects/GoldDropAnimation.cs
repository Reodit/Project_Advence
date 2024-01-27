using System.Collections;
//using Character.Monster;
using UnityEngine;
using UnityEngine.Serialization;

namespace Effects
{
    // TODO Pooling 필요
    public class GoldDropAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Vector2 goldOffset;
    
        private Vector2 _startPos;
        private float _elapsedTime;

        [SerializeField] private AnimationCurve animationCurve;
        public float duration;
        public float ratio;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }
    
        /*public void StartUpdate(Monster monster)
        {
            _startPos = goldOffset + (Vector2)monster.transform.position;
            StartCoroutine(GoldPosUpdate());
        }*/
    
        public IEnumerator GoldPosUpdate()
        {
            while (_elapsedTime <= duration)
            {
                float normalizedTime = _elapsedTime / duration;

                float newX = animationCurve.keys[animationCurve.length - 1].time * normalizedTime;
                float newY = animationCurve.Evaluate(newX);
            
                transform.position = (new Vector2(newX, newY) + _startPos) * ratio;
                _elapsedTime += Time.deltaTime;
            
                yield return null;
            }

            Destroy(this.gameObject);
            yield return null;
        }
    }
}
