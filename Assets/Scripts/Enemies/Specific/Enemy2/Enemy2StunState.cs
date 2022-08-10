using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2StunState : EntityStunState
{
    private Enemy2 enemy;
    public Enemy2StunState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // Stun State를 벗어나는 조건들
        
        // 스턴 시간이 종료되어야만 가능
        if (isStunTimeOver)
        {
            // 플레이어가 최소 탐지 범위에 존재할 때
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
