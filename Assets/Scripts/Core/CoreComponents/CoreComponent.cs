using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{ 
    protected Core core;

    // 현재 core에 맞게 설정
    public virtual void Init(Core core)
    {
        this.core = core;
    }

    // Core의 ILogicUpdate List <- Core Component <- Movement, Combat, ...
    protected virtual void Awake()
    {
    }

    // 하위 컴포넌트들이 오버라이딩
    public virtual void LogicUpdate()
    {
        
    }
}
