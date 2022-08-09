using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedStates
{
    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();
        
        player.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (!isExitingState)
        {
            // x Velocity를 crouchMove 값만큼 바라보는 방향으로 지속 갱신
            Movement.SetVelocityX(playerData.crouchMoveVelocity * Movement.FacingDir);
            Movement.CheckFlip(inputX);
            
            // crouch Move 상태를 벗어나는 조건들
            
            // x축 입력이 없다면 crouch Idle 상태로
            if (inputX == 0)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            // y축 아랫 방향 입력이 없으며 천장에 닿지 않았다면 move 상태로
            else if (inputY != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
