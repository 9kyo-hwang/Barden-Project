using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Serialization;

public class CollisionSenses : CoreComponent
{
    #region Check Transforms
    // SerializeField 변수들은 public getter setter가 작동하지 않음
    // 따로 프로퍼티 설정
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform horLedgeChecker;
    [SerializeField] private Transform verLedgeChecker;
    [SerializeField] private Transform ceilingChecker;
    
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    #endregion
    
    #region Variable Properties

    public Transform GroundChecker
    {
        get
        {
            if (groundChecker) return groundChecker;
            
            Debug.LogError("No Ground Check on " + core.transform.parent.name);
            return null;
        }
        private set => groundChecker = value;
    }
    public Transform WallChecker     
    {
        get
        {
            if (wallChecker) return wallChecker;
            
            Debug.LogError("No Wall Check on " + core.transform.parent.name);
            return null;
        }
        private set => wallChecker = value;
    }
    public Transform HorLedgeChecker
    {
        get
        {
            if (horLedgeChecker) return horLedgeChecker;
            
            Debug.LogError("No Horizontal Ledge Check on " + core.transform.parent.name);
            return null;
        }
        private set => horLedgeChecker = value;
    }
    public Transform VerLedgeChecker
    {
        get
        {
            if (verLedgeChecker) return verLedgeChecker;
            
            Debug.LogError("No Vertical Ledge Check on " + core.transform.parent.name);
            return null;
        }
        private set => verLedgeChecker = value;
    }
    public Transform CeilingChecker
    {
        get
        {
            if (ceilingChecker) return ceilingChecker;
            
            Debug.LogError("No Ceiling Check on " + core.transform.parent.name);
            return null;
        }
        private set => ceilingChecker = value;
    }
    
    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
    #endregion
    
    #region Get Properties(before Check Functions)
    public bool GetCeiling => Physics2D.OverlapCircle(CeilingChecker.position, groundCheckRadius, whatIsGround);

    public bool GetGround => Physics2D.OverlapCircle(GroundChecker.position, groundCheckRadius, whatIsGround);

    public bool GetWall =>
        Physics2D.Raycast(WallChecker.position, Vector2.right * core.Movement.FacingDir, wallCheckDistance,
            whatIsGround);

    public bool GetWallBack =>
        Physics2D.Raycast(WallChecker.position, Vector2.right * -core.Movement.FacingDir, wallCheckDistance,
            whatIsGround);

    public bool GetLedgeHor =>
        Physics2D.Raycast(HorLedgeChecker.position, Vector2.right * core.Movement.FacingDir, wallCheckDistance,
            whatIsGround);

    public bool GetLedgeVer =>
        Physics2D.Raycast(VerLedgeChecker.position, Vector2.down, wallCheckDistance, whatIsGround);

    #endregion
}
