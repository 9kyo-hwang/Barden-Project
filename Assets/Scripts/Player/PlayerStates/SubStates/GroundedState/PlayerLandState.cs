using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedStates
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            // 땅에 착지했을 때 x축 입력이 있다면 move 상태로
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            // 착지 애니메이션이 끝까지 수행되었다면 idle 상태로
            else if(isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
