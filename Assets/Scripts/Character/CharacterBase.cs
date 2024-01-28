using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    // 메인이 되는 캐릭터당 1개의 SpriteRenderer, Animator
    // TODO 몬스터까지 만든 다음에 다시 고민..
    
    /*protected SpriteRenderer SpriteRenderer;
    protected Animator Animator;
    protected Transform _transform;*/

    public abstract void Init();
}
