using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1IdleState : EntityIdleState
{
    private Enemy1 enemy;
    public Enemy1IdleState(Entity entity, EntityStateMachine stateMachine, string animBoolName, EntityData_IdleState idleData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, idleData)
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

        // Idle State를 벗어나는 조건들

        if(isDetectingPlayerInMinRange)
        {
            stateMachine.ChangeState(enemy.PlayerDetectedState);
        }
        else if(isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
