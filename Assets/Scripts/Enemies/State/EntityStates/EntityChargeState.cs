using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityChargeState : EntityState
{
    #region Variables
    protected bool isEnteringPlayerInMinDetectionRange;
    protected bool isTouchingLedge;
    protected bool isTouchingWall;
    protected bool isChargeTimeOver;
    protected bool isEnteringPlayerInCloseActionRange;
    #endregion
    
    #region Core Components
    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;
    #endregion
    
    public EntityChargeState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) 
        : base(entity, stateMachine, data, animBoolName)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        
        isTouchingLedge = CollisionSenses.GetLedgeVer;
        isTouchingWall = CollisionSenses.GetWall;
        isEnteringPlayerInMinDetectionRange = entity.GetPlayerInMinDetectionRange;
        isEnteringPlayerInCloseActionRange = entity.GetPlayerInCloseActionRange;
    }

    public override void Enter()
    {
        base.Enter();

        isChargeTimeOver = false;
        Movement.SetVelocityX(data.chargeSpeed * Movement.FacingDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Movement.SetVelocityX(data.chargeSpeed * Movement.FacingDir);

        if(Time.time >= StartTime + data.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
