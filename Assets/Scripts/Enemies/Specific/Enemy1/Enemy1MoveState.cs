using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1MoveState : EntityMoveState
{
    private Enemy1 enemy; // 이 클래스로부터 더이상 상속이 일어나지 않도록 private

    public Enemy1MoveState(Entity entity, EntityStateMachine stateMachine, EntityData entityData, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, entityData, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Move State를 벗어나는 조건들

        // Player가 탐지되었을 경우 PlayerDetected State로
        if(isDetectingPlayerInMinRange)
        {
            stateMachine.ChangeState(enemy.DetectedPlayerState);
        }
        // 벽에 닿았거나 난간에 닿은게 아니라면 Idle State로
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
