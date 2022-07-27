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

    [Header("Check Variables")] 
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask whatIsGround;
}
