using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    private bool isHolding; // Time scale holding boolean variable
    private bool isDashInputStopped;

    private float lastDashTime;

    private Vector2 dashDirection;
    private Vector2 dashDirectionInput;
    
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
        
    }
    public override void Enter()
    {
        base.Enter();

        // dash 상태 진입 시 boolean 변수 false화
        CanDash = false;
        player.InputHandler.UsedDashInput();

        isHolding = true;
        dashDirection = Vector2.right * Movement.FacingDir;

        Time.timeScale = playerData.holdTimeScale; // 타임 스케일 변경
        startTime = Time.unscaledTime; // 타임 스케일에 영향 받지 않은 시작 시간 저장
        
        // 대시 상태 진입 시 인디케이터 이미지 활성화
        player.DashDirIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        // dash 상태 탈출 때 과도한 y축 상승을 막기 위해 가중치 할당
        if (Movement?.CurrentVelocity.y > 0)
        {
            Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
            if (isHolding)
            {
                dashDirectionInput = player.InputHandler.InputDashDirectionInt;
                isDashInputStopped = player.InputHandler.IsInputDashCanceled;
                
                // 아무 방향키도 안 누른 게 아닐 경우 방향 설정
                if (dashDirectionInput != Vector2.zero)
                {
                    dashDirection = dashDirectionInput;
                    dashDirection.Normalize();
                }

                // 사이각 구해서 indicator 이미지 회전, 단 기본적으로 45도 틀어져있어서 -45도
                var angle = Vector2.SignedAngle(Vector2.right, dashDirection);
                player.DashDirIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

                // dash 키 업 또는 최대 hold 시간 초과 시 holding stop
                if (isDashInputStopped || Time.unscaledTime >= startTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;
                    Movement?.CheckFlip(Mathf.RoundToInt(dashDirection.x));
                    player.Rb2d.drag = playerData.dashDrag; // 일시적으로 drag 증가
                    Movement?.SetVelocityAngle(playerData.dashVelocity, dashDirection);
                    player.DashDirIndicator.gameObject.SetActive(false); // 대시 발동 시 인디케이서 비활성화
                }
            }
            // 대시 키 업 시
            else
            {
                // 속력 설정
                Movement?.SetVelocityAngle(playerData.dashVelocity, dashDirection);

                // 대시 후 일정 시간 지났을 시
                if (Time.time >= startTime + playerData.dashTime)
                {
                    player.Rb2d.drag = 0f; // drag 복구
                    isAbilityDone = true; // ability done
                    lastDashTime = Time.time; // 마지막 대시 시간 갱신
                }
            }
        }
    }

    public bool CheckCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash()
    {
        CanDash = true;
    }

}
