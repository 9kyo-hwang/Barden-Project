using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState 상속
public class PlayerGroundedStates : PlayerState
{
    protected int xInput;
    private bool isJumpInputted;
    private bool isGrabInputted;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    
    public PlayerGroundedStates(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        // grounded 상태에 진입했다면 남은 점프 횟수 초기화
        player.jumpState.ResetJumpCount();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        xInput = player.inputHandler.normalizedInputX;
        isJumpInputted = player.inputHandler.isJumpInputStarted;
        isGrabInputted = player.inputHandler.isGrabInputStarted;
        
        // grounded 상태에서 벗어나는 조건들

        // 어떤 지상 상태에서든 점프 키 입력 시 점프 상태로 바뀔 수 있음
        // 단 남은 점프 횟수가 0보다 클 경우(CanJump() return 조건)
        if (isJumpInputted && player.jumpState.CanJump())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        // grounded 상태에서, 지면에서 벗어나게 될 경우
        // coyote time 적용 후 inAir 상태로 변경
        else if (!isGrounded)
        {
            player.inAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.inAirState);
        }
        // 벽과 난간에 닿은 상태로 grab 키를 눌렀을 경우 wall Grab 상태로
        // 낮은 턱에서 grab 키를 누르면 난간에 매달리는 모션을 취하기 때문
        else if (isTouchingWall && isGrabInputted && isTouchingLedge)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGrounded = player.CheckGround();
        isTouchingWall = player.CheckWall();
        isTouchingLedge = player.CheckLedge();
    }
}
