using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재 Enemy가 어떤 상태인 지 나타내는 스크립트
public class EntityStateMachine
{
    // PlayerState를 참조하는 모든 스크립트들은 변수에 접근 가능하나 수정은 불가능함
    public EntityState CurrentState { get; private set; }

    // 초기화 함수
    // 시작 상태로 변경, 진입 함수 실행
    public void Initialize(EntityState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    // 상태 변경 함수
    // 기존 상태에서 탈출 후 새 상태로 갱신, 진입 함수 실행
    public void ChangeState(EntityState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
