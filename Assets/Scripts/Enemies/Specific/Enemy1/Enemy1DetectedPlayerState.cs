using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1DetectedPlayerState : EntityDetectedPlayerState
{
    private Enemy1 enemy;

    public Enemy1DetectedPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
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

        // Detected Player State를 벗어나는 조건들

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.ChargeState);
        }
        else if(!isDetectingPlayerInMaxRange)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        else if (!isDetectingLedge)
        {
            entity.Flip();
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
