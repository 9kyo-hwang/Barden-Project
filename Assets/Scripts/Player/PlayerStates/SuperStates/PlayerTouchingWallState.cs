using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;
    protected bool isGrabInputted;
    protected bool isJumpInputted;
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.inputHandler.normalizedInputX;
        yInput = player.inputHandler.normalizedInputY;
        isGrabInputted = player.inputHandler.isGrabInputStarted;
        isJumpInputted = player.inputHandler.isJumpInputStarted;

        // touching Wall 상태에서 벗어나는 조건들

        // TouchingWall 상태에 속하는 모든 sub state 상태에서
        // wall jump가 수행될 수 있음
        if (isJumpInputted)
        {
            player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.wallJumpState);
        }
        // 땅에 닿았으면서 !grab input인 경우 idle 상태로
        else if (isGrounded && !isGrabInputted)
        {
            stateMachine.ChangeState(player.idleState);
        }
        // 벽에서 떨어졌거나, x축 입력이 바라보는 방향과 다르거나 없는 경우
        // 또는 !grabInput이면서 땅에 닿지 않았으면 inAir 상태로
        else if (!isTouchingWall || (xInput != core.movement.facingDir && !isGrabInputted))
        {
            stateMachine.ChangeState(player.inAirState);
        }
        // 벽에 닿았으면서 난간에 닿지 않았다면 ledgeClimb 상태로
        else if (isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.ledgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGrounded = core.colSenses.getGround;
        isTouchingWall = core.colSenses.getWallFront;
        isTouchingLedge = core.colSenses.getLedge;

        if (isTouchingWall && !isTouchingLedge)
        {
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }
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
