using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    #region States
    public Enemy2IdleState IdleState { get; private set; }
    public Enemy2MoveState MoveState { get; private set; }
    public Enemy2DetectPlayerState DetectPlayerState { get; private set; }
    public Enemy2MeleeAttackState MeleeAttackState { get; private set; }
    public Enemy2LookForPlayerState LookForPlayerState { get; private set; }
    public Enemy2StunState StunState { get; private set; }
    public Enemy2DeadState DeadState { get; private set; }
    public Enemy2DodgeState DodgeState { get; private set; }
    public Enemy2RangedAttackState RangedAttackState { get; private set; }
    #endregion

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();

        IdleState = new Enemy2IdleState(this, StateMachine, data, "idle", this);
        MoveState = new Enemy2MoveState(this, StateMachine, data, "move", this);
        DetectPlayerState = new Enemy2DetectPlayerState(this, StateMachine, data, "detect", this);
        MeleeAttackState = new Enemy2MeleeAttackState(this, StateMachine, data, "meleeAttack", meleeAttackPosition, this);
        LookForPlayerState = new Enemy2LookForPlayerState(this, StateMachine, data, "lookForPlayer", this);
        StunState = new Enemy2StunState(this, StateMachine, data, "stun", this);
        DeadState = new Enemy2DeadState(this, StateMachine, data, "dead", this);
        DodgeState = new Enemy2DodgeState(this, StateMachine, data, "dodge", this);
        RangedAttackState =
            new Enemy2RangedAttackState(this, StateMachine, data, "rangedAttack", rangedAttackPosition, this);
    }

    public override void Start()
    {
        base.Start();
        
        StateMachine.Initialize(MoveState);
    }

    public override void Update()
    {
        base.Update();
        
        Anim.SetFloat("yVelocity", Movement.Rb2d.velocity.y);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.DrawWireSphere(meleeAttackPosition.position, data.attackRadius);
    }
}
