using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DetectPlayerState : EntityDetectPlayerState
{
    private Enemy2 enemy;
    public Enemy2DetectPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy2 enemy) 
        : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // Detect Player 상태를 벗어나는 조건들
        
        // 근접한 공격 범위에 플레이어가 들어왔을 때
        if (isEnteringPlayerInCloseActionRange)
        {
            // 회피 쿨타임이 경과했다면
            if (Time.time >= enemy.DodgeState.StartTime + data.dodgeCooldown)
            {
                stateMachine.ChangeState(enemy.DodgeState);
            }
            else // 아니라면
            {
                stateMachine.ChangeState(enemy.MeleeAttackState);
            }
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
}
