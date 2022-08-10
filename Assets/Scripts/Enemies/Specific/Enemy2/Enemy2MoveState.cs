using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2MoveState : EntityMoveState
{
    private Enemy2 enemy;
    
    public Enemy2MoveState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // Move State를 벗어나는 조건들

        if (isDetectingWall)
        {
            
        }
        else if (isDetectingWall || isDetectingLedge)
        {
            
        }
    }
}
