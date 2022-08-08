using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    #region Core Components
    protected Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    private CollisionSenses CollisionSenses => collisionSenses ?? core.GetCoreComponentValue(ref collisionSenses);
    private CollisionSenses collisionSenses;
    #endregion
    
    protected bool isAbilityDone; // 행동이 종료됐는 지 나타내는 변수
    private bool isGrounded; // 땅에 닿아있는 지 나타내는 변수
    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        // ability 상태가 끝났을 경우 변경될 수 있는 상태는 지상 or 공중
        // isGrounded에 따라 상태 변경
        if (isAbilityDone)
        {
            // 즉각적으로 점프를 수행할 수 있기 때문에 y Velocity 제약 조건 설정
            if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
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

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.GetGround;
        }
    }
}
