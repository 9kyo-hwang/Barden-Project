using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// 어느 상위 상태에 속하지 않는 독립 하위 상태
public class PlayerInAirState : PlayerState
{
    #region Variables
    // Input
    private int xInput;
    private bool isInputJumpStarted;
    private bool isInputJumpCanceled;
    private bool isInputGrab;
    private bool isInputDash;
    
    // Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isTouchingWallBefore; // wall jump coyote time 적용을 위한 변수
    private bool isTouchingWallBackBefore; // wall jump coyote time 적용을 위한 변수
    private bool isTouchingLedge;

    // other variables
    private bool isJumping;
    private bool isCoyoteTime;
    private bool isWallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;
    #endregion
    
    #region Core Components
    protected Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;
    #endregion

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
        isInputJumpStarted = player.InputHandler.IsInputJumpStarted;
        isInputJumpCanceled = player.InputHandler.IsInputJumpCanceled;
        isInputGrab = player.InputHandler.IsInputGrabStarted;
        isInputDash = player.InputHandler.IsInputDashStarted;
        
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
        // 땅에 닿았다면 land
        else if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        // 벽에 닿았을 때
        else if (isTouchingWall)
        {
            // 난간과 땅에 닿지 않은 상태라면 Ledge Climb
            if (!isTouchingLedge && !isGrounded)
            {
                stateMachine.ChangeState(player.LedgeClimbState);
            }
            // 난간에 닿은 상태로 Grab 키를 눌렀다면 Wall Grab
            // Wall Slide보다 우선됨
            else if (isTouchingLedge && isInputGrab)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            // 바라보는 방향으로 방향키를 눌렀으며 캐릭터가 내려가는 중이라면 Wall Slide
            else if (xInput == Movement.FacingDir && Movement.CurrentVelocity.y <= 0f)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }
        // 점프 버튼이 눌렸을 때
        else if (isInputJumpStarted)
        {
            // 벽에 닿았거나 벽점프 코요테 타임이라면 벽점프
            // 일반 점프보다 우선됨
            if (isTouchingWall || isTouchingWallBack || isWallJumpCoyoteTime)
            {
                StopWallJumpCoyoteTime(); // 벽점프 수행 시 벽점프 코요테 타임 중단
                isTouchingWall = CollisionSenses.GetWall; // 벽 점프 조건의 정확성을 위해 LogicUpdate()에서 재검사(DoCheck()의 시점은 약간 이전)
                player.WallJumpState.DetermineWallJumpDirection(isTouchingWall); // 점프 방향 결정
                stateMachine.ChangeState(player.WallJumpState);
            }
            // 점프 가능한 상태라면 일반 점프
            else if (player.JumpState.CanJump())
            {
                stateMachine.ChangeState(player.JumpState);
            }
        }
        // 대시 키를 눌렀으면서 대시를 할 수 있는 상태라면 dash 상태로
        else if (isInputDash && player.DashState.CheckCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        // 아니라면 x Velocity 재설정(공중 이동)
        else
        {
            Movement.CheckFlip(xInput);
            Movement.SetVelocityX(playerData.moveVelocity * xInput);
            
            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
        }
    }

    private void CheckJumpMultiplier()
    {
        // 점프 중이 아니라면 작동하지 않음
        if (!isJumping) return;
        
        // 점프 키 업 시 y Velocity에 가중치를 곱해서 점프 높이를 낮추고 점프 중이 아니도록 설정
        if (isInputJumpCanceled)
        {
            Movement.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
            isJumping = false;
        }
        // 현재 y Velocity가 0보다 작으면 떨어지는 중이므로 점프 중이 아니도록 설정
        else if(Movement.CurrentVelocity.y <= 0f)
        {
            isJumping = false;
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
        
        isGrounded = CollisionSenses.GetGround;
        isTouchingWall = CollisionSenses.GetWall;
        isTouchingWallBack = CollisionSenses.GetWallBack;
        isTouchingLedge = CollisionSenses.GetLedgeHor;

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
    }

    public void SetIsJumping()
    {
        isJumping = true;
    }

    private void StartWallJumpCoyoteTime()
    {
        isWallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    private void StopWallJumpCoyoteTime()
    {
        isWallJumpCoyoteTime = false;
    }
}
