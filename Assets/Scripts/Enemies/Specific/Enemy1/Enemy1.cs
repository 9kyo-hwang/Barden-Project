using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy1 : Entity
{
    public Enemy1IdleState IdleState { get; private set; }
    public Enemy1MoveState MoveState { get; private set; }
    public Enemy1DetectedPlayerState DetectedPlayerState { get; private set; }
    public Enemy1ChargeState ChargeState { get; private set; }
    public Enemy1LookForPlayerState LookForPlayerState { get; private set; }
    public Enemy1MeleeAttackState MeleeAttackState { get; private set; }
    public Enemy1StunState StunState { get; private set; }
    public Enemy1DeadState DeadState { get; private set; }

    [SerializeField] private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        IdleState = new Enemy1IdleState(this, StateMachine, data, "idle", this);
        MoveState = new Enemy1MoveState(this, StateMachine, data, "move", this);
        DetectedPlayerState = new Enemy1DetectedPlayerState(this, StateMachine, data, "playerDetected", this);
        ChargeState = new Enemy1ChargeState(this, StateMachine, data, "charge", this);
        LookForPlayerState = new Enemy1LookForPlayerState(this, StateMachine, data, "lookForPlayer", this);
        MeleeAttackState = new Enemy1MeleeAttackState(this, StateMachine, data, "meleeAttack", meleeAttackPosition, this);
        StunState = new Enemy1StunState(this, StateMachine, data, "stun", this);
        DeadState = new Enemy1DeadState(this, StateMachine, data, "dead", this);
    }

    public override void Start()
    {
        base.Start();
        
        StateMachine.Initialize(MoveState); // Awake에 있으면 안됨
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.DrawWireSphere(meleeAttackPosition.position, data.attackRadius);
    }
}