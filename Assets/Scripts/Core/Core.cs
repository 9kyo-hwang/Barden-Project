using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics; // NotImplementedError 사용을 위해 필요

// 코어 컴포넌트들을 총괄하는 스크립트
public class Core : MonoBehaviour
{
    public Movement Movement
    {
        get => GenericNotImplementedError<Movement>.TryGet(movement, transform.parent.name);
        private set => movement = value;
    }
    public CollisionSenses CollisionSenses
    {
        get => GenericNotImplementedError<CollisionSenses>.TryGet(collisionSenses, transform.parent.name);
        private set => collisionSenses = value;
    }
    public Combat Combat
    {
        get => GenericNotImplementedError<Combat>.TryGet(combat, transform.parent.name);
        private set => combat = value;
    }

    private CollisionSenses collisionSenses;
    private Movement movement;
    private Combat combat;
    
    // 코어 컴포넌트들의 할당
    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>();
        Combat = GetComponentInChildren<Combat>();
    }

    // 코어 컴포넌트들의 LogicUpdate 수행
    public void LogicUpdate()
    {
        Movement.LogicUpdate();
        Combat.LogicUpdate();
    }
}
