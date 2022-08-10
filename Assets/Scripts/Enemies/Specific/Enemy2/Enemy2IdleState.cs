using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2IdleState : EntityIdleState
{
    private Enemy2 enemy;
    
    public Enemy2IdleState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
