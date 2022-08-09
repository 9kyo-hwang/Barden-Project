using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1MoveState : EntityMoveState
{
    private Enemy1 enemy; // 이 클래스로부터 더이상 상속이 일어나지 않도록 private

    public Enemy1MoveState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Move State를 벗어나는 조건들

        // 최소 탐지 범위 내에 플레이어가 있을 경우
        if(isDetectingPlayerInMinRange)
        {
            stateMachine.ChangeState(enemy.DetectedPlayerState);
        }
        // 벽에 닿았거나 난간에 닿지 않았다면(땅 끝에 도달함)
        else if(isDetectingWall || !isDetectingLedge)
        {
            enemy.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
