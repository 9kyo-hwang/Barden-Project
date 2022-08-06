using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDeadState : EntityState
{
    public EntityDeadState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        GameObject.Instantiate(data.deathBloodParticle, entity.transform.position,
            data.deathBloodParticle.transform.rotation);
        GameObject.Instantiate(data.deathChunkParticle, entity.transform.position,
            data.deathChunkParticle.transform.rotation);
        
        entity.gameObject.SetActive(false);
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
}
