using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1ChargeState : EntityChargeState
{
    private Enemy1 enemy;

    public Enemy1ChargeState(Entity entity, EntityStateMachine stateMachine, EntityData entityData, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, entityData, animBoolName)
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

        if(!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        else if(isChargeTimeOver && isDetectingPlayerInMinRange)
        {
            stateMachine.ChangeState(enemy.DetectedPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
