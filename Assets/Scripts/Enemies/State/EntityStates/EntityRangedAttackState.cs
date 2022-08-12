using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRangedAttackState : EntityAttackState
{
    protected GameObject projectileObject;
    protected Projectile projectileScript;
    
    public EntityRangedAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition) 
        : base(entity, stateMachine, data, animBoolName, attackPosition)
    {
    }

    public override void AttackTrigger()
    {
        base.AttackTrigger();

        projectileObject = Object.Instantiate(data.projectile, attackPosition.position, attackPosition.rotation);
        projectileScript = projectileObject.GetComponent<Projectile>();
        projectileScript.FireProjectile(data.projectileSpeed, data.projectileReach, data.projectileDamage);
    }
}
