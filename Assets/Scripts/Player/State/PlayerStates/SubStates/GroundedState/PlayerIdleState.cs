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
        
        Movement?.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // idle 상태를 빠져나가는 조건들
        if (!isExitingState)
        {
            // x축 입력이 있다면 move 상태로
            if (inputX != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            // y축 아래로 입력이 있다면 crouch idle 상태로
            else if (inputY == -1)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
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
