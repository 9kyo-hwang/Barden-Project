using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1DeadState : EntityDeadState
{
    private Enemy1 enemy;
    
    public Enemy1DeadState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }
}
