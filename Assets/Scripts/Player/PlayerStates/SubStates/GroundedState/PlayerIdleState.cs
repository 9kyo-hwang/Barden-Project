using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedStates
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void TimeUpdate()
    {
        base.TimeUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }
}
