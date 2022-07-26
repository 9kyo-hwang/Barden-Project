using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        // player의 y Velocity를 지속적으로 wallClimbVelocity로 갱신
        player.SetVelocityY(playerData.wallClimbVelocity);

        // wallClimb 상태를 벗어나는 조건들
        
        // y축 입력이 윗 방향이 아니라면 wallGrab 상태로
        if (yInput != 1)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
    }
}
