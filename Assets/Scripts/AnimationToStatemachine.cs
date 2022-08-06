using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStatemachine : MonoBehaviour
{
    public EntityAttackState attackState;

    private void AnimationTrigger()
    {
        attackState.AnimationTrigger();
    }

    private void AnimationFinishTrigger()
    {
        attackState.AnimationFinishTrigger();
    }

}
