using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMoveState : EntityState
{
    protected EntityData_MoveState data;
    
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isDetectingPlayerInMinRange;

    public EntityMoveState(Entity entity, EntityStateMachine stateMachine, string animBoolName, EntityData_MoveState data) : base(entity, stateMachine, animBoolName)
    {
        this.data = data;
    }

    public override void Enter()
    {
        base.Enter();
        
        entity.SetVelocityX(data.movementSpeed);
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

        isDetectingLedge = entity.GetLedge;
        isDetectingWall = entity.GetWall;
        isDetectingPlayerInMinRange = entity.GetPlayerInMinRange;
    }
}
