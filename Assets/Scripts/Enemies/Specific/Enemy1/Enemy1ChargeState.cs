using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1ChargeState : EntityChargeState
{
    private Enemy1 enemy;

    public Enemy1ChargeState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
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

        // Charge State를 벗어나는 조건들

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else if(isChargeTimeOver && isDetectingPlayerInMinRange)
        {
            stateMachine.ChangeState(enemy.DetectedPlayerState);
        }
        else if(isChargeTimeOver || !isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
