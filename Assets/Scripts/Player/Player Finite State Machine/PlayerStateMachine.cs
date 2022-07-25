using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 상태를 변경해주는 스크립트
public class PlayerStateMachine
{
    // PlayerState를 참조하는 모든 스크립트들은 변수에 접근 가능하나 수정은 불가능함
    // 프로퍼티(getter, setter)는 변수명을 대문자로 시작하는 것을 추천
    // 하지만 난 카멜을 쓸 거임
    public PlayerState currentState { get; private set; }

    // 초기화 함수
    // 시작 상태로 변경, 진입 함수 실행
    public void Initialize(PlayerState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    // 상태 변경 함수
    // 기존 상태에서 탈출 후 새 상태로 갱신, 진입 함수 실행
    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
