using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public Enemy1IdleState IdleState { get; private set; }
    public Enemy1MoveState MoveState { get; private set; }
    public Enemy1DetectedPlayerState DetectedPlayerState { get; private set; }
    public Enemy1ChargeState ChargeState { get; private set; }
    public Enemy1LookForPlayerState LookForPlayerState { get; private set; }

    public override void Awake()
    {
        base.Awake();

        IdleState = new Enemy1IdleState(this, StateMachine, data, "idle", this);
        MoveState = new Enemy1MoveState(this, StateMachine, data, "move", this);
        DetectedPlayerState = new Enemy1DetectedPlayerState(this, StateMachine, data, "playerDetected", this);
        ChargeState = new Enemy1ChargeState(this, StateMachine, data, "charge", this);
        LookForPlayerState = new Enemy1LookForPlayerState(this, StateMachine, data, "lookForPlayer", this);
    }

    public override void Start()
    {
        base.Start();
        
        StateMachine.Initialize(MoveState);
    }
}
