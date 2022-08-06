using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코어 컴포넌트들을 총괄하는 스크립트
public class Core : MonoBehaviour
{
    // Better Error Handling
    public Movement Movement
    {
        get
        {
            if (movement) return movement;
            
            Debug.LogError("No Movement Core Component on " + transform.parent.name);
            return null;
        }

        private set => movement = value;
    }
    private Movement movement;

    public CollisionSenses ColSenses
    {
        get
        {
            if (colSenses) return colSenses;
            
            Debug.LogError("No Collision Senses Core Component on " + transform.parent.name);
            return null;
        }
        private set => colSenses = value;
    }
    private CollisionSenses colSenses;

    // 코어 컴포넌트들의 할당
    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        ColSenses = GetComponentInChildren<CollisionSenses>();
    }

    // 코어 컴포넌트들의 LogicUpdate 수행
    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }
}
