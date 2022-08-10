using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1DetectPlayerState : EntityDetectPlayerState
{
    private Enemy1 enemy;

    public Enemy1DetectPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Detect Player State를 벗어나는 조건들

        // 근접한 공격 범위에 플레이어가 들어왔다면
        if (isEnteringPlayerInCloseActionRange)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        // 떨어진 공격 범위에 플레이어가 들어왔다면
        else if(isEnteringPlayerInLongActionRange)
        {
            stateMachine.ChangeState(enemy.ChargeState);
        }
        // 플레이어 최대 탐지 범위 밖으로 나가지 않았다면
        else if(!isEnteringPlayerInMaxDetectionRange)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        // 난간에 닿지 않았다면(발 끝에 달려있는 난간 탐지용 ray에 정보가 없다면 난간에 도달했다는 의미)
        else if (!isTouchingLedge)
        {
            Movement.Flip();
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
