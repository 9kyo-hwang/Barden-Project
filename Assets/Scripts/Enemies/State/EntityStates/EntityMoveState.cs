using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMoveState : EntityState
{
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isDetectingPlayerInMinRange;
    
    #region Core Components
    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;
    #endregion

    public EntityMoveState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        
        Movement?.SetVelocityX(data.movementSpeed * Movement.FacingDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Movement?.SetVelocityX(data.movementSpeed * Movement.FacingDir);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
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
    }
}
