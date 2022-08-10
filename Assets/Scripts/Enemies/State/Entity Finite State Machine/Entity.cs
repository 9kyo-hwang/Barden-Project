using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{
    #region State Variables
    public EntityStateMachine StateMachine { get; private set; }
    public EntityData data;
    #endregion
    
    #region Components
    public Animator Anim { get; private set; }
    public AnimationToStatemachine AnimToStateMachine { get; private set; }
    public Core Core { get; private set; }
    
    private Movement Movement => movement ?? Core.GetCoreComponentValue(ref movement);
    private Movement movement;
    #endregion

    #region Other Variables
    public int LastDamageDir { get; private set; }

    protected bool isStunned;
    protected bool isDead;
    
    private Vector2 workspace;

    private float currentHealthPoint;
    private float currentStunResistance;
    private float lastDamagedTime;
    #endregion

    #region Transform Variables

    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform ledgeChecker;
    [SerializeField] private Transform playerChecker;
    [SerializeField] private Transform groundChecker;

    #endregion

    #region Unity Callback Functions
    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        
        StateMachine = new EntityStateMachine();
    }

    public virtual void Start()
    {
        Anim = GetComponent<Animator>();
        AnimToStateMachine = GetComponent<AnimationToStatemachine>();
        
        currentHealthPoint = data.maxHealthPoint;
        currentStunResistance = data.stunResistance;
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
        
        //Anim.SetFloat("yVelocity", Core.Movement.Rb2d.velocity.y);

        if (Time.time >= lastDamagedTime + data.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Get Properties(Before Check Functions)

    public bool GetPlayerInMinDetectionRange => // 탐지 최소 거리에 들어왔는 지
        Physics2D.Raycast(playerChecker.position, transform.right, data.minDetectionDistance, data.whatIsPlayer);

    public bool GetPlayerInMaxDetectionRange => // 탐지 최대 거리에 들어왔는 지
        Physics2D.Raycast(playerChecker.position, transform.right, data.maxDetectionDistance, data.whatIsPlayer);

    public bool GetPlayerInCloseActionRange => // 근접 공격 범위에 들어왔는 지
        Physics2D.Raycast(playerChecker.position, transform.right, data.closeRangeActionDistance,
            data.whatIsPlayer);
        
    #endregion

    #region Other Functions

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = data.stunResistance;
    }

    public virtual void OnDrawGizmos()
    {
        if (Core == null) return;

        var wallCheckerPos = wallChecker.position;
        var ledgeCheckerPos = ledgeChecker.position;
        var playerCheckerPos = playerChecker.position;
        
        Gizmos.DrawLine(wallCheckerPos, wallCheckerPos + (Vector3)(Vector2.right * Movement.FacingDir * data.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheckerPos, ledgeCheckerPos + (Vector3)(Vector2.down * data.ledgeCheckDistance));
        
        Gizmos.DrawWireSphere(playerCheckerPos + (Vector3)(Vector2.right * data.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheckerPos + (Vector3)(Vector2.right * data.minDetectionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheckerPos + (Vector3)(Vector2.right * data.maxDetectionDistance), 0.2f);
    }
    #endregion
}
