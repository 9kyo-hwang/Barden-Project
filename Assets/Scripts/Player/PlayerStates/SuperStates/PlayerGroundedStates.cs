using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState 상속
public class PlayerGroundedStates : PlayerState
{
    protected int xInput;
    public PlayerGroundedStates(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
