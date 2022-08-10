using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    #region States
    public Enemy2IdleState IdleState { get; private set; }
    public Enemy2MoveState MoveState { get; private set; }
    #endregion
    
    public override void Awake()
    {
        base.Awake();

        IdleState = new Enemy2IdleState(this, StateMachine, data, "idle", this);
        MoveState = new Enemy2MoveState(this, StateMachine, data, "move", this);
    }

    public override void Start()
    {
        base.Start();
        
        StateMachine.Initialize(MoveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
