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
    
    public Core core {get; private set;}
    public Animator anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D rb2d { get; private set; }
    public Transform dashDirIndicator { get; private set; }
    public BoxCollider2D boxCol2d { get; private set; }
    public PlayerInventory inventory {get; private set;}
    #endregion
    
    #region Other Variables
    private Vector2 workspace; // velocity 변경용 임시 벡터 변수
    #endregion
    
    #region Unity Callback Functions
    private void Awake()
    {
        core = GetComponentInChildren<Core>();
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

        // 장비 세팅
        primaryAttackState.SetWeapon(inventory.weapons[(int)AttackInput.primary]);
        //secondaryAttackState.SetWeapon(inventory.weapons[(int)AttackInput.secondary]);

        // idle 상태로 상태머신 초기화
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        core.LogicUpdate(); // 코어의 LogicUpdate 수행
        stateMachine.currentState.LogicUpdate(); // 현재 상태에 대한 FrameUpdate() 수행
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate(); // 현재 상태에 대한 TimeUpdate() 수행
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

    // PlayerState에 있는 AnimationTrigger 함수 수행
    private void AnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();    
    }

    private void AnimationFinishTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    #endregion
}
