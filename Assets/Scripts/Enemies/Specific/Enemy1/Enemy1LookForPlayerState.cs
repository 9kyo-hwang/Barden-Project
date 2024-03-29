using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1LookForPlayerState : EntityLookForPlayerState
{
    private Enemy1 enemy;
    public Enemy1LookForPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, data, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Look For Player State를 벗어나는 조건들

        // 플레이어 최소 탐지 범위에 플레이어가 들어왔다면
        if(isEnteringPlayerInMinDetectionRange)
        {
            stateMachine.ChangeState(enemy.DetectPlayerState);
        }
        // 돌아보는 횟수를 모두 소모한 경우
        else if(isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
