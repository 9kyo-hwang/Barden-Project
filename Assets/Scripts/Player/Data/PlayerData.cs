using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

// 플레이어에 사용될 각종 변수 및 값을 저장하는 클래스
// 스크립트로부터 애셋 생성 가능
[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")] 
    public float moveVelocity = 10f;

    [Header("Jump State")] 
    public float jumpVelocity = 15f;
    public int maxJumpCount = 1;

    [Header("In Air State")] 
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")] 
    public float wallSlideVelocity = 3f;

    [Header("Wall Climb State")] 
    public float wallClimbVelocity = 1.5f;

    [Header("Wall Jump State")] 
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f; // 벽 점프 상태를 짧게 유지하는 용도의 변수
    public Vector2 wallJumpAngle = new Vector2(1, 2); // 벽 점프 각도

    [Header("Ledge Climb State")] 
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("Dash State")] 
    public float dashCooldown = 0.5f;
    public float maxHoldTime = 1f; // 최대 대시 키다운 시간
    public float holdTimeScale = 0.25f; // 대시 키다운 중 시간 스케일 조정값
    public float dashTime = 0.2f; // 대시 수행 후 지난 시간. drag 돌리기 위한 용도
    public float dashVelocity = 30f;
    public float dashDrag = 10f; // air density를 일시적으로 조정
    public float dashEndYMultiplier = 0.2f; // dash 후 y축으로 과도하게 올라가는 걸 막는 용도
    public float distanceBetweenAfterImage = 0.5f;

    [Header("Crouch States")] 
    public float crouchMoveVelocity = 5f;
    public float crouchColliderHeight = 0.8f; // crouch 시 콜라이더 세로 길이 절반으로 조정
    public float standColliderHeight = 1.6f; // 기본 콜라이더 세로 길이

    [Header("Check Variables")] 
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask whatIsGround;
}
