using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected Core core;

    // Core의 ILogicUpdate List <- Core Component <- Movement, Combat, ...
    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();

        if (core == null)
        {
            Debug.LogError("No Core on the parent");
        }
        
        core.AddComponent(this);
    }

    // 하위 컴포넌트들이 오버라이딩
    public virtual void LogicUpdate()
    {
        
    }
}
