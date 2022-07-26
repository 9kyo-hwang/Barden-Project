using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isGrabInputted;
    protected int xInput;
    protected int yInput;
    
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        xInput = player.inputHandler.normalizedInputX;
        yInput = player.inputHandler.normalizedInputY;
        isGrabInputted = player.inputHandler.isGrabInputStarted;

        // touching Wall 상태에서 벗어나는 조건들
        
        // 땅에 닿았으면서 !grab input인 경우 idle 상태로
        if (isGrounded && !isGrabInputted)
        {
            stateMachine.ChangeState(player.idleState);
        }
        // 벽에서 떨어졌거나, x축 입력이 바라보는 방향과 다르거나 없는 경우
        // 또는 !grabInput이면서 땅에 닿지 않았으면 inAir 상태로
        else if (!isTouchingWall || (xInput != player.facingDirection && !isGrabInputted))
        {
            stateMachine.ChangeState(player.inAirState);
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

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
