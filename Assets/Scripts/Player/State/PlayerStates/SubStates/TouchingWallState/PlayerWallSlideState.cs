using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState) // 상태를 벗어나지 않았을 경우 수행
        {
            // player의 y Velocity를 지속적으로 -wallSlideVelocity로 갱신
            Movement.SetVelocityY(-playerData.wallSlideVelocity);

            // wall Slide를 벗어나는 조건들
        
            // grab 버튼을 누른 상태에서 y축 방향키 입력이 없다면 wallGrab 상태로
            if (isInputGrab && inputY == 0)
            {
                stateMachine.ChangeState(player.WallGrabState);
            } 
        }
    }
}
