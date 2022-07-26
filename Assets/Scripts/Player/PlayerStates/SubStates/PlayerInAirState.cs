using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 어느 상위 상태에 속하지 않는 독립 하위 상태
public class PlayerInAirState : PlayerState
{
    private int xInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isJumpInputted;
    private bool isJumpInputStopped;
    private bool isJumping;
    private bool isGrabInputted;
    private bool isCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        CheckCoyoteTime();
        
        xInput = player.inputHandler.normalizedInputX;
        isJumpInputted = player.inputHandler.isJumpInputStarted;
        isJumpInputStopped = player.inputHandler.isJumpInputCanceled;
        isGrabInputted = player.inputHandler.isGrabInputStarted;
        
        CheckJumpMultiplier();
        
        // inAir 상태에서 벗어나는 조건들

        // 땅에 닿았다면 land 상태로
        if (isGrounded && player.currentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.landState);
        }
        // 점프 버튼이 눌렸으면서 점프 할 수 있는 상태라면 jump 상태로(공중 점프)
        else if (isJumpInputted && player.jumpState.CanJump())
        {
            player.inputHandler.UsedJumpInput();
            stateMachine.ChangeState(player.jumpState);
        }
        // wall Slide보다 우선되는 wall Grab
        // 벽에 닿았으면서 그랩 버튼을 눌렀을 경우 wall Grab 상태로
        else if (isTouchingWall && isGrabInputted)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        // 벽에 닿았으면서 플레이어가 바라보는 방향과 x축 입력 방향이 같고
        // 캐릭터의 y Velocity가 0 이하라면 wall Slide 상태로
        else if (isTouchingWall && xInput == player.facingDirection && player.currentVelocity.y <= 0f)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        // 아니라면 x Velocity 재설정(공중 이동)
        else
        {
            player.CheckFlip(xInput);
            player.SetVelocityX(playerData.moveVelocity * xInput);
            
            player.animator.SetFloat("yVelocity", player.currentVelocity.y);
            player.animator.SetFloat("xVelocity", Mathf.Abs(player.currentVelocity.x));
        }
    }

    private void CheckJumpMultiplier()
    {
        // 점프 중일 때
        if (isJumping)
        {
            // 점프 키 업 시 y Velocity에 가중치를 곱해서 점프 높이를 낮추고 점프 중이 아니도록 설정
            if (isJumpInputStopped)
            {
                player.SetVelocityY(player.currentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            // 현재 y Velocity가 0보다 작으면 떨어지는 중이므로 점프 중이 아니도록 설정
            else if(player.currentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    public override void TimeUpdate()
    {
        base.TimeUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGrounded = player.CheckGround();
        isTouchingWall = player.CheckWall();
    }

    private void CheckCoyoteTime()
    {
        if (isCoyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            isCoyoteTime = false;
            player.jumpState.DecreaseLeftJumpCount();
        }
    }

    public void StartCoyoteTime()
    {
        isCoyoteTime = true;
    }

    public void SetIsJumping()
    {
        isJumping = true;
    }
}
