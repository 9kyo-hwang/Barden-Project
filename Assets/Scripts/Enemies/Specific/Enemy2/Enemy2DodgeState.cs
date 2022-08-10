using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DodgeState : EntityDodgeState
{
    private Enemy2 enemy;
    public Enemy2DodgeState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // Dodge State를 벗어나는 조건들

        // dodge가 끝나야만 수행 가능
        if (isDodgeOver)
        {
            // 플레이어가 최대 탐색 범위 안에 존재할 때
            if (isEnteringPlayerInMaxDetectionRange)
            {
                // 근접한 공격 범위 내에 존재한다면
                if (isEnteringPlayerInCloseActionRange)
                {
                    stateMachine.ChangeState(enemy.MeleeAttackState);
                }
            }
            else // 최대 탐지 범위 밖에 있다면
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }
}
