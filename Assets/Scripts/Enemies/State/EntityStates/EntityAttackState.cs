using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackState : EntityState
{
    protected Transform attackPosition; // 공격점

    protected bool isAnimationFinished;
    protected bool isDetectingPlayerInMinRange;
    public EntityAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition) : base(entity, stateMachine, data, animBoolName)
    {
        this.attackPosition = attackPosition;
    }

    public override void Enter()
    {
        base.Enter();
        
        isAnimationFinished = false;
        entity.AnimToStateMachine.attackState = this;
        entity.SetVelocityX(0f);
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

        isDetectingPlayerInMinRange = entity.GetPlayerInMinRange;
    }

    public virtual void AnimationTrigger()
    {
        AttackTrigger();
    }

    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
        
        AttackFinish();
    }

    public virtual void AttackTrigger()
    {
        
    }

    public virtual void AttackFinish()
    {
        
    }
}
