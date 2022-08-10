using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2RangedAttackState : EntityRangedAttackState
{
    private Enemy2 enemy;
    public Enemy2RangedAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName, attackPosition)
    {
        this.enemy = enemy;
    }
    
    
}
