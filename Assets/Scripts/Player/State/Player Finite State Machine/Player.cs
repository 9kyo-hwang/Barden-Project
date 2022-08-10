using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    #region State Variables
    // property이므로 대문자 명명
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }
    
    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    // property이므로 대문자
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D Rb2d { get; private set; }
    public Transform DashDirIndicator { get; private set; }
    public BoxCollider2D BoxCol2d { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    #endregion

    #region Other Variables
    private Vector2 workspace; // velocity 변경용 임시 벡터 변수
    #endregion
    
    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir"); // 점프 시 공중 애니메이션 실행
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir"); // 벽점프 시 공중 애니메이션 수행
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir"); // 대시 중 공중 애니메이션 수행
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        Rb2d = GetComponent<Rigidbody2D>();
        DashDirIndicator = transform.Find("DashDirectionIndicator");
        BoxCol2d = GetComponent<BoxCollider2D>();
        Inventory = GetComponent<PlayerInventory>();

        // 장비 세팅
        PrimaryAttackState.SetWeapon(Inventory.weapons[(int)AttackInput.primary]);
        //secondaryAttackState.SetWeapon(inventory.weapons[(int)AttackInput.secondary]);

        // idle 상태로 상태머신 초기화
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Other Functions

    // 콜라이더 크기 및 중심점 재설정 함수
    public void SetColliderHeight(float height)
    {
        var center = BoxCol2d.offset;
        workspace.Set(BoxCol2d.size.x, height);

        center.y += (height - BoxCol2d.size.y) / 2;
        
        BoxCol2d.size = workspace;
        BoxCol2d.offset = center;
    }

    // PlayerState에 있는 AnimationTrigger 함수 수행
    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();    
    }

    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }
    #endregion
}
