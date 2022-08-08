using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class EntityMeleeAttackState : EntityAttackState
{
    public EntityMeleeAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition) : base(entity, stateMachine, data, animBoolName, attackPosition)
    {
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

        var detectedObjs =
            Physics2D.OverlapCircleAll(attackPosition.position, data.attackRadius, data.whatIsPlayer);

        foreach (var col in detectedObjs)
        {
            // Interface 적용
            var damageable = col.GetComponent<IDamageable>();
            // null 전파 연산자(damageable != null -> damageable.damage() 수행)
            damageable?.Damage(data.attackDamage);
            
            var knockbackable = col.GetComponent<IKnockbackable>();
            knockbackable?.Knockback(data.knockbackStrength, data.knockbackAngle, core.Movement.FacingDir);
        }
    }

    public override void AttackFinish()
    {
        base.AttackFinish();
    }
}
