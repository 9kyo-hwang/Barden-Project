using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Stats : CoreComponent
{
    [SerializeField] private float maxHealthPoint;
    private float currentHealthPoint;

    protected override void Awake()
    {
        base.Awake();

        currentHealthPoint = maxHealthPoint;
    }

    public void DecreaseHealthPoint(float amount)
    {
        currentHealthPoint -= amount;

        if (currentHealthPoint <= 0)
        {
            currentHealthPoint = 0;
            Debug.Log( core.transform.parent.name + " Health Point is Zero");
        }
    }

    public void IncreaseHealthPoint(float amount)
    {
        // Mathf.Clamp: 최소/최대값 내에서 현재값 재설정 함수
        currentHealthPoint = Mathf.Clamp(currentHealthPoint + amount, 0, maxHealthPoint);
    }
}
