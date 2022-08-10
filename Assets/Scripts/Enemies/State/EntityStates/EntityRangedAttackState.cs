using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRangedAttackState : EntityAttackState
{
    public EntityRangedAttackState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Transform attackPosition) 
        : base(entity, stateMachine, data, animBoolName, attackPosition)
    {
    }
    
    
}
