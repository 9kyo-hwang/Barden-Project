using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코어 컴포넌트들을 총괄하는 스크립트
public class Core : MonoBehaviour
{
    public Movement Movement { get; private set; }
    public CollisionSenses ColSenses { get; private set; }

    // 코어 컴포넌트들의 할당
    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        ColSenses = GetComponentInChildren<CollisionSenses>();

        if (!Movement || !ColSenses)
        {
            Debug.LogError("Missing Core Components");
        }
    }

    // 코어 컴포넌트들의 LogicUpdate 수행
    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }
}
