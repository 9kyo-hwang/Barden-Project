using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1DetectedPlayerState : EntityDetectedPlayerState
{
    private Enemy1 enemy;

    public Enemy1DetectedPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData entityData, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, entityData, animBoolName)
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

        if(performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.ChargeState);
        }
        else if(!isDetectingPlayerInMaxRange)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
