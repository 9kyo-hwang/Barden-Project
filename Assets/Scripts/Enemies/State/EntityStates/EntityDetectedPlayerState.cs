using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDetectedPlayerState : EntityState
{
    #region Variables
    protected bool isDetectingPlayerInMinRange;
    protected bool isDetectingPlayerInMaxRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;
    #endregion
    
    #region Core Components
    protected Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;
    #endregion
    
    public EntityDetectedPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        performLongRangeAction = false;
        Movement?.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Movement?.SetVelocityX(0f);

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

        if(CollisionSenses)
            isDetectingLedge = CollisionSenses.GetLedgeVer;
        
        isDetectingPlayerInMinRange = entity.GetPlayerInMinRange;
        isDetectingPlayerInMaxRange = entity.GetPlayerInMaxRange;
        performCloseRangeAction = entity.GetPlayerInCloseRangeAction;
    }
}
