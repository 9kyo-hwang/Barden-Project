using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDodgeState : EntityState
{
    protected bool isEnteringPlayerInCloseActionRange;
    protected bool isEnteringPlayerInMaxDetectionRange;
    protected bool isGrounded;
    protected bool isDodgeOver;

    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;

    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;

    public EntityDodgeState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isEnteringPlayerInCloseActionRange = entity.GetPlayerInCloseActionRange;
        isEnteringPlayerInMaxDetectionRange = entity.GetPlayerInMaxDetectionRange;
        isGrounded = CollisionSenses.GetGround;
    }

    public override void Enter()
    {
        base.Enter();

        // Dodge 상태 진입 시 false로 시작
        isDodgeOver = false;
        
        // Dodge 상태 진입 시 바라보는 방향의 반대로 Velocity 변경
        Movement.SetVelocityDirection(data.dodgeSpeed, data.dodgeAngle, -Movement.FacingDir);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // dodge 지속 시간이 경과했으면서 땅 위에 있다면 회피 끝
        if (Time.time >= StartTime + data.dodgeTime && isGrounded)
        {
            isDodgeOver = true;
        }
    }
}
