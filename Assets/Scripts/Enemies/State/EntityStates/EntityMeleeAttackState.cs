using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class EntityMeleeAttackState : EntityAttackState
{
    #region Core Components
    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    #endregion

    private readonly Collider2D[] cols; // OverlapCircleNonAlloc을 쓰기 위한 out용 변수
    
    public EntityMeleeAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition) : base(entity, stateMachine, data, animBoolName, attackPosition)
    {
    }

    public override void AttackTrigger()
    {
        base.AttackTrigger();
        
        var size = Physics2D.OverlapCircleNonAlloc(attackPosition.position, data.attackRadius, cols, data.whatIsPlayer);

        foreach (var col in cols)
        {
            // Interface 적용
            var damageable = col.GetComponent<IDamageable>();
            // null 전파 연산자(damageable != null -> damageable.damage() 수행)
            damageable?.Damage(data.attackDamage);
            
            var knockbackable = col.GetComponent<IKnockbackable>();
            knockbackable?.Knockback(data.knockbackStrength, data.knockbackAngle, Movement.FacingDir);
        }
    }
}
