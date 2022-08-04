using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    #region State Variables
    public EntityStateMachine StateMachine { get; private set; }
    #endregion

    #region Components
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
    [SerializeField] private Transform playerChecker;

    #endregion

    #region Unity Callback Functions
    public virtual void Awake()
    {
        StateMachine = new EntityStateMachine();
    }

    public virtual void Start()
    {
        Alive = transform.Find("Alive").gameObject;
        Rb2d = Alive.GetComponent<Rigidbody2D>();
        Anim = Alive.GetComponent<Animator>();

        FacingDir = 1;
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

    public bool GetPlayerInMinRange =>
        Physics2D.Raycast(playerChecker.position, Alive.transform.right, data.minDetectionDistance, data.whatIsPlayer);

    public bool GetPlayerInMaxRange =>
        Physics2D.Raycast(playerChecker.position, Alive.transform.right, data.maxDetectionDistance, data.whatIsPlayer);
        
    #endregion

    #region Other Functions
    public virtual void Flip()
    {
        FacingDir *= -1;
        Alive.transform.Rotate(0f, 180f, 0f);   
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallChecker.position, wallChecker.position + (Vector3)(Vector2.right * FacingDir * data.wallCheckDistance));
        Gizmos.DrawLine(ledgeChecker.position, ledgeChecker.position + (Vector3)(Vector2.down * data.ledgeCheckDistance));
    }
    #endregion
}
