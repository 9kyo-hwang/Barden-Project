using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2RangedAttackState : EntityRangedAttackState
{
    private Enemy2 enemy;
    
    public Enemy2RangedAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName, attackPosition)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Ranged Attack State를 벗어나기 위한 조건들
        
        // 애니메이션이 끝나야만 가능
        if (isAnimationFinished)
        {
            
            if (isEnteringPlayerInMinDetectionRange)
            {
                stateMachine.ChangeState(enemy.DetectPlayerState);
            }
            else
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }
}
