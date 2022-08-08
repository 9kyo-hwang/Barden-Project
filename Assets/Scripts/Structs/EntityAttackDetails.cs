using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable을 선언해줘야 SO가 Details를 가져갈 때 Inspector 창에 표시됨
[System.Serializable]
public struct WeaponAttackDetails
{
    public string attackName;
    public float moveSpeed;
    public float damageAmount;

    public float knockbackStrength;
    public Vector2 knockbackAngle;
}