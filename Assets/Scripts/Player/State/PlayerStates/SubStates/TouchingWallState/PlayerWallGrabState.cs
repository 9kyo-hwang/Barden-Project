using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;
    
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        // 상태 진입 시점의 캐릭터 포지션 저장
        holdPosition = player.transform.position;
        
        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isExitingState) // 상태를 벗어나지 않았을 경우 수행
        {
            HoldPosition();
            
            // wall Grab 상태를 벗어나는 조건들
            
            // y Input이 0보다 크다면 wall Climb 상태로
            if (inputY > 0)
            {
                stateMachine.ChangeState(player.WallClimbState);
            }
            // y Input이 0보다 작거나 grab 버튼 키를 안눌렀을 경우
            else if (inputY < 0 || !isInputGrab)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }   
        }
    }

    private void HoldPosition()
    {
        // player의 위치를 지속적으로 grab 시작 위치로 갱신
        player.transform.position = holdPosition;
        
        // player의 x, y Velocity를 지속적으로 0으로 갱신
        Movement.SetVelocityZero();
    }
}
