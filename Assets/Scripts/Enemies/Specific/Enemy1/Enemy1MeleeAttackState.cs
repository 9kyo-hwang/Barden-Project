using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1MeleeAttackState : EntityMeleeAttackState
{
    private Enemy1 enemy;
    
    public Enemy1MeleeAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName, attackPosition)
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // Melee Attack을 벗어나는 조건들
        if (isAnimationFinished && isDetectingPlayerInMinRange)
        {
            stateMachine.ChangeState(enemy.DetectedPlayerState);
        }
        else if (isAnimationFinished && !isDetectingPlayerInMinRange)
        {
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

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AttackTrigger()
    {
        base.AttackTrigger();
    }

    public override void AttackFinish()
    {
        base.AttackFinish();
    }
}
