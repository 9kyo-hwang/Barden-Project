using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData_IdleState : EntityState
{
    protected EntityData_IdleState idleData;
    
    public EntityData_IdleState(Entity entity, EntityStateMachine stateMachine, string animBoolName, EntityData_IdleState idleData) : base(entity, stateMachine, animBoolName)
    {
        this.idleData = idleData;
    }

    public override void Enter()
    {
        base.Enter();
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
