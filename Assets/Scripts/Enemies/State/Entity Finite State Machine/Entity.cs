using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    #region State Variables
    public EntityStateMachine StateMachine { get; private set; }
    public EntityIdleState IdleState { get; private set; }
    public EntityMoveState MoveState { get; private set; }
    #endregion

    #region Components
    //public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb2d { get; private set; }
    public GameObject Alive { get; private set; }
    public EntityData data;
    #endregion
    
    #region Other Variables
    public int FacingDir { get; private set; }
    private Vector2 workspace;
    #endregion

    #region Transform Variables

    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform ledgeChecker;

    #endregion

    #region Unity Callback Functions
    public virtual void Awake()
    {
        StateMachine = new EntityStateMachine();
        //IdleState = new EntityIdleState(this, StateMachine, "idle");
        //MoveState = new EntityMoveState(this, StateMachine, "move");
    }

    public void Start()
    {
        Alive = transform.Find("Alive").gameObject;
        Rb2d = Alive.GetComponent<Rigidbody2D>();
        Anim = Alive.GetComponent<Animator>();
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions

    public virtual void SetVelocityX(float velocity)
    {
        workspace.Set(FacingDir * velocity, Rb2d.velocity.y);
        Rb2d.velocity = workspace;
    }
    
    #endregion

    #region Get Properties(Before Check Functions)

    public bool GetWall => 
        Physics2D.Raycast(wallChecker.position, Alive.transform.right, data.wallCheckDistance, 
            data.whatIsGround);

    public bool GetLedge =>
        Physics2D.Raycast(ledgeChecker.position, Vector2.down, data.ledgeCheckDistance, 
            data.whatIsGround);

    #endregion
}
