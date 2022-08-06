using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1LookForPlayerState : EntityLookForPlayerState
{
    private Enemy1 enemy;
    public Enemy1LookForPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
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

        // Look For Player State를 벗어나는 조건들

        if(isDetectingPlayerInMinRange)
        {
            stateMachine.ChangeState(enemy.DetectedPlayerState);
        }
        else if(isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
