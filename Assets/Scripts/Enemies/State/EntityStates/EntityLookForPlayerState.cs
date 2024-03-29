using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLookForPlayerState : EntityState
{
    #region Variables
    protected bool turnImmediately;
    protected bool isEnteringPlayerInMinDetectionRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;
    protected int currentTurnCount;
    #endregion
    
    #region Core Components
    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    #endregion

    public EntityLookForPlayerState(Entity entity, EntityStateMachine stateMachine, EntityData data, string animBoolName) : base(entity, stateMachine, data, animBoolName)
    {
        
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isEnteringPlayerInMinDetectionRange = entity.GetPlayerInMinDetectionRange;
    }

    public override void Enter()
    {
        base.Enter();

        // Look For Player 상태 진입 시 각종 변수 초기화
        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;

        lastTurnTime = StartTime;
        currentTurnCount = 0;

        Movement.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Movement.SetVelocityX(0f);

        // 즉시 turn 해야하는 상황일 경우
        if(turnImmediately)
        {
            Movement.Flip();
            lastTurnTime = Time.time;
            currentTurnCount++;
            turnImmediately = false;
        }
        // 턴 대기시간을 초과했으면서 아직 모든 턴 횟수를 다 쓴게 아니라면
        else if(Time.time >= lastTurnTime + data.timeBetweenTurn && !isAllTurnsDone)
        {
            Movement.Flip();
            lastTurnTime = Time.time;
            currentTurnCount++;
        }

        if(currentTurnCount >= data.maxTurnCount)
        {
            isAllTurnsDone = true;
        }

        if(Time.time >= lastTurnTime + data.timeBetweenTurn && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    #region Set Functions
    // 즉각 플레이어를 향해 돌게 하는 함수
    public void SetTurnImmediately(bool turn)
    {
        turnImmediately = turn;
    }
    #endregion
}
