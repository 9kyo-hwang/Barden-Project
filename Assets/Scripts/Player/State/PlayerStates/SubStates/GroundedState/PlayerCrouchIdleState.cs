using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedStates
{
    public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // 상태 진입 시 velocity 0으로 설정
        // move 상태에서 진입할 때 더이상 움직이지 않게 하기 위해
        Movement.SetVelocityZero();
        // 상태 진입 시 콜라이더 높이 재설정
        player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        
        // 상태 탈출 시 콜라이더 높이 재설정
        player.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (!isExitingState)
        {
            // crouch Idle 상태를 벗어나는 조건들
            
            // x축 입력이 있다면 crouch Move 상태로
            if (inputX != 0)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            // y축 아랫 방향 입력이 없으며 천장에 닿지 않았다면 idle 상태로
            else if (inputY != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
