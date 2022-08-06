using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// 어느 상위 상태에 속하지 않는 독립 하위 상태
public class PlayerInAirState : PlayerState
{
    // Input
    private int xInput;
    private int yInput;
    private bool isJumpInputted;
    private bool isJumpInputStopped;
    private bool isGrabInputted;
    private bool isDashInputted;
    
    // Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isTouchingWallBefore; // wall jump coyote time 적용을 위한 변수
    private bool isTouchingWallBackBefore; // wall jump coyote time 적용을 위한 변수
    private bool isTouchingLedge;

    private bool isJumping;
    private bool isCoyoteTime;
    private bool isWallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;

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

        // 벽점프 코요테 시간의 정확성을 위해
        // inAir -> TouchingWall 같은 상황일 때, 즉 inAir 상태에서 나갈 때
        // touching wall 관련 변수 모두 false로 초기화
        isTouchingWallBefore = false;
        isTouchingWallBackBefore = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        
        xInput = player.InputHandler.InputXNormalize;
        yInput = player.InputHandler.InputYNormalize;
        isJumpInputted = player.InputHandler.IsInputJumpStarted;
        isJumpInputStopped = player.InputHandler.IsInputJumpCanceled;
        isGrabInputted = player.InputHandler.IsInputGrabStarted;
        isDashInputted = player.InputHandler.IsInputDashStarted;
        
        CheckJumpMultiplier();
        
        // inAir 상태에서 벗어나는 조건들

        if(player.InputHandler.IsInputAttackArray[(int)AttackInput.primary])
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if(player.InputHandler.IsInputAttackArray[(int)AttackInput.secondary])
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        // 땅에 닿았다면 land 상태로
        else if (isGrounded && core.Movement.CurVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        // 벽에 닿았으면서 난간과 땅에 닿지 않았을 경우 ledge Climb 상태로
        // 낮은 턱에 플레이어가 붙은 채 점프를 하면 난간에 매달리는 모션이 취해지기 때문
        else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        // jump보다 우선되는 wall Jump
        // 점프 버튼이 눌렸으면서 캐릭터 앞이나 뒤로 벽에 닿아있다면
        else if (isJumpInputted && (isTouchingWall || isTouchingWallBack || isWallJumpCoyoteTime))
        {
            // 점프 방향 결정 후 벽 점프 상태로
            // 벽 점프 조건의 정확성을 위해 다시 한 번 LogicUpdate()에서 검사해줌
            // PhysicsUpdate()의 DoCheck()에서 검사한 조건은 약간 이전의 조건으로 오류 발생 가능
            // 벽점프 수행 시 벽점프 코요테 타임 중단
            StopWallJumpCoyoteTime();
            isTouchingWall = core.ColSenses.GetWall;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        // 점프 버튼이 눌렸으면서 점프 할 수 있는 상태라면 jump 상태로(공중 점프)
        else if (isJumpInputted && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        // wall Slide보다 우선되는 wall Grab
        // 벽과 난간에 닿았으면서 그랩 버튼을 눌렀을 경우 wall Grab 상태로
        // 낮은 턱에서 그랩 키를 누른 채 점프를 하면 난간 오르는 모션이 취해지기 때문
        else if (isTouchingWall && isGrabInputted && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        // 벽에 닿았으면서 플레이어가 바라보는 방향과 x축 입력 방향이 같고
        // 캐릭터의 y Velocity가 0 이하라면 wall Slide 상태로
        else if (isTouchingWall && xInput == core.Movement.FacingDir && core.Movement.CurVelocity.y <= 0f)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        // 대시 키를 눌렀으면서 대시를 할 수 있는 상태라면 dash 상태로
        else if (isDashInputted && player.DashState.CheckCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        // 아니라면 x Velocity 재설정(공중 이동)
        else
        {
            core.Movement.CheckFlip(xInput);
            core.Movement.SetVelocityX(playerData.moveVelocity * xInput);
            
            player.Anim.SetFloat("yVelocity", core.Movement.CurVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(core.Movement.CurVelocity.x));
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
                core.Movement.SetVelocityY(core.Movement.CurVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            // 현재 y Velocity가 0보다 작으면 떨어지는 중이므로 점프 중이 아니도록 설정
            else if(core.Movement.CurVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();

        // 벽에 닿아있는 지 정보 기록
        isTouchingWallBefore = isTouchingWall;
        isTouchingWallBackBefore = isTouchingWallBack;

        // 땅이나 벽에 닿아있는 지 정보 갱신
        isGrounded = core.ColSenses.GetGround;
        isTouchingWall = core.ColSenses.GetWall;
        isTouchingWallBack = core.ColSenses.GetWallBack;
        isTouchingLedge = core.ColSenses.GetLedgeHor;

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        // 아직 벽점프 코요테 시간에 진입하지 않았으면서 지금은 벽에 닿아 있지 않고,
        // 직전까지 벽에 닿아 있었다면 벽점프 코요테 시간 시작
        if (!isWallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack &&
            (isTouchingWallBefore || isTouchingWallBackBefore))
        {
            StartWallJumpCoyoteTime();
        }
    }

    private void CheckCoyoteTime()
    {
        if (isCoyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            isCoyoteTime = false;
            player.JumpState.DecreaseLeftJumpCount();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (isWallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            isWallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime()
    {
        isCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void SetIsJumping()
    {
        isJumping = true;
    }

    public void StartWallJumpCoyoteTime()
    {
        isWallJumpCoyoteTime = true;
    }

    public void StopWallJumpCoyoteTime()
    {
        isWallJumpCoyoteTime = false;
    }
}
