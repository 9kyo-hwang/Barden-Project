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
    public Rigidbody2D Rb2d { get; private set; }
    public GameObject Alive { get; private set; }
    public AnimationToStatemachine AnimToStateMachine { get; private set; }
    
    #endregion
    
    #region Other Variables
    public int FacingDir { get; private set; }
    public int LastDamageDir { get; private set; }

    protected bool isStunned;
    
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
        StateMachine = new EntityStateMachine();
    }

    public virtual void Start()
    {
        Alive = transform.Find("Alive").gameObject;
        Rb2d = Alive.GetComponent<Rigidbody2D>();
        Anim = Alive.GetComponent<Animator>();
        AnimToStateMachine = Alive.GetComponent<AnimationToStatemachine>();

        curHp = data.maxHp;
        curStunResistance = data.stunResistance;
        FacingDir = 1;
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();

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

    #region Set Functions

    public virtual void SetVelocityX(float velocity)
    {
        workspace.Set(FacingDir * velocity, Rb2d.velocity.y);
        Rb2d.velocity = workspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int dir)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * dir, angle.y * velocity);
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

    public bool GetGround =>
        Physics2D.OverlapCircle(groundChecker.position, data.groundCheckRadius, data.whatIsGround);

    public bool GetPlayerInMinRange =>
        Physics2D.Raycast(playerChecker.position, Alive.transform.right, data.minDetectionDistance, data.whatIsPlayer);

    public bool GetPlayerInMaxRange =>
        Physics2D.Raycast(playerChecker.position, Alive.transform.right, data.maxDetectionDistance, data.whatIsPlayer);

    public bool GetPlayerInCloseRangeAction => // 공격 범위 내 들어왔는 지
        Physics2D.Raycast(playerChecker.position, Alive.transform.right, data.closeRangeActionDistance,
            data.whatIsPlayer);
        
    #endregion

    #region Other Functions
    public virtual void Flip()
    {
        FacingDir *= -1;
        Alive.transform.Rotate(0f, 180f, 0f);   
    }

    public virtual void Knockback(float velocity)
    {
        workspace.Set(Rb2d.velocity.x, velocity);
        Rb2d.velocity = workspace;
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
        
        LastDamageDir = details.position.x > Alive.transform.position.x ? -1 : 1;

        if (curStunResistance <= 0) // 데미지가 일정 수준 이상이면 스턴 발동
        {
            isStunned = true;
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallChecker.position, wallChecker.position + (Vector3)(Vector2.right * FacingDir * data.wallCheckDistance));
        Gizmos.DrawLine(ledgeChecker.position, ledgeChecker.position + (Vector3)(Vector2.down * data.ledgeCheckDistance));
        
        Gizmos.DrawWireSphere(playerChecker.position + (Vector3)(Vector2.right * data.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerChecker.position + (Vector3)(Vector2.right * data.minDetectionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerChecker.position + (Vector3)(Vector2.right * data.maxDetectionDistance), 0.2f);
    }
    #endregion
}
