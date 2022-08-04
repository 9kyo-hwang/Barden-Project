using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1PlayerDetectedState : PlayerDetectedState
{
    private Enemy1 enemy;

    public Enemy1PlayerDetectedState(Entity entity, EntityStateMachine stateMachine, string animBoolName, EntityData_PlayerDetectedState playerDetectedData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, playerDetectedData)
    {
        this.enemy = enemy;
    }

    public override void DoCheck()
    {
        base.DoCheck();
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

        if(!isDetectingPlayerInMaxRange)
        {
            enemy.IdleState.SetFlipAfterIdle(false);
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
