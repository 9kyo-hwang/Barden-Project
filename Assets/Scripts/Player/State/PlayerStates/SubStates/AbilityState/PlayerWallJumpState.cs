using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // 점프 횟수 초기화, 방향 및 속력 설정, 벽점프 방향 받아서 Flip, 남은 점프 횟수 -1
        player.InputHandler.UsedJumpInput();
        player.JumpState.ResetJumpCount();
        Movement.SetVelocityDirection(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        Movement.CheckFlip(wallJumpDirection);
        player.JumpState.DecreaseLeftJumpCount();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // 벽점프 진행 중 애니메이션 x, y 속력에 따라 조정
        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

        // 벽 점프 유지 시간 초과 시 ability Done
        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    // 벽점프 방향 결정 함수
    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        // 벽에 닿았으면 플레이어가 바라보는 방향의 반대, 아니라면 정방향
        wallJumpDirection = isTouchingWall ? -Movement.FacingDir : Movement.FacingDir;
    }
}
