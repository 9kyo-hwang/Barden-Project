using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DeadState : EntityDeadState
{
    private Enemy2 enemy;
    
    public Enemy2DeadState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }
    
    
}
