using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : EntityState
{
    protected EntityData_PlayerDetectedState data;

    protected bool isDetectingPlayerInMinRange;
    protected bool isDetectingPlayerInMaxRange;

    public PlayerDetectedState(Entity entity, EntityStateMachine stateMachine, string animBoolName, EntityData_PlayerDetectedState data) : base(entity, stateMachine, animBoolName)
    {
        this.data = data;
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isDetectingPlayerInMinRange = entity.GetPlayerInMinRange;
        isDetectingPlayerInMaxRange = entity.GetPlayerInMaxRange;
    }
}
