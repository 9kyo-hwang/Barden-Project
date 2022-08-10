using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2MeleeAttackState : EntityMeleeAttackState
{
    private Enemy2 enemy;
    public Enemy2MeleeAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName, attackPosition)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // Melee Attack 상태를 벗어나는 조건들
        
        // 공격 애니메이션이 끝났다면
        if (isAnimationFinished)
        {
            // 여전히 플레이어가 최소 탐지 범위 내에 있을 때
            if (isEnteringPlayerInMinDetectionRange)
            {
                stateMachine.ChangeState(enemy.DetectPlayerState);
            }
            else // 아니라면
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }
}
