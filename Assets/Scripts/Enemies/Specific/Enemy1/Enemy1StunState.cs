using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Enemy1StunState : EntityStunState
{
    private Enemy1 enemy;
    
    public Enemy1StunState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        
        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Stun State를 벗어나는 조건들
        if (isStunTimeOver && performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else if (isStunTimeOver && isDetectingPlayerInMinRange)
        {
            stateMachine.ChangeState(enemy.ChargeState);
        }
        else if (isStunTimeOver)
        {
            enemy.LookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
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
