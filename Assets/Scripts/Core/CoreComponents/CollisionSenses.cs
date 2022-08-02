using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    #region Check Transforms
    // SerializeField 변수들은 public getter setter가 작동하지 않음
    // 따로 프로퍼티 설정
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform ledgeChecker;
    [SerializeField] private Transform ceilingChecker;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    #endregion
    
    #region Variable Properties
    public Transform GroundChecker { get => groundChecker; set => groundChecker = value; }
    public Transform WallChecker { get => wallChecker; set => wallChecker = value; }
    public Transform LedgeChecker { get => ledgeChecker; set => ledgeChecker = value; }
    public Transform CeilingChecker { get => ceilingChecker; set => ceilingChecker = value; }
    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
    #endregion
    
    #region Get Properties(before Check Functions)
    public bool GetCeiling => Physics2D.OverlapCircle(ceilingChecker.position, groundCheckRadius, whatIsGround);

    public bool GetGround => Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, whatIsGround);

    public bool GetWall =>
        Physics2D.Raycast(wallChecker.position, Vector2.right * core.Movement.FacingDir, wallCheckDistance,
            whatIsGround);

    public bool GetWallBack =>
        Physics2D.Raycast(wallChecker.position, Vector2.right * -core.Movement.FacingDir, wallCheckDistance,
            whatIsGround);

    public bool GetLedge =>
        Physics2D.Raycast(ledgeChecker.position, Vector2.right * core.Movement.FacingDir, wallCheckDistance,
            whatIsGround);
    #endregion
}
