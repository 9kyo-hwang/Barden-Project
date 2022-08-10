using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1ChargeState : EntityChargeState
{
    private Enemy1 enemy;

    public Enemy1ChargeState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Charge State를 벗어나는 조건들

        // 근접 공격 범위에 들어왔다면
        if(isEnteringPlayerInCloseActionRange)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        // 차징 시간이 끝났으면서 최소 플레이어 탐지 범위 내에 플레이어가 있다면
        else if(isChargeTimeOver && isEnteringPlayerInMinDetectionRange)
        {
            stateMachine.ChangeState(enemy.DetectPlayerState);
        }
        // 차징 시간이 끝났거나 난간에 닿지 않았거나 벽에 닿았다면
        else if(isChargeTimeOver || !isTouchingLedge || isTouchingWall)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
