using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedStates
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
        
        core.Movement.CheckFlip(xInput);
        core.Movement.SetVelocityX(playerData.moveVelocity * xInput);

        // move 상태를 빠져나가는 조건들
        if (!isExitingState)
        {
            // x축 입력이 없다면 idle 상태로
            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            // y축 아래로 입력이 있다면 crouchMove 상태로
            else if(yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }
}
