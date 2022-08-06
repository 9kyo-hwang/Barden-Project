using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMeleeAttackState : EntityAttackState
{
    protected EntityAttackDetails entityAttackDetails;
    
    public EntityMeleeAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition) : base(entity, stateMachine, data, animBoolName, attackPosition)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 근접 공격 상태 진입 시 데미지와 공격 위치 설정
        entityAttackDetails.damage = data.attackDamage;
        entityAttackDetails.position = entity.transform.position;
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

        Collider2D[] detectedObjs =
            Physics2D.OverlapCircleAll(attackPosition.position, data.attackRadius, data.whatIsPlayer);

        foreach (Collider2D col in detectedObjs)
        {
            col.transform.SendMessage("Damage", entityAttackDetails);
        }
    }

    public override void AttackFinish()
    {
        base.AttackFinish();
    }
}
