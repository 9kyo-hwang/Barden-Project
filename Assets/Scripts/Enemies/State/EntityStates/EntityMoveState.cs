using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMoveState : EntityState
{
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isDetectingPlayerInMinRange;

    public EntityMoveState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {

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
