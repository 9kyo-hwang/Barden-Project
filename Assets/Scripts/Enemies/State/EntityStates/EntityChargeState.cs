using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityChargeState : EntityState
{
    #region Variables
    protected bool isDetectingPlayerInMinRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;
    #endregion
    
    #region Core Components
    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;
    #endregion
    
    public EntityChargeState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();

        if (CollisionSenses)
        {
            isDetectingLedge = CollisionSenses.GetLedgeVer;
            isDetectingWall = CollisionSenses.GetWall;
        }
        isDetectingPlayerInMinRange = entity.GetPlayerInMinRange;
        performCloseRangeAction = entity.GetPlayerInCloseRangeAction;
    }

    public override void Enter()
    {
        base.Enter();

        isChargeTimeOver = false;
        Movement?.SetVelocityX(data.chargeSpeed * Movement.FacingDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Movement?.SetVelocityX(data.chargeSpeed * Movement.FacingDir);

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
