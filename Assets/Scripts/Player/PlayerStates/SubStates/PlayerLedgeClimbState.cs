using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 어느 상위 상태에 속하지 않는 독립 하위 상태
public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPosition;
    private Vector2 stopPosition;

    private bool isHanging;
    private bool isClimbing;
    private bool isJumpInputted;
    private bool isTouchingCeiling;

    private int xInput;
    private int yInput;
    
    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        
    }

    public void SetDetectedPosition(Vector2 position)
    {
        detectedPosition = position;
    }

    public override void Enter()
    {
        base.Enter();
        
        // 상태 진입 시 x, y 벨로서티 0으로, 위치를 탐지한 위치로 설정
        player.SetVelocityZero();
        player.transform.position = detectedPosition;
        cornerPosition = player.DetermineCornerPosition();

        startPosition.Set(cornerPosition.x - (player.facingDir * playerData.startOffset.x),
            cornerPosition.y - playerData.startOffset.y);
        stopPosition.Set(cornerPosition.x + (player.facingDir * playerData.stopOffset.x),
            cornerPosition.y + playerData.stopOffset.y);

        player.transform.position = startPosition;
    }

    public override void Exit()
    {
        base.Exit();

        // LedgeClimb 상태 탈출 -> 매달려있지 않음
        isHanging = false;

        // LedgeClimb 상태 탈출 -> 캐릭터 위치 이동
        if (isClimbing)
        {
            player.transform.position = stopPosition;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // 애니메이션 종료: 난간 다 오름 -> Idle 상태
        if (isAnimationFinished)
        {
            // 애니메이션 종료 때 천장에 닿는 게 있다면 crouch Idle 상태로
            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }
            // 아니라면 일반 idle 상태로
            else
            {
                stateMachine.ChangeState(player.idleState);   
            }
        }
        // 아니라면 Ledge Climb 상태 유지중
        // 입력 및 위치 조정 수행
        else
        {
            xInput = player.inputHandler.normalizedInputX;
            yInput = player.inputHandler.normalizedInputY;
            isJumpInputted = player.inputHandler.isJumpInputStarted;
        
            player.SetVelocityZero();
            player.transform.position = startPosition;

            // LedgeClimb 상태를 벗어나는 조건들 
            
            // x축 입력이 플레이어가 바라보는 방향과 같으면서 매달려 있고 올라가는 중이 아니라면
            // 클라이밍 시작, 애니메이션 재생
            if (xInput == player.facingDir && isHanging && !isClimbing)
            {
                CheckSpace(); // 올라간 위치에 천장이 있는 지 확인
                isClimbing = true;
                player.anim.SetBool("climbLedge", true);
            }
            // y축 입력이 아래방향이면서 매달려 있고 올라가는 중이 아니라면
            // 매달려있는 것에서 떨어짐 -> inAir 상태
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.inAirState);
            }
            // 매달린 상태에서(오르는 중이 아님) 점프키를 누르면 wallJump 상태로
            else if (isJumpInputted && !isClimbing)
            {
                player.wallJumpState.DetermineWallJumpDirection(true); // 매달려 있음 -> 벽에 붙어있는 상태
                stateMachine.ChangeState(player.wallJumpState);
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        // LedgeGrab 애니메이션 끝에 도달 시 매달려있기 시작
        isHanging = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        
        // 애니메이션 종료 시 climbLedge false
        player.anim.SetBool("climbLedge", false);
    }

    // 난간을 올랐을 때 온전히 서있을 수 있는 공간이 있는 지 확인하는 함수
    // 코너 위치 기준 stand collider height 만큼 ray를 쏴서 닿는 게 있다면 높이가 낮음(천장 존재)
    private void CheckSpace()
    {
        isTouchingCeiling =
            Physics2D.Raycast(cornerPosition + (Vector2.up * 0.015f) + (Vector2.right * player.facingDir * 0.015f),
                Vector2.up, playerData.standColliderHeight, playerData.whatIsGround);
        player.anim.SetBool("isTouchingCeiling", isTouchingCeiling);
    }
}
