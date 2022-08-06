using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityChargeState : EntityState
{
    protected bool isDetectingPlayerInMinRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;

    public EntityChargeState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isDetectingPlayerInMinRange = entity.GetPlayerInMinRange;
        isDetectingLedge = entity.GetLedge;
        isDetectingWall = entity.GetWall;
        performCloseRangeAction = entity.GetPlayerInCloseRangeAction;
    }

    public override void Enter()
    {
        base.Enter();

        isChargeTimeOver = false;
        entity.SetVelocityX(data.chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + data.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}