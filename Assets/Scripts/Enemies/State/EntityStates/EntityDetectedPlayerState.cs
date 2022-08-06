using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDetectedPlayerState : EntityState
{

    protected bool isDetectingPlayerInMinRange;
    protected bool isDetectingPlayerInMaxRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;

    public EntityDetectedPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        performLongRangeAction = false;
        entity.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + data.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
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
        performCloseRangeAction = entity.GetPlayerInCloseRangeAction;
    }
}
