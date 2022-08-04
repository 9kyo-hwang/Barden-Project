using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public Enemy1IdleState IdleState { get; private set; }
    public Enemy1MoveState MoveState { get; private set; }
    public Enemy1PlayerDetectedState PlayerDetectedState { get; private set; }

    [SerializeField] private EntityData_IdleState idleData;
    [SerializeField] private EntityData_MoveState moveData;
    [SerializeField] private EntityData_PlayerDetectedState playerDetectedData;

    public override void Awake()
    {
        base.Awake();

        IdleState = new Enemy1IdleState(this, StateMachine, "idle", idleData, this);
        MoveState = new Enemy1MoveState(this, StateMachine, "move", moveData, this);
        PlayerDetectedState = new Enemy1PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedData, this);
    }

    public override void Start()
    {
        base.Start();
        
        StateMachine.Initialize(MoveState);
    }
}
