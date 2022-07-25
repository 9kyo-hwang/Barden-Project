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
    [SerializeField]
    private PlayerData playerData;
    #endregion
    
    #region Components
    public Animator anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D rb2d { get; private set; }
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
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rb2d = GetComponent<Rigidbody2D>();

        facingDirection = 1;
        
        // 상태머신 초기화
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        currentVelocity = rb2d.velocity;
        stateMachine.currentState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.TimeUpdate();
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
    public void CheckFlip(int xInput)
    {
        if (xInput != 0 && xInput != facingDirection)
            Flip();
    }
    #endregion

    #region Other Functions
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
