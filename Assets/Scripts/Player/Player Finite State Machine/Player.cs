using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerInAirState inAirState { get; private set; }
    public PlayerLandState landState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallClimbState wallClimbState { get; private set; }
    public PlayerWallGrabState wallGrabState { get; private set; }
    
    [SerializeField]
    private PlayerData playerData;
    #endregion
    
    #region Components
    public Animator animator { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D rb2d { get; private set; }
    #endregion
    
    #region Check Transforms

    [SerializeField] 
    private Transform groundChecker;
    [SerializeField] 
    private Transform wallChecker;
    
    #endregion
    
    #region Other Variables
    public Vector2 currentVelocity { get; private set; }
    public int facingDirection { get; private set; }
    // velocity 변경용 임시 벡터2
    private Vector2 workspace;
    #endregion
    
    #region Unity Callback Functions
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        moveState = new PlayerMoveState(this, stateMachine, playerData, "move");
        jumpState = new PlayerJumpState(this, stateMachine, playerData, "inAir"); // 점프 시 공중에 뜨게 되므로 자동적으로 inAir
        inAirState = new PlayerInAirState(this, stateMachine, playerData, "inAir");
        landState = new PlayerLandState(this, stateMachine, playerData, "land");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, playerData, "wallSlide");
        wallClimbState = new PlayerWallClimbState(this, stateMachine, playerData, "wallClimb");
        wallGrabState = new PlayerWallGrabState(this, stateMachine, playerData, "wallGrab");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rb2d = GetComponent<Rigidbody2D>();

        facingDirection = 1;
        
        // idle 상태로 상태머신 초기화
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        currentVelocity = rb2d.velocity; // 현재 벨로서티 지속 갱신
        stateMachine.currentState.FrameUpdate(); // 현재 상태에 대한 FrameUpdate() 수행
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.TimeUpdate(); // 현재 상태에 대한 TimeUpdate() 수행
    }
    #endregion
    
    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, currentVelocity.y);
        rb2d.velocity = workspace;
        currentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(currentVelocity.x, velocity);
        rb2d.velocity = workspace;
        currentVelocity = workspace;
    }
    #endregion

    #region Check Functions

    public bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundChecker.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckWall()
    {
        return Physics2D.Raycast(wallChecker.position, Vector2.right * facingDirection, playerData.wallCheckDistance,
            playerData.whatIsGround);
    }
    
    public void CheckFlip(int xInput)
    {
        if (xInput != 0 && xInput != facingDirection)
            Flip();
    }
    #endregion

    #region Other Functions

    // PlayerState에 있는 AnimationTrigger 함수 수행
    private void AnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();    
    }

    private void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }
    
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
