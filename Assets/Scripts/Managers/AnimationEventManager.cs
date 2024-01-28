using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class AnimationEventManager : MonoBehaviour
    {
        public static AnimationEventManager Instance { get; private set; }

        private AnimationEventManager() { }

        public class CustomAnimationEvent
        {
            public Animator Animator { get; }
            
            // 유니티에서 제공하는 AnimationEvent를 쓰는게 좋은지
            // Custom된 AnimationEvent를 쓰는게 좋은지... 확인 필요
            public Dictionary<string, AnimationTriggerEvent> AnimationTriggerEvents { get; }

            public CustomAnimationEvent(Animator animator)
            {
                Animator = animator;
                AnimationTriggerEvents = new Dictionary<string, AnimationTriggerEvent>();
            }
        }

        public class AnimationTriggerEvent
        {
            public string AnimationClipName;
            public float ProgressTime;
            public bool IsAnimationPlayed;
            public CustomAnimationEvent AnimationEvent;

            public AnimationTriggerEvent(string animationClipName, float progressTime, CustomAnimationEvent animationEvent)
            {
                AnimationClipName = animationClipName;
                ProgressTime = progressTime;
                IsAnimationPlayed = false;
                AnimationEvent = animationEvent;
            }
        }
        
        private Dictionary<Animator, CustomAnimationEvent> _customAnimationEvents;

        private void Awake()
        {
            Instance = this;
            _customAnimationEvents = new Dictionary<Animator, CustomAnimationEvent>();
        }

        public void RegisterAnimator(Animator animator)
        {
            CustomAnimationEvent animationEvent = new CustomAnimationEvent(animator);
            _customAnimationEvents.Add(animator, animationEvent);
        }

        public void RegisterAnimationEvent(Animator animator, 
            string animationClipName, float progressTime)
        {
            if (_customAnimationEvents.TryGetValue(animator, out var customAnimationEvent))
            {
                AnimationTriggerEvent animationEvent = new AnimationTriggerEvent(
                    animationClipName, progressTime, customAnimationEvent);
                customAnimationEvent.AnimationTriggerEvents.Add(animationClipName, animationEvent);
            }

            else
            {
                throw new Exception();
            }
        }

        private void Update()
        {
            foreach (var e in _customAnimationEvents.Values
                         .SelectMany(t => t.AnimationTriggerEvents.Values))
            {
                CheckAnimationState(e.AnimationEvent.Animator, e);
            }
        }


        private void CheckAnimationState(Animator animator, AnimationTriggerEvent animationTriggerEvent)
        {
            // 레이어가 없다고 가정
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName(animationTriggerEvent.AnimationClipName) &&
                stateInfo.normalizedTime >= animationTriggerEvent.ProgressTime)
            {
                animationTriggerEvent.IsAnimationPlayed = true;
            }

            if (stateInfo.normalizedTime < animationTriggerEvent.ProgressTime)
            {
                animationTriggerEvent.IsAnimationPlayed = false;
            }
        }
    }
}
