using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌 판정을 필요로 하는 모든 오브젝트가 참조하는 충돌 코어 컴포넌트 
public class CollisionSenses : CoreComponent
{

    #region Check Variables
    // private 선언된 변수들을 외부에서 접근할 수 있게 프로퍼티 설정
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform ledgeChecker;
    [SerializeField] private Transform ceilingChecker;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public Transform GroundChecker { get => groundChecker; set => groundChecker = value; }
    public Transform WallChecker { get => wallChecker; set => wallChecker = value; }
    public Transform LedgeChecker { get => ledgeChecker; set => ledgeChecker = value; }
    public Transform CeilingChecker { get => ceilingChecker; set => ceilingChecker = value; }
    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
    #endregion

    #region Properties(Before Check Functions)

    // 기존 함수들을 프로퍼티로 변경
    public bool getCeiling => Physics2D.OverlapCircle(ceilingChecker.position, groundCheckRadius, whatIsGround);
    public bool getGround { get => Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, whatIsGround); }
    public bool getWallFront { get => Physics2D.Raycast(wallChecker.position, Vector2.right * core.movement.facingDir, wallCheckDistance, whatIsGround); }
    public bool getWallBack { get => Physics2D.Raycast(wallChecker.position, Vector2.right * -core.movement.facingDir, wallCheckDistance, whatIsGround); }
    public bool getLedge { get => Physics2D.Raycast(ledgeChecker.position, Vector2.right * core.movement.facingDir, wallCheckDistance, whatIsGround); }

    #endregion
}
