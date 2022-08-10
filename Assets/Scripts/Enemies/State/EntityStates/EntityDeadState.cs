using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDeadState : EntityState
{
    public EntityDeadState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) 
        : base(entity, stateMachine, data, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        /*
        var position = entity.transform.position;
        Object.Instantiate(data.deathBloodParticle, position,
            data.deathBloodParticle.transform.rotation);
        Object.Instantiate(data.deathChunkParticle, position,
            data.deathChunkParticle.transform.rotation);
        
        entity.gameObject.SetActive(false);
        */
    }
}
