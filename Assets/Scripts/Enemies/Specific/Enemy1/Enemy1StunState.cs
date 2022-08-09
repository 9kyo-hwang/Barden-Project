using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Enemy1StunState : EntityStunState
{
    private Enemy1 enemy;
    
    public Enemy1StunState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        
        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Stun State를 벗어나는 조건들
        
        // 스턴 시간이 종료되어야만 가능
        if (isStunTimeOver)
        {
            // 플레이어가 근접한 공격 범위에 있다면
            if (isEnteringPlayerInCloseActionRange)
            {
                stateMachine.ChangeState(enemy.MeleeAttackState);
            }
            // 플레이어가 최소 탐지 범위에 있다면
            else if (isEnteringPlayerInMinDetectionRange)
            {
                stateMachine.ChangeState(enemy.ChargeState);
            }
            else // 아니라면
            {
                enemy.LookForPlayerState.SetTurnImmediately(true);
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }
}
