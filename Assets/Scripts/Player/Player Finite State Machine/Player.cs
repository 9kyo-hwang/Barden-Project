using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerLedgeClimbState ledgeClimbState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerCrouchIdleState crouchIdleState { get; private set; }
    public PlayerCrouchMoveState crouchMoveState { get; private set; }
    public PlayerAttackState primaryAttackState {get; private set;}
    public PlayerAttackState secondaryAttackState {get; private set;}
    
    [SerializeField]
    private PlayerData playerData;
    #endregion
    
    #region Components
    public Animator anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D rb2d { get; private set; }
    public Transform dashDirIndicator { get; private set; }
    public BoxCollider2D boxCol2d { get; private set; }
    public PlayerInventory inventory {get; private set;}
    #endregion
    
    #region Check Transforms

    [SerializeField] 
    private Transform groundChecker;
    [SerializeField] 
    private Transform wallChecker;
    [SerializeField] 
    private Transform ledgeChecker;
    [SerializeField] 
    private Transform ceilingChecker;
    
    #endregion
    
    #region Other Variables
    public Vector2 curVelocity { get; private set; }
    public int facingDir { get; private set; }
    private Vector2 workspace; // velocity 변경용 임시 벡터 변수
    #endregion
    
    #region Unity Callback Functions
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
        moveState = new PlayerMoveState(this, stateMachine, playerData, "move");
        jumpState = new PlayerJumpState(this, stateMachine, playerData, "inAir"); // 점프 시 공중 애니메이션 실행
        inAirState = new PlayerInAirState(this, stateMachine, playerData, "inAir");
        landState = new PlayerLandState(this, stateMachine, playerData, "land");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, playerData, "wallSlide");
        wallClimbState = new PlayerWallClimbState(this, stateMachine, playerData, "wallClimb");
        wallGrabState = new PlayerWallGrabState(this, stateMachine, playerData, "wallGrab");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, playerData, "inAir"); // 벽점프 시 공중 애니메이션 수행
        ledgeClimbState = new PlayerLedgeClimbState(this, stateMachine, playerData, "ledgeClimbState");
        dashState = new PlayerDashState(this, stateMachine, playerData, "inAir"); // 대시 중 공중 애니메이션 수행
        crouchIdleState = new PlayerCrouchIdleState(this, stateMachine, playerData, "crouchIdle");
        crouchMoveState = new PlayerCrouchMoveState(this, stateMachine, playerData, "crouchMove");
        primaryAttackState = new PlayerAttackState(this, stateMachine, playerData, "attack");
        secondaryAttackState = new PlayerAttackState(this, stateMachine, playerData, "attack");
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rb2d = GetComponent<Rigidbody2D>();
        dashDirIndicator = transform.Find("DashDirectionIndicator");
        boxCol2d = GetComponent<BoxCollider2D>();
        inventory = GetComponent<PlayerInventory>();

        facingDir = 1;

        // 장비 세팅
        primaryAttackState.SetWeapon(inventory.weapons[(int)AttackInput.primary]);
        //secondaryAttackState.SetWeapon(inventory.weapons[(int)AttackInput.secondary]);

        // idle 상태로 상태머신 초기화
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        curVelocity = rb2d.velocity; // 현재 벨로서티 지속 갱신
        stateMachine.currentState.LogicUpdate(); // 현재 상태에 대한 FrameUpdate() 수행
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate(); // 현재 상태에 대한 TimeUpdate() 수행
    }
    #endregion
    
    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, curVelocity.y);
        rb2d.velocity = workspace;
        curVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(curVelocity.x, velocity);
        rb2d.velocity = curVelocity = workspace;
    }

    // 속력뿐만 아니라 방향까지 정하는 함수
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize(); // 벡터 정규화 필요
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb2d.velocity = curVelocity = workspace;
    }

    public void SetVelocityZero()
    {
        rb2d.velocity = curVelocity = Vector2.zero;
    }

    // Vector2 방향을 정해주는 함수
    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        rb2d.velocity = curVelocity = workspace;
    }
    #endregion

    #region Check Functions

    public bool CheckCeiling()
    {
        return Physics2D.OverlapCircle(ceilingChecker.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    
    public bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundChecker.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckWall()
    {
        return Physics2D.Raycast(wallChecker.position, Vector2.right * facingDir, playerData.wallCheckDistance,
            playerData.whatIsGround);
    }

    public bool CheckWallBack()
    {
        return Physics2D.Raycast(wallChecker.position, Vector2.right * -facingDir, playerData.wallCheckDistance,
            playerData.whatIsGround);
    }

    public bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeChecker.position, Vector2.right * facingDir, playerData.wallCheckDistance,
            playerData.whatIsGround);
    }
    
    public void CheckFlip(int xInput)
    {
        if (xInput != 0 && xInput != facingDir)
            Flip();
    }
    #endregion

    #region Other Functions

    // 콜라이더 크기 및 중심점 재설정 함수
    public void SetColliderHeight(float height)
    {
        Vector2 center = boxCol2d.offset;
        workspace.Set(boxCol2d.size.x, height);

        center.y += (height - boxCol2d.size.y) / 2;
        
        boxCol2d.size = workspace;
        boxCol2d.offset = center;
    }

    // 난간 오르기 수행 시 꼭짓점 위치(도착 지점)를 잡아주는 함수
    // xDistance: 벽 체커 위치에서 Raycast 발사한 거리
    // yDistance: 난간 체커 위치에서 xDistance만큼 떨어진 거리에서 아랫 방향으로 난간 체커 - 벽 체커 길이만큼 Raycast 발사한 거리
    // 최종적으로 (벽 체커 위치 + xDistance, 난간 체커 위치 - yDistance) 위치 설정
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallChecker.position, Vector2.right * facingDir,
            playerData.wallCheckDistance, playerData.whatIsGround);
        var xDistance = xHit.distance + 0.015f; // 약간의 오차 허용 범위
        workspace.Set(xDistance * facingDir, 0f);

        RaycastHit2D yHit = Physics2D.Raycast(ledgeChecker.position + (Vector3)(workspace), Vector2.down,
            ledgeChecker.position.y - wallChecker.position.y, playerData.whatIsGround);
        var yDistance = yHit.distance + 0.015f; // 약간의 오차 허용 범위
        workspace.Set(wallChecker.position.x + (xDistance * facingDir), ledgeChecker.position.y - yDistance);

        return workspace;
    }

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
        facingDir *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
