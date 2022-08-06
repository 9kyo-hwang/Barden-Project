using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStunState : EntityState
{
    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isDetectingPlayerInMinRange;

    public EntityStunState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isMovementStopped = false;
        entity.SetVelocity(data.knockbackSpeed, data.knockbackAngle, entity.LastDamageDir);
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

        // 땅에서 넉백으로 인해 움직이는 중일 때, 넉백 시간을 초과했다면 더이상 속력 변화가 없도록
        if (isGrounded && Time.time >= startTime + data.knockbackTime && !isMovementStopped)
        {
            isMovementStopped = true;
            entity.SetVelocityX(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGrounded = entity.GetGround;
        performCloseRangeAction = entity.GetPlayerInCloseRangeAction;
        isDetectingPlayerInMinRange = entity.GetPlayerInMinRange;
    }
}
