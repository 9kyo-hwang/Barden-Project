using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDetectPlayerState : EntityState
{
    #region Variables
    protected bool isEnteringPlayerInMinDetectionRange;
    protected bool isEnteringPlayerInMaxDetectionRange;
    protected bool isEnteringPlayerInLongActionRange;
    protected bool isEnteringPlayerInCloseActionRange;
    protected bool isTouchingLedge;
    #endregion
    
    #region Core Components
    protected Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;
    #endregion
    
    public EntityDetectPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        isEnteringPlayerInLongActionRange = false;
        Movement.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Movement.SetVelocityX(0f);

        if(Time.time >= StartTime + data.longRangeActionTime)
        {
            isEnteringPlayerInLongActionRange = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();
        
        isTouchingLedge = CollisionSenses.GetLedgeVer;
        isEnteringPlayerInMinDetectionRange = entity.GetPlayerInMinDetectionRange;
        isEnteringPlayerInMaxDetectionRange = entity.GetPlayerInMaxDetectionRange;
        isEnteringPlayerInCloseActionRange = entity.GetPlayerInCloseActionRange;
    }
}
