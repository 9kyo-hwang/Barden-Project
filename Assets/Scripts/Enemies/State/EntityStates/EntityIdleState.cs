using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIdleState : EntityState
{
    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isEnteringPlayerInMinDetectionRange;

    protected float idleTime;

    #region Core Components
    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    #endregion
    
    public EntityIdleState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityX(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if(flipAfterIdle)
        {
            Movement.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Movement.SetVelocityX(0f);

        if(Time.time >= StartTime + idleTime)
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

        isEnteringPlayerInMinDetectionRange = entity.GetPlayerInMinDetectionRange;
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
