using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStunState : EntityState
{
    #region Variables
    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isStopMovement;
    protected bool isEnteringPlayerInCloseActionRange;
    protected bool isEnteringPlayerInMinDetectionRange;
    #endregion
    
    #region Core Components
    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;
    #endregion

    public EntityStunState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isStopMovement = false;
        Movement.SetVelocityDirection(data.stunKnockbackSpeed, data.knockbackAngle, entity.LastDamageDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + data.stunTime)
        {
            isStunTimeOver = true;
        }

        // 땅에서 넉백 중일 때, 넉백 시간을 초과했다면 더이상 속력 변화가 없도록
        if (isGrounded && Time.time >= startTime + data.stunKnockbackTime && !isStopMovement)
        {
            isStopMovement = true;
            Movement.SetVelocityX(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();
        
        isGrounded = CollisionSenses.GetGround;
        isEnteringPlayerInCloseActionRange = entity.GetPlayerInCloseActionRange;
        isEnteringPlayerInMinDetectionRange = entity.GetPlayerInMinDetectionRange;
    }
}
