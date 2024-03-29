using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2IdleState : EntityIdleState
{
    private Enemy2 enemy;
    
    public Enemy2IdleState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // Idle State를 벗어나는 조건들
        
        // 플레이어 최소 탐지 범위에 플레이어가 있다면
        if(isEnteringPlayerInMinDetectionRange)
        {
            stateMachine.ChangeState(enemy.DetectPlayerState);
        }
        // 아이들 시간이 끝났다면
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }
}
