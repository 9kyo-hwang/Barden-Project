using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIdleState : EntityState
{
    protected EntityData_IdleState data;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isDetectingPlayerInMinRange;

    protected float idleTime;

    public EntityIdleState(Entity entity, EntityStateMachine stateMachine, string animBoolName, EntityData_IdleState data) : base(entity, stateMachine, animBoolName)
    {
        this.data = data;
    }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocityX(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if(flipAfterIdle)
        {
            entity.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
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
    }

    #region Set Functions

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(data.minIdleTime, data.maxIdleTime);
    }
    #endregion
}
