using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 Enemy들이 공통적으로 가져야하는 상태 함수들을 정의하는 스크립트
public class EntityState
{
    protected Core core;

    protected Entity entity; // 참조용 엔터티
    protected EntityStateMachine stateMachine; // 참조용 상태 머신
    //protected EntityData playerData; // 참조용 데이터

    protected bool isAnimationFinished; // 애니메이션이 끝났는 지 판단하는 변수
    protected bool isExitingState; // 상태를 벗어났는 지 나타내는 변수
    
    protected float startTime; // 상태 진입 시작 시간. 항상 참조됨
    
    private string animBoolName; // 어떤 애니메이션을 실행시켜야 하는 지 판단하는 스트링 변수
    
    public EntityState(Entity entity, EntityStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        //this.playerData = playerData;
        this.animBoolName = animBoolName;
        //core = entity.Core;
    }

    // 모든 상태별 진입, 탈출, Update, FixedUpdate 함수 필요
    // 하위 클래스에서 오버라이딩 될 수 있음
    
    // 특정 상태에 진입했을 때 호출
    public virtual void Enter()
    {
        DoCheck();
        entity.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
    }

    // 특정 상태에서 탈출했을 때 호출
    public virtual void Exit()
    {
        entity.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }
    
    // 매 프레임마다 호출(Update()에서 수행)
    public virtual void LogicUpdate()
    {
        
    }

    // 매 Timestep마다 호출(FixedUpdate()에서 수행)
    public virtual void PhysicsUpdate()
    {
        DoCheck();
    }

    // TimeUpdate랑 Enter에서 호출될 함수
    // 땅에 있는 지, 또는 벽에 있는 지 등등을 확인하는 용도
    public virtual void DoCheck()
    {
        
    }

    // 애니메이션 컨트롤용 함수
    public virtual void AnimationTrigger()
    {
        
    }

    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
    }
}
