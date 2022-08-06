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
    
    #endregion
    
    #region Other Variables
    public int LastDamageDir { get; private set; }

    protected bool isStunned;
    protected bool isDead;
    
    private Vector2 workspace;
    private float curHp;
    private float curStunResistance;
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
        
        Anim = GetComponent<Animator>();
        AnimToStateMachine = GetComponent<AnimationToStatemachine>();
    }

    public virtual void Start()
    {
        curHp = data.maxHp;
        curStunResistance = data.stunResistance;
    }

    private void Update()
    {
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

    public bool GetPlayerInMinRange =>
        Physics2D.Raycast(playerChecker.position, transform.right, data.minDetectionDistance, data.whatIsPlayer);

    public bool GetPlayerInMaxRange =>
        Physics2D.Raycast(playerChecker.position, transform.right, data.maxDetectionDistance, data.whatIsPlayer);

    public bool GetPlayerInCloseRangeAction => // 공격 범위 내 들어왔는 지
        Physics2D.Raycast(playerChecker.position, transform.right, data.closeRangeActionDistance,
            data.whatIsPlayer);
        
    #endregion

    #region Other Functions

    public virtual void Knockback(float velocity)
    {
        workspace.Set(Core.Movement.Rb2d.velocity.x, velocity);
        Core.Movement.Rb2d.velocity = workspace;
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        curStunResistance = data.stunResistance;
    }

    public virtual void Damage(EntityAttackDetails details)
    {
        lastDamagedTime = Time.time;
        
        curHp -= details.damage;
        curStunResistance -= details.stunDamage;
        
        Knockback(data.knockbackSpeed);

        Instantiate(data.hitParticle, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        
        LastDamageDir = details.position.x > transform.position.x ? -1 : 1;

        if (curStunResistance <= 0) // 데미지가 일정 수준 이상이면 스턴 발동
        {
            isStunned = true;
        }

        if (curHp <= 0)
        {
            isDead = true;
        }
    }

    public virtual void OnDrawGizmos()
    {
        if (Core == null) return;

        var wallCheckerPos = wallChecker.position;
        var ledgeCheckerPos = ledgeChecker.position;
        var playerCheckerPos = playerChecker.position;
        
        Gizmos.DrawLine(wallCheckerPos, wallCheckerPos + (Vector3)(Vector2.right * Core.Movement.FacingDir * data.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheckerPos, ledgeCheckerPos + (Vector3)(Vector2.down * data.ledgeCheckDistance));
        
        Gizmos.DrawWireSphere(playerCheckerPos + (Vector3)(Vector2.right * data.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheckerPos + (Vector3)(Vector2.right * data.minDetectionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheckerPos + (Vector3)(Vector2.right * data.maxDetectionDistance), 0.2f);
    }
    #endregion
}
