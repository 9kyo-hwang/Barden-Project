using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMoveState : EntityState
{
    protected EntityData_MoveState moveData;
    
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    
    public EntityMoveState(Entity entity, EntityStateMachine stateMachine, string animBoolName, EntityData_MoveState moveData) : base(entity, stateMachine, animBoolName)
    {
        this.moveData = moveData;
    }

    public override void Enter()
    {
        base.Enter();
        
        entity.SetVelocityX(moveData.movementSpeed);

        isDetectingLedge = entity.GetLedge;
        isDetectingWall = entity.GetWall;
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
    }
}
