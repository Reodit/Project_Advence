using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
public class SPUM_Prefabs : MonoBehaviour
{
    public float _version;
    public SPUM_SpriteList _spriteOBj;
    public bool EditChk;
    public string _code;
    public Animator _anim;
    public bool _horse;
    public bool isRideHorse{
        get => _horse;
        set {
            _horse = value;
            UnitTypeChanged?.Invoke();
        }
    }
    public string _horseString;

    public UnityEvent UnitTypeChanged = new UnityEvent();
    private AnimationClip[] _animationClips;
    public AnimationClip[] AnimationClips => _animationClips;
    private Dictionary<string, int> _nameToHashPair = new Dictionary<string, int>();

   
    public List<string> animationStatesNames;
    public string selectedAnimationName;
    private void InitAnimPair(){
        _nameToHashPair.Clear();
        _animationClips = _anim.runtimeAnimatorController.animationClips;
        foreach (var clip in _animationClips)
        {
            
            int hash = Animator.StringToHash(clip.name);
            _nameToHashPair.Add(clip.name, hash);
        }
    }
    private void Awake() {
        InitAnimPair();
    }
    private void Start() {
        UnitTypeChanged.AddListener(InitAnimPair);
        animationStatesNames = new List<string>() { "RunState", "AttackState", "MagicAttack", "RangedAttack", "Idle" };
        transform.GetChild(0).gameObject.AddComponent<AnimationFunction>();
    }
    
    [ContextMenu("TestScript")]
    public void PlayTestAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        
        StartCoroutine(PlayAnimationLoop(selectedAnimationName));
    }

    private Coroutine animationCoroutine;
    IEnumerator PlayAnimationLoop(string animationStateName)
    {
        // 무한 루프
        while (true)
        {
            _anim.Play(animationStateName);

            // 애니메이션의 길이를 기다린다. 여기서 transitionTime은 애니메이션 사이의 전환 시간을 고려합니다.
            yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length + 0.01f);
        }
    }
    public void PlayAnimation(string name){

        Debug.Log(name);
        
        foreach (var animationName in _nameToHashPair)
        {
            if(animationName.Key.ToLower().Contains(name.ToLower()) ){
                _anim.Play(animationName.Value, 0);
                break;
            }
        }
    }
}
