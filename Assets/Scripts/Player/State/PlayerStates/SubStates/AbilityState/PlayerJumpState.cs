using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int leftJumpCount;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        leftJumpCount = playerData.maxJumpCount;
    }
    
    // 진입 시 yVelocity 변경, isJumping
    // 점프 동작 즉시 ability가 종료되며 남은 점프 횟수 1 감소
    public override void Enter()
    {
        base.Enter();
        
        // 다른 상태에 흩어져있던 isJumpInput = false 기능을 jump 상태 진입 시 동작하도록 일원화
        player.InputHandler.UsedJumpInput();
        Movement?.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        leftJumpCount--;
        player.InAirState.SetIsJumping();
    }

    // 남은 점프 횟수를 판단해 점프를 할 수 있는 지 판단하는 함수
    public bool CanJump()
    {
        return leftJumpCount > 0;
    }

    // 점프 가능한 횟수를 초기화 시키는 함수
    public void ResetJumpCount()
    {
        leftJumpCount = playerData.maxJumpCount;
    }

    // 남은 점프 횟수를 감소시키는 함수
    public void DecreaseLeftJumpCount()
    {
        leftJumpCount--;
    }
}
