using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2MoveState : EntityMoveState
{
    private Enemy2 enemy;
    
    public Enemy2MoveState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // Move State를 벗어나는 조건들

        // 최소 탐지 범위 내에 플레이어가 있을 경우
        if(isEnteringPlayerInMinDetectionRange)
        {
            stateMachine.ChangeState(enemy.DetectPlayerState);
        }
        // 벽에 닿았거나 난간에 닿지 않았다면(땅 끝에 도달함)
        else if (isTouchingWall || !isTouchingLedge)
        {
            enemy.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
